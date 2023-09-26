using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class AddAirExportBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppAirExportBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceType = table.Column<int>(type: "int", nullable: false),
                    SoNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SoNoDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HblNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ItnNo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CustomerRefNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DocNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsHold = table.Column<bool>(type: "bit", nullable: false),
                    HoldReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubAgentAwb = table.Column<bool>(type: "bit", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierBkgNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SvcTermTypeFrom = table.Column<int>(type: "int", nullable: false),
                    SvcTermTypeTo = table.Column<int>(type: "int", nullable: false),
                    IncotermsType = table.Column<int>(type: "int", nullable: false),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BillToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NotifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActualShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForwardingAgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoLoaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContainerQtyInputText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalEta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransPort1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CargoTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContainerPickupNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoPickupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TruckerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CargoArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsStackable = table.Column<bool>(type: "bit", nullable: false),
                    Mark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CargoRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreightTermForBuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FreightTermForSalerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PoNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DepatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlightNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DVCarriage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DVCustomer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Insurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WtVal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Package = table.Column<double>(type: "float", nullable: false),
                    MawbPackageUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BuyingRate = table.Column<double>(type: "float", nullable: false),
                    BuyingRateUnitType = table.Column<int>(type: "int", nullable: false),
                    SellingRate = table.Column<double>(type: "float", nullable: false),
                    SellingRateUnitType = table.Column<int>(type: "int", nullable: false),
                    VolumeWeightKg = table.Column<double>(type: "float", nullable: false),
                    VolumeWeightCbm = table.Column<double>(type: "float", nullable: false),
                    GrossWeightKg = table.Column<double>(type: "float", nullable: false),
                    GrossWeightLb = table.Column<double>(type: "float", nullable: false),
                    GrossWeightAmount = table.Column<double>(type: "float", nullable: false),
                    AwbGrossWeightKg = table.Column<double>(type: "float", nullable: false),
                    AwbGrossWeightLb = table.Column<double>(type: "float", nullable: false),
                    AwbGrossWeightAmount = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightKg = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightLb = table.Column<double>(type: "float", nullable: false),
                    ChargeableWeightAmount = table.Column<double>(type: "float", nullable: false),
                    AwbChargeableWeightKg = table.Column<double>(type: "float", nullable: false),
                    AwbChargeableWeightLb = table.Column<double>(type: "float", nullable: false),
                    AwbChargeableWeightAmount = table.Column<double>(type: "float", nullable: false),
                    DisplayUnit = table.Column<int>(type: "int", nullable: false),
                    SalesType = table.Column<int>(type: "int", nullable: false),
                    ShipType = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAirExportBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppAirports_DepatureId",
                        column: x => x.DepatureId,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppAirports_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "AppAirports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppPackageUnits_PackageId",
                        column: x => x.PackageId,
                        principalTable: "AppPackageUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppSubstations_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "AppSubstations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppSysCodes_CargoTypeId",
                        column: x => x.CargoTypeId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppSysCodes_FreightTermForBuyerId",
                        column: x => x.FreightTermForBuyerId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppSysCodes_FreightTermForSalerId",
                        column: x => x.FreightTermForSalerId,
                        principalTable: "AppSysCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_ActualShipperId",
                        column: x => x.ActualShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_AgentId",
                        column: x => x.AgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_BillToId",
                        column: x => x.BillToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_CargoPickupId",
                        column: x => x.CargoPickupId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_CoLoaderId",
                        column: x => x.CoLoaderId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_ConsigneeId",
                        column: x => x.ConsigneeId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_DeliveryToId",
                        column: x => x.DeliveryToId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_ForwardingAgentId",
                        column: x => x.ForwardingAgentId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_NotifyId",
                        column: x => x.NotifyId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_ShipperId",
                        column: x => x.ShipperId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_AppTradePartners_TruckerId",
                        column: x => x.TruckerId,
                        principalTable: "AppTradePartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAirExportBookings_UserData_HolderId",
                        column: x => x.HolderId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_ActualShipperId",
                table: "AppAirExportBookings",
                column: "ActualShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_AgentId",
                table: "AppAirExportBookings",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_BillToId",
                table: "AppAirExportBookings",
                column: "BillToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CargoPickupId",
                table: "AppAirExportBookings",
                column: "CargoPickupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CargoTypeId",
                table: "AppAirExportBookings",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CarrierId",
                table: "AppAirExportBookings",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CoLoaderId",
                table: "AppAirExportBookings",
                column: "CoLoaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_ConsigneeId",
                table: "AppAirExportBookings",
                column: "ConsigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_CustomerId",
                table: "AppAirExportBookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_DeliveryToId",
                table: "AppAirExportBookings",
                column: "DeliveryToId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_DepatureId",
                table: "AppAirExportBookings",
                column: "DepatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_DestinationId",
                table: "AppAirExportBookings",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_ForwardingAgentId",
                table: "AppAirExportBookings",
                column: "ForwardingAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_FreightTermForBuyerId",
                table: "AppAirExportBookings",
                column: "FreightTermForBuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_FreightTermForSalerId",
                table: "AppAirExportBookings",
                column: "FreightTermForSalerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_HolderId",
                table: "AppAirExportBookings",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_NotifyId",
                table: "AppAirExportBookings",
                column: "NotifyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_OfficeId",
                table: "AppAirExportBookings",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_PackageId",
                table: "AppAirExportBookings",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_ShipperId",
                table: "AppAirExportBookings",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAirExportBookings_TruckerId",
                table: "AppAirExportBookings",
                column: "TruckerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppAirExportBookings");
        }
    }
}
