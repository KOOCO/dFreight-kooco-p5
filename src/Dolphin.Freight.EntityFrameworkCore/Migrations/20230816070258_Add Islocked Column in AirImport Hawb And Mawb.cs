using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddIslockedColumninAirImportHawbAndMawb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "AppAirImportMawbs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "AppAirImportHawbs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "AppAirImportMawbs");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "AppAirImportHawbs");
        }
    }
}
