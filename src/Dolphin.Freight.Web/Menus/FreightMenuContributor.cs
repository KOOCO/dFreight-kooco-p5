using System.Threading.Tasks;
using Dolphin.Freight.Localization;
using Dolphin.Freight.MultiTenancy;
using Dolphin.Freight.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace Dolphin.Freight.Web.Menus;

public class FreightMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<FreightResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                FreightMenus.Home,
                l["Menu:Home"],
                url: "/Dashboard/Dashboard",
                icon: "fa fa-home",
                order: 0
            )
        );

        #region 海運出口menu權限控管
        var oceanExportsMenu = new ApplicationMenuItem(
            "OceanExports",
            l["Menu:OceanExports"],
            icon: "fa dF icon-ocena-export"
        );

        context.Menu.AddItem(oceanExportsMenu);

        #region 海運出口管理
        if (await context.IsGrantedAsync(OceanExportPermissions.OceanExportMbls.Create)) 
        {
            oceanExportsMenu.AddItem(new ApplicationMenuItem(
                "OceanExports.CreateMbl",
                l["Menu:OceanExports.CreateMbl"],
                url: "/OceanExports/CreateMbl"
            ));
        }
        if (await context.IsGrantedAsync(OceanExportPermissions.OceanExportMbls.Default))
        {

            oceanExportsMenu.AddItem(new ApplicationMenuItem(
                    "OceanExports.IndexList",
                    l["Menu:OceanExports.IndexList"],
                    url: "/OceanExports/Index"
            ));
            oceanExportsMenu.AddItem(new ApplicationMenuItem(
                    "OceanExports.MblList",
                    l["Menu:OceanExports.MblList"],
                    url: "/OceanExports/MblList"
            ));
            oceanExportsMenu.AddItem(new ApplicationMenuItem(
                    "OceanExports.HblList",
                    l["Menu:OceanExports.HblList"],
                    url: "/OceanExports/HblList"
            ));
        }
        if (await context.IsGrantedAsync(OceanExportPermissions.ExportBookings.Create))
        {
            oceanExportsMenu.AddItem(new ApplicationMenuItem(
                    "ExportBookings.Create",
                    l["Menu:ExportBookings.Create"],
                    url: "/OceanExports/ExportBookings/Create"
            ));
        }
        if (await context.IsGrantedAsync(OceanExportPermissions.ExportBookings.Default))
        {
            oceanExportsMenu.AddItem(new ApplicationMenuItem(
                    "ExportBookings.Index",
                    l["Menu:ExportBookings.Index"],
                    url: "/OceanExports/ExportBookings/Index"
            ));
        }
        if (await context.IsGrantedAsync(OceanExportPermissions.VesselSchedules.Create))
        {
            oceanExportsMenu.AddItem(new ApplicationMenuItem(
                "VesselSchedules.Create",
                l["Menu:VesselSchedules.Create"],
                url: "/OceanExports/VesselSchedules/Create"
            ));
        }
        if (await context.IsGrantedAsync(OceanExportPermissions.VesselSchedules.Default))
        {
            oceanExportsMenu.AddItem(new ApplicationMenuItem(
                    "VesselSchedules.Index",
                    l["Menu:VesselSchedules.Index"],
                    url: "/OceanExports/VesselSchedules/Index"
            ));
        }

        #endregion
        #endregion        

        #region 海運進口menu權限控管
        var oceanImportsMenu = new ApplicationMenuItem(
            "OceanImports",
            l["Menu:OceanImports"],
            icon: "fa dF icon-ocena-import"
        );

        context.Menu.AddItem(oceanImportsMenu);


        #region 海運出口管理
        if (await context.IsGrantedAsync(OceanImportPermissions.OceanImportMbls.Create))
        {
            oceanImportsMenu.AddItem(new ApplicationMenuItem(
                "OceanImports.CreateMbl",
                l["Menu:OceanImports.CreateMbl"],
                url: "/OceanImports/CreateMbl"
            ));
        }
        if (await context.IsGrantedAsync(OceanImportPermissions.OceanImportMbls.Default))
        {
            oceanImportsMenu.AddItem(new ApplicationMenuItem(
                    "OceanImports.IndexList",
                    l["Menu:OceanImports.IndexList"],
                    url: "/OceanImports/Index"
            ));
            oceanImportsMenu.AddItem(new ApplicationMenuItem(
                    "OceanImports.MblList",
                    l["Menu:OceanImports.MblList"],
                    url: "/OceanImports/MblList"
            ));
            oceanImportsMenu.AddItem(new ApplicationMenuItem(
                    "OceanImports.HblList",
                    l["Menu:OceanImports.HblList"],
                    url: "/OceanImports/HblList"
            ));
        }

        #endregion
        #endregion

        #region 設定menu權限控管
        var settingsMenu = new ApplicationMenuItem(
            "Settings",
            l["Menu:Settings"],
            icon: "fa-solid fa-gear"
        );

        context.Menu.AddItem(settingsMenu);

        #region IT號碼管理
        if (await context.IsGrantedAsync(SettingsPermissions.ItNoRanges.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.ItNoRanges",
                    l["Menu:ItNoRanges"],
                    url: "/Settings/ItNoRanges"
            ));
        }
        #endregion

        #region AWB號碼管理
        if (await context.IsGrantedAsync(SettingsPermissions.AwbNoRanges.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.AwbNoRanges",
                    l["Menu:AwbNoRanges"],
                    url: "/Settings/AwbNoRanges"
            ));
        }
        #endregion

        #region 包裝管理
        if (await context.IsGrantedAsync(SettingsPermissions.PackageUnits.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.PackageUnits",
                    l["Menu:PackageUnits"],
                    url: "/Settings/PackageUnits"
            ));
        }
        #endregion
        #region 集裝箱管理
        if (await context.IsGrantedAsync(SettingsPermissions.ContainerSizes.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.ContainerSizes",
                    l["Menu:ContainerSizes"],
                    url: "/Settings/ContainerSizes"
            ));
        }
        #endregion

        #region 空運出口其他費用
        if (await context.IsGrantedAsync(SettingsPermissions.AirOtherCharge.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.AirOtherCharge",
                    l["Menu:AirOtherCharge"],
                    url: "/Settings/AirOtherCharge"
            ));
        }
        #endregion

		#region 港口管理
        if (await context.IsGrantedAsync(SettingsPermissions.PortsManagement.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.PortsManagement",
                    l["Menu:PortsManagement"],
                    url: "/Settings/PortsManagement"
            ));
        }
        #endregion
        #region 貨幣表
        if (await context.IsGrantedAsync(SettingsPermissions.CurrencySetting.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.CurrencySetting",
                    l["Menu:CurrencySetting"],
                    url: "/Settings/CurrencySetting"
            ));
        }
        #endregion
        #region 國家管理
        if (await context.IsGrantedAsync(SettingsPermissions.CountryDisplayName.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.CountryDisplayName",
                    l["Menu:CountryDisplayName"],
                    url: "/Settings/CountryDisplayName"
            ));
        }

        if (await context.IsGrantedAsync(SettingsPermissions.AirOtherCharge.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.Users",
                    l["Menu:UserManagement"],
                    url: "/Settings/Users/Management"
            ));
        }
        #endregion

		#region 顯示設定
        if (await context.IsGrantedAsync(SettingsPermissions.DisplaySetting.Default))
        {
            settingsMenu.AddItem(new ApplicationMenuItem(
                    "Settings.DisplaySetting",
                    l["Menu:DisplaySetting"],
                    url: "/Settings/DisplaySetting"
            ));
        }
        #endregion

        #endregion

        #region 會計設定menu權限控管
        var accountingSettingMenu = new ApplicationMenuItem(
            "AccountingSettings",
            l["Menu:AccountingSettings"],
            icon: "fa-solid fa-calculator"
        );

        context.Menu.AddItem(accountingSettingMenu);

        #region 科目代碼管理
        if (await context.IsGrantedAsync(SettingsPermissions.ItNoRanges.Default))
        {
            accountingSettingMenu.AddItem(new ApplicationMenuItem(
                    "AccountingSettings.GlCodes",
                    l["Menu:GlCodes"],
                    url: "/AccountingSettings/GlCodes"
            ));
        }
        #endregion

        #region 計費代碼管理
        if (await context.IsGrantedAsync(SettingsPermissions.AwbNoRanges.Default))
        {
            accountingSettingMenu.AddItem(new ApplicationMenuItem(
                    "AccountingSetting.BillingCodes",
                    l["Menu:BillingCodes"],
                    url: "/AccountingSettings/BillingCodes"
            ));
        }

        #endregion

        //context.Menu.AddItem(accountingSettingMenu);

        #endregion

        #region Trade Partner Menu
        if (await context.IsGrantedAsync(SettingsPermissions.AwbNoRanges.Default))
        {
            var tradePartnerMenu = new ApplicationMenuItem(
           name: FreightMenus.TradePartnerManagement.GroupName,
           displayName: l["Menu:TradePartner"], // 貿易夥伴
           icon: "fa-solid fa-handshake"
       );
            tradePartnerMenu.
                  AddItem(new ApplicationMenuItem(
                    name: FreightMenus.TradePartnerManagement.Addition,
                    displayName: l["Menu:TradePartner.Add"], // 新增貿易夥伴
                    url: "/Sales/TradePartner/TradePartnerInfo")///Sales/TradePartner/TradePartnerInfo
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.TradePartnerManagement.CreditEntry,
                    displayName: l["Menu:TradePartner.CreditEntry"],  // 貿易夥伴信用額度
                    url: "/Sales/TradePartner/Credit")
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.TradePartnerManagement.List,
                    displayName: l["Menu:TradePartner.List"],  // 貿易夥伴清單
                    url: "/Sales/TradePartner/List")
            );
            context.Menu.AddItem(tradePartnerMenu);
        }

        #endregion

        #region AirExport Menu
        if (await context.IsGrantedAsync(SettingsPermissions.AwbNoRanges.Default))
        {
            var airExportMenu = new ApplicationMenuItem(
                name: FreightMenus.AirExportManagement.GroupName,
                displayName: l["Menu:AirExport"], // 空運出口
                icon: "fa dF icon-air-export");
            airExportMenu.
                  AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirExportManagement.Shipment,
                    displayName: l["Menu:AirExport.CreateMawb"], // New Shipment
                    url: "/AirExports/CreateMawb")
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirExportManagement.ShipmentList,
                    displayName: l["Menu:AirExport.List"],  // My Shipment List
                     url: "/AirExports/Index")
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirExportManagement.MawbList,
                    displayName: l["Menu:AirExport.MawbList"],  // Mawb List
                    url: "/AirExports/MawbList")
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirExportManagement.HawbList,
                    displayName: l["Menu:AirExport.HawbList"],  // Hawb List
                    url: "/AirExports/HawbList")
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirExportManagement.Booking,
                    displayName: l["Menu:AirExport.Booking"],  // Hawb List
                    url: "/AirExports/Bookings/Create")
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirExportManagement.BookingList,
                    displayName: l["Menu:AirExport.BookingList"],  // Hawb List
                    url: "/AirExports/Bookings/Index")
            //).AddItem(new ApplicationMenuItem(
            //    name: FreightMenus.AirExportManagement.Booking,
            //    displayName: l["Menu:AirExport.Booking"],  // Create Booking
            //    url: "/AirExports/CreateBooking")
            //).AddItem(new ApplicationMenuItem(
            //    name: FreightMenus.AirExportManagement.BookingList,
            //    displayName: l["Menu:AirExport.BookingList"],  // Booking List
            //    url: "/AirExports/BookingList")

            );
            context.Menu.AddItem(airExportMenu);
        }

        #endregion

        #region AirImport Menu
        if (await context.IsGrantedAsync(SettingsPermissions.AwbNoRanges.Default))
        {
            var airImportMenu = new ApplicationMenuItem(
                name: FreightMenus.AirImportManagement.GroupName,
                displayName: l["Menu:AirImport"], // 空運進口
                icon: "fa dF icon-air-import");
            airImportMenu.
                  AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirImportManagement.Shipment,
                    displayName: l["Menu:AirImport.CreateMawb"], // New Shipment
                    url: "/AirImports/CreateMawb")
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirImportManagement.ShipmentList,
                    displayName: l["Menu:AirImport.List"],  // My Shipment List
                    url: "/AirImports/Index")
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirImportManagement.MawbList,
                    displayName: l["Menu:AirImport.MawbList"],  // Mawb List
                    url: "/AirImports/MawbList")
                ).AddItem(new ApplicationMenuItem(
                    name: FreightMenus.AirImportManagement.HawbList,
                    displayName: l["Menu:AirImport.HawbList"],  // Hawb List
                    url: "/AirImports/HawbList")
            );
            context.Menu.AddItem(airImportMenu);
        }

        #endregion

        #region Reports Menu
        if (await context.IsGrantedAsync(ReportsPermissions.VolumeProfitReport.Default))
        {
            var reportMenu = new ApplicationMenuItem(
                name: FreightMenus.ReportManagement.GroupName,
                displayName: l["Menu:ReportManagement"], 
                icon: "fa-solid fa-chart-pie");
            reportMenu.
                  AddItem(new ApplicationMenuItem(
                    name: FreightMenus.ReportManagement.VolumeProfitReport,
                    displayName: l["Menu:Reports.VolumeProfitReport"], 
                    url: "/ReportScreen/VolumeProfitReport")
                
            );
            context.Menu.AddItem(reportMenu);
        }

        #endregion

        #region 運價中心
        if (await context.IsGrantedAsync(FreightCenterPermissions.ContractCostQuery.Default))
        {

            var menuFreightCenter = new ApplicationMenuItem(
                "FrtCenter",
                l["Menu:FreightCenter"],
                icon: "fa fa-truck"
            );
            context.Menu.AddItem(menuFreightCenter);

            #region 運價成本查詢
            // if (await context.IsGrantedAsync(FreightCenterPermissions.ShippingCostManagement.Default))
            menuFreightCenter.AddItem(
                   new ApplicationMenuItem(
                       "FreightCenter.ContractCostQuery",
                       " " + l["Menu:ContractCostQuery"],
                       url: "/FreightCenter/ContractCostQuery",
                       icon: "fa fa-search"
                   )
               );
            #endregion

            #region 運價成本列表
            menuFreightCenter.AddItem(
                   new ApplicationMenuItem(
                       "FreightCenter.ShippingCostList",
                       " " + l["Menu:ShippingCostList"],
                       url: "/FreightCenter/ShippingCostList",
                       icon: "fa fa-box"
                   )
               );
            #endregion

            #region 運價成本管理
            // menuFreightCenter.AddItem(
            //        new ApplicationMenuItem(
            //            "FreightCenter.ShippingCostManagement",
            //            " " + l["Menu:ShippingCostManagement"],
            //            url: "/FreightCenter/ShippingCostManagement",
            //            icon: "fa fa-box"
            //        )
            //    );
            #endregion
        }
        #endregion

        #region 設置會計menu權限控管
        var accountingMenu = new ApplicationMenuItem(
            "Accounting",
            l["Menu:Accounting"],
            icon: "fa fa-calculator"
        );

        context.Menu.AddItem(accountingMenu);

        var invoiceMenu = new ApplicationMenuItem(
            "Invoice",
            l["Menu:Invoice"],
            icon: "fas fa-file-invoice"
        );
        accountingMenu.AddItem(invoiceMenu);
        #region 會計-收入成本清單
        if (await context.IsGrantedAsync(AccountingPermissions.Invoice.Default))
        {
            invoiceMenu.AddItem(new ApplicationMenuItem(
                    "Accounting.Invoice",
                    l["Menu:Invoice.List"],
                    url: "/Accounting/Invoices/List"
            ));
        }
        #endregion
        #region 會計-G&A收入清單
        if (await context.IsGrantedAsync(AccountingPermissions.Invoice.GAList))
        {
            invoiceMenu.AddItem(new ApplicationMenuItem(
                    "Accounting.Invoice.GAList",
                    l["Menu:Invoice.GAList"],
                    url: "/Accounting/Invoices/GAList"
            ));
        }
        #endregion
        #region 會計-新增收入成本
        if (await context.IsGrantedAsync(AccountingPermissions.Invoice.GARCreate))
        {
            invoiceMenu.AddItem(new ApplicationMenuItem(
                    "Accounting.Invoice.GARCreate",
                    l["Menu:Invoice.GARCreate"],
                    url: "/Accounting/Invoices/GACreate?InvoiceType=3"
            ));
        }
        #endregion
        #region 會計-新增G&A收入支出清單
        if (await context.IsGrantedAsync(AccountingPermissions.Invoice.GAPCreate))
        {
            invoiceMenu.AddItem(new ApplicationMenuItem(
                    "Accounting.GAPCreate",
                    l["Menu:Invoice.GAPCreate"],
                    url: "/Accounting/Invoices/GACreate?InvoiceType=4"
            ));
        }
        #endregion

        var paymentMenu = new ApplicationMenuItem(
            "Payment",
            l["Menu:Payment"],
            icon: "fa fa-bank"
        );

        accountingMenu.AddItem(paymentMenu);

        #region 會計-收付款-收款
        if (await context.IsGrantedAsync(AccountingPermissions.CustomerPayment.Default))
        {
            paymentMenu.AddItem(new ApplicationMenuItem(
                    "Accounting.CustomerPayment",
                    l["Menu:CustomerPayment"],
                    url: "/Accounting/Payment/CustomerPayment"
            ));
        }
        #endregion

        #region 會計-收付款-收款清單
        if (await context.IsGrantedAsync(AccountingPermissions.CustomerPayment.Default))
        {
            paymentMenu.AddItem(new ApplicationMenuItem(
                    "Accounting.Customer",
                    l["Menu:Customer"],
                    url: "/Accounting/Payment/Customer"
            ));
        }
        #endregion

        #region 會計-收付款-付款
        if (await context.IsGrantedAsync(AccountingPermissions.Payment.Default))
        {
            paymentMenu.AddItem(new ApplicationMenuItem(
                    "Accounting.Payment",
                    l["Menu:MakePayment"],
                    url: "/Accounting/Payment/Payment"
            ));
        }
        #endregion

        #region 會計-收付款-付款清單
        if (await context.IsGrantedAsync(AccountingPermissions.PaymentMadeList.Default))
        {
            paymentMenu.AddItem(new ApplicationMenuItem(
                    "Accounting.PaymentMadeList",
                    l["Menu:PaymentMadeList"],
                    url: "/Accounting/Payment/PaymentMadeList"
            ));
        }
        #endregion

        #endregion

        /*
        #region 匯率表管理
        if (await context.IsGrantedAsync(SettingsPermissions.PackageUnits.Default))
        {
            accountingSettingMenu.AddItem(new ApplicationMenuItem(
                    "AccountingSetting.CurrencyTables",
                    l["Menu:CurrencyTables"],
                    url: "/AccountingSettings/CurrencyTables"
            ));
        }
        #endregion
        */

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
        administration.Icon = "fa-solid fa-user-gear";
    }
}
