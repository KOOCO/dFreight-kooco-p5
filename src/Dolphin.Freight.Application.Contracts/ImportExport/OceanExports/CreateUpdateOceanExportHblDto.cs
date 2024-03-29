﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class CreateUpdateOceanExportHblDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// HB/L號碼
        /// </summary>
        
        public Guid? MblId { get; set; }
        public Guid? ContainerId { get; set; }
        /// <summary>
        /// HB/L號碼
        /// </summary>
        [Required]
        public string HblNo { get; set; }

        public string ItnNo { get; set; }
        /// <summary>
        /// S/O號碼
        /// </summary>

        public string SoNo { get; set; }
        /// <summary>
        /// 客戶參考編號
        /// </summary>
        public string CustomerRefNo { get; set; }
        public string ContainerNo { get; set; }
        /// <summary>
        /// 文件號碼
        /// </summary>
        public string DocNo { get; set; }
        /// <summary>
        /// B2b號碼 (Ovly for Loyal Group)
        /// </summary>
        public string B2bNo { get; set; }
        /// <summary>
        /// 報價單號
        /// </summary>
        public string QuotationNo { get; set; }
        /// <summary>
        /// 實際託運人ID
        /// </summary>
        public Guid? HblShipperId { get; set; }
        /// <summary>
        /// 客戶ID
        /// </summary>
        public Guid? HblCustomerId { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        public Guid? HblBillToId { get; set; }
        /// <summary>
        /// 收貨人ID
        /// </summary>
        public Guid? HblConsigneeId { get; set; }
        /// <summary>
        /// 通知方ID
        /// </summary>
        public Guid? HblNotifyId { get; set; }
        /// <summary>
        /// 報關行ID
        /// </summary>
        public Guid? CustomsBrokerId { get; set; }
        /// <summary>
        /// 卡車ID
        /// </summary>
        public Guid? TruckerId { get; set; }
        /// <summary>
        /// HB/L代理ID
        /// </summary>
        public Guid? AgentId { get; set; }
        /// <summary>
        /// 業務員ID
        /// </summary>
        public Guid? HblSaleId { get; set; }
        /// <summary>
        /// 業務員
        /// </summary>
        [ForeignKey("HblSaleId")]
        public virtual UserData HblSale { get; set; }
        /// <summary>
        /// 轉運代理ID
        /// </summary>
        public Guid? ForwardingAgentId { get; set; }
        /// <summary>
        /// 操作員ID
        /// </summary>
        public Guid? HblOperatorId { get; set; }
        /// <summary>
        /// 是否子代理提單
        /// </summary>
        public bool IsSubAgentBl { get; set; }
        /// <summary>
        /// 收穫方代理ID
        /// </summary>
        public Guid? ReceivingAgentId { get; set; }
        /// <summary>
        /// 收貨地(POR)ID
        /// </summary>
        public Guid? PolId { get; set; }
        /// <summary>
        /// 收貨地(POR) ETD
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? PolEtd { get; set; }
        public Guid? PorId { get; set; }
        /// <summary>
        /// 收貨地(POR) ETD
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? PorEtd { get; set; }
        /// <summary>
        /// 卸貨港(POD)ID
        /// </summary>
        public Guid? PodId { get; set; }
        /// <summary>
        /// 卸貨港(POD) ETA
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? PodEta { get; set; }
        /// <summary>
        /// 交貨地(DEL)ID
        /// </summary>
        public Guid? DelId { get; set; }
        /// <summary>
        /// 交貨地(DEL) ETA
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DelEta { get; set; }
        /// <summary>
        /// 最終目的地ID
        /// </summary>
        public Guid? FdestId { get; set; }
        /// <summary>
        /// 最終目的地ETA
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? FdestEta { get; set; }
        /// <summary>
        /// FBA倉庫ID
        /// </summary>
        public Guid? FbaFcId { get; set; }

        /// <summary>
        /// 提空櫃地點ID
        /// </summary>
        public Guid? EmptyPickupId { get; set; }
        /// <summary>
        /// 卡車交貨地ID
        /// </summary>
        public Guid? DeliveryToId { get; set; }
        /// <summary>
        /// 貨物就緒日期
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CargoArrivalDate { get; set; }
        /// <summary>
        /// 卡車收貨地ID
        /// </summary>
        public Guid? CargoPickupId { get; set; }
        /// <summary>
        /// 運輸模式ID
        /// </summary>
        public Guid? ShipModeId { get; set; }
        /// <summary>
        /// 運費買方ID
        /// </summary>
        public Guid? FreightTermForBuyerId { get; set; }
        /// <summary>
        /// 運費賣方ID
        /// </summary>
        public Guid? FreightTermForSalerId { get; set; }
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
        public string DisplayHblContainerSizeQtyInfo { get; set; }
        /// <summary>
        /// 是否快捷提單
        /// </summary>
        public bool IsExpress { get; set; }
        /// <summary>
        /// 貨物類別ID
        /// </summary>
        public Guid? CargoTypeId { get; set; }
        /// <summary>
        /// 業務類別ID
        /// </summary>
        public Guid? MblSalesTypeId { get; set; }
        /// <summary>
        /// 倉儲結關日
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? HblWhCutOffTime { get; set; }
        /// <summary>
        /// 最早收櫃日
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? EarlyReturnDateTime { get; set; }
        /// <summary>
        /// 信用狀編號
        /// </summary>
        public string LcNo { get; set; }
        /// <summary>
        /// 信用狀開立銀行
        /// </summary>
        public string LcIssueBank { get; set; }
        /// <summary>
        /// 信用狀開立日期
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? LcIssueDate { get; set; }
        /// <summary>
        /// 裝船日期
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? OnBoardDate { get; set; }
        /// <summary>
        /// 是否可堆積
        /// </summary>
        public bool IsStackable { get; set; }
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
        /// 已放貨
        /// </summary>
        public bool IsReleased { get; set; }
        /// <summary>
        /// 放貨日期
        /// </summary>
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? HblReleaseDate { get; set; }
        /// <summary>
        /// 放貨人ID
        /// </summary>
        public Guid? ReleaseById { get; set; }
        /// <summary>
        /// 提單已取消
        /// </summary>
        public bool IsCanceled { get; set; }
        /// <summary>
        /// 取消日期
        /// </summary>
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CanceledDate { get; set; }
        /// <summary>
        /// 取消者ID
        /// </summary>
        public Guid? CancelById { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason { get; set; }
        /// <summary>
        /// 業務推廣人ID
        /// </summary>
        public Guid? MblReferralById { get; set; }
        /// <summary>
        /// W/O號碼
        /// </summary>
        public string WoNo { get; set; }
        /// <summary>
        /// 運輸類別ID
        /// </summary>
        public Guid? ShipTypeId { get; set; }
        /// <summary>
        /// 國貿條規ID
        /// </summary>
        public Guid? IncotermsId { get; set; }
        /// <summary>
        /// NRA號碼
        /// </summary>
        public string NraNo { get; set; }
        /// <summary>
        /// 是否電子商務
        /// </summary>
        public bool IsEcommerce { get; set; }
        /// <summary>
        /// 目的地CY/CFS地點ID
        /// </summary>
        public Guid? CyCfsLocationId { get; set; }
        /// <summary>
        /// 是否目的地鐵路
        /// </summary>
        public bool IsRailwayCode { get; set; }
        /// <summary>
        /// 目的地鐵路ID
        /// </summary>
        public Guid? RailwayCodeId { get; set; }
        /// <summary>
        /// 預計最終交付時間
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DoorDeliveryETA { get; set; }
        /// <summary>
        /// 實際最終交付時間
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DoorDeliveryATA { get; set; }
        /// <summary>
        /// 出口報關時間
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CustomClearance { get; set; }
        /// <summary>
        /// 進口清關時間
        /// </summary>
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CustomDeclaration { get; set; }
        /// <summary>
        /// 卡片顏色標記ID
        /// </summary>
        public Guid? CardColorId { get; set; }
        public string CardColorValue { get; set; }
        /// <summary>
        /// 顏色標記ID
        /// </summary>
        public Guid? ColorRemarkId { get; set; }
        /// <summary>
        /// 嘜頭
        /// </summary>
        public string Mark { get; set; }
        public int? PackageNo { get; set; }
        public double? PackageWeight { get; set; }
        public double? PackageMeasurement { get; set; }
        /// <summary>
        /// 貨描
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 國內行程 / 出口指示
        /// </summary>
        public string DomesticInstructions { get; set; }
        /// <summary>
        /// S/O備註
        /// </summary>
        public string SoNote { get; set; }
        /// <summary>
        /// AR結餘
        /// </summary>
        public double ARSurplus { get; set; }
        /// <summary>
        /// AP結餘
        /// </summary>
        public double APSurplus { get; set; }
        /// <summary>
        /// DC結餘
        /// </summary>
        public double DCSurplus { get; set; }
        /// <summary>
        /// 0：集裝箱總數，1：手動輸入總數
        /// </summary>
        public int SurplusType { get; set; }
        public string PoNo { get; set; }
        /// <summary>
        /// 是否鎖定
        /// </summary>
        public bool IsLocked { get; set; }
        public bool IsCreateBySystem { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
    }
}
