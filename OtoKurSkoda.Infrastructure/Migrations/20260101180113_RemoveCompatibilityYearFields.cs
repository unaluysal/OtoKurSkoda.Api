using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtoKurSkoda.Infrastructure.Migrations
{
    public partial class RemoveCompatibilityYearFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartYear",
                schema: "OtoKurSkoda",
                table: "ProductCompatibilities");

            migrationBuilder.DropColumn(
                name: "EndYear",
                schema: "OtoKurSkoda",
                table: "ProductCompatibilities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StartYear",
                schema: "OtoKurSkoda",
                table: "ProductCompatibilities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EndYear",
                schema: "OtoKurSkoda",
                table: "ProductCompatibilities",
                type: "integer",
                nullable: true);
        }
    }
}
