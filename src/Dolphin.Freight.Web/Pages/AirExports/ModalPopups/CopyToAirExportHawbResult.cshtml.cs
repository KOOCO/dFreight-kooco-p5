using System;
using System.Threading.Tasks;
using Dolphin.Freight.ImportExport.AirExports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dolphin.Freight.Web.Pages.AirExports.ModalPopups
{
    public class CopyToAirExportHawbResultModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty]
        public string MawbDocNo { get; set; }

        private readonly IAirExportMawbAppService _airExportMawbAppService;

        public CopyToAirExportHawbResultModel(IAirExportMawbAppService airExportMawbAppService)
        {
            _airExportMawbAppService = airExportMawbAppService;
        }

        public async Task OnGetAsync()
        {
            var mawb = await _airExportMawbAppService.GetAsync(Id);
            MawbDocNo = mawb.FilingNo;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            return NoContent();
        }
    }
}
