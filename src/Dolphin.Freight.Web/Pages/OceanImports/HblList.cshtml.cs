using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.OceanImports
{
    public class HblListModel : PageModel
    {
        private readonly ISubstationAppService _substationAppService;
        public string Search { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        public List<SelectListItem> ShipModeLookupList { get; set; }
        public List<SelectListItem> BlTypeList { get; set; }
        public HblListModel(ISubstationAppService substationAppService, ITradePartnerAppService tradePartnerAppService, IAjaxDropdownAppService ajaxDropdownAppService,
                          ISysCodeAppService sysCodeAppService)
        {
            _substationAppService = substationAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _substationAppService = substationAppService;
            _sysCodeAppService = sysCodeAppService;

        }


        public async Task OnGetAsync()
        {
            await FillSubstationAsync();
            await FillTradePartnerAsync();
            await FillShipModeAsync();
            await FillBlType();
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
        #region FillShipModeAsync()
        private async Task FillShipModeAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "ShipModeId" });

            ShipModeLookupList = lookUp
                                        .Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion
        public async Task FillBlType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "BlTypeId" });

            BlTypeList = blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }
    }
}
