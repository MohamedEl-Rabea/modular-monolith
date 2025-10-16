using Microsoft.Extensions.DependencyInjection;

namespace WT.Customers.Portal.BuildingBlocks.Runtime;

public static class Extensions
{
    public static IServiceCollection AddContextAccessor(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
        return services;
    }
}