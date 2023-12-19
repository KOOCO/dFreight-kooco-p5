
using Dolphin.Freight.Localization;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dolphin.Freight.Web.Pages.Settings.PackageUnits
{
    public class EditModalModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        public List<SelectListItem> AmsNoLookup { get; set; }
        public List<SelectListItem> EManifestLookup { get; set; }
        [BindProperty]
        public CreateUpdatePackageUnitDto PackageUnit { get; set; }
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IStringLocalizer<FreightResource> _localizer;
        private readonly ISysCodeAppService _sysCodeAppService;
        public EditModalModel(IPackageUnitAppService packageUnitAppService, IStringLocalizer<FreightResource> localizer, ISysCodeAppService sysCodeAppService)
        {
            _packageUnitAppService = packageUnitAppService;
            _localizer = localizer;
            _sysCodeAppService = sysCodeAppService;
        }
        public async Task OnGetAsync()
        {
            var packageUnit = await _packageUnitAppService.GetAsync(Id);
            PackageUnit = ObjectMapper.Map<PackageUnitDto, CreateUpdatePackageUnitDto>(packageUnit);

            await FillAmsNo();
            await FillEManifest();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            bool isPackageUnitAlreadyExist = await _packageUnitAppService.IsPackageUnitAlreadyExist(PackageUnit.PackageCode, Id);
            if (isPackageUnitAlreadyExist)
                throw new UserFriendlyException(string.Format(_localizer["PackageUnitAlreadyExist"].ToString(), PackageUnit.PackageCode));
            await _packageUnitAppService.UpdateAsync(Id, PackageUnit);
            return NoContent();
        }

        public async Task FillAmsNo()
        {
            var AmsNoLookUp = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "AmsNoId" });

            AmsNoLookup = AmsNoLookUp.Select(s => new SelectListItem(s.CodeValue, s.Id.ToString(), false)).OrderBy(o => o.Text).ToList();
        }

        public async Task FillEManifest()
        {
            var EManifestLookUp = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "EManifestId" });

            EManifestLookup = EManifestLookUp.Select(s => new SelectListItem(s.CodeValue, s.Id.ToString(), false)).OrderBy(o => o.Text).ToList();
        }
    }
}
