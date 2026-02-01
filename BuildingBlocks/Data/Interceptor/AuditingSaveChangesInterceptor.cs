using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WT.B2C.API.BuildingBlocks.Domain;
using WT.B2C.API.BuildingBlocks.Runtime;

namespace WT.B2C.API.BuildingBlocks.Data.Interceptor;

public class AuditingSaveChangesInterceptor(IExecutionContextAccessor executionContextAccessor)
    : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAuditFields(eventData.Context);
        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditFields(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private void ApplyAuditFields(Microsoft.EntityFrameworkCore.DbContext context)
    {
        // Get all the entries that are added, modified, or deleted
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is Entity &&
                        (e.State == EntityState.Added ||
                         e.State == EntityState.Modified ||
                         e.State == EntityState.Deleted))
            .ToList();

        var currentUser = executionContextAccessor.GetCurrentUser();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity as ICreated != null)
                {
                    ((ICreated)entry.Entity).CreatedAt = DateTime.UtcNow;
                    ((ICreated)entry.Entity).CreatedBy = Convert.ToInt64(currentUser.Id);
                }
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity as IUpdated != null)
                {
                    ((IUpdated)entry.Entity).UpdatedAt = DateTime.UtcNow;
                    ((IUpdated)entry.Entity).UpdatedBy = Convert.ToInt64(currentUser.Id);
                }
            }

            if (entry.State == EntityState.Deleted)
            {
                var softDeleteEntity = entry.Entity as ISoftDelete;
                if (softDeleteEntity != null)
                {
                    softDeleteEntity.IsDeleted = true;
                    softDeleteEntity.DeletedAt = DateTime.UtcNow;
                    softDeleteEntity.DeletedBy = Convert.ToInt64(currentUser.Id);
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}