using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtoKurSkoda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCartAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddColumn<Guid>(
                name: "CreateUserId",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes",
                type: "boolean",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AddColumn<Guid>(
                name: "TenatId",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes",
                type: "timestamp",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdateUserId",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes",
                type: "uuid",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 5);

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
                    LastActivityAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "OrderItems",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Carts",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "OtoKurSkoda");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "TenatId",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                schema: "OtoKurSkoda",
                table: "CategoryAttributes");
        }
    }
}
