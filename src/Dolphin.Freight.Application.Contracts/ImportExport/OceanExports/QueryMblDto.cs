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
        public Guid? OvearseaAgentId { get; set; }
        public Guid? CarrierId { get; set; }
        public string Vessel { get; set; }
        public Guid? Pol { get; set; }
        public Guid? Pod { get; set; }
        public Guid? Del { get; set; }
        public Guid? DeliverTo { get; set; }
        public Guid? ShippingAgentId { get; set; }
        public Guid? ForwardingAgentId { get; set; }
        public DateTime? PostDate { get; set; }
        public Guid? ShipModeId { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime? Eta { get; set; }
        public bool? BlCancelled { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
