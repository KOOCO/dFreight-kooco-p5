﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using System.ComponentModel.DataAnnotations.Schema;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settings.Substations;

namespace Dolphin.Freight.Accounting.Payment
{
    public class CustomerPayment : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 收付款ID
        /// </summary>
        public Guid PaymentId { get; set; }

        /// <summary>
        /// 收付款級別
        /// </summary>
        public string PaymentLevel { get; set; }

        /// <summary>
        /// 收款來源
        /// </summary>
        public Guid? ReceivablesSources { get; set; }
        /// <summary>
        /// 收款來源
        /// </summary>
        [ForeignKey("ReceivablesSources")]
        public virtual Dolphin.Freight.TradePartners.TradePartner ReceivablesSourcesName { get; set; }

        /// <summary>
        /// 發布日期
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 支票號碼
        /// </summary>
        public string CheckNo { get; set; }

        /// <summary>
        /// 存款
        /// </summary>
        public DateTime? Deposit { get; set; }

        /// <summary>
        /// 作廢
        /// </summary>
        public DateTime? Invalid { get; set; }

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
        /// 銀行
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 銀行幣種
        /// </summary>
        public string BankCurrency { get; set; }

        /// <summary>
        /// USD => TWD
        /// </summary>
        public string U2T { get; set; }

        /// <summary>
        /// RMB => TWD
        /// </summary>
        public string R2T { get; set; }

        /// <summary>
        /// HKD => TWD
        /// </summary>
        public string H2T { get; set; }

        /// <summary>
        /// 備忘錄
        /// </summary>       
        public string Memo { get; set; }
    }
}
