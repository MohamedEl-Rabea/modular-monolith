using Sample.Contracts;
using Sample.Contracts.Models;
using Sample.Service.Data.Queries;

namespace Sample.Service;

internal sealed class SampleModuleApi(ISampleItemsQueries itemsQueries) : ISampleModuleApi
{
    public async Task<IReadOnlyList<SampleItem>> GetAllSampleItems(CancellationToken cancellationToken = default)
    {
        var items = await itemsQueries.GetAllItems(cancellationToken);
        return items.Select(i => new SampleItem
        {
            Id = i.Id,
            Code = i.Code,
            Name = i.Name,
            IsActive = i.IsActive
        }).ToList();
    }
}
