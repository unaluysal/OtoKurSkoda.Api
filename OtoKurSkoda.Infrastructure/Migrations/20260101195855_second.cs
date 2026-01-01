using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtoKurSkoda.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryAttribute",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "ProductAttribute",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "AttributeDefinition",
                schema: "OtoKurSkoda");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                    DataType = table.Column<string>(type: "text", nullable: false),
                    DefaultValue = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsFilterable = table.Column<bool>(type: "boolean", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IsVisibleOnProductPage = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PossibleValues = table.Column<string>(type: "text", nullable: true),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeDefinition", x => x.Id);
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
                    AttributeDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false)
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
                    AttributeDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "IX_ProductAttribute_AttributeDefinitionId",
                schema: "OtoKurSkoda",
                table: "ProductAttribute",
                column: "AttributeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_ProductId",
                schema: "OtoKurSkoda",
                table: "ProductAttribute",
                column: "ProductId");
        }
    }
}
