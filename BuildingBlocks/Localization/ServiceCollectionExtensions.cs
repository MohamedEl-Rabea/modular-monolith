using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace WT.B2C.API.BuildingBlocks.Localization;

public static class ServiceCollectionExtensions
{
    public static void AddAppLocalization(this IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supported = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ar"),
            };

            options.DefaultRequestCulture = new RequestCulture("en");
            options.SupportedCultures = supported;
            options.SupportedUICultures = supported;
            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new NormalizingAcceptLanguageProvider(
                supportedCultures: new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "en", "ar"
                },
                fallbackCulture: "en"));
        });
    }
}