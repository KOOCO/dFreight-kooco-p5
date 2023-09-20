using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Added_PackageWeightUnit_For_AppContainers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PackageWeightUnit",
                table: "AppContainers",
                type: "nvarchar(10)",
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageWeightUnit",
                table: "AppContainers"
            );
        }
    }
}
