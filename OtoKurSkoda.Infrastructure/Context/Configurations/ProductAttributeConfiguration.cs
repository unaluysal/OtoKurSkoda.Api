using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Infrastructure.Context.Configurations
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.ToTable("ProductAttributes");

            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(1000);

            // Unique constraint: Bir ürün için aynı özellik bir kere tanımlanabilir
            builder.HasIndex(x => new { x.ProductId, x.AttributeDefinitionId })
                .IsUnique();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Attributes)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.AttributeDefinition)
                .WithMany(x => x.ProductAttributes)
                .HasForeignKey(x => x.AttributeDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
