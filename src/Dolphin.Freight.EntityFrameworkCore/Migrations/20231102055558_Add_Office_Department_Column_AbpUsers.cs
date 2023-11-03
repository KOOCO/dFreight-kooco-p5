using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_Office_Department_Column_AbpUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Office",
                table: "AbpUsers",
                maxLength: 512,
                nullable: true,
                defaultValue: null
            );

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AbpUsers",
                maxLength: 512,
                nullable: true,
                defaultValue: null
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Office",
                table: "AbpUsers"
            );

            migrationBuilder.DropColumn(
                name: "Department",
                table: "AbpUsers"
            );
        }
    }
}
