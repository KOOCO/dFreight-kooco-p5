using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraint_AppPortsManagement_AppAirExportHawbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAirExportHawbs_UserData_SalesId",
                table: "AppAirExportHawbs"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportHawbs_AppTradePartners_SalesId",
                table: "AppAirExportHawbs",
                column: "SalesId",
                principalTable: "AppTradePartners",
                principalColumn: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportHawbs_AppPortsManagements_DepartureId",
                table: "AppAirExportHawbs",
                column: "DepartureId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportHawbs_AppPortsManagements_DestinationId",
                table: "AppAirExportHawbs",
                column: "DestinationId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
