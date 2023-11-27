using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dolphin.Freight.EntityFrameworkCore;
using Dolphin.Freight.Localization;
using Dolphin.Freight.MultiTenancy;
using Dolphin.Freight.Web.Menus;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dolphin.Freight.Permissions;
using Wkhtmltopdf.NetCore;
using AutoMapper;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.Account.Web.Pages.Account.Custom;
using Dolphin.Freight.Web.CommonService;

namespace Dolphin.Freight.Web;

[DependsOn(
    typeof(FreightHttpApiModule),
    typeof(FreightApplicationModule),
    typeof(FreightEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpAccountWebIdentityServerModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpTenantManagementWebModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
    )]
public class FreightWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {

    
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(FreightResource),
                typeof(FreightDomainModule).Assembly,
                typeof(FreightDomainSharedModule).Assembly,
                typeof(FreightApplicationModule).Assembly,
                typeof(FreightApplicationContractsModule).Assembly,
                typeof(FreightWebModule).Assembly
            );
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureAuthentication(context, configuration);
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureLocalizationServices();
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureSwaggerServices(context.Services);
        context.Services.AddWkhtmltopdf();
        ConfigureProfileManagementPage();
        context.Services.AddSameSiteCookiePolicy(); // cookie policy to deal with temporary browser incompatibilities

        context.Services.AddSingleton<IDropdownService, DropdownService>();


        //頁面權限設定
        Configure<RazorPagesOptions>(options =>
        {

            #region 設置
            #region IT號碼管理頁面
            options.Conventions.AuthorizePage("/Settings/ItNoRanges/Index", SettingsPermissions.ItNoRanges.Default);
            options.Conventions.AuthorizePage("/Settings/ItNoRanges/CreateModal", SettingsPermissions.ItNoRanges.Create);
            options.Conventions.AuthorizePage("/Settings/ItNoRanges/EditModal", SettingsPermissions.ItNoRanges.Edit);
            #endregion 
            #region AWB號碼管理頁面
            options.Conventions.AuthorizePage("/Settings/AwbRanges/Index", SettingsPermissions.AwbNoRanges.Default);
            options.Conventions.AuthorizePage("/Settings/AwbRanges/CreateModal", SettingsPermissions.AwbNoRanges.Create);
            options.Conventions.AuthorizePage("/Settings/AwbRanges/EditModal", SettingsPermissions.AwbNoRanges.Edit);
            #endregion
            #region 包裝管理頁面
            options.Conventions.AuthorizePage("/Settings/PackageUnits/Index", SettingsPermissions.PackageUnits.Default);
            options.Conventions.AuthorizePage("/Settings/PackageUnits/CreateModal", SettingsPermissions.PackageUnits.Create);
            options.Conventions.AuthorizePage("/Settings/PackageUnits/EditModal", SettingsPermissions.PackageUnits.Edit);
            #endregion
            #region 集裝箱管理頁面
            options.Conventions.AuthorizePage("/Settings/ContainerSizes/Index", SettingsPermissions.ContainerSizes.Default);
            options.Conventions.AuthorizePage("/Settings/ContainerSizes/CreateModal", SettingsPermissions.ContainerSizes.Create);
            options.Conventions.AuthorizePage("/Settings/ContainerSizes/EditModal", SettingsPermissions.ContainerSizes.Edit);
            #endregion
            #region 空運出口其他費用頁面
            options.Conventions.AuthorizePage("/Settings/AirOtherCharge/Index", SettingsPermissions.AirOtherCharge.Default);
            options.Conventions.AuthorizePage("/Settings/AirOtherCharge/CreateModal", SettingsPermissions.AirOtherCharge.Create);
            options.Conventions.AuthorizePage("/Settings/AirOtherCharge/EditModal", SettingsPermissions.AirOtherCharge.Edit);
            #endregion
			#region 港口管理
            options.Conventions.AuthorizePage("/Settings/PortsManagement/Index", SettingsPermissions.PortsManagement.Default);
            options.Conventions.AuthorizePage("/Settings/PortsManagement/CreateModal", SettingsPermissions.PortsManagement.Create);
            options.Conventions.AuthorizePage("/Settings/PortsManagement/EditModal", SettingsPermissions.PortsManagement.Edit);
            #endregion
            #region 貨幣表
            options.Conventions.AuthorizePage("/Settings/CurrencySetting/Index", SettingsPermissions.CurrencySetting.Default);
            options.Conventions.AuthorizePage("/Settings/CurrencySetting/CreateModal", SettingsPermissions.CurrencySetting.Create);
            options.Conventions.AuthorizePage("/Settings/CurrencySetting/EditModal", SettingsPermissions.CurrencySetting.Edit);
            #endregion
            #region 國家管理
            options.Conventions.AuthorizePage("/Settings/CountryDisplayName/Index", SettingsPermissions.CountryDisplayName.Default);
            #endregion

			#region User Management
            //options.Conventions.AuthorizePage("/Settings/Users/Management/Index", SettingsPermissions.Users.Default);
            #endregion
			#region 顯示設定頁面
            options.Conventions.AuthorizePage("/Settings/DisplaySetting/Index", SettingsPermissions.DisplaySetting.Default);
            options.Conventions.AuthorizePage("/Settings/DisplaySetting/CreateModal", SettingsPermissions.DisplaySetting.Create);
            options.Conventions.AuthorizePage("/Settings/DisplaySetting/EditModal", SettingsPermissions.DisplaySetting.Edit);
            #endregion
            #endregion

            #region 會計設定
            #region 科目管理頁面
            options.Conventions.AuthorizePage("/AccountingSettings/GlCodes/Index", SettingsPermissions.ContainerSizes.Default);
            options.Conventions.AuthorizePage("/AccountingSettings/GlCodes/CreateModal", SettingsPermissions.ContainerSizes.Create);
            options.Conventions.AuthorizePage("/AccountingSettings/GlCodes/EditModal", SettingsPermissions.ContainerSizes.Edit);
            #endregion
            #endregion
            
            #region 會計
            #region 收付款-收款
            options.Conventions.AuthorizePage("/Accounting/Payment/CustomerPayment/Index", AccountingPermissions.CustomerPayment.Default);
            #endregion 
            #region 收付款-收款清單
            options.Conventions.AuthorizePage("/Accounting/Payment/Customer/Index", AccountingPermissions.Customer.Default);
            #endregion
            #endregion

            #region 報表
            options.Conventions.AuthorizePage("/ReportScreen/VolumeProfitReport", ReportsPermissions.VolumeProfitReport.Default);
            #endregion

            #region 運價中心

            #region 運價成本查詢
            options.Conventions.AuthorizePage("/FreightCenter/ContractCostQuery/Index", FreightCenterPermissions.ContractCostQuery.Default);
            #endregion

            #endregion

        });
    }
    private void ConfigureProfileManagementPage()
    {
        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Account/Manage");
        });

        Configure<ProfileManagementPageOptionsCustom>(options =>
        {
            options.Contributors.Add(new AccountProfileManagementPageContributorCustom());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(typeof(ManageModel).FullName,
                    configuration =>
                    {
                        configuration.AddFiles("/client-proxies/account-proxy.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/Password/Default.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Default.js");
                        configuration.AddFiles("/Pages/Account/ProfileManagementGroup/Settings/Default.js");
                        configuration.AddFiles("/Pages/Account/ProfileManagementGroup/ProfileHeader/Default.js");
                    });
        });

    }
    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                BasicThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/css/select.dataTables.css");
                    bundle.AddFiles("/css/buttons.dataTables.css");
                    bundle.AddFiles("/css/jquery.datetimepicker.css");
                }
            );

            options.ScriptBundles.Configure(
                BasicThemeBundles.Scripts.Global,
                bundle => {
                    bundle.AddFiles("/js/scripts.js");
                    bundle.AddFiles("/js/dataTables.select.js");
                    bundle.AddFiles("/js/dataTables.buttons.js");
                    bundle.AddFiles("/js/dataTables.rowsGroup.js");
                    bundle.AddFiles("/js/dataTables.altEditor.free.js");
                    bundle.AddFiles("/js/jquery.datetimepicker.full.js");
                    bundle.AddFiles("/js/jquery.tag.js");
                    bundle.AddFiles("/js/tempus-dominus.js");
                    bundle.AddFiles("/js/jQuery-provider.js");
                    bundle.AddFiles("/js/popper.min.js");
                    bundle.AddFiles("/js/tempus-dominus.js");
                    bundle.AddFiles("/js/customDateFormat.js");
                    bundle.AddFiles("/js/jquery-templates.tmpl.min.js");
                    /*
                    bundle.AddFiles("/newwebsite/js/popper.min.js");
                    bundle.AddFiles("/newwebsite/assets/bootstrap/js/bootstrap.min.js");
                    bundle.AddFiles("/newwebsite/assets/bootstrap-select/js/bootstrap-select.min.js");
                    bundle.AddFiles("/newwebsite/assets/bootstrap-select/js/i18n/defaults-zh_TW.js");
                    bundle.AddFiles("/newwebsite/assets/bootstrap-select/js/i18n/defaults-zh_TW.js");
                    bundle.AddFiles("/newwebsite/js/script.js"); */
                });
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "Freight";
            });

        context.Services.ForwardIdentityAuthenticationForBearer();
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<FreightWebModule>();
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                    options.FileSets.ReplaceEmbeddedByPhysical<FreightDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Dolphin.Freight.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<FreightDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Dolphin.Freight.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<FreightApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Dolphin.Freight.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<FreightApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Dolphin.Freight.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<FreightWebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureLocalizationServices()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
        });
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new FreightMenuContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(FreightApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Freight API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCookiePolicy();

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Freight API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
