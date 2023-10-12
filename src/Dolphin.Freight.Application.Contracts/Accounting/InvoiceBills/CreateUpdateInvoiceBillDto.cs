using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Accounting.InvoiceBills
{
    public class CreateUpdateInvoiceBillDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Invoice的Id
        /// </summary>
        public Guid? InvoiceId { get; set; }
        /// <summary>
        /// 費用代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 費用描述(英文)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 到付或預付
        /// </summary>
        public string PPOrCC { get; set; }
        /// <summary>
        /// 類別
        /// </summary>
        public string BillType { get; set; }
        /// <summary>
        /// 單位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 幣種
        /// </summary>
        public string Currency { get; set; }
        public int Price { get; set; }
        public Guid? TaxId { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public double Quantity { get; set; }
        /// <summary>
        /// 匯率
        /// </summary>
        public double Rate { get; set; }
        /// <summary>
        /// 金額
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 給代辦金額
        /// </summary>
        public double AmountToAgent { get; set; }
        /// <summary>
        /// 是否NonProfit
        /// </summary>
        public bool IsNonProfit { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
