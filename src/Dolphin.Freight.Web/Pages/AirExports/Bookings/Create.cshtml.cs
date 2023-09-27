using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirExports.Bookings;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.AirExports;

namespace Dolphin.Freight.Web.Pages.AirExports.Bookings
{
    public class CreateModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
       
        [BindProperty(SupportsGet = true)]
        public bool IsCopy { get; set; }
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
        public List<SelectListItem> EnumList { get; set; }
        public List<SelectListItem> ChargeItemList { get; set; }
        [BindProperty]
        public List<Commodity> Commodities { get; set; }
        public CreateModel(ISysCodeAppService sysCodeAppService,IExportBookingAppService exportBookingAppService, IContainerAppService containerAppService)
        {
            _exportBookingAppService = exportBookingAppService;
            _sysCodeAppService = sysCodeAppService;
            _containerAppService = containerAppService;
        }
        public async Task OnGetAsync()
        {
            ChargeItemList = GetChargeItemSelectList();
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
            if (IsCopy)
            {
                var exportBooking = await _exportBookingAppService.GetAsync(Id);
                ExportBooking = ObjectMapper.Map<ExportBookingDto, CreateUpdateExportBookingDto>(exportBooking);
                ExportBooking.SoNo = null;
            }
            else
            {
                ExportBooking = new CreateUpdateExportBookingDto
                {
                    SoNoDate = DateTime.Now,
                    DVCarriage = "NVD",
                    DVCustomer = "NCV",
                    Insurance = "XXX",
                    WtVal = "PPD",
                    Other = "PPD"
                };
            }
           
        }
        public async Task<JsonResult> OnPostAsync()
        {
            if (IsCopy)
            {
                ExportBooking.Id = Guid.Empty;
            
            }
            if (ExportBooking.IsCreateBySystem) {
                ExportBooking.SoNo = await _sysCodeAppService.GetSystemBookingNoAsync(new() { QueryType = "ExportBooking_SoNo" });
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

            if (ExportBooking.OtherCharges != null)
            {
                ExportBooking.ExtraProperties.Add("OtherCharges", ExportBooking.OtherCharges);
            }


            var exportBooking = await _exportBookingAppService.CreateAsync(ExportBooking);
           
         
            Dictionary<string, Guid> rs = new()
            {
                { "id", exportBooking.Id }
            };
            return new JsonResult(rs);
        }
        private List<SelectListItem> GetChargeItemSelectList()
        {
            var enumValues = Enum.GetValues(typeof(ChargeItems))
                                  .Cast<ChargeItems>()
                                  .Select(e => new SelectListItem
                                  {
                                      Text = L["Enum:ItemCharge." + e.ToString()],
                                      Value = e.ToString(),

                                  })
                                  .ToList();

            return enumValues;
        }
    }
}
