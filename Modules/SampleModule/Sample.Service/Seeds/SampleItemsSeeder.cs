using Microsoft.Extensions.DependencyInjection;
using Sample.Service.Data.Context;
using Sample.Service.Models;
using WT.B2C.API.BuildingBlocks.Data.Seeds;

namespace Sample.Service.Seeds;

public sealed class SampleItemsSeeder(IServiceProvider serviceProvider) : IDataSeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();

        if (dbContext.SampleItems.Any())
            return;

        // TODO: Replace with real seed data logic.
        dbContext.SampleItems.Add(new SampleItem { Code = "SAMPLE", Name = "Sample Item" });
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
