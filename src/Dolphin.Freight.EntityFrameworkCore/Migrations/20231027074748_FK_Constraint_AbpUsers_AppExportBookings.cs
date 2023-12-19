using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class FK_Constraint_AbpUsers_AppExportBookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppExportBookings_UserData_CancelById",
                table: "AppExportBookings"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppExportBookings_AbpUsers_CancelById",
                table: "AppExportBookings",
                column: "CancelById",
                principalTable: "AbpUsers",
                principalColumn: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
