using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraints_AppCountries_AppOceanImportMbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppTradePartners_CfsLocationId",
                table: "AppOceanImportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppCountries_CfsLocationId",
                table: "AppOceanImportMbls",
                column: "CfsLocationId",
                principalTable: "AppCountries",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppTradePartners_CyLocationId",
                table: "AppOceanImportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppCountries_CyLocationId",
                table: "AppOceanImportMbls",
                column: "CyLocationId",
                principalTable: "AppCountries",
                principalColumn: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
