using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraint_AppPortsManagement_AppExportBookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppExportBookings_AppPorts_DelId",
                table: "AppExportBookings"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppExportBookings_AppPortsManagements_DelId",
                table: "AppExportBookings",
                column: "DelId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppExportBookings_AppPorts_FdestId",
                table: "AppExportBookings"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppExportBookings_AppPortsManagements_FdestId",
                table: "AppExportBookings",
                column: "FdestId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppExportBookings_AppPorts_PodId",
                table: "AppExportBookings"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppExportBookings_AppPortsManagements_PodId",
                table: "AppExportBookings",
                column: "PodId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppExportBookings_AppPorts_PolId",
                table: "AppExportBookings"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppExportBookings_AppPortsManagements_PolId",
                table: "AppExportBookings",
                column: "PolId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppExportBookings_AppPorts_PorId",
                table: "AppExportBookings"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppExportBookings_AppPortsManagements_PorId",
                table: "AppExportBookings",
                column: "PorId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppExportBookings_AppPorts_TransPort1Id",
                table: "AppExportBookings"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppExportBookings_AppPortsManagements_TransPort1Id",
                table: "AppExportBookings",
                column: "TransPort1Id",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
