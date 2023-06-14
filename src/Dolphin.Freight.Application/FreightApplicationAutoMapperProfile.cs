using AutoMapper;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Settings.AwbNoRanges;
using Dolphin.Freight.Settings.ItNoRanges;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.TradePartners.Credits;
using Volo.Abp.AutoMapper;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settings.ContainerSizes;
using Dolphin.Freight.Settinngs.ContainerSizes;
using Dolphin.Freight.AccountingSettings.BillingCodes;
using Dolphin.Freight.AccountingSettings.CurrencyTables;
using Dolphin.Freight.AccountingSettings.GlCodes;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settinngs.Ports;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Accounting.Inv;
using System;
using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Payment;
using Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.Settings.Countries;
using Dolphin.Freight.Settings.Currencies;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ReportLog;
using Dolphin.Freight.Settings.AirOtherCharge;
using Dolphin.Freight.Settinngs.AirOtherCharge;
using Dolphin.Freight.Settings.DisplaySetting;
using Dolphin.Freight.Settings.Offices;
using Dolphin.Freight.Settinngs.Offices;
using Dolphin.Freight.TradePartners.TradeParties;
using Dolphin.Freight.TradePartners.TradeParties;
using Dolphin.Freight.Common.Memos;

using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Common.Memos;
using Dolphin.Freight.Settings.CurrencySetting;
using Dolphin.Freight.TradePartners.DefaultFreight;
using Dolphin.Freight.ImportExport.Configuration;


namespace Dolphin.Freight;

public class FreightApplicationAutoMapperProfile : Profile
{
    public FreightApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<ItNoRange, ItNoRangeDto>();
        CreateMap<AirOtherCharge, AirOtherChargeDTO>();
        CreateMap<CreateUpdateAirOtherChargeDTO, AirOtherCharge>();
		CreateMap<PortsManagement,  PortsManagementDTO>();
        CreateMap<CreateUpdatePortsManagementDto, PortsManagement>();
        CreateMap<CurrencySetting, CurrencySettingDTO>();
        CreateMap<CreateUpdateCurrencySettingDTO, CurrencySetting>();
        CreateMap<CreateUpdateItNoRangeDto, ItNoRange>();
        CreateMap<AwbNoRange, AwbNoRangeDto>();
        CreateMap<CreateUpdateAwbNoRangeDto, AwbNoRange>();
        CreateMap<ContainerSize, ContainerSizeDto>();
        CreateMap<CreateUpdateContainerSizeDto, ContainerSize>();
        CreateMap<Substation, SubstationDto>();
        CreateMap<CreateUpdateSubstationDto, Substation>();
        CreateMap<CreateUpdateItNoRangeDto, ItNoRange>();
        CreateMap<BillingCode,BillingCodeDto>();
        CreateMap<CreateUpdateBillingCodeDto, BillingCode>();
        CreateMap<CurrencyTable,CurrencyTableDto>();
        CreateMap<CreateUpdateCurrencyTableDto, CurrencyTable>();
        CreateMap<GlCode, GlCodeDto>();
        CreateMap<CreateUpdateGlCodeDto, GlCode>();
        CreateMap<Port, PortDto>();
        CreateMap<CreateUpdatePortDto, Port>();

        
        CreateMap<CreateUpdateInvoiceBillDto, InvoiceBill>();
        CreateMap<InvoiceBill, InvoiceBillDto>();
        CreateMap<InvoiceBillDto, InvoiceBill>();


        CreateMap<CustomerPayment, CustomerPaymentDto>();
            //.ForMember(x => x.ReceivablesSources, opt => opt.MapFrom(src => src.ReceivablesSourcesName == null ? "" : src.ReceivablesSourcesName.TPName));
        CreateMap<CreateUpdateCustomerPaymentDto, CustomerPayment>();

        CreateMap<Payment, PaymentDto>();
            //.ForMember(a => a.PaidTo, opt => opt.MapFrom(src => src.PaidToName == null ? "" : src.PaidToName.TPName));
        CreateMap<CreateUpdatePaymentDto, Payment>();

        CreateMap<VesselSchedule, VesselScheduleDto>();
        CreateMap<VesselScheduleDto, VesselSchedule>();
        CreateMap<CreateUpdateVesselScheduleDto, VesselSchedule>();
        CreateMap<ExportBooking, ExportBookingDto>();
        CreateMap<ExportBookingDto, ExportBooking>();
        CreateMap<CreateUpdateExportBookingDto, ExportBooking>();
        CreateMap<Attachment, AttachmentDto>();
        CreateMap<AttachmentDto, Attachment>();
        CreateMap<CreateUpdateAttachmentDto, Attachment>();
        CreateMap<Container, ContainerDto>();
        CreateMap<ContainerDto, Container>();
        CreateMap<CreateUpdateContainerDto, Container>();
        CreateMap<Memo, MemoDto>();
        CreateMap<MemoDto, Memo>();
        CreateMap<CreateUpdateMemoDto, Memo>();
        // TradePartner Credit
        CreateMap<CreditLimitGroup, CreditLimitGroupDto>();
        CreateMap<CreateUpdateCreditLimitGroupDto, CreditLimitGroup>();
        CreateMap<CreditLimitGroup, CreditLimitGroupNameLookupDto>();
        CreateMap<CreditLimitGroup, CreditLimitGroupDto>();
        CreateMap<CreateUpdateCreditLimitGroupDto, CreditLimitGroup>();
        CreateMap<CreditLimitGroup, CreditLimitGroupNameLookupDto>();

        // TradePartner
        CreateMap<Dolphin.Freight.TradePartners.TradePartner, TradePartnerDto>();
        CreateMap<CreateUpdateTradePartnerDto, Dolphin.Freight.TradePartners.TradePartner>()
            .IgnoreFullAuditedObjectProperties()
            .Ignore(c => c.IsDeleted)
            .Ignore(c => c.DeleterId)
            .Ignore(c => c.DeletionTime)
            .Ignore(c => c.ExtraProperties)
            .Ignore(c => c.ConcurrencyStamp);
        CreateMap<Dolphin.Freight.TradePartners.TradePartner, TradePartnerLookupDto>();

        // TradePartner Attachment
        CreateMap<TradePartnerAttachment, TradePartnerAttachmentDto>();
        CreateMap<CreateUpdateTradePartnerAttachmentDto, TradePartnerAttachment>();

        // AccountGroup
        CreateMap<AccountGroup, AccountGroupDto>();
        CreateMap<CreateUpdateAccountGroupDto, AccountGroup>();
        //OceanExport
        CreateMap<OceanExportMbl, OceanExportMblDto>();
        CreateMap<CreateUpdateOceanExportMblDto, OceanExportMbl>();
        CreateMap<OceanExportMbl,CreateUpdateOceanExportMblDto >();
        CreateMap<OceanExportHbl, OceanExportHblDto>();
        CreateMap<OceanExportHbl, CreateUpdateOceanExportHblDto>();
        CreateMap<CreateUpdateOceanExportHblDto, OceanExportHbl>();
        CreateMap<SysCode, SysCodeDto>();
        CreateMap<PackageUnit, PackageUnitDto>();
        CreateMap<CreateUpdatePackageUnitDto, PackageUnit>();

        // ContactPerson
        CreateMap<ContactPerson, ContactPersonDto>();
        CreateMap<ContactPersonPagedAndSortedResultRequestDto, ContactPersonFilter>();

		// Country
        CreateMap<Country, CountryLookupDto>();
        // Currency
        CreateMap<Currency, CurrencyLookupDto>();
        //OceanImport
        CreateMap<OceanImportMbl, OceanImportMblDto>();
        CreateMap<CreateUpdateOceanImportMblDto, OceanImportMbl>();
        CreateMap<OceanImportMbl, CreateUpdateOceanImportMblDto>();
        CreateMap<OceanImportHbl, OceanImportHblDto>();
        CreateMap<OceanImportHbl, CreateUpdateOceanImportHblDto>();
        CreateMap<CreateUpdateOceanImportHblDto, OceanImportHbl>();

        // Substation
        CreateMap<Substation, SubstationLookupDto>();

        // Airport
        CreateMap<Airport, AirportDto>();

        // PackageUnit
        CreateMap<PackageUnit, PackageUnitLookupDto>();

        // AirExportMawb
        CreateMap<AirExportMawb, AirExportMawbDto>();
        CreateMap<CreateUpdateAirExportMawbDto, AirExportMawb>();
        CreateMap<AirExportMawbDto, CreateUpdateAirExportMawbDto>();

        // AirExportHawb
        CreateMap<AirExportHawb, AirExportHawbDto>();
        CreateMap<CreateUpdateAirExportHawbDto, AirExportHawb>();
        CreateMap<AirExportHawbDto, CreateUpdateAirExportHawbDto>();

        // AirImportMawb
        CreateMap<AirImportMawb, AirImportMawbDto>();
        CreateMap<CreateUpdateAirImportMawbDto, AirImportMawb>();

        // AirExportHawb
        CreateMap<AirImportHawb, AirImportHawbDto>();
        CreateMap<CreateUpdateAirImportHawbDto, AirImportHawb>();
        CreateMap<AirImportHawbDto, CreateUpdateAirImportHawbDto>();

        #region iFreight 的資料表

        // STEMP
        CreateMap<iFreightDB.BaseTables.Stemps.Stemp, iFreightDB.BaseTables.Stemps.Stemp_Dto>();
        CreateMap<iFreightDB.BaseTables.Stemps.Stemp_CreateUpdateDto, iFreightDB.BaseTables.Stemps.Stemp>();

        // BSFRTCENTERTD
        CreateMap<iFreightDB.BaseTables.Bsfrtcentertds.Bsfrtcentertd, iFreightDB.BaseTables.Bsfrtcentertds.Bsfrtcentertd_Dto>();
        CreateMap<iFreightDB.BaseTables.Bsfrtcentertds.Bsfrtcentertd_CreateUpdateDto, iFreightDB.BaseTables.Bsfrtcentertds.Bsfrtcentertd>();

        // BSFRTCENTERTM
        CreateMap<iFreightDB.BaseTables.Bsfrtcentertms.Bsfrtcentertm, iFreightDB.BaseTables.Bsfrtcentertms.Bsfrtcentertm_Dto>();
        CreateMap<iFreightDB.BaseTables.Bsfrtcentertms.Bsfrtcentertm_CreateUpdateDto, iFreightDB.BaseTables.Bsfrtcentertms.Bsfrtcentertm>();

        // BSFRTCENTERTM
        CreateMap<iFreightDB.BaseTables.BsfrtcentertExcelTemps.BsfrtcentertExcelTemp, iFreightDB.BaseTables.BsfrtcentertExcelTemps.BsfrtcentertExcelTemp_Dto>();
        CreateMap<iFreightDB.BaseTables.BsfrtcentertExcelTemps.BsfrtcentertExcelTemp_CreateUpdateDto, iFreightDB.BaseTables.BsfrtcentertExcelTemps.BsfrtcentertExcelTemp>();

        // BSFRTCENTERT_PORT_MAPPING
        CreateMap<iFreightDB.BaseTables.BsfrtcentertPortMappings.BsfrtcentertPortMapping, iFreightDB.BaseTables.BsfrtcentertPortMappings.BsfrtcentertPortMapping_Dto>();
        CreateMap<iFreightDB.BaseTables.BsfrtcentertPortMappings.BsfrtcentertPortMapping_CreateUpdateDto, iFreightDB.BaseTables.BsfrtcentertPortMappings.BsfrtcentertPortMapping>();

        #endregion

        CreateMap<CreateUpdateOceanImportHblDto, OceanImportHbl>();        
        CreateMap<Inv, InvDto>()
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.HasValue ? src.DueDate.Value.ToString("yyyy-MM-dd") : ""))
            .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(src => src.InvoiceDate.HasValue ? src.InvoiceDate.Value.ToString("yyyy-MM-dd") : ""))
            .ForMember(dest => dest.PostDate, opt => opt.MapFrom(src => src.PostDate.HasValue ? src.PostDate.Value.ToString("yyyy-MM-dd") : ""))
            .ForMember(dest => dest.InvoiceAmount, opt => opt.MapFrom(src => src.InvoiceAmount.HasValue? src.InvoiceAmount.Value.ToString("#0.00") : ""))
            .ForMember(dest => dest.BalanceAmount, opt => opt.MapFrom(src => src.BalanceAmount.HasValue ? src.BalanceAmount.Value.ToString("#0.00") : ""))
            .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.PaymentAmount.HasValue ? src.PaymentAmount.Value.ToString("#0.00") : ""))
            .ForMember(dest => dest.PaymentAmountTwd, opt => opt.MapFrom(src => src.PaymentAmountTwd.HasValue ? src.PaymentAmountTwd.Value.ToString("#0.00") : ""));
        CreateMap<CreateUpdateInvDto, Inv>();

        CreateMap<Invoice, InvoiceDto>();
        /*
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.HasValue ? src.DueDate.Value.ToString("yyyy-MM-dd") : ""))
            .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(src => src.InvoiceDate.HasValue ? src.InvoiceDate.Value.ToString("yyyy-MM-dd") : ""))
            .ForMember(dest => dest.PostDate, opt => opt.MapFrom(src => src.PostDate.HasValue ? src.PostDate.Value.ToString("yyyy-MM-dd") : ""))
            .ForMember(dest => dest.InvoiceAmount, opt => opt.MapFrom(src => src.InvoiceAmount.HasValue ? src.InvoiceAmount.Value.ToString("#0.00") : ""))
            .ForMember(dest => dest.BalanceAmount, opt => opt.MapFrom(src => src.BalanceAmount.HasValue ? src.BalanceAmount.Value.ToString("#0.00") : ""))
            .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.PaymentAmount.HasValue ? src.PaymentAmount.Value.ToString("#0.00") : ""))
            .ForMember(dest => dest.PaymentAmountTwd, opt => opt.MapFrom(src => src.PaymentAmountTwd.HasValue ? src.PaymentAmountTwd.Value.ToString("#0.00") : ""));
        */
        CreateMap<CreateUpdateInvoiceDto, Invoice>();

        CreateMap<Country, CountryDto>();
        CreateMap<CreateUpdateDisplaySettingDTO, DisplaySetting>();
        CreateMap<DisplaySetting, DisplaySettingDTO>();
        CreateMap<Dolphin.Freight.ReportLog.ReportLog, ReportLogDto>();
        CreateMap<CreateUpdateReportLogDto, Dolphin.Freight.ReportLog.ReportLog>();
        CreateMap<ReportLogDto, Dolphin.Freight.ReportLog.ReportLog>();

        CreateMap<CountryDisplayName, CountryDisplayNameDto>();
        CreateMap<CreateUpdateCountryDisplayNameDto, CountryDisplayName>();

        CreateMap<Office, OfficeDto>();
        CreateMap<TradePartnerMemo, TradePartnerMemoDto>();
        CreateMap<TradeParty, TradePartyDto>();

		CreateMap<Memo, MemoDto>();
        CreateMap<DefaultFreightAR, DefaultFreightARDto>();
        CreateMap<DefaultFreightAP, DefaultFreightAPDto>();
        CreateMap<DefaultFreightDC, DefaultFreightDCDto>();
        CreateMap<DefaultFreightAR, DefaultFreightARListDto>();
        CreateMap<DefaultFreightAP, DefaultFreightAPListDto>();
        CreateMap<DefaultFreightDC, DefaultFreightDCListDto>();
        CreateMap<CreateUpdateDefaultFreightARDto, DefaultFreightAR>();
        CreateMap<CreateUpdateDefaultFreightAPDto, DefaultFreightAP>();
        CreateMap<CreateUpdateDefaultFreightDCDto, DefaultFreightDC>();

        CreateMap<MawbReport, MawbReportDto>();

        // Configuration - Grid preference
        CreateMap<GridPreference, GridPreferenceDto>();
        CreateMap<CreateUpdateGridPreferenceDto, GridPreference>();
        CreateMap<GridPreferenceDto, CreateUpdateGridPreferenceDto>();
    }
}
