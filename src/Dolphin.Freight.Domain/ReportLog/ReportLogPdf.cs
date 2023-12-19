using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin.Freight.ReportLog
{
    public class ReportLogPdf
    {
        public List<MawbReport> OceanImports { get; set; }
        public List<MawbReport> OceanExports { get; set; }
        public List<MawbReport> AirExports { get; set; }
        public List<MawbReport> AirImports { get; set; }
    }
}
