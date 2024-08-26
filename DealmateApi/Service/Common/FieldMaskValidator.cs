using FluentValidation;

namespace DealmateApi.Service.Common;

public class FieldMaskValidator : AbstractValidator<FieldMask>
{
    public FieldMaskValidator(params string[] allowedPaths)
    {
        Array.Sort(allowedPaths); // Sort the allowed paths alphabetically

        this.RuleFor(_ => _.Paths)
            .NotEmpty()
            .WithMessage("'FieldMask Paths' must not be empty.")
            .ForEach((path) =>
                path.In(allowedPaths)
                    .WithMessage(path =>
                    {
                        var allowedPathsFormatted = string.Join("', '", allowedPaths);
                        return allowedPaths.Length > 1
                            ? $"Only the following paths are allowed: '{allowedPathsFormatted}'."
                            : $"Only '{allowedPaths.First()}' is allowed.";
                    }));
    }
}

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TProperty> In<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, params TProperty[] validOptions)
    {
        if (validOptions == null || validOptions.Length == 0)
        {
            throw new ArgumentException("At least one valid option is expected", nameof(validOptions));
        }

        if (validOptions.Length == 1)
        {
            return ruleBuilder
                .Must(validOptions.Contains)
                .WithMessage($"'{{PropertyName}}' only allows the value: {validOptions[0]}");
        }
        else
        {
            var formatted = string.Join(", ", validOptions.Take(validOptions.Length - 1).Select(vo => vo.ToString()))
                            + " or "
                            + validOptions.Last();
            return ruleBuilder
                .Must(validOptions.Contains)
                .WithMessage($"'{{PropertyName}}' must be one of these values: {formatted}");
        }
    }
}


