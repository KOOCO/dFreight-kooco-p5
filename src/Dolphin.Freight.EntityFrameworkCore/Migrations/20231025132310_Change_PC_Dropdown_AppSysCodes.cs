using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Change_PC_Dropdown_AppSysCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AppSysCodes SET IsDeleted = '1' WHERE CodeType = 'PPOrCC' AND ShowName = 'pPOrCCTest'");
            migrationBuilder.Sql("UPDATE AppSysCodes SET IsDeleted = '1' WHERE CodeType = 'PPOrCC' AND ShowName = 'pPOrCCTest2'");
            migrationBuilder.Sql("UPDATE AppSysCodes SET ShowName = 'Collect' WHERE CodeType = 'PPOrCC' AND CodeValue = '到付 Collect'");
            migrationBuilder.Sql("UPDATE AppSysCodes SET ShowName = 'Prepaid' WHERE CodeType = 'PPOrCC' AND CodeValue = '預付 Prepaid'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
