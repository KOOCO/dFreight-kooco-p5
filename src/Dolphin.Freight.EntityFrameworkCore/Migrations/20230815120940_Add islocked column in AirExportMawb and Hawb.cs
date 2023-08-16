using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddislockedcolumninAirExportMawbandHawb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
             
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "AppAirExportMawbs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "AppAirExportHawbs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "AppAirExportHawbs");
        }
    }
}
