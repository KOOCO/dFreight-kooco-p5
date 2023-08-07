using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dolphin.Freight.Web.Pages.AirImports
{
    public class MawbListModel : PageModel
    {
        public string Search { get; set; }
        public void OnGet()
        {
        }
    }
}
