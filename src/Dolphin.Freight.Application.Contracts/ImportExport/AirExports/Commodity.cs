using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class Commodity
    {
        public string Id { get; set; }
        public string BookingNo { get; set; }
        public string Description { get; set; }
        public string PackagingType { get; set; }
        public string HTSCode { get; set; }
        public string PONumber { get; set; }
        public string NoOfPcs { get; set; }
        public string NetWeight { get; set; }
        public string GrossWeight { get; set; }
        public string UnitPrice { get; set; }
        public string Amount { get; set; }
        public string Details { get; set; }
        public string Container { get; set; }
    }
}
