using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraints_AppPortsManagement_AppOceanExportHbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_DelId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_DelId",
                table: "AppOceanExportHbls",
                column: "DelId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_PorId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_PorId",
                table: "AppOceanExportHbls",
                column: "PorId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_PodId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_PodId",
                table: "AppOceanExportHbls",
                column: "PodId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_PolId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_PolId",
                table: "AppOceanExportHbls",
                column: "PolId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_FdestId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_FdestId",
                table: "AppOceanExportHbls",
                column: "FdestId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_DelId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_DelId",
                table: "AppOceanExportHbls",
                column: "DelId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_PorId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_PorId",
                table: "AppOceanExportHbls",
                column: "PorId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_PodId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_PodId",
                table: "AppOceanExportHbls",
                column: "PodId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_PolId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_PolId",
                table: "AppOceanExportHbls",
                column: "PolId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportHbls_AppPortsManagements_FdestId",
                table: "AppOceanExportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportHbls_AppPorts_FdestId",
                table: "AppOceanExportHbls",
                column: "FdestId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );
        }
    }
}
