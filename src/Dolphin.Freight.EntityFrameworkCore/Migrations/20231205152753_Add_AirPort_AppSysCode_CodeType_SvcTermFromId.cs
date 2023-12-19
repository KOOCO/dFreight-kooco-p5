using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_AirPort_AppSysCode_CodeType_SvcTermFromId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AppSysCodes (Id, CodeType, CodeValue, ShowName, IsDeleted, CreationTime) VALUES (NEWID(), 'SvcTermFromId', 'AIRPORT', 'AIRPORT', '0', GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
