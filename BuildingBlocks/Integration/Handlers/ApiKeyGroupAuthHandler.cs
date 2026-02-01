using Microsoft.Extensions.DependencyInjection;
using WT.B2C.API.BuildingBlocks.Integration.Resolvers;
using WT.B2C.API.BuildingBlocks.Runtime;

namespace WT.B2C.API.BuildingBlocks.Integration.Handlers;

public class ApiKeyGroupAuthHandler(
    CommunicationOptions options,
    IExecutionContextAccessor contextAccessor,
    IServiceProvider serviceProvider)
    : HttpClientDelegatingHandler<CommunicationOptions>
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var apiKeyGroupAuth = options.Authentication?.ApiKeyGroup;
        if (apiKeyGroupAuth is not null)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var groupKeyResolver = serviceScope.ServiceProvider.GetService<IApiKeyGroupKeyResolver>();
            if (groupKeyResolver is null)
                throw new InvalidOperationException(
                    $"IApiKeyGroupKeyResolver is not registered in the dependency injection container.");

            var groupKey = groupKeyResolver.GetKey();
            var apiKeyAuth = apiKeyGroupAuth.TryGetValue(groupKey, out var apiKey) ? apiKey : null;
            if (apiKeyAuth is not null)
                request.Headers.Add(apiKeyAuth.HeaderName, apiKeyAuth.Value);
        }

        return base.SendAsync(request, cancellationToken);
    }
}