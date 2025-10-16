using Microsoft.AspNetCore.Routing;

namespace WT.Customers.Portal.BuildingBlocks.Integration.Types;

public interface IFeatureEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}