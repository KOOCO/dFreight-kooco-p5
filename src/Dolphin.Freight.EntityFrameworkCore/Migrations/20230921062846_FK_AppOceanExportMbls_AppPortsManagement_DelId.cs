using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_AppOceanExportMbls_AppPortsManagement_DelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanExportMbls_AppPortsManagements_DelId",
                table: "AppOceanExportMbls",
                column: "DelId",
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
        }
    }
}
