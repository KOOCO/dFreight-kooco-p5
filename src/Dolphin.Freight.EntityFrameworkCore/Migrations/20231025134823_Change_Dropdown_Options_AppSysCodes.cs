using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Change_Dropdown_Options_AppSysCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AppSysCodes SET IsDeleted = '1' WHERE CodeType = 'BillType'");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), N'BillType', '我們的成本', '我們的成本', '0', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), N'BillType', '為代理付款', '為代理付款', '0', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), N'BillType', '我們的成本', '我們的成本', '0', GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
