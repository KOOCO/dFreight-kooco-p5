using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.AirExports.Bookings
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
        public Guid? ShipperId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? ConsigneeId { get; set; }
        public Guid? OfficeId { get; set; }
        public Guid? NotifyId { get; set; }
        public Guid? ForwadingAgentId { get; set; }
        public Guid? CarrierId { get; set; }
        public Guid? SalesId { get; set; }
        public Guid? DepatureId { get; set; }
        public Guid? DestinationId { get; set; }
        public string FlightNo { get; set; }
        public string BookingRemarks { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? DepatureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
    }
}
