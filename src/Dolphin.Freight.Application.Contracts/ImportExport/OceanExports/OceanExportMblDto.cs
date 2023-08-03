using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class OceanExportMblDto : AuditedEntityDto<Guid>
    {
        public Guid Mid { get; set; }
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
        public Guid? OfficeId { get; set; }
        public string OfficeName { get; set; }
        /// <summary>
        /// 提單類別ID
        /// </summary>
        public Guid? BlTypeId { get; set; }

        /// <summary>
        /// 發布日期
        /// </summary>
        public DateTime? PostDate { get; set; }
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
        public Guid? MblCarrierId { get; set; }
        public string MblCarrierName { get; set; }
        /// <summary>
        /// 船公司(財務窗口)ID
        /// </summary>
        public Guid? BlAcctCarrierId { get; set; }
        /// <summary>
        /// 本地代理ID
        /// </summary>
        public Guid? ShippingAgentId { get; set; }
        /// <summary>
        /// 海外代理收貨人ID
        /// </summary>
        public Guid? MblOverseaAgentId { get; set; }
        /// <summary>
        /// 通知方ID
        /// </summary>
        public Guid? MblNotifyId { get; set; }
        /// <summary>
        /// 轉運代理ID
        /// </summary>
        public Guid? ForwardingAgentId { get; set; }
        /// <summary>
        /// 共同裝運人ID
        /// </summary>
        public Guid? CoLoaderId { get; set; }
        /// <summary>
        /// 操作員ID
        /// </summary>
        public Guid? MblOperatorId { get; set; }
        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("MblOperatorId")]
        public UserData MblOperator { get; set; }
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
        public Guid? MblCustomerId { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        public Guid? MblBillToId { get; set; }
        /// <summary>
        /// 收貨人ID
        /// </summary>
        public Guid? MblConsigneeId { get; set; }
        /// <summary>
        /// 業務類別ID
        /// </summary>
        public Guid? MblSalesTypeId { get; set; }
        /// <summary>
        /// 訂倉方式
        /// </summary>
        public string BookingMode { get; set; }
        /// <summary>
        /// 貨物類別ID
        /// </summary>
        public Guid? CargoTypeId { get; set; }
        /// <summary>
        /// 業務員ID
        /// </summary>
        public Guid? MblSaleId { get; set; }
        /// <summary>
        /// 業務員
        /// </summary>
        [ForeignKey("MblSaleId")]
        public UserData MblSale { get; set; }
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
        public string PorName { get; set; }
        /// <summary>
        /// 收貨地(POR) ETD
        /// </summary>
        public DateTime? PorEtd { get; set; }
        /// <summary>
        /// 裝貨港(POL)ID
        /// </summary>
        public Guid? PolId { get; set; }
        public string PolName { get; set; }
        /// <summary>
        /// 裝貨港(POL) ETD
        /// </summary>
        public DateTime? PolEtd { get; set; }
        /// <summary>
        /// 卸貨港(POD)ID
        /// </summary>
        public Guid? PodId { get; set; }
        public string PodName { get; set; }
        /// <summary>
        /// 卸貨港(POD) ETA
        /// </summary>
        public DateTime? PodEta { get; set; }
        /// <summary>
        /// 交貨地(DEL)ID
        /// </summary>
        public Guid? DelId { get; set; }
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
        /// 最終目的地ETA
        /// </summary>
        public DateTime? FdestEta { get; set; }
        /// <summary>
        /// 提空櫃地點ID
        /// </summary>
        public Guid? EmptyPickupId { get; set; }
        /// <summary>
        /// 卡車交貨地ID
        /// </summary>
        public Guid? DeliveryToId { get; set; }
        /// <summary>
        /// 前期運輸由(名稱)ID
        /// </summary>
        public Guid? PreCarriageVesselNameId { get; set; }
        /// <summary>
        /// 前期運輸由(航程)
        /// </summary>
        public string PrepreCarriageVoyage { get; set; }
        /// <summary>
        /// 運費ID
        /// </summary>
        public Guid? FreightTermId { get; set; }
        /// <summary>
        /// 運輸模式ID
        /// </summary>
        public Guid? ShipModeId { get; set; }
        public string ShipModeName { get; set; }
        /// <summary>
        /// 運輸條款FronID
        /// </summary>
        public Guid? SvcTermFromId { get; set; }

        /// <summary>
        /// 運輸條款ToID
        /// </summary>
        public Guid? SvcTermToId { get; set; }
        /// <summary>
        /// 櫃型數量
        /// </summary>
        public string DisplayMblContainerSizeQtyInfo { get; set; }
        /// <summary>
        /// OB/L類別ID
        /// </summary>
        public Guid? OblTypeId { get; set; }

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
        public UserData CancelBy { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason { get; set; }
        /// <summary>
        /// 業務推廣人ID
        /// </summary>
        public Guid? MblReferralById { get; set; }
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
        public UserData ReleaseBy { get; set; }
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
        /// 是否鎖定
        /// </summary>
        public bool IsLocked { get; set; }
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
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public string ATD { get; set; }
        public string ATA { get; set; }
        public string VesselScheduleNo { get; set; }
        public string ContainerNo { get; set; }
        public string OverseaAgentName { get; set; }
        public string HBLNo { get; set; }
        public string CT { get; set; }
        public int ContainerQty  { get; set; }
        public string ARBalance { get; set; }
        public string APBalance { get; set; }
        public string DCBalance { get; set; }
        public int? Package { get; set; }
        public string PKGUnit { get; set; }
        public string Weight { get; set; }
        public string Measurement { get; set; }
        public string Sales { get; set; }
        public string BlAcctCarrierName { get; set; }
        public string ShippingAgentName { get; set; }
        public string ReleaseByName { get; set; }
        public string OblTypeName { get; set; }
        public string ReferralByName { get; set; }
        public string OperatorName { get; set; }
        public string NotifyName { get; set; }
        public string CustomerName { get; set; }
        public string ConsigneeName { get; set; }
        public string BillToName { get; set; }
        public string FreightTermName { get; set; }
        public string ForwardingAgentName { get; set; }
        public string FdestName { get; set; }
        public string EmptyPickupName { get; set; }
        public string DeliveryToName { get; set; }
        public string CargoTypeName { get; set; }
        public string CoLoaderName { get; set; }
    }
}
