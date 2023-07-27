using Dolphin.Freight.Localization;
using Dolphin.Freight.Settinngs.PackageUnits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dolphin.Freight.Web.Pages.Settings.PackageUnits
{
    public class CreateModalModel : FreightPageModel
    {
        [BindProperty]
        public CreateUpdatePackageUnitDto PackageUnit { get; set; }
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IStringLocalizer<FreightResource> _localizer;

        public CreateModalModel(IPackageUnitAppService packageUnitAppService, IStringLocalizer<FreightResource> localizer)
        {
            _packageUnitAppService = packageUnitAppService;
            _localizer = localizer;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            bool isPackageUnitAlreadyExist = await _packageUnitAppService.IsPackageUnitAlreadyExist(PackageUnit.PackageCode);
            if (isPackageUnitAlreadyExist)
                throw new UserFriendlyException(string.Format(_localizer["PackageUnitAlreadyExist"].ToString(), PackageUnit.PackageCode));
            await _packageUnitAppService.CreateAsync(PackageUnit);
            return NoContent();
        }
    }
}
