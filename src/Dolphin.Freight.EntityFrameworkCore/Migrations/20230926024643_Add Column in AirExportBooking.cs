using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddColumninAirExportBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAirExportBookings_AppSysCodes_FreightTermForBuyerId",
                table: "AppAirExportBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAirExportBookings_AppSysCodes_FreightTermForSalerId",
                table: "AppAirExportBookings");

            migrationBuilder.DropIndex(
                name: "IX_AppAirExportBookings_FreightTermForBuyerId",
                table: "AppAirExportBookings");

            migrationBuilder.DropColumn(
                name: "ContainerPickupNo",
                table: "AppAirExportBookings");

            migrationBuilder.DropColumn(
                name: "ContainerQtyInputText",
                table: "AppAirExportBookings");

            migrationBuilder.DropColumn(
                name: "FreightTermForBuyerId",
                table: "AppAirExportBookings");

            migrationBuilder.RenameColumn(
                name: "TransPort1Id",
                table: "AppAirExportBookings",
                newName: "SalesPersonId");

            migrationBuilder.RenameColumn(
                name: "ShippingRemark",
                table: "AppAirExportBookings",
                newName: "NatureAndQuantity");

            migrationBuilder.RenameColumn(
                name: "FreightTermForSalerId",
                table: "AppAirExportBookings",
                newName: "SaleId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AppAirExportBookings",
                newName: "DeliveryInstruction");

            migrationBuilder.RenameIndex(
                name: "IX_AppAirExportBookings_FreightTermForSalerId",
                table: "AppAirExportBookings",
                newName: "IX_AppAirExportBookings_SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportBookings_UserData_SaleId",
                table: "AppAirExportBookings",
                column: "SaleId",
                principalTable: "UserData",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAirExportBookings_UserData_SaleId",
                table: "AppAirExportBookings");

            migrationBuilder.RenameColumn(
                name: "SalesPersonId",
                table: "AppAirExportBookings",
                newName: "TransPort1Id");

            migrationBuilder.RenameColumn(
                name: "SaleId",
                table: "AppAirExportBookings",
                newName: "FreightTermForSalerId");

            migrationBuilder.RenameColumn(
                name: "NatureAndQuantity",
                table: "AppAirExportBookings",
                newName: "ShippingRemark");

            migrationBuilder.RenameColumn(
                name: "DeliveryInstruction",
                table: "AppAirExportBookings",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_AppAirExportBookings_SaleId",
                table: "AppAirExportBookings",
                newName: "IX_AppAirExportBookings_FreightTermForSalerId");

            migrationBuilder.AddColumn<string>(
                name: "ContainerPickupNo",
                table: "AppAirExportBookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContainerQtyInputText",
                table: "AppAirExportBookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FreightTermForBuyerId",
                table: "AppAirExportBookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_FreightTermForBuyerId",
                table: "AppAirExportBookings",
                column: "FreightTermForBuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportBookings_AppSysCodes_FreightTermForBuyerId",
                table: "AppAirExportBookings",
                column: "FreightTermForBuyerId",
                principalTable: "AppSysCodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportBookings_AppSysCodes_FreightTermForSalerId",
                table: "AppAirExportBookings",
                column: "FreightTermForSalerId",
                principalTable: "AppSysCodes",
                principalColumn: "Id");
        }
    }
}
