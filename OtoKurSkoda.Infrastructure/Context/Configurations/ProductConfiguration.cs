using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Infrastructure.Context.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.Property(x => x.ShortDescription)
                .HasMaxLength(1000);

            builder.Property(x => x.Description)
                .HasColumnType("text");

            builder.Property(x => x.Sku)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Sku)
                .IsUnique();

            builder.Property(x => x.Barcode)
                .HasMaxLength(50);

            builder.Property(x => x.OemNumber)
                .HasMaxLength(100);

            builder.HasIndex(x => x.OemNumber);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DiscountedPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CostPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TaxRate)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.MetaTitle)
                .HasMaxLength(200);

            builder.Property(x => x.MetaDescription)
                .HasMaxLength(500);

            builder.Property(x => x.MetaKeywords)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Manufacturer)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ManufacturerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Images)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Attributes)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Compatibilities)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
