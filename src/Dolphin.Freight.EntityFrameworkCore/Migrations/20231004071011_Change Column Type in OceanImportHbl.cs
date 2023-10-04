using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class ChangeColumnTypeinOceanImportHbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOceanImportHbls_AppSysCodes_ShipTypeId",
                table: "AppOceanImportHbls");

            migrationBuilder.DropIndex(
                name: "IX_AppOceanImportHbls_ShipTypeId",
                table: "AppOceanImportHbls");

            migrationBuilder.DropColumn(
                name: "ShipTypeId",
                table: "AppOceanImportHbls"
                );
            migrationBuilder.AddColumn<int>(
                  name: "ShipTypeId",
                table: "AppOceanImportHbls",
                type: "int",
                nullable: true
                );
            migrationBuilder.DropColumn(
                name: "CustomDoc",
                table: "AppOceanImportHbls"
               );
            migrationBuilder.AddColumn<bool>(
             name: "CustomDoc",
             table: "AppOceanImportHbls",
             type: "bit",
             nullable: false,
             defaultValue: false
            );
        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AlterColumn<Guid>(
                name: "ShipTypeId",
                table: "AppOceanImportHbls",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomDoc",
                table: "AppOceanImportHbls",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_AppOceanImportHbls_ShipTypeId",
                table: "AppOceanImportHbls",
                column: "ShipTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOceanImportHbls_AppSysCodes_ShipTypeId",
                table: "AppOceanImportHbls",
                column: "ShipTypeId",
                principalTable: "AppSysCodes",
                principalColumn: "Id");
        }
    }
}
