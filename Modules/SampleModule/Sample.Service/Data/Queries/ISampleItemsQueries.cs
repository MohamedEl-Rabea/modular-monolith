using Sample.Service.Models;

namespace Sample.Service.Data.Queries;

public interface ISampleItemsQueries
{
    Task<IReadOnlyList<SampleItem>> GetAllItems(CancellationToken cancellationToken = default);
}
