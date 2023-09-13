using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddCargoReadyDateinExportBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<DateTime>(
                name: "CargoReadyDate",
                table: "AppExportBookings",
                type: "datetime2",
                nullable: true);

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropColumn(
                name: "CargoReadyDate",
                table: "AppExportBookings");

           
        }
    }
}
