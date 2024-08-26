using System.Reflection;

namespace DealmateApi.Service.Common;

[AttributeUsage(AttributeTargets.Property)]
public class IncludeAttribute : Attribute
{
    public static IEnumerable<string> GetIncludeProperties(Type type)
    {
        var includeProperties = new List<string>();

        var properties = type.GetProperties()
            .Where(p => p.GetCustomAttribute<IncludeAttribute>() != null);

        foreach (var property in properties)
        {
            includeProperties.Add(property.Name);
            var propertyType = property.PropertyType;

            // Handle nested properties
            if (propertyType.IsClass && propertyType != typeof(string))
            {
                // Recursively get nested includes
                includeProperties.AddRange(GetNestedIncludes(propertyType, property.Name));
            }
        }

        return includeProperties;
    }

    private static IEnumerable<string> GetNestedIncludes(Type type, string parentProperty)
    {
        var nestedIncludes = new List<string>();
        var properties = type.GetProperties()
            .Where(p => p.GetCustomAttribute<IncludeAttribute>() != null);

        foreach (var property in properties)
        {
            var propertyName = property.Name;
            nestedIncludes.Add($"{parentProperty}.{propertyName}");

            var propertyType = property.PropertyType;

            // Handle further nesting
            if (propertyType.IsClass && propertyType != typeof(string))
            {
                nestedIncludes.AddRange(GetNestedIncludes(propertyType, $"{parentProperty}.{propertyName}"));
            }
        }

        return nestedIncludes;
    }
}
