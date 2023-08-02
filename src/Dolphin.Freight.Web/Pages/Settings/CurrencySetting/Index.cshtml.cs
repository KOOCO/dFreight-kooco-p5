using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dolphin.Freight.Web.Pages.CurrencySetting
{
    public class IndexModel : PageModel
    {
        public string Search { get; set; }
        public void OnGet()
        {
        }
    }
}
