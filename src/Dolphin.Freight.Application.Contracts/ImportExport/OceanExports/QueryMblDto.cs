using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class QueryMblDto : PagedAndSortedResultRequestDto
    {
        public string Search { get; set; }
        public string QueryType { get; set; }
        public string QueryName { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string QueryKey { get; set; }
        public Guid? MbId { get; set; }
    }
}
