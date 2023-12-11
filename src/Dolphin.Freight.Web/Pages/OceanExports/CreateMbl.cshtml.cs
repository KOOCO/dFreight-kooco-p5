
using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanExports;
//using Dolphin.Freight.Migrations;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dolphin.Freight.Web.Pages.OceanExports
{
    public class CreateMblModel : FreightPageModel
    {
        public Guid MblId { get; set; }
        public Guid HblId { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsCopyVesselInfoAndShippingSchedule { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool CopyAccountingInformation {  get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsCopyContainerInfo {  get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsAP {  get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsAR {  get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsDC {  get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? Hid { get; set; }
     
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> BlTypeLookupList { get; set; }
        public List<SelectListItem> PortsManagementLookupList { get; set; }
        [BindProperty]
        public CreateUpdateOceanExportMblDto OceanExportMbl { get; set; }
        [BindProperty]
        public CreateUpdateOceanExportHblDto OceanExportHbl { get; set; }
        [BindProperty]
        public OceanExportMblDto OceanExportMblDto { get; set; }
        [BindProperty]
        public int AddHbl { get; set; } = 0;
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IPortsManagementAppService _portsManagementAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IInvoiceBillAppService _invoiceBillAppService;
        private readonly IContainerAppService _containerAppService;
        private readonly ICurrentUser _currentUser;
        private Guid? releasedBy;
        public CreateMblModel(IOceanExportMblAppService oceanExportMblAppService, IPortsManagementAppService portsManagementAppService, 
                            ITradePartnerAppService tradePartnerAppService, ISubstationAppService substationAppService, 
                            IOceanExportHblAppService oceanExportHblAppService, ISysCodeAppService sysCodeAppService,
                            ICurrentUser currentUser, IInvoiceAppService invoiceAppService, IInvoiceBillAppService invoiceBillAppService,
                            IContainerAppService containerAppService)
        {
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _sysCodeAppService = sysCodeAppService;
            _substationAppService = substationAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _portsManagementAppService = portsManagementAppService;
            _invoiceAppService = invoiceAppService;
            _invoiceBillAppService = invoiceBillAppService;
            _containerAppService = containerAppService;
            _currentUser = currentUser;
        }
        public async Task OnGetAsync()
        {
            if (Id != Guid.Empty)
            {
                OceanExportMblDto = await _oceanExportMblAppService.GetAsync(Id);

                OceanExportMbl = ObjectMapper.Map<OceanExportMblDto, CreateUpdateOceanExportMblDto>(OceanExportMblDto);
                OceanExportMbl.MblNo = null;
                OceanExportMbl.FilingNo = null;

                if (!IsCopyVesselInfoAndShippingSchedule)
                {
                    OceanExportMbl.VesselName = null;
                    OceanExportMbl.Voyage = null;
                    OceanExportMbl.PolEtd = null;
                    OceanExportMbl.PodEta = null;
                    OceanExportMbl.PorEtd = null;
                    OceanExportMbl.DelEta = null;
                    OceanExportMbl.FdestEta = null;
                }
            }
            else
            {
                OceanExportHbl = new CreateUpdateOceanExportHblDto();
                OceanExportMbl = new CreateUpdateOceanExportMblDto();

                releasedBy = _currentUser.Id;
            }

            await FillBlType();
            await FillSubstationAsync();
            await FillTradePartnerAsync();
            await FillPortAsync();
        }
        public async Task<JsonResult> OnPostAsync()
        {
            if (Id != Guid.Empty)
            {
                Dictionary<Guid, Guid> hblIds = new();

                var oldMblId = Id;

                OceanExportMbl.Id = Guid.Empty;

                var inputDto = await _oceanExportMblAppService.CreateAsync(OceanExportMbl);

                var hblList = await _oceanExportHblAppService.GetHblCardsById(oldMblId);

                foreach (var item in hblList)
                {
                    var oldHblId = item.Id;

                    item.Id = Guid.Empty;

                    item.MblId = inputDto.Id;

                    var inputHblDto = await _oceanExportHblAppService.CreateAsync(ObjectMapper.Map<OceanExportHblDto, CreateUpdateOceanExportHblDto>(item));

                    await _invoiceAppService.CreateReplicaAccounting(oldHblId, inputHblDto.Id, 1, IsAP, IsAR, IsDC);

                    hblIds.Add(oldHblId, inputHblDto.Id);
                }

                if (CopyAccountingInformation)
                {
                    await _invoiceAppService.CreateReplicaAccounting(oldMblId, inputDto.Id, 3, IsAP, IsAR, IsDC);
                }

                if (IsCopyContainerInfo)
                {
                    await _containerAppService.CreateMblHblContainerForCopiedOE_OI(oldMblId, inputDto.Id, hblIds);
                }

                Dictionary<string, object> rs = new()
                {
                    { "id", inputDto.Id }
                };
                rs.Add("HId", Hid);

                return new JsonResult(rs);
            }
            else
            {
                if (OceanExportMbl.PolEtd == null) OceanExportMbl.PostDate = DateTime.Now;
                else OceanExportMbl.PostDate = OceanExportMbl.PolEtd;
                OceanExportMbl.FilingNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "OceanExportMbl_FilingNo" });
                var mbl = await _oceanExportMblAppService.CreateAsync(OceanExportMbl);
                Id = mbl.Id;
                if (OceanExportHbl is not null && !string.IsNullOrEmpty(OceanExportHbl.HblNo))
                {
                    if (OceanExportHbl.CardColorId is null)
                    {
                        SysCode sysCode = new SysCode();
                        sysCode.CodeType = "CardColorId";
                        sysCode.CodeValue = OceanExportHbl.CardColorValue;
                        sysCode.ShowName = OceanExportHbl.HblNo;

                        var newSysCode = await _sysCodeAppService.CreateAsync(ObjectMapper.Map<SysCode, CreateUpdateSysCodeDto>(sysCode));

                        OceanExportHbl.CardColorId = newSysCode.Id;
                    }

                    if (OceanExportHbl.ExtraProperties == null)
                    {
                        OceanExportHbl.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                    }

                    OceanExportHbl.MblId = Id;

                    await _oceanExportHblAppService.CreateAsync(OceanExportHbl);
                }

                Dictionary<string, object> rs = new()
                {
                    { "id", Id

                    }
                };
                rs.Add("HId", Hid);

                return new JsonResult(rs);
            }
        }

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

        #region FillPortAsync()
        private async Task FillPortAsync()
        {
            var lookup = await _portsManagementAppService.QueryListAsync();

            PortsManagementLookupList = lookup.Select(x => new SelectListItem(x.PortName, x.Id.ToString(), false))
                                   .ToList();
        }
        #endregion

        public async Task FillBlType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "BlTypeId" });

            BlTypeLookupList = blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }
    }
}
