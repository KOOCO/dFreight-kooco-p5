using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dolphin.Freight.Account.Languages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HPSF;
using NUglify.Helpers;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Volo.Abp.Validation;

namespace Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.Settings;

public class AccountProfileSettingsManagementGroupViewComponentCustom : AbpViewComponent
{
    private readonly IIdentityUserAppService _identityUserAppService;
    private readonly ILanguagesAppService _languagesAppService;
    private readonly ICurrentUser _currentUser;
    public AccountProfileSettingsManagementGroupViewComponentCustom(
        IIdentityUserAppService identityUserAppService,
        ILanguagesAppService languagesAppService,
        ICurrentUser currentUser)
    {
        _identityUserAppService = identityUserAppService;
        _languagesAppService = languagesAppService;
        _currentUser = currentUser;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        SettingsInfoModel model = new ();

        var user = await _identityUserAppService.GetAsync(_currentUser.Id.Value);

        Guid languageId = Guid.Empty;

        string extraProperty = Convert.ToString(user.GetProperty("LanguageId"));

        if (!string.IsNullOrEmpty(extraProperty))
            languageId = Guid.Parse(extraProperty);

        var languages = await _languagesAppService.GetListAsync(new Application.Dtos.PagedAndSortedResultRequestDto());
        model.LanguageLookupList = languages.Items
                                            .Select(s => new SelectListItem(s.Name, s.Id.ToString(), s.Id == languageId))
                                            .OrderByDescending(o => o.Value)
                                            .ToList();

        return View("~/Pages/Account/ProfileManagementGroup/Settings/Default.cshtml", model);
    }

    public class SettingsInfoModel
    {
        [Display(Name = "Language")]
        [DisableAuditing]
        public string Language { get; set; }
        public List<SelectListItem> LanguageLookupList { get; set; }

    }
}
