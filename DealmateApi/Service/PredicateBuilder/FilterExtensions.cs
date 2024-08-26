using DealmateApi.Service.Query.Matchers;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace DealmateApi.Service.PredicateBuilder;

public static class FilterExtensions
{
    public static bool IsEmpty<T>(this T filter)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            var value = property.GetValue(filter);
            if (value is IEnumerable enumerable && enumerable.Cast<object>().Any())
            {
                return false;
            }
            else if (value is BoolMatch boolMatch && boolMatch.IsType != BoolMatch.MatchType.Unspecified)
            {
                return false;
            }
        }
        return true;
    }

    public static Expression<Func<TEntity, bool>> CreateDefaultPredicate<TEntity>() where TEntity : class
    {
        var parameter = Expression.Parameter(typeof(TEntity), "v");
        var body = Expression.Constant(true);  // Use true to match all records
        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> BuildCombinedPredicate<T>(
    object filter,
    Func<Expression<Func<T, bool>>> defaultPredicate)
    {
        var filterType = filter.GetType();
        var properties = filterType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        Expression<Func<T, bool>>? combinedPredicate = null;
        HashSet<string> usedFields = new();


        foreach (var property in properties)
        {
            var value = property.GetValue(filter);
            Expression<Func<T, bool>>? propertyPredicate = null;
            if (value is IEnumerable enumerable && enumerable.Cast<object>().Any())
            {
                var propertyName = property.Name;
                usedFields.Add(propertyName);

                var elementType = enumerable.GetType().GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        ?.GetGenericArguments()[0];

                if (elementType == typeof(StringMatch))
                {
                    propertyPredicate = BuildTypePredicate<T, StringMatch, string>(
                        property, enumerable.Cast<StringMatch>(), match => match.BuildPredicate());
                }
                else if (elementType == typeof(IntMatch))
                {
                    propertyPredicate = BuildTypePredicate<T, IntMatch, int>(
                        property, enumerable.Cast<IntMatch>(), match => match.BuildPredicate());
                }
                else if (elementType == typeof(DoubleMatch))
                {
                    propertyPredicate = BuildTypePredicate<T, DoubleMatch, double>(
                        property, enumerable.Cast<DoubleMatch>(), match => match.BuildPredicate());
                }
                else if (elementType == typeof(DateTimeMatch))
                {
                    propertyPredicate = BuildTypePredicate<T, DateTimeMatch, DateTime>(
                        property, enumerable.Cast<DateTimeMatch>(), match => match.BuildPredicate());
                }
                else if (elementType == typeof(TimeSpanMatch))
                {
                    propertyPredicate = BuildTypePredicate<T, TimeSpanMatch, TimeSpan>(
                        property, enumerable.Cast<TimeSpanMatch>(), match => match.BuildPredicate());
                }

            }
            else if (value is BoolMatch boolMatch && boolMatch.IsType != BoolMatch.MatchType.Unspecified)
            {
                var propertyName = property.Name;
                var boolPredicate = BoolMatch.BuildPredicate(boolMatch);
                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyExpression = Expression.Property(parameter, propertyName);
                var boolExpression = Expression.Invoke(boolPredicate, propertyExpression);
                propertyPredicate = Expression.Lambda<Func<T, bool>>(boolExpression, parameter);
            }

            if (propertyPredicate != null)
            {
                combinedPredicate = combinedPredicate == null
                    ? propertyPredicate
                    : combinedPredicate.And(propertyPredicate);
            }
        }
        foreach (var field in usedFields)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);
            var notNullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
            var nullCheckPredicate = Expression.Lambda<Func<T, bool>>(notNullCheck, parameter);
            combinedPredicate = combinedPredicate!.And(nullCheckPredicate);
        }

        return combinedPredicate ?? defaultPredicate();
    }


    #region Methods
    private static Expression<Func<T, bool>> BuildPredicate<T, TProperty>(
    MemberExpression propertyExpression,
    Expression<Func<TProperty, bool>> matchPredicate)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyExpression.Member.Name);

        var propertyType = typeof(TProperty);
        var convertedProperty = property.Type == propertyType
            ? (Expression)property
            : Expression.Convert(property, propertyType);

        var matchBody = Expression.Invoke(matchPredicate, convertedProperty);
        return Expression.Lambda<Func<T, bool>>(matchBody, parameter);
    }

    private static Expression<Func<T, bool>> BuildTypePredicate<T, TMatch, TProperty>(
    PropertyInfo property,
    IEnumerable<TMatch> matches,
    Func<TMatch, Expression<Func<TProperty, bool>>> getPredicate)
    where TMatch : class
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property.Name);

        var predicates = matches.Select(match =>
        {
            var matchPredicate = getPredicate(match);
            return BuildPredicate<T, TProperty>(propertyExpression, matchPredicate);
        });

        var combinedPredicate = predicates.Aggregate((current, next) => current.Or(next));
        return combinedPredicate;
    }

    #endregion

}
