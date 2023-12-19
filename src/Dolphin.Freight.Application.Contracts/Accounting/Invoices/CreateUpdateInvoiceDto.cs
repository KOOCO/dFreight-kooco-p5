using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Accounting.Invoices
{
    public class CreateUpdateInvoiceDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 對應的Mbl
        /// </summary>
        public Guid? MblId { get; set; }
        /// <summary>
        /// 對應的Hbl
        /// </summary>
        public Guid? HblId { get; set; }
        /// <summary>
        /// 對應的VesselSchedule
        /// </summary>
        public Guid? VesselScheduleId { get; set; }
        /// <summary>
        /// 對應的ExportBooking
        /// </summary>
        public Guid? BookingId { get; set; }
        /// <summary>
        /// 對應的MawbId
        /// </summary>
        public Guid? MawbId { get; set; }
        /// <summary>
        /// 對應的HawbId
        /// </summary>
        public Guid? HawbId { get; set; }
        /// <summary>
        /// 分站
        /// </summary>
        public Guid? OfficeId { get; set; }
        /// <summary>
        /// 發票號碼
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// 運送地
        /// </summary>
        public Guid? ShipToId { get; set; }
        /// <summary>
        /// 發布日期
        /// </summary>
        public DateTime? PostDate { get; set; }
        /// <summary>
        /// 發票日期
        /// </summary>
        public DateTime? InvoiceDate { get; set; }
        /// <summary>
        /// 最後應付日期
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? LastDate { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// 發票公司ID
        /// </summary>
        public Guid? InvoiceCompanyId { get; set; }
        /// <summary>
        /// 發票公司
        /// </summary>
        public string InvoiceCompanyName { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string AttentionTo { get; set; }
        /// <summary>
        /// 最後支付號碼
        /// </summary>
        public string LastNo { get; set; }
        /// <summary>
        /// 內部應付號碼
        /// </summary>
        public string SystemNo { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 匯率來源ID
        /// </summary>
        public Guid? CcyRateSourceId { get; set; }

        /// <summary>
        /// 國貿條規ID
        /// </summary>
        public Guid? IncotermsId { get; set; }
        /// <summary>
        /// 未結清金額
        /// </summary>
        public double OutstandingAmount { get; set; }
        /// <summary>
        /// 已支付金額
        /// </summary>
        public double PaidAmount { get; set; }
        /// <summary>
        /// 參考編號/P.O.號碼
        /// </summary>
        public string PoNo { get; set; }
        /// <summary>
        /// 備忘錄
        /// </summary>
        public string Memorandum { get; set; }
        /// <summary>
        /// 利潤分成(%)
        /// </summary>
        public int ProfitShare { get; set; }
        /// <summary>
        /// 金額是否確認
        /// </summary>
        public bool IsAmountConfirmed { get; set; }
        /// <summary>
        /// 代理參考編號
        /// </summary>
        public string CustomerRefNo { get; set; }
        /// <summary>
        ///  類別：0：AR，1：D/C，2：AP ，3：G&A 收入，4：G&A 支出
        /// </summary>
        public int InvoiceType { get; set; }
        /// <summary>
        /// 狀態：0：正常，1：鎖定，2：草稿
        /// </summary>
        public int InvoiceStatus { get; set; }
        /// <summary>
        /// 收付款ID
        /// </summary>
        public Guid PaymentId { get; set; }
        /// <summary>
        /// Mbl編號
        /// </summary>
        public string MblNo { get; set; }
        /// <summary>
        /// Hbl編號
        /// </summary>
        public string HblNo { get; set; }
        /// <summary>
        /// 文件編號
        /// </summary>
        public string FilingNo { get; set; }
        /// <summary>
        /// 是否收到發票
        /// </summary>
        public bool IsReceiveed { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }

        public double? TotalBeforeTax { get; set; }
        public double? TotalTax { get; set; }
        public double? TotalAmount { get; set; }
    }
}

