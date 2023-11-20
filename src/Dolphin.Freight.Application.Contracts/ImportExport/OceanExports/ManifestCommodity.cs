using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class ManifestCommodity
    {
        public string Description { get; set; }
        public Guid? PackageCode { get; set; }
        public string PackagingType { get; set; }
        public string HTSCode { get; set; }
        public int? NoOfPcs { get; set; }
        public double? NetWeight { get; set; }
        public double? GrossWeight { get; set; }
        public double? UnitPrice { get; set; }
        public double? Amount { get; set; }
        public string Details { get; set; }
        public string Container { get; set; }
    }
}
