using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraints_AppPortsManagement_AppOceanImportMbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppPorts_DelId",
                table: "AppOceanImportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppPortsManagements_DelId",
                table: "AppOceanImportMbls",
                column: "DelId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppPorts_PorId",
                table: "AppOceanImportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppPortsManagements_PorId",
                table: "AppOceanImportMbls",
                column: "PorId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppPorts_PodId",
                table: "AppOceanImportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppPortsManagements_PodId",
                table: "AppOceanImportMbls",
                column: "PodId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppPorts_PolId",
                table: "AppOceanImportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppPortsManagements_PolId",
                table: "AppOceanImportMbls",
                column: "PolId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppPorts_FdestId",
                table: "AppOceanImportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppPortsManagements_FdestId",
                table: "AppOceanImportMbls",
                column: "FdestId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportMbls_AppPorts_TransPort1Id",
                table: "AppOceanImportMbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportMbls_AppPortsManagements_TransPort1Id",
                table: "AppOceanImportMbls",
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
