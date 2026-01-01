using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Infrastructure.Context.Configurations
{
    public class RoleGroupConfiguration : IEntityTypeConfiguration<RoleGroup>
    {
        public void Configure(EntityTypeBuilder<RoleGroup> builder)
        {
            builder.ToTable("RoleGroups");
        }
    }
}
