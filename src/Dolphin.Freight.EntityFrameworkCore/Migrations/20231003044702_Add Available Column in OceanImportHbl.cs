using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddAvailableColumninOceanImportHbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<DateTime>(
                name: "Available",
                table: "AppOceanImportHbls",
                type: "datetime2",
                nullable: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "Available",
                table: "AppOceanImportHbls");

          
        }
    }
}
