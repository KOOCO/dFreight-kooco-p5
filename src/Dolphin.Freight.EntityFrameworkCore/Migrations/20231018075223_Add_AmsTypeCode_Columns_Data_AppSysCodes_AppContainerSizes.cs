using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_AmsTypeCode_Columns_Data_AppSysCodes_AppContainerSizes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AmsTypeCodeId",
                table: "AppContainerSizes",
                type: "uniqueidentifier",
                nullable: true
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppContainerSizes_AppSysCodes_AmsTypeCodeId",
                table: "AppContainerSizes",
                column: "AmsTypeCodeId",
                principalTable: "AppSysCodes",
                principalColumn: "Id"
            );

            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', 'A20 GP', 'A20 GP', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', '40 GP', '40 GP', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', '45 GP', '45 GP', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', '20 HQ', '20 HQ', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', '40 HQ', '40 HQ', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', '45 HQ', '45 HQ', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', '53 HQ (9400)', '53 HQ (9400)', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', '53 HQ (9500)', '53 HQ (9500)', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', '20 SPECIAL EQUIP', '20 SPECIAL EQUIP', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, CreationTime) VALUES (NEWID(), 'AmsTypeCodeId', '40 SPECIAL EQUIP', '40 SPECIAL EQUIP', GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmsTypeCodeId",
                table: "AppContainerSizes"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AppContainerSizes_AppSysCodes_AmsTypeCodeId",
                table: "AppContainerSizes"
            );
        }
    }
}
