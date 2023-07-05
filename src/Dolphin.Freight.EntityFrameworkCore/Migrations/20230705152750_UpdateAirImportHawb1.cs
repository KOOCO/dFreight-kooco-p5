using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class UpdateAirImportHawb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "BillToId",
                table: "AppAirImportHawbs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "AppAirImportHawbs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomsBroker",
                table: "AppAirImportHawbs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notify",
                table: "AppAirImportHawbs",
                type: "nvarchar(max)",
                nullable: true);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillToId",
                table: "AppAirImportHawbs");

            migrationBuilder.DropColumn(
                name: "Customer",
                table: "AppAirImportHawbs");

            migrationBuilder.DropColumn(
                name: "CustomsBroker",
                table: "AppAirImportHawbs");

            migrationBuilder.DropColumn(
                name: "Notify",
                table: "AppAirImportHawbs");

        }
    }
}
