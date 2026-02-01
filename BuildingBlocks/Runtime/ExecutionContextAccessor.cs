using Microsoft.AspNetCore.Http;

namespace WT.B2C.API.BuildingBlocks.Runtime;

public class ExecutionContextAccessor(
    IHttpContextAccessor httpContextAccessor,
    IGeoIpService geoIpService) : IExecutionContextAccessor
{
    private string _correlationId = string.Empty;

    public HttpContext? HttpContext => httpContextAccessor.HttpContext!;

    public string CorrelationId
    {
        get =>
            string.IsNullOrEmpty(_correlationId)
                ? httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("X-Correlation-ID", out var v) ==
                  true
                    ? v.ToString()
                    : httpContextAccessor.HttpContext?.TraceIdentifier ?? string.Empty
                : _correlationId;
        set
        {
            if (httpContextAccessor.HttpContext == null)
            {
                _correlationId = value;
            }
        }
    }

    public string? ClientCountryIso2
    {
        get
        {
            var clientIp = GetClientIpAddress();
            if (string.IsNullOrEmpty(clientIp))
                return null;

            return geoIpService.GetCountryIsoCode(clientIp);
        }
    }

    public ContextUser GetCurrentUser()
    {
        return ContextUser.Empty();
    }

    private string? GetClientIpAddress()
    {
        if (httpContextAccessor.HttpContext == null)
            return null;

        // Check for forwarded IP first (when behind proxy/load balancer)
        if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            var ip = forwardedFor.ToString().Split(',').FirstOrDefault()?.Trim();
            if (!string.IsNullOrEmpty(ip))
                return ip;
        }

        // Check X-Real-IP header
        if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Real-IP", out var realIp))
        {
            return realIp.ToString();
        }

        // Fall back to direct connection IP
        return httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}