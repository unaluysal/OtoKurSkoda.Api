using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtoKurSkoda.Infrastructure.Migrations
{
    public partial class RemoveEAVTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttributes",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "CategoryAttributes",
                schema: "OtoKurSkoda");

            migrationBuilder.DropTable(
                name: "AttributeDefinitions",
                schema: "OtoKurSkoda");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // EAV tabloları geri yüklenmeyecek - gerekirse yeniden oluşturulur
        }
    }
}
