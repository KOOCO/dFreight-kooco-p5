using Dolphin.Freight.AccountingSettings.BillingCodes;
using Dolphin.Freight.AccountingSettings.GlCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectMapping;
using SelectItems = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

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

        private readonly IGlCodeAppService _glCodeAppService;
        [BindProperty]
        public List<SelectItems> GiCodes { get; set; }


        public EditModalModel(IBillingCodeAppService billingCodeAppService)
        {
            _billingCodeAppService = billingCodeAppService;
            GiCodes = new List<SelectItems>();
        }
        public async Task OnGetAsync()
        {
            await GetGiCodesAsync();
            var billingCode = await _billingCodeAppService.GetAsync(Id);
            BillingCode = ObjectMapper.Map < BillingCodeDto, CreateUpdateBillingCodeDto >(billingCode);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _billingCodeAppService.UpdateAsync(Id, BillingCode);
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
