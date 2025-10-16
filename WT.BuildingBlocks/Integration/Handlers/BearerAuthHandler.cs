using System.Net.Http.Headers;

namespace WT.Customers.Portal.BuildingBlocks.Integration.Handlers;

public class BearerAuthHandler(CommunicationOptions options)
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