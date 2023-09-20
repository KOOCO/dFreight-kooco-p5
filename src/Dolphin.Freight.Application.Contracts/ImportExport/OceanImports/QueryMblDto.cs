using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.OceanImports
{
    public class QueryMblDto : PagedAndSortedResultRequestDto
    {
        public string QueryType { get; set; }
        public string QueryName { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string QueryKey { get; set; }
        public Guid? MbId { get; set; }
        public string Search { get; set; }
        public Guid? OvearseaAgentId { get; set; }
        public Guid? CarrierId { get; set; }
        public string Vessel { get; set; }
        public Guid? Pol { get; set; }
        public Guid? Pod { get; set; }
        public Guid? Del { get; set; }
        public Guid? OfficeId { get; set; }
        public Guid? CoLoaderId { get; set; }
        public Guid? ShippingAgentId { get; set; }
        public Guid? CyLocationId { get; set; }
        public DateTime? PostDate { get; set; }
        public Guid? SaleId { get; set; }
   
        public DateTime? CreationDate { get; set; }
    }
}
