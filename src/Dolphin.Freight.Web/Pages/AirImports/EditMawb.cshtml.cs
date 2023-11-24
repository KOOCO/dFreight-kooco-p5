using Dolphin.Freight.AirExports;
using Dolphin.Freight.AirImports;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.Settings.Countries;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Data;

namespace Dolphin.Freight.Web.Pages.AirImports
{
    public class EditMawbModel : AbpPageModel
    {
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        private readonly IAirImportHawbAppService _airImportHawbAppService;
        private readonly ICountryAppService _countryAppService;

        // �Ψӱ���CreateMawb redirect�L�Ӫ���T
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ShowMsg { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid ResultedMawbId { get; set; }
        [BindProperty]
        public CreateAIMMawbViewModel MawbModel { get; set; }

        [BindProperty]
        public CreateUpdateAirImportHawbDto HawbModel { get; set; }
        [BindProperty]
        public string DimensionsJSON { get; set; }
        [BindProperty]
        public List<Dimension> Dimensions { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }

        public EditMawbModel(ITradePartnerAppService tradePartnerAppService,
            ISubstationAppService substationAppService,
            IAirportAppService airportAppService,
            IPackageUnitAppService packageUnitAppService,
            IAirImportMawbAppService airImportMawbAppService,
            IAirImportHawbAppService airImportHawbAppService,
             ICountryAppService countryAppService
            )
        {
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airImportMawbAppService = airImportMawbAppService;
            _airImportHawbAppService = airImportHawbAppService;
            _countryAppService= countryAppService;
        }

        #region OnGetAsync()
        public async Task<IActionResult> OnGetAsync(Guid Id)
        {
            Logger.LogDebug("Enter into onGetyAsync With id:" + Id);
            if (Id == Guid.Empty) 
            {
                return NotFound();
            }
            AirImportMawbDto airImportDto = await _airImportMawbAppService.GetAsync(Id);

            //HawbModel = new AirImportHawbDto();

            //HawbModel = await _airImportHawbAppService.GetHblById(Id);

            if (airImportDto == null)
            {
                return NotFound();
            }

            await FillTradePartnerAsync();
            await FillSubstationAsync();
            await FillAirportAsync();
            await FillPackageUnitAsync();

            MawbModel = ObjectMapper.Map<AirImportMawbDto, CreateAIMMawbViewModel>(airImportDto);
            
            MawbModel.DepatureDate = (MawbModel.DepatureDate == DateTime.MinValue) ? null : MawbModel.DepatureDate;
            MawbModel.StorageStartDate = (MawbModel.StorageStartDate == DateTime.MinValue) ? null : MawbModel.StorageStartDate;
            MawbModel.RouteTrans1ArrivalDate = (MawbModel.RouteTrans1ArrivalDate == DateTime.MinValue) ? null : MawbModel.RouteTrans1ArrivalDate;
            MawbModel.RouteTrans2ArrivalDate = (MawbModel.RouteTrans2ArrivalDate == DateTime.MinValue) ? null : MawbModel.RouteTrans2ArrivalDate;
            MawbModel.RouteTrans3ArrivalDate = (MawbModel.RouteTrans3ArrivalDate == DateTime.MinValue) ? null : MawbModel.RouteTrans3ArrivalDate;
            MawbModel.RouteTrans1DepatureDate = (MawbModel.RouteTrans1DepatureDate == DateTime.MinValue) ? null : MawbModel.RouteTrans1DepatureDate;
            MawbModel.RouteTrans2DepatureDate = (MawbModel.RouteTrans2DepatureDate == DateTime.MinValue) ? null : MawbModel.RouteTrans2DepatureDate;
            MawbModel.RouteTrans3DepatureDate = (MawbModel.RouteTrans3DepatureDate == DateTime.MinValue) ? null : MawbModel.RouteTrans3DepatureDate;

            return Page();
        }
        #endregion

        #region OnPostAsync()
        public async Task<IActionResult> OnPostAsync()
        {
            var updateItem = ObjectMapper.Map<CreateAIMMawbViewModel, CreateUpdateAirImportMawbDto>(MawbModel);

            if (DimensionsJSON is not null)
            {
                Dimensions = JsonConvert.DeserializeObject<List<Dimension>>(DimensionsJSON);

                updateItem.ExtraProperties.Remove("Dimensions");
                updateItem.ExtraProperties.Add("Dimensions", Dimensions);
            }

            await _airImportMawbAppService.UpdateAsync(updateItem.Id, updateItem);

            if (HawbModel is not null && HawbModel.Id != MawbModel.Id)
            {
                var updateHawb = HawbModel;
                //var updateHawb1 = ObjectMapper.Map<AirImportHawbDto, CreateUpdateAirImportHawbDto>(HawbModel);
                if (updateHawb.ExtraProperties == null)
                {
                    updateHawb.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                }

                if (updateHawb.Commodities != null)
                {
                    updateHawb.ExtraProperties.Add("Commodities", updateHawb.Commodities);
                }

                if (updateHawb.SubHawbs != null)
                {
                    updateHawb.ExtraProperties.Add("SubHawbs", updateHawb.SubHawbs);
                }

                if (updateHawb.HawbDimensionsJSON is not null)
                {
                    Dimensions = JsonConvert.DeserializeObject<List<Dimension>>(updateHawb.HawbDimensionsJSON);

                    updateHawb.ExtraProperties.Remove("Dimensios");
                    updateHawb.ExtraProperties.Add("Dimensions", Dimensions);
                }

                updateHawb.MawbId = updateItem.Id;
                if (updateHawb.Id != Guid.Empty)
                {
                    await _airImportHawbAppService.UpdateAsync(updateHawb.Id, updateHawb);
                }
                else
                {
                    await _airImportHawbAppService.CreateAsync(updateHawb);
                }
            }
            return new ObjectResult(new { id = MawbModel.Id });
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
            var airportLookup = await _countryAppService.GetCountryLookupAsync();
            AirportLookupList = airportLookup.Items
                                                .Select(x => new SelectListItem(x.Code + " " + x.CountryName, x.Id.ToString(), false))
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
        /// �]�wAir Import��File No
        /// </summary>
        private string SetAirImportFileNo()
        {
            string today = DateTime.Now.ToString("yyyyMMddhhmmss");
            string AIFileNo = "AIM-" + today;
            return AIFileNo;
        }
        #endregion

        #region CreateAIMMawbViewModel
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
            public DateTime? DepatureDate { get; set; }
            
            public string FlightNo { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String RouteTrans1Id { get; set; }
            public DateTime? RouteTrans1ArrivalDate { get; set; }
            public DateTime? RouteTrans1DepatureDate { get; set; }
            
            public string RouteTrans1FlightNo { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String RouteTrans1CarrierId { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String RouteTrans2Id { get; set; }
            public DateTime? RouteTrans2ArrivalDate { get; set; }
            public DateTime? RouteTrans2DepatureDate { get; set; }
            
            public string RouteTrans2FlightNo { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String RouteTrans2CarrierId { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String RouteTrans3Id { get; set; }
            public DateTime? RouteTrans3ArrivalDate { get; set; }
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
            public DateTime? ArrivalDate { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String FreightLocationId { get; set; }
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
            
            public ServiceTermType? ServiceTermTypeFrom { get; set; }
            
            public ServiceTermType? ServiceTermTypeTo { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String BusinessReferredId { get; set; }
            
            public bool IsECom { get; set; }
            public bool IsLocked { get; set; }
            public ExtraPropertyDictionary ExtraProperties { get; set; }
            public DisplayUnitType? DisplayUnit { get; set; }
        }
        #endregion
    }
}
