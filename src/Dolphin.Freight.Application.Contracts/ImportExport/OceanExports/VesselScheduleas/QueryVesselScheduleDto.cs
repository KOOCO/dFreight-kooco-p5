using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas
{
    public class QueryVesselScheduleDto : PagedAndSortedResultRequestDto
    {
        public string QueryType { get; set; }
        public string QueryName { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string QueryKey { get; set; }
        public string Search { get; set; }
        public Guid? OfficeId { get; set; }
        public Guid? OvearseaAgentId { get; set; }
        public Guid? CarrierId { get; set; }
        public string Vessel { get; set; }
        public Guid? Pol { get; set;}
        public Guid? ShippingAgentId { get; set; }
        public Guid? ForwardingAgentId { get; set; }
        public DateTime? PostDate { get; set; }
        public Guid? ShipModeId { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime? Eta { get; set; }
        public Guid? BlTypeId { get; set; }

    }
}

