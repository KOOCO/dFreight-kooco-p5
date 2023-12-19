using Dolphin.Freight.AccountingSettings.BillingCodes;
using Dolphin.Freight.AccountingSettings.GlCodes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.AccountingSettings.BillingCodes
{
    public class EditModalModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty]
        public CreateUpdateBillingCodeDto BillingCode { get; set; }
        private readonly IBillingCodeAppService _billingCodeAppService;

        public EditModalModel(IBillingCodeAppService billingCodeAppService)
        {
            _billingCodeAppService = billingCodeAppService;
        }
        public async Task OnGetAsync()
        {
            var billingCode = await _billingCodeAppService.GetAsync(Id);
            BillingCode = ObjectMapper.Map < BillingCodeDto, CreateUpdateBillingCodeDto >(billingCode);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _billingCodeAppService.UpdateAsync(Id, BillingCode);
            return NoContent();
        }
    }
}
