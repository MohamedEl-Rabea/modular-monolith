using Microsoft.AspNetCore.Routing;

namespace WT.B2C.API.BuildingBlocks.Types;

public interface IFeatureEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}