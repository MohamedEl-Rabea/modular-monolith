using Microsoft.Extensions.DependencyInjection;

namespace WT.B2C.API.BuildingBlocks.Runtime;

public static class Extensions
{
    public static IServiceCollection AddContextAccessor(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IGeoIpService, GeoIpService>();
        services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
        return services;
    }
}