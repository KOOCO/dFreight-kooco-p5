using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.Pages.Settings.ContainerSizes
{
    public class IndexModel : PageModel
    {
        public string Search { get; set; }
        [BindProperty]
        public List<SelectListItem> StatusList { get; set; }
        public bool? Active { get; set; }
        public void OnGet()
        {
            StatusList = new List<SelectListItem>();
            StatusList.Add(new SelectListItem { Value = "", Text = "All" });
            StatusList.Add(new SelectListItem { Value = "1", Text = "Yes" });
            StatusList.Add(new SelectListItem { Value = "0", Text = "No" });
        }
    }
}
