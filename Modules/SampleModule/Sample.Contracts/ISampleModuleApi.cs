using Sample.Contracts.Models;

namespace Sample.Contracts;

public interface ISampleModuleApi
{
    Task<IReadOnlyList<SampleItem>> GetAllSampleItems(CancellationToken cancellationToken = default);
}
