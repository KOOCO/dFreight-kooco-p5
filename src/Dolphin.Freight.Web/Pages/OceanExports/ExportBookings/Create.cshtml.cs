using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [BindProperty]
        public CreateUpdateContainerDto CreateUpdateContainerBooking { get; set; }
        [BindProperty]
        public List<CreateUpdateContainerDto> CreateUpdateContainerDtos { get; set; }
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IExportBookingAppService _exportBookingAppService;
        private readonly IContainerAppService _containerAppService;
        public List<SelectListItem> RateUnitTypeLookupList { get; set; }
        public List<SelectListItem> UnitTypeLookupList { get; set; }
        [BindProperty]
        public List<ManifestCommodity> Commodities { get; set; }
        public CreateModel(ISysCodeAppService sysCodeAppService,IExportBookingAppService exportBookingAppService, IContainerAppService containerAppService)
        {
            _exportBookingAppService = exportBookingAppService;
            _sysCodeAppService = sysCodeAppService;
            _containerAppService = containerAppService;
        }
        public void OnGet()
        {
            ExportBooking = new CreateUpdateExportBookingDto
            {
                SoNoDate = DateTime.Now
            };
            RateUnitTypeLookupList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "KG", Value = "KG" },
                new SelectListItem() { Text = "LG", Value = "LG" }
            };
            UnitTypeLookupList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "CBM", Value = "CBM" },
                new SelectListItem() { Text = "CFT", Value = "CFT" }
            };
        }
        public async Task<JsonResult> OnPostAsync()
        {
            if (ExportBooking.IsCreateBySystem) {
                ExportBooking.SoNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "ExportBooking_SoNo" });
            }

            if (ExportBooking.ExtraProperties == null)
            {
                ExportBooking.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
            }

            if (Commodities != null && Commodities.Any())
            {
                ExportBooking.ExtraProperties.Remove("Commodities");
                ExportBooking.ExtraProperties.Add("Commodities", Commodities);
            }


            

            var exportBooking = await _exportBookingAppService.CreateAsync(ExportBooking);
            QueryContainerDto query = new QueryContainerDto() { QueryId = exportBooking.Id };
            if (CreateUpdateContainerBooking != null)
            {
                CreateUpdateContainerBooking.BookingId = exportBooking.Id;

                var container = await _containerAppService.CreateAsync(CreateUpdateContainerBooking);
            }
         
            Dictionary<string, Guid> rs = new()
            {
                { "id", exportBooking.Id }
            };
            return new JsonResult(rs);
        }
    }
}
