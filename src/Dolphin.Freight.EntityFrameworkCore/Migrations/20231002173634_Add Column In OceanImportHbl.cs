using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddColumnInOceanImportHbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppSysCodes_RailwayCodeId",
                table: "AppOceanImportHbls");

            migrationBuilder.DropIndex(
                name: "IX_AppOceanImportHbls_RailwayCodeId",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "RailwayCodeId",
                table: "AppOceanImportHbls"
                );
            migrationBuilder.AddColumn<int>(
              name: "RailwayCodeId",
              table: "AppOceanImportHbls",
              type: "int",
              nullable: true
           );
            migrationBuilder.AddColumn<bool>(
                name: "CClearance",
                table: "AppOceanImportHbls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CHold",
                table: "AppOceanImportHbls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CReleasedDate",
                table: "AppOceanImportHbls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomDoc",
                table: "AppOceanImportHbls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DoorMove",
                table: "AppOceanImportHbls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDocSent",
                table: "AppOceanImportHbls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryNo",
                table: "AppOceanImportHbls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Freight",
                table: "AppOceanImportHbls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GoDate",
                table: "AppOceanImportHbls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsfMatchDate",
                table: "AppOceanImportHbls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItIssuedLocation",
                table: "AppOceanImportHbls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Lfd",
                table: "AppOceanImportHbls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ror",
                table: "AppOceanImportHbls",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CClearance",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "CHold",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "CReleasedDate",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "CustomDoc",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "DoorMove",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "EntryDocSent",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "EntryNo",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "Freight",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "GoDate",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "IsfMatchDate",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "ItIssuedLocation",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "Lfd",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "Ror",
                table: "AppOceanImportHbls");

            migrationBuilder.AlterColumn<Guid>(
                name: "RailwayCodeId",
                table: "AppOceanImportHbls",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_RailwayCodeId",
                table: "AppOceanImportHbls",
                column: "RailwayCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppSysCodes_RailwayCodeId",
                table: "AppOceanImportHbls",
                column: "RailwayCodeId",
                principalTable: "AppSysCodes",
                principalColumn: "Id");
        }
    }
}
