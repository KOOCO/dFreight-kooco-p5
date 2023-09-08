using Dolphin.Freight.Web.ViewModels.ExcelExportConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.Shared
{
    public class _ExcelExportConfigurationModel : PageModel
    {
        public _ExcelExportConfigurationModel(ExcelExportConfigurationViewModel Model)
        {
           
        }

        [BindProperty]
        public ExcelExportConfigurationViewModel ViewModel { get; set; }
        public void OnGet()
        {
           
        }

        public void OnPost() 
        { 
        
        }    
       
    }
}
