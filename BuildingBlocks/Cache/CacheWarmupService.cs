using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WT.B2C.API.BuildingBlocks.Cache;

public interface ICacheWarmupService
{
    Task WarmupAsync(CancellationToken cancellationToken = default);
}

public class CacheWarmupService(
    IServiceProvider serviceProvider,
    ILogger<CacheWarmupService> logger) : ICacheWarmupService
{
    public async Task WarmupAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting cache warmup...");
        var startTime = DateTimeOffset.UtcNow;

        using var scope = serviceProvider.CreateScope();
        var cacheManagers = scope.ServiceProvider.GetServices<ICacheWarmup>();

        foreach (var cacheManager in cacheManagers)
        {
            await WarmupCacheAsync(cacheManager, cancellationToken);
        }

        var elapsed = DateTimeOffset.UtcNow - startTime;
        logger.LogInformation("Cache warmup completed in {ElapsedMs}ms", elapsed.TotalMilliseconds);
    }

    private async Task WarmupCacheAsync(ICacheWarmup cacheManager, CancellationToken cancellationToken)
    {
        try
        {
            var cacheName = cacheManager.GetType().Name;
            logger.LogInformation("Warming up cache: {CacheName}", cacheName);

            var startTime = DateTimeOffset.UtcNow;
            await cacheManager.WarmupAsync(cancellationToken);
            var elapsed = DateTimeOffset.UtcNow - startTime;

            logger.LogInformation("Cache {CacheName} warmed up in {ElapsedMs}ms", cacheName, elapsed.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error warming up cache {CacheName}", cacheManager.GetType().Name);
        }
    }
}