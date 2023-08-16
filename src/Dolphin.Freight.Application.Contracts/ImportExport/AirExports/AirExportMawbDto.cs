using Dolphin.Freight.AirExports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;
using Dolphin.Freight.Common;
using Volo.Abp.Data;
using NPOI.SS.UserModel;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class AirExportMawbDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 文件編號
        /// </summary>
        public string FilingNo { get; set; }
        /// <summary>
        /// 航空公司Id
        /// </summary>
        public Guid? MawbCarrierId { get; set; }
        
        /// <summary>
        ///  承運人/代理Id
        /// </summary>
        public Guid? IssuingCarrierId { get; set; }
        
        /// <summary>
        /// 提單類別
        /// </summary> 
        public AWBType AwbType { get; set; }
        /// <summary>
        /// Mawb號碼
        /// </summary> 
        public string MawbNo { get; set; }
        /// <summary>
        /// 提單日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime AwbDate { get; set; }
        /// <summary>
        /// ITN號碼
        /// </summary> 
        public string ItnNo { get; set; }
        /// <summary>
        ///  託運人Id
        /// </summary>
        public Guid? ShipperId { get; set; }
        /// <summary>
        /// 託運人
        /// </summary>
        
        /// <summary>
        ///  收貨人Id
        /// </summary>
        public Guid? ConsigneeId { get; set; }
        /// <summary>
        /// 收貨人
        /// </summary>
        
        /// <summary>
        ///  通知方Id
        /// </summary>
        public Guid? NotifyId { get; set; }
        /// <summary>
        /// 通知方
        /// </summary>
        
        /// <summary>
        /// 發佈日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 分站ID
        /// </summary>
        public Guid? OfficeId { get; set; }
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
        public Guid? MawbOperatorId { get; set; }
        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("MawbOperatorId")]
        public virtual UserData MawbOperator { get; set; }
        /// <summary>
        /// 是否直單
        /// </summary>
        public bool IsDirectMaster { get; set; }
        /// <summary>
        ///  出發地Id
        /// </summary>
        public Guid? DepatureId { get; set; }
        /// <summary>
        /// 出發日期
        /// </summary>
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
        /// <summary>
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
        ///  目的地Id
        /// </summary>
        public Guid? DestinationId { get; set; }
        /// <summary>
        /// 抵達日期
        /// </summary>
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
        /// <summary>
        /// 航空公司報價單號
        /// </summary>
        public string CarrierSpotNo { get; set; }
        /// <summary>
        /// 運費及聲明價值費(PPD / COLL)
        /// </summary>
        public string WtVal { get; set; }
        /// <summary>
        /// 其他費用(PPD / COLL)
        /// </summary>
        public string Other { get; set; }
        /// <summary>
        /// 卡車交貨地Id
        /// </summary>
        public Guid? DeliveryId { get; set; }
        /// <summary>
        /// 其他運輸資料
        /// </summary>
        public string ShippingInfo { get; set; }
        /// <summary>
        /// 托運人裝載和記數
        /// </summary>
        public string ShipperLoad { get; set; }
        /// <summary>
        /// SCI
        /// </summary>
        public string Sci { get; set; }
        /// <summary>
        /// 件數
        /// </summary>
        public double Package { get; set; }
        /// <summary>
        /// 件數Id
        /// </summary>
        public Guid? MawbPackageUnitId { get; set; }
        /// <summary>
        /// 買入價格
        /// </summary>
        public double BuyingRate { get; set; }
        /// <summary>
        /// 買入價格單位(Kg /Lb)
        /// </summary>
        public RateUnitType? BuyingRateUnitType { get; set; }
        /// <summary>
        /// 銷售單價
        /// </summary>
        public double SellingRate { get; set; }
        /// <summary>
        /// 銷售單價單位(Kg /Lb)
        /// </summary>
        public RateUnitType? SellingRateUnitType { get; set; }
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
        /// <summary>
        /// 國貿條規
        /// </summary>
        public IncotermsType? IncotermsType { get; set; }
        /// <summary>
        /// 運輸條款來
        /// </summary>
        public ServiceTermType ServiceTermTypeFrom { get; set; }
        /// <summary>
        /// 運輸條款自
        /// </summary>
        public ServiceTermType ServiceTermTypeTo { get; set; }
        /// <summary>
        /// 是否取消提單
        /// </summary>
        public bool IsAwbCancelled { get; set; }
        /// <summary>
        /// 提單取消日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? AwbCancelledDate { get; set; }
        /// <summary>
        /// 取消者Id
        /// </summary>
        public Guid? AwbCancelledOpId { get; set; }
        /// <summary>
        /// 取消者
        /// </summary>
        [ForeignKey("AwbCancelledOpId")]
        public virtual UserData AwbCancelledOp { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        public ReasonForCancelType? ReasonForCancel { get; set; }
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

        public string OfficeName { get; set; }
        public string Shipper { get; set; }
       
        public string OverseaAgentTPName { get; set; }
        public string DepatureAirportName { get; set; }
        public string DestinationAirportName { get; set; }
        public string CarrierTPName { get; set; }
        public string ConsigneeName { get; set; }
        public string NotifyName { get; set; }
        public List<Commodity> Commodities { get; set; }
        public string PONo { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
        public bool IsLocked { get; set; }
    }
}
