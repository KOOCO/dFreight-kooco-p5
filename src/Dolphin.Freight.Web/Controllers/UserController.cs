using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
//using Dolphin.Freight.Migrations;
using Dolphin.Freight.Web.ViewModels.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.Password;
using Volo.Abp.Identity;

namespace Dolphin.Freight.Web.Controllers
{
    public class UserController : Controller
    {
        public IIdentityUserAppService _identityUserAppService { get; set; }
        public UserController(IIdentityUserAppService identityUserAppService)
        {
            _identityUserAppService = identityUserAppService;
        }

        public async Task<PartialViewResult> ResetPassword()
        {
            var user = await _identityUserAppService.GetAsync(Guid.Parse("8B175BF1-5F14-06F7-D293-3A0ADA6A1696"));
            
            AccountProfilePasswordManagementGroupViewComponentCustom.ChangePasswordInfoModel model = new();
            model.HideOldPasswordInput = true;

            return PartialView("Pages/Account/_ResetPassword.cshtml", model);
        }
    }
}
