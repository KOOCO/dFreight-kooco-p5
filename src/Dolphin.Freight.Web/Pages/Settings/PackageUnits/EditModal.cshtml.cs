
using Dolphin.Freight.Localization;
using Dolphin.Freight.Settinngs.PackageUnits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dolphin.Freight.Web.Pages.Settings.PackageUnits
{
    public class EditModalModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty]
        public CreateUpdatePackageUnitDto PackageUnit { get; set; }
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IStringLocalizer<FreightResource> _localizer;
        public EditModalModel(IPackageUnitAppService packageUnitAppService, IStringLocalizer<FreightResource> localizer)
        {
            _packageUnitAppService = packageUnitAppService;
            _localizer = localizer;
        }
        public async Task OnGetAsync()
        {
            var packageUnit = await _packageUnitAppService.GetAsync(Id);
            PackageUnit = ObjectMapper.Map<PackageUnitDto, CreateUpdatePackageUnitDto>(packageUnit);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            bool isPackageUnitAlreadyExist = await _packageUnitAppService.IsPackageUnitAlreadyExist(PackageUnit.PackageCode, Id);
            if (isPackageUnitAlreadyExist)
                throw new UserFriendlyException(string.Format(_localizer["PackageUnitAlreadyExist"].ToString(), PackageUnit.PackageCode));
            await _packageUnitAppService.UpdateAsync(Id, PackageUnit);
            return NoContent();
        }
    }
}
