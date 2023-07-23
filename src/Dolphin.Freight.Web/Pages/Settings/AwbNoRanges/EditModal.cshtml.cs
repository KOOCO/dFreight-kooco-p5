using Dolphin.Freight.Settings.AwbNoRanges;
using Dolphin.Freight.Settings.ItNoRanges;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.Web.Pages.AwbNoRanges
{
    public class EditModalModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateAwbNoRangeDto AwbNoRange { get; set; }
        [BindProperty]
        public Guid? CarrierId { get; set; }

        private readonly IAwbNoRangeAppService _AwbNoRangeAppService;

        public EditModalModel(IAwbNoRangeAppService awbNoRangeAppService)
        {
            _AwbNoRangeAppService = awbNoRangeAppService;

        }

        public async Task OnGetAsync()
        {
            var awbNoRange = await _AwbNoRangeAppService.GetAsync(Id);
            AwbNoRange = ObjectMapper.Map<AwbNoRangeDto, CreateUpdateAwbNoRangeDto>(awbNoRange);
            CarrierId = AwbNoRange.CompanyId;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AwbNoRange.CompanyId = CarrierId;
            await _AwbNoRangeAppService.UpdateAsync(Id, AwbNoRange);
            return NoContent();
        }
    }
}
