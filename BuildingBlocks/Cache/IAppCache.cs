namespace WT.B2C.API.BuildingBlocks.Cache;

public interface IAppCache
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? ttl = null,
        CancellationToken ct = default);

    Task RemoveAsync(string key, CancellationToken ct = default);
}
