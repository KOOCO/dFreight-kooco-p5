using Org.BouncyCastle.Asn1.BC;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.Manifest
{
    public class ManifestByAgentAirExportMawb
    {
        public bool IsPDF { get; set; }
        public string Carrier_Agent { get; set; }
        public string Mawb_No { get; set; }
        public string File_No { get; set; }
        public string Flight_No { get; set; }
        public string Carrier { get; set; }
        public string Departure { get; set; }
        public string ETD { get; set; }
        public string Destination { get; set; }
        public string ETA { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Total_Package { get; set; }
        public string Total_Pc { get; set; }
        public string Itn_No { get; set; }
        public string Shipper { get; set; }
        public string Gross_Weight { get; set; }
        public string Chargable_Weight { get; set; }
        public string Measurement { get; set; }
        public string Term { get; set; }
        public string Agent { get; set; }
        public List<OverSeaAgent> OverSeaAgents { get; set; }
    }

    public class OverSeaAgent
    {
        public string Name { get; set; }
    }
}
