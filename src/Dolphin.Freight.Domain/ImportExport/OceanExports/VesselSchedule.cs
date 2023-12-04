using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using Dolphin.Freight.Settings.Substations;
using System.ComponentModel.DataAnnotations.Schema;
using Dolphin.Freight.Settings.SysCodes;
using System.ComponentModel.DataAnnotations;
using Dolphin.Freight.Settings.Ports;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    /// <summary>
    /// 船期
    /// </summary>
    public class VesselSchedule : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        /// <summary>
        /// 船期編號 
        /// </summary>
        public string ReferenceNo { get; set; }
        /// <summary>
        /// 分站ID
        /// </summary>
        public Guid? OfficeId { get; set; }
        /// <summary>
        /// 分站
        /// </summary>
        [ForeignKey("OfficeId")]
        public virtual Substation Office { get; set; }
        /// <summary>
        /// 提單類別ID
        /// </summary>
        public Guid? BlTypeId { get; set; }
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
        /// 船公司ID
        /// </summary>
        public Guid? MblCarrierId { get; set; }
        /// <summary>
        /// 船公司
        /// </summary>
        [ForeignKey("MblCarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblCarrier { get; set; }
        /// <summary>
        /// 船公司(財務窗口)ID
        /// </summary>
        public Guid? BlAcctCarrierId { get; set; }
        /// <summary>
        /// 船公司(財務窗口)
        /// </summary>
        [ForeignKey("BlAcctCarrierId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner BlAcctCarrier { get; set; }
        /// <summary>
        /// 本地代理ID
        /// </summary>
        public Guid? ShippingAgentId { get; set; }
        /// <summary>
        /// 本地代理
        /// </summary>
        [ForeignKey("ShippingAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner ShippingAgent { get; set; }
        /// <summary>
        /// 海外代理收貨人ID
        /// </summary>
        public Guid? MblOverseaAgentId { get; set; }
        /// <summary>
        /// 海外代理收貨人
        /// </summary>
        [ForeignKey("MblOverseaAgentId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblOverseaAgent { get; set; }
        /// <summary>
        /// 通知方ID
        /// </summary>
        public Guid? MblNotifyId { get; set; }
        /// <summary>
        /// 通知方
        /// </summary>
        [ForeignKey("MblNotifyId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner MblNotify { get; set; }
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
        /// 共同裝運人ID
        /// </summary>
        public Guid? CoLoaderId { get; set; }
        /// <summary>
        /// 共同裝運人
        /// </summary>
        [ForeignKey("CoLoaderId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner CoLoader { get; set; }
        /// <summary>
        /// 船名
        /// </summary>
        public string VesselName { get; set; }
        /// <summary>
        /// 航次
        /// </summary>
        public string Voyage { get; set; }
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
        /// 提空櫃地點ID
        /// </summary>
        public Guid? EmptyPickupId { get; set; }
        /// <summary>
        /// 提空櫃地點
        /// </summary>
        [ForeignKey("EmptyPickupId")]
        public virtual Dolphin.Freight.TradePartners.TradePartner EmptyPickup { get; set; }
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
        /// 運費ID
        /// </summary>
        public Guid? FreightTermId { get; set; }
        /// <summary>
        /// 運費
        /// </summary>
        [ForeignKey("FreightTermId")]
        public virtual SysCode FreightTerm { get; set; }
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
        /// OB/L類別ID
        /// </summary>
        public Guid? OblTypeId { get; set; }
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
        public virtual Port TransPort1 { get; set; }
        /// <summary>
        /// 中轉港ETA
        /// </summary>
        public DateTime? Trans1Eta { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
