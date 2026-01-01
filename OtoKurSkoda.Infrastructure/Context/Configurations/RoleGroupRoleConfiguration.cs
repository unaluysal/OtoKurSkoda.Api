using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtoKurSkoda.Domain.Entitys;

namespace OtoKurSkoda.Infrastructure.Context.Configurations
{
    public class RoleGroupRoleConfiguration : IEntityTypeConfiguration<RoleGroupRole>
    {
        public void Configure(EntityTypeBuilder<RoleGroupRole> builder)
        {
            builder.ToTable("RoleGroupRoles");
        }
    }
}
