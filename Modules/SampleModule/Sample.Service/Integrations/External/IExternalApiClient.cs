namespace Sample.Service.Integrations.External;

public interface IExternalApiClient
{
    Task<string> PingAsync(CancellationToken cancellationToken = default);
}
