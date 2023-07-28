using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class UpdateSysCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "AppSysCodes",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "AppSysCodes");
        }
    }
}
