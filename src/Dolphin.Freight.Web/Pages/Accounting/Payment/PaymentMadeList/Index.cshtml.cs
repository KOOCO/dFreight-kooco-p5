using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dolphin.Freight.Web.Pages.PaymentMadeList
{ 
    public class IndexModel : PageModel
    {
        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> BankList { get; set; }
        private readonly ISysCodeAppService _sysCodeAppService;
        public IndexModel(ISysCodeAppService sysCodeAppService) { 
        
        _sysCodeAppService= sysCodeAppService;
        }
        public async Task OnGetAsync()
        {
           var queryDto = new QueryDto();
            queryDto.QueryType = "Category";
           var sysCodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(queryDto);
           var rs = sysCodes.Where(x => x.CodeType.Equals("Category")).ToList();
            CategoryList =rs.Select(x => new SelectListItem(x.ShowName , x.Id.ToString(), false))
                                                .ToList();
            BankList = new List<SelectListItem>
            {new SelectListItem { Value = "", Text = "" },
                new SelectListItem { Value = "UOB", Text = "UOB" },
                new SelectListItem { Value = "????-USD", Text = "\u83EF\u5357\u9280\u884C-USD" },

                new SelectListItem { Value = "Chase Credit Card Example", Text = "Chase Credit Card Example" },

                new SelectListItem { Value = "Credit Card Example", Text = "Credit Card Example" },
                new SelectListItem { Value = "Petty Cash", Text = "Petty Cash" },
                new SelectListItem { Value = "Petty Cash", Text = "Petty Cash" },
                new SelectListItem { Value = "testing", Text = "testing" },
                new SelectListItem { Value = "OCBC", Text = "OCBC" },
                new SelectListItem { Value = "HSBC", Text = "HSBC" },
                new SelectListItem { Value = "UOB", Text = "UOB" },
                new SelectListItem { Value = "???", Text = "\u96F6\u7528\u91D1" }
            };
        }
    }
}
