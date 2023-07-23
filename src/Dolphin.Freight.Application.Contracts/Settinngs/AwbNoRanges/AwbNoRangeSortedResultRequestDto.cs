using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Settinngs.AwbNoRanges
{
    public class AwbNoRangeSortedResultRequestDto : PagedAndSortedResultRequestDto
    {
        public DateTime? CreatedDate { get; set; }
        //public string Prefix { get; set; }
        public string Filter { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
