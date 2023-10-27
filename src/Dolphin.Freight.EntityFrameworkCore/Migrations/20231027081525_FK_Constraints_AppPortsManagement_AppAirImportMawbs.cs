using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraints_AppPortsManagement_AppAirImportMawbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAirImportMawbs_AppAirports_DepatureId",
                table: "AppAirImportMawbs"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirImportMawbs_AppPortsManagements_DepatureId",
                table: "AppAirImportMawbs",
                column: "DepatureId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppAirImportMawbs_AppAirports_DestinationId",
                table: "AppAirImportMawbs"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirImportMawbs_AppPortsManagements_DestinationId",
                table: "AppAirImportMawbs",
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
