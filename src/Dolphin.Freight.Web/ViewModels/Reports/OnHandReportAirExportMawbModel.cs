using Dolphin.Freight.Web.ViewModels.PackageLabel;
using System;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.Reports
{
    public class OnHandReportAirExportMawbModel
    {
        public bool IsPDF { get; set; }
        public string HawbListsJson { get; set; }
        public List<AllHawbList> AllHawbLists { get; set; }
    }
}
