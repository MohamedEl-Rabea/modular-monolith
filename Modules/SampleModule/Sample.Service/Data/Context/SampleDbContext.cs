using Microsoft.EntityFrameworkCore;
using Sample.Service.Models;
using WT.B2C.API.BuildingBlocks.Data.Context;
using WT.B2C.API.BuildingBlocks.Runtime;

namespace Sample.Service.Data.Context;

public class SampleDbContext(
    DbContextOptions<SampleDbContext> options,
    IExecutionContextAccessor executionContextAccessor) : BaseDbContext(options, executionContextAccessor)
{
    public DbSet<SampleItem> SampleItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SampleDbContext).Assembly);
    }
}
