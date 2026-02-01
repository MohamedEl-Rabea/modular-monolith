using Microsoft.Extensions.DependencyInjection;

namespace WT.B2C.API.BuildingBlocks.FeatureBehaviors;

public static class Setup
{
    public static void AddFeatureBehaviors(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IFeatureBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddSingleton(typeof(IFeatureBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddSingleton(typeof(IFeatureBehavior<,>), typeof(TransactionBehavior<,>));
    }
}