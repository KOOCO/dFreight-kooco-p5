using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settinngs.ContainerSizes;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.Settings.ContainerSizes
{
    public class EditModalModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        public List<SelectListItem> AmsTypeCodeLookup { get; set; }
        public List<SelectListItem> ContainerGroupLookup { get; set; }
        [BindProperty]
        public CreateUpdateContainerSizeDto ContainerSize { get; set; }
        private readonly IContainerSizeAppService _containerSizeAppService;
        private readonly ISysCodeAppService _sysCodeAppService;

        public EditModalModel(IContainerSizeAppService containerSizeAppService, ISysCodeAppService sysCodeAppService)
        {
            _containerSizeAppService = containerSizeAppService;
            _sysCodeAppService = sysCodeAppService;
        }
        public async Task OnGetAsync()
        {
            var containerSize = await _containerSizeAppService.GetAsync(Id);
            ContainerSize = ObjectMapper.Map<ContainerSizeDto, CreateUpdateContainerSizeDto>(containerSize);

            await FillAmsTypeCode();
            await FillContainerGroupCode();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _containerSizeAppService.UpdateAsync(Id, ContainerSize);
            return NoContent();
        }

        public async Task FillAmsTypeCode()
        {
            var AmsTypeCodeLookUp = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "AmsTypeCodeId" });

            AmsTypeCodeLookup = AmsTypeCodeLookUp.Select(s => new SelectListItem(s.CodeValue, s.Id.ToString(), false)).ToList();
        }

        public async Task FillContainerGroupCode()
        {
            var ContainerGroupLookUp = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "ContainerGroupId" });

            ContainerGroupLookup = ContainerGroupLookUp.Select(s => new SelectListItem(s.ShowName, s.Id.ToString(), false)).OrderBy(s => s.Text).ToList();
        }
    }
}
