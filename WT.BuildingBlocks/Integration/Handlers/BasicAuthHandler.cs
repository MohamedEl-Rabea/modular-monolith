using System.Net.Http.Headers;
using System.Text;

namespace WT.Customers.Portal.BuildingBlocks.Integration.Handlers;

public class BasicAuthHandler(CommunicationOptions options)
    : HttpClientDelegatingHandler<CommunicationOptions>
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var basicAuth = options.Authentication?.Basic;
        if (basicAuth is not null)
        {
            var authHeader = string.IsNullOrEmpty(basicAuth.Value)
                ? Convert.ToBase64String(Encoding.UTF8.GetBytes($"{basicAuth.UserName}:{basicAuth.Password}"))
                : basicAuth.Value;
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        }

        return base.SendAsync(request, cancellationToken);
    }
}