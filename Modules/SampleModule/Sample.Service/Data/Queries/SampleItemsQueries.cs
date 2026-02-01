using Sample.Service.Data.Context;
using Sample.Service.Models;
using WT.B2C.API.BuildingBlocks.Data.Queries;

namespace Sample.Service.Data.Queries;

public class SampleItemsQueries(IQueryBuilderCreator<SampleDbContext> queryBuilderCreator) : ISampleItemsQueries
{
    public async Task<IReadOnlyList<SampleItem>> GetAllItems(CancellationToken cancellationToken = default)
    {
        using var queryBuilder = queryBuilderCreator.Create(cancellationToken);
        return await queryBuilder.CachedQueryAsync<SampleItem>();
    }
}
