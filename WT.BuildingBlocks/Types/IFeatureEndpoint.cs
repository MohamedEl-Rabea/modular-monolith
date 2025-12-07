using Microsoft.AspNetCore.Routing;

namespace WT.Customers.Portal.BuildingBlocks.Types;

public interface IFeatureEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}