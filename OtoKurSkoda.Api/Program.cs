using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OtoKurSkoda.Application.Mapping;
using OtoKurSkoda.Application.Services.AuthServices.Interfaces;
using OtoKurSkoda.Application.Services.AuthServices.Services;
using OtoKurSkoda.Application.Services.CatalogServices.Interfaces;
using OtoKurSkoda.Application.Services.CatalogServices.Services;
using OtoKurSkoda.Application.Services.ProductServices.Interfaces;
using OtoKurSkoda.Application.Services.ProductServices.Services;
using OtoKurSkoda.Application.Services.RoleServices.Interfaces;
using OtoKurSkoda.Application.Services.RoleServices.Services;
using OtoKurSkoda.Application.Services.UserServices.Interfaces;
using OtoKurSkoda.Application.Services.UserServices.Services;
using OtoKurSkoda.Application.Services.VehicleServices.Interfaces;
using OtoKurSkoda.Application.Services.VehicleServices.Services;
using OtoKurSkoda.Application.Settings;
using OtoKurSkoda.Infrastructure.Context;
using OtoKurSkoda.Infrastructure.Repositories;
using OtoKurSkoda.Infrastructure.UnitOfWork;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ═══════════════════════════════════════
// CONTROLLERS
// ═══════════════════════════════════════
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ═══════════════════════════════════════
// DATABASE
// ═══════════════════════════════════════
builder.Services.AddDbContext<OtoKurSkodaDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "OtoKurSkoda")
    ));

// ═══════════════════════════════════════
// DEPENDENCY INJECTION
// ═══════════════════════════════════════
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleGroupService, RoleGroupService>();
builder.Services.AddScoped<IRoleGroupRoleService, RoleGroupRoleService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IVehicleModelService, VehicleModelService>();
builder.Services.AddScoped<IVehicleGenerationService, VehicleGenerationService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
builder.Services.AddScoped<IAttributeDefinitionService, AttributeDefinitionService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IProductAttributeService, ProductAttributeService>();
builder.Services.AddScoped<IProductCompatibilityService, ProductCompatibilityService>();


builder.Services.AddHttpContextAccessor();

// ═══════════════════════════════════════
// AUTOMAPPER
// ═══════════════════════════════════════
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ═══════════════════════════════════════
// JWT SETTINGS
// ═══════════════════════════════════════
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// ═══════════════════════════════════════
// CORS (React Frontend için)
// ═══════════════════════════════════════
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ═══════════════════════════════════════
// MIDDLEWARE PIPELINE
// ═══════════════════════════════════════
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();  // JWT için - Authorization'dan önce olmalı!
app.UseAuthorization();

app.MapControllers();

app.Run();