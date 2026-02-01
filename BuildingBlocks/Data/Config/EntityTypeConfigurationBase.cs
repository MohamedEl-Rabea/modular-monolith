using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WT.B2C.API.BuildingBlocks.Localization;

namespace WT.B2C.API.BuildingBlocks.Data.Config;

public abstract class EntityTypeConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    protected static void OwnsLocalizedText<TEntity>(
        EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, LocalizedText>> propertyExpression,
        string columnPrefix,
        bool required = false,
        bool addIndex = false,
        int maxLength = 256)
        where TEntity : class
    {
        builder.ComplexProperty(propertyExpression, owned =>
        {
            var en = owned.Property(p => p.En)
                .HasColumnName($"{columnPrefix}_en")
                .IsRequired(required);
            var ar = owned.Property(p => p.Ar)
                .HasColumnName($"{columnPrefix}_ar")
                .IsRequired(required);

            en.HasMaxLength(maxLength);
            ar.HasMaxLength(maxLength);
        });
    }
}