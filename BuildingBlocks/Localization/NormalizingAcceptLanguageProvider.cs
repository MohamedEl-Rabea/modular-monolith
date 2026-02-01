using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace WT.B2C.API.BuildingBlocks.Localization;

public sealed class NormalizingAcceptLanguageProvider(HashSet<string> supportedCultures, string fallbackCulture)
    : RequestCultureProvider
{
    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var header = httpContext.Request.Headers.AcceptLanguage.ToString();
        if (string.IsNullOrWhiteSpace(header))
            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(fallbackCulture));


        var first = header.Split(',')[0].Trim();
        if (string.IsNullOrWhiteSpace(first))
            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(fallbackCulture));

        var normalized = first.Split('-')[0];

        if (!supportedCultures.Contains(normalized))
            normalized = fallbackCulture;

        return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(normalized, normalized));
    }
}