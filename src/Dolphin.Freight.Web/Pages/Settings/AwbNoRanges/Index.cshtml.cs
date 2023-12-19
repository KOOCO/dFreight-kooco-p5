using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Dolphin.Freight.Web.Pages.AwbNoRanges
{
    public class IndexModel : PageModel
    {
        public DateTime? CreatedDate { get; set; }
        public string Prefix { get; set; }
        public string Search { get; set; }
        public Guid? CarrierId { get; set; }
        public void OnGet()
        {
        }
    }
}
