using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using System.Linq;
using Dolphin.Freight.Settinngs.SysCodes;
using NUglify.JavaScript.Visitors;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.Ports;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;

namespace Dolphin.Freight.Web.Pages.OceanExports.VesselScheduleas
{
    public class CreateModel : FreightPageModel
    {
     
       public Guid Id { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }

        public List<SelectListItem> BlTypeList { get; set; }

        public List<SelectListItem> OblTypeList { get; set; }

        public List<SelectListItem> TransPortList { get; set; }
        public List<SelectListItem> MblCarrierList { get; set; }
        public List<SelectListItem> SvcTermList { get; set; }
        public List<SelectListItem> PortLookupList { get; set; }
        public List<SelectListItem> ShipModeLookupList { get; set; }
        public List<SelectListItem> FreightTermLookupList { get; set; }
        [BindProperty]
        public List<CreateUpdateContainerDto> CreateUpdateContainerDtos { get; set; }
        [BindProperty]
        public CreateUpdateExportBookingDto ExportBookingDto { get; set; } = null;
        [BindProperty]
        public CreateUpdateContainerDto CreateUpdateContainerBooking { get; set; }
        [BindProperty]
        public CreateUpdateVesselScheduleDto VesselSchedule { get; set; }
        [BindProperty]
        public List<ManifestCommodity> Commodities { get; set; }
        private readonly IVesselScheduleAppService _vesselScheduleAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IPortAppService _portAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly IContainerAppService _containerAppService;
        private readonly IExportBookingAppService _exportBookingAppService;
        public CreateModel(VesselScheduleAppService vesselScheduleAppService, ITradePartnerAppService tradePartnerAppService, ISubstationAppService substationAppService,
            IAirportAppService airportAppService, IPackageUnitAppService packageUnitAppService, ISysCodeAppService sysCodeAppService, IPortAppService portAppService, IAjaxDropdownAppService ajaxDropdownAppService,
            IContainerAppService containerAppService, IExportBookingAppService exportBookingAppService) 
        {
            _vesselScheduleAppService = vesselScheduleAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _sysCodeAppService = sysCodeAppService;
            _portAppService = portAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _containerAppService= containerAppService;
            _exportBookingAppService = exportBookingAppService;
        }
        public async Task OnGetAsync()
        {
            VesselSchedule = new CreateUpdateVesselScheduleDto();
            await FillTradePartnerAsync();
            await FillAirportAsync();
            await FillPackageUnitAsync();
            await FillSubstationAsync();
            await FillBlType();
            await FillOBlType();
            await FillMblCarrierType();
            await FillTransPortType();
            await FillPortAsync();
            await FillSvcTermType();
            await FillShipModeAsync();
            await FillFreightTermAsync();

            FillWtValOther();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var vesselSchedule = await _vesselScheduleAppService.CreateAsync(VesselSchedule);
            foreach (var dto in CreateUpdateContainerDtos)
            {
                var a = dto.IsDeleted;
                dto.VesselId = vesselSchedule.Id;
                if (dto.Status == 0) await _containerAppService.CreateAsync(dto);
            }

            if (ExportBookingDto is not null && !string.IsNullOrEmpty(ExportBookingDto.SoNo) || ExportBookingDto.IsCreateBySystem)
            {
                if (ExportBookingDto.IsCreateBySystem)
                {
                    ExportBookingDto.SoNo = await _sysCodeAppService.GetSystemBookingNoAsync(new() { QueryType = "ExportBooking_SoNo" });
                }

                ExportBookingDto.Id = Guid.Empty;
                if (ExportBookingDto.ExtraProperties == null)
                {
                    ExportBookingDto.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                }

                if (Commodities != null && Commodities.Any())
                {
                    ExportBookingDto.ExtraProperties.Remove("Commodities");
                    ExportBookingDto.ExtraProperties.Add("Commodities", Commodities);
                }

                var addBooking = ObjectMapper.Map<CreateUpdateExportBookingDto, CreateUpdateExportBookingDto>(ExportBookingDto);
                addBooking.VesselScheduleId = vesselSchedule.Id;

                var updatedBooking = await _exportBookingAppService.CreateAsync(addBooking);

                if (CreateUpdateContainerBooking is not null)
                {
                    QueryContainerDto queryContainerDto = new QueryContainerDto() { QueryId = updatedBooking.Id };
                    await _containerAppService.DeleteByBookingIdAsync(queryContainerDto);
                    CreateUpdateContainerBooking.BookingId = updatedBooking.Id;
                    await _containerAppService.CreateAsync(CreateUpdateContainerBooking);
                }
            }

            Dictionary<string, Guid> rs = new Dictionary<string, Guid>
            {
                { "id", vesselSchedule.Id }
            };
            return new JsonResult(rs);
        }

        #region FillTradePartnerAsync()
        private async Task FillTradePartnerAsync()
        {
            var tradePartnerLookup = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            TradePartnerLookupList = tradePartnerLookup.Items
                                                .Select(x => new SelectListItem(x.TPName + " / " + x.TPCode, x.Id.ToString(), false))
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

        #region FillAirportAsync()
        private async Task FillAirportAsync()
        {
            var airportLookup = await _airportAppService.GetAirportLookupAsync();
            AirportLookupList = airportLookup.Items
                                                .Select(x => new SelectListItem(x.AirportIataCode + " " + x.AirportName, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillPackageUnitAsync()
        private async Task FillPackageUnitAsync()
        {
            var packageUnitLookup = await _packageUnitAppService.GetPackageUnitsLookupAsync();
            PackageUnitLookupList = packageUnitLookup.Items
                                                .Select(x => new SelectListItem(x.PackageName, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillWtVal()
        private void FillWtValOther()
        {
            WtValOtherList = new List<SelectListItem>
            {
                new SelectListItem { Value = "PPD", Text = "PPD"},
                new SelectListItem { Value = "COLL", Text = "COLL"}
            };
        }
        #endregion

        #region MyRegion
        public async Task FillBlType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "BlTypeId" });

            BlTypeList = blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }

        public async Task FillOBlType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "OblTypeId" });

            OblTypeList = blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }

        public async Task FillTransPortType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "TransPort1Id" });

            TransPortList = blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }

        public async Task FillMblCarrierType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "MblCarrierId" });

            MblCarrierList = blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }

        public async Task FillSvcTermType()
        {
            var svcTermLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "SvcTermFromId" });

            SvcTermList = svcTermLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
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

        #region FillFreightTermAsync()
        private async Task FillFreightTermAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "FreightTermId" });

            FreightTermLookupList = lookUp
                                        .Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion

        #region FillPortAsync()
        private async Task FillPortAsync()
        {
            var portLookup = await _portAppService.GetListAsync(new QueryDto());
            PortLookupList = portLookup.Items
                                            .Select(x => new SelectListItem(x.PortName, x.Id.ToString(), false))
                                            .ToList();
        }
        #endregion


        #region SetAirImportFileNo() 
        /// <summary>
        /// ³]©wAir ImportªºFile No
        /// </summary>
        private string SetAirImportFileNo()
        {
            string today = DateTime.Now.ToString("yyyyMMddhhmmss");
            string AIFileNo = "AIM-" + today;
            return AIFileNo;
        }
        #endregion
    }
}
