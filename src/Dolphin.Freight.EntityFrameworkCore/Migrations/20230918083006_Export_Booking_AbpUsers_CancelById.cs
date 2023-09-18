using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Export_Booking_AbpUsers_CancelById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<Guid>(
            //    name: "VesselScheduleId",
            //    table: "AppExportBookings",
            //    type: "uniqueidentifier",
            //    nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppExportBookings_VesselScheduleId",
                table: "AppExportBookings",
                column: "VesselScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppExportBookings_AppVesselSchedules_VesselScheduleId",
                table: "AppExportBookings",
                column: "VesselScheduleId",
                principalTable: "AppVesselSchedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppExportBookings_AbpUsers_CancelById",
                table: "AppExportBookings",
                column: "CancelById",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppExportBookings_AppVesselSchedules_VesselScheduleId",
                table: "AppExportBookings");
            
            migrationBuilder.DropForeignKey(
                name: "FK_AppExportBookings_AbpUsers_CancelById",
                table: "AppExportBookings");

            migrationBuilder.DropIndex(
                name: "IX_AppExportBookings_VesselScheduleId",
                table: "AppExportBookings");

            //migrationBuilder.DropColumn(
            //    name: "VesselScheduleId",
            //    table: "AppExportBookings");
        }
    }
}
