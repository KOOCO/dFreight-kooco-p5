using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
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
        public bool ISToolTipShow { get; set; }
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
            OceanImportHbl = new();
            IsShowHbl = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _oceanImportMblAppService.UpdateAsync(OceanImportMbl.Id, OceanImportMbl);

            if (OceanImportHbl is not null && !string.IsNullOrEmpty(OceanImportHbl.HblNo))
            {
                OceanImportHbl.MblId = OceanImportMbl.Id;

                if (OceanImportHbl.Id != Guid.Empty)
                {
                    if (OceanImportHbl.CardColorId != Guid.Empty)
                    {
                        var sysCode = await _sysCodeAppService.GetAsync((Guid)OceanImportHbl.CardColorId);
                        sysCode.CodeValue = OceanImportHbl.CardColorValue;
                        sysCode.ShowName = OceanImportHbl.HblNo;

                        await _sysCodeAppService.UpdateAsync((Guid)OceanImportHbl.CardColorId, ObjectMapper.Map<SysCodeDto, CreateUpdateSysCodeDto>(sysCode));
                    }
                    else
                    {
                        SysCode sysCodeDto = new SysCode();
                        sysCodeDto.CodeType = "CardColorId";
                        sysCodeDto.CodeValue = OceanImportHbl.CardColorValue;
                        sysCodeDto.ShowName = OceanImportHbl.HblNo;

                        var newSysCode = await _sysCodeAppService.CreateAsync(ObjectMapper.Map<SysCode, CreateUpdateSysCodeDto>(sysCodeDto));

                        OceanImportHbl.CardColorId = newSysCode.Id;
                    }
                    try
                    {
                        await _oceanImportHblAppService.UpdateAsync(OceanImportHbl.Id, OceanImportHbl);
                    }
                    catch (DbUpdateException ex)
                    {
                        var failedEntries = ex.Entries;
                        foreach (var entry in failedEntries)
                        {
                            var entityName = entry.Metadata.Name;
                            var properties = entry.Properties.Where(p => p.IsModified && !p.IsTemporary);
                            foreach (var property in properties)
                            {
                                var propertyName = property.Metadata.Name;
                                Console.WriteLine($"Failed to update field: {propertyName} in entity: {entityName}");
                            }
                        }
                        throw;
                    }
                }
                else
                {
                    if (OceanImportHbl.CardColorId == Guid.Empty)
                    {
                        SysCodeDto sysCodeDto = new SysCodeDto();
                        sysCodeDto.Id = Guid.Empty;
                        sysCodeDto.CodeValue = OceanImportHbl.CardColorValue;
                        sysCodeDto.ShowName = OceanImportHbl.HblNo;

                        var newSysCode = await _sysCodeAppService.CreateAsync(ObjectMapper.Map<SysCodeDto, CreateUpdateSysCodeDto>(sysCodeDto));

                        OceanImportHbl.CardColorId = newSysCode.Id;
                    }
                    await _oceanImportHblAppService.CreateAsync(OceanImportHbl);
                }
            }

            return new ObjectResult(new { id = OceanImportMblDto.Id });
        }
    }
}
