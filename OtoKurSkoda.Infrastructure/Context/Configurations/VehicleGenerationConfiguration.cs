using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Infrastructure.Context.Configurations
{
    public class VehicleGenerationConfiguration : IEntityTypeConfiguration<VehicleGeneration>
    {
        public void Configure(EntityTypeBuilder<VehicleGeneration> builder)
        {
            builder.ToTable("VehicleGenerations");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Code)
                .HasMaxLength(20);

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasOne(x => x.VehicleModel)
                .WithMany(x => x.Generations)
                .HasForeignKey(x => x.VehicleModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CompatibleProducts)
                .WithOne(x => x.VehicleGeneration)
                .HasForeignKey(x => x.VehicleGenerationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
