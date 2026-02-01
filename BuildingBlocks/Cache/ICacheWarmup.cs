namespace WT.B2C.API.BuildingBlocks.Cache;

public interface ICacheWarmup
{
    Task WarmupAsync(CancellationToken cancellationToken = default);
}
