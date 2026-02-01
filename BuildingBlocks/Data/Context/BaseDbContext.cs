using Microsoft.EntityFrameworkCore;
using WT.B2C.API.BuildingBlocks.Data.Interceptor;
using WT.B2C.API.BuildingBlocks.Runtime;

namespace WT.B2C.API.BuildingBlocks.Data.Context;

public abstract class BaseDbContext(
    DbContextOptions options,
    IExecutionContextAccessor executionContextAccessor) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SetStringsMaxLength(modelBuilder);
        ApplySnakeCaseConvention(modelBuilder);
        modelBuilder.ApplyDynamicGlobalFilters(executionContextAccessor);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(new AuditingSaveChangesInterceptor(executionContextAccessor));

    private static void SetStringsMaxLength(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                {
                    property.SetMaxLength(255);
                }
            }
        }
    }

    private static void ApplySnakeCaseConvention(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
        }
    }
    private static string ToSnakeCase(string name)
    {
        return string.Concat(name
            .Select((ch, i) => i > 0 && char.IsUpper(ch) ? "_" + char.ToLower(ch) : char.ToLower(ch).ToString()));
    }
}