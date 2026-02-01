using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WT.B2C.API.BuildingBlocks.Domain;
using WT.B2C.API.BuildingBlocks.Runtime;

namespace WT.B2C.API.BuildingBlocks.Data.Interceptor;

public static class ModelBuilderExtensions
{
    public static void ApplyDynamicGlobalFilters(this ModelBuilder modelBuilder,
        IExecutionContextAccessor contextAccessor)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (clrType == null || entityType.IsOwned())
                continue;

            var parameter = Expression.Parameter(clrType, "e");

            Expression? combinedFilter = null;

            // 1) SOFT DELETE SUPPORT
            if (typeof(ISoftDelete).IsAssignableFrom(clrType))
            {
                var isDeletedProp = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                var notDeleted = Expression.Equal(isDeletedProp, Expression.Constant(false));

                combinedFilter = combinedFilter == null
                    ? notDeleted
                    : Expression.AndAlso(combinedFilter, notDeleted);
            }

            if (combinedFilter != null)
            {
                var lambda = Expression.Lambda(combinedFilter, parameter);

                modelBuilder.Entity(clrType).HasQueryFilter(lambda);
            }
        }
    }
}