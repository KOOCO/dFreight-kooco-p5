
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanImports;
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

namespace Dolphin.Freight.Web.Pages.OceanImports
{
    public class CreateMblModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> PortsManagementLookupList { get; set; }
        public List<SelectListItem> SalesTypeLookupList { get; set; }
        public List<SelectListItem> CargoTypeLookupList { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportMblDto? OceanImportMbl { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportHblDto? OceanImportHbl { get; set; }
        [BindProperty]
        public int AddHbl { get; set; } = 0;
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IPortsManagementAppService _portsManagementAppService;
        private readonly ICurrentUser _currentUser;
        public CreateMblModel( IOceanImportMblAppService oceanImportMblAppService,IOceanImportHblAppService oceanImportHblAppService, ISysCodeAppService sysCodeAppService, IPortsManagementAppService portsManagementAppService, ITradePartnerAppService tradePartnerAppService, ISubstationAppService substationAppService, ICurrentUser currentUser)
        {
            _oceanImportMblAppService = oceanImportMblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _sysCodeAppService = sysCodeAppService;
            _substationAppService = substationAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _portsManagementAppService = portsManagementAppService;
            _currentUser = currentUser;
        }
        public async Task OnGetAsync()
        {
            OceanImportHbl = new CreateUpdateOceanImportHblDto();
            OceanImportMbl = new CreateUpdateOceanImportMblDto();
            await FillSubstationAsync();
            await FillTradePartnerAsync();
            await FillPortAsync();
        }
        public async Task<JsonResult> OnPostAsync()
        {
            if (OceanImportMbl.PolEtd == null) OceanImportMbl.PostDate = DateTime.Now;
            else OceanImportMbl.PostDate = OceanImportMbl.PolEtd;
            OceanImportMbl.FilingNo = await _sysCodeAppService.GetSystemNoAsync(new (){ QueryType= "OceanImportMbl_FilingNo" });
            var mbl = await _oceanImportMblAppService.CreateAsync(OceanImportMbl);
            OceanImportHbl.MblId = mbl.Id;
            Id = mbl.Id;
            if (AddHbl == 1)
            {
                if (OceanImportHbl.IsCreateBySystem)
                {
                    OceanImportHbl.HblNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "OceanImportHbl_HblNo" });
                }
                QueryDto query  = new QueryDto();
                query.QueryType = "CardColorId";
                var syscodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(query);
                if (syscodes != null && syscodes.Count >0) 
                {
                    var syscode = syscodes[0];
                    OceanImportHbl.CardColorId = syscode.Id;
                }
                await _oceanImportHblAppService.CreateAsync(OceanImportHbl);

            }

            Dictionary<string, Guid> rs = new()
            {
                { "id", Id.Value }
            };
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


