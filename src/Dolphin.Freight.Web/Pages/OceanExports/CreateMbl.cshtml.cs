
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Migrations;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? Hid { get; set; }
     
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
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
        private readonly ICurrentUser _currentUser;
        private Guid? releasedBy;
        public CreateMblModel( IOceanExportMblAppService oceanExportMblAppService, IPortsManagementAppService portsManagementAppService, ITradePartnerAppService tradePartnerAppService, ISubstationAppService substationAppService, IOceanExportHblAppService oceanExportHblAppService, ISysCodeAppService sysCodeAppService,
            ICurrentUser currentUser)
        {
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _sysCodeAppService = sysCodeAppService;
            _substationAppService = substationAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _portsManagementAppService = portsManagementAppService;
            _currentUser = currentUser;
        }
        public async Task OnGetAsync()
        {
            OceanExportHbl = new CreateUpdateOceanExportHblDto();
            OceanExportMbl = new CreateUpdateOceanExportMblDto();

            releasedBy = _currentUser.Id;

            await FillSubstationAsync();
            await FillTradePartnerAsync();
            await FillPortAsync();
        }
        public async Task<JsonResult> OnPostAsync()
        {
            if (OceanExportMbl.PolEtd == null) OceanExportMbl.PostDate = DateTime.Now;
            else OceanExportMbl.PostDate = OceanExportMbl.PolEtd;
            OceanExportMbl.FilingNo = await _sysCodeAppService.GetSystemNoAsync(new (){ QueryType= "OceanExportMbl_FilingNo" });
            var mbl = await _oceanExportMblAppService.CreateAsync(OceanExportMbl);
            Id = mbl.Id;
            if (OceanExportHbl is not null && !string.IsNullOrEmpty(OceanExportHbl.HblNo))
            {
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
    }
}
