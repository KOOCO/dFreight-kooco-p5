using Dolphin.Freight.Accounting;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settinngs.ContainerSizes;
using Dolphin.Freight.Settinngs.Ports;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static Dolphin.Freight.Permissions.OceanExportPermissions;

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
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> PortsManagementLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> ContainerLookupList { get; set; }
        public List<SelectListItem> RateUnitTypeLookupList { get; set; }
        public List<SelectListItem> UnitTypeLookupList { get; set; }
        public string CabinateSize { get; set; }
        public int Quantity { get; set; }
        public int Index { get; set; }


        private readonly IExportBookingAppService _exportBookingAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly IPortsManagementAppService _portsManagementAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IContainerSizeAppService _containerSizeAppService;
        private readonly IContainerAppService _containerAppService;
       
        [BindProperty]
        public CreateUpdateContainerDto CreateUpdateContainerBooking { get; set; }
        [BindProperty]
        public List<ManifestCommodity> Commodities { get; set; }
        public EditModel(IExportBookingAppService exportBookingAppService,  
                        ISysCodeAppService sysCodeAppService, 
                        IInvoiceAppService invoiceAppService, 
                        IAjaxDropdownAppService ajaxDropdownAppService, 
                        ITradePartnerAppService tradePartnerAppService,
                        IPortsManagementAppService portsManagementAppService,
                        ISubstationAppService substationAppService,
                        IContainerSizeAppService containerSizeAppService,
                        IContainerAppService containerAppService)
        {
            _exportBookingAppService = exportBookingAppService;
            _sysCodeAppService = sysCodeAppService;
            _invoiceAppService = invoiceAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _portsManagementAppService = portsManagementAppService;
            _substationAppService = substationAppService;
            _containerSizeAppService = containerSizeAppService;
            _containerAppService = containerAppService;

        }
        public async Task OnGetAsync()
        {
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
            if (CopyId == null)
            {
                var exportBooking = await _exportBookingAppService.GetAsync(Id);
                ExportBooking = ObjectMapper.Map<ExportBookingDto, CreateUpdateExportBookingDto>(exportBooking);
                CreateUpdateContainerBooking = await _containerAppService.GetContainerByBookingId(ExportBooking.Id);
            }
            else 
            {
                QueryInvoiceDto query = new QueryInvoiceDto();
                var exportBooking = await _exportBookingAppService.GetAsync(CopyId.Value);
                query.ParentId = CopyId;
                ExportBooking = ObjectMapper.Map<ExportBookingDto, CreateUpdateExportBookingDto>(exportBooking);
               var SoNo=await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "ExportBooking_SoNo" });
                ExportBooking.SoNo =SoNo!=null?SoNo:ExportBooking.SoNo+" Copy";
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
            await FillTradePartnerAsync();
            await FillPortAsync();
            await FillSubstationAsync();
            await FillContainerAsync(); 
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ExportBooking.ExtraProperties == null)
            {
                ExportBooking.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
            }

            if (Commodities != null && Commodities.Any())
            {
                ExportBooking.ExtraProperties.Remove("Commodities");
                ExportBooking.ExtraProperties.Add("Commodities", Commodities);
            }
            await _exportBookingAppService.UpdateAsync(Id, ExportBooking);
            if (CreateUpdateContainerBooking is not null)
            {
                QueryContainerDto query = new QueryContainerDto();
                query.QueryId = ExportBooking.Id;
                await _containerAppService.DeleteByBookingIdAsync(query);
                CreateUpdateContainerBooking.BookingId = ExportBooking.Id;
                await _containerAppService.CreateAsync(CreateUpdateContainerBooking);

            }
            return NoContent();
        }

        #region FillReferenceNumberAsync()
        private async Task FillReferenceNumberAsync()
        {
            var referenceLookup = await _ajaxDropdownAppService.GetReferenceItemsByTypeAsync(new QueryDto());

            referenceLookup = referenceLookup.Where(w => !string.IsNullOrEmpty(w.ReferenceNo)).ToList();

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

        #region FillTradePartnerAsync()
        private async Task FillTradePartnerAsync()
        {
            var tradePartnerLookup = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            TradePartnerLookupList = tradePartnerLookup.Items
                                                .Select(x => new SelectListItem(x.TPName + " / " + x.TPCode, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillPortAsync()
        private async Task FillPortAsync()
        {
            var lookup = await _portsManagementAppService.QueryListAsync();

            PortsManagementLookupList = lookup.Select(x => new SelectListItem(x.PortName, x.Id.ToString(), false))
                                   .ToList();
        }
        #endregion

        #region FillSubstationAsync()
        private async Task FillSubstationAsync()
        {
            var substatiosLookup = await _substationAppService.GetSubstationsAsync(new QueryDto());

            SubstationLookupList = substatiosLookup
                                                .Select(x => new SelectListItem(x.SubstationName, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillContainerAsync()
        private async Task FillContainerAsync()
        {
            var containerLookup = await _containerSizeAppService.QueryListAsync(new QueryDto());

            ContainerLookupList = containerLookup.Items
                                                 .Select(x => new SelectListItem(x.ContainerCode, x.Id.ToString(), false))
                                                 .ToList();
        }
        #endregion
    }
}
