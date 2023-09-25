using Dolphin.Freight.Accounting.InvoiceBills;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Accounting.Invoices
{
    public class InvoiceDto : AuditedEntityDto<Guid>
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
        /// 對應的Mawb
        /// </summary>
        public Guid? MawbId { get; set; }
        /// <summary>
        /// 對應的Hawb
        /// </summary>
        public Guid? HawbId { get; set; }
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
        public string ShipTo { get; set; }
        /// <summary>
        /// 發布日期
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? PostDate { get; set; }
        /// <summary>
        /// 發票日期
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
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
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
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
        /// 類別：0：AR，1：D/C，2：AP ，3：G&A 收入，4：G&A 支出
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
        /// 類別
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 分站
        /// </summary>
        public Guid? OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string ShipToName { get; set; }
        public string IssuedBy { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public Guid? CustomerId { get; set; }
        /// <summary>
        /// 客戶名稱
        /// </summary>
        public Guid? CustomerName { get; set; }

        /// <summary>
        /// 科目ID
        /// </summary>
        public Guid? GlCodeId { get; set; }

        /// <summary>
        /// 幣種
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 發票金額
        /// </summary>
        public decimal? InvoiceAmount { get; set; }

        /// <summary>
        /// 餘額金額
        /// </summary>
        public decimal? BalanceAmount { get; set; }

        /// <summary>
        /// 收付款
        /// </summary>
        public string PaymentAmount { get; set; }

        /// <summary>
        /// 收付款(TWD)
        /// </summary>
        public string PaymentAmountTwd { get; set; }

        /// <summary>
        /// 發票描述
        /// </summary>
        public string InvoiceDescription { get; set; }

        /// <summary>
        /// 文件編號
        /// </summary>
        public string DocNo { get; set; }

        /// <summary>
        /// 提單號碼
        /// </summary>
        public string BlNo { get; set; }

        /// <summary>
        /// 操作員
        /// </summary>
        public string CsCode { get; set; }

        /// <summary>
        /// 業務員
        /// </summary>
        public string SalesCode { get; set; }
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
        /// 對象註冊地
        /// </summary>
        public string PlaceOfRegistration { get; set; }
        /// <summary>
        /// 參考編號
        /// </summary>
        public string TheReferenceNo { get; set; }
        /// <summary>
        /// 顯示用ETD
        /// </summary>
        public string Etd { get; set; }
        /// <summary>
        /// 顯示用ETA
        /// </summary>
        public string Eta { get; set; }
        /// <summary>
        /// 顯示用OB/L
        /// </summary>
        public string OblName { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public List<CreateUpdateInvoiceBillDto> InvoiceBillDtos { get; set; }
    }
}

