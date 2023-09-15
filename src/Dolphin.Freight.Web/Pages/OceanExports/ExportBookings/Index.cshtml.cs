using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.OceanExports.ExportBookings
{
    public class IndexModel : FreightPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid BookingId { get; set; }
        [BindProperty]
        public CreateUpdateOceanExportMblDto OceanExportMbl { get; set; }
        [BindProperty]
        public CreateUpdateOceanExportHblDto OceanExportHbl { get; set; }
        private readonly ISubstationAppService _substationAppService;
        public string Search { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> ReferenceLookupList { get; set; }
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        public List<SelectListItem> ShipModeLookupList { get; set; }
        public List<SelectListItem> BlTypeList { get; set; }
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly IExportBookingAppService _exportBookingAppService;

        public IndexModel(ISubstationAppService substationAppService, ITradePartnerAppService tradePartnerAppService, IAjaxDropdownAppService ajaxDropdownAppService,
                        ISysCodeAppService sysCodeAppService, IOceanExportMblAppService oceanExportMblAppService, IOceanExportHblAppService oceanExportHblAppService,
                        IExportBookingAppService exportBookingAppService)
        {
            _substationAppService = substationAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _substationAppService = substationAppService;
            _sysCodeAppService = sysCodeAppService;
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _exportBookingAppService = exportBookingAppService;

        }
        public async Task OnGetAsync()
        {
            await FillReferenceNumberAsync();
            await FillSubstationAsync();
            await FillTradePartnerAsync();
            await FillShipModeAsync();
           

        }
        public async Task<JsonResult> OnPostCreateBookingMbl(Guid id)
        {
            
                var booking = await _exportBookingAppService.GetAsync(id);
            await FillSubstationAsync();

            OceanExportHbl = new CreateUpdateOceanExportHblDto();
                OceanExportMbl = new CreateUpdateOceanExportMblDto();
                if (OceanExportMbl.PolEtd == null) OceanExportMbl.PostDate = DateTime.Now;
                else OceanExportMbl.PostDate = OceanExportMbl.PolEtd;
                OceanExportMbl.FilingNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "OceanExportMbl_FilingNo" });
                OceanExportMbl.OfficeId = Guid.Parse(SubstationLookupList.Select(x => x.Value).FirstOrDefault());
                var mbl = await _oceanExportMblAppService.CreateAsync(OceanExportMbl);
                OceanExportHbl.MblId = mbl.Id;
                QueryDto query = new QueryDto();
               
                OceanExportHbl.HblNo = booking.SoNo;
                OceanExportHbl.SoNo = booking.SoNo;
                var hbl = await _oceanExportHblAppService.CreateAsync(OceanExportHbl);


                booking.ReferenceId = mbl.Id;
                booking.HblNo = hbl.HblNo;
                var newbooking = ObjectMapper.Map<ExportBookingDto, CreateUpdateExportBookingDto>(booking);
                await _exportBookingAppService.UpdateAsync(id, newbooking);
                string baseUrl = Request.GetDisplayUrl();
                


          

            Dictionary<string, Guid> rs = new()
            {
                { "id", mbl.Id }
            };
            return new JsonResult(rs);
        }
        public async Task<JsonResult> OnPostAssignMbl(Guid id,Guid mblId,bool isShipMode,bool isSvcFrom,bool isSvcTo)
        {

            var booking = await _exportBookingAppService.GetAsync(id);
            await FillSubstationAsync();

            var mbl = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(mblId);
            if (isShipMode)
            {
                booking.ShipModeId = mbl.ShipModeId;

            
            }
            if (isSvcFrom)
            {
                booking.SvcTermFromId = mbl.SvcTermFromId;


            }
            if (isSvcTo)
            {
                booking.SvcTermToId = mbl.SvcTermToId;


            }
            booking.ReferenceId = mbl.Id;
           
            var newbooking = ObjectMapper.Map<ExportBookingDto, CreateUpdateExportBookingDto>(booking);
            await _exportBookingAppService.UpdateAsync(id, newbooking);
            





            Dictionary<string, Guid> rs = new()
            {
                { "id", mbl.Id }
            };
            return new JsonResult(rs);
        }
        #region FillReferenceNumberAsync()
        private async Task FillReferenceNumberAsync()
        {
            var referenceLookup = await _ajaxDropdownAppService.GetReferenceMblAsync(new QueryDto());

           

            ReferenceLookupList = referenceLookup
                                                .Select(x => new SelectListItem(x.ReferenceNo+" "+x.MblNo, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion
        #region FillSubstationAsync()
        private async Task FillSubstationAsync()
        {
            var substationLookup = await _substationAppService.GetSubstationsLookupAsync();
            SubstationLookupList = substationLookup.Items
                                                .Select(x => new SelectListItem(x.SubstationName + "  (" + x.AbbreviationName + ")", x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillTradePartnerAsync()
        private async Task FillTradePartnerAsync()
        {
            var tradePartnerLookup = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            TradePartnerLookupList = tradePartnerLookup.Items
                                                .Select(x => new SelectListItem(x.TPName + " / " + x.TPCode, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion
        #region FillShipModeAsync()
        private async Task FillShipModeAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "ShipModeId" });

            ShipModeLookupList = lookUp
                                        .Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion

    }
}
