using FluentValidation;

namespace EventManagement.Application.Common;

/// <summary>
/// Set of Extension methods for <see cref="FluentValidation.IRuleBuilder{T, TProperty}"/>
/// </summary>
public static class ValidationExtensions
{

    /// <summary>
    /// Validates that a string property contains only letters and has a length within the specified range.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validation rule is being defined.</param>
    /// <param name="minLength">The minimum length of the string. Defaults to 2 characters.</param>
    /// <param name="maxLength">The maximum length of the string. Defaults to 35 characters.</param>
    /// <remarks>
    /// This validation method checks that the property is not empty,
    /// contains only letters (including special characters like commas, hyphens, and spaces),
    /// and has a length within the specified range.
    /// <para>
    /// If the validation fails, appropriate error messages are returned to indicate the specific violation 
    /// </para>
    /// </remarks>
    /// <returns>Rule builder options for further chaining validation rules.</returns>
    public static IRuleBuilderOptions<T, string> ValidName<T>(this IRuleBuilder<T, string> ruleBuilder,
        int minLength = 2,
        int maxLength = 35)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("'{PropertyName}' is required.")
            .Matches(@"^[A-Za-z(),\-\s]*$").WithMessage("'{PropertyName}' should only contain letters.")
            .Length(minLength, maxLength);
    }

    /// <summary>
    /// Extension method to validate if a string is a numeric string of a specific length.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validation rule is being defined.</param>
    /// <param name="length">The exact length the numeric string should be.</param>
    /// <returns>Rule builder options for further chaining validation rules.</returns>
    public static IRuleBuilderOptions<T, string> ValidNumericString<T>(this IRuleBuilder<T, string> ruleBuilder,
               int length)
    {
        return ruleBuilder
         .NotEmpty().WithMessage("'{PropertyName}' is required.")
         .Matches($"^[0-9]{{{length}}}$").WithMessage("'{PropertyName}' " + $"must be exactly {length}-digits.");
    }

    // validate date of birth
    /// <summary>
    /// Extension method to validate if a date is in a valid range.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validation rule is being defined.</param>
    /// <param name="minAge">The minimum age of the date. Defaults to 18 years.</param>
    /// <param name="maxAge">The maximum age of the date. Defaults to 100 years.</param>
    /// <returns>Rule builder options for further chaining validation rules.</returns>
    public static IRuleBuilderOptions<T, DateTime> ValidDateOfBirth<T>(this IRuleBuilder<T, DateTime> ruleBuilder,
        int minAge = 18,
        int maxAge = 100)
    {
        var minDate = DateTime.Today.AddYears(-maxAge);
        var maxDate = DateTime.Today.AddYears(-minAge);
        return ruleBuilder
            .NotEmpty().WithMessage("'{PropertyName}' is required.")
            .LessThan(maxDate).WithMessage("'{PropertyName}' " + $"should be less than {maxDate:d}.")
            .GreaterThan(minDate).WithMessage("'{PropertyName}' " + $"should be greater than {minDate:d}.");
    }
}
