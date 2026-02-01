namespace WT.B2C.API.BuildingBlocks.Cache;

public abstract class CacheManagerBase<T>(IAppCache cache) : ICacheWarmup
{
    protected abstract string CacheKey { get; }
    protected virtual TimeSpan? CacheTtl => null;

    protected abstract Task<IReadOnlyList<T>> LoadFromSourceAsync(CancellationToken ct);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct)
    {
        var cached = await cache.GetAsync<IReadOnlyList<T>>(CacheKey, ct);
        if (cached is not null)
            return cached;

        var data = await LoadFromSourceAsync(ct);

        await cache.SetAsync(CacheKey, data, CacheTtl, ct);
        return data;
    }

    public Task InvalidateAsync(CancellationToken ct)
        => cache.RemoveAsync(CacheKey, ct);

    public async Task WarmupAsync(CancellationToken cancellationToken = default)
    {
        // Load data into cache if not already cached
        await GetAllAsync(cancellationToken);
    }
}