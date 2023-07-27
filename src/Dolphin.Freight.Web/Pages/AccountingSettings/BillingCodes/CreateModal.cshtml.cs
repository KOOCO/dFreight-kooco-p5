using Dolphin.Freight.AccountingSettings.BillingCodes;
using Dolphin.Freight.AccountingSettings.GlCodes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using SelectItems = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;
using System.Linq;

namespace Dolphin.Freight.Web.Pages.AccountingSettings.BillingCodes
{
    public class CreateModalModel : FreightPageModel
    {
        [BindProperty]
        public CreateUpdateBillingCodeDto BillingCode { get; set; }
        private readonly IBillingCodeAppService _billingCodeAppService;
        private readonly IGlCodeAppService _glCodeAppService;
        [BindProperty]
        public List<SelectItems> GiCodes { get; set; }

        public CreateModalModel(IBillingCodeAppService billingCodeAppService, IGlCodeAppService glCodeAppService)
        {
            _billingCodeAppService = billingCodeAppService;
            _glCodeAppService = glCodeAppService;
            BillingCode = new CreateUpdateBillingCodeDto();
            GiCodes = new List<SelectItems>();
        }
        public async void OnGet()
        {
            await GetGiCodesAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _billingCodeAppService.CreateAsync(BillingCode);
            return NoContent();
        }

        private async Task GetGiCodesAsync()
        {
            var glCodes = (await _glCodeAppService.GetGlCodesAsync(new Common.QueryDto())).Select(x => new SelectItems
            {
                Text = x.Code,
                Value = x.Id.ToString()
            }).ToList();
            GiCodes.AddRange(glCodes);
        }
    }
}
