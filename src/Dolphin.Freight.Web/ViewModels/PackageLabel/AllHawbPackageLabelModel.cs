using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.PackageLabel
{
    public class AllHawbPackageLabelModel
    {
        public string Id { get; set; }
        public string Air_WayBill_No { get; set; }
        public string Destination { get; set; }
        public string Total_No_Of_Pieces { get; set; }
        public string Name_Of_Forwarder { get; set; }
        public string Origin { get; set; }
        public string Hawb_No { get; set; }
        public string Hawb_Pc { get; set; }

        //public List<SelectListItem> AllHawbList { get; set; }
    }
}
