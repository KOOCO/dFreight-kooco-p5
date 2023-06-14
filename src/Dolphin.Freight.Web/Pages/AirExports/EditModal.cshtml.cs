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
        private readonly IAirExportHawbAppService _airExportHawbAppService;

        public EditModalModel(ITradePartnerAppService tradePartnerAppService,
            ISubstationAppService substationAppService,
            IAirportAppService airportAppService,
            IPackageUnitAppService packageUnitAppService,
            IAirExportMawbAppService airExportMawbAppService,
            IAirExportHawbAppService airExportHawbAppService
            )
        {
            Logger = NullLogger<CreateMawbModel>.Instance;
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airExportMawbAppService = airExportMawbAppService;
            _airExportHawbAppService = airExportHawbAppService;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public async Task OnGetAsync(Guid Id)
        {
            AirExportMawbDto = await _airExportMawbAppService.GetAsync(Id);
            AirExportHawbDto = new AirExportHawbDto();
            /*AirExportHawbDto = await _airExportHawbAppService.GetHblCardsById(Id);*/

            await FillTradePartnerAsync();
            await FillSubstationAsync();
            await FillAirportAsync();
            FillWtValOther();
            await FillPackageUnitAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var updateItem = ObjectMapper.Map<AirExportMawbDto, CreateUpdateAirExportMawbDto>(AirExportMawbDto);

            await _airExportMawbAppService.UpdateAsync(AirExportMawbDto.Id, updateItem);

            if(AirExportHawbDto is not null)
            {
                var updateHawb = ObjectMapper.Map<AirExportHawbDto, CreateUpdateAirExportHawbDto>(AirExportHawbDto);
                updateHawb.MawbId = AirExportMawbDto.Id;

                if (AirExportHawbDto.Id != Guid.Empty)
                {
                    await _airExportHawbAppService.UpdateAsync(AirExportHawbDto.Id, updateHawb);
                }
                else
                {
                    await _airExportHawbAppService.CreateAsync(updateHawb);
                }
            }

            return new ObjectResult(new { id = AirExportMawbDto.Id });
        }
      
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }

        [BindProperty]
        public AirExportMawbDto AirExportMawbDto { get; set; }

        [BindProperty]
        public AirExportHawbDto AirExportHawbDto { get; set; }

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

    }
}
