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
using Dolphin.Freight.Web.CommonService;

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
        private readonly IDropdownService _dropdownService;

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
            IAirExportMawbAppService airExportMawbAppService,
            IDropdownService dropdownService
            )
        {
            Logger = NullLogger<CreateMawbModel>.Instance;
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airExportMawbAppService = airExportMawbAppService;
            _dropdownService = dropdownService;
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
                new SelectListItem { Value = "PPD", Text = L["PPD"]},
                new SelectListItem { Value = "COLL", Text = L["COLL"]}
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
            public string ServiceTermTypeFrom { get; set; }
            [DisplayName("Service Term End")]
            public string ServiceTermTypeTo { get; set; }
            public string Status { get; set; }
            [DisplayName("Sales Type")]
            public string SalesTypeId { get; set; }
            [DisplayName("Invoice Type")]
            public string InvoiceType { get; set; }
            [DisplayName("E-Commerce")]
            public string IsEcommerce { get; set; }
            [DisplayName("Output By")]
            public string OutputBy { get; set; }

            public decimal? MinProfit { get; set; }
            
            public decimal? MaxProfit { get; set; }

            /// <summary>
            /// OUTPUT Options
            /// </summary>
            public bool IsShipper { get; set; }
            public bool IsOverseaAgent { get; set; }
            public bool IsConsignee { get; set; }
            public bool IsCustomer { get; set; }
            public bool IsCarrier { get; set; }
            public bool IsCustomsBroker { get; set; }
            public bool IsTrucker { get; set; }
            public bool IsAccountGroup { get; set; }
            public bool IsBillTo { get; set; }
            public bool IsReferredBy { get; set; }
            public bool IsOutputOffice { get; set; }
            public bool IsBLPostDate { get; set; }
            public bool IsETD { get; set; }
            public bool IsETA { get; set; }
            public bool IsOutputFreightTerm { get; set; }
            public bool IsIncoterms { get; set; }
            public bool IsServiceTerm { get; set; }
            public bool IsCargoType { get; set; }
            public bool IsSalesPerson { get; set; }
            public bool IsMBLOP { get; set; }
            public bool IsOperation { get; set; }
            public bool IsOPCOOPOP { get; set; }
            public bool IsShipLine { get; set; }
            public bool IsPOL { get; set; }
            public bool IsPOD { get; set; }
            public bool IsCountryOfPOL { get; set; }
            public bool IsCountryOfPOD { get; set; }
            public bool IsDEL { get; set; }
            public bool IsFinalDestination { get; set; }
            public bool IsVesselFlight { get; set; }
            public bool IsMblMawbWarehouse { get; set; }
            public bool IsHblHawb { get; set; }
            public bool IsOutputFile { get; set; }
            public bool IsDoorMove { get; set; }
            public bool IsClearance { get; set; }
            public bool IsISF { get; set; }
            public bool IsFBAFC { get; set; }
            public bool IsOutputSalesType { get; set; }
            public bool IsHblNominatedAgent { get; set; }
            public bool IsOutputECommerce { get; set; }
            public bool IsForwardingAgent { get; set; }
            public bool IsCarrierContractNo { get; set; }
            public bool IsMblColorRemark { get; set; }
            public bool IsHblColorRemark { get; set; }
            public bool IsCoLoader { get; set; }
            public bool IsBlType { get; set; }
            public bool IsLatestGateIn { get; set; }


            public bool IsOceanImport { get; set; }
            public bool IsOceanExport { get; set; }
            public bool IsAirImport { get; set; }
            public bool IsAirExport { get; set; }
            public bool IsTruck { get; set; }
            public bool IsMisc { get; set; }
            public bool IsWarehouse { get; set; }
            public bool IsVolumeDetail { get; set; }
            public bool IsProfitDetail { get; set; }
            public bool IsContainerDetail { get; set; }
            public bool IsVolByCntrAndSalesType { get; set; }
            public bool IsNA { get; set; }
            public bool IsCOLoad { get; set; }
            public bool IsFreeCargo { get; set; }
            public bool IsNomi { get; set; }

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
            new SelectListItem { Value = "D7972F83-91D9-9F5D-D038-3A0ADA6A3515", Text = "Sub 1"}

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
            new SelectListItem { Value = "03322EAC-7369-95F1-39B1-3A0AEF20C399", Text = "AE01A/F: AIR FREIGHT CHARGE"},
            new SelectListItem { Value = "6ECBE3BE-AC03-B396-06F2-3A0AEF20C399", Text = "AE02FSC: FUEL SUR CHARGE"},
            new SelectListItem { Value = "8CB45D76-81FD-2029-AFEE-3A0AEF20C399", Text = "AE03SECURI: SECURITY CHARGE"},
            new SelectListItem { Value = "7ECCFA7D-7D05-3BCE-6717-3A0AEF20C399", Text = "AE04H/C: HANDLING CHARGE"},
            new SelectListItem { Value = "4D95CCA0-30BA-99EA-F050-3A0AEF20C399", Text = "AE05DG: DANGEROUS GOOD HANDLING"},
            new SelectListItem { Value = "4BEDC2EE-D6ED-1D78-3BD9-3A0AEF20C399", Text = "AE06TRUCKI: TRUCKING CHARGE"},
            new SelectListItem { Value = "5F698F8B-4E35-3DC9-6324-3A0AEF20C399", Text = "AE07WHSETR: WAREHOUSE TRANSFER FEE"},
            new SelectListItem { Value = "B3DBCDA5-A433-5C8B-BBAC-3A0AEF20C399", Text = "AE08L/C: LETTER OF CREDIT BANKING"},
            new SelectListItem { Value = "B50C9F44-FD59-7968-6CA1-3A0AEF20C399", Text = "AE09C/O: CERTIFICATE OF ORIGIN"},
            new SelectListItem { Value = "91E7D784-73FE-BFB7-0937-3A0AEF20C399", Text = "AE10INS: INSURANCE PREMIUM"},
            new SelectListItem { Value = "708B7168-953D-887C-30BF-3A0AEF20C399", Text = "AE12MSG: SPECIAL MESSENGER FEE"},
            new SelectListItem { Value = "C703FAEC-F4A7-DAE0-1746-3A0AEF20C399", Text = "AE13OTHER: OTHER CHARGES"},
            new SelectListItem { Value = "8D47A13C-CDB1-3DC9-04C3-3A0AEF20C399", Text = "AE14COMM: COMMISSION FEE"},
            new SelectListItem { Value = "C2FA41EC-EE02-0248-C7A8-3A0AEF20C399", Text = "AEDEST: DESTINATION CHARGES"},
            new SelectListItem { Value = "03322EAC-7369-95F1-39B1-3A0AEF20C399", Text = "AI01A/F: AIR FREIGHT CHARGE"},
            new SelectListItem { Value = "FF7AEE18-A039-0AE7-5B4E-3A0AEF20C399", Text = "AI02H/C: HANDLING CHARGE"},
            new SelectListItem { Value = "E004439C-2CC5-3847-9ACF-3A0AEF20C399", Text = "AI03IMPORT: IMPORT SERVICE FEE TO AIRLINE"},
            new SelectListItem { Value = "1ACB35D9-94A4-191A-7851-3A0AEF20C399", Text = "AI04TRUCKI: TRUCKING CHARGE"},
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
            //new SelectListItem { Value = "all", Text = "ABBOTT, BROWN AND GARCIA"},
            //new SelectListItem { Value = "range", Text = "ABBOTT, BROWN AND MASON"},
            //new SelectListItem { Value = "negative", Text = "ABBOTT, DIAZ AND GREEN"}

            new SelectListItem { Value = "Shipper", Text = "Shipper"},
            new SelectListItem { Value = "OverseaAgent", Text = "Oversea Agent"},
            new SelectListItem { Value = "Consignee", Text = "Consignee"},
            new SelectListItem { Value = "Customer", Text = "Customer"},
            new SelectListItem { Value = "Carrier", Text = "Carrier"},
            new SelectListItem { Value = "CustomsBroker", Text = "Customs Broker"},
            new SelectListItem { Value = "Trucker", Text = "Trucker"},
            new SelectListItem { Value = "AccountGroup", Text = "Account Group"},
            new SelectListItem { Value = "BillTo", Text = "Bill To"},
            new SelectListItem { Value = "ReferredBy", Text = "Referred By"},
            new SelectListItem { Value = "Office", Text = "Office"},
            new SelectListItem { Value = "BLPostDate", Text = "BL Post Date"},
            new SelectListItem { Value = "ETD", Text = "ETD"},
            new SelectListItem { Value = "ΕΤΑ", Text = "ΕΤΑ"},
            new SelectListItem { Value = "FreightTerm", Text = "Freight Term"},
            new SelectListItem { Value = "Incoterms", Text = "Incoterms"},
            new SelectListItem { Value = "ServiceTerm", Text = "Service Term"},
            new SelectListItem { Value = "CargoType", Text = "Cargo Type"},
            new SelectListItem { Value = "SalesPerson", Text = "Sales Person"},
            new SelectListItem { Value = "MB/LOP", Text = "MB/L OP"},
            new SelectListItem { Value = "Operation", Text = "Operation"},
            new SelectListItem { Value = "OP/Co-opOP", Text = "OP/Co-op OP"},
            new SelectListItem { Value = "ShipLine", Text = "Ship Line"},
            new SelectListItem { Value = "POL", Text = "POL"},
            new SelectListItem { Value = "POD", Text = "POD"},
            new SelectListItem { Value = "CountryofPOL", Text = "Country of POL"},
            new SelectListItem { Value = "CountryofPOD", Text = "Country of POD"},
            new SelectListItem { Value = "DEL", Text = "DEL"},
            new SelectListItem { Value = "FinalDestination", Text = "Final Destination"},
            new SelectListItem { Value = "Vessel/Flight", Text = "Vessel / Flight"},
            new SelectListItem { Value = "MBL/MAWB No./WarehouseB/LNo.", Text = "MBL/MAWB No./ Warehouse B/L No."},
            new SelectListItem { Value = "HB/L|HAWB No.", Text = "HB/L | HAWB No."},
            new SelectListItem { Value = "FileNo.", Text = "File No."},
            new SelectListItem { Value = "DoorMove", Text = "Door Move"},
            new SelectListItem { Value = "C.Clearance", Text = "C.Clearance"},
            new SelectListItem { Value = "ISF No.", Text = "ISF No."},
            new SelectListItem { Value = "FBAFC", Text = "FBA FC"},
            new SelectListItem { Value = "SalesType", Text = "Sales Type"},
            new SelectListItem { Value = "HB/LReferredBy/NominatedAgent", Text = "HB/L Referred By / Nominated Agent"},
            new SelectListItem { Value = "E-Commerce", Text = "E-Commerce"},
            new SelectListItem { Value = "ForwardingAgent", Text = "Forwarding Agent"},
            new SelectListItem { Value = "CarrierContractNo.", Text = "Carrier Contract No."},
            new SelectListItem { Value = "MBLColorRemark", Text = "MBL Color Remark"},
            new SelectListItem { Value = "HBLColorRemark", Text = "HBL Color Remark"},
            new SelectListItem { Value = "Co-Loader", Text = "Co-Loader"},
            new SelectListItem { Value = "B/LType", Text = "B/L Type"},
            new SelectListItem { Value = "LatestGateIn", Text = "Latest Gate In"}

        };

        public List<SelectListItem> ServiceTermStart { get; set; } = new List<SelectListItem>
        {
            new SelectListItem {Value = "0", Text = "Select"},
            new SelectListItem { Value = "1", Text = "AIRPORT"},
             new SelectListItem { Value = "2", Text = "BT"},
              new SelectListItem { Value = "3", Text = "CFS"},
              new SelectListItem { Value = "4", Text = "CY"},
              new SelectListItem { Value = "5", Text = "DOOR"},
              new SelectListItem { Value = "6", Text = "FI"},
              new SelectListItem { Value = "7", Text = "FO"},
              new SelectListItem { Value = "8", Text = "FOT"},
              new SelectListItem { Value = "9", Text = "TACKLE"}
        };

        public List<SelectListItem> ServiceTermEnd { get; set; } = new List<SelectListItem>
        {
           new SelectListItem { Value = "1", Text = "AIRPORT"},
             new SelectListItem { Value = "2", Text = "BT"},
              new SelectListItem { Value = "3", Text = "CFS"},
              new SelectListItem { Value = "4", Text = "CY"},
              new SelectListItem { Value = "5", Text = "DOOR"},
              new SelectListItem { Value = "6", Text = "FI"},
              new SelectListItem { Value = "7", Text = "FO"},
              new SelectListItem { Value = "8", Text = "FOT"},
              new SelectListItem { Value = "9", Text = "TACKLE"}
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
             new SelectListItem { Value = "no", Text = "NO", Selected = true}
        };


        

    }
}
