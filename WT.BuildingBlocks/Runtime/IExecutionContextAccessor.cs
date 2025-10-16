namespace WT.Customers.Portal.BuildingBlocks.Runtime;

public interface IExecutionContextAccessor
{
    string CorrelationId { get; set; }
    string CountryCode { get; }
    ContextUser GetCurrentUser();
}