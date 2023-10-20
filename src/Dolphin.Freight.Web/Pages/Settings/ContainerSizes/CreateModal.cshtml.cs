using Dolphin.Freight.Settings.ContainerSizes;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settinngs.ContainerSizes;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.Settings.ContainerSizes
{
    public class CreateModalModel : FreightPageModel
    {
        [BindProperty]
        public CreateUpdateContainerSizeDto ContainerSize { get; set; }
        public List<SelectListItem> AmsTypeCodeLookup { get; set; }
        public List<SelectListItem> ContainerGroupLookup { get; set; }
        private readonly IContainerSizeAppService _containerSizeAppService;
        private readonly ISysCodeAppService _sysCodeAppService;

        public CreateModalModel(IContainerSizeAppService containerSizeAppService, ISysCodeAppService sysCodeAppService)
        {
            _containerSizeAppService = containerSizeAppService;
            _sysCodeAppService = sysCodeAppService;
        }
        public async Task OnGet()
        {
            ContainerSize = new CreateUpdateContainerSizeDto();

            await FillAmsTypeCode();
            await FillContainerGroupCode();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _containerSizeAppService.CreateAsync(ContainerSize);
            return NoContent();
        }

        public async Task FillAmsTypeCode()
        {
            var AmsTypeCodeLookUp = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "AmsTypeCodeId" });

            AmsTypeCodeLookup = AmsTypeCodeLookUp.Select(s => new SelectListItem(s.CodeValue, s.Id.ToString(), false)).OrderBy(o => o.Text).ToList();
        }

        public async Task FillContainerGroupCode()
        {
            var ContainerGroupLookUp = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "ContainerGroupId" });

            ContainerGroupLookup = ContainerGroupLookUp.Select(s => new SelectListItem(s.ShowName, s.Id.ToString(), false)).OrderBy(s => s.Text).ToList();
        }
    }
}
