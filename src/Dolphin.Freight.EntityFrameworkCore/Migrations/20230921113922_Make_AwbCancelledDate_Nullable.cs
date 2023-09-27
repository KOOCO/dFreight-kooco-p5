using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Make_AwbCancelledDate_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AwbCancelledDate",
                table: "AppAirExportMawbs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AwbCancelledDate",
                table: "AppAirExportMawbs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime?),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
