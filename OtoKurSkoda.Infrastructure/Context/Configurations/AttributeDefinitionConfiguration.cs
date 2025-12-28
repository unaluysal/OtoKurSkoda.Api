using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Infrastructure.Context.Configurations
{
    public class AttributeDefinitionConfiguration : IEntityTypeConfiguration<AttributeDefinition>
    {
        public void Configure(EntityTypeBuilder<AttributeDefinition> builder)
        {
            builder.ToTable("AttributeDefinitions");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.Property(x => x.DataType)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("text");

            builder.Property(x => x.Unit)
                .HasMaxLength(50);

            builder.Property(x => x.PossibleValues)
                .HasColumnType("text");

            builder.Property(x => x.DefaultValue)
                .HasMaxLength(500);

            builder.HasMany(x => x.CategoryAttributes)
                .WithOne(x => x.AttributeDefinition)
                .HasForeignKey(x => x.AttributeDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ProductAttributes)
                .WithOne(x => x.AttributeDefinition)
                .HasForeignKey(x => x.AttributeDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
