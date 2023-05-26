using Dolphin.Freight.AirExports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Dolphin.Freight.AirExports;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.Web.Pages.Sales.TradePartner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Timing;
using Dolphin.Freight.Web.Pages.AirExports;
using Dolphin.Freight.ReportLog;

namespace Dolphin.Freight.Web.Pages.ReportScreen
{
    public class VolumeProfileReportModel : AbpPageModel
    {


        public Guid? MawbId { get; set; }
        public ILogger<CreateMawbModel> Logger { get; set; }
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IAirExportMawbAppService _airExportMawbAppService;
        private readonly IReportLogRepository _reportLogRepository;

        [BindProperty]
        public CreateMawbViewModel MawbModel { get; set; }
        public VolumeProfileReportViewModel VPRM { get; set; }





        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }


        public VolumeProfileReportModel(ITradePartnerAppService tradePartnerAppService,
            ISubstationAppService substationAppService,
            IAirportAppService airportAppService,
            IPackageUnitAppService packageUnitAppService,
            IAirExportMawbAppService airExportMawbAppService
            )
        {
            Logger = NullLogger<CreateMawbModel>.Instance;
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airExportMawbAppService = airExportMawbAppService;            
        }

   

        public async Task OnGetAsync()
        {
            MawbModel = new CreateMawbViewModel
            {
                // set default value
                //AwbDate = Clock.Now.ClearTime(),
                ItnNo = "NO EEI 30.37(a)",
                AwbType = AWBType.Normal,
                DVCarriage = "NVD",
                DVCustomer = "NCV",
                Insurance = "XXX",
                WtVal = "PPD",
                Other = "PPD"
            };

            await FillTradePartnerAsync();
            await FillSubstationAsync();
            await FillAirportAsync();
            FillWtValOther();
            await FillPackageUnitAsync();

        }

        #region FillTradePartnerAsync()
        private async Task FillTradePartnerAsync()
        {
            var tradePartnerLookup = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            TradePartnerLookupList = tradePartnerLookup.Items
                                                .Select(x => new SelectListItem(x.TPName + " / " + x.TPCode, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillSubstationAsync()
        private async Task FillSubstationAsync()
        {
            var substationLookup = await _substationAppService.GetSubstationsLookupAsync();
            SubstationLookupList = substationLookup.Items
                                                .Select(x => new SelectListItem(x.SubstationName + "  (" + x.AbbreviationName + ")", x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillAirportAsync()
        private async Task FillAirportAsync()
        {
            var airportLookup = await _airportAppService.GetAirportLookupAsync();
            AirportLookupList = airportLookup.Items
                                                .Select(x => new SelectListItem(x.AirportIataCode + " " + x.AirportName, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillWtVal()
        private void FillWtValOther()
        {
            WtValOtherList = new List<SelectListItem>
            {
                new SelectListItem { Value = "PPD", Text = "PPD"},
                new SelectListItem { Value = "COLL", Text = "COLL"}
            };
        }
        #endregion

        #region FillPackageUnitAsync()
        private async Task FillPackageUnitAsync()
        {
            var packageUnitLookup = await _packageUnitAppService.GetPackageUnitsLookupAsync();
            PackageUnitLookupList = packageUnitLookup.Items
                                                .Select(x => new SelectListItem(x.PackageName, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region Get MAWB Report


        #endregion

        #region CreateMawbViewModel
        public class CreateMawbViewModel
        {
            public int ReportType { get; set; }
            public Guid Id { get; set; }
            public string FilingNo { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String MawbCarrierId { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String IssuingCarrierId { get; set; }
            public AWBType AwbType { get; set; }

            public string MawbNo { get; set; }
            [DataType(DataType.Date)]
            public DateTime AwbDate { get; set; }
            public string ItnNo { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String ShipperId { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]

            public String ConsigneeId { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String NotifyId { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? PostDate { get; set; }
            [Required]
            [SelectItems(nameof(SubstationLookupList))]
            public String OfficeId { get; set; }

            public bool IsDirectMaster { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String AwbAcctCarrierId { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String CoLoaderId { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String MawbOperatorId { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String DepatureId { get; set; }
            [Required]
            [DataType(DataType.DateTime)]
            public DateTime? DepatureDate { get; set; }
            public string FlightNo { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public string DestinationId { get; set; }
            public DateTime ArrivalDate { get; set; }
            public string DVCarriage { get; set; }
            public string DVCustomer { get; set; }

            public string Insurance { get; set; }
            public string CarrierSpotNo { get; set; }
            [SelectItems(nameof(WtValOtherList))]
            public string WtVal { get; set; }
            [Display(Name = "Other")]
            [SelectItems(nameof(WtValOtherList))]
            public string Other { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String DeliveryId { get; set; }
            public string ShippingInfo { get; set; }
            public string ShipperLoad { get; set; }
            public string Sci { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String RouteTrans1Id { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans1ArrivalDate { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans1DepatureDate { get; set; }
            public string RouteTrans1FlightNo { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String RouteTrans1CarrierId { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String RouteTrans2Id { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans2ArrivalDate { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans2DepatureDate { get; set; }
            public string RouteTrans2FlightNo { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String RouteTrans2CarrierId { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String RouteTrans3Id { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans3ArrivalDate { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans3DepatureDate { get; set; }
            public string RouteTrans3FlightNo { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String RouteTrans3CarrierId { get; set; }

            public double Package { get; set; }
            [SelectItems(nameof(PackageUnitLookupList))]
            public String MawbPackageUnitId { get; set; }
            public double BuyingRate { get; set; }
            public RateUnitType? BuyingRateUnitType { get; set; }
            public double SellingRate { get; set; }
            public RateUnitType? SellingRateUnitType { get; set; }
            public double VolumeWeightKg { get; set; }
            public double VolumeWeightCbm { get; set; }

            public double GrossWeightKg { get; set; }
            public double GrossWeightLb { get; set; }
            public double GrossWeightAmount { get; set; }

            public double AwbGrossWeightKg { get; set; }
            public double AwbGrossWeightLb { get; set; }
            public double AwbGrossWeightAmount { get; set; }

            public double ChargeableWeightKg { get; set; }
            public double ChargeableWeightLb { get; set; }
            public double ChargeableWeightAmount { get; set; }

            public double AwbChargeableWeightKg { get; set; }
            public double AwbChargeableWeightLb { get; set; }
            public double AwbChargeableWeightAmount { get; set; }

            public IncotermsType IncotermsType { get; set; }
            public ServiceTermType ServiceTermTypeFrom { get; set; }
            [DisplayName("~")]
            public ServiceTermType ServiceTermTypeTo { get; set; }
            public DisplayUnitType DisplayUnit { get; set; }

            public bool IsAwbCancelled { get; set; }

            [DataType(DataType.Date)]
            public DateTime? AwbCancelledDate { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String AwbCancelledOpId { get; set; }
            public ReasonForCancelType? ReasonForCancel { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String BusinessReferredId { get; set; }
            public bool IsECom { get; set; }
            public string Report { get; set; }
        }
        #endregion

        public class VolumeProfileReportViewModel
        {
            public int Id { get; set; }
            public string Report { get; set; }
            [DisplayName("Output Format")]
            public string OutputFormat { get; set; }
            [DisplayName("Period Type")]
            public string PeriodType { get; set; }
            [DisplayName("Period Range")]
            public DateTime PeriodRange { get; set; }
            public string Office { get; set; }
            [DisplayName("Period")]
            public string ShippingType { get; set; }
            public string Currency { get; set; }
            public string Profit { get; set; }
            [DisplayName("Ship Mode")]
            public string ShipMode { get; set; }
            [DisplayName("Freight Code")]
            public string FreightCode { get; set; }
            [DisplayName("Freight Term")]
            public string FreightTerm { get; set; }
            public string Detail { get; set; }
            public string Sales { get; set; }
            [DisplayName("Output Type")]
            public string OutputType { get; set; }
            [DisplayName("Service Term Start")]
            public string ServiceTermStart { get; set; }
            [DisplayName("Service Term End")]
            public string ServiceTermEnd { get; set; }
            public string Status { get; set; }
            [DisplayName("Sales Type")]
            public string SalesType { get; set; }
            [DisplayName("Invoice Type")]
            public string InvoiceType { get; set; }
            [DisplayName("E-Commerce")]
            public string ECommerce { get; set; }
            [DisplayName("Output By")]
            public string OutputBy { get; set; }


            /// <summary>
            /// OUTPUT Options
            /// </summary>
            public bool Shipper { get; set; }
            public bool OverseaAgent { get; set; }
            public bool Consignee { get; set; }
            public bool Customer { get; set; }
            public bool Carrier { get; set; }
            public bool CustomsBroker { get; set; }
            public bool Trucker { get; set; }
            public bool AccountGroup { get; set; }
            public bool BillTo { get; set; }
            public bool ReferredBy { get; set; }
            public bool OutputOffice { get; set; }
            public bool ETD { get; set; }
            public bool ETA { get; set; }
            public bool OutputFreightTerm { get; set; }
            public bool Incoterms { get; set; }
            public bool ServiceTerm { get; set; }
            public bool MBLOP { get; set; }
            public bool Operation { get; set; }
            public bool OPCOOPOP { get; set; }
            public bool ShipLine { get; set; }
            public bool POL { get; set; }
            public bool POD { get; set; }
            public bool CountryOfPOL { get; set; }
            public bool CountryOfPOD { get; set; }
            public bool DEL { get; set; }
            public bool FinalDestination { get; set; }
            public bool VesselFlight { get; set; }
            public bool MblMawbWarehouse { get; set; }
            public bool HblHawb { get; set; }
            public bool OutputFile { get; set; }
            public bool DoorMove { get; set; }
            public bool Clearance { get; set; }
            public bool ISF { get; set; }
            public bool FBAFC { get; set; }
            public bool OutputSalesType { get; set; }
            public bool HblNominatedAgent { get; set; }
            public bool OutputECommerce { get; set; }
            public bool ForwardingAgent { get; set; }
            public bool CarrierContractNo { get; set; }
            public bool MblColorRemark { get; set; }
            public bool HblColorRemark { get; set; }
            public bool CoLoader { get; set; }
            public bool BlType { get; set; }
            public bool LatestGateIn { get; set; }



        }

        public List<SelectListItem> ReportList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "vp", Text = "Volume & Profit"},
            new SelectListItem { Value = "v", Text = "Volume Only"}

        };
        public List<SelectListItem> PeriodType { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "postdate", Text = "Post Date"},
            new SelectListItem { Value = "ETD", Text = "ETD"},
            new SelectListItem { Value = "ETA", Text = "ETA"}

        };

        public List<SelectListItem> Format { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "DS", Text = "Data Sheet"},
            new SelectListItem { Value = "PDF", Text = "PDF"},

        };
        public List<SelectListItem> Office { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "taipei", Text = "Taipei"}

        };

        public List<SelectListItem> ShippingType { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "taipei", Text = "Taipei"}

        };
        public List<SelectListItem> Currency { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "usd", Text = "usd"}

        };
        public List<SelectListItem> Profit { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "all", Text = "ALL"},
             new SelectListItem { Value = "range", Text = "Range"},
              new SelectListItem { Value = "negative", Text = "Negative"}

        };
        public List<SelectListItem> ShipMode { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "fcl", Text = "FCL"},
             new SelectListItem { Value = "lcl", Text = "LCL"},
              new SelectListItem { Value = "fak", Text = "FAK"},
              new SelectListItem { Value = "bulk", Text = "BULK"}

        };

        public List<SelectListItem> FreightCode { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "all", Text = "AE01A/F: AIR FREIGHT CHARGE"},
             new SelectListItem { Value = "range", Text = "AE02FSC: FUEL SUR CHARGE"},
              new SelectListItem { Value = "negative", Text = "AE03SECURI: SECURITY CHARGE"},
              new SelectListItem { Value = "range", Text = "AE02FSC: FUEL SUR CHARGE"},
              new SelectListItem { Value = "range", Text = "AE05DG: DANGEROUS GOOD HANDLING"},
              new SelectListItem { Value = "range", Text = "AE06TRUCKI: TRUCKING CHARGE"},
              new SelectListItem { Value = "range", Text = "AE07WHSETR: WAREHOUSE TRANSFER FEE"},
              new SelectListItem { Value = "range", Text = "AE08L/C: LETTER OF CREDIT BANKING"},
        };

        public List<SelectListItem> FreightTerm { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "all", Text = "All"},
             new SelectListItem { Value = "collect", Text = "COLLECT"},
              new SelectListItem { Value = "prepaid", Text = "PREPAID"}
        };

        public List<SelectListItem> Detail { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "showVolumeDetail", Text = "Show Volume Detail"},
            new SelectListItem { Value = "showProfitDetail", Text = "Show Profit Detail"},
            new SelectListItem { Value = "showContainerDetail", Text = "Show Container Detail"},
            new SelectListItem { Value = "volumeByContainerTypeAndSaleType", Text = "Volume By Container Type And Sales Type"},

        };

        public List<SelectListItem> Sales { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "all", Text = "Steve"},
             new SelectListItem { Value = "range", Text = "Tang"},
              new SelectListItem { Value = "negative", Text = "David"}
        };

        public List<SelectListItem> OutputType { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "all", Text = "ABBOTT, BROWN AND GARCIA"},
             new SelectListItem { Value = "range", Text = "ABBOTT, BROWN AND MASON"},
              new SelectListItem { Value = "negative", Text = "ABBOTT, DIAZ AND GREEN"}
        };

        public List<SelectListItem> ServiceTermStart { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "all", Text = "AIRPORT"},
             new SelectListItem { Value = "bt", Text = "BT"},
              new SelectListItem { Value = "CFS", Text = "CFS"},
              new SelectListItem { Value = "cy", Text = "CY"},
              new SelectListItem { Value = "door", Text = "DOOR"},
              new SelectListItem { Value = "fi", Text = "FI"},
              new SelectListItem { Value = "fo", Text = "FO"},
              new SelectListItem { Value = "fot", Text = "FOT"},
              new SelectListItem { Value = "tackle", Text = "TACKLE"}
        };

        public List<SelectListItem> ServiceTermEnd { get; set; } = new List<SelectListItem>
        {
           new SelectListItem { Value = "all", Text = "AIRPORT"},
             new SelectListItem { Value = "bt", Text = "BT"},
              new SelectListItem { Value = "CFS", Text = "CFS"},
              new SelectListItem { Value = "cy", Text = "CY"},
              new SelectListItem { Value = "door", Text = "DOOR"},
              new SelectListItem { Value = "fi", Text = "FI"},
              new SelectListItem { Value = "fo", Text = "FO"},
              new SelectListItem { Value = "fot", Text = "FOT"},
              new SelectListItem { Value = "tackle", Text = "TACKLE"}
        };

        public List<SelectListItem> Status { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "all", Text = "All"},
             new SelectListItem { Value = "open", Text = "Open"},
              new SelectListItem { Value = "blocked", Text = "Blocked"}
        };

        public List<SelectListItem> SalesType { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "na", Text = "N/A"},
             new SelectListItem { Value = "co-load", Text = "CO-LOAD"},
              new SelectListItem { Value = "freecargo", Text = "FREE CARGO"},
              new SelectListItem { Value = "nomi", Text = "NOMI"},
        };

        public List<SelectListItem> InvoiceType { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "normal-only", Text = "Normal Only"},
             new SelectListItem { Value = "normal-draft", Text = "Normal + Draft"}
        };

        public List<SelectListItem> ECommerce { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "yes", Text = "YES"},
             new SelectListItem { Value = "no", Text = "NO"}
        };

     


    }
}
