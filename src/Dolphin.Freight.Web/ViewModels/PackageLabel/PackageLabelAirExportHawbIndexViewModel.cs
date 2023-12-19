using System;

namespace Dolphin.Freight.Web.ViewModels.PackageLabel
{
    public class PackageLabelAirExportHawbIndexViewModel
    {
        public string Delivery_To { get; set; }
        public string Destination { get; set; }
        public string Total_no_of_pieces { get; set; }
        public string Number_of_forwarder { get; set; }
        public string Origin { get; set; }
        public string HawbNo { get; set; }
        public string Hawb_Pc { get; set; }
        public string Air_way_bill_no { get; set; }
        public string BaseUrl { get; set; }
        public Guid ReportId { get; set; }
    }
}
