using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.OceanImports
{
    /// <summary>
    /// 海運出口MB/L
    /// </summary>
    public class OceanImportMbl : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        /// <summary>
        /// 運輸編號
        /// </summary>
        public string ShipmentNo { get; set; }
        /// <summary>
        /// 文件編號
        /// </summary>
        public string FilingNo { get; set; }
        /// <summary>
        /// MB/L編號
        /// </summary>
        public string MblNo { get; set; }
        /// <summary>
        /// 分站ID
        /// </summary>
        public Guid?  OfficeId { get; set; }
        /// <summary>
        /// 分站
        /// </summary>
        [ForeignKey("OfficeId")]
        public virtual Substation Office { get; set; }
        /// <summary>
        /// 提單類別ID
        /// </summary>
        public Guid?  BlTypeId { get; set; }
        /// <summary>
        /// 提單類別
        /// </summary>
        [ForeignKey("BlTypeId")]
        public virtual SysCode BlType { get; set; }
        /// <summary>
        /// 發布日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }
        /// <summary>
        /// 船公司S/O號碼
        /// </summary>
        public string SoNo { get; set; }
        /// <summary>
        /// ITN號碼
        /// </summary>
        public string ItnNo { get; set; }
        /// <summary>
        /// AMS號碼
        /// </summary>
        public string AmsNo { get; set; }
        /// <summary>
        /// 船公司ID
        /// </summary>
        public Guid?  MblCarrierId { get; set; }
        /// <summary>
        /// 船公司
        /// </summary>
        [ForeignKey("MblCarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblCarrier { get; set; }
        /// <summary>
        /// 船公司報表上顯示的資訊
        /// </summary>
        public string MblCarrierContent { get; set; }
        /// <summary>
        /// 船公司(財務窗口)ID
        /// </summary>
        public Guid?  BlAcctCarrierId { get; set; }
        /// <summary>
        /// 船公司(財務窗口)
        /// </summary>
        [ForeignKey("BlAcctCarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner BlAcctCarrier { get; set; }
        /// <summary>
        /// 船公司(財務窗口)報表上顯示的資訊
        /// </summary>
        public string BlAcctCarrierContent { get; set; }
        /// <summary>
        /// 本地代理ID
        /// </summary>
        public Guid?  ShippingAgentId { get; set; }
        /// <summary>
        /// 本地代理
        /// </summary>
        [ForeignKey("ShippingAgentId")]
        public virtual  Dolphin.Freight.TradePartners.TradePartner ShippingAgent { get; set; }
        /// <summary>
        /// 本地代理報表上顯示的資訊
        /// </summary>
        public string ShippingAgentContent { get; set; }
        /// <summary>
        /// 海外代理收貨人ID
        /// </summary>
        public Guid?  MblOverseaAgentId { get; set; }
        /// <summary>
        /// 海外代理收貨人
        /// </summary>
        [ForeignKey("MblOverseaAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblOverseaAgent { get; set; }
        /// <summary>
        /// 海外代理收貨人報表上顯示的資訊
        /// </summary>
        public string MblOverseaAgentContent { get; set; }
        /// <summary>
        /// 通知方ID
        /// </summary>
        public Guid?  MblNotifyId { get; set; }
        /// <summary>
        /// 通知方
        /// </summary>
        [ForeignKey("MblNotifyId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblNotify { get; set; }
        /// <summary>
        /// 通知方報表上顯示的資訊
        /// </summary>
        public string MblNotifyContent { get; set; }
        /// <summary>
        /// 轉運代理ID
        /// </summary>
        public Guid?  ForwardingAgentId { get; set; }
        /// <summary>
        /// 轉運代理
        /// </summary>
        [ForeignKey("ForwardingAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner ForwardingAgent { get; set; }
        /// <summary>
        /// 轉運代理報表上顯示的資訊
        /// </summary>
        public string ForwardingAgentContent { get; set; }
        /// <summary>
        /// 共同裝運人ID
        /// </summary>
        public Guid?  CoLoaderId { get; set; }
        /// <summary>
        /// 共同裝運人
        /// </summary>
        [ForeignKey("CoLoaderId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CoLoader { get; set; }
        /// <summary>
        /// 共同裝運人報表上顯示的資訊
        /// </summary>
        public string CoLoaderContent { get; set; }
        /// <summary>
        /// 是否使用CareOf
        /// </summary>
        public bool IsUsedCareOf{ get; set; }
        /// <summary>
        /// C/O ID
        /// </summary>
        public Guid? CareOfId { get; set; }
        /// <summary>
        /// C/O
        /// </summary>
        [ForeignKey("CareOfId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CareOf { get; set; }
        /// <summary>
        /// C/O報表上顯示的資訊
        /// </summary>
        public string CareOfContent { get; set; }
        /// <summary>
        /// 操作員ID
        /// </summary>
        public Guid?  MblOperatorId { get; set; }
        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("MblOperatorId")]
        public virtual UserData MblOperator { get; set; }

        /// <summary>
        /// 船公司合約編號
        /// </summary>
        public string CarrierContractNo { get; set; }
        /// <summary>
        /// 是否直單
        /// </summary>
        public bool IsDirect { get; set; }
        /// <summary>
        /// 客戶參考編號
        /// </summary>
        public string CustomerRefNo { get; set; }
        /// <summary>
        /// 客戶ID
        /// </summary>
        public Guid?  MblCustomerId { get; set; }
        /// <summary>
        /// 客戶
        /// </summary>
        [ForeignKey("MblCustomerId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblCustomer { get; set; }
        /// <summary>
        /// 客戶報表上顯示的資訊
        /// </summary>
        public string MblCustomerContent { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        public Guid?  MblBillToId { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        [ForeignKey("MblBillToId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblBillTo { get; set; }
        /// <summary>
        /// 收票人報表上顯示的資訊
        /// </summary>
        public string MblBillToContent { get; set; }
        /// <summary>
        /// 收貨人ID
        /// </summary>
        public Guid?  MblConsigneeId { get; set; }
        /// <summary>
        /// 收貨人
        /// </summary>
        [ForeignKey("MblConsigneeId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblConsignee { get; set; }
        /// <summary>
        /// 收貨人報表上顯示的資訊
        /// </summary>
        public string MblConsigneeContent { get; set; }
        /// <summary>
        /// 業務類別ID
        /// </summary>
        public Guid?  MblSalesTypeId { get; set; }
        /// <summary>
        /// 業務類別
        /// </summary>
        [ForeignKey("MblSalesTypeId")]
        public virtual SysCode MblSalesType { get; set; }
        /// <summary>
        /// 訂倉方式
        /// </summary>
        public string BookingMode { get; set; }
        /// <summary>
        /// 貨物類別ID
        /// </summary>
        public Guid?  CargoTypeId { get; set; }
        /// <summary>
        /// 貨物類別
        /// </summary>
        [ForeignKey("CargoTypeId")]
        public virtual SysCode CargoType { get; set; }
        /// <summary>
        /// 業務員ID
        /// </summary>
        public Guid?  MblSaleId { get; set; }
        /// <summary>
        /// 業務員
        /// </summary>
        [ForeignKey("MblSaleId")]
        public virtual UserData MblSale { get; set; }
        /// <summary>
        /// 船名
        /// </summary>
        public string VesselName { get; set; }
        /// <summary>
        /// 航次
        /// </summary>
        public string Voyage { get; set; }
        /// <summary>
        /// 收貨地(POR)ID
        /// </summary>
        public Guid?  PorId { get; set; }
        /// <summary>
        /// 收貨地(POR)
        /// </summary>
        [ForeignKey("PorId")]
        public virtual Port Por { get; set; }
        /// <summary>
        /// 收貨地(POR) ETD
        /// </summary>
        public DateTime? PorEtd { get; set; }
        /// <summary>
        /// 裝貨港(POL)ID
        /// </summary>
        public Guid?  PolId { get; set; }
        /// <summary>
        /// 裝貨港(POL)
        /// </summary>
        [ForeignKey("PolId")]
        public virtual Port Pol { get; set; }
        /// <summary>
        /// 裝貨港(POL) ETD
        /// </summary>
        public DateTime? PolEtd { get; set; }
        /// <summary>
        /// 卸貨港(POD)ID
        /// </summary>
        public Guid?  PodId { get; set; }
        /// <summary>
        /// 卸貨港(POD)
        /// </summary>
        [ForeignKey("PodId")]
        public virtual Port Pod { get; set; }
        /// <summary>
        /// 卸貨港(POD) ETA
        /// </summary>
        public DateTime? PodEta { get; set; }
        /// <summary>
        /// 交貨地(DEL)ID
        /// </summary>
        public Guid?  DelId { get; set; }
        /// <summary>
        /// 交貨地(DEL)
        /// </summary>
        [ForeignKey("DelId")]
        public virtual Port Del { get; set; }
        /// <summary>
        /// 交貨地(DEL) ETA
        /// </summary>
        public DateTime? DelEta { get; set; }
        /// <summary>
        /// 最終目的地ID
        /// </summary>
        public Guid?  FdestId { get; set; }
        /// <summary>
        /// 最終目的地
        /// </summary>
        [ForeignKey("FdestId")]
        public virtual Port Fdest { get; set; }
        /// <summary>
        /// 最終目的地ETA
        /// </summary>
        public DateTime? FdestEta { get; set; }
        /// <summary>
        /// 提空櫃地點ID
        /// </summary>
        public Guid?  EmptyPickupId { get; set; }
        /// <summary>
        /// 提空櫃地點
        /// </summary>
        [ForeignKey("EmptyPickupId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner EmptyPickup { get; set; }
        /// <summary>
        /// 提空櫃地點報表上顯示的資訊
        /// </summary>
        public string EmptyPickupContent { get; set; }
        /// <summary>
        /// 卡車交貨地ID
        /// </summary>
        public Guid?  DeliveryToId { get; set; }
        /// <summary>
        /// 卡車交貨地
        /// </summary>
        [ForeignKey("DeliveryToId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner DeliveryTo { get; set; }
        /// <summary>
        /// 卡車交貨地報表上顯示的資訊
        /// </summary>
        public string DeliveryToContent { get; set; }
        /// <summary>
        /// 前期運輸由(名稱)ID
        /// </summary>
        public Guid?  PreCarriageVesselNameId { get; set; }
        /// <summary>
        /// 前期運輸由(名稱)
        /// </summary>
        [ForeignKey("PreCarriageVesselNameId")]
        public virtual SysCode PreCarriageVesselName { get; set; }
        /// <summary>
        /// 前期運輸由(航程)
        /// </summary>
        public string PrepreCarriageVoyage { get; set; }
        /// <summary>
        /// 運費ID
        /// </summary>
        public Guid?  FreightTermId { get; set; }
        /// <summary>
        /// 運費
        /// </summary>
        [ForeignKey("FreightTermId")]
        public virtual SysCode FreightTerm { get; set; }
        /// <summary>
        /// 運輸模式ID
        /// </summary>
        public Guid?  ShipModeId { get; set; }
        /// <summary>
        /// 運輸模式
        /// </summary>
        [ForeignKey("ShipModeId")]
        public virtual SysCode ShipMode { get; set; }
        /// <summary>
        /// 運輸條款FronID
        /// </summary>
        public Guid?  SvcTermFromId { get; set; }
        /// <summary>
        /// 運輸條款Fron
        /// </summary>
        [ForeignKey("SvcTermFromId")]
        public virtual SysCode SvcTermFrom { get; set; }
        /// <summary>
        /// 運輸條款ToID
        /// </summary>
        public Guid?  SvcTermToId { get; set; }
        /// <summary>
        /// 運輸條款To
        /// </summary>
        [ForeignKey("SvcTermToId")]
        public virtual SysCode SvcTermTo { get; set; }
        /// <summary>
        /// 櫃型數量
        /// </summary>
        public string DisplayMblContainerSizeQtyInfo { get; set; }
        /// <summary>
        /// OB/L類別ID
        /// </summary>
        public Guid?  OblTypeId { get; set; }
        /// <summary>
        /// OB/L類別
        /// </summary>
        [ForeignKey("OblTypeId")]
        public virtual SysCode OblType { get; set; }
        /// <summary>
        /// 文件結關日
        /// </summary>
        public DateTime? DocCutOffTime { get; set; }
        /// <summary>
        /// 港口結關日
        /// </summary>
        public DateTime? PortCutOffTime { get; set; }
        /// <summary>
        /// VGM結關日期
        /// </summary>
        public DateTime? VgmCutOffTime { get; set; }
        /// <summary>
        /// 鐵路結關日
        /// </summary>
        public DateTime? RailCutOffTime { get; set; }
        /// <summary>
        /// 提單已取消
        /// </summary>
        public bool IsCanceled { get; set; }
        /// <summary>
        /// 取消日期
        /// </summary>
        public DateTime? CanceledDate { get; set; }
        /// <summary>
        /// 取消者ID
        /// </summary>
        public Guid?  CancelById { get; set; }
        /// <summary>
        /// 取消者
        /// </summary>
        [ForeignKey("CancelById")]
        public virtual UserData CancelBy { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason { get; set; }
        /// <summary>
        /// 業務推廣人ID
        /// </summary>
        public Guid?  MblReferralById { get; set; }
        /// <summary>
        /// 業務推廣人
        /// </summary>
        [ForeignKey("MblReferralById")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblReferralBy { get; set; }
        /// <summary>
        /// 業務推廣人報表上顯示的資訊
        /// </summary>
        public string MblReferralByContent { get; set; }
        /// <summary>
        /// 攬貨代理
        /// </summary>
        public bool IsBookingAgent { get; set; }
        /// <summary>
        /// 已放貨
        /// </summary>
        public bool IsReleased { get; set; }
        /// <summary>
        /// 放貨日期
        /// </summary>
        public DateTime? MblReleaseDate { get; set; }
        /// <summary>
        /// 放貨人ID
        /// </summary>
        public Guid?  ReleaseById { get; set; }
        /// <summary>
        /// 放貨人
        /// </summary>
        [ForeignKey("ReleaseById")]
        public virtual UserData ReleaseBy { get; set; }
        /// <summary>
        /// 裝船日期
        /// </summary>
        public DateTime? OnBoardDate { get; set; }
        /// <summary>
        /// 子提單號碼
        /// </summary>
        public string SubBlNo { get; set; }
        /// <summary>
        /// 服務合同編號
        /// </summary>
        public string ServiceContactNo { get; set; }
        /// <summary>
        /// 同行參考編號
        /// </summary>
        public string ForwardRefNo { get; set; }
        /// <summary>
        /// 中轉港 ID
        /// </summary>
        public Guid?  TransPort1Id { get; set; }
        /// <summary>
        /// 中轉港 
        /// </summary>
        [ForeignKey("TransPort1Id")]
        public virtual Port TransPort1 { get; set; }
        /// <summary>
        /// 中轉港ETA
        /// </summary>
        public DateTime? Trans1Eta { get; set; }
        /// <summary>
        /// ECTN號碼 
        /// </summary>
        public string EctNo { get; set; }
        /// <summary>
        /// PRN號碼
        /// </summary>
        public string PrnNo { get; set; }
        /// <summary>
        /// 是否電子商務
        /// </summary>
        public bool IsEcommerce { get; set; }
        /// <summary>
        /// 顏色標記ID
        /// </summary>
        public Guid?  ColorRemarkId { get; set; }
        /// <summary>
        /// 顏色標記
        /// </summary>
        [ForeignKey("ColorRemarkId")]

        public virtual SysCode ColorRemark { get; set; }
        /// <summary>
        /// 嘜頭
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 貨描
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 國內行程 / 出口指示
        /// </summary>
        public string DomesticInstructions { get; set; }
        /// <summary>
        /// 集裝箱的包裝種類 ID
        /// </summary>
        public Guid?  PackageCategoryId { get; set; }
        /// <summary>
        /// 集裝箱的包裝種類
        /// </summary>
        [ForeignKey("PackageCategoryId")]
        public virtual SysCode PackageCategory { get; set; }
        /// <summary>
        /// 集裝箱的包裝重量單位 ID
        /// </summary>
        public Guid?  PackageWeightId { get; set; }
        /// <summary>
        /// 集裝箱的包裝重量單位
        /// </summary>
        [ForeignKey("PackageWeightId")]
        public virtual SysCode PackageWeight { get; set; }
        /// <summary>
        /// 集裝箱的包裝材積 ID
        /// </summary>
        public Guid?  PackageMeasureId { get; set; }
        /// <summary>
        /// 集裝箱的包裝材積
        /// </summary>
        [ForeignKey("PackageMeasureId")]
        public virtual SysCode PackageMeasure { get; set; }
        /// <summary>
        /// 輸入總數 true用輸入的，False用加總的
        /// </summary>
        public bool TotalAmountType { get; set; }
        /// <summary>
        /// 包裹數量
        /// </summary>
        public int TotalPackage { get; set; }
        /// <summary>
        /// 總重
        /// </summary>
        public double TotalWeight { get; set; }
        /// <summary>
        /// 總材積
        /// </summary>
        public double TotalMeasure { get; set; }

        /// <summary>
        /// 是否鎖定
        /// </summary>
        public bool IsLocked { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }


        /// <summary>
        /// 代理參考號碼
        /// </summary>
        public string AgentRefNo { get; set; }
        /// <summary>
        /// CY地點ID
        /// </summary>
        public Guid? CyLocationId { get; set; }
        /// <summary>
        /// CY地點
        /// </summary>
        [ForeignKey("CyLocationId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CyLocation { get; set; }
        /// <summary>
        /// CFS地點ID
        /// </summary>
        public Guid? CfsLocationId { get; set; }
        /// <summary>
        /// CFS地點
        /// </summary>
        [ForeignKey("CfsLocationId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CfsLocation { get; set; }
        /// <summary>
        /// ETB
        /// </summary>
        public DateTime? Etb { get; set; }
        public DateTime? ATA { get; set; }
        public DateTime? ATD { get; set; }
        /// <summary>
        /// OB/L已接收
        /// </summary>
        public bool IsOblReceived { get; set; }
        /// <summary>
        /// OB/L已接收日期
        /// </summary>
        public DateTime? OblReceivedDate { get; set; }
        /// <summary>
        /// 業務推薦人ID
        /// </summary>
        public Guid? BusinessReferrerId { get; set; }
        /// <summary>
        /// 業務推薦人
        /// </summary>
        [ForeignKey("BusinessReferrerId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner BusinessReferrer { get; set; }
        /// <summary>
        /// 最遲貨櫃進場日期
        /// </summary>
        public DateTime? LatestContainerEntryDate { get; set; }
        /// <summary>
        /// IT號碼
        /// </summary>
        public string ItNo { get; set; }
        /// <summary>
        /// IT日期
        /// </summary>
        public DateTime? ItDate { get; set; }
        /// <summary>
        /// IT發布地點
        /// </summary>
        public string ItLocation { get; set; }
        /// <summary>
        /// 返還地點ID
        /// </summary>
        public Guid? ReturnLocationId { get; set; }
        /// <summary>
        /// 返還地點
        /// </summary>
        [ForeignKey("ReturnLocationId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner ReturnLocation { get; set; }

        /// <summary>
        /// 託運人ID
        /// </summary>
        public Guid? MblShipperId { get; set; }
        /// <summary>
        /// 託運人
        /// </summary>
        [ForeignKey("MblShipperId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblShipper { get; set; }
    }
}
