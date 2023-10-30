using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_ATA_ATD_AppOceanImportMbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ATA",
                table: "AppOceanImportMbls",
                type: "datetime2",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "ATD",
                table: "AppOceanImportMbls",
                type: "datetime2",
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ATA",
                table: "AppOceanImportMbls"
            );

            migrationBuilder.DropColumn(
                name: "ATD",
                table: "AppOceanImportMbls"
            );
        }
    }
}
