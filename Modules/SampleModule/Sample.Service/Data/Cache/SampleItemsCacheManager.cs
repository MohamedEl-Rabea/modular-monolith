using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Service.Data.Context;
using Sample.Service.Models;
using WT.B2C.API.BuildingBlocks.Cache;

namespace Sample.Service.Data.Cache;

public sealed class SampleItemsCacheManager(
    SampleDbContext db,
    [FromKeyedServices(CacheProviderType.Memory)] IAppCache appCache)
    : CacheManagerBase<SampleItem>(appCache), ICacheManager<SampleItem>
{
    protected override string CacheKey => "sample-items:all:v1";

    protected override async Task<IReadOnlyList<SampleItem>> LoadFromSourceAsync(CancellationToken ct)
    {
        return await db.SampleItems.Where(i => i.IsActive).ToListAsync(ct);
    }
}
