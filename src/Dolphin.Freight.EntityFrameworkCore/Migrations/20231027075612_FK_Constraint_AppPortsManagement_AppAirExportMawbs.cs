using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraint_AppPortsManagement_AppAirExportMawbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAirExportMawbs_AppAirports_DepatureId",
                table: "AppAirExportMawbs"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportMawbs_AppPortsManagements_DepatureId",
                table: "AppAirExportMawbs",
                column: "DepatureId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppAirExportMawbs_AppAirports_DestinationId",
                table: "AppAirExportMawbs"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportMawbs_AppPortsManagements_DestinationId",
                table: "AppAirExportMawbs",
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
