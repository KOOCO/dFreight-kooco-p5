using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.Password;
using Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;
using Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.Settings;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Volo.Abp.Account.Web.ProfileManagement;

public class AccountProfileManagementPageContributorCustom : IProfileManagementPageContributorCustom
{
    public async Task ConfigureAsync(ProfileManagementPageCreationContextCustom context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AccountResource>>();

        context.Groups.Add(
            new ProfileManagementPageGroupCustom(
                "Volo.Abp.Account.PersonalInfo",
                "Basic Information",
                typeof(AccountProfilePersonalInfoManagementGroupViewComponentCustom)
            )
        );

        context.Groups.Add(
           new ProfileManagementPageGroupCustom(
               "Volo.Abp.Account.Settings",
               "Set up",
               typeof(AccountProfileSettingsManagementGroupViewComponentCustom)
           )
       );


        if (await IsPasswordChangeEnabled(context))
        {
            context.Groups.Add(
                new ProfileManagementPageGroupCustom(
                    "Volo.Abp.Account.Password",
                    "Reset Password",
                    typeof(AccountProfilePasswordManagementGroupViewComponentCustom)
                )
            );
        }
    }

    protected virtual async Task<bool> IsPasswordChangeEnabled(ProfileManagementPageCreationContextCustom context)
    {
        var userManager = context.ServiceProvider.GetRequiredService<IdentityUserManager>();
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

        var user = await userManager.GetByIdAsync(currentUser.GetId());

        return !user.IsExternal;
    }
}
