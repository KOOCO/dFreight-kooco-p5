﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Users;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Common;
using Dolphin.Freight.Accounting.Invoices;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class OceanExportDetails
    {
        public Guid MblId { get; set; }
        public string ShipmentNo { get; set; }
        public string FilingNo { get; set; }
        public string MblNo { get; set; }
        public Guid? OfficeId { get; set; }
        public virtual SubstationDto Office { get; set; }
        public Guid? BlTypeId { get; set; }

        [ForeignKey("BlTypeId")]
        public virtual SysCodeDto BlType { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

        public string SoNo { get; set; }
        public string ItnNo { get; set; }
        public string AmsNo { get; set; }
        public Guid? MblCarrierId { get; set; }

        [ForeignKey("MblCarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto MblCarrier { get; set; }
        public string MblCarrierName { get { if (MblCarrier != null) { return string.Concat(MblCarrier?.TPName, "/", MblCarrier?.TPCode); } else return string.Empty; } }

        public string MblCarrierContent { get; set; }
        public Guid? BlAcctCarrierId { get; set; }
        [ForeignKey("BlAcctCarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto BlAcctCarrier { get; set; }
        public string BlAcctCarrierName { get { return string.Concat(BlAcctCarrier?.TPName, "/", BlAcctCarrier?.TPCode); } }
        public string BlAcctCarrierContent { get; set; }
        public Guid? ShippingAgentId { get; set; }

        [ForeignKey("ShippingAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto ShippingAgent { get; set; }
        public string ShippingAgentName { get { return string.Concat(ShippingAgent?.TPName, "/", ShippingAgent?.TPCode); } }

        public string ShippingAgentContent { get; set; }
        public Guid? MblOverseaAgentId { get; set; }
    
        [ForeignKey("MblOverseaAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto MblOverseaAgent { get; set; }
        public string MblOverseaAgentName { get { return string.Concat(MblOverseaAgent?.TPName, "/", MblOverseaAgent?.TPCode); } }

        public string MblOverseaAgentContent { get; set; }
        
        public Guid? MblNotifyId { get; set; }
       
        [ForeignKey("MblNotifyId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto MblNotify { get; set; }
        public string MblNotifyName { get { return string.Concat(MblNotify?.TPName, "/", MblNotify?.TPCode); } }

        public string MblNotifyContent { get; set; }
      
        public Guid? ForwardingAgentId { get; set; }
       
        [ForeignKey("ForwardingAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto ForwardingAgent { get; set; }
        public string ForwardingAgentName { get { return string.Concat(ForwardingAgent?.TPName, "/", ForwardingAgent?.TPCode); } }

        public string ForwardingAgentContent { get; set; }
       
        public Guid? CoLoaderId { get; set; }
        
        [ForeignKey("CoLoaderId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto CoLoader { get; set; }
        public string CoLoaderName { get { return string.Concat(CoLoader?.TPName, "/", CoLoader?.TPCode); } }

        public string CoLoaderContent { get; set; }
    
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
       
        [ForeignKey("MblCustomerId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto MblCustomer { get; set; }
        public string MblCustomerName { get { return string.Concat(MblCustomer?.TPName, "/", MblCustomer?.TPCode)?.TrimStart('/'); } }
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
        [ForeignKey("MblBillToId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto MblBillTo { get; set; }
        public string MblBillToName { get { return string.Concat(MblBillTo?.TPName, "/", MblBillTo?.TPCode)?.TrimStart('/'); } }
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
        [ForeignKey("MblConsigneeId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto MblConsignee { get; set; }
        public string MblConsigneeName { get { return string.Concat(MblConsignee?.TPName, "/", MblConsignee?.TPCode)?.TrimStart('/'); } }
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
        [ForeignKey("CargoTypeId")]
        public virtual SysCodeDto CargoType { get; set; }
        public string CargoTypeName { get { return CargoType?.ShowName; } }
        /// <summary>
        /// 業務員ID
        /// </summary>
        public Guid? MblSaleId { get; set; }
        /// <summary>
        /// 業務員
        /// </summary>
        [ForeignKey("MblSaleId")]
        public virtual UserData MblSale { get; set; }
        public string MblSaleName { get { return string.Concat(MblSale?.Name, "/", MblSale?.Surname)?.TrimStart('/'); } }
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
        /// <summary>
        /// 收貨地(POR)
        /// </summary>
        [ForeignKey("PorId")]
        public virtual PortsManagementDTO Por { get; set; }
        public string PorName { get { return Por?.PortName; } }
        /// <summary>
        /// 收貨地(POR) ETD
        /// </summary>
        public DateTime? PorEtd { get; set; }
        /// <summary>
        /// 裝貨港(POL)ID
        /// </summary>
        public Guid? PolId { get; set; }
        /// <summary>
        /// 裝貨港(POL)
        /// </summary>
        [ForeignKey("PolId")]
        public virtual PortsManagementDTO Pol { get; set; }
        public string PolName { get { return Pol?.PortName; } }
        /// <summary>
        /// 裝貨港(POL) ETD
        /// </summary>
        public DateTime? PolEtd { get; set; }
        /// <summary>
        /// 卸貨港(POD)ID
        /// </summary>
        public Guid? PodId { get; set; }
        /// <summary>
        /// 卸貨港(POD)
        /// </summary>
        [ForeignKey("PodId")]
        public virtual PortsManagementDTO Pod { get; set; }
        public string PodName { get { return Pod?.PortName; } }
        /// <summary>
        /// 卸貨港(POD) ETA
        /// </summary>
        public DateTime? PodEta { get; set; }
        /// <summary>
        /// 交貨地(DEL)ID
        /// </summary>
        public Guid? DelId { get; set; }
        /// <summary>
        /// 交貨地(DEL)
        /// </summary>
        [ForeignKey("DelId")]
        public virtual PortsManagementDTO Del { get; set; }
        public string DelName { get { return Del?.PortName; } }
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
        [ForeignKey("FdestId")]
        public virtual PortsManagementDTO Fdest { get; set; }
        public string FdestName { get { return Fdest?.PortName; } }
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
        [ForeignKey("EmptyPickupId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto EmptyPickup { get; set; }
        public string EmptyPickupName { get { return string.Concat(EmptyPickup?.TPName, "/", EmptyPickup?.TPCode)?.TrimStart('/'); } }
        /// <summary>
        /// 提空櫃地點報表上顯示的資訊
        /// </summary>
        public string EmptyPickupContent { get; set; }
        /// <summary>
        /// 卡車交貨地ID
        /// </summary>
        public Guid? DeliveryToId { get; set; }
        /// <summary>
        /// 卡車交貨地
        /// </summary>
        [ForeignKey("DeliveryToId")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto DeliveryTo { get; set; }
        public string DeliveryToName { get { return string.Concat(DeliveryTo?.TPName, "/", DeliveryTo?.TPCode)?.TrimStart('/'); } }
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
        [ForeignKey("PreCarriageVesselNameId")]
        public virtual SysCodeDto PreCarriageVesselName { get; set; }
        public string PreCarriageVesselNameValue { get { return PreCarriageVesselName?.ShowName; } }
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
        [ForeignKey("FreightTermId")]
        public virtual SysCodeDto FreightTerm { get; set; }
        public string FreightTermName { get { return FreightTerm?.ShowName; } }
        /// <summary>
        /// 運輸模式ID
        /// </summary>
        public Guid? ShipModeId { get; set; }
        /// <summary>
        /// 運輸模式
        /// </summary>
        [ForeignKey("ShipModeId")]
        public virtual SysCodeDto ShipMode { get; set; }
        public string ShipModeName { get { return ShipMode?.ShowName; } }
        /// <summary>
        /// 運輸條款FronID
        /// </summary>
        public Guid? SvcTermFromId { get; set; }
        /// <summary>
        /// 運輸條款Fron
        /// </summary>
        [ForeignKey("SvcTermFromId")]
        public virtual SysCodeDto SvcTermFrom { get; set; }
        public string SvcTermFromName { get { return SvcTermFrom?.ShowName; } }
        /// <summary>
        /// 運輸條款ToID
        /// </summary>
        public Guid? SvcTermToId { get; set; }
        /// <summary>
        /// 運輸條款To
        /// </summary>
        [ForeignKey("SvcTermToId")]
        public virtual SysCodeDto SvcTermTo { get; set; }
        public string SvcTermToName { get { return SvcTermTo?.ShowName; } }
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
        [ForeignKey("OblTypeId")]
        public virtual SysCodeDto OblType { get; set; }
        public string OblTypeName { get { return OblType?.ShowName; } }
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
        [ForeignKey("MblReferralById")]
        public virtual Dolphin.Freight.TradePartners.TradePartnerDto MblReferralBy { get; set; }
        public string MblReferralByName { get { return string.Concat(MblReferralBy?.TPName, "/", MblReferralBy?.TPCode); } }
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
        public Guid? TransPort1Id { get; set; }
        /// <summary>
        /// 中轉港 
        /// </summary>
        [ForeignKey("TransPort1Id")]
        public virtual PortsManagementDTO TransPort1 { get; set; }
        public string TransPort1Name { get { return TransPort1?.PortName; } }
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
        [ForeignKey("PackageCategoryId")]
        public virtual SysCodeDto PackageCategory { get; set; }
        public string PackageCategoryName { get { return PackageCategory?.ShowName; } }
        /// <summary>
        /// 集裝箱的包裝重量單位 ID
        /// </summary>
        public Guid? PackageWeightId { get; set; }
        /// <summary>
        /// 集裝箱的包裝重量單位
        /// </summary>
        [ForeignKey("PackageWeightId")]
        public virtual SysCodeDto PackageWeight { get; set; }
        public string PackageWeightName { get { return PackageWeight?.ShowName; } }
        /// <summary>
        /// 集裝箱的包裝材積 ID
        /// </summary>
        public Guid? PackageMeasureId { get; set; }
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
        public bool IsCustomerRef { get; set; }
    }
}