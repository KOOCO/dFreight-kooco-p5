﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class CreateUpdateAirExportHawbDto
    {
        public Guid Id { get; set; }
        public Guid? MawbId { get; set; }
        public string HawbNo { get; set; }
        public string BookingNo { get; set; }
        public DateTime BookingDate { get; set; }
        public string ITNNo { get; set; }
        public Guid? QuotationId { get; set; }
        public string ActualShippedr { get; set; }
        public string Customer { get; set; }
        public string BillTo { get; set; }
        public Guid? ConsigneeId { get; set; }
        public string Consignee { get; set; }
        public string Notify { get; set; }
        public string OverseaAgent { get; set; }
        public string IssuingCarrier { get; set; }
        public string Trucker { get; set; }
        public Guid? SalesId { get; set; }
        [ForeignKey("SalesId")]
        public virtual UserData Sales { get; set; }
        public Guid? OPId { get; set; }
        [ForeignKey("OPId")]
        public virtual UserData OP { get; set; }

        public bool SubAgentAwb { get; set; }
        public Guid? DepartureId { get; set; }
        public Guid? DestinationId { get; set; }
        public string CargoPickup { get; set; }
        public string DeliveryTo { get; set; }
        public string CargoType { get; set; }
        public string SalesType { get; set; }
        public string ShipType { get; set; }
        public DateTime FinalEta { get; set; }
        public string DVCarriage { get; set; }
        public string DVCustoms { get; set; }
        public string WTVAL { get; set; }
        public string Other { get; set; }
        public string Package { get; set; }
        public string PackageUnit { get; set; }
        public string BuyingRate { get; set; }
        public string BuyingRateUnit { get; set; }
        public string SellingRate { get; set; }
        public string SellingRateUnit { get; set; }
        public string VolumeWeightKG { get; set; }
        public string VolumeWeightCBM { get; set; }
        public string GrossWeightShprKG { get; set; }
        public string GrossWeightShprLB { get; set; }
        public string GrossWeightShprAmount { get; set; }
        public string GrossWeightCneeKG { get; set; }
        public string GrossWeightCneeLB { get; set; }
        public string GrossWeightCneeAmount { get; set; }
        public string ChargeableWeightShprKG { get; set; }
        public string ChargeableWeightShprLB { get; set; }
        public string ChargeableWeightShprAmount { get; set; }
        public string ChargeableWeightCneeKG { get; set; }
        public string ChargeableWeightCneeLB { get; set; }
        public string ChargeableWeightCneeAmount { get; set; }
        public string Incoterms { get; set; }
        public string ServiceTermStart { get; set; }
        public string ServiceTermEnd { get; set; }
        public bool AWBCancelled { get; set; }
        public DateTime AWBCancelledDate { get; set; }
        public string CanceledBy { get; set; }
        public string ReasonForCancel { get; set; }
        public string PONo { get; set; }
        public string Mark { get; set; }
        public string NatureAndQuantityOfGoods { get; set; }
        public string ManifestNatureAndQuantityOfGoods { get; set; }
        public string HandlingInformation { get; set; }
        public string BookingRemarks { get; set; }
        public string PickupInstruction { get; set; }
        public bool IsDeleted { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
    }
}
