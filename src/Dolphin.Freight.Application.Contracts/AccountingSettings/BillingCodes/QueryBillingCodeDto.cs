using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.AccountingSettings.BillingCodes
{
    public class QueryBillingCodeDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 科目代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 使用類別0：AR、1：DC、2；AP、3：GA
        /// </summary>
        public int? BillType { get; set; }
        /// <summary>
        /// 使用功能0：是否海運進口Mbl、1：是否海運進口Hbl、2：是否海運出口Mbl、3：是否海運出口Hbl、4：是否空運進口Mbl、5：是否空運進口Hbl、6：是否空運出口Mbl、7：是否空運出口Hbl
        /// </summary>
        public int? MethodType { get; set; }
        public bool? IsUsed { get; set; }
        public string Name { get; set; }
        public string GlCode { get; set; }
        public string GlDescription { get; set; }
    }
}
