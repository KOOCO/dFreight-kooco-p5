﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dolphin.Freight.Migrations
{
    public partial class Add_ATA_ATD_AppGridPreference_OceanImportHblList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var query = "UPDATE AppGridPreference SET Preference = '[{\"Checkable\":false,\"Configurable\":true,\"Freeze\":true,\"Lock\":true,\"Name\":\"actions\",\"Show\":true,\"Text\":\"actions\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isLocked\",\"Show\":true,\"Text\":\"isLocked\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"filingNo\",\"Show\":true,\"Text\":\"FilingNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblNo\",\"Show\":true,\"Text\":\"HblNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mblNo\",\"Show\":true,\"Text\":\"MblNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"officeName\",\"Show\":true,\"Text\":\"OfficeId\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"vesselName\",\"Show\":true,\"Text\":\"VesselName\",\"Width\":\"\"},                 {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"voyage\",\"Show\":true,\"Text\":\"Voyage\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"agentName\",\"Show\":true,\"Text\":\"OverseaAgentId\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblShipperName\",\"Show\":true,\"Text\":\"IHblShipperName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblConsigneeName\",\"Show\":true,\"Text\":\"ConsigneeName\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblNotifyName\",\"Show\":true,\"Text\":\"NotifyName\",\"Width\":\"\"},                 {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"fdestName\",\"Show\":true,\"Text\":\"FinalDestName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"fdestEta\",\"Show\":true,\"Text\":\"FinalEta\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"amsNo\",\"Show\":true,\"Text\":\"AmsNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"package\",\"Show\":true,\"Text\":\"Packages\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"weight\",\"Show\":true,\"Text\":\"Weight\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"measurement\",\"Show\":true,\"Text\":\"Measurement\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"svcTermFromName\",\"Show\":true,\"Text\":\"SvcTermFromId\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"arSurplus\",\"Show\":true,\"Text\":\"ARBalance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"apSurplus\",\"Show\":true,\"Text\":\"APBalance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"dcSurplus\",\"Show\":true,\"Text\":\"DCBalance\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"shipModeName\",\"Show\":true,\"Text\":\"HShipModeId\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"truckerName\",\"Show\":true,\"Text\":\"Trucker\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"containerNo\",\"Show\":true,\"Text\":\"Display:ContainerNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"poNo\",\"Show\":true,\"Text\":\"InvoiceList:PoNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"lastFreeDate\",\"Show\":true,\"Text\":\"LastFreeDate\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"cyCfsLocationId\",\"Show\":true,\"Text\":\"CyCfsLocation\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"carrierContractNo\",\"Show\":true,\"Text\":\"CarrierContractNo\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isHold\",\"Show\":true,\"Text\":\"IsHold\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isReleased\",\"Show\":true,\"Text\":\"IsReleased\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"oblTypeName\",\"Show\":true,\"Text\":\"OblTypeName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"hblCustomerName\",\"Show\":true,\"Text\":\"CustomerBroker\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polEtd\",\"Show\":true,\"Text\":\"PolEtd\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podEta\",\"Show\":true,\"Text\":\"PodEta\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"deliveryLocation\",\"Show\":true,\"Text\":\"DeliveryLocation\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"doorDeliveryATA\",\"Show\":true,\"Text\":\"DoorDeliveryATA\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"ata\",\"Show\":true,\"Text\":\"ATA\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"atd\",\"Show\":true,\"Text\":\"ATD\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"porName\",\"Show\":true,\"Text\":\"PorName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"polName\",\"Show\":true,\"Text\":\"PolName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"podName\",\"Show\":true,\"Text\":\"PodName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"delName\",\"Show\":true,\"Text\":\"DelName\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isfNo\",\"Show\":true,\"Text\":\"ISFNo\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isfByThirdParty\",\"Show\":true,\"Text\":\"ISFByThirdParty\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"isfMatchDate\",\"Show\":true,\"Text\":\"ISFMatchDate\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"itNo\",\"Show\":true,\"Text\":\"ItNo\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"mblCarrierName\",\"Show\":true,\"Text\":\"MblCarrierName\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"salesName\",\"Show\":true,\"Text\":\"Sales\",\"Width\":\"\"},{\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"remark\",\"Show\":true,\"Text\":\"Remark\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"operatorName\",\"Show\":true,\"Text\":\"Operator\",\"Width\":\"\"},  {\"Checkable\":true,\"Configurable\":true,\"Freeze\":false,\"Lock\":false,\"Name\":\"creationTime\",\"Show\":true,\"Text\":\"CreateDate\",\"Width\":\"\"}]' WHERE PreferenceSrc = 'OceanImportHblList'";
            migrationBuilder.Sql(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
