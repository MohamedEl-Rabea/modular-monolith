using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Service.Data;
using Sample.Service.Integrations.External;
using WT.B2C.API.BuildingBlocks.Extensions;
using WT.B2C.API.BuildingBlocks.Integration;

namespace Sample.Service;

public static class SampleModule
{
    public static void AddSampleModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleData(configuration);
        services.AddFeatures(Assembly.GetExecutingAssembly());
        services.AddIntegrationClients(Assembly.GetExecutingAssembly());
        services.Configure<ExternalApiOptions>(configuration.GetSection(ExternalApiOptions.SectionName));
    }

    public static void MapSampleModule(this IEndpointRouteBuilder app)
    {
        app.MapFeatures(Assembly.GetExecutingAssembly());
    }
}
