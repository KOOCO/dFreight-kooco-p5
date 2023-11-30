using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class ChangeOceanImportReturnLocationRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppTradePartners_ReturnLocationId",
                table: "AppOceanImportMbls");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppPorts_ReturnLocationId",
                table: "AppOceanImportMbls",
                column: "ReturnLocationId",
                principalTable: "AppPorts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppPorts_ReturnLocationId",
                table: "AppOceanImportMbls");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppTradePartners_ReturnLocationId",
                table: "AppOceanImportMbls",
                column: "ReturnLocationId",
                principalTable: "AppTradePartners",
                principalColumn: "Id");
        }
    }
}
