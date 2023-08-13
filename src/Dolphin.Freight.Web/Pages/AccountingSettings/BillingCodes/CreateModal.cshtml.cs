using Dolphin.Freight.AccountingSettings.BillingCodes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using SelectItems = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

namespace Dolphin.Freight.Web.Pages.AccountingSettings.BillingCodes
{
    public class CreateModalModel : FreightPageModel
    {
        [BindProperty]
        public CreateUpdateBillingCodeDto BillingCode { get; set; }
        private readonly IBillingCodeAppService _billingCodeAppService;
        [BindProperty]
        public List<SelectItems> GiCodes { get; set; }

        public CreateModalModel(IBillingCodeAppService billingCodeAppService)
        {
            _billingCodeAppService = billingCodeAppService;
            BillingCode = new CreateUpdateBillingCodeDto();
        }
        public async void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _billingCodeAppService.CreateAsync(BillingCode);
            return NoContent();
        }
    }
}
