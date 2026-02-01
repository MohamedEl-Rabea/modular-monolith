using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WT.B2C.API.BuildingBlocks.Types;

namespace WT.B2C.API.BuildingBlocks.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddFeatures(this IServiceCollection services,
        params Assembly[]? assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
            assemblies = new[] { Assembly.GetExecutingAssembly() };

        var openHandlerInterface = typeof(IFeatureHandler<,>);

        foreach (var assembly in assemblies.Distinct())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;

                var handlerInterfaces = type
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openHandlerInterface)
                    .ToArray();

                if (handlerInterfaces.Length == 0)
                    continue;

                foreach (var handlerInterface in handlerInterfaces)
                {
                    services.TryAddScoped(handlerInterface, type);
                }
            }
        }
    }

    public static void MapFeatures(this IEndpointRouteBuilder app, params Assembly[]? assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
            assemblies = [Assembly.GetExecutingAssembly()];

        var interfaceType = typeof(IFeatureEndpoint);
        var implementations = assemblies
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    return e.Types.Where(t => t != null)!;
                }
            })
            .Where(t => !t!.IsAbstract && !t.IsInterface && interfaceType.IsAssignableFrom(t));

        foreach (var type in implementations)
        {
            var instance = ActivatorUtilities.CreateInstance(app.ServiceProvider, type!) as IFeatureEndpoint;
            instance?.MapEndpoint(app);
        }
    }
}