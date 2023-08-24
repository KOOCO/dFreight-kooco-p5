using Dolphin.Freight.Common;
using Dolphin.Freight.Common.Memos;
using Dolphin.Freight.ImportExport.AirImports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dolphin.Freight.Web.Pages.AirImports
{
    public class CopyHawbResultModal : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
     
       
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        [BindProperty]
        public string MawbDocNo { get; set; }
       
        public CopyHawbResultModal(IAirImportMawbAppService airImportMawbAppService)
        {

            
            _airImportMawbAppService= airImportMawbAppService;

        }
      


        public async Task OnGetAsync()
        {
            var mawb = await _airImportMawbAppService.GetAsync(Id);
            MawbDocNo = mawb.FilingNo;

        }

        public async Task<IActionResult> OnPostAsync()
        {

          


            return NoContent();

        }
        private List<SelectListItem> FillHblOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "FirstHawbOnly", Text = "First HAWB Only"},
                new SelectListItem { Value = "AllHAWB", Text = "All HAWB"}
            };
        }



    }

}
