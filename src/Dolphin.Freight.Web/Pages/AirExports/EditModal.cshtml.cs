using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using static Dolphin.Freight.Web.Pages.AirExports.CreateMawbModel;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.AirExports;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using System.Security.Cryptography;
using Volo.Abp.ObjectMapping;
using System.Security.Cryptography.Xml;
using Dolphin.Freight.Settings.Countries;
using Volo.Abp.Uow;
using System.Threading;
using Newtonsoft.Json;
using Dolphin.Freight.Accounting;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.AirImports;

namespace Dolphin.Freight.Web.Pages.AirExports
{
    public class EditModalModel : FreightPageModel
    {
        public ILogger<CreateMawbModel> Logger { get; set; }
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IAirExportMawbAppService _airExportMawbAppService;
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        private readonly IAirExportHawbAppService _airExportHawbAppService;
        private readonly ICountryDisplayNameAppService _countryDisplayNameAppService;
        private readonly ICountryAppService _countryAppService;
        [BindProperty]
       public List<SelectListItem> EnumList { get; set; }

        public EditModalModel(ITradePartnerAppService tradePartnerAppService,
            ISubstationAppService substationAppService,
            IAirportAppService airportAppService,
            IPackageUnitAppService packageUnitAppService,
            IAirExportMawbAppService airExportMawbAppService,
            IAirExportHawbAppService airExportHawbAppService,
            ICountryDisplayNameAppService countryDisplayNameAppService,
            ICountryAppService countryAppService,
            IAirImportMawbAppService airImportMawbAppService
            )
        {
            Logger = NullLogger<CreateMawbModel>.Instance;
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airExportMawbAppService = airExportMawbAppService;
            _airExportHawbAppService = airExportHawbAppService;
            _countryDisplayNameAppService = countryDisplayNameAppService;
            _countryAppService = countryAppService;
            _airImportMawbAppService= airImportMawbAppService;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ShowMsg { get; set; } = false;
        [BindProperty(SupportsGet = true)]
        public Guid ResultedMawbId { get; set; }
        [BindProperty]
        public string DimensionsJSON { get; set; }
        [BindProperty]
        public bool ISToolTipShow { get; set; }
        [BindProperty]
        public List<Dimension> Dimensions { get; set; }
        [BindProperty]
        public List<MoreInformation> MoreInformations { get; set; }
        [BindProperty]
        public MoreInformation MoreInformationPrepaid { get; set; }
        [BindProperty]
        public MoreInformation MoreInformationCollect { get; set; }
        [BindProperty]
        public List<AccountingInformation> AccountingInformation { get; set; }
        public List<SelectListItem> ChargeItemList { get; set; }
        public Dictionary<Guid, string> DepartureDictionary { get; set; } = new Dictionary<Guid, string>();
        private static readonly Object lockObject = new object();
        public async Task OnGetAsync(Guid Id)
        {
            AirExportMawbDto = await _airExportMawbAppService.GetAsync(Id);
            
            if (AirExportMawbDto.RouteTrans1ArrivalDate == DateTime.MinValue)
            {
                AirExportMawbDto.RouteTrans1ArrivalDate = (DateTime?)null;
            }
            if (AirExportMawbDto.RouteTrans1DepatureDate == DateTime.MinValue)
            {
                AirExportMawbDto.RouteTrans1DepatureDate = (DateTime?)null;
            }
            if (AirExportMawbDto.RouteTrans2ArrivalDate == DateTime.MinValue)
            {
                AirExportMawbDto.RouteTrans2ArrivalDate = (DateTime?)null;
            }
            if (AirExportMawbDto.RouteTrans2DepatureDate == DateTime.MinValue)
            {
                AirExportMawbDto.RouteTrans2DepatureDate = (DateTime?)null;
            }
            if (AirExportMawbDto.RouteTrans3ArrivalDate == DateTime.MinValue)
            {
                AirExportMawbDto.RouteTrans3ArrivalDate = (DateTime?)null;
            }
            if (AirExportMawbDto.RouteTrans3DepatureDate == DateTime.MinValue)
            {
                AirExportMawbDto.RouteTrans3DepatureDate = (DateTime?)null;
            }
            //AirExportHawbDto = new AirExportHawbDto();
            /*AirExportHawbDto = await _airExportHawbAppService.GetHblCardsById(Id);*/
            //if (AirExportMawbDto.ExtraProperties != null)
            //{


            //     var extraproperty=AirExportMawbDto.ExtraProperties;

            //    var json = AirExportMawbDto.ExtraProperties.Where(x => x.Key == "MoreInformation").Select(x => x.Value).ToList();

            //    MoreInformations = JsonConvert.DeserializeObject<List<MoreInformation>>(json.ToString());

            //}
            EnumList = GetIncotermsSelectList();
            ChargeItemList = GetChargeItemSelectList();
            await FillTradePartnerAsync();
            await FillSubstationAsync();
            await FillAirportAsync();
            FillWtValOther();
            FillOther();
            await FillPackageUnitAsync();
            await FillCountryNameAsync();
            ISToolTipShow = await _airImportMawbAppService.GetCardSettings();
        }

        public IActionResult OnPostAsync()
        {
            var updateItem = ObjectMapper.Map<AirExportMawbDto, CreateUpdateAirExportMawbDto>(AirExportMawbDto);
            
            if (updateItem.ExtraProperties == null)
            {
                updateItem.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
            }
            if (AirExportMawbDto.Commodities != null )
            {
                updateItem.ExtraProperties.Remove("Commodites");
                updateItem.ExtraProperties.Add("Commodities", AirExportMawbDto.Commodities);

            }
            if (AirExportMawbDto.OtherCharges != null)
            {
                updateItem.ExtraProperties.Remove("OtherCharges");
                updateItem.ExtraProperties.Add("OtherCharges", AirExportMawbDto.OtherCharges);

            }
            if (AccountingInformation != null)
            {
                updateItem.ExtraProperties.Remove("AccountingInformation");
                updateItem.ExtraProperties.Add("AccountingInformation", AccountingInformation);
            }
            if (MoreInformationPrepaid != null || MoreInformationCollect != null)
            {
                MoreInformations.Add(MoreInformationPrepaid);
                MoreInformations.Add(MoreInformationCollect);

                updateItem.ExtraProperties.Remove("MoreInformation");
                updateItem.ExtraProperties.Add("MoreInformation", MoreInformations);

            }
            if (DimensionsJSON is not null) {
                Dimensions = JsonConvert.DeserializeObject<List<Dimension>>(DimensionsJSON);

                updateItem.ExtraProperties.Remove("Dimensions");
                updateItem.ExtraProperties.Add("Dimensions", Dimensions);
            }
            _airExportMawbAppService.UpdateAsync(AirExportMawbDto.Id, updateItem).Wait();

            if (AirExportHawbDto is not null && !string.IsNullOrEmpty(AirExportHawbDto.HawbNo))
            {
                var updateHawb = ObjectMapper.Map<AirExportHawbDto, CreateUpdateAirExportHawbDto>(AirExportHawbDto);
                updateHawb.MawbId = AirExportMawbDto.Id;
                bool isUpdate = AirExportHawbDto.Id != Guid.Empty;

                if (updateHawb.ExtraProperties == null)
                {
                    updateHawb.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                }

                if (AirExportHawbDto.Commodities != null)
                {
                    updateHawb.ExtraProperties.Remove("Commodites");
                    updateHawb.ExtraProperties.Add("Commodities", AirExportHawbDto.Commodities);
                }
                

                if (AirExportHawbDto.OtherCharges != null)
                {
                    updateHawb.ExtraProperties.Remove("OtherCharges");
                    updateHawb.ExtraProperties.Add("OtherCharges", AirExportHawbDto.OtherCharges);
                }

                if (string.IsNullOrEmpty(AirExportHawbDto.HawbNo) || AirExportHawbDto.HawbNo == "0")
                {
                    AirExportHawbDto.Id = Guid.NewGuid();
                    isUpdate = false;
                }

                if (AirExportHawbDto.HawbDimensionsJSON is not null)
                {
                    Dimensions = JsonConvert.DeserializeObject<List<Dimension>>(AirExportHawbDto.HawbDimensionsJSON);

                    updateHawb.ExtraProperties.Remove("Dimensions");
                    updateHawb.ExtraProperties.Add("Dimensions", Dimensions);
                }

                if (isUpdate)
                {
                    _airExportHawbAppService.UpdateAsync(AirExportHawbDto.Id, updateHawb).Wait();
                }
                else
                {
                    _airExportHawbAppService.CreateAsync(updateHawb).Wait();
                }

            }

            return new ObjectResult(new { id = AirExportMawbDto.Id });
        }
      
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }
        public List<SelectListItem> OtherList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        public List<SelectListItem> CountryName { get; set; }

        [BindProperty]
        public AirExportMawbDto AirExportMawbDto { get; set; }

        [BindProperty]
        public AirExportHawbDto AirExportHawbDto { get; set; }

        #region FillCountryNameAsync()
        private async Task FillCountryNameAsync()
        {
            var countryName = await _tradePartnerAppService.GetCountriesLookupAsync();
            CountryName = countryName.Items
                                     .Select(x => new SelectListItem(x.CountryName, x.Id.ToString(), false))
                                     .ToList();
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

        #region FillWtVal()
        private void FillWtValOther()
        {
            WtValOtherList = new List<SelectListItem>
            {
                new SelectListItem { Value = "PPD", Text = L["PPD"] },
                new SelectListItem { Value = "COLL", Text = L["COLL"] }
            };
        }

        private void FillOther()
        {
            OtherList = new List<SelectListItem>
            {
                new SelectListItem { Value = "PPD", Text = L["PPD"] },
                new SelectListItem { Value = "COLL", Text = L["COLL"] }
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

        #region SetAirExportFileNo() 
        /// <summary>
        /// �]�wAir Export��File No
        /// </summary>
        private string SetAirExportFileNo()
        {
            string today = DateTime.Now.ToString("yyyyMMddhhmmss");
            string AEFileNo = "AXE-" + today;
            return AEFileNo;
        }
        #endregion
        private List<SelectListItem> GetIncotermsSelectList()
        {
            var enumValues = Enum.GetValues(typeof(AccountingInformationCode))
                                  .Cast<AccountingInformationCode>()
                                  .Select(e => new SelectListItem
                                  {
                                      Text =L["Enum:" + e.ToString()],
                                      Value = e.ToString(),
                                    
                                  })
                                  .ToList();

            return enumValues;
        }
        private List<SelectListItem> GetChargeItemSelectList()
        {
            var enumValues = Enum.GetValues(typeof(ChargeItems))
                                  .Cast<ChargeItems>()
                                  .Select(e => new SelectListItem
                                  {
                                      Text = L["Enum:ItemCharge." + e.ToString()],
                                      Value = e.ToString(),

                                  })
                                  .ToList();

            return enumValues;
        }
    }
}
