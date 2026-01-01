using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtoKurSkoda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "OtoKurSkoda");

            migrationBuilder.CreateTable(
                name: "AttributeDefinition",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    DataType = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    PossibleValues = table.Column<string>(type: "text", nullable: true),
                    DefaultValue = table.Column<string>(type: "text", nullable: true),
                    IsFilterable = table.Column<bool>(type: "boolean", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IsVisibleOnProductPage = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IconClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    MetaTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MetaDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MetaKeywords = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleGroups",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PhoneConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleModels",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    BrandId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleModels_Brands_BrandId",
                        column: x => x.BrandId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryAttribute",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryAttribute_AttributeDefinition_AttributeDefinitionId",
                        column: x => x.AttributeDefinitionId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "AttributeDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryAttribute_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Slug = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    OemNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DiscountedPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CostPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TaxRate = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    LowStockThreshold = table.Column<int>(type: "integer", nullable: true),
                    TrackInventory = table.Column<bool>(type: "boolean", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    IsNewArrival = table.Column<bool>(type: "boolean", nullable: false),
                    IsBestSeller = table.Column<bool>(type: "boolean", nullable: false),
                    IsOnSale = table.Column<bool>(type: "boolean", nullable: false),
                    MetaTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MetaDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MetaKeywords = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ViewCount = table.Column<int>(type: "integer", nullable: false),
                    SoldCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RoleGroupRoles",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroupRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleGroupRoles_RoleGroups_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "RoleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleGroupRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastActivityAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderStatus = table.Column<int>(type: "integer", nullable: false),
                    PaymentStatus = table.Column<int>(type: "integer", nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ShippingCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ShippingFirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShippingLastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShippingPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ShippingEmail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ShippingAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ShippingCity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShippingDistrict = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShippingPostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BillingFirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BillingLastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BillingPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BillingAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BillingCity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BillingDistrict = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BillingPostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TaxNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CustomerNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    AdminNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CargoCompany = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TrackingNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RevokedByIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReplacedByToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddresses ",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    AddressLine = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    TaxNumber = table.Column<string>(type: "text", nullable: true),
                    TaxOffice = table.Column<string>(type: "text", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddresses _Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_RoleGroups_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "RoleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleGenerations",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    VehicleModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    StartYear = table.Column<int>(type: "integer", nullable: false),
                    EndYear = table.Column<int>(type: "integer", nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleGenerations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleGenerations_VehicleModels_VehicleModelId",
                        column: x => x.VehicleModelId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "VehicleModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttribute",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttribute_AttributeDefinition_AttributeDefinitionId",
                        column: x => x.AttributeDefinitionId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "AttributeDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAttribute_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AltText = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedProducts",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelatedProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "similar"),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedProducts", x => new { x.ProductId, x.RelatedProductId });
                    table.ForeignKey(
                        name: "FK_RelatedProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatedProducts_Products_RelatedProductId",
                        column: x => x.RelatedProductId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CartId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProductSku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProductOemNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProductImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCompatibilities",
                schema: "OtoKurSkoda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreateUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdateUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleGenerationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCompatibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCompatibilities_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCompatibilities_VehicleGenerations_VehicleGeneration~",
                        column: x => x.VehicleGenerationId,
                        principalSchema: "OtoKurSkoda",
                        principalTable: "VehicleGenerations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Slug",
                schema: "OtoKurSkoda",
                table: "Brands",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId_ProductId",
                schema: "OtoKurSkoda",
                table: "CartItems",
                columns: new[] { "CartId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                schema: "OtoKurSkoda",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                schema: "OtoKurSkoda",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                schema: "OtoKurSkoda",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                schema: "OtoKurSkoda",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAttribute_AttributeDefinitionId",
                schema: "OtoKurSkoda",
                table: "CategoryAttribute",
                column: "AttributeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAttribute_CategoryId",
                schema: "OtoKurSkoda",
                table: "CategoryAttribute",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Slug",
                schema: "OtoKurSkoda",
                table: "Manufacturers",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                schema: "OtoKurSkoda",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                schema: "OtoKurSkoda",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreateTime",
                schema: "OtoKurSkoda",
                table: "Orders",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                schema: "OtoKurSkoda",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatus",
                schema: "OtoKurSkoda",
                table: "Orders",
                column: "OrderStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                schema: "OtoKurSkoda",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_AttributeDefinitionId",
                schema: "OtoKurSkoda",
                table: "ProductAttribute",
                column: "AttributeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_ProductId",
                schema: "OtoKurSkoda",
                table: "ProductAttribute",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompatibilities_ProductId_VehicleGenerationId",
                schema: "OtoKurSkoda",
                table: "ProductCompatibilities",
                columns: new[] { "ProductId", "VehicleGenerationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompatibilities_VehicleGenerationId",
                schema: "OtoKurSkoda",
                table: "ProductCompatibilities",
                column: "VehicleGenerationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                schema: "OtoKurSkoda",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "OtoKurSkoda",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManufacturerId",
                schema: "OtoKurSkoda",
                table: "Products",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OemNumber",
                schema: "OtoKurSkoda",
                table: "Products",
                column: "OemNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Sku",
                schema: "OtoKurSkoda",
                table: "Products",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                schema: "OtoKurSkoda",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                schema: "OtoKurSkoda",
                table: "RefreshTokens",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "OtoKurSkoda",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProducts_RelatedProductId",
                schema: "OtoKurSkoda",
                table: "RelatedProducts",
                column: "RelatedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupRoles_RoleGroupId",
                schema: "OtoKurSkoda",
                table: "RoleGroupRoles",
                column: "RoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupRoles_RoleId",
                schema: "OtoKurSkoda",
                table: "RoleGroupRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses _UserId",
                schema: "OtoKurSkoda",
                table: "UserAddresses ",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleGroupId",
                schema: "OtoKurSkoda",
                table: "UserRoles",
                column: "RoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "OtoKurSkoda",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleGenerations_VehicleModelId",
                schema: "OtoKurSkoda",
                table: "VehicleGenerations",
                column: "VehicleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModels_BrandId_Slug",
                schema: "OtoKurSkoda",
                table: "VehicleModels",
                columns: new[] { "BrandId", "Slug" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "CategoryAttribute",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "OrderItems",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "ProductAttribute",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "ProductCompatibilities",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "ProductImages",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "RelatedProducts",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "RoleGroupRoles",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "UserAddresses ",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Carts",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "AttributeDefinition",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "VehicleGenerations",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "RoleGroups",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "VehicleModels",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Manufacturers",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Brands",
                schema: "OtoKurSkoda");
        }
    }
}
