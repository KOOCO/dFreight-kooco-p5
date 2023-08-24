using Dolphin.Freight.Common;
using Dolphin.Freight.Common.Memos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dolphin.Freight.Web.Pages.AirExports
{
    public class CopyModalMawbList : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty]
        public CopyModelInfo CopyModel { get; set; }
      public  List<SelectListItem> HblOptions { get; set; }

       
      


        public async Task OnGetAsync()
        {
            CopyModel = new CopyModelInfo();
            HblOptions = FillHblOptions();
            CopyModel.CopyAccountingInformation = true;
            CopyModel.AP = true;
            CopyModel.AR = true;
            CopyModel.DC = true;
            CopyModel.CopyHawb = "FirstHawbOnly";
            CopyModel.IsCopyFlightInfo = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var rs = new Dictionary<string, object>
    {
        { "Id", Id },
        { "IsCopyFlightInfo", CopyModel.IsCopyFlightInfo },
       
        { "CopyCommodity", CopyModel.CopyCommodity },
        { "CopyAccountingInformation", CopyModel.CopyAccountingInformation },
        { "AP", CopyModel.AP },
        { "AR", CopyModel.AR },
        { "DC", CopyModel.DC },
    };

            // Return the query string parameters as JSON result
            return new JsonResult(rs);

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
