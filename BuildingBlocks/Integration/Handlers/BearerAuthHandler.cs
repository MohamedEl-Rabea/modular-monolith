using System.Net.Http.Headers;
using WT.B2C.API.BuildingBlocks.Runtime;

namespace WT.B2C.API.BuildingBlocks.Integration.Handlers;

public class BearerAuthHandler(CommunicationOptions options,
    IExecutionContextAccessor contextAccessor,
    IServiceProvider serviceProvider)
    : HttpClientDelegatingHandler<CommunicationOptions>
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var bearerAuth = options.Authentication?.Bearer;
        if (bearerAuth is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerAuth.Value);
        }

        return base.SendAsync(request, cancellationToken);
    }
}