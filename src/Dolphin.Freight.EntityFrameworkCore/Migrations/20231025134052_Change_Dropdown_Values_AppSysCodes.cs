using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Change_Dropdown_Values_AppSysCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AppSysCodes SET ShowName = 'Collect' WHERE CodeType = 'PPOrCC' AND CodeValue = 'cc'");
            migrationBuilder.Sql("UPDATE AppSysCodes SET ShowName = 'Prepaid' WHERE CodeType = 'PPOrCC' AND CodeValue = 'pp'");
            migrationBuilder.Sql("UPDATE AppSysCodes SET IsDeleted = '1' WHERE CodeType = 'BillType' AND ShowName = 'BillTypeTest'");
            migrationBuilder.Sql("UPDATE AppSysCodes SET IsDeleted = '1' WHERE CodeType = 'BillType' AND ShowName = 'BillTypeTest2'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
