using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.PackageLabel
{
    public class AllHblPackageLabelOceanExportMBL
    {
        public bool IsPDF { get; set; }
        public string Carrier { get; set; }
        public string To { get; set; }
        public string Mbl_No { get; set; }
        public string Hbl_No { get; set; }
        public string Pieces { get; set; }
        public string Destination { get; set; }
        public string Total_Pieces { get; set; }

        public List<AllHblList> AllHblLists { get; set; }
    }
    public class AllHblList
    {
        public string Id { get; set; }
        public string Hbl_No { get; set; }
        public string Hbl_Pc { get; set; }
    }
}
