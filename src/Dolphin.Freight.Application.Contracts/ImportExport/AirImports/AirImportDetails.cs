﻿using Dolphin.Freight.AirExports;
using Dolphin.Freight.AirImports;
using Dolphin.Freight.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Security.Permissions;
using System.Text;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public class AirImportDetails
    {
        public Guid? Id { get; set; }
        public bool IsPDF { get; set; }
        public bool IsOceanExportHbl { get; set; }
        public string AirWayBillNo { get; set; }
        public string TotalPackage { get; set; }
        public Guid? HawbId { get; set; }
        public Guid MawbId { get; set; }
        public string HawbNo { get; set; }
        public string DocNumber { get; set; }
        public string FilingNo { get; set; }
        /// <summary>
        /// Mawb號碼
        /// </summary> 
        public string MawbNo { get; set; }       
        public string Hawb_No { get; set; }       
        public string Hawb_Nos { get; set; }
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
        public string IATA { get; set; }
        /// 航空公司Id
        /// </summary>
        public Guid? CarrierId { get; set; }
        public string CarrierTPName { get; set; }
        /// <summary>
        ///  航空公司財務窗口Id
        /// </summary>
        public Guid? AwbAcctCarrierId { get; set; }
        public string AwbAcctCarrierName { get; set; }
        /// <summary>
        ///  共通運裝人Id
        /// </summary>
        public Guid? CoLoaderId { get; set; }
        public string CoLoaderName { get; set; }
        /// <summary>
        /// 操作員Id
        /// </summary>
        public Guid? OPId { get; set; }
        public string OPName { get; set; }
        public string HItNo { get; set; }
        public string HItDate { get; set; }
        public string CurrentDate { get; set; }
        public string HItLocation { get; set; }
        public string HMark { get; set; }
        public string HDescription { get; set; }
        public string HRemark { get; set; }
        /// <summary>
        /// 是否直單
        /// </summary>
        public bool IsDirectMaster { get; set; }
        /// <summary>
        ///  託運人Id
        /// </summary>
        public Guid? ShipperId { get; set; }
        public string ShipperName { get; set; }
        /// <summary>
        ///  收貨人Id
        /// </summary>
        public Guid? ConsigneeId { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        /// <summary>
        ///  通知方Id
        /// </summary>
        public Guid? NotifyId { get; set; }
        public string NotifyName { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomBroker { get; set; }
        public string CustomBrokerName { get; set; }
        public string HPackageUnitName { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public Guid? BillToId { get; set; }
        public string BillToName { get; set; }
        public string BillToDetail { get; set; }
        // <summary>
        /// 操作員Id
        /// </summary>
        public Guid? SalesId { get; set; }
        public string SalesName { get; set; }
        public string SalesType { get; set; }
        public string PONo { get; set; }
        /// <summary>
        ///  出發地Id
        /// </summary>
        public Guid? DepatureId { get; set; }
        public string DepatureName { get; set; }
        public string DepatureAirportName { get; set; }
        /// <summary>
        /// 出發日期
        /// </summary>    
        [DataType(DataType.Date)]
        public DateTime? DepatureDate { get; set; }
        /// <summary>
        /// 航班號碼
        /// </summary>
        public string FlightNo { get; set; } 
        public string HTruckerName { get; set; }
        /// <summary>
        ///  中轉航班1-Id
        /// </summary>
        public Guid? RouteTrans1Id { get; set; }
        public string RouteTrans1Name { get; set; }
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
        public string RouteTrans1CarrierName { get; set; }
        ///  中轉航班2-Id
        /// </summary>
        public Guid? RouteTrans2Id { get; set; }
        public string RouteTrans2Name { get; set; }
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
        public string RouteTrans2CarrierName { get; set; }
        /// <summary>
        ///  中轉航班3-Id
        /// </summary>
        public Guid? RouteTrans3Id { get; set; }
        public string RouteTrans3Name { get; set; }
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
        public string RouteTrans3CarrierName { get; set; }
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
        public string DestinationCountry { get; set; }
        /// <summary>
        /// 抵達日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? ArrivalDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? FreightLocationId { get; set; }
        public string FreightLocationName { get; set; }
        public string FreightLocationWithPhoneNo { get; set; }
        public string FreightLocationFaxNo { get; set; }
        public string FreightLocationPhoneNo { get; set; }
        public string FreightLocationAddress { get; set; }
        public string MFreightLocationName { get; set; }
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
        public string MawbPackageUnitName { get; set; }
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
        public string BusinessReferredName { get; set; }
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
        public string HawbString { get; set; }
        public string ChargableWeight { get; set; }
        public string ChargableWeightStr { get; set; }
        public string GrossWeightStr { get; set; }
        public string VolumeWeightStr { get; set; }
        public string MeasurementStr { get; set; }
        public string CurrentAgentId { get; set; }
        public string CurrentAgent { get; set; }
        public string PackagesStr { get; set; }
        public FreightPageType PageType { get; set; }
        public List<OverSeaAgentAirImport> OverSeaAgents { get; set; }
        public List<HawbNo> HawbNos { get; set; }
        public List<Hawb> HawbList { get; set; }
        public string HawbListJson { get; set; }
        public string Hawb_Id { get; set; }
        public string ITNo { get; set; }
        public string ReleasedBy { get; set; }
        public string FinalDestination { get; set; }
        public string DeliveryLocationName { get; set; }
        public string FDestETA { get; set; }
        public DateTime? FinalDestETA { get; set; }
        public DateTime? LastFreeDay { get; set; }
        public string Remark { get; set; }
        public string SubHawbJson { get; set; }
        public List<SubHawbs> SubHawbs { get; set; }
        public string Trucker { get; set; }
        public DateTime ITDate { get; set; }
        public string ITIssuedLocation { get; set; }
        public string ClassOfEntry { get; set; }

        public List<AirImportMawbDto> MawbList { get; set; }
    }
    public class OverSeaAgentAirImport
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class HawbNo
    {
        public string Id { get; set; }
        public string HawbNos { get; set; }
        public string OverSeaAgent { get; set; }
        public string GrossWeightKG { get; set; }
        public string GrossWeightLBS { get; set; }
        public string ChargableWeightKG { get; set; }
        public string ChargableWeightLBS { get; set; }
        public string VolumeWeightKG { get; set; }
        public string MeasurementWeight { get; set; }
        public string Packages { get; set; }
        public string Consignee { get; set; }
        public string Customer { get; set; }
        public string Notify { get; set; }
    }
    public class Hawb
    {
        public string Id { get; set; }
        public string HawbNo { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Notify { get; set; }
        public string FinalDestName { get; set; }
        public string FDestETA { get; set; }
        public DateTime? LastFreeDay { get; set; }
        public string FreightLocation { get; set; }
        public string ITNo { get; set; }
        public string ITIssuePlace { get; set; }
        public string ITDate { get; set; }
        public string SubHawbJson { get; set; }
        public List<SubHawbs> SubHawbs { get; set; }
    }
}
