using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dolphin.Freight.Web.Pages.Settings.PackageUnits
{
    public class IndexModel : PageModel
    {
        public string Search { get; set; }
        public void OnGet()
        {
        }
    }
}
