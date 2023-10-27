using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraints_AppPortsManagement_AppOceanImportHbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppPorts_DelId",
                table: "AppOceanImportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppPortsManagements_DelId",
                table: "AppOceanImportHbls",
                column: "DelId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppPorts_PorId",
                table: "AppOceanImportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppPortsManagements_PorId",
                table: "AppOceanImportHbls",
                column: "PorId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppPorts_PodId",
                table: "AppOceanImportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppPortsManagements_PodId",
                table: "AppOceanImportHbls",
                column: "PodId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppPorts_PolId",
                table: "AppOceanImportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppPortsManagements_PolId",
                table: "AppOceanImportHbls",
                column: "PolId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppPorts_FdestId",
                table: "AppOceanImportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppPortsManagements_FdestId",
                table: "AppOceanImportHbls",
                column: "FdestId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppTradePartners_DeliveryToId",
                table: "AppOceanImportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppPortsManagements_DeliveryToId",
                table: "AppOceanImportHbls",
                column: "DeliveryToId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppTradePartners_FbaFcId",
                table: "AppOceanImportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppPortsManagements_FbaFcId",
                table: "AppOceanImportHbls",
                column: "FbaFcId",
                principalTable: "AppPortsManagements",
                principalColumn: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
