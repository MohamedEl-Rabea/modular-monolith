using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace WT.Customers.Portal.BuildingBlocks.Extensions;

public static class ValidationExtensions
{
    public static async Task<IEnumerable<string>> ValidateAsync<T>(this T model, IServiceProvider services)
    {
        var validator = services.GetRequiredService<IValidator<T>>();
        var validationResult = await validator.ValidateAsync(model);
        if (validationResult?.IsValid == false)
        {
            var errors = validationResult!.Errors
                .Select(f => $"{f.PropertyName} - {f.ErrorMessage}");
            return errors;
        }

        return [];
    }
}