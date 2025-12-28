using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Infrastructure.Context.Configurations
{
    public class ProductCompatibilityConfiguration : IEntityTypeConfiguration<ProductCompatibility>
    {
        public void Configure(EntityTypeBuilder<ProductCompatibility> builder)
        {
            builder.ToTable("ProductCompatibilities");

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            // Unique constraint: Bir ürün-araç kombinasyonu bir kere tanımlanabilir
            builder.HasIndex(x => new { x.ProductId, x.VehicleGenerationId })
                .IsUnique();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Compatibilities)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.VehicleGeneration)
                .WithMany(x => x.CompatibleProducts)
                .HasForeignKey(x => x.VehicleGenerationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
