using WT.B2C.API.BuildingBlocks.Runtime;

namespace WT.B2C.API.BuildingBlocks.Integration.Handlers;

public class ApiKeyAuthHandler(
    CommunicationOptions options,
    IExecutionContextAccessor contextAccessor,
    IServiceProvider serviceProvider)
    : HttpClientDelegatingHandler<CommunicationOptions>
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var apiKeyAuth = options.Authentication?.ApiKey;
        if (apiKeyAuth is not null)
        {
            request.Headers.Add(apiKeyAuth.HeaderName, apiKeyAuth.Value);
        }

        return base.SendAsync(request, cancellationToken);
    }
}