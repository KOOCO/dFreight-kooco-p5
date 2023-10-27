using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraint_AppPortsManagement_AppVesselSchedules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppVesselSchedules_AppPorts_DelId",
                table: "AppVesselSchedules"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppVesselSchedules_AppPortsManagements_DelId",
                table: "AppVesselSchedules",
                column: "DelId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppVesselSchedules_AppPorts_FdestId",
                table: "AppVesselSchedules"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppVesselSchedules_AppPortsManagements_FdestId",
                table: "AppVesselSchedules",
                column: "FdestId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppVesselSchedules_AppPorts_PodId",
                table: "AppVesselSchedules"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppVesselSchedules_AppPortsManagements_PodId",
                table: "AppVesselSchedules",
                column: "PodId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppVesselSchedules_AppPorts_PolId",
                table: "AppVesselSchedules"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppVesselSchedules_AppPortsManagements_PolId",
                table: "AppVesselSchedules",
                column: "PolId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppVesselSchedules_AppPorts_PorId",
                table: "AppVesselSchedules"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppVesselSchedules_AppPortsManagements_PorId",
                table: "AppVesselSchedules",
                column: "PorId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppVesselSchedules_AppPorts_TransPort1Id",
                table: "AppVesselSchedules"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppVesselSchedules_AppPortsManagements_TransPort1Id",
                table: "AppVesselSchedules",
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
