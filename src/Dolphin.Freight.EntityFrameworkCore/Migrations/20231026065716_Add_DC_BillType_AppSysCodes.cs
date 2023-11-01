using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_DC_BillType_AppSysCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillTypeDC', 'Debit', 'Debit (Origin Revenue)', '0', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillTypeDC', 'Credit', 'Credit (Origin Cost)', '0', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillTypeDC', 'Revenue', 'Profit Share - Revenue', '0', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillTypeDC', 'Cost', 'Profit Share - Cost', '0', GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
