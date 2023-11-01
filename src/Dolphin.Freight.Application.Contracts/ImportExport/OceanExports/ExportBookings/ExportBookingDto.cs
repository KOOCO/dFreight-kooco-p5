using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using static Dolphin.Freight.Permissions.OceanExportPermissions;
using Volo.Abp.Users;
using Volo.Abp.Data;

namespace Dolphin.Freight.ImportExport.OceanExports.ExportBookings
{
    public class ExportBookingDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 對應的ID，可能是船期也可能是MBL
        /// </summary>
        public Guid? VesselScheduleId { get; set; }
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
        public DateTime? CargoReadyDate { get; set; }
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
        /// 業務員ID
        /// </summary>
        public Guid? SalespersonId { get; set; }
        /// <summary>
        /// 是否扣留
        /// </summary>
        public bool IsHold { get; set; }
        /// <summary>
        /// 扣留原因
        /// </summary>
        public string HoldReason { get; set; }
        /// <summary>
        /// 扣留人員ID
        /// </summary>
        public Guid? HolderId { get; set; }
        /// <summary>
        /// BL已取消
        /// </summary>
        public bool IsCanceled { get; set; }
        /// <summary>
        /// 取消日期
        /// </summary>
        public DateTime CanceledDate { get; set; }
        /// <summary>
        /// 取消者ID
        /// </summary>
        public Guid? CancelById { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason { get; set; }
        /// <summary>
        /// 已放貨
        /// </summary>
        public bool IsReleased { get; set; }
        /// <summary>
        /// 放貨日期
        /// </summary>
        public DateTime HblReleaseDate { get; set; }
        /// <summary>
        /// 放貨人ID
        /// </summary>
        public Guid? ReleaseById { get; set; }
        /// <summary>
        /// 船期編號 
        /// </summary>
        public string ReferenceNo { get; set; }
        /// <summary>
        /// 船公司S/O號碼
        /// </summary>
        public string CarrierBkgNo { get; set; }
        /// <summary>
        /// 船公司ID
        /// </summary>
        public Guid? CarrierId { get; set; }
        public string CarrierName { get; set; }
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
        public string SvcName { get; set; }
        /// <summary>
        /// 國貿條規ID
        /// </summary>
        public Guid? IncotermsId { get; set; }
        /// <summary>
        /// 實際託運人ID
        /// </summary>
        public Guid? ShipperId { get; set; }
        public string ShipperName { get; set; }
        /// <summary>
        /// 客戶ID
        /// </summary>
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        public Guid? BillToId { get; set; }
        /// <summary>
        /// 收貨方ID
        /// </summary>
        public Guid? ConsigneeId { get; set; }
        /// <summary>
        /// 通知方ID
        /// </summary>
        public Guid? NotifyId { get; set; }
        /// <summary>
        /// 本地代理ID
        /// </summary>
        public Guid? ShippingAgentId { get; set; }
        public string ShippingAgentName { get; set; }
        public string HblConsigneeName { get; set; }
        /// <summary>
        /// HBL代理ID
        /// </summary>
        public Guid? HblAgentId { get; set; }
        /// <summary>
        /// 轉運代理ID
        /// </summary>
        public Guid? ForwardingAgentId { get; set; }
        /// <summary>
        /// 共同裝運人ID
        /// </summary>
        public Guid? CoLoaderId { get; set; }
        /// <summary>
        /// 船名
        /// </summary>
        public string VesselName { get; set; }
        /// <summary>
        /// 航次
        /// </summary>
        public string Voyage { get; set; }
        /// <summary>
        /// 集裝箱 櫃型/尺寸
        /// </summary>
        public string ContainerQtyInputText { get; set; }
        /// <summary>
        /// 收貨地(POR)ID
        /// </summary>
        public Guid? PorId { get; set; }
        public string PorName { get; set; }
        /// <summary>
        /// 收貨地(POR) ETD
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime PorEtd { get; set; }
        /// <summary>
        /// 裝貨港(POL)ID
        /// </summary>
        public Guid? PolId { get; set; }
        public string PolName { get; set; }
        /// <summary>
        /// 裝貨港(POL) ETD
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime PolEtd { get; set; }
        /// <summary>
        /// 卸貨港(POD)ID
        /// </summary>
        public Guid? PodId { get; set; }
        public string PodName { get; set; }
        /// <summary>
        /// 卸貨港(POD) ETA
        /// </summary>
        [DataType(DataType.Date)]
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
        public DateTime? FdestEta { get; set; }
        /// <summary>
        /// FBA倉儲ID
        /// </summary>
        public Guid? FbaId { get; set; }
        public int PackageNum { get; set; }
        public string PackageWeight { get; set; }
        public string PackageMeasure { get; set; }
        /// <summary>
        /// 中轉港 ID
        /// </summary>
        public Guid? TransPort1Id { get; set; }
        /// <summary>
        /// 中轉港ETA
        /// </summary>
        public DateTime Trans1Eta { get; set; }
        /// <summary>
        /// ECTN號碼 
        /// </summary>
        public string EctNo { get; set; }
        /// <summary>
        /// PRN號碼
        /// </summary>
        public string PrnNo { get; set; }
        /// <summary>
        /// 貨物類別ID
        /// </summary>
        public Guid? CargoTypeId { get; set; }
        /// <summary>
        /// 業務推廣人ID
        /// </summary>
        public Guid? ReferralById { get; set; }
        /// <summary>
        /// 領交櫃代號
        /// </summary>
        public string ContainerPickupNo { get; set; }
        /// <summary>
        /// 卡車收貨地ID
        /// </summary>
        public Guid? CargoPickupId { get; set; }
        /// <summary>
        /// 卡車ID
        /// </summary>
        public Guid? TruckerId { get; set; }
        /// <summary>
        /// 卡車交貨地ID
        /// </summary>
        public Guid? DeliveryToId { get; set; }
        /// <summary>
        /// 貨物就緒日期
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? CargoArrivalDate { get; set; }
        /// <summary>
        /// 提空櫃地點ID
        /// </summary>
        public Guid? EmptyPickupId { get; set; }
        /// <summary>
        /// 倉儲結關日
        /// </summary>
        public DateTime? WhCutOffTime { get; set; }

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
        /// 最早收櫃日
        /// </summary>
        public DateTime? EarlyReturnDatetime { get; set; }
        /// <summary>
        /// 分站ID
        /// </summary>
        public Guid? OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string FbaName { get; set; }
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
        public string Description { get; set; }
        /// <summary>
        /// S/O備註
        /// </summary>
        public string BookingRemark { get; set; }
        /// <summary>
        /// 提單做法備註
        /// </summary>
        public string ShippingRemark { get; set; }
        /// <summary>
        /// 貨物艙單備註
        /// </summary>
        public string CargoRemark { get; set; }
        public string PoNo { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
    }
}