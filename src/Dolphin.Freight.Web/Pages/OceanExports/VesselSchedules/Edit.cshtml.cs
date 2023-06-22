using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas;
using Dolphin.Freight.Settinngs.PackageUnits;
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
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.Web.Pages.OceanExports.VesselSchedules
{
    public class EditModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ShowMsg { get; set; } = false;
        [BindProperty]
        public CreateUpdateVesselScheduleDto VesselSchedule { get; set; }

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
        public List<SelectListItem> SvgTermLookupList { get; set; }
        public List<SelectListItem> ShipModeLookupList { get; set; }
        public List<SelectListItem> FreightTermLookupList { get; set; }

        private readonly IVesselScheduleAppService _vesselScheduleAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IPortAppService _portAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        public EditModel(VesselScheduleAppService vesselScheduleAppService, ITradePartnerAppService tradePartnerAppService, ISubstationAppService substationAppService,
            IAirportAppService airportAppService, IPackageUnitAppService packageUnitAppService, ISysCodeAppService sysCodeAppService, IPortAppService portAppService, IAjaxDropdownAppService ajaxDropdownAppService)
        {
            _vesselScheduleAppService = vesselScheduleAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _sysCodeAppService = sysCodeAppService;
            _portAppService = portAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
        }
        public async Task OnGetAsync()
        {
            var vesselSchedule = await _vesselScheduleAppService.GetAsync(Id);
            VesselSchedule  = ObjectMapper.Map<VesselScheduleDto, CreateUpdateVesselScheduleDto>(vesselSchedule);

            await FillTradePartnerAsync();
            await FillSubstationAsync();
            await FillAirportAsync();
            await FillPackageUnitAsync();
            FillWtValOther();
            await FillBlType();
            await FillOBlType();
            await FillTransPortType();
            await FillMblCarrierType();
            await FillPortAsync();
            await FillSvgTermAsync();
            await FillShipModeAsync();
            await FillFreightTermAsync();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _vesselScheduleAppService.UpdateAsync(Id, VesselSchedule);
            return NoContent();
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

        #region FillPortAsync()
        private async Task FillPortAsync()
        {
            var portLookup = await _portAppService.GetListAsync(new Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto());
            PortLookupList = portLookup.Items
                                            .Select(x => new SelectListItem(x.PortName, x.Id.ToString(), false))
                                            .ToList();
        }
        #endregion

        #region FillSvgTermAsync()
        private async Task FillSvgTermAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "SvcTermFromId" });

            SvgTermLookupList = lookUp
                                        .Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
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


        #endregion
    }
}
