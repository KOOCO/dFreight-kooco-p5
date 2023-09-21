using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.Sales.TradePartner
{
    public class ListModel : PageModel
    {
        public List<SelectListItem> AccountGroupNameLookupList { get; set; }
        public List<SelectListItem> CountryLookupList { get; set; }
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IAccountGroupAppService _accountGroupAppService;
        public string Search { get; set; }
        public ListModel(IAccountGroupAppService accountGroupAppService, ITradePartnerAppService tradePartnerAppService) { 
        _accountGroupAppService= accountGroupAppService;
            _tradePartnerAppService= tradePartnerAppService;
        }
        public async Task OnGetAsync()
        {
            var accountGroupNameLookup = await _accountGroupAppService.GetAccountGroupNameLookupAsync();
            AccountGroupNameLookupList = accountGroupNameLookup.Items
                                                .Select(x => new SelectListItem(x.AccountGroupName, x.Id.ToString(), false))
                                                .ToList<SelectListItem>();
            var countryNameLookup = await _tradePartnerAppService.GetCountriesLookupAsync();
            CountryLookupList = countryNameLookup.Items
                                                .Select(x => new SelectListItem(x.ShowName + " " + x.CountryName, x.Id.ToString(), false))
                                                .ToList();
        }
    }
}
