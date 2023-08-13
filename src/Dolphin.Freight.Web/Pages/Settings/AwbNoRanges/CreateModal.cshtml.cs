
using Dolphin.Freight.Settings.AwbNoRanges;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.AwbNoRanges
{
    public class CreateModalModel : FreightPageModel
    {
        [BindProperty]
        public CreateUpdateAwbNoRangeDto AwbNoRange { get; set; }
        [BindProperty]
        public Guid? CarrierId { get; set; }

        private readonly IAwbNoRangeAppService _AwbNoRangeAppService;

        public CreateModalModel(IAwbNoRangeAppService awbNoRangeAppService)
        {
            _AwbNoRangeAppService = awbNoRangeAppService;
            
        }

        public async void OnGet()
        {
            AwbNoRange = new CreateUpdateAwbNoRangeDto();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AwbNoRange.CompanyId = CarrierId;
          
                await _AwbNoRangeAppService.CreateAsync(AwbNoRange);
                return NoContent();
           
            
        }
    }
}
