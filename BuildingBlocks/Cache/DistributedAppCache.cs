using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace WT.B2C.API.BuildingBlocks.Cache;

public sealed class DistributedAppCache(IDistributedCache distributedCache) : IAppCache
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var value = await distributedCache.GetStringAsync(key, ct);
        return string.IsNullOrEmpty(value)
            ? default
            : JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? ttl = null,
        CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(value);
        await distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl,
        }, ct);
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
        => distributedCache.RemoveAsync(key, ct);
}