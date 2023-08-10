using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.OceanImports
{
    public class OceanImportDetails
    {

        public Guid MblId { get; set; }
        public string ShipmentNo { get; set; }
        public string FilingNo { get; set; }
        public string MblNo { get; set; }
        public string PoNo { get; set; }
        public string DocNo { get; set; }
        public string HblNo { get; set; }
        public string LCNo { get; set; }
        public string LCIssueBankName { get; set; }
        public string ETD { get; set; }
        public Guid? OfficeId { get; set; }
        public virtual SubstationDto Office { get; set; }
        public Guid? BlTypeId { get; set; }
        public string MblDel { get; set; }
        [ForeignKey("BlTypeId")]
        public virtual SysCodeDto BlType { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }
        public Guid? CyLocationId { get; set; }
        public string CyLocation { get; set; }
        public string SoNo { get; set; }
        public string HblSoNo { get; set; }
        public string ItnNo { get; set; }
        public string AmsNo { get; set; }
        public Guid? MblCarrierId { get; set; }
        public string MblCarrierName { get; set; }
        public string FbaFC { get; set; }
        public string MblCarrierContent { get; set; }
        public Guid? BlAcctCarrierId { get; set; }
        public string BlAcctCarrierName { get; set; }
        public string BlAcctCarrierContent { get; set; }
        public Guid? ShippingAgentId { get; set; }
        public string ShippingAgentName { get; set; }

        public string ShippingAgentContent { get; set; }
        public Guid? MblOverseaAgentId { get; set; }
        public string MblOverseaAgentName { get; set; }

        public string MblOverseaAgentContent { get; set; }
        public List<ManifestCommodity> Commodity { get; set; }
        public Guid? MblNotifyId { get; set; }
        public string MblNotifyName { get; set; }
        public string HblNotifyName { get; set; }

        public string MblNotifyContent { get; set; }

        public Guid? ForwardingAgentId { get; set; }
        public string ForwardingAgentName { get; set; }

        public string ForwardingAgentContent { get; set; }

        public Guid? CoLoaderId { get; set; }
        public string CoLoaderName { get; set; }

        public string CoLoaderContent { get; set; }
        public DateTime? EarlyReturnDateTime { get; set; }
        public bool IsUsedCareOf { get; set; }

        public Guid? CareOfId { get; set; }

        [ForeignKey("CareOfId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto CareOf { get; set; }
        public string CareOfName { get { if (CareOf != null) { return string.Concat(CareOf?.TPName, "/", CareOf?.TPCode); } else return string.Empty; } }

        public string CareOfContent { get; set; }

        public Guid? MblOperatorId { get; set; }

        [ForeignKey("MblOperatorId")]
        public virtual UserData MblOperator { get; set; }
        public string MblOperatorName { get; set; }

        public string CarrierContractNo { get; set; }

        public bool IsDirect { get; set; }

        public string CustomerRefNo { get; set; }

        public Guid? MblCustomerId { get; set; }
        public string MblCustomerName { get; set; }
        /// <summary>
        /// 客戶報表上顯示的資訊
        /// </summary>
        public string MblCustomerContent { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        public Guid? MblBillToId { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        public string MblBillToName { get; set; }
        /// <summary>
        /// 收票人報表上顯示的資訊
        /// </summary>
        public string MblBillToContent { get; set; }
        /// <summary>
        /// 收貨人ID
        /// </summary>
        public Guid? MblConsigneeId { get; set; }
        /// <summary>
        /// 收貨人
        /// </summary>
        public string MblConsigneeName { get; set; }
        public string HblConsigneeName { get; set; }
        /// <summary>
        /// 收貨人報表上顯示的資訊
        /// </summary>
        public string MblConsigneeContent { get; set; }
        /// <summary>
        /// 業務類別ID
        /// </summary>
        public Guid? MblSalesTypeId { get; set; }
        /// <summary>
        /// 業務類別
        /// </summary>
        [ForeignKey("MblSalesTypeId")]
        public virtual SysCodeDto MblSalesType { get; set; }
        public string MblSalesTypeName { get { return MblSalesType?.ShowName; } }
        /// <summary>
        /// 訂倉方式
        /// </summary>
        public string BookingMode { get; set; }
        /// <summary>
        /// 貨物類別ID
        /// </summary>
        public Guid? CargoTypeId { get; set; }
        /// <summary>
        /// 貨物類別
        /// </summary>
        public string CargoTypeName { get; set; }
        /// <summary>
        /// 業務員ID
        /// </summary>
        public Guid? MblSaleId { get; set; }
        /// <summary>
        /// 業務員
        /// </summary>
        public string MblSaleName { get; set; }
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
        public Guid? PorId { get; set; }
        public Guid? MPorId { get; set; }
        /// <summary>
        /// 收貨地(POR)
        /// </summary>
        public string PorName { get; set; }
        public string MPorName { get; set; }
        /// <summary>
        /// 收貨地(POR) ETD
        /// </summary>
        public DateTime? PorEtd { get; set; }
        public DateTime? MPorEtd { get; set; }
        /// <summary>
        /// 裝貨港(POL)ID
        /// </summary>
        public Guid? PolId { get; set; }
        /// <summary>
        /// 裝貨港(POL)
        /// </summary>
        public string PolName { get; set; }
        /// <summary>
        /// 裝貨港(POL) ETD
        /// </summary>
        public DateTime? PolEtd { get; set; }
        public string FlightNo { get; set; }
        /// <summary>
        /// 卸貨港(POD)ID
        /// </summary>
        public Guid? PodId { get; set; }
        public Guid? MPodId { get; set; }
        /// <summary>
        /// 卸貨港(POD)
        /// </summary>
        public string PodName { get; set; }
        public string MPodName { get; set; }
        /// <summary>
        /// 卸貨港(POD) ETA
        /// </summary>
        public DateTime? PodEta { get; set; }
        public DateTime? MPodEta { get; set; }
        /// <summary>
        /// 交貨地(DEL)ID
        /// </summary>
        public Guid? DelId { get; set; }
        /// <summary>
        /// 交貨地(DEL)
        /// </summary>
        public string DelName { get; set; }
        /// <summary>
        /// 交貨地(DEL) ETA
        /// </summary>
        public DateTime? DelEta { get; set; }
        /// <summary>
        /// 最終目的地ID
        /// </summary>
        public Guid? FdestId { get; set; }
        /// <summary>
        /// 最終目的地
        /// </summary>
        public string FdestName { get; set; }
        /// <summary>
        /// 最終目的地ETA
        /// </summary>
        public DateTime? FdestEta { get; set; }
        /// <summary>
        /// 提空櫃地點ID
        /// </summary>
        public Guid? EmptyPickupId { get; set; }
        /// <summary>
        /// 提空櫃地點
        /// </summary>
        public string EmptyPickupName { get; set; }
        public string EmptyPickupContent { get; set; }
        /// <summary>
        /// 卡車交貨地ID
        /// </summary>
        public Guid? DeliveryToId { get; set; }
        /// <summary>
        /// 卡車交貨地
        /// </summary>
        public string DeliveryToName { get; set; }
        /// <summary>
        /// 卡車交貨地報表上顯示的資訊
        /// </summary>
        public string DeliveryToContent { get; set; }
        /// <summary>
        /// 前期運輸由(名稱)ID
        /// </summary>
        public Guid? PreCarriageVesselNameId { get; set; }
        /// <summary>
        /// 前期運輸由(名稱)
        /// </summary>
        public string PreCarriageVesselNameValue { get; set; }
        /// <summary>
        /// 前期運輸由(航程)
        /// </summary>
        public string PrepreCarriageVoyage { get; set; }
        /// <summary>
        /// 運費ID
        /// </summary>
        public Guid? FreightTermId { get; set; }
        /// <summary>
        /// 運費
        /// </summary>
        public string FreightTermName { get; set; }
        /// <summary>
        /// 運輸模式ID
        /// </summary>
        public Guid? ShipModeId { get; set; }
        /// <summary>
        /// 運輸模式
        /// </summary>
        public string ShipModeName { get; set; }
        /// <summary>
        /// 運輸條款FronID
        /// </summary>
        public Guid? SvcTermFromId { get; set; }
        /// <summary>
        /// 運輸條款Fron
        /// </summary>
        public string SvcTermFromName { get; set; }
        /// <summary>
        /// 運輸條款ToID
        /// </summary>
        public Guid? SvcTermToId { get; set; }
        /// <summary>
        /// 運輸條款To
        /// </summary>
        public string SvcTermToName { get; set; }
        /// <summary>
        /// 櫃型數量
        /// </summary>
        public string DisplayMblContainerSizeQtyInfo { get; set; }
        /// <summary>
        /// OB/L類別ID
        /// </summary>
        public Guid? OblTypeId { get; set; }
        /// <summary>
        /// OB/L類別
        /// </summary>
        public string OblTypeName { get; set; }
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
        public Guid? CancelById { get; set; }
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
        public Guid? MblReferralById { get; set; }
        /// <summary>
        /// 業務推廣人
        /// </summary>
        public string MblReferralByName { get; set; }
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
        public Guid? ReleaseById { get; set; }
        /// <summary>
        /// 放貨人
        /// </summary>
        public string ReleaseBy { get; set; }
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
        public Guid? TransPort1Id { get; set; }
        /// <summary>
        /// 中轉港 
        /// </summary>
        public string TransPort1Name { get; set; }
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
        public Guid? ColorRemarkId { get; set; }
        /// <summary>
        /// 顏色標記
        /// </summary>
        [ForeignKey("ColorRemarkId")]

        public virtual SysCodeDto ColorRemark { get; set; }
        public string ColorRemarkName { get { return ColorRemark?.ShowName; } }
        /// <summary>
        /// 嘜頭
        /// </summary>
        public string Mark { get; set; }
        public string HblOperatorName { get; set; }
        public DateTime? CurrentDate { get; set; }
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
        public Guid? PackageCategoryId { get; set; }
        /// <summary>
        /// 集裝箱的包裝種類
        /// </summary>
        public string PackageCategoryName { get; set; }
        /// <summary>
        /// 集裝箱的包裝重量單位 ID
        /// </summary>
        public Guid? PackageWeightId { get; set; }
        /// <summary>
        /// 集裝箱的包裝重量單位
        /// </summary>
        public string PackageWeightName { get; set; }
        /// <summary>
        /// 集裝箱的包裝材積 ID
        /// </summary>
        public Guid? PackageMeasureId { get; set; }
        public string PackageMeasureName { get; set; }
        public Guid? TruckerId { get; set; }
        public string TruckerName { get; set; }
        /// <summary>
        /// 集裝箱的包裝材積
        /// </summary>
        [ForeignKey("PackageMeasureId")]
        public virtual SysCodeDto PackageMeasure { get; set; }
        /// <summary>
        /// 輸入總數 true用輸入的，False用加總的
        /// </summary>
        public bool TotalAmountType { get; set; }
        /// <summary>
        /// 包裹數量
        /// </summary>
        public int TotalPackage { get; set; }
        public string TotalPackageStr { get; set; }
        /// <summary>
        /// 總重
        /// </summary>
        public double TotalWeight { get; set; }
        public string TotalWeightStr { get; set; }
        public string TotalWeightStrLBS { get; set; }
        public string TotalGrossWeightStr { get; set; }
        public string TotalNetWeightStr { get; set; }
        public string TotalPCSStr { get; set; }
        public string TotalPackagesStr { get; set; }

        public string TotalAmountStr { get; set; }
        /// <summary>
        /// 總材積
        /// </summary>
        public double TotalMeasure { get; set; }
        public string TotalMeasureStr { get; set; }
        public string TotalMeasureStrLBS { get; set; }

        /// <summary>
        /// 是否鎖定
        /// </summary>
        public bool IsLocked { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public DateTime? LCIssueDate { get; set; }
        public FreightPageType PageType { get; set; }
        public List<InvoiceDto> Invoices { get; set; }
        public string InvoicesJson { get; set; }
        public bool IsPDF { get; set; }
        public List<InvoiceDto> AR { get; set; }
        public List<InvoiceDto> DC { get; set; }
        public List<InvoiceDto> AP { get; set; }
        public double ARTotal { get; set; }
        public double DCTotal { get; set; }
        public double APTotal { get; set; }
        public double Total { get; set; }
        public string TotalStr { get; set; }
        public bool IsCustomerRef { get; set; }
        public string CreateUpdateContainerDtosJson { get; set; }
        public List<CreateUpdateContainerDto> CreateUpdateContainerDtos { get; set; }
        public string HblAgentName { get; set; }
        public List<CreateUpdateContainerDto> list { get; set; }
        public string BillOfLanding { get; set; }
        public string DisplayUnit { get; set; }
        public string ContainerNo { get; set; }
        public string MblFdestName { get; set; }
        public string PackagpackageUnitName { get; set; }

        public TradePartnerDto CargoPickUp { get; set; }
        public string CreateUpdateContainerJson { get; set; }
        public string HblsJson { get; set; }
        public List<CreateUpdateContainerDto> CreateUpdateContainer { get; set; }
        public DateTime? MblPostDate { get; set; }
        public List<Hbl> Hbls { get; set; }
        public string BillOfLandingMaster { get; set; }
        public List<Commodity> Commodities { get; set; }
        public string CommoditiesJson { get; set; }
        public string Net_weight_unit { get; set; }
        public string Gross_weight_unit { get; set; }
        public DateTime? EmptyPickUpDate { get; set; }
        public List<ContainerList> ContainerList { get; set; }
        public string MblFillingNo { get; set; }
    }
    public class ContainerList
    {
        public string chk { get; set; }
        public string CONTAINER_NO { get; set; }
        public string TYPE { get; set; }
        public string SEAL_NO { get; set; }
        public string PACKAGE { get; set; }
        public string WEIGHT { get; set; }
        public string PICKUP_NO { get; set; }
        public string LFD { get; set; }
    }

}

