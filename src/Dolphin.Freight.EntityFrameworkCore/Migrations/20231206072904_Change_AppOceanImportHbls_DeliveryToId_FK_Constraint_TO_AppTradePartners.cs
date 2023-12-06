using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Change_AppOceanImportHbls_DeliveryToId_FK_Constraint_TO_AppTradePartners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AppOceanImportHbls SET DeliveryToId = NULL WHERE DeliveryToId is not NULL");

            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppPortsManagements_DeliveryToId",
                table: "AppOceanImportHbls"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppTradePartners_DeliveryToId",
                table: "AppOceanImportHbls",
                column: "DeliveryToId",
                principalTable: "AppTradePartners",
                principalColumn: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
