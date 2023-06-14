using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dolphin.Freight.ReportLog
{
    public class ReportLog : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// MBLId or HBLId
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// 報表名稱
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// 報表資料內容
        /// </summary>
        public string ReportData { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }
    }

    public class MawbReport
    {
        public Guid Id { get; set; }
        public string ReportType { get; set; }
        public int No { get; set; }
        public string ConsigneeId { get; set; }
        public string OverseaAgentId { get; set; }
        public string OfficeId { get; set; } = "";
        public string Free { get; set; } = "";
        public string Nomi { get; set; } = "";
        public string Col { get; set; } = "";
        public string SUM { get; set; } = "";
        public string CNTR { get; set; } = "";
        public string OverseaAgent { get; set; } = "";
        public string Office { get; set; } = "";
        public string Consignee { get; set; } = "";
        public string FreightTermId { get; set; }
        public string SalesId { get; set; }
        public string SalesTypeId { get; set; } = "";
        public int? ServiceTermTypeFrom { get; set; }
        public int? ServiceTermTypeTo { get; set; }
        public string IsEcommerce { get; set; } = "";
        public string ShipperId { get; set; } = "";
        public string Shipper { get; set; } = "";
        public string CarrierId { get; set; } = "";
        public string CarrierName { get; set; } = "";
        public string CustomerId { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string BillToId { get; set; } = "";
        public string BillToName { get; set; } = "";
        public string CustomerRefNo { get; set; } = "";
        public string OpId { get; set; } = "";
        public string OpName { get; set; } = "";
        public string POLId { get; set; } = "";
        public string POLName { get; set; } = "";
        public string PODId { get; set; } = "";
        public string PODName { get; set; } = "";
        public string DestinationId { get; set; } = "";
        public string DestinationName { get; set; } = "";
        public string Vessel { get; set; } = "";
        public string MawbNo { get; set; } = "";
        public string CoLoaderId { get; set; } = "";
        public string FileNo { get; set; } = "";
        public string ForwardingAgentId { get; set; } = "";
        public string ForwardingAgentName { get; set; } = "";
        public string ColorRemarkId { get; set; } = "";
        public string ColorRemarkName { get; set; } = "";
    }
}
