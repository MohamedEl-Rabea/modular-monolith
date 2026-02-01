using System.Net.Http.Headers;
using System.Text;
using WT.B2C.API.BuildingBlocks.Runtime;

namespace WT.B2C.API.BuildingBlocks.Integration.Handlers;

public class BasicAuthHandler(
    CommunicationOptions options,
    IExecutionContextAccessor contextAccessor,
    IServiceProvider serviceProvider)
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