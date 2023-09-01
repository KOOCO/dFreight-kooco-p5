using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ReportLog
{
    public class ReportPdfLogDto
    {
        public List<MawbReportDto> AirExports { get; set; }
        public List<MawbReportDto> AirImports { get; set; }
        public List<MawbReportDto> OceanExports { get; set; }
        public List<MawbReportDto> OceanImports { get; set; }


    }
}
