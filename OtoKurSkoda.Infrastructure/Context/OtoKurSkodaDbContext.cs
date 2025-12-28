using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.Context.Configurations;

namespace OtoKurSkoda.Infrastructure.Context
{
    public class OtoKurSkodaDbContext : DbContext
    {
        public OtoKurSkodaDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("OtoKurSkoda");

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleGroupConfiguration());
            modelBuilder.ApplyConfiguration(new RoleGroupRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserAddressConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());  

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleGroup> RoleGroups { get; set; }
        public DbSet<RoleGroupRole> RoleGroupRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }  
    }
}