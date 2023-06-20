using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.Web.ViewModels.CertificateOfOrigin;
using Dolphin.Freight.Web.ViewModels.DockRecepit;
using Dolphin.Freight.Web.ViewModels.ForwarderCargoReceipt;
using Dolphin.Freight.Web.ViewModels.HblClauses;
using Dolphin.Freight.Web.ViewModels.PreAlert;
using Dolphin.Freight.Web.ViewModels.UsdaHeatTreatment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Wkhtmltopdf.NetCore;
using static Dolphin.Freight.Permissions.SettingsPermissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Dolphin.Freight.Web.ViewModels.Reports;
using Dolphin.Freight.Web.ViewModels.Hbl;
using Volo.Abp.ObjectMapping;
using Dolphin.Freight.ImportExport.OceanImports;
using static Dolphin.Freight.Permissions.OceanExportPermissions;
using QueryHblDto = Dolphin.Freight.ImportExport.OceanExports.QueryHblDto;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.ImportExport.AirExports;

namespace Dolphin.Freight.Web.Pages.OceanExports
{
    public class EditModalModel : FreightPageModel
    {

        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ShowMsg { get; set; } = false;

        [BindProperty(SupportsGet = true)]
        public Guid? Hid { get; set; }
        public bool IsShowHbl { get; set; } = false;
        [BindProperty(SupportsGet = true)]
        public int NewHbl { get; set; } = 0;
        [BindProperty]
        public int AddHbl { get; set; } = 0;
        [BindProperty]
        public string CardClass { get; set; }
        [BindProperty]
        public CreateUpdateOceanExportMblDto OceanExportMbl { get; set; }
        [BindProperty]
        public OceanExportMblDto OceanExportMblDto { get; set; }
        [BindProperty]
        public OceanExportHblDto OceanExportHblDto { get; set; }
        [BindProperty]
        public CreateUpdateOceanExportHblDto OceanExportHbl { get; set; }
        public IList<OceanExportHblDto> OceanExportHbls { get; set; }
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IGeneratePdf _generatePdf;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        public EditModalModel(IOceanExportMblAppService oceanExportMblAppService, IOceanExportHblAppService oceanExportHblAppService, ISysCodeAppService sysCodeAppService, IGeneratePdf generatePdf, ITradePartnerAppService tradePartnerAppService, ISubstationAppService substationAppService,
            IAirportAppService airportAppService, IPackageUnitAppService packageUnitAppService)
        {
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _sysCodeAppService = sysCodeAppService;
            _generatePdf = generatePdf;
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
        }

        public async Task OnGetAsync()
        {
            ViewData["HAVEHBL"] = "N";
            OceanExportMbl = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(Id);
            QueryHblDto query = new QueryHblDto() { MblId = Id };

            await FillTradePartnerAsync();
            await FillAirportAsync();
            await FillPackageUnitAsync();
            await FillSubstationAsync();
            FillWtValOther();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _oceanExportMblAppService.UpdateAsync(OceanExportMbl.Id, OceanExportMbl);

            if (OceanExportHbl is not null)
            {
                OceanExportHbl.MblId = OceanExportMblDto.Id;

                if (OceanExportHbl.Id != Guid.Empty)
                {
                    await _oceanExportHblAppService.UpdateAsync(OceanExportHbl.Id, OceanExportHbl);
                }
                else
                {
                    await _oceanExportHblAppService.CreateAsync(OceanExportHbl);
                }
            }

            return new ObjectResult(new { id = OceanExportMblDto.Id });
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

        #region FillPackageUnitAsync()
        private async Task FillPackageUnitAsync()
        {
            var packageUnitLookup = await _packageUnitAppService.GetPackageUnitsLookupAsync();
            PackageUnitLookupList = packageUnitLookup.Items
                                                .Select(x => new SelectListItem(x.PackageName, x.Id.ToString(), false))
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

    }
}
