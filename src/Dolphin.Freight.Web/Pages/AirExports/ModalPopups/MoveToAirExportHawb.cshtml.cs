using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.AirExports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dolphin.Freight.Web.Pages.AirExports.ModalPopups
{
    public class MoveToAirExportHawbModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid HawbId { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid PrevMawbId { get; set; }
        [BindProperty]
        public AirExportDetails AirExportDetails { get; set; }

        private readonly IAirExportHawbAppService _airExportHawbAppService;
        private readonly IAirExportMawbAppService _airExportMawbAppService;

        public MoveToAirExportHawbModel(IAirExportHawbAppService airExportHawbAppService, IAirExportMawbAppService airExportMawbAppService)
        {
            _airExportHawbAppService = airExportHawbAppService;
            _airExportMawbAppService = airExportMawbAppService;
        }

        public async Task OnGet()
        {
            AirExportDetails = new();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var newMawbId = AirExportDetails.MawbId;
            var hawbId = HawbId;

            await _airExportHawbAppService.UpdateMawbIdOfHawbAsync(hawbId, newMawbId);

            var mawb = await _airExportMawbAppService.GetAsync(newMawbId);
            var docNo = mawb.FilingNo;

            var rs = new Dictionary<string, object>
            {
                { "MawbId", newMawbId },
                { "DocNo", docNo }
            };

            return new JsonResult(rs);
        }
    }
}
