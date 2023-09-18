using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.OceanImports
{
    public class QueryHblDto : PagedAndSortedResultRequestDto
    {
        public string Search { get; set; }
        public string QueryType { get; set; }
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
        public Guid? OvearseaAgentId { get; set; }
        public Guid? ShipperId { get; set; }
        public string Vessel { get; set; }
        public Guid? Pol { get; set; }
        public Guid? Pod { get; set; }
        public Guid? PorId { get; set; }
        public Guid? Del { get; set; }

        public Guid? NotifyId { get; set; }
        public Guid? ConsigneeId { get; set; }

        public Guid? ShipModeId { get; set; }
        public Guid? SaleId { get; set; }
        public Guid? OfficeId { get; set; }
        public Guid? CyLocationId { get; set; }
        public Guid? TruckerId { get; set; }
        public Guid? FinalDestId { get; set; }
        public Guid? SvcTermId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime? Eta { get; set; }
        public DateTime? DelEta { get; set; }
        public DateTime? FinalDestEta { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
