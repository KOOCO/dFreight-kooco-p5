using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dolphin.Freight.Web.Pages.OceanExports
{
    public class HblListModel : PageModel
    {
        public string Search { get; set; }
        public void OnGet()
        {
        }
    }
}
