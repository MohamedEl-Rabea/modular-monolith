namespace WT.B2C.API.BuildingBlocks.Data.Seeds;

public interface IDataSeeder
{
    Task SeedAsync(CancellationToken cancellationToken);
}