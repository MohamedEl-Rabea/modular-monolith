using Microsoft.Extensions.Caching.Memory;

namespace WT.B2C.API.BuildingBlocks.Cache;

public sealed class InMemoryAppCache(IMemoryCache cache) : IAppCache
{
    public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        => Task.FromResult(cache.TryGetValue(key, out T? value) ? value : default);

    public Task SetAsync<T>(string key, T value, TimeSpan? ttl = null, CancellationToken ct = default)
    {
        var options = new MemoryCacheEntryOptions();
        if (ttl.HasValue)
            options.AbsoluteExpirationRelativeToNow = ttl;

        cache.Set(key, value, options);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        cache.Remove(key);
        return Task.CompletedTask;
    }
}