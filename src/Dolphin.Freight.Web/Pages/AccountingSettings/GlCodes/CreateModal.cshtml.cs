
using Dolphin.Freight.AccountingSettings.GlCodes;
using Dolphin.Freight.Settings.ContainerSizes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.AccountingSettings.GlCodes
{
    public class CreateModalModel : FreightPageModel
    {
        [BindProperty]
        public CreateUpdateGlCodeDto GlCode { get; set; }
        private readonly IGlCodeAppService _glCodesAppService;

        public CreateModalModel(IGlCodeAppService glCodesAppService)
        {
            _glCodesAppService = glCodesAppService;

        }
        public void OnGetAsync()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _glCodesAppService.CreateAsync(GlCode);
            return NoContent();
        }
    }
}
