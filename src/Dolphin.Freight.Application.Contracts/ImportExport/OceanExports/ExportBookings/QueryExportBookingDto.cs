using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.OceanExports.ExportBookings
{
    public class QueryExportBookingDto : PagedAndSortedResultRequestDto
    {
        public string QueryType { get; set; }
        public string QueryName { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string QueryKey { get; set; }
        public string Search { get; set; }
        public Guid? OfficeId { get; set; }
        public Guid? CarrierId { get; set; }
        public Guid? ShipperId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? Pod { get; set; }
        public Guid? Del { get; set; }
        public Guid? DeliverTo { get; set; }
        public Guid? NotifyId { get; set; }
        public Guid? ConsigneeId { get; set; }
        public Guid? FbaFcId { get; set; }
        public DateTime? PostDate { get; set; }
        public Guid? ShipModeId { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime? Eta { get; set; }
        public bool? BlCancelled { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CargoReadyDate { get; set; }
    }
}

