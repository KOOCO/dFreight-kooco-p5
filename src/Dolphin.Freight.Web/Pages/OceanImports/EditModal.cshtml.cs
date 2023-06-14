using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Dolphin.Freight.Permissions.SettingsPermissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dolphin.Freight.Web.Pages.OceanImports
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
        public OceanImportMblDto OceanImportMblDto { get; set; }
        [BindProperty]
        public OceanImportHblDto OceanImportHblDto { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportMblDto OceanImportMbl { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportHblDto OceanImportHbl { get; set; }
        public IList<OceanImportHblDto> OceanImportHbls { get; set; }
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        public EditModalModel(IOceanImportMblAppService oceanImportMblAppService, IOceanImportHblAppService oceanImportHblAppService, ISysCodeAppService sysCodeAppService)
        {
            _oceanImportMblAppService = oceanImportMblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _sysCodeAppService = sysCodeAppService;
        }
        public async Task OnGetAsync()
        {
            OceanImportMbl = await _oceanImportMblAppService.GetCreateUpdateOceanImportMblDtoById(Id);
            ImportExport.OceanImports.QueryHblDto query = new ImportExport.OceanImports.QueryHblDto() { MblId = Id };
            query.Id = Hid;
            OceanImportHbl = new ();
            IsShowHbl = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var updateItem = ObjectMapper.Map<OceanImportMblDto, CreateUpdateOceanImportMblDto>(OceanImportMblDto);
            await _oceanImportMblAppService.UpdateAsync(OceanImportMblDto.Id, updateItem);

            if (OceanImportHblDto is not null)
            {
                OceanImportHbl.MblId = OceanImportMblDto.Id;

                if (OceanImportHbl.Id != Guid.Empty)
                {
                    await _oceanImportHblAppService.UpdateAsync(OceanImportHbl.Id, OceanImportHbl);
                }
                else
                {
                    await _oceanImportHblAppService.CreateAsync(OceanImportHbl);
                }
            }
            return new ObjectResult(new { id = OceanImportMblDto.Id });
        }
    }
}
