using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Name_Change_AppSysCodes_BillType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AppSysCodes SET IsDeleted = '1' WHERE CodeType = 'BillType'");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillType', CAST(N'我們的成本' AS varbinary), CAST(N'我們的成本' AS varbinary), '0', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillType', CAST(N'為代理付款' AS varbinary), CAST(N'為代理付款' AS varbinary), '0', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillType', CAST(N'我們的成本' AS varbinary), CAST(N'我們的成本' AS varbinary), '0', GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
