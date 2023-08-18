using Dolphin.Freight.Common;
using Dolphin.Freight.Common.Memos;
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
    public class CopyMawbModal : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

      public  List<SelectListItem> HblOptions { get; set; }

               [BindProperty]
        public bool IsCopyFlightInfo { get; set; }
        [BindProperty]
        public string CopyHawb { get; set; }
        [BindProperty]
        public bool CopyCommodity { get; set; }
        [BindProperty]
        public bool CopyAccountingInformation { get; set; }
        [BindProperty]
        public bool AP { get; set; }
        [BindProperty]
        public bool AR { get; set; }
        [BindProperty]
        public bool DC { get; set; }
      

        public CopyMawbModal()
        {
            
        }

        public async Task OnGetAsync()
        {
            HblOptions = FillHblOptions();
            CopyAccountingInformation = true;
            AP = true;
            AR = true;
            DC = true;
            CopyHawb = "FirstHawbOnly";
            IsCopyFlightInfo = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Dictionary<string, Guid> rs = new()
            {
                { "id", Id }
            };
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
