using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddColumninAirImportMawb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RouteDepartureArrivalDate",
                table: "AppAirImportMawbs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RouteDepartureCarrierId",
                table: "AppAirImportMawbs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteDepartureFlightNo",
                table: "AppAirImportMawbs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RouteDepartureId",
                table: "AppAirImportMawbs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RouteDepatureDate",
                table: "AppAirImportMawbs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RouteDestinationArrivalDate",
                table: "AppAirImportMawbs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RouteDestinationId",
                table: "AppAirImportMawbs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_RouteDepartureCarrierId",
                table: "AppAirImportMawbs",
                column: "RouteDepartureCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_RouteDepartureId",
                table: "AppAirImportMawbs",
                column: "RouteDepartureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirImportMawbs_RouteDestinationId",
                table: "AppAirImportMawbs",
                column: "RouteDestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirImportMawbs_AppAirports_RouteDepartureId",
                table: "AppAirImportMawbs",
                column: "RouteDepartureId",
                principalTable: "AppAirports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirImportMawbs_AppAirports_RouteDestinationId",
                table: "AppAirImportMawbs",
                column: "RouteDestinationId",
                principalTable: "AppAirports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAirImportMawbs_AppTradePartners_RouteDepartureCarrierId",
                table: "AppAirImportMawbs",
                column: "RouteDepartureCarrierId",
                principalTable: "AppTradePartners",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAirImportMawbs_AppAirports_RouteDepartureId",
                table: "AppAirImportMawbs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAirImportMawbs_AppAirports_RouteDestinationId",
                table: "AppAirImportMawbs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAirImportMawbs_AppTradePartners_RouteDepartureCarrierId",
                table: "AppAirImportMawbs");

            migrationBuilder.DropIndex(
                name: "IX_AppAirImportMawbs_RouteDepartureCarrierId",
                table: "AppAirImportMawbs");

            migrationBuilder.DropIndex(
                name: "IX_AppAirImportMawbs_RouteDepartureId",
                table: "AppAirImportMawbs");

            migrationBuilder.DropIndex(
                name: "IX_AppAirImportMawbs_RouteDestinationId",
                table: "AppAirImportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepartureArrivalDate",
                table: "AppAirImportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepartureCarrierId",
                table: "AppAirImportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepartureFlightNo",
                table: "AppAirImportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepartureId",
                table: "AppAirImportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDepatureDate",
                table: "AppAirImportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDestinationArrivalDate",
                table: "AppAirImportMawbs");

            migrationBuilder.DropColumn(
                name: "RouteDestinationId",
                table: "AppAirImportMawbs");
        }
    }
}
