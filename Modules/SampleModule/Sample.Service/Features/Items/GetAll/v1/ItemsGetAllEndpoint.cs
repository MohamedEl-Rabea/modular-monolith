using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using WT.B2C.API.BuildingBlocks.FeatureBehaviors;
using WT.B2C.API.BuildingBlocks.Types;

namespace Sample.Service.Features.Items.GetAll.v1;

public class ItemsGetAllEndpoint : FeatureEndpoint
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/sample-items",
                EndpointsStaticHandler.HandleGet<ItemsGetAllInputDto, ItemsGetAllOutputDto>)
            .Produces<FeatureResult<ItemsGetAllOutputDto>>()
            .Produces<FeatureResult<ItemsGetAllOutputDto>>(StatusCodes.Status500InternalServerError)
            .WithTags("SampleItems")
            .WithName("GetAllSampleItems")
            .WithSummary("Get All Sample Items")
            .WithDescription("Retrieves all active sample items.")
            .WithApiVersionSet(GetApiVersionSet(app))
            .MapToApiVersion(1)
            .WithOpenApi();
    }
}
