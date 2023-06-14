using Dolphin.Freight.Accounting.Inv;
using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Accounting.Payment;
using Dolphin.Freight.AccountingSettings.BillingCodes;
using Dolphin.Freight.AccountingSettings.CurrencyTables;
using Dolphin.Freight.AccountingSettings.GlCodes;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.ImportExport.Common;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settings.DisplaySetting;
using Dolphin.Freight.Settings.AirOtherCharge;
using Dolphin.Freight.Settings.AwbNoRanges;
using Dolphin.Freight.Settings.ContainerSizes;
using Dolphin.Freight.Settings.Offices;
using Dolphin.Freight.Settings.Countries;
using Dolphin.Freight.Settings.Currencies;
using Dolphin.Freight.Settings.ItNoRanges;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.TradePartners.Credits;
using Dolphin.Freight.TradePartners.TradeParties;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.CurrencySetting;
using Dolphin.Freight.Common.Memos;
using Dolphin.Freight.TradePartners.DefaultFreight;
using Dolphin.Freight.ImportExport.Configuration;


namespace Dolphin.Freight.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class FreightDbContext :
    AbpDbContext<FreightDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<CreditLimitGroup> CreditLimitGroups { get; set; }
    public DbSet<Dolphin.Freight.TradePartners.TradePartner> TradePartners { get; set; }
    public DbSet<AccountGroup> AccountGroups { get; set; }
    public DbSet<ContactPerson> ContactPersons { get; set; }
    public DbSet<ContactPersonChild> ContactPersonChild { get; set; }
    public DbSet<TradeParty> TradeParties { get; set; }
    public DbSet<TradePartnerMemo> TradePartnerMemo { get; set; }
    public DbSet<DefaultFreightAP> DefaultFreightAP { get; set; }
    public DbSet<DefaultFreightAR> DefaultFreightAR { get; set; }
    public DbSet<DefaultFreightDC> DefaultFreightDC { get; set; }
    public DbSet<Airport> Airports { get; set; }
    public DbSet<AirExportMawb> AirExportMawbs { get; set; }
    public DbSet<AirImportMawb> AirImportMawbs { get; set; }
    public DbSet<AirImportHawb> AirImportHawbs { get; set; }
    public DbSet<AirExportHawb> AirExportHawbs { get; set; }
    public DbSet<GridPreference> GridPreference { get; set; }


    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    public DbSet<ItNoRange> ItNoRanges { get; set; }
    public DbSet<AirOtherCharge> AirOtherCharges { get; set; }
	public DbSet<PortsManagement> PortsManagements { get; set; }
    public DbSet<CurrencySetting> CurrencySetting { get; set; }
    public DbSet<DisplaySetting> DisplaySettings { get; set; }
    public DbSet<Currency> Currencies { get; set; }

    public DbSet<Office> Offices { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<CountryDisplayName> CountryDisplayName { get; set; }
    public DbSet<SysCode> SysCodes { get; set; }
    public DbSet<PackageUnit> PackageUnits { get; set; }
    public DbSet<ContainerSize> ContainerSizes { get; set; }

    public DbSet<AwbNoRange> AwbNoRanges { get; set; }

    public DbSet<OceanExportMbl> OceanExportMbls { get; set; }
    public DbSet<OceanExportHbl> OceanExportHbls { get; set; }
    public DbSet<Memo> Memos { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Container> Containers { get; set; }
    public DbSet<ContainerDimension> ContainerDimensions { get; set; }
    public DbSet<CargoManifest> CargoManifests { get; set; }
    public DbSet<WarehouseReceipt> WarehouseReceipts { get; set; }
    public DbSet<GlCode> GlCodes { get; set; }
    public DbSet<BillingCode> BillingCodes { get; set; }
    public DbSet<CurrencyTable> CurrencyTables { get; set; }
    public DbSet<Substation> Substations { get; set; }
    public DbSet<Port> Ports { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceBill> InvoiceBills { get; set; }
	public DbSet<CustomerPayment> CustomerPayment { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<VesselSchedule> VesselSchedules { get; set; }
    public DbSet<ExportBooking> ExportBookings { get; set; }
    // Tenant Management 
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    public DbSet<OceanImportMbl> OceanImportMbls { get; set; }
    public DbSet<OceanImportHbl> OceanImportHbls { get; set; }
	public DbSet<Inv> Inv { get; set; }
    public DbSet<Dolphin.Freight.ReportLog.ReportLog> ReportLog { get; set; }
    #endregion

    public FreightDbContext(DbContextOptions<FreightDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureIdentityServer();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */
        builder.Entity<TradeParty>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradeParties", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.TradePartyDescription).HasMaxLength(128);
        });
        builder.Entity<ContactPerson>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "ContactPersons", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ContactName)
            .IsRequired()
            .HasMaxLength(128);
            b.Property(x => x.ContactTitle).HasMaxLength(128);
            b.Property(x => x.ContactDivision).HasMaxLength(128);
            b.Property(x => x.ContactCellPhone).HasMaxLength(26);
            b.Property(x => x.ContactPhone).HasMaxLength(26);
            b.Property(x => x.ContactFax).HasMaxLength(26);
            b.Property(x => x.ContactEmailAddress).HasMaxLength(512);
            b.Property(x => x.ContactRemark).HasMaxLength(2048);
        }
        );

        builder.Entity<ContactPersonChild>(b => {
            b.ToTable(FreightConsts.DbTablePrefix + "ContactPersonChilds", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Remark).HasMaxLength(2048);
        });

        builder.Entity<AccountGroup>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "AccountGroups", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.AccountGroupName)
                .IsRequired()
                .HasMaxLength(384);
        });

        builder.Entity<Dolphin.Freight.TradePartners.TradePartner>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradePartners", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.TPCode)
               .IsRequired()
               .HasMaxLength(TradePartnerConsts.MaxTPCodeLength);
            b.Property(x => x.TPName)
                .IsRequired()
                .HasMaxLength(TradePartnerConsts.MaxTPNameLength);
            b.Property(x => x.TPNamePrint)
                .IsRequired()
                .HasMaxLength(384);
            b.Property(x => x.TPNameLocal).HasMaxLength(384);
            b.Property(x => x.PostCode).HasMaxLength(11);
            b.Property(x => x.TPPrintAddress).HasMaxLength(512);
            b.Property(x => x.TPLocalAddress).HasMaxLength(512);
            b.Property(x => x.CityCode).HasMaxLength(128);
            b.Property(x => x.StateCode).HasMaxLength(128);
            b.Property(x => x.CountryCode).HasMaxLength(64);
            b.Property(x => x.Telephone).HasMaxLength(26);
            b.Property(x => x.Fax).HasMaxLength(26);
            b.Property(x => x.Website).HasMaxLength(2048);
            b.Property(x => x.TPAliasName).HasMaxLength(384);
            b.Property(x => x.Ceo).HasMaxLength(384);
            b.Property(x => x.CorpNo).HasMaxLength(64);
            b.Property(x => x.AccountNo).HasMaxLength(64);
            b.Property(x => x.FirmsCode).HasMaxLength(32);
            b.Property(x => x.ScacCode).HasMaxLength(16);
            b.Property(x => x.CbsaCode).HasMaxLength(16);
            b.Property(x => x.IataCode).HasMaxLength(64);
            b.Property(x => x.IataPrefix).HasMaxLength(5);
            b.Property(x => x.DutyPaymentType).HasMaxLength(128);
            b.Property(x => x.OpenHours).HasMaxLength(128);
            b.Property(x => x.SalesOfficeCode).HasMaxLength(128);
            b.Property(x => x.SalesCode).HasMaxLength(128);
            b.Property(x => x.SalesCodeOe).HasMaxLength(128);
            b.Property(x => x.SalesCodeOi).HasMaxLength(128);
            b.Property(x => x.SalesCodeAe).HasMaxLength(128);
            b.Property(x => x.SalesCodeAi).HasMaxLength(128);
            b.Property(x => x.SalesCodeCuOe).HasMaxLength(128);
            b.Property(x => x.SalesCodeCuAe).HasMaxLength(128);
            b.Property(x => x.SalesCodeCuOi).HasMaxLength(128);
            b.Property(x => x.CsCode).HasMaxLength(128);
            b.Property(x => x.AccountAddress).HasMaxLength(512);
            b.Property(x => x.TaxId).HasMaxLength(72);
            b.Property(x => x.AccountName).HasMaxLength(384);
            b.Property(x => x.BankName).HasMaxLength(384);
            b.Property(x => x.AccountCurrencyCode).HasMaxLength(128);
            b.Property(x => x.BankName2).HasMaxLength(384);
            b.Property(x => x.AccountNo2).HasMaxLength(64);
            b.Property(x => x.AccountCurrencyCode2).HasMaxLength(128);
            b.Property(x => x.BankName3).HasMaxLength(384);
            b.Property(x => x.AccountNo3).HasMaxLength(64);
            b.Property(x => x.AccountCurrencyCode3).HasMaxLength(128);
            b.Property(x => x.IsfSubmissionName).HasMaxLength(384);
            b.Property(x => x.ImporterNo).HasMaxLength(256);
        });

        builder.Entity<TradePartnerMemo>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradePartnerMemo", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(b => b.Title).IsRequired().HasMaxLength(100);
            b.Property(b => b.Memo).IsRequired().HasMaxLength(2048);
        }); 
        builder.Entity<DefaultFreightAP>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradePartnerDefaultFreightAP", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(b => b.Category).IsRequired();
            b.Property(b => b.PCType).IsRequired();
            b.Property(b => b.Type).IsRequired();
            b.Property(b => b.Unit).IsRequired();
            b.Property(b => b.Rate).IsRequired();
            b.Property(b => b.AgentAmount).IsRequired();
        });

        builder.Entity<DefaultFreightAR>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradePartnerDefaultFreightAR", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(b => b.Category).IsRequired();
            b.Property(b => b.PCType).IsRequired();
            b.Property(b => b.Type).IsRequired();
            b.Property(b => b.Unit).IsRequired();
            b.Property(b => b.Rate).IsRequired();
            b.Property(b => b.AgentAmount).IsRequired();
        });

        builder.Entity<DefaultFreightDC>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradePartnerDefaultFreightDC", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(b => b.Category).IsRequired();
            b.Property(b => b.PCType).IsRequired();
            b.Property(b => b.Type).IsRequired();
            b.Property(b => b.Unit).IsRequired();
            b.Property(b => b.Rate).IsRequired();
            b.Property(b => b.AgentAmount).IsRequired();
        });

        builder.Entity<DefaultFreightAP>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradePartnerDefaultFreightAP", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(b => b.Category).IsRequired();
            b.Property(b => b.PCType).IsRequired();
            b.Property(b => b.Type).IsRequired();
            b.Property(b => b.Unit).IsRequired();
            b.Property(b => b.Rate).IsRequired();
            b.Property(b => b.AgentAmount).IsRequired();
        });

        builder.Entity<CreditLimitGroup>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "CreditLimitGroups", FreightConsts.DbSchema);
            // configure the base properties
            b.ConfigureByConvention();
            b.Property(x => x.CreditLimitGroupName)
                .IsRequired()
                .HasMaxLength(CreditLimitGroupConsts.MaxCreditLimitGroupNameLength);
        });

        builder.Entity<DefaultFreightAR>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradePartnerDefaultFreightAR", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(b => b.Category).IsRequired();
            b.Property(b => b.PCType).IsRequired();
            b.Property(b => b.Type).IsRequired();
            b.Property(b => b.Unit).IsRequired();
            b.Property(b => b.Rate).IsRequired();
            b.Property(b => b.AgentAmount).IsRequired();
        });

        builder.Entity<DefaultFreightDC>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradePartnerDefaultFreightDC", FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(b => b.Category).IsRequired();
            b.Property(b => b.PCType).IsRequired();
            b.Property(b => b.Type).IsRequired();
            b.Property(b => b.Unit).IsRequired();
            b.Property(b => b.Rate).IsRequired();
            b.Property(b => b.AgentAmount).IsRequired();
        });

        //TODO: Add TradePartner & CreditLimitGroup Relation (1 TP Has 1 CLG, 1 CLG Has Many TP)
        builder.Entity<ItNoRange>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "ItNoRanges",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.StartNo).IsRequired().HasMaxLength(8);
            b.Property(x => x.EndNo).IsRequired().HasMaxLength(8);
            b.Property(x => x.CurrentNo).HasMaxLength(8);
        });

        builder.Entity<AirOtherCharge>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "AirOtherCharges",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.companyType).IsRequired();
            b.Property(x => x.paymentType).IsRequired();
            b.Property(x => x.showOnMAWB);
            b.Property(x => x.showOnHAWB);
            b.Property(x => x.chargeItem).IsRequired();
            b.Property(x => x.chargeItemSubitem).IsRequired();
            b.Property(x => x.chargeRate).IsRequired();
            b.Property(x => x.chargeRateUnit).IsRequired();
            b.Property(x => x.IsDeleted);
        });

		builder.Entity<PortsManagement>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "PortsManagements",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.IsActive);
            b.Property(x => x.CountryId);
            b.Property(x => x.Country);
            b.Property(x => x.PortName);
            b.Property(x => x.Locode);
            b.Property(x => x.SubDiv);
            b.Property(x => x.IsPort);
            b.Property(x => x.IsDeleted);
        });

        builder.Entity<CurrencySetting>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "CurrencySetting",
                               FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.CurrencyDepartment).IsRequired().HasMaxLength(5);
            b.Property(x => x.CustomerShortCode);
            b.Property(x => x.StartingCurrency).IsRequired().HasMaxLength(5);
            b.Property(x => x.EndCurrency).IsRequired();
            b.Property(x => x.ExChangeRate).IsRequired();
            b.Property(x => x.EffectDate).IsRequired();
            b.Property(x => x.IsDeleted);
        });

        builder.Entity<DisplaySetting>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "DisplaySettings",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.CompleteDay).IsRequired();
        });

        builder.Entity<Currency>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Currencies",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.CurrencyName).IsRequired().HasMaxLength(50);
            b.Property(x => x.AlphabetCode).HasMaxLength(5);

        });
        builder.Entity<SysCode>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "SysCodes",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.CodeValue).IsRequired().HasMaxLength(50);
            b.Property(x => x.CodeType).IsRequired().HasMaxLength(50);
            b.Property(x => x.ShowName).IsRequired().HasMaxLength(50);

        });
        builder.Entity<Office>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Offices",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OfficeName).IsRequired().HasMaxLength(50);
            b.Property(x => x.OfficeCode).IsRequired().HasMaxLength(10);

        });
        builder.Entity<Country>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Countries",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.CountryName).IsRequired().HasMaxLength(50);
            b.Property(x => x.CountryCode).IsRequired().HasMaxLength(10);

        });
        builder.Entity<CountryDisplayName>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "CountryDisplayName",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.DisplayName).IsRequired().HasMaxLength(50);
        });
        builder.Entity<AwbNoRange>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "AwbNoRanges",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.StartNo).IsRequired().HasMaxLength(8);
            b.Property(x => x.EndNo).IsRequired().HasMaxLength(8);
            b.Property(x => x.CurrentNo).HasMaxLength(8);
        });
        builder.Entity<PackageUnit>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "PackageUnits",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.PackageCode).IsRequired().HasMaxLength(3);
            b.Property(x => x.PackageName).HasMaxLength(50);
            b.Property(x => x.Description).HasMaxLength(128);
            b.HasIndex(x=>x.PackageCode).IsUnique();
        });
        builder.Entity<ContainerSize>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "ContainerSizes",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ContainerCode).IsRequired().HasMaxLength(5);
            b.Property(x => x.SizeDescription).HasMaxLength(128);
        });
        builder.Entity<OceanExportMbl>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "OceanExportMbls",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OfficeId).IsRequired();
            b.Property(x => x.ShipmentNo).HasMaxLength(32);
            b.Property(x => x.MblNo).HasMaxLength(32);
            b.Property(x => x.SoNo).HasMaxLength(32);
            b.Property(x => x.ItnNo).HasMaxLength(64);
            b.Property(x => x.AmsNo).HasMaxLength(32);
            b.Property(x => x.CarrierContractNo).HasMaxLength(48);
            b.Property(x => x.CustomerRefNo).HasMaxLength(128);
            b.Property(x => x.Voyage).HasMaxLength(32);
            b.Property(x => x.PrepreCarriageVoyage).HasMaxLength(32);
            b.Property(x => x.ServiceContactNo).HasMaxLength(24);
            b.Property(x => x.ForwardRefNo).HasMaxLength(32);
            b.Property(x => x.SubBlNo).HasMaxLength(32);
            b.Property(x => x.EctNo).HasMaxLength(32);
            b.Property(x => x.PrnNo).HasMaxLength(32);

        });
        builder.Entity<OceanExportHbl>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "OceanExportHbls",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.MblId).IsRequired();
            b.Property(x => x.HblNo).IsRequired().HasMaxLength(32);
            b.Property(x => x.CustomerRefNo).HasMaxLength(128);
            b.Property(x => x.DocNo).HasMaxLength(128);
            b.Property(x => x.B2bNo).HasMaxLength(32);
            b.Property(x => x.SoNo).HasMaxLength(32);
            b.Property(x => x.LcNo).HasMaxLength(32);
            b.Property(x => x.LcIssueBank).HasMaxLength(64);
            b.Property(x => x.HoldReason).HasMaxLength(32);
            b.Property(x => x.WoNo).HasMaxLength(32);
            b.Property(x => x.NraNo).HasMaxLength(32);

        });
        builder.Entity<Memo>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Memos",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Subject).IsRequired().HasMaxLength(300);
            b.Property(x => x.Content).HasMaxLength(3000);


        });
        builder.Entity<Attachment>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Attachments",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FileName).IsRequired().HasMaxLength(128);
            b.Property(x => x.ShowName).IsRequired().HasMaxLength(128);


        });
        builder.Entity<Container>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Containers",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ContainerNo).HasMaxLength(16);
            b.Property(x => x.SealNo).HasMaxLength(20);
            b.Property(x => x.SealNo2).HasMaxLength(20);
            b.Property(x => x.PicupNo).HasMaxLength(50);
            b.Property(x => x.TemperatureUnit).HasMaxLength(1);
            b.Property(x => x.Remark).HasMaxLength(512);
            b.Property(x => x.YardLocation).HasMaxLength(18);
        });
        builder.Entity<ContainerDimension>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "ContainerDimensions",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
        });
        builder.Entity<CargoManifest>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "CargoManifests",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Description).IsRequired().HasMaxLength(128);
            b.Property(x => x.Detail).HasMaxLength(128);
        });
        builder.Entity<WarehouseReceipt>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "WarehouseReceipts",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ReceiptNo).IsRequired().HasMaxLength(32);
            b.Property(x => x.LoadPlanRemarks).HasMaxLength(64);
        });
        builder.Entity<GlCode>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "GlCodes",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(10);
        });
        builder.Entity<BillingCode>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "BillingCodes",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(16);
        });
        builder.Entity<CurrencyTable>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "CurrencyTables",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.StartDate).IsRequired();
            b.Property(x => x.RateInternal).IsRequired();
            b.Property(x => x.RateInterna2).IsRequired();
            b.Property(x => x.Remark).HasMaxLength(64);
        });
        builder.Entity<Port>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Ports",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();

        });

        builder.Entity<Substation>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Substations",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();

        });
        builder.Entity<Invoice>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Invoices",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.InvoiceNo).IsRequired().HasMaxLength(50);
            b.Property(x => x.AttentionTo).HasMaxLength(128);
            b.Property(x => x.LastNo).HasMaxLength(128);
            b.Property(x => x.SystemNo).HasMaxLength(50);
            b.Property(x => x.InvoiceAmount).HasPrecision(20, 10);
            b.Property(x => x.BalanceAmount).HasPrecision(20, 10);
            b.Property(x => x.PaymentAmount).HasPrecision(20, 10);
            b.Property(x => x.PaymentAmountTwd).HasPrecision(20, 10);


        });
		builder.Entity<InvoiceBill>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "InvoiceBills",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();

        });

		builder.Entity<CustomerPayment>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "CustomerPayment",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();

        });

        builder.Entity<Payment>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Payment",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();

        });

        builder.Entity<Inv>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Inv",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.PaymentId).IsRequired();
            b.Property(x => x.InvoiceNo).HasMaxLength(50);
            b.Property(x => x.InvoiceDescription).HasMaxLength(50);
            b.Property(x => x.InvoiceAmount).HasPrecision(20, 10);
            b.Property(x => x.BalanceAmount).HasPrecision(20, 10);
            b.Property(x => x.PaymentAmount).HasPrecision(20, 10);
            b.Property(x => x.PaymentAmountTwd).HasPrecision(20, 10);
        });

        builder.Entity<VesselSchedule>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "VesselSchedules",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ReferenceNo).IsRequired().HasMaxLength(32);
            b.Property(x => x.SoNo).HasMaxLength(32);
            b.Property(x => x.ItnNo).HasMaxLength(64);
            b.Property(x => x.Voyage).HasMaxLength(32);
        });
        builder.Entity<ExportBooking>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "ExportBookings",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.SoNo).IsRequired().HasMaxLength(32);
            b.Property(x => x.HblNo).HasMaxLength(32);
            b.Property(x => x.ItnNo).HasMaxLength(64);
            b.Property(x => x.CustomerRefNo).HasMaxLength(128);
            b.Property(x => x.DocNo).HasMaxLength(32);
            b.Property(x => x.Voyage).HasMaxLength(32);
        });

        builder.Entity<OceanImportMbl>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "OceanImportMbls",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OfficeId).IsRequired();
            b.Property(x => x.ShipmentNo).HasMaxLength(32);
            b.Property(x => x.MblNo).HasMaxLength(32);
            b.Property(x => x.SoNo).HasMaxLength(32);
            b.Property(x => x.ItnNo).HasMaxLength(64);
            b.Property(x => x.AmsNo).HasMaxLength(32);
            b.Property(x => x.CarrierContractNo).HasMaxLength(48);
            b.Property(x => x.CustomerRefNo).HasMaxLength(128);
            b.Property(x => x.Voyage).HasMaxLength(32);
            b.Property(x => x.PrepreCarriageVoyage).HasMaxLength(32);
            b.Property(x => x.ServiceContactNo).HasMaxLength(24);
            b.Property(x => x.ForwardRefNo).HasMaxLength(32);
            b.Property(x => x.SubBlNo).HasMaxLength(32);
            b.Property(x => x.EctNo).HasMaxLength(32);
            b.Property(x => x.PrnNo).HasMaxLength(32);
            b.Property(x => x.AgentRefNo).HasMaxLength(128);
            b.Property(x => x.ItNo).HasMaxLength(128);
            b.Property(x => x.ItLocation).HasMaxLength(128);
        });
        builder.Entity<OceanImportHbl>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "OceanImportHbls",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.MblId).IsRequired();
            b.Property(x => x.HblNo).IsRequired().HasMaxLength(32);
            b.Property(x => x.CustomerRefNo).HasMaxLength(128);
            b.Property(x => x.DocNo).HasMaxLength(128);
            b.Property(x => x.B2bNo).HasMaxLength(32);
            b.Property(x => x.SoNo).HasMaxLength(32);
            b.Property(x => x.LcNo).HasMaxLength(32);
            b.Property(x => x.LcIssueBank).HasMaxLength(64);
            b.Property(x => x.HoldReason).HasMaxLength(32);
            b.Property(x => x.WoNo).HasMaxLength(32);
            b.Property(x => x.NraNo).HasMaxLength(32);
            b.Property(x => x.AmsNo).HasMaxLength(128);
            b.Property(x => x.IsfNo).HasMaxLength(128);
            b.Property(x => x.ItNo).HasMaxLength(128);
            b.Property(x => x.ItLocation).HasMaxLength(128);
            b.Property(x => x.ScNo).HasMaxLength(128);
            b.Property(x => x.NameAccount).HasMaxLength(128);
            b.Property(x => x.GroupComm).HasMaxLength(128);
            b.Property(x => x.LineCode).HasMaxLength(128);
        });
        builder.Entity<GridPreference>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "GridPreference",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();

        });

        builder.Entity<Airport>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "Airports",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.AirportName).HasMaxLength(64);
            b.Property(x => x.AirportIataCode).HasMaxLength(3);
        });
        builder.Entity<AirExportMawb>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "AirExportMawbs",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OfficeId).IsRequired();
            b.Property(x => x.ItnNo).HasMaxLength(32);
            b.Property(x => x.FlightNo).HasMaxLength(32);
            b.Property(x => x.DVCarriage).HasMaxLength(32);
            b.Property(x => x.DVCustomer).HasMaxLength(32);
            b.Property(x => x.Insurance).HasMaxLength(32);
            b.Property(x => x.CarrierSpotNo).HasMaxLength(32);
            b.Property(x => x.ShippingInfo).HasMaxLength(128);
            b.Property(x => x.ShipperLoad).HasMaxLength(128);
            b.Property(x => x.Sci).HasMaxLength(32);
            b.Property(x => x.RouteTrans1FlightNo).HasMaxLength(32);
            b.Property(x => x.RouteTrans2FlightNo).HasMaxLength(32);
            b.Property(x => x.RouteTrans3FlightNo).HasMaxLength(32);
        });

        builder.Entity<AirExportHawb>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "AirExportHawbs",
              FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.MawbId);
            b.Property(x => x.HawbNo);
            b.Property(x => x.BookingNo);
            b.Property(x => x.ITNNo);
            b.Property(x => x.QuotationId);
            b.Property(x => x.ActualShippedr);
            b.Property(x => x.ConsigneeId);
            b.Property(x => x.SalesId);
            b.Property(x => x.OPId);
            b.Property(x => x.Mark);
            b.Property(x => x.NatureAndQuantityOfGoods);
            b.Property(x => x.ManifestNatureAndQuantityOfGoods);
            b.Property(x => x.HandlingInformation);
            b.Property(x => x.BookingRemarks);
            b.Property(x => x.PickupInstruction);
        });

        builder.Entity<AirImportMawb>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "AirImportMawbs",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.OfficeId).IsRequired();
            b.Property(x => x.FlightNo).HasMaxLength(32);
            b.Property(x => x.MawbNo).HasMaxLength(32);
            b.Property(x => x.RouteTrans1FlightNo).HasMaxLength(32);
            b.Property(x => x.RouteTrans2FlightNo).HasMaxLength(32);
            b.Property(x => x.RouteTrans3FlightNo).HasMaxLength(32);
        });

          builder.Entity<AirImportHawb>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "AirImportHawbs",
              FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.MawbId);
            b.Property(x => x.HawbNo);
            b.Property(x => x.QuotationId);
            b.Property(x => x.Hsn);
            b.Property(x => x.ShipperId);
            b.Property(x => x.ConsigneeId);
            b.Property(x => x.OPId);
            b.Property(x => x.SalesId);
            b.Property(x => x.FreightLocation);
            b.Property(x => x.FinalDestination);
            b.Property(x => x.FinalETA);
            b.Property(x => x.ShipType);
            b.Property(x => x.ServiceTermStart);
            b.Property(x => x.ServiceTermTo);


        });

        builder.Entity<Dolphin.Freight.ReportLog.ReportLog>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "ReportLog",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
        });
        builder.Entity<TradePartnerAttachment>(b =>
        {
            b.ToTable(FreightConsts.DbTablePrefix + "TradePartnerAttachments",
                FreightConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.AttachmentName).IsRequired();
            b.Property(x => x.AttachmentName).HasMaxLength(255);
        });
    }
}
