using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dolphin.Freight.Web.Pages.AirExports
{
    public class HawbListModel : PageModel
    {
        public string Search { get; set; }
        public void OnGet()
        {
        }
    }
}
