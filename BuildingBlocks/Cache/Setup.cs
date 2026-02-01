using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WT.B2C.API.BuildingBlocks.Cache;

public static class Setup
{
    public static void AddCacheService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppCacheOptions>(configuration.GetSection("CacheOptions"));
        services.AddMemoryCache();
        services.AddRedisDistributedCache();
        services.AddDefaultDistributedCache(configuration);
        services.AddKeyedSingleton<IAppCache, InMemoryAppCache>(CacheProviderType.Memory);
        services.AddKeyedSingleton<IAppCache, DistributedAppCache>(CacheProviderType.Redis);
        services.AddSingleton<ICacheWarmupService, CacheWarmupService>();
    }

    public static async Task UseCacheWarmupAsync(this Microsoft.AspNetCore.Builder.WebApplication app)
    {
        var warmupService = app.Services.GetRequiredService<ICacheWarmupService>();
        await warmupService.WarmupAsync();
    }

    private static void AddRedisDistributedCache(this IServiceCollection services)
    {
        services.AddKeyedSingleton<IDistributedCache>(CacheProviderType.Redis, (sp, _) =>
        {
            var cacheOptions = sp.GetRequiredService<IOptions<AppCacheOptions>>();
            var options = new RedisCacheOptions
            {
                Configuration = cacheOptions.Value.Redis.ConnectionString,
                InstanceName = cacheOptions.Value.Redis.InstanceName
            };

            return new RedisCache(options);
        });

        services.AddKeyedSingleton<IAppCache>(CacheProviderType.Redis, (sp, _) =>
        {
            var dist = sp.GetRequiredKeyedService<IDistributedCache>(CacheProviderType.Redis);
            return new DistributedAppCache(dist);
        });
    }

    private static void AddDefaultDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        var appCacheOptions = configuration.GetSection("CacheOptions").Get<AppCacheOptions>();
        switch (appCacheOptions!.DistributedCacheType)
        {
            case CacheProviderType.Memory:
            {
                services.AddDistributedMemoryCache();
                break;
            }
            default:
            {
                services.AddStackExchangeRedisCache(o =>
                {
                    o.Configuration = appCacheOptions.Redis.ConnectionString;
                    o.InstanceName = appCacheOptions.Redis.InstanceName;
                });
                break;
            }
        }
    }
}