using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Update_data_BillType_AppSysCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AppSysCodes SET IsDeleted = '1' WHERE CodeType = 'BillType'");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillType', 'Our Cost', 'Our Cost', '0', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillType', 'Pay For Agent', 'Pay For Agent', '0', GETDATE())");
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'BillType', 'Cost For Agent', 'Cost For Agent', '0', GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
