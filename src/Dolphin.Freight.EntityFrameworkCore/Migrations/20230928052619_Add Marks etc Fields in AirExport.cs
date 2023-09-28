using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddMarksetcFieldsinAirExport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<string>(
                name: "HandlingInformation",
                table: "AppAirExportMawbs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManifestNatureAndQuantityOfGoods",
                table: "AppAirExportMawbs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mark",
                table: "AppAirExportMawbs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NatureAndQuantityOfGoods",
                table: "AppAirExportMawbs",
                type: "nvarchar(max)",
                nullable: true);

    
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "HandlingInformation",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "ManifestNatureAndQuantityOfGoods",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "Mark",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "NatureAndQuantityOfGoods",
                table: "AppAirExportMawbs");

      
         
        }
    }
}
