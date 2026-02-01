using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace WT.B2C.API.BuildingBlocks.Extensions;

public static class ValidationExtensions
{
    public static async Task<IEnumerable<string>> ValidateAsync<T>(this T model, IServiceProvider services)
    {
        var validator = services.GetService<IValidator<T>>();
        if (validator == null)
            return [];

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