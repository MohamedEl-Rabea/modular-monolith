using System.Text;
using Microsoft.Extensions.Logging;

namespace WT.Customers.Portal.BuildingBlocks.Integration.Handlers;

public class LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger) : DelegatingHandler
{
    private readonly ILogger<LoggingDelegatingHandler> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var requestId = Guid.NewGuid();
        await LogRequestAsync(request, requestId, cancellationToken);

        var response = await base.SendAsync(request, cancellationToken);

        await LogResponseAsync(response, requestId, cancellationToken);

        return response;
    }


    private async Task LogRequestAsync(HttpRequestMessage request, Guid requestId, CancellationToken cancellationToken)
    {
        var curlCommand = await request.ToCurlCommand();
        _logger.LogInformation("=== CURL COMMAND === [{RequestId}]\n{CurlCommand}", requestId, curlCommand);
    }


    private async Task LogResponseAsync(HttpResponseMessage response, Guid requestId,
        CancellationToken cancellationToken)
    {
        var responseLog = new StringBuilder();
        responseLog.AppendLine($"=== HTTP RESPONSE === [{requestId}]");
        responseLog.AppendLine($"Status Code: {response.StatusCode}");
        responseLog.AppendLine("Headers:");

        foreach (var header in response.Headers)
        {
            responseLog.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
        }

        if (response.Content != null)
        {
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            responseLog.AppendLine($"Payload: {responseContent}");
        }

        _logger.LogInformation(responseLog.ToString());
    }
}