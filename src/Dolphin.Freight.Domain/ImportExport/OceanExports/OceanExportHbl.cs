using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Users;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.SysCodes;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class OceanExportHbl : AuditedAggregateRoot<Guid>, ISoftDelete
    {

        /// <summary>
        /// HB/L號碼
        /// </summary>
        public Guid? MblId { get; set; }
        /// <summary>
        /// HB/L
        /// </summary>
        [ForeignKey("MblId")]
        public virtual OceanExportMbl Mbl { get; set; }
        /// <summary>
        /// HB/L號碼
        /// </summary>
        public string HblNo { get; set; }
        /// <summary>
        /// S/O號碼
        /// </summary>
        public string SoNo { get; set; }
        /// <summary>
        /// Itn號碼
        /// </summary>
        public string ItnNo { get; set; }
        /// <summary>
        /// 客戶參考編號
        /// </summary>
        public string CustomerRefNo { get; set; }
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
        /// 實際託運人
        /// </summary>
        [ForeignKey("HblShipperId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner HblShipper { get; set; }
        /// <summary>
        /// 客戶ID
        /// </summary>
        public Guid? HblCustomerId { get; set; }
        /// <summary>
        /// 客戶
        /// </summary>
        [ForeignKey("HblCustomerId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner HblCustomer { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        public Guid? HblBillToId { get; set; }
        /// <summary>
        /// 收票人ID
        /// </summary>
        [ForeignKey("HblBillToId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner HblBillTo { get; set; }
        /// <summary>
        /// 收貨人ID
        /// </summary>
        public Guid? HblConsigneeId { get; set; }
        /// <summary>
        /// 收貨人
        /// </summary>
        [ForeignKey("HblConsigneeId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner HblConsignee { get; set; }
        /// <summary>
        /// 通知方ID
        /// </summary>
        public Guid? HblNotifyId { get; set; }
        /// <summary>
        /// 通知方
        /// </summary>
        [ForeignKey("HblNotifyId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner HblNotify { get; set; }
        /// <summary>
        /// 報關行ID
        /// </summary>
        public Guid? CustomsBrokerId { get; set; }
        /// <summary>
        /// 報關行
        /// </summary>
        [ForeignKey("CustomsBrokerId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CustomsBroker { get; set; }
        /// <summary>
        /// 卡車ID
        /// </summary>
        public Guid? TruckerId { get; set; }
        /// <summary>
        /// 卡車行
        /// </summary>
        [ForeignKey("TruckerId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Truckerr { get; set; }
        /// <summary>
        /// HB/L代理ID
        /// </summary>
        public Guid? AgentId { get; set; }
        /// <summary>
        /// HB/L代理
        /// </summary>
        [ForeignKey("AgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner Agent { get; set; }
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
        /// 轉運代理
        /// </summary>
        [ForeignKey("ForwardingAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner ForwardingAgent { get; set; }
        /// <summary>
        /// 操作員ID
        /// </summary>
        public Guid? HblOperatorId { get; set; }
        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("HblOperatorId")]
        public virtual UserData HblOperator { get; set; }
        /// <summary>
        /// 是否子代理提單
        /// </summary>
        public bool IsSubAgentBl { get; set; }
        /// <summary>
        /// 官方代理ID
        /// </summary>
        public Guid? ReceivingAgentId { get; set; }
        /// <summary>
        /// 官方代理
        /// </summary>
        [ForeignKey("ReceivingAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner ReceivingAgent { get; set; }
        /// <summary>
        /// 收貨地(POR)ID
        /// </summary>
        public Guid? PorId { get; set; }
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
        public Guid? PolId { get; set; }
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
        public Guid? PodId { get; set; }
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
        public Guid? DelId { get; set; }
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
        public Guid? FdestId { get; set; }
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
        /// FBA倉庫ID
        /// </summary>
        public Guid? FbaFcId { get; set; }
        /// <summary>
        /// FBA倉庫代理
        /// </summary>
        [ForeignKey("FbaFcId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner FbaFct { get; set; }
        /// <summary>
        /// 提空櫃地點ID
        /// </summary>
        public Guid? EmptyPickupId { get; set; }
        /// <summary>
        /// 提空櫃地點
        /// </summary>
        [ForeignKey("EmptyPickupId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner EmptyPickup { get; set; }
        /// <summary>
        /// 卡車交貨地ID
        /// </summary>
        public Guid? DeliveryToId { get; set; }
        /// <summary>
        /// 卡車交貨地
        /// </summary>
        [ForeignKey("DeliveryToId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner DeliveryTo { get; set; }
        /// <summary>
        /// 貨物就緒日期
        /// </summary>
        public DateTime? CargoArrivalDate { get; set; }
        /// <summary>
        /// 卡車收貨地ID
        /// </summary>
        public Guid? CargoPickupId { get; set; }
        /// <summary>
        /// 卡車收貨地
        /// </summary>
        [ForeignKey("CargoPickupId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CargoPickup { get; set; }
        /// <summary>
        /// 運輸模式ID
        /// </summary>
        public Guid? ShipModeId { get; set; }
        /// <summary>
        /// 運輸模式
        /// </summary>
        [ForeignKey("ShipModeId")]
        public virtual SysCode ShipMode { get; set; }
        /// <summary>
        /// 運費買方ID
        /// </summary>
        public Guid? FreightTermForBuyerId { get; set; }
        /// <summary>
        /// 運費買方
        /// </summary>
        [ForeignKey("FreightTermForBuyerId")]
        public virtual SysCode FreightTermForBuyer { get; set; }
        /// <summary>
        /// 運費賣方ID
        /// </summary>
        public Guid? FreightTermForSalerId { get; set; }
        /// <summary>
        /// 運費賣方
        /// </summary>
        [ForeignKey("FreightTermForSalerId")]
        public virtual SysCode FreightTermForSalerr { get; set; }
        /// <summary>
        /// 運輸條款FronID
        /// </summary>
        public Guid? SvcTermFromId { get; set; }
        /// <summary>
        /// 運輸條款Fron
        /// </summary>
        [ForeignKey("SvcTermFromId")]
        public virtual SysCode SvcTermFrom { get; set; }
        /// <summary>
        /// 運輸條款ToID
        /// </summary>
        public Guid? SvcTermToId { get; set; }
        /// <summary>
        /// 運輸條款To
        /// </summary>
        [ForeignKey("SvcTermToId")]
        public virtual SysCode SvcTermTo { get; set; }
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
        /// 貨物類別
        /// </summary>
        [ForeignKey("CargoTypeId")]
        public virtual SysCode CargoType { get; set; }
        /// <summary>
        /// 業務類別ID
        /// </summary>
        public Guid? MblSalesTypeId { get; set; }
        /// <summary>
        /// 業務類別
        /// </summary>
        [ForeignKey("MblSalesTypeId")]
        public virtual SysCode MblSalesType { get; set; }
        /// <summary>
        /// 倉儲結關日
        /// </summary>
        public DateTime? HblWhCutOffTime { get; set; }
        /// <summary>
        /// 最早收櫃日
        /// </summary>
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
        public DateTime? LcIssueDate { get; set; }
        /// <summary>
        /// 裝船日期
        /// </summary>
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
        /// 扣留人員
        /// </summary>
        [ForeignKey("HolderId")]
        public virtual UserData Holder { get; set; }
        /// <summary>
        /// 已放貨
        /// </summary>
        public bool IsReleased { get; set; }
        /// <summary>
        /// 放貨日期
        /// </summary>
        public DateTime? HblReleaseDate { get; set; }
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
        public virtual Dolphin.Freight.TradePartners.TradePartner MblReferralBy { get; set; }
        /// <summary>
        /// W/O號碼
        /// </summary>
        public string WoNo { get; set; }
        /// <summary>
        /// 運輸類別ID
        /// </summary>
        public Guid? ShipTypeId { get; set; }
        /// <summary>
        /// 運輸類別
        /// </summary>
        [ForeignKey("ShipTypeId")]
        public virtual SysCode ShipType { get; set; }
        /// <summary>
        /// 國貿條規ID
        /// </summary>
        public Guid? IncotermsId { get; set; }
        /// <summary>
        /// 國貿條規
        /// </summary>
        [ForeignKey("IncotermsId")]
        public virtual SysCode Incoterms { get; set; }
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
        /// 目的地CY/CFS地點
        /// </summary>
        [ForeignKey("CyCfsLocationId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CyCfsLocation { get; set; }
        /// <summary>
        /// 是否目的地鐵路
        /// </summary>
        public bool IsRailwayCode { get; set; }
        /// <summary>
        /// 目的地鐵路ID
        /// </summary>
        public Guid? RailwayCodeId { get; set; }
        /// <summary>
        /// 目的地鐵路
        /// </summary>
        [ForeignKey("RailwayCodeId")]
        public virtual SysCode RailwayCode { get; set; }
        /// <summary>
        /// 預計最終交付時間
        /// </summary>
        public DateTime? DoorDeliveryETA { get; set; }
        /// <summary>
        /// 實際最終交付時間
        /// </summary>
        public DateTime? DoorDeliveryATA { get; set; }
        /// <summary>
        /// 出口報關時間
        /// </summary>
        public DateTime? CustomClearance { get; set; }
        /// <summary>
        /// 進口清關時間
        /// </summary>
        public DateTime? CustomDeclaration { get; set; }
        /// <summary>
        /// 卡片顏色標記ID
        /// </summary>
        public Guid? CardColorId { get; set; }
        /// <summary>
        /// 卡片顏色標記
        /// </summary>
        [ForeignKey("CardColorId")]
        public virtual SysCode CardColor { get; set; }
        /// <summary>
        /// 顏色標記ID
        /// </summary>
        public Guid? ColorRemarkId { get; set; }
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
        public int SurplusType {get;set;}
        public string PoNo { get; set; }
        /// <summary>
        /// 是否鎖定
        /// </summary>
        public bool IsLocked { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
