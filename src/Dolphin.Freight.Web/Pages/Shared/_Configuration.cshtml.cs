using Dolphin.Freight.Web.ViewModels.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.Shared
{
    public class _ConfigurationModel : PageModel
    {
        public _ConfigurationModel(ConfigurationViewModel Model)
        {
           
        }

        [BindProperty]
        public ConfigurationViewModel ViewModel { get; set; }
        public void OnGet()
        {
           
        }

        public void OnPost() 
        { 
        
        }    
       
    }
}
