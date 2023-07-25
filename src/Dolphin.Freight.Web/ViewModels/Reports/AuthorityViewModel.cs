using Dolphin.Freight.Common;
using Microsoft.VisualBasic;
using System;

namespace Dolphin.Freight.Web.ViewModels.Reports
{
    public class AuthorityViewModel
    {

        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string NotifyParty { get; set; }
        public string MerchandiseImportedAt { get; set; }
        public string On { get; set; }
        public string VIA { get; set; }
        public string FileNo { get; set; }
        public string Date { get; set; }
        public string PrepBy { get; set; }
        public string SubAwbNO { get; set; }
        public string MawbNo { get; set; }
        public string DepAirport { get; set; }
        public DateTime? ETD { get; set; }
        public string HawbNo { get; set; }
        public string EntryPort { get; set; }
        public string ETA1 { get; set; }
        public string ETA2 { get; set; }
        public string ETA3 { get; set; }
        public string ManifestNo { get; set; }
        public string DestAirport { get; set; }
        public string FlightNo { get; set; }
        public string FinalDest { get; set; }
        public DateTime? LastFreeDay { get; set; }
        public string FrightLoc { get; set; }
        public string ITNO { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string ITIssuePlace { get; set; }
        public string FirmCode { get; set; }
        public string ITDate { get; set; }
        public string SubHawb { get; set; }
        public string DescriptionITNO { get; set; }
        public double PCS { get; set; }
        public double? Amount { get; set; }
        public string Remark { get; set; }
        public string AgentName { get; set; }
        public string CustomsBroker { get; set; }
        public string Trucker { get; set; }
        public string DocumentPickedBy { get; set; }
        public DateTime DocumentPickedDate { get; set; }
        public DateTime DocumentPickedTime { get; set; }
        public bool IsPDF { get; set; }
        public string ReportType { get; set; }
        public FreightPageType PageType { get; set; }
    }
}
