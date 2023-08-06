using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class QueryHblDto : PagedAndSortedResultRequestDto
    {
        public string QueryType { get; set; }
        public string Search { get; set; }
        public string QueryName { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string QueryKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? MblId { get; set; }

        public Guid? HblId { get; set; }
        public Guid? Id { get; set; }
    }
}
