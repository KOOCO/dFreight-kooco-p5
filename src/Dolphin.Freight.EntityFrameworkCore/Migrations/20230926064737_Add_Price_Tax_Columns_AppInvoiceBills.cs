using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_Price_Tax_Columns_AppInvoiceBills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "AppInvoiceBills",
                nullable: true,
                maxLength: 20,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<Guid>(
                name: "TaxId",
                table: "AppInvoiceBills",
                type: "uniqueidentifier",
                nullable: true
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppInvoiceBills_AppSysCodes_TaxId",
                table: "AppInvoiceBills",
                column: "TaxId",
                principalTable: "AppSysCodes",
                principalColumn: "Id"
            );

            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, ExtraProperties, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, ParentId) VALUES (NEWID(), 'TaxId', '7%', 'Tax', '0', NULL, NULL, GETDATE(), NULL, NULL, NULL, NULL)");

            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, ExtraProperties, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, ParentId) VALUES (NEWID(), 'TaxId', '0%', 'Tax', '0', NULL, NULL, GETDATE(), NULL, NULL, NULL, NULL)");

            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, ExtraProperties, ConcurrencyStamp, CreationTime, CreatorId, LastModificationTime, LastModifierId, ParentId) VALUES (NEWID(), 'TaxId', 'TAX', 'Exempt', '0', NULL, NULL, GETDATE(), NULL, NULL, NULL, NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "AppInvoiceBills"
            );

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "AppInvoiceBills"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppInvoiceBills_AppSysCodes_TaxId",
                table: "AppInvoiceBills"
            );

            migrationBuilder.Sql("DELETE FROM AppSysCodes WHERE CodeType = 'TaxId' AND CodeValue = '7%'");

            migrationBuilder.Sql("DELETE FROM AppSysCodes WHERE CodeType = 'TaxId' AND CodeValue = '0%'");

            migrationBuilder.Sql("DELETE FROM AppSysCodes WHERE CodeType = 'TaxId' AND CodeValue = 'TAX'");
        }
    }
}
