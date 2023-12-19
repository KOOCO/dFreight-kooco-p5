using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Accounting.Payment
{
    public class PaymentDto : AuditedEntityDto<Guid>
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
        /// 收款人
        /// </summary>
        public Guid? PaidTo { get; set; }

        /// <summary>
        /// 收款人
        /// </summary>
        public string PaidToName { get; set; }
        /// <summary>
        /// 在支票上顯示收款人
        /// </summary>
        public bool ShowPartyOnCheck { get; set; }

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
        /// 結算
        /// </summary>
        public DateTime? Clear { get; set; }

        /// <summary>
        /// 作廢
        /// </summary>
        public DateTime? Invalid { get; set; }

        /// <summary>
        /// 分站ID
        /// </summary>
        public Guid? OfficeId { get; set; }
        public string OfficeName { get; set; }

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
        /// <summary>
        /// 建立者
        /// </summary>
        public string Creator { get; set; }
        public bool Void
        {
            get { return Invalid != null; }
            set { /* You can't set a value for Void since it's based on Invalid property */ }
        }
        public string OpName { get; set; }
    }
}

