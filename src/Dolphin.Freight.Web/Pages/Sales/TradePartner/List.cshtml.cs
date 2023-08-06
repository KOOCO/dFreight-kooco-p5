using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dolphin.Freight.Web.Pages.Sales.TradePartner
{
    public class ListModel : PageModel
    {
        public string Search { get; set; }
        public void OnGet()
        {
        }
    }
}
