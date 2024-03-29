﻿using Dolphin.Freight.AirExports;
using Dolphin.Freight.AirImports;
using Dolphin.Freight.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public class AirImportMawbDto : AuditedEntityDto<Guid>
    {
        public string HawbJson { get; set; }
        public bool IsPDF { get; set; }
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
        public string OfficeName { get; set; }
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
        public string OverseaAgentTPName { get; set; }
        /// 航空公司Id
        /// </summary>
        public Guid? CarrierId { get; set; }
        public string CarrierTPName { get; set; }
        /// <summary>
        ///  航空公司財務窗口Id
        /// </summary>
        public Guid? AwbAcctCarrierId { get; set; }
        /// <summary>
        ///  共通運裝人Id
        /// </summary>
        public Guid? CoLoaderId { get; set; }
        /// <summary>
        /// 操作員Id
        /// </summary>
        public Guid? OPId { get; set; }
        /// <summary>
        /// 是否直單
        /// </summary>
        public bool IsDirectMaster { get; set; }
        /// <summary>
        ///  託運人Id
        /// </summary>
        public Guid? ShipperId { get; set; }
        /// <summary>
        ///  收貨人Id
        /// </summary>
        public Guid? ConsigneeId { get; set; }
        /// <summary>
        ///  通知方Id
        /// </summary>
        public Guid? NotifyId { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public Guid? CustomerId { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public Guid? BillToId { get; set; }
        // <summary>
        /// 操作員Id
        /// </summary>
        public Guid? SalesId { get; set; }
        public string SalesName { get; set; }
        /// <summary>
        ///  出發地Id
        /// </summary>
        public Guid? DepatureId { get; set; }
        public string DepatureAirportName { get; set; }
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
        ///  中轉航班2-Id
        /// </summary>
        public Guid? RouteTrans2Id { get; set; }
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
        ///  中轉航班3-Id
        /// </summary>
        public Guid? RouteTrans3Id { get; set; }
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
        public string DestinationAirportName { get; set; }
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
        public string DepartureName { get; set; }
        public string DestinationName { get; set; }
        public string CarrierName { get; set; }
        public FreightPageType PageType { get; set; }
        public bool IsLocked { get; set; }
        public List<AllHawbListAirImport> AllHawbListAirImports { get; set; }
        public string ARBalance { get; set; }
        public string APBalance { get; set; }
        public string DCBalance { get; set; }
        public string Mark { get; set; }
        public string NatureAndQuantityOfGoods { get; set; }
        public string ManifestNatureAndQuantityOfGoods { get; set; }
        public string HandlingInformation { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
        public string OpName { get; set; }
        public Guid? RouteDepartureId { get; set; }
        public DateTime? RouteDepartureArrivalDate { get; set; }
        public DateTime? RouteDepatureDate { get; set; }
        public string RouteDepartureFlightNo { get; set; }
        public Guid? RouteDepartureCarrierId { get; set; }

        public Guid? RouteDestinationId { get; set; }
        public DateTime? RouteDestinationArrivalDate { get; set; }
    }
    public class AllHawbListAirImport
    {
        public string HawbNo { get; set; }
        public string Customer { get; set; }
        public string Shipper { get; set; }
        public string Packages { get; set; }
        public string Chargeable_Weight { get; set; }
        public string Freight_Weight { get; set; }
    }
}
