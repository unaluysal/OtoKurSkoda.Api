using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Infrastructure.Context.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "OtoKurSkoda");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.OrderStatus)
                .IsRequired();

            builder.Property(x => x.PaymentStatus)
                .IsRequired();

            // Fiyatlar
            builder.Property(x => x.SubTotal).HasPrecision(18, 2);
            builder.Property(x => x.ShippingCost).HasPrecision(18, 2);
            builder.Property(x => x.TaxAmount).HasPrecision(18, 2);
            builder.Property(x => x.DiscountAmount).HasPrecision(18, 2);
            builder.Property(x => x.TotalAmount).HasPrecision(18, 2);

            // Teslimat Bilgileri
            builder.Property(x => x.ShippingFirstName).HasMaxLength(100);
            builder.Property(x => x.ShippingLastName).HasMaxLength(100);
            builder.Property(x => x.ShippingPhone).HasMaxLength(20);
            builder.Property(x => x.ShippingEmail).HasMaxLength(200);
            builder.Property(x => x.ShippingAddress).HasMaxLength(500);
            builder.Property(x => x.ShippingCity).HasMaxLength(100);
            builder.Property(x => x.ShippingDistrict).HasMaxLength(100);
            builder.Property(x => x.ShippingPostalCode).HasMaxLength(20);

            // Fatura Bilgileri
            builder.Property(x => x.BillingFirstName).HasMaxLength(100);
            builder.Property(x => x.BillingLastName).HasMaxLength(100);
            builder.Property(x => x.BillingPhone).HasMaxLength(20);
            builder.Property(x => x.BillingAddress).HasMaxLength(500);
            builder.Property(x => x.BillingCity).HasMaxLength(100);
            builder.Property(x => x.BillingDistrict).HasMaxLength(100);
            builder.Property(x => x.BillingPostalCode).HasMaxLength(20);
            builder.Property(x => x.TaxNumber).HasMaxLength(20);
            builder.Property(x => x.CompanyName).HasMaxLength(200);

            // Notlar
            builder.Property(x => x.CustomerNote).HasMaxLength(1000);
            builder.Property(x => x.AdminNote).HasMaxLength(1000);

            // Kargo
            builder.Property(x => x.CargoCompany).HasMaxLength(100);
            builder.Property(x => x.TrackingNumber).HasMaxLength(100);

            // Relations
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.OrderNumber).IsUnique();
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.OrderStatus);
            builder.HasIndex(x => x.CreateTime);
        }
    }
}
