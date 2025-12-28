using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Infrastructure.Context.Configurations
{
    public class RelatedProductConfiguration : IEntityTypeConfiguration<RelatedProduct>
    {
        public void Configure(EntityTypeBuilder<RelatedProduct> builder)
        {
            builder.ToTable("RelatedProducts");

            // Composite primary key
            builder.HasKey(x => new { x.ProductId, x.RelatedProductId });

            builder.Property(x => x.RelationType)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("similar");

            builder.HasOne(x => x.Product)
                .WithMany(x => x.RelatedProducts)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Related)
                .WithMany(x => x.RelatedToProducts)
                .HasForeignKey(x => x.RelatedProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
