namespace WT.B2C.API.BuildingBlocks.Cache;

public interface ICacheManager<T>
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct);
    Task InvalidateAsync(CancellationToken ct);
}