using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddColumnInAirExportMawbTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RouteDepartureArrivalDate",
                table: "AppAirExportMawbs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RouteDepartureCarrierId",
                table: "AppAirExportMawbs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteDepartureFlightNo",
                table: "AppAirExportMawbs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RouteDepartureId",
                table: "AppAirExportMawbs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RouteDepatureDate",
                table: "AppAirExportMawbs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RouteDestinationArrivalDate",
                table: "AppAirExportMawbs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RouteDestinationId",
                table: "AppAirExportMawbs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_RouteDepartureCarrierId",
                table: "AppAirExportMawbs",
                column: "RouteDepartureCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_RouteDepartureId",
                table: "AppAirExportMawbs",
                column: "RouteDepartureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportMawbs_RouteDestinationId",
                table: "AppAirExportMawbs",
                column: "RouteDestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportMawbs_AppAirports_RouteDepartureId",
                table: "AppAirExportMawbs",
                column: "RouteDepartureId",
                principalTable: "AppAirports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportMawbs_AppAirports_RouteDestinationId",
                table: "AppAirExportMawbs",
                column: "RouteDestinationId",
                principalTable: "AppAirports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirExportMawbs_AppTradePartners_RouteDepartureCarrierId",
                table: "AppAirExportMawbs",
                column: "RouteDepartureCarrierId",
                principalTable: "AppTradePartners",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAirExportMawbs_AppAirports_RouteDepartureId",
                table: "AppAirExportMawbs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAirExportMawbs_AppAirports_RouteDestinationId",
                table: "AppAirExportMawbs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAirExportMawbs_AppTradePartners_RouteDepartureCarrierId",
                table: "AppAirExportMawbs");

            migrationBuilder.DropIndex(
                name: "IX_AppAirExportMawbs_RouteDepartureCarrierId",
                table: "AppAirExportMawbs");

            migrationBuilder.DropIndex(
                name: "IX_AppAirExportMawbs_RouteDepartureId",
                table: "AppAirExportMawbs");

            migrationBuilder.DropIndex(
                name: "IX_AppAirExportMawbs_RouteDestinationId",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepartureArrivalDate",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepartureCarrierId",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepartureFlightNo",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepartureId",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepatureDate",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDestinationArrivalDate",
                table: "AppAirExportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDestinationId",
                table: "AppAirExportMawbs");
        }
    }
}
