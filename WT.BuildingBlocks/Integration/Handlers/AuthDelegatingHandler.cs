namespace WT.Customers.Portal.BuildingBlocks.Integration.Handlers;

public abstract class HttpClientDelegatingHandler<TOptions> : DelegatingHandler
    where TOptions : CommunicationOptions
{
}