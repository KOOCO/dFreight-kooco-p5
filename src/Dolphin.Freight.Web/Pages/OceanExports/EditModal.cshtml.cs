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
using Dolphin.Freight.Settings.PortsManagement;

namespace Dolphin.Freight.Web.Pages.OceanExports
{
    public class EditModalModel : FreightPageModel
    {

        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }
        public List<SelectListItem> PortsManagementLookupList { get; set; }

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
        public EditModalModel(IOceanExportMblAppService oceanExportMblAppService, IOceanExportHblAppService oceanExportHblAppService)
        {
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
        }

        public async Task OnGetAsync()
        {
            ViewData["HAVEHBL"] = "N";
            OceanExportMbl = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(Id);
            QueryHblDto query = new QueryHblDto() { MblId = Id };
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _oceanExportMblAppService.UpdateAsync(OceanExportMbl.Id, OceanExportMbl);

            if (OceanExportHbl is not null && !string.IsNullOrEmpty(OceanExportHbl.HblNo))
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

    }
}
