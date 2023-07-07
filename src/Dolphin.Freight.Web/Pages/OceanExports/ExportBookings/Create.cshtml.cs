using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Dolphin.Freight.Permissions.OceanExportPermissions;

namespace Dolphin.Freight.Web.Pages.OceanExports.ExportBookings
{
    public class CreateModel : FreightPageModel
    {
        [BindProperty]
        public CreateUpdateExportBookingDto ExportBooking { get; set; }
        public string CabinateSize { get; set; }
        public int Quantity { get; set; }
        public int Index { get; set; }

        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IExportBookingAppService _exportBookingAppService;
        public CreateModel(ISysCodeAppService sysCodeAppService,IExportBookingAppService exportBookingAppService )
        {
            _exportBookingAppService = exportBookingAppService;
            _sysCodeAppService = sysCodeAppService;
        }
        public void OnGet()
        {
            ExportBooking = new CreateUpdateExportBookingDto
            {
                SoNoDate = DateTime.Now
            };
        }
        public async Task<JsonResult> OnPostAsync()
        {
            if (ExportBooking.IsCreateBySystem) {
                ExportBooking.SoNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "ExportBooking_SoNo" });
            }
            var exportBooking = await _exportBookingAppService.CreateAsync(ExportBooking);
            Dictionary<string, Guid> rs = new()
            {
                { "id", exportBooking.Id }
            };
            return new JsonResult(rs);
        }
    }
}
