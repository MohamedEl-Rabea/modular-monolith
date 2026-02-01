using Microsoft.Extensions.Options;

namespace Sample.Service.Integrations.External;

public sealed class ExternalApiClient(IOptions<ExternalApiOptions> options) : IExternalApiClient
{
    public Task<string> PingAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Replace with real HTTP client logic.
        var result = $"Stubbed client against {options.Value.BaseUrl}";
        return Task.FromResult(result);
    }
}
