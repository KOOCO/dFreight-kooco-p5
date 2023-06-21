using Dolphin.Freight.Accounting;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.OceanExports.ExportBookings
{
    public class EditModel : FreightPageModel
    {
        
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? CopyId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int WithInvoice { get; set; }
        [BindProperty(SupportsGet = true)]
        public int IsAR { get; set; }
        [BindProperty(SupportsGet = true)]
        public int IsAP { get; set; }
        [BindProperty(SupportsGet = true)]
        public int IsDC { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ShowMsg { get; set; } = false;
        [BindProperty(SupportsGet = true)]
        public bool ShowCopyMsg { get; set; } = false;
        [BindProperty]
        public CreateUpdateExportBookingDto ExportBooking { get; set; }

        public List<SelectListItem> ReferenceLookupList { get; set; }
        public List<SelectListItem> CancelReasonLookupList { get; set; }
        public List<SelectListItem> ShipModeLookupList { get; set; }
        public List<SelectListItem> FreightTermLookupList { get; set; }
        public List<SelectListItem> SvgTermLookupList { get; set; }
        public List<SelectListItem> IncotermsLookupList { get; set; }
        public List<SelectListItem> CargoTypeLookupList { get; set; }
    

        private readonly IExportBookingAppService _exportBookingAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IVesselScheduleAppService _vesselScheduleAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        public EditModel(IExportBookingAppService exportBookingAppService,  ISysCodeAppService sysCodeAppService, IInvoiceAppService invoiceAppService, IVesselScheduleAppService vesselScheduleAppService, IAjaxDropdownAppService ajaxDropdownAppService)
        {
            _exportBookingAppService = exportBookingAppService;
            _sysCodeAppService = sysCodeAppService;
            _invoiceAppService = invoiceAppService;
            _vesselScheduleAppService = vesselScheduleAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
        }
        public async Task OnGetAsync()
        {
            if (CopyId == null)
            {
                var exportBooking = await _exportBookingAppService.GetAsync(Id);
                ExportBooking = ObjectMapper.Map<ExportBookingDto, CreateUpdateExportBookingDto>(exportBooking);
            }
            else 
            {
                QueryInvoiceDto query = new QueryInvoiceDto();
                var exportBooking = await _exportBookingAppService.GetAsync(CopyId.Value);
                query.ParentId = CopyId;
                ExportBooking = ObjectMapper.Map<ExportBookingDto, CreateUpdateExportBookingDto>(exportBooking);
                ExportBooking.SoNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "ExportBooking_SoNo" });
                CreateUpdateExportBookingDto dto = new CreateUpdateExportBookingDto();
                ExportBooking.Id = new Guid();
                var booking = await _exportBookingAppService.CreateAsync(ExportBooking);
                if (WithInvoice == 1) {
                    query.NewParentId = booking.Id;
                    List<CopyIdDto> clist = await _invoiceAppService.CopyByBookingId(query, IsAR, IsAP, IsDC);
                }
                ExportBooking = ObjectMapper.Map<ExportBookingDto, CreateUpdateExportBookingDto>(booking);
                query.NewParentId = booking.Id;

                Id = booking.Id;
                

                ShowCopyMsg = true;
            }

            await FillReferenceNumberAsync();
            await FillCancelReasonAsync();
            await FillShipModeAsync();
            await FillFreightTermAsync();
            await FillSvgTermAsync();
            await FillIncotermsIdAsync();
            await FillCategoryTypeAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _exportBookingAppService.UpdateAsync(Id, ExportBooking);
            return NoContent();
        }

        #region FillReferenceNumberAsync()
        private async Task FillReferenceNumberAsync()
        {
            var referenceLookup = await _ajaxDropdownAppService.GetReferenceItemsByTypeAsync(new QueryDto());

            ReferenceLookupList = referenceLookup
                                                .Select(x => new SelectListItem(x.ReferenceNo, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillCancelReasonAsync()
        private async Task FillCancelReasonAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType= "CancelReason" });

            CancelReasonLookupList = lookUp
                                        .Select(x => new SelectListItem(x.CodeValue, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion

        #region FillShipModeAsync()
        private async Task FillShipModeAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "ShipModeId" });

            ShipModeLookupList = lookUp
                                        .Select(x => new SelectListItem(x.CodeValue, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion

        #region FillFreightTermAsync()
        private async Task FillFreightTermAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "FreightTermId" });

            FreightTermLookupList = lookUp
                                        .Select(x => new SelectListItem(x.CodeValue, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion

        #region FillSvgTermAsync()
        private async Task FillSvgTermAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "SvcTermFromId" });

            SvgTermLookupList = lookUp
                                        .Select(x => new SelectListItem(x.CodeValue, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion

        #region FillIncotermsIdAsync()
        private async Task FillIncotermsIdAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "IncotermsId" });

            IncotermsLookupList = lookUp
                                        .Select(x => new SelectListItem(x.CodeValue, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion

        #region FillCategoryTypeAsync()
        private async Task FillCategoryTypeAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "CargoTypeId" });

            CargoTypeLookupList = lookUp
                                        .Select(x => new SelectListItem(x.CodeValue, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion
    }
}
