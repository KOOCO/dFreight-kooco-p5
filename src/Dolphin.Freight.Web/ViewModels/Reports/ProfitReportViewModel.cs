using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Common;
using System;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.Reports
{
    public class ProfitReportViewModel
    {
        public DateTime PostDate { get; set; }
        public string Currency { get; set; } = "USD";
        public string Sales { get; set; }
        public string Customer { get; set; }
        public string AgentName { get; set; }
        public string PolEtd { get; set; }
        public string Shipper { get; set; }
        public string PodEtd { get; set; }
        public string Consignee { get; set; }
        public string Operator { get; set; }
        public string FileNo { get; set; }
        public string MawbNo { get; set; }
        public string ChargableWeight { get; set; }
        public string HawbNo { get; set; }
        public string Measurement { get; set; }
        public IList<InvoiceDto> Invoices { get; set; }
        public IList<InvoiceDto> AR { get; set; }
        public IList <InvoiceDto> AP { get; set;}
        public IList<InvoiceDto> DC { get; set; }
        public double ARTotal { get; set; }
        public double APTotal { get; set; }
        public double DCTotal { get; set; }
        public double Total { get; set; }
        public FreightPageType PageType { get; set; }
        public bool IsPDF { get; set; }
        public string InvoicesJson { get; set; }
        public string ReportType { get; set; }
        public string AWbNo { get; set; }
    }
}
