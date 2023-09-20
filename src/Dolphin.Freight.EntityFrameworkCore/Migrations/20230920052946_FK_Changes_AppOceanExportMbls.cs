using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Changes_AppOceanExportMbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_DelId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_DelId",
                table: "AppOceanExportMbls",
                column: "DelId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_PorId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_PorId",
                table: "AppOceanExportMbls",
                column: "PorId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_PodId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_PodId",
                table: "AppOceanExportMbls",
                column: "PodId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_PolId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_PolId",
                table: "AppOceanExportMbls",
                column: "PolId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_FdestId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_FdestId",
                table: "AppOceanExportMbls",
                column: "FdestId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_TransPort1Id",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_TransPort1Id",
                table: "AppOceanExportMbls",
                column: "TransPort1Id",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_DelId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_DelId",
                table: "AppOceanExportMbls",
                column: "DelId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_PorId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_PorId",
                table: "AppOceanExportMbls",
                column: "PorId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_PodId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_PodId",
                table: "AppOceanExportMbls",
                column: "PodId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_PolId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_PolId",
                table: "AppOceanExportMbls",
                column: "PolId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_FdestId",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_FdestId",
                table: "AppOceanExportMbls",
                column: "FdestId",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_TransPort1Id",
                table: "AppOceanExportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPorts_TransPort1Id",
                table: "AppOceanExportMbls",
                column: "TransPort1Id",
                principalTable: "AppPorts",
                principalColumn: "Id"
            );
        }
    }
}
