namespace WT.B2C.API.BuildingBlocks.Integration.Handlers;

public abstract class HttpClientDelegatingHandler<TOptions> : DelegatingHandler
    where TOptions : CommunicationOptions
{
}