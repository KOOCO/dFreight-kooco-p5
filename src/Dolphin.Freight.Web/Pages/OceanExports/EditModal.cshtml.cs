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
            OceanExportHbls = await _oceanExportHblAppService.QueryListByMidAsync(query);
            QueryHblDto queryHbl = new QueryHblDto();
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
            //        
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
            //        
            //    }
            //
            //}
            //else {
                queryHbl.Id = Hid;
                OceanExportHbl = new();
                IsShowHbl = true;

                ViewData["HAVEHBL"] = "Y";
            //}
            TempData["PrintData"] = JsonConvert.SerializeObject(OceanExportMbl);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (Hid != null && Hid != Guid.Empty) await _oceanExportHblAppService.UpdateAsync(Hid.Value, OceanExportHbl);
            else 
            {
                if (NewHbl == 1) {
                    OceanExportHbl.MblId = Id;
                    if (OceanExportHbl.HblNo == null || OceanExportHbl.HblNo.Equals(""))
                    {
                        OceanExportHbl.HblNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "OceanExportHbl_HblNo" });
                    }
                    /*
                    QueryDto query = new QueryDto();
                    query.QueryType = "CardColorId";
                    var syscodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(query);
                    if (syscodes != null && syscodes.Count > 0)
                    {
                        var syscode = syscodes[0];
                        OceanExportHbl.CardColorId = syscode.Id;
                    }*/
                    var hbl = await _oceanExportHblAppService.CreateAsync(OceanExportHbl);
                    Hid = hbl.Id;
                } 
            } 
            var boceanExportMbl = await _oceanExportMblAppService.GetAsync(Id);
            if (boceanExportMbl != null) 
            {
                OceanExportMbl.PostDate = boceanExportMbl.PostDate;
                OceanExportMbl.FilingNo = boceanExportMbl.FilingNo;
                await _oceanExportMblAppService.UpdateAsync(Id, OceanExportMbl);
            }

            return new ObjectResult(new { status = "success",Hid = Hid,Id=Id });
        }
    }
}
