using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Accounting.Payment
{
    public class QueryPaymentDto : PagedAndSortedResultRequestDto
    {
        public string QueryType { get; set; }
        public string QueryName { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string QueryKey { get; set; }

        public Guid? OfficeId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PaidTo { get; set; }
        public string Bank { get; set; }
        public string RefNo { get; set; }
        public Guid? IssuedBy { get; set; }
        public DateTime? PostDate { get; set; }
        public DateTime? ClearDate { get; set; }
        public DateTime? VoidDate { get; set; }
        public bool? Clear { get; set; }
        public bool? Void { get; set; }

    }
}
