using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.Web.ViewModels.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.Password;

namespace Dolphin.Freight.Web.Controllers
{
    public class UserController : Controller
    {
        public IAirExportMawbAppService _airExportMawbAppService { get; set; }
        public UserController(IAirExportMawbAppService airExportMawbAppService)
        {
            _airExportMawbAppService = airExportMawbAppService;
        }

        public PartialViewResult ResetPassword()
        {
            AccountProfilePasswordManagementGroupViewComponentCustom.ChangePasswordInfoModel model = new();
            model.HideOldPasswordInput = true;

            return PartialView("Pages/Account/_ResetPassword.cshtml", model);
        }
    }
}
