using Dolphin.Freight.AirExports;
using Dolphin.Freight.AirImports;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.Web.Pages.AirImports
{
    public class CreateMawbModel : AbpPageModel
    {
        public Guid? MawbId { get; set; }
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        private readonly IAirImportHawbAppService _airImportHawbAppService;

        [BindProperty]
        public CreateAIMMawbViewModel MawbModel { get; set; }
        [BindProperty]
        public AirImportHawbDto HawbModel { get; set; }

        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }

        public CreateMawbModel(ITradePartnerAppService tradePartnerAppService,
            ISubstationAppService substationAppService,
            IAirportAppService airportAppService,
            IPackageUnitAppService packageUnitAppService,
            IAirImportMawbAppService airImportMawbAppService,
            IAirImportHawbAppService airImportHawbAppService
            )
        {
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airImportMawbAppService = airImportMawbAppService;
            _airImportHawbAppService = airImportHawbAppService;
        }


        #region OnGetAsync()
        public async Task OnGetAsync()
        {
            MawbModel = new CreateAIMMawbViewModel
            {
                // set default value
                AwbType = AWBType.Normal,
                ServiceTermTypeFrom = ServiceTermType.Airport,
                ServiceTermTypeTo = ServiceTermType.Airport,
                DisplayUnit = DisplayUnitType.Both
            };

            await FillTradePartnerAsync();
            await FillSubstationAsync();
            await FillAirportAsync();
            await FillPackageUnitAsync();
        }
        #endregion

        #region OnPostAsync()
        public async Task<IActionResult> OnPostAsync()
        {
            MawbModel.FilingNo = SetAirImportFileNo();
            MawbModel.PostDate = Clock.Now.ClearTime();
            var inputDto = await _airImportMawbAppService.CreateAsync(
                   ObjectMapper.Map<CreateAIMMawbViewModel, CreateUpdateAirImportMawbDto>(MawbModel)
                );
            
            MawbId = inputDto.Id;

            if (HawbModel is not null && !string.IsNullOrEmpty(HawbModel.HawbNo))
            {
                var addHawb = ObjectMapper.Map<AirImportHawbDto, CreateUpdateAirImportHawbDto>(HawbModel);
                addHawb.MawbId = MawbId;

                await _airImportHawbAppService.CreateAsync(addHawb);
            }

            Dictionary<string, Guid> rs = new()
            {
                { "id", MawbId.Value }
            };
            return new JsonResult(rs);
        }
        #endregion

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

        #region FillPackageUnitAsync()
        private async Task FillPackageUnitAsync()
        {
            var packageUnitLookup = await _packageUnitAppService.GetPackageUnitsLookupAsync();
            PackageUnitLookupList = packageUnitLookup.Items
                                                .Select(x => new SelectListItem(x.PackageName, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region SetAirImportFileNo() 
        /// <summary>
        /// ³]©wAir ImportªºFile No
        /// </summary>
        private string SetAirImportFileNo()
        {
            string today = DateTime.Now.ToString("yyyyMMddhhmmss");
            string AIFileNo = "AIM-" + today;
            return AIFileNo;
        }
        #endregion





        #region CreateMawbViewModel
        public class CreateAIMMawbViewModel
        {
            [HiddenInput]
            [BindProperty(SupportsGet = true)]
            public Guid Id { get; set; }
            public string FilingNo { get; set; }
            [Required]
            [MaxLength(12)]
            [RegularExpression("^[0-9]{3}-[0-9]{1,8}$", ErrorMessage = "Invalid Mawb No.")]
            public string MawbNo { get; set; }
            [Required]
            [SelectItems(nameof(SubstationLookupList))]
            public String OfficeId { get; set; }
            
            public AWBType AwbType { get; set; }
            
            [DataType(DataType.Date)]
            public DateTime? PostDate { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String OverseaAgentId { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String CarrierId { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String? AwbAcctCarrierId { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String? CoLoaderId { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String OPId { get; set; }
            
            public bool IsDirectMaster { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String ShipperId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String ConsigneeId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String NotifyId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String CustomerId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String BillToId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String SalesId { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String DepatureId { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime? DepatureDate { get; set; }
            
            public string FlightNo { get; set; }

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

            public string FPOEDepature { get; set; }
            
            public string FPOETrans1 { get; set; }
            
            public string FPOETrans2 { get; set; }
            
            public string FPOETrans3 { get; set; }
            
            public string FPOEDestination { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String DestinationId { get; set; }

            [Required]
            [DataType(DataType.DateTime)]
            public DateTime? ArrivalDate { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String FreightLocationId { get; set; }
            
            [DataType(DataType.Date)]
            public DateTime? StorageStartDate { get; set; }
            
            public double Package { get; set; }

            [SelectItems(nameof(PackageUnitLookupList))]
            public String MawbPackageUnitId { get; set; }
            
            public double GrossWeightKg { get; set; }
            
            public double GrossWeightLb { get; set; }
            
            public double ChargeableWeightKg { get; set; }
            
            public double ChargeableWeightLb { get; set; }
            
            public double VolumeWeightKg { get; set; }
            
            public double VolumeWeightCbm { get; set; }
            
            public FreightType? FreightType { get; set; }
            
            public IncotermsType? IncotermsType { get; set; }
            
            public ServiceTermType ServiceTermTypeFrom { get; set; }
            
            public ServiceTermType ServiceTermTypeTo { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String BusinessReferredId { get; set; }
            
            public bool IsECom { get; set; }
            
            public DisplayUnitType DisplayUnit { get; set; }
        }
        #endregion
    }
}
