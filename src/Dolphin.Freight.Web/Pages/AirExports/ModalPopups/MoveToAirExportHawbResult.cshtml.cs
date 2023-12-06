using Dolphin.Freight.ImportExport.AirExports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dolphin.Freight.Web.Pages.AirExports.ModalPopups
{
    public class MoveToAirExportHawbResultModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty]
        public string MawbDocs { get; set; }

        private readonly IAirExportMawbAppService _airExportMawbAppService;

        public MoveToAirExportHawbResultModel(IAirExportMawbAppService airExportMawbAppService)
        {
            _airExportMawbAppService = airExportMawbAppService; 
        }

        public async Task OnGetAsync()
        {
            var mawb = await _airExportMawbAppService.GetAsync(Id);

            MawbDocs = mawb.FilingNo;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return NoContent();
        }
    }
}
