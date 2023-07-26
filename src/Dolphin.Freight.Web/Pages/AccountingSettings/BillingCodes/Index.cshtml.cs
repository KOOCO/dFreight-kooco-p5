using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace Dolphin.Freight.Web.Pages.AccountingSetting.BillingCodes
{
    public class IndexModel : PageModel
    {
        public string Code { get; set; }
        [DisplayName("Name(Eng./Local)")]
        public string Name { get; set; }
        [DisplayName("G/L Code")]
        public string GlCode { get; set; }
        [DisplayName("G/L Description")]
        public string GlDescription { get; set; }
        public void OnGet()
        {
        }
    }
}
