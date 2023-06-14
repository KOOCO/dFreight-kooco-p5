using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Users;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.OceanImports
{
    public class CreateUpdateOceanImportMblDto : AuditedEntityDto<Guid>
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
        public Guid OfficeId { get; set; }
        /// <summary>
        /// 提單類別ID
        /// </summary>
        public Guid? BlTypeId { get; set; }
        /// <summary>
        /// 發布日期
        /// </summary>
        [DataType(DataType.Date)]
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
        public string MblCarrierContent { get; set; } = " ";
        /// <summary>
        /// 船公司(財務窗口)ID
        /// </summary>
        public Guid? BlAcctCarrierId { get; set; }
        /// <summary>
        /// 船公司(財務窗口)報表上顯示的資訊
        /// </summary>
        public string BlAcctCarrierContent { get; set; }
        /// <summary>
        /// 本地代理ID
        /// </summary>
        public Guid? ShippingAgentId { get; set; }
        /// <summary>
        /// 本地代理報表上顯示的資訊
        /// </summary>
        public string ShippingAgentContent { get; set; }
        /// <summary>
        /// 海外代理收貨人ID
        /// </summary>
        public Guid? MblOverseaAgentId { get; set; }
        public string MblOverseaAgentName { get; set; }
        /// <summary>
        /// 海外代理收貨人報表上顯示的資訊
        /// </summary>
        public string MblOverseaAgentContent { get; set; }
        /// <summary>
        /// 通知方ID
        /// </summary>
        public Guid? MblNotifyId { get; set; }
        /// <summary>
        /// 通知方報表上顯示的資訊
        /// </summary>
        public string MblNotifyContent { get; set; }
        /// <summary>
        /// 轉運代理ID
        /// </summary>
        public Guid? ForwardingAgentId { get; set; }
        /// <summary>
        /// 轉運代理報表上顯示的資訊
        /// </summary>
        public string ForwardingAgentContent { get; set; }
        /// <summary>
        /// 共同裝運人ID
        /// </summary>
        public Guid? CoLoaderId { get; set; }
        /// <summary>
        /// 共同裝運人報表上顯示的資訊
        /// </summary>
        public string CoLoaderContent { get; set; }
        /// <summary>
        /// 是否使用CareOf
        /// </summary>
        public bool IsUsedCareOf { get; set; }
        /// <summary>
        /// C/O ID
        /// </summary>
        public Guid? CareOfId { get; set; }
        /// <summary>
        /// C/O報表上顯示的資訊
        /// </summary>
        public string CareOfContent { get; set; }
        /// <summary>
        /// 操作員ID
        /// </summary>
        public Guid? MblOperatorId { get; set; }
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
        public Guid? MblCustomerId { get; set; }
        /// <summary>
        /// 客戶報表上顯示的資訊
        /// </summary>
        public string MblCustomerContent { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        public Guid? MblBillToId { get; set; }
        /// <summary>
        /// 收票人報表上顯示的資訊
        /// </summary>
        public string MblBillToContent { get; set; }
        /// <summary>
        /// 收貨人ID
        /// </summary>
        public Guid? MblConsigneeId { get; set; }
        /// <summary>
        /// 收貨人報表上顯示的資訊
        /// </summary>
        public string MblConsigneeContent { get; set; }
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
        public Guid? PorId { get; set; }
        public string PorName { get; set; }
        /// <summary>
        /// 收貨地(POR) ETD
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? PorEtd { get; set; }
        /// <summary>
        /// 裝貨港(POL)ID
        /// </summary>
        public Guid? PolId { get; set; }
        /// <summary>
        /// 裝貨港(POL) ETD
        /// </summary>
        public string PolName { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? PolEtd { get; set; }
        /// <summary>
        /// 卸貨港(POD)ID
        /// </summary>
        public Guid? PodId { get; set; }
        public string PodName { get; set; }
        /// <summary>
        /// 卸貨港(POD) ETA
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? PodEta { get; set; }
        /// <summary>
        /// 交貨地(DEL)ID
        /// </summary>
        public Guid? DelId { get; set; }
        public string DelName { get; set; }
        /// <summary>
        /// 交貨地(DEL) ETA
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DelEta { get; set; }
        /// <summary>
        /// 最終目的地ID
        /// </summary>
        public Guid? FdestId { get; set; }
        public string FdestName { get; set; }
        /// <summary>
        /// 最終目的地ETA
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FdestEta { get; set; }
        /// <summary>
        /// 提空櫃地點ID
        /// </summary>
        public Guid? EmptyPickupId { get; set; }
        /// <summary>
        /// 提空櫃地點報表上顯示的資訊
        /// </summary>
        public string EmptyPickupContent { get; set; }
        /// <summary>
        /// 卡車交貨地ID
        /// </summary>
        public Guid? DeliveryToId { get; set; }
        /// <summary>
        /// 卡車交貨地報表上顯示的資訊
        /// </summary>
        public string DeliveryToContent { get; set; }
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? DocCutOffTime { get; set; }
        /// <summary>
        /// 港口結關日
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? PortCutOffTime { get; set; }
        /// <summary>
        /// VGM結關日期
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? VgmCutOffTime { get; set; }
        /// <summary>
        /// 鐵路結關日
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? RailCutOffTime { get; set; }
        /// <summary>
        /// 提單已取消
        /// </summary>
        public bool IsCanceled { get; set; }
        /// <summary>
        /// 取消日期
        /// </summary>
        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
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
        public string CancelByName { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason { get; set; }
        /// <summary>
        /// 業務推廣人ID
        /// </summary>
        public Guid? MblReferralById { get; set; }
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
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
        // [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? OnBoardDate { get; set; }
        public int GetHideCheck()
        {
            return (SubBlNo == null && ServiceContactNo == null && ForwardRefNo == null && TransPort1Id == null && Trans1Eta == null && EctNo == null && PrnNo == null && IsEcommerce) ? 1 : 0;
        }
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
        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
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
        /// 集裝箱的包裝重量單位 ID
        /// </summary>
        public Guid? PackageWeightId { get; set; }
        /// <summary>
        /// 集裝箱的包裝材積 ID
        /// </summary>
        public Guid? PackageMeasureId { get; set; }
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
        /// CFS地點ID
        /// </summary>
        public Guid? CfsLocationId { get; set; }
        /// <summary>
        /// ETB
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Etb { get; set; }
        ///// <summary>
        ///// OB/L已接收
        ///// </summary>
        public bool IsOblReceived { get; set; }
        /// <summary>
        /// OB/L已接收日期
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? OblReceivedDate { get; set; }
        /// <summary>
        /// 業務推薦人ID
        /// </summary>
        public Guid? BusinessReferrerId { get; set; }
        /// <summary>
        /// 最遲貨櫃進場日期
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? LatestContainerEntryDate { get; set; }
        /// <summary>
        /// IT號碼
        /// </summary>
        public string ItNo { get; set; }
        /// <summary>
        /// IT日期
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
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
        /// 託運人ID
        /// </summary>
        public Guid? MblShipperId { get; set; }
    }
}
