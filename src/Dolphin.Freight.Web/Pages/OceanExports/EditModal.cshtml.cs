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

namespace Dolphin.Freight.Web.Pages.OceanExports
{
    public class EditModalModel : FreightPageModel
    {
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
        public EditModalModel(IOceanExportMblAppService oceanExportMblAppService, IOceanExportHblAppService oceanExportHblAppService, ISysCodeAppService sysCodeAppService, IGeneratePdf generatePdf)
        {
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _sysCodeAppService = sysCodeAppService;
            _generatePdf = generatePdf;
        }

        public async Task OnGetAsync()
        {
            ViewData["HAVEHBL"] = "N";
            OceanExportMbl = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(Id);
            QueryHblDto query = new QueryHblDto() { MblId = Id };
            //OceanExportHbls = await _oceanExportHblAppService.QueryListByMidAsync(query);
            //if (Hid == null)
            //{
            //    if (NewHbl == 1)
            //    {
            //        OceanExportHbl = new CreateUpdateOceanExportHblDto();
            //        QueryDto cquery = new QueryDto();
            //        cquery.QueryType = "CardColorId";
            //        var syscodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(cquery);
            //        if (OceanExportHbls != null && OceanExportHbls.Count > 0)
            //        {
            //            int index =  OceanExportHbls.Count % syscodes.Count ;
            //            OceanExportHbl.CardColorId = syscodes[index].Id;
            //            OceanExportHbl.CardColorValue = syscodes[index].CodeValue;
            //            CardClass = syscodes[index].CodeValue;
            //        }
            //        else 
            //        {
            //            OceanExportHbl.CardColorId = syscodes[0].Id;
            //            OceanExportHbl.CardColorValue = syscodes[0].CodeValue;
            //            CardClass = syscodes[0].CodeValue;
            //        }
                    
            //    }
            //    else 
            //    {
            //        OceanExportHbl = new CreateUpdateOceanExportHblDto();
            //        if (OceanExportHbls != null && OceanExportHbls.Count > 0)
            //        {
            //            OceanExportHbl = ObjectMapper.Map<OceanExportHblDto, CreateUpdateOceanExportHblDto>(OceanExportHbls[0]);
            //            IsShowHbl = true;
            //            ViewData["HAVEHBL"] = "Y";
            //        }
                    
            //    }
 
            //}
            //else {
                //queryHbl.Id = Hid;
                //IsShowHbl = true;
                //
                //ViewData["HAVEHBL"] = "Y";
            //}
            //TempData["PrintData"] = JsonConvert.SerializeObject(OceanExportMbl);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var updateItem = ObjectMapper.Map<OceanExportMblDto, CreateUpdateOceanExportMblDto>(OceanExportMblDto);
            await _oceanExportMblAppService.UpdateAsync(OceanExportMblDto.Id, updateItem);

            if (OceanExportHblDto is not null)
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
