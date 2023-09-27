﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_Data_AppGridPreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AppGridPreference (Id, Preference, PreferenceSrc, UserId) VALUES (NEWID(), '[{\"Checkable\":false,\"Configurable\":true,\"Freeze\":true,\"Lock\":true,\"Name\":\"actions\",\"Show\":true,\"Text\":\"actions\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isLocked\",\"Show\":true,\"Text\":\"isLocked\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"filingNo\",\"Show\":true,\"Text\":\"FilingNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblNo\",\"Show\":true,\"Text\":\"HblNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mblNo\",\"Show\":true,\"Text\":\"MblNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"officeName\",\"Show\":true,\"Text\":\"OfficeId\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"agentName\",\"Show\":true,\"Text\":\"AgentName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblShipperName\",\"Show\":true,\"Text\":\"IHblShipperName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblConsigneeName\",\"Show\":true,\"Text\":\"MblConsigneeName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"arSurplus\",\"Show\":true,\"Text\":\"ARSurplus\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"apSurplus\",\"Show\":true,\"Text\":\"APSurplus\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"dcSurplus\",\"Show\":true,\"Text\":\"DCSurplus\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"soNo\",\"Show\":true,\"Text\":\"SoNO\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isHold\",\"Show\":true,\"Text\":\"IsHold\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isReleased\",\"Show\":true,\"Text\":\"IsReleased\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"oblTypeName\",\"Show\":true,\"Text\":\"OblTypeName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblCustomerName\",\"Show\":true,\"Text\":\"HblCustomerName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polEtd\",\"Show\":true,\"Text\":\"PolEtd\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podEta\",\"Show\":true,\"Text\":\"PodEta\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"porName\",\"Show\":true,\"Text\":\"PorName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polName\",\"Show\":true,\"Text\":\"PolName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podName\",\"Show\":true,\"Text\":\"PodName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"delName\",\"Show\":true,\"Text\":\"DelName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mblCarrierName\",\"Show\":true,\"Text\":\"MblCarrierName\",\"Width\":\"\"}]', 'OceanExportHblList', '8B2697BA-F71B-E556-618E-3A0B4463AB45')");
            migrationBuilder.Sql("INSERT INTO AppGridPreference (Id, Preference, PreferenceSrc, UserId) VALUES (NEWID(), '[{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"referenceNo\",\"Show\":true,\"Text\":\"ReferenceNo\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"soNos\",\"Show\":true,\"Text\":\"SoNo\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"vesselName\",\"Show\":true,\"Text\":\"VesselName\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"voyage\",\"Show\":true,\"Text\":\"Voyage\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polEtd\",\"Show\":true,\"Text\":\"PolEtd\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podEta\",\"Show\":true,\"Text\":\"PodEta\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"porName\",\"Show\":true,\"Text\":\"PorName\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polName\",\"Show\":true,\"Text\":\"PolName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podName\",\"Show\":true,\"Text\":\"PodName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"delName\",\"Show\":true,\"Text\":\"DelName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"docCutOffTime\",\"Show\":true,\"Text\":\"DocCutOffTime\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"vgmCutOffTime\",\"Show\":true,\"Text\":\"VgmCutOffTime\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mblOverseaAgentName\",\"Show\":true,\"Text\":\"MblOverseaAgent\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mblCarrierName\",\"Show\":true,\"Text\":\"Carrier\",\"Width\":\"\"}]', 'VesselSchedulesExcelList', '8B2697BA-F71B-E556-618E-3A0B4463AB45')");
            migrationBuilder.Sql("INSERT INTO AppGridPreference (Id, Preference, PreferenceSrc, UserId) VALUES (NEWID(), '[{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isLocked\",\"Show\":false,\"Text\":\"IsLocked\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"filingNo\",\"Show\":false,\"Text\":\"File No.\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mawbNo\",\"Show\":false,\"Text\":\"MAWB No.\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"officeName\",\"Show\":false,\"Text\":\"Office\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"depatureDate\",\"Show\":false,\"Text\":\"Departure Date/Time\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"arrivalDate\",\"Show\":false,\"Text\":\"Arrival Date/Time\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"depatureAirportName\",\"Show\":false,\"Text\":\"Departure\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"destinationAirportName\",\"Show\":false,\"Text\":\"Destination\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"awbDate\",\"Show\":false,\"Text\":\"B/L Date\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"shipper\",\"Show\":false,\"Text\":\"Shipper (Shipping Agent)\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"consigneeName\",\"Show\":false,\"Text\":\"Consignee (Oversea Agent)\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"carrierTPName\",\"Show\":false,\"Text\":\"Carrier\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"flightNo\",\"Show\":false,\"Text\":\"Flight No.\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hawbNos\",\"Show\":false,\"Text\":\"Hawb Number\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"awbAccountCarrierName\",\"Show\":false,\"Text\":\"Bill To\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"arTotal\",\"Show\":false,\"Text\":\"A/R Balance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"apTotal\",\"Show\":false,\"Text\":\"A/P Balance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"dcTotal\",\"Show\":false,\"Text\":\"D/C Balance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"postDate\",\"Show\":false,\"Text\":\"Post Date\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"sales\",\"Show\":false,\"Text\":\"Sales\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"oP\",\"Show\":false,\"Text\":\"Operator\",\"Width\":\"\"}]', 'AirExportMawbList', '8B2697BA-F71B-E556-618E-3A0B4463AB45')");
            migrationBuilder.Sql("INSERT INTO AppGridPreference (Id, Preference, PreferenceSrc, UserId) VALUES (NEWID(), '[{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"referenceNo\",\"Show\":true,\"Text\":\"ReferenceNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"soNos\",\"Show\":true,\"Text\":\"SoNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"vesselName\",\"Show\":true,\"Text\":\"VesselName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"voyage\",\"Show\":true,\"Text\":\"Voyage\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polEtd\",\"Show\":true,\"Text\":\"PolEtd\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podEta\",\"Show\":true,\"Text\":\"PodEta\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"porName\",\"Show\":true,\"Text\":\"PorName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polName\",\"Show\":true,\"Text\":\"PolName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podName\",\"Show\":true,\"Text\":\"PodName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"delName\",\"Show\":true,\"Text\":\"DelName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"docCutOffTime\",\"Show\":true,\"Text\":\"DocCutOffTime\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"vgmCutOffTime\",\"Show\":true,\"Text\":\"VgmCutOffTime\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mblOverseaAgentName\",\"Show\":true,\"Text\":\"MblOverseaAgent\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mblCarrierName\",\"Show\":true,\"Text\":\"Carrier\",\"Width\":\"\"}]', 'VesselSchedules', '8B2697BA-F71B-E556-618E-3A0B4463AB45')");
            migrationBuilder.Sql("INSERT INTO AppGridPreference (Id, Preference, PreferenceSrc, UserId) VALUES (NEWID(), '[{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isLocked\",\"Show\":false,\"Text\":\"IsLocked\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"docNumber\",\"Show\":false,\"Text\":\"File No.\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hawbNo\",\"Show\":false,\"Text\":\"HAWB Number\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mawbId\",\"Show\":false,\"Text\":\"MawbId\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"departureName\",\"Show\":false,\"Text\":\"Departure\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"depatureDate\",\"Show\":false,\"Text\":\"Depature Date/Time\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"arrivalDate\",\"Show\":false,\"Text\":\"Arrival Date/Time\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"aes\",\"Show\":false,\"Text\":\"AES\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"destinationName\",\"Show\":false,\"Text\":\"Destination\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"shippperName\",\"Show\":false,\"Text\":\"ActualShipper\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"consigneeName\",\"Show\":false,\"Text\":\"Consignee (Oversea Agent)\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"overSeaAgentName\",\"Show\":false,\"Text\":\"OverSea Agent\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"arTotal\",\"Show\":false,\"Text\":\"AR Balance\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"apTotal\",\"Show\":false,\"Text\":\"AP Balance\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"dcTotal\",\"Show\":false,\"Text\":\"DC Balance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"bookingNumber\",\"Show\":false,\"Text\":\"Booking number\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"oP\",\"Show\":false,\"Text\":\"Operator\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"status\",\"Show\":false,\"Text\":\"Status\",\"Width\":\"\"}]', 'AirExportHawbList', '8B2697BA-F71B-E556-618E-3A0B4463AB45')");
            migrationBuilder.Sql("INSERT INTO AppGridPreference (Id, Preference, PreferenceSrc, UserId) VALUES (NEWID(), '[{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isLocked\",\"Show\":false,\"Text\":\"IsLocked\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"docNo\",\"Show\":false,\"Text\":\"FileNo.\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hawbNo\",\"Show\":false,\"Text\":\"HAWBNo.\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mawbNo\",\"Show\":false,\"Text\":\"MAWBNo.\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"officeName\",\"Show\":false,\"Text\":\"Office\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"package\",\"Show\":false,\"Text\":\"Package\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"grossWeightKG\",\"Show\":false,\"Text\":\"G.Weight\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"chargeableWeightKG\",\"Show\":false,\"Text\":\"C.Weight\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"volumeWeightCBM\",\"Show\":false,\"Text\":\"V.Weight\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"measurement\",\"Show\":false,\"Text\":\"Measurement\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"arBalance\",\"Show\":false,\"Text\":\"ARBalance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"apBalance\",\"Show\":false,\"Text\":\"A/PBalance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"dcBalance\",\"Show\":false,\"Text\":\"D/CBalance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"depatureDate\",\"Show\":false,\"Text\":\"DepartureDate/Time\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"arrivalDate\",\"Show\":false,\"Text\":\"ArrivalDate/Time\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"depatureName\",\"Show\":false,\"Text\":\"Departure\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"finalDestination\",\"Show\":false,\"Text\":\"Destination\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"deliveryLocationName\",\"Show\":false,\"Text\":\"DeliveryLocation\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"freightLocationName\",\"Show\":false,\"Text\":\"FreightLocation\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"shipperName\",\"Show\":false,\"Text\":\"Shipper(ShippingAgent)\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"consigneeName\",\"Show\":false,\"Text\":\"Consignee(OverseaAgent)\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"docNo\",\"Show\":false,\"Text\":\"FlightNo.\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"operatorName\",\"Show\":false,\"Text\":\"Operator\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"salesName\",\"Show\":false,\"Text\":\"Sales\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"poNo\",\"Show\":false,\"Text\":\"P.O.NO.\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"itNo\",\"Show\":false,\"Text\":\"ITNO\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"remark\",\"Show\":false,\"Text\":\"Remark\",\"Width\":\"\"}]', 'AirImportHawbList', '8B2697BA-F71B-E556-618E-3A0B4463AB45')");
            migrationBuilder.Sql("INSERT INTO AppGridPreference (Id, Preference, PreferenceSrc, UserId) VALUES (NEWID(), '[{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"soNo\",\"Show\":true,\"Text\":\"SoNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblNo\",\"Show\":true,\"Text\":\"HblNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"shipperName\",\"Show\":true,\"Text\":\"ShipperName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"referenceNo\",\"Show\":true,\"Text\":\"ReferenceNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"officeName\",\"Show\":true,\"Text\":\"OfficeId\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"carrierName\",\"Show\":true,\"Text\":\"CarrierName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"carrierBkgNo\",\"Show\":true,\"Text\":\"CarrierBkgNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"shippingAgentName\",\"Show\":true,\"Text\":\"ShippingAgentName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"vesselName\",\"Show\":true,\"Text\":\"VesselName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"voyage\",\"Show\":true,\"Text\":\"Voyage\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"cargoArrivalDate\",\"Show\":true,\"Text\":\"CargoArrivalDate\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polEtd\",\"Show\":true,\"Text\":\"PolEtd\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podEta\",\"Show\":true,\"Text\":\"PodEta\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"porName\",\"Show\":true,\"Text\":\"PorName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polName\",\"Show\":true,\"Text\":\"PolName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podName\",\"Show\":true,\"Text\":\"PodName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"delName\",\"Show\":true,\"Text\":\"DelName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"svcName\",\"Show\":true,\"Text\":\"SvcName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"docCutOffTime\",\"Show\":true,\"Text\":\"DocCutOffTime\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"portCutOffTime\",\"Show\":true,\"Text\":\"PortCutOffTime\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"vgmCutOffTime\",\"Show\":true,\"Text\":\"VgmCutOffTime\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"railCutOffTime\",\"Show\":true,\"Text\":\"RailCutOffTime\",\"Width\":\"\"},{\"Checkable\":false,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"fdestName\",\"Show\":true,\"Text\":\"FdestName\",\"Width\":\"\"}]', 'OceanExportBookingListExcel', '8B2697BA-F71B-E556-618E-3A0B4463AB45')");
            migrationBuilder.Sql("INSERT INTO AppGridPreference (Id, Preference, PreferenceSrc, UserId) VALUES (NEWID(), '[{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"soNo\",\"Show\":true,\"Text\":\"SoNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblNo\",\"Show\":true,\"Text\":\"HblNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"shipperName\",\"Show\":true,\"Text\":\"ShipperName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"referenceNo\",\"Show\":true,\"Text\":\"ReferenceNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"officeName\",\"Show\":true,\"Text\":\"OfficeId\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"carrierName\",\"Show\":true,\"Text\":\"CarrierName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"carrierBkgNo\",\"Show\":true,\"Text\":\"CarrierBkgNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"shippingAgentName\",\"Show\":true,\"Text\":\"ShippingAgentName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"vesselName\",\"Show\":true,\"Text\":\"VesselName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"voyage\",\"Show\":true,\"Text\":\"Voyage\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"cargoArrivalDate\",\"Show\":true,\"Text\":\"CargoArrivalDate\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polEtd\",\"Show\":true,\"Text\":\"PolEtd\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podEta\",\"Show\":true,\"Text\":\"PodEta\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"porName\",\"Show\":true,\"Text\":\"PorName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polName\",\"Show\":true,\"Text\":\"PolName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podName\",\"Show\":true,\"Text\":\"PodName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"delName\",\"Show\":true,\"Text\":\"DelName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"svcName\",\"Show\":true,\"Text\":\"SvcName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"docCutOffTime\",\"Show\":true,\"Text\":\"DocCutOffTime\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"portCutOffTime\",\"Show\":true,\"Text\":\"PortCutOffTime\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"vgmCutOffTime\",\"Show\":true,\"Text\":\"VgmCutOffTime\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"railCutOffTime\",\"Show\":true,\"Text\":\"RailCutOffTime\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"fdestName\",\"Show\":true,\"Text\":\"FdestName\",\"Width\":\"\"}]', 'OceanExportBookingList', '8B2697BA-F71B-E556-618E-3A0B4463AB45')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
