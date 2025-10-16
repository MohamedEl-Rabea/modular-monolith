using System.Text;

namespace WT.Customers.Portal.BuildingBlocks.Integration;

public static class HttpRequestMessageExtensions
{
    public static async Task<string> ToCurlCommand(this HttpRequestMessage request)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"curl -X {request.Method} \"{request.RequestUri}\" \\");

        foreach (var header in request.Headers)
        {
            var headerLine = $"-H \"{header.Key}: {string.Join(", ", header.Value)}\"";
            builder.AppendLine($"  {headerLine} \\");
        }

        if (request.Content != null)
        {
            foreach (var header in request.Content.Headers)
            {
                var headerLine = $"-H \"{header.Key}: {string.Join(", ", header.Value)}\"";
                builder.AppendLine($"  {headerLine} \\");
            }

            var body = await request.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(body))
            {
                var escapedBody = body.Replace("\"", "\\\"").Replace("\n", "").Replace("\r", "");
                builder.AppendLine($"  --data \"{escapedBody}\"");
            }
        }
        else
        {
            // Remove the trailing backslash if there’s no body
            var result = builder.ToString().TrimEnd();
            if (result.EndsWith("\\"))
            {
                builder = new StringBuilder(result[..^1].TrimEnd());
            }
        }

        return builder.ToString();
    }
}