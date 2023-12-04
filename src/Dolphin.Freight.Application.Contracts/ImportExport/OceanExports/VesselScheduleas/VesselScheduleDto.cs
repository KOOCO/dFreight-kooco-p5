using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas
{
    public class VesselScheduleDto : AuditedEntityDto<Guid>
    {
        public bool isNewVessel { get; set; }
        /// <summary>
        /// 船期編號 
        /// </summary>
        public string ReferenceNo { get; set; }
        /// <summary>
        /// 列表顯示用，包含的SO清單編號
        /// </summary>
        public string SoNos { get; set; }
        /// <summary>
        /// 列表顯示用，包含的SO數量
        /// </summary>
        public string SoNoCount { get; set; }
        /// <summary>
        /// 分站ID
        /// </summary>
        public Guid? OfficeId { get; set; }
        /// <summary>
        ///列表顯示用分站
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 提單類別ID
        /// </summary>
        public Guid? BlTypeId { get; set; }
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
        /// 顯示用船公司名稱
        /// </summary>
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
        /// 列表顯示用海外代理
        /// </summary>
        public string MblOverseaAgentName { get; set; }
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
        /// 提空櫃地點ID
        /// </summary>
        public Guid? EmptyPickupId { get; set; }
        /// <summary>
        /// 裝貨港(POL)ID
        /// </summary>
        public Guid? PolId { get; set; }
        /// <summary>
        /// 列表顯示用裝貨港
        /// </summary>
        public string PolName { get; set; }
        /// <summary>
        /// 裝貨港(POL) ETD
        /// </summary>
        public DateTime? PolEtd { get; set; }
        /// <summary>
        /// 收貨地(POR)ID
        /// </summary>
        public Guid? PorId { get; set; }
        /// <summary>
        /// 列表顯示用收貨地
        /// </summary>
        public string PorName { get; set; }
        /// <summary>
        /// 收貨地(POR) ETD
        /// </summary>
        public DateTime? PorEtd { get; set; }
        /// <summary>
        /// 卸貨港(POD)ID
        /// </summary>
        public Guid? PodId { get; set; }
        /// <summary>
        /// 列表顯示用 卸貨港
        /// </summary>
        public string PodName { get; set; }
        /// <summary>
        /// 卸貨港(POD) ETA
        /// </summary>
        public DateTime? PodEta { get; set; }
        /// <summary>
        /// 交貨地(DEL)ID
        /// </summary>
        public Guid? DelId { get; set; }
        /// <summary>
        /// 列表顯示用交貨地
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
        /// 最終目的地ETA
        /// </summary>
        public DateTime? FdestEta { get; set; }
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
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public string OpName { get; set; }


    }
}