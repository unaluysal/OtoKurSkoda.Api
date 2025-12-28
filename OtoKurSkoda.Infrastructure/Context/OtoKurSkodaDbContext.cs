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

            // User & Auth Configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleGroupConfiguration());
            modelBuilder.ApplyConfiguration(new RoleGroupRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserAddressConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

            // Vehicle Configurations
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleModelConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleGenerationConfiguration());

            // Product Configurations
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ManufacturerConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCompatibilityConfiguration());
            modelBuilder.ApplyConfiguration(new RelatedProductConfiguration());

            // Attribute Configurations (EAV)
            modelBuilder.ApplyConfiguration(new AttributeDefinitionConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductAttributeConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        // User & Auth
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleGroup> RoleGroups { get; set; }
        public DbSet<RoleGroupRole> RoleGroupRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Vehicle
        public DbSet<Brand> Brands { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<VehicleGeneration> VehicleGenerations { get; set; }

        // Product
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductCompatibility> ProductCompatibilities { get; set; }
        public DbSet<RelatedProduct> RelatedProducts { get; set; }

        // Attributes (EAV)
        public DbSet<AttributeDefinition> AttributeDefinitions { get; set; }
        public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
    }
}
