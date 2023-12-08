using Dolphin.Freight.AirExports;
using Dolphin.Freight.AirImports;
using Dolphin.Freight.Settings.Substations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public class AirImportMawb : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        /// <summary>
        /// 文件編號
        /// </summary>
        public string FilingNo { get; set; }
        /// <summary>
        /// Mawb號碼
        /// </summary> 
        public string MawbNo { get; set; }
        /// <summary>
        /// 分站ID
        /// </summary>
        public Guid? OfficeId { get; set; }
        /// <summary>
        /// 分站
        /// </summary>
        [ForeignKey("OfficeId")]
        public virtual Substation Office { get; set; }
        /// <summary>
        /// 提單類別
        /// </summary> 
        public AWBType AwbType { get; set; }
        /// <summary>
        /// 發佈日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? OverseaAgentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("OverseaAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner OverseaAgent { get; set; }
        /// <summary>
        /// 航空公司Id
        /// </summary>
        public Guid? CarrierId { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        [ForeignKey("CarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Carrier { get; set; }
        /// <summary>
        ///  航空公司財務窗口Id
        /// </summary>
        public Guid? AwbAcctCarrierId { get; set; }
        /// <summary>
        /// 航空公司財務窗口
        /// </summary>
        [ForeignKey("AwbAcctCarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner AwbAcctCarrier { get; set; }
        /// <summary>
        ///  共通運裝人Id
        /// </summary>
        public Guid? CoLoaderId { get; set; }
        /// <summary>
        /// 共通運裝人
        /// </summary>
        [ForeignKey("CoLoaderId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CoLoader { get; set; }
        /// <summary>
        /// 操作員Id
        /// </summary>
        public Guid? OPId { get; set; }
        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("OPId")]
        public virtual UserData OP { get; set; }
        /// <summary>
        /// 是否直單
        /// </summary>
        public bool IsDirectMaster { get; set; }
        /// <summary>
        ///  託運人Id
        /// </summary>
        public Guid? ShipperId { get; set; }
        /// <summary>
        /// 託運人
        /// </summary>
        [ForeignKey("ShipperId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Shipper { get; set; }
        /// <summary>
        ///  收貨人Id
        /// </summary>
        public Guid? ConsigneeId { get; set; }
        /// <summary>
        /// 收貨人
        /// </summary>
        [ForeignKey("ConsigneeId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Consignee { get; set; }
        /// <summary>
        ///  通知方Id
        /// </summary>
        public Guid? NotifyId { get; set; }
        /// <summary>
        /// 通知方
        /// </summary>
        [ForeignKey("NotifyId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Notify { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public Guid? CustomerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("CustomerId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Customer { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public Guid? BillToId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("BillToId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner BillTo { get; set; }
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
        ///  出發地Id
        /// </summary>
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
        /// <summary>
        ///  中轉航班1-Id
        /// </summary>
        public Guid? RouteTrans1Id { get; set; }
        /// <summary>
        /// 中轉航班1
        /// </summary>
        [ForeignKey("RouteTrans1Id")]
        public virtual Dolphin.Freight.ImportExport.AirExports.Airport RouteTrans1 { get; set; }
        /// <summary>
        /// 中轉航班1到達日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime RouteTrans1ArrivalDate { get; set; }
        /// <summary>
        /// 中轉航班1出發日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime RouteTrans1DepatureDate { get; set; }
        /// <summary>
        /// 中轉航班1航班號碼
        /// </summary>
        public string RouteTrans1FlightNo { get; set; }
        /// <summary>
        /// 中轉航班1航空公司Id
        /// </summary>
        public Guid? RouteTrans1CarrierId { get; set; }
        /// <summary>
        /// 中轉航班1航空公司
        /// </summary>
        [ForeignKey("RouteTrans1CarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner RouteTrans1Carrier { get; set; }
        /// <summary>
        ///  中轉航班2-Id
        /// </summary>
        public Guid? RouteTrans2Id { get; set; }
        /// <summary>
        /// 中轉航班2
        /// </summary>
        [ForeignKey("RouteTrans2Id")]
        public virtual Dolphin.Freight.ImportExport.AirExports.Airport RouteTrans2 { get; set; }
        /// <summary>
        /// 中轉航班2到達日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime RouteTrans2ArrivalDate { get; set; }
        /// <summary>
        /// 中轉航班2出發日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime RouteTrans2DepatureDate { get; set; }
        /// <summary>
        /// 中轉航班2航班號碼
        /// </summary>
        public string RouteTrans2FlightNo { get; set; }
        /// <summary>
        /// 中轉航班2航空公司Id
        /// </summary>
        public Guid? RouteTrans2CarrierId { get; set; }
        /// <summary>
        /// 中轉航班2航空公司
        /// </summary>
        [ForeignKey("RouteTrans2CarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner RouteTrans2Carrier { get; set; }
        /// <summary>
        ///  中轉航班3-Id
        /// </summary>
        public Guid? RouteTrans3Id { get; set; }
        /// <summary>
        /// 中轉航班3
        /// </summary>
        [ForeignKey("RouteTrans3Id")]
        public virtual Dolphin.Freight.ImportExport.AirExports.Airport RouteTrans3 { get; set; }
        /// <summary>
        /// 中轉航班3到達日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime RouteTrans3ArrivalDate { get; set; }
        /// <summary>
        /// 中轉航班3出發日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime RouteTrans3DepatureDate { get; set; }
        /// <summary>
        /// 中轉航班3航班號碼
        /// </summary>
        public string RouteTrans3FlightNo { get; set; }
        /// <summary>
        /// 中轉航班3航空公司Id
        /// </summary>
        public Guid? RouteTrans3CarrierId { get; set; }
        /// <summary>
        /// 中轉航班3航空公司
        /// </summary>
        [ForeignKey("RouteTrans3CarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner RouteTrans3Carrier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FPOEDepature { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FPOETrans1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FPOETrans2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FPOETrans3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FPOEDestination { get; set; }

        /// <summary>
        ///  目的地Id
        /// </summary>
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
        public DateTime ArrivalDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? FreightLocationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("FreightLocationId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner FreightLocation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime StorageStartDate { get; set; }
        /// <summary>
        /// 件數
        /// </summary>
        public double Package { get; set; }
        /// <summary>
        /// 件數Id
        /// </summary>
        public Guid? MawbPackageUnitId { get; set; }
        /// <summary>
        /// 件數單位
        /// </summary>
        [ForeignKey("MawbPackageId")]
        public virtual Dolphin.Freight.Settings.PackageUnits.PackageUnit MawbPackageUnit { get; set; }
        /// <summary>
        /// 毛重公斤
        /// </summary>
        public double GrossWeightKg { get; set; }
        /// <summary>
        /// 毛重磅
        /// </summary>
        public double GrossWeightLb { get; set; }
        /// <summary>
        /// 計費重量公斤
        /// </summary>
        public double ChargeableWeightKg { get; set; }
        /// <summary>
        /// 計費重量磅
        /// </summary>
        public double ChargeableWeightLb { get; set; }
        /// <summary>
        /// 體積重量公斤
        /// </summary>
        public double VolumeWeightKg { get; set; }
        /// <summary>
        /// 體積重量CBM
        /// </summary>
        public double VolumeWeightCbm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FreightType FreightType { get; set; }
        /// <summary>
        /// 國貿條規
        /// </summary>
        public IncotermsType IncotermsType { get; set; }
        /// <summary>
        /// 運輸條款來
        /// </summary>
        public ServiceTermType ServiceTermTypeFrom { get; set; }
        /// <summary>
        /// 運輸條款自
        /// </summary>
        public ServiceTermType ServiceTermTypeTo { get; set; }
        /// <summary>
        /// 業務推廣人
        /// </summary>
        public Guid? BusinessReferredId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("BusinessReferredId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner BusinessReferred { get; set; }
        /// <summary>
        /// 是否電子商務
        /// </summary>
        public bool IsECom { get; set; }
        /// <summary>
        /// 顯示單位
        /// </summary>
        public DisplayUnitType DisplayUnit { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public bool IsLocked { get; set; }

        [ForeignKey("RouteDepartureId")]
        public virtual Dolphin.Freight.ImportExport.AirExports.Airport RouteDeparture { get; set; }
        public DateTime? RouteDepartureArrivalDate { get; set; }
        public DateTime? RouteDepatureDate { get; set; }
        public string RouteDepartureFlightNo { get; set; }
        public Guid? RouteDepartureCarrierId { get; set; }
        [ForeignKey("RouteDepartureCarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner RouteDepartureCarrier { get; set; }

        public Guid? RouteDestinationId { get; set; }

        [ForeignKey("RouteDestinationId")]
        public virtual Dolphin.Freight.ImportExport.AirExports.Airport RouteDestination { get; set; }
        public DateTime? RouteDestinationArrivalDate { get; set; }


    }
}
