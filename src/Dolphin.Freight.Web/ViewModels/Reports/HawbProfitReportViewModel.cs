using Dolphin.Freight.Accounting.Invoices;
using System;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.Reports
{
    public class HawbProfitReportViewModel
    {
        public DateTime PostDate { get { return new DateTime(); } }
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
    }
}
