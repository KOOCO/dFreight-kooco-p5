using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Remove_AppOceanImportHbls_DeliveryToId_IfNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AppOceanImportHbls SET DeliveryToId = NULL WHERE DeliveryToId != NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
