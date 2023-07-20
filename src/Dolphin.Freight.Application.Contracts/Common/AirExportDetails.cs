using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Dolphin.Freight.Common
{
    public class AirExportDetails
    {
        public string ShippperName { get; set; }
        public string IssuingCarrierName { get; set; }
        public string NotifyName { get; set; }
        public string AirWayBillNo { get; set; }
        public string HawbNo { get; set; }
        public string MawbNo { get; set; }
        public string ConsigneeName { get; set; }
        public string DepartureName { get; set; }
        public string CarrierName { get; set; }
        public string DestinationName { get; set; }
        public string HandlingInformation { get; set; }
        public string GrossWeight { get; set; }
        public string IATA { get; set; }
        public string DocNumber { get; set; }
        public string BillTo { get; set; }
        public string ArrivalDate { get; set; }
        public string NVD { get; set; }
        public string NCV { get; set; }
        public bool IsPDF { get; set; }
        public string Package { get; set; }
        public string ChargableWeight { get; set; }
        public string Operator { get; set; }
        public string OverSeaAgentName { get; set; }
        public string SalesName { get; set; }
        public string CustomerName { get; set; }
        public string TruckerName { get; set; }
        public string CargoPickupName { get; set; }
        public string BillToName { get; set; }
        public string ChargeableWeightCneeLB { get; set; }
        public string ChargeableWeightCneeKG { get; set; }
        public Guid MawbId { get; set; }
        public DateTime? DepatureDate { get; set; }
        public string DestinationCountry { get; set; }
        public FreightPageType PageType { get; set; }
    }
}
