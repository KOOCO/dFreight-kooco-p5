using System.Security.Principal;

namespace Dolphin.Freight.Web.ViewModels.Reports
{
    public class DocumentPackageViewModel
    {
        public string Shippper { get; set; }
        public string IssuingCarrier { get; set; }
        public string Notify { get; set; }
        public string AirWayBillNo { get; set; }
        public string Consignee { get; set; }
        public string DepartureName { get; set; }
        public string Carrier { get; set; }
        public string DestinationName { get; set; }
        public string HandlingInformation { get; set; }
        public string RCP { get; set; }
        public string GrossWeight { get; set; }
        public string IATA { get; set; }
        public string DocNumber { get; set; }
        public string BillTo { get; set; }
        public string ArrivalDate { get; set; }
        public string NVD { get; set; }
        public string NCV { get; set; }
        public bool IsPDF { get; set; }
    }
}