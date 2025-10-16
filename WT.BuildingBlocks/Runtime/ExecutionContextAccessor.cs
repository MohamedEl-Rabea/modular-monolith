using Microsoft.AspNetCore.Http;

namespace WT.Customers.Portal.BuildingBlocks.Runtime;

public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    public string CorrelationId { get; set; } = string.Empty;
    public string CountryCode => httpContextAccessor.HttpContext.Request.Headers.ContainsKey("CountryCode")
        ? httpContextAccessor.HttpContext.Request.Headers["CountryCode"].ToString()
        : string.Empty;

    public ContextUser GetCurrentUser()
    {
        var httpcontext = httpContextAccessor.HttpContext;
        if (httpcontext != null)
        {
            httpcontext.Request.Headers.TryGetValue("userId", out var userId);
            return new ContextUser
            {
                Id = Convert.ToInt64(userId),
            };
        }

        return ContextUser.Empty();
    }
}