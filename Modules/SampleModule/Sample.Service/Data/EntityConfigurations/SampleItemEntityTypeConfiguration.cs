using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Service.Models;
using WT.B2C.API.BuildingBlocks.Data.Config;

namespace Sample.Service.Data.EntityConfigurations;

public class SampleItemEntityTypeConfiguration : EntityTypeConfigurationBase<SampleItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SampleItem> builder)
    {
        builder.ToTable("md_sample_items");

        builder.Property(i => i.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(i => i.Code)
            .IsUnique();
    }
}
