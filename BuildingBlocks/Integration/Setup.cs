using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using WT.B2C.API.BuildingBlocks.Integration.Handlers;
using WT.B2C.API.BuildingBlocks.Runtime;

namespace WT.B2C.API.BuildingBlocks.Integration;

public static class Setup
{
    public static IServiceCollection AddIntegrationClients(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var allInterfaces = assemblies.SelectMany(a => a.GetTypes().Where(t => t.IsInterface));
        var httpClientInterfaces = allInterfaces
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHttpClient<>)))
            .ToList();

        var allHandlerTypes = GetAllClientHandlers(services);

        foreach (var httpClientInterface in httpClientInterfaces)
        {
            var optionsType = httpClientInterface.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHttpClient<>))
                .GetGenericArguments()[0];
            if (optionsType == null)
                throw new InvalidOperationException(
                    $"HttClient: [{httpClientInterface}] must have generic argument for options");

            // Register Refit Client
            services.AddRefitClient(httpClientInterface)
                .ConfigureHttpClient((sp, client) =>
                {
                    var optionsTypeToResolve = typeof(IOptions<>).MakeGenericType(optionsType);
                    var options = (sp.GetRequiredService(optionsTypeToResolve) as IOptions<CommunicationOptions>)!;
                    client.BaseAddress = new Uri(options.Value.BaseUrl);
                })
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                .ConfigureAdditionalHttpMessageHandlers((handlers, sp) =>
                {
                    var applicableHandlerType =
                        allHandlerTypes.Where(t => t.BaseType!.GetGenericArguments()[0].IsAssignableFrom(optionsType));
                    var optionsTypeToResolve = typeof(IOptions<>).MakeGenericType(optionsType);
                    var options = (sp.GetRequiredService(optionsTypeToResolve) as IOptions<CommunicationOptions>)!;
                    var contextAccessor = sp.GetRequiredService<IExecutionContextAccessor>();
                    foreach (var handlerType in applicableHandlerType)
                    {
                        var handler = Activator.CreateInstance(handlerType, options.Value, contextAccessor, sp);
                        handlers.Add((handler as DelegatingHandler)!);
                    }
                });
        }

        services.AddTransient<LoggingDelegatingHandler>();
        return services;
    }

    private static IEnumerable<Type> GetAllClientHandlers(IServiceCollection services)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                t is { IsClass: true, IsAbstract: false } &&
                typeof(DelegatingHandler).IsAssignableFrom(t) &&
                t.BaseType?.IsGenericType == true &&
                t.BaseType.GetGenericTypeDefinition() == typeof(HttpClientDelegatingHandler<>));
    }
}