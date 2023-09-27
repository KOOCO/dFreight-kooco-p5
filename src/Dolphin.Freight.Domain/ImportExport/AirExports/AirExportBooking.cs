using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Users;
using Volo.Abp;
using Dolphin.Freight.AirExports;
using Dolphin.Freight.AirImports;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class AirExportBooking : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        

        public Guid? ReferenceId { get; set; }
        /// <summary>
        /// 0：船期，1：MBL
        /// </summary>
        public int ReferenceType { get; set; }
        /// <summary>
        /// SO編號
        /// </summary>
        [Required]
        public string SoNo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime SoNoDate { get; set; }
        /// <summary>
        /// HBL編號
        /// </summary>
        public string HblNo { get; set; }
        /// <summary>
        /// Itn編號
        /// </summary>
        public string ItnNo { get; set; }
        /// <summary>
        /// 客戶參考編號
        /// </summary>
        public string CustomerRefNo { get; set; }
        /// <summary>
        /// 文件編號
        /// </summary>
        public string DocNo { get; set; }
      
        /// <summary>
        /// 是否扣留
        /// </summary>
        public bool IsHold { get; set; }
        /// <summary>
        /// 扣留原因
        /// </summary>
        public string HoldReason { get; set; }
        public Guid? SalesPersonId { get; set; }
        /// <summary>
        /// 業務員
        /// </summary>
        [ForeignKey("SaleId")]
        public virtual UserData SalesPerson { get; set; }
        /// <summary>
        /// 扣留人員ID
        /// </summary>
        public Guid? HolderId { get; set; }
        /// <summary>
        /// 扣留人員
        /// </summary>
        [ForeignKey("HolderId")]
        public virtual UserData Holder { get; set; }
        /// <summary>
        /// BL已取消
        /// </summary>
        public bool SubAgentAwb { get; set; }
        
        public string ReferenceNo { get; set; }
        /// <summary>
        /// 船公司S/O號碼
        /// </summary>
        public string CarrierBkgNo { get; set; }
        /// <summary>
        /// 船公司ID
        /// </summary>
        public Guid? CarrierId { get; set; }
        /// <summary>
        /// 船公司
        /// </summary>
        [ForeignKey("CarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Carrier { get; set; }
        /// <summary>
        /// 運輸模式ID
        /// </summary>
      
        /// <summary>
        /// 運輸條款FronID
        /// </summary>
        public ServiceTermType SvcTermTypeFrom { get; set; }
       
        /// <summary>
        /// 運輸條款ToID
        /// </summary>
        public ServiceTermType SvcTermTypeTo { get; set; }

        public IncotermsType IncotermsType { get; set; }
       
        /// <summary>
        /// 實際託運人ID
        /// </summary>
        public Guid? ShipperId { get; set; }
        /// <summary>
        /// 實際託運人
        /// </summary>
        [ForeignKey("ShipperId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Shipper { get; set; }
        /// <summary>
        /// 客戶ID
        /// </summary>
        public Guid? CustomerId { get; set; }
        /// <summary>
        /// 客戶
        /// </summary>
        [ForeignKey("CustomerId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Customer { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        public Guid? BillToId { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        [ForeignKey("BillToId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner BillTo { get; set; }
        /// <summary>
        /// 收貨方ID
        /// </summary>
        public Guid? ConsigneeId { get; set; }
        /// <summary>
        /// 收貨方
        /// </summary>
        [ForeignKey("ConsigneeId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Consignee { get; set; }
        /// <summary>
        /// 通知方ID
        /// </summary>
        public Guid? NotifyId { get; set; }
        /// <summary>
        /// 通知方
        /// </summary>
        [ForeignKey("NotifyId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Notify { get; set; }
        /// <summary>
        /// 本地代理ID
        /// </summary>
        public Guid? ActualShipperId { get; set; }
        /// <summary>
        /// 本地代理
        /// </summary>
        [ForeignKey("ActualShipperId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner ActualShipper { get; set; }
        /// <summary>
        /// HBL代理ID
        /// </summary>
        public Guid? AgentId { get; set; }
        /// <summary>
        /// HBL代理
        /// </summary>
        [ForeignKey("AgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Agent { get; set; }
        /// <summary>
        /// 轉運代理ID
        /// </summary>
        public Guid? ForwardingAgentId { get; set; }
        /// <summary>
        /// 轉運代理
        /// </summary>
        [ForeignKey("ForwardingAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner ForwardingAgent { get; set; }
        /// <summary>
        /// 共同裝運人ID
        /// </summary>
        public Guid? CoLoaderId { get; set; }
        /// <summary>
        /// 共同裝運人
        /// </summary>
        [ForeignKey("CoLoaderId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CoLoader { get; set; }
     
       
        /// <summary>
        /// 收貨地(POR)ID
        /// </summary>

        public DateTime FinalEta { get; set; }
      
       
       
        public Guid? CargoTypeId { get; set; }
        /// <summary>
        /// 貨物類別
        /// </summary>
        [ForeignKey("CargoTypeId")]
        public virtual SysCode CargoType { get; set; }
      
   
        /// <summary>
        /// 卡車收貨地ID
        /// </summary>
        public Guid? CargoPickupId { get; set; }
        /// <summary>
        /// 卡車收貨地
        /// </summary>
        [ForeignKey("CargoPickupId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CargoPickup { get; set; }
        /// <summary>
        /// 卡車ID
        /// </summary>
        public Guid? TruckerId { get; set; }
        /// <summary>
        /// 卡車行
        /// </summary>
        [ForeignKey("TruckerId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Truckerr { get; set; }
        /// <summary>
        /// 卡車交貨地ID
        /// </summary>
        public Guid? DeliveryToId { get; set; }
        /// <summary>
        /// 卡車交貨地
        /// </summary>
        [ForeignKey("DeliveryToId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner DeliveryTo { get; set; }
        /// <summary>
        /// 貨物就緒日期
        /// </summary>
        public DateTime? CargoArrivalDate { get; set; }
       
       
        public Guid? OfficeId { get; set; }
        /// <summary>
        /// 分站
        /// </summary>
        [ForeignKey("OfficeId")]
        public virtual Substation Office { get; set; }
        /// <summary>
        /// 是否可堆積
        /// </summary>
        public bool IsStackable { get; set; }
        /// <summary>
        /// 嘜頭
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 貨描
        /// </summary>
        public string NatureAndQuantity { get; set; }
        /// <summary>
        /// S/O備註
        /// </summary>
        public string BookingRemark { get; set; }
        /// <summary>
        /// 提單做法備註
        /// </summary>
        public string DeliveryInstruction { get; set; }
        public string HandlingInformation { get; set; }
        /// <summary>
        /// 貨物艙單備註
        /// </summary>
        public string CargoRemark { get; set; }
     
        /// <summary>
        /// P.O.號碼
        /// </summary>
        public string PoNo { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public Guid? DepatureId { get; set; }
        /// <summary>
        /// 出發地
        /// </summary>
        [ForeignKey("DepatureId")]
        public virtual Dolphin.Freight.ImportExport.AirExports.Airport Depature { get; set; }
        /// <summary>
        /// 出發日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime DepatureDate { get; set; }
        /// <summary>
        /// 航班號碼
        /// </summary>
        public string FlightNo { get; set; }
        public Guid? DestinationId { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        [ForeignKey("DestinationId")]
        public virtual Dolphin.Freight.ImportExport.AirExports.Airport Destination { get; set; }
        /// <summary>
        /// 抵達日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? ArrivalDate { get; set; }
        /// <summary>
        /// 運輸聲明價值
        /// </summary>
        public string DVCarriage { get; set; }
        /// <summary>
        /// 海關聲明價值
        /// </summary>
        public string DVCustomer { get; set; }
        /// <summary>
        /// 保險金額
        /// </summary>
        public string Insurance { get; set; }
        public string WtVal { get; set; }
        /// <summary>
        /// 其他費用(PPD / COLL)
        /// </summary>
        public string Other { get; set; }
        public double Package { get; set; }
        public Guid? MawbPackageUnitId { get; set; }
        /// <summary>
        /// 件數單位
        /// </summary>
        [ForeignKey("PackageId")]
        public virtual Dolphin.Freight.Settings.PackageUnits.PackageUnit MawbPackageUnit { get; set; }
        public double BuyingRate { get; set; }
        /// <summary>
        /// 買入價格單位(Kg /Lb)
        /// </summary>
        public RateUnitType BuyingRateUnitType { get; set; }
        public double SellingRate { get; set; }
        /// <summary>
        /// 銷售單價單位(Kg /Lb)
        /// </summary>
        public RateUnitType SellingRateUnitType { get; set; }
        /// <summary>
        /// 體積重量公斤
        /// </summary>
        public double VolumeWeightKg { get; set; }
        /// <summary>
        /// 體積重量CBM
        /// </summary>
        public double VolumeWeightCbm { get; set; }
        /// <summary>
        /// 毛重公斤
        /// </summary>
        public double GrossWeightKg { get; set; }
        /// <summary>
        /// 毛重磅
        /// </summary>
        public double GrossWeightLb { get; set; }
        /// <summary>
        /// 毛重金額
        /// </summary>
        public double GrossWeightAmount { get; set; }
        /// <summary>
        /// 提單毛重公斤
        /// </summary>
        public double AwbGrossWeightKg { get; set; }
        /// <summary>
        /// 提單毛重磅
        /// </summary>
        public double AwbGrossWeightLb { get; set; }
        /// <summary>
        /// 提單毛重金額
        /// </summary>
        public double AwbGrossWeightAmount { get; set; }
        /// <summary>
        /// 計費重量公斤
        /// </summary>
        public double ChargeableWeightKg { get; set; }
        /// <summary>
        /// 計費重量磅
        /// </summary>
        public double ChargeableWeightLb { get; set; }
        /// <summary>
        /// 計費重量金額
        /// </summary>
        public double ChargeableWeightAmount { get; set; }
        /// <summary>
        /// 提單計費重量公斤
        /// </summary>
        public double AwbChargeableWeightKg { get; set; }
        /// <summary>
        /// 提單計費重量磅
        /// </summary>
        public double AwbChargeableWeightLb { get; set; }
        /// <summary>
        /// 提單計費重量金額
        /// </summary>
        public double AwbChargeableWeightAmount { get; set; }
        public DisplayUnitType DisplayUnit { get; set; }
        public SalesType SalesType { get; set; }
        public ShipType ShipType { get; set; }


    }
}