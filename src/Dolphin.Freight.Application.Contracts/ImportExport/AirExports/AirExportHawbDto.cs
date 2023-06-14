using Dolphin.Freight.AirExports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class AirExportHawbDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Mawb Id
        /// </summary>
        public Guid? MawbId { get; set; }
        /// <summary>
        /// Hawb 號碼
        /// </summary>
        [Required]
        public string HawbNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? BookingNo { get; set; }
        public DateTime BookingDate { get; set; }
        public string ITNNo { get; set; }

        public Guid? QuotationId { get; set; }
        public string ActualShippedr { get; set; }
        public string Customer { get; set; }
        public string BillTo { get; set; }
        /// <summary>
        ///  收貨人Id
        /// </summary>
        public Guid? ConsigneeId { get; set; }
        /// <summary>
        /// 收貨人
        /// </summary>
        public string Consignee { get; set; }
        public string Notify { get; set; }
        public string OverseaAgent { get; set; }
        public string IssuingCarrier { get; set; }
        public string Trucker { get; set; }
        public string CardColorValue { get; set; }
        // <summary>
        /// 操作員Id
        /// </summary>
        public Guid? SalesId { get; set; }
        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("SalesId")]
        public virtual UserData Sales { get; set; }
        /// <summary>
        /// 操作員Id
        /// </summary>
        public Guid? OPId { get; set; }

        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("OPId")]
        public virtual UserData OP { get; set; }

        public bool SubAgentAwb { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
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

        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
