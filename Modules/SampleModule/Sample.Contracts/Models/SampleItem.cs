namespace Sample.Contracts.Models;

public class SampleItem
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
}
