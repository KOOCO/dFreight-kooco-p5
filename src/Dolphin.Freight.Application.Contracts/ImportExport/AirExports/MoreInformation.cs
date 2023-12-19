using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class MoreInformation
    {
        public string Id { get; set; }
        public string Valuation { get; set; }
        public string Tax { get; set; }
        public string CurrencyConversionRate { get; set; }
        public string CCChargesinDestCurrency { get; set; }
        public string ChargesatDestination { get; set; }
    }
}
