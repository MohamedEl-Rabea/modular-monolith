using WT.B2C.API.BuildingBlocks.Domain;

namespace Sample.Service.Models;

public class SampleItem : Entity<int>
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; } = true;
}
