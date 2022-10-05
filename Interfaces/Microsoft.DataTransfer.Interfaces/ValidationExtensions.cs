using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.Interfaces;

public static class ValidationExtensions
{
    public static IEnumerable<string?> GetValidationErrors(this IDataExtensionSettings settings)
    {
        var context = new ValidationContext(settings, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(settings, context, results, true);
        foreach (var validationResult in results)
        {
            yield return validationResult.ErrorMessage;
        }
    }

    public static void Validate(this IDataExtensionSettings settings)
    {
        var validationErrors = settings.GetValidationErrors().ToList();
        if (validationErrors.Any())
        {
            throw new AggregateException($"Configuration for {settings.GetType().Name} is invalid", validationErrors.Select(s => new Exception(s)));
        }
    }
}
