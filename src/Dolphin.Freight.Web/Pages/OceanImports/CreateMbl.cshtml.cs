
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq;
using Dolphin.Freight.Settings.PortsManagement;

namespace Dolphin.Freight.Web.Pages.OceanImports
{
    public class CreateMblModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportMblDto? OceanImportMbl { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> PortsManagementLookupList { get; set; }

        [BindProperty]
        public CreateUpdateOceanImportHblDto? OceanImportHbl { get; set; }
        [BindProperty]
        public int AddHbl { get; set; } = 0;
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IPortsManagementAppService _portsManagementAppService;

        public CreateMblModel( IOceanImportMblAppService oceanImportMblAppService, IPortsManagementAppService portsManagementAppService, IOceanImportHblAppService oceanImportHblAppService, ISysCodeAppService sysCodeAppService, ITradePartnerAppService tradePartnerAppService)
        {
            _oceanImportMblAppService = oceanImportMblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _sysCodeAppService = sysCodeAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _portsManagementAppService = portsManagementAppService;

        }
        public async Task OnGetAsync()
        {
            OceanImportHbl = new CreateUpdateOceanImportHblDto();
            OceanImportMbl = new CreateUpdateOceanImportMblDto();

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
