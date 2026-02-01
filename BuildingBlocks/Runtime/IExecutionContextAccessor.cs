using Microsoft.AspNetCore.Http;

namespace WT.B2C.API.BuildingBlocks.Runtime;

public interface IExecutionContextAccessor
{
    HttpContext? HttpContext { get; }
    string CorrelationId { get; set; }
    string? ClientCountryIso2 { get; }
    ContextUser GetCurrentUser();
}