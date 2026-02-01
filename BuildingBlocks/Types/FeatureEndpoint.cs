using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace WT.B2C.API.BuildingBlocks.Types;

public abstract class FeatureEndpoint : IFeatureEndpoint
{
    protected ApiVersionSet GetApiVersionSet(IEndpointRouteBuilder app) =>
        app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

    public abstract void MapEndpoint(IEndpointRouteBuilder app);
}