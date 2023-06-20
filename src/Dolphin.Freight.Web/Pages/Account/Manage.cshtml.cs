using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Volo.Abp.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.Validation;
using Volo.Abp.Users;
using Volo.Abp.Data;

namespace Volo.Abp.Account.Web.Pages.Account.Custom;

public class ManageModel : AccountPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    public string Gender { get; set; }
    public string Name { get; set; }

    public ProfileManagementPageCreationContextCustom ProfileManagementPageCreationContext { get; private set; }

    protected ProfileManagementPageOptionsCustom Options { get; }

    private readonly IIdentityUserAppService _identityUserAppService;
    private readonly ICurrentUser _currentUser;

    public ManageModel(IOptions<ProfileManagementPageOptionsCustom> options, IIdentityUserAppService identityUserAppService, ICurrentUser currentUser)
    {
        Options = options.Value;
        _identityUserAppService = identityUserAppService;
        _currentUser = currentUser;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ProfileManagementPageCreationContext = new ProfileManagementPageCreationContextCustom(ServiceProvider);

        var user = await _identityUserAppService.GetAsync(_currentUser.Id.Value);

        Gender = Convert.ToString(user.GetProperty("Gender"));

        Name = string.Concat(user.Name, " ", user.Surname);

        foreach (var contributor in Options.Contributors)
        {
            await contributor.ConfigureAsync(ProfileManagementPageCreationContext);
        }

        if (ReturnUrl != null)
        {
            if (!Url.IsLocalUrl(ReturnUrl) &&
                !ReturnUrl.StartsWith(UriHelper.BuildAbsolute(Request.Scheme, Request.Host, Request.PathBase).RemovePostFix("/")) &&
                !AppUrlProvider.IsRedirectAllowedUrl(ReturnUrl))
            {
                ReturnUrl = null;
            }
        }

        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
