using System;

using System.ComponentModel.DataAnnotations.Schema;

using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using Dolphin.Freight.AccountingSettings.GlCodes;

namespace Dolphin.Freight.AccountingSettings.BillingCodes
{
    /// <summary>
    /// 計費代碼
    /// </summary>
    public class BillingCode : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        /// <summary>
        /// 科目代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名稱(英文)
        /// </summary>
        public string BillingName { get; set; }
        /// <summary>
        /// 本地名稱
        /// </summary>
        public string LocalName { get; set; }
        /// <summary>
        /// 收入ID
        /// </summary>
        public Guid? RevenueId { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        [ForeignKey("RevenueId")]
        public virtual GlCode Revenue { get; set; }
        /// <summary>
        /// 支出ID
        /// </summary>
        public Guid? CostId { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        [ForeignKey("CostId")]
        public virtual GlCode Cost { get; set; }
        /// <summary>
        /// 貸記ID
        /// </summary>
        public Guid? CreditId { get; set; }
        /// <summary>
        /// 貸記
        /// </summary>
        [ForeignKey("CreditId")]
        public virtual GlCode Credit { get; set; }
        /// <summary>
        /// 借記ID
        /// </summary>
        public Guid? DeitId { get; set; }
        /// <summary>
        /// 借記
        /// </summary>
        [ForeignKey("DeitId")]
        public virtual GlCode Deit { get; set; }
        /// <summary>
        /// 是否A/R
        /// </summary>
        public bool IsAR { get; set; }
        /// <summary>
        /// 是否A/P
        /// </summary>
        public bool IsAP { get; set; }
        /// <summary>
        /// 是否DC
        /// </summary>
        public bool IsDC { get; set; }
        /// <summary>
        /// 是否G&A
        /// </summary>
        public bool IsGA { get; set; }
        /// <summary>
        /// 是否海運進口Mbl
        /// </summary>
        public bool IsOceanImportMbl { get; set; }
        /// <summary>
        /// 是否海運出口Hbl
        /// </summary>
        public bool IsOceanImportHbl { get; set; }
        /// <summary>
        /// 是否海運出口Mbl
        /// </summary>
        public bool IsOceanExportMbl { get; set; }
        /// <summary>
        /// 是否海運進口Hbl
        /// </summary>
        public bool IsOceanExportHbl { get; set; }
        /// <summary>
        /// 是否空運進口Mbl
        /// </summary>
        public bool IsAirImportMbl { get; set; }
        /// <summary>
        /// 是否空運出口Hbl
        /// </summary>
        public bool IsAirImportHbl { get; set; }
        /// <summary>
        /// 是否空運出口Mbl
        /// </summary>
        public bool IsAirExportMbl { get; set; }
        /// <summary>
        /// 是否空運進口Hbl
        /// </summary>
        public bool IsAirExportHbl { get; set; }
        /// <summary>
        /// 是否卡車
        /// </summary>
        public bool IsTkm { get; set; }
        /// <summary>
        /// 是否綜合業務
        /// </summary>
        public bool IsMsm { get; set; }
        /// <summary>
        /// 是否倉儲
        /// </summary>
        public bool IsWhs { get; set; }
        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public bool IsPayroll { get; set; }
    }
}
