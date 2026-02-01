namespace WT.B2C.API.BuildingBlocks.Cache;

public class AppCacheOptions
{
    public CacheProviderType DistributedCacheType { get; set; } = CacheProviderType.Redis;
    public RedisOptions Redis { get; set; }
}

public class RedisOptions
{
    public string ConnectionString { get; set; }
    public string? InstanceName { get; set; }
}