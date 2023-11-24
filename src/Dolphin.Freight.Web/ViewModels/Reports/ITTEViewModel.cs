using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Security.Principal;

namespace Dolphin.Freight.Web.ViewModels.Reports
{
    public class ITTEViewModel
    {
        public string ITDate { get; set; }
        public string FlightNo { get; set; }
        public string ArrivalDate { get; set; }
        public string Port { get; set; }
        public string EntryNo { get; set; }
        public string ClassOfEntry { get; set; }
        public string PortCodeNo { get; set; }
        public string PortCodeNoOfLoading { get; set; }
        public string PortOf { get; set; }
        public string ImportedBy { get; set; }       
        public string Importer { get; set; }
        public string BondVia { get; set; }
        public string VesselOrCarrier { get; set; }
        public string PierOrStation { get; set; }
        public string CHLNo { get; set; }
        public string CustomPort { get; set; }
        public string ForeignDestination { get; set; }
        public string Consignee { get; set; }
        public string PortOfLoading { get; set; }
        public string AWBNo { get; set; }
        public string DateOfSailing { get; set; }
        public string VesselOrCarrierName { get; set; }
        public string Flag { get; set; }
        public string DateImported { get; set; }
        public string ForeignPort { get; set; }
        public string ExportedCountry { get; set; }
        public string ExportedOn { get; set; }
        public string GoodsNowAt { get; set; }
        public string GONo { get; set; }
        public string IsVesselSupplies { get; set; }
        public string LadenOn { get; set; }
        public string ClearedFor { get; set; }
        public string Package { get; set; }
        public string PackageName { get; set; }
        public double WeightKG { get; set; }
        public double WeightLG { get; set; }
        public string MawbNo { get; set; }
        public string HawbNo { get; set; }
        public string DestinationName { get; set; }
        public string Mark { get; set; }
        public string Description { get; set; }
        public bool IsPDF { get; set; }
    }
}
