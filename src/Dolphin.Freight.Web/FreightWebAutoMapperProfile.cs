﻿using AutoMapper;
using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Accounting.Payment;
using Dolphin.Freight.AccountingSettings.BillingCodes;
using Dolphin.Freight.AccountingSettings.CurrencyTables;
using Dolphin.Freight.AccountingSettings.GlCodes;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settings.AirOtherCharge;
using Dolphin.Freight.Settings.AwbNoRanges;
using Dolphin.Freight.Settings.ContainerSizes;
using Dolphin.Freight.Settings.Countries;
using Dolphin.Freight.Settings.ItNoRanges;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settinngs.ContainerSizes;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.TradePartners.Credits;
using Dolphin.Freight.Web.Pages.Sales.TradePartner;
using Dolphin.Freight.Web.Pages.Sales.TradePartner.Credit;
using Dolphin.Freight.Settings.DisplaySetting;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.CurrencySetting;
//using Dolphin.Freight.Accounting.Payment;
using Volo.Abp.AutoMapper;
using static Dolphin.Freight.Web.Pages.ReportScreen.VolumeProfileReportModel;
using Dolphin.Freight.ReportLog;
using static Dolphin.Freight.Web.Pages.AirImports.CreateMawbModel;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.Common;

namespace Dolphin.Freight.Web;

public class FreightWebAutoMapperProfile : Profile
{
    public FreightWebAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Web project.
        CreateMap<AirImportMawbDto, InvoiceMblDto>();
        CreateMap<ItNoRangeDto, CreateUpdateItNoRangeDto>();
        CreateMap<AirOtherChargeDTO, CreateUpdateAirOtherChargeDTO>();
        CreateMap<PortsManagementDTO, CreateUpdatePortsManagementDto>();
        CreateMap<AwbNoRangeDto, CreateUpdateAwbNoRangeDto>();
        CreateMap<PackageUnitDto, CreateUpdatePackageUnitDto>();
        CreateMap<ContainerSizeDto, CreateUpdateContainerSizeDto>();
        CreateMap<BillingCodeDto, CreateUpdateBillingCodeDto>();
        CreateMap<CurrencyTableDto, CreateUpdateCurrencyTableDto>();
        CreateMap<GlCodeDto, CreateUpdateGlCodeDto>();
        CreateMap<VesselScheduleDto,CreateUpdateVesselScheduleDto>();
        CreateMap<ExportBookingDto,CreateUpdateExportBookingDto>();
        CreateMap<ExportBooking, ExportBookingDto>();
        CreateMap<ExportBooking, CreateUpdateExportBookingDto>();
        CreateMap<OceanExportHblDto, OceanExportHbl>();
        CreateMap<AirExportMawbDto, InvoiceMblDto>();
        // TradePartner
        CreateMap<CreateEditCreditLimitGroupViewModel, CreateUpdateCreditLimitGroupDto>();
        CreateMap<CreditLimitGroupDto, CreateEditCreditLimitGroupViewModel>();
        CreateMap<Pages.Sales.TradePartner.TradePartnerInfoModel.CreateTradePartnerInfoViewModel, CreateUpdateTradePartnerDto>();
        CreateMap<CreateEditCreditLimitGroupViewModel, CreateUpdateCreditLimitGroupDto>();
        CreateMap<CreateEditTradePartnerInfoViewModel, CreateUpdateTradePartnerDto>();
        CreateMap<TradePartnerDto, CreateEditTradePartnerInfoViewModel>();
        CreateMap<TradePartnerDto, Pages.Sales.TradePartner.EditTradePartnerInfoModel.CreateTradePartnerInfoViewModel>();
        CreateMap<Pages.Sales.TradePartner.EditTradePartnerInfoModel.CreateTradePartnerInfoViewModel, CreateUpdateTradePartnerDto>();

        // TradePartner Attachment
        CreateMap<Pages.Sales.TradePartner.DocumentModel.DocumentUploadViewModel, CreateUpdateTradePartnerAttachmentDto>();

        // SysCode
        CreateMap<Settings.SysCodes.SysCode, CreateUpdateSysCodeDto>();
        CreateMap<SysCodeDto, CreateUpdateSysCodeDto>();
        CreateMap<CreateUpdateSysCodeDto, Settings.SysCodes.SysCode>();

        // ContactPerson
        CreateMap<Pages.Sales.TradePartner.ModalWithCreateContactPersonModel.CreateContactPersonViewModel, CreateUpdateContactPersonDto>();
        CreateMap<ContactPersonDto, Pages.Sales.TradePartner.ModalWithEditContactPersonModel.EditContactPersonViewModel>();
        CreateMap<Pages.Sales.TradePartner.ModalWithEditContactPersonModel.EditContactPersonViewModel, CreateUpdateContactPersonDto>();

        // 海運進口
        CreateMap<OceanExportMblDto, CreateUpdateOceanExportMblDto>();
        CreateMap<OceanExportHblDto, CreateUpdateOceanExportHblDto>();
        CreateMap<InvoiceDto, CreateUpdateInvoiceDto>();
        CreateMap<Invoice, CreateUpdateInvoiceDto>();
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<InvoiceDto, Invoice>();
        CreateMap<InvoiceBill, InvoiceBillDto>();
        CreateMap<InvoiceBillDto, InvoiceBill>();
        CreateMap<InvoiceBillDto, CreateUpdateInvoiceBillDto>();
        CreateMap<VesselScheduleDto, CreateUpdateVesselScheduleDto>();
        CreateMap<OceanExportHblDto, CreateUpdateOceanExportHblDto>();
		//Accounting
        CreateMap<CustomerPayment, CustomerPaymentDto>();
        CreateMap<CreateUpdateCustomerPaymentDto, CustomerPayment>();        
        CreateMap<OceanImportMblDto, CreateUpdateOceanImportMblDto>();
        CreateMap<OceanImportHblDto, CreateUpdateOceanImportHblDto>();

        // AirExportMawb
        CreateMap<Pages.AirExports.CreateMawbModel.CreateMawbViewModel, CreateUpdateAirExportMawbDto>();
        CreateMap<AirExportMawbDto, Pages.AirExports.CreateMawbModel.CreateMawbViewModel>();
        //AirExportBooking
        CreateMap<Dolphin.Freight.ImportExport.AirExports.Bookings.ExportBookingDto, Dolphin.Freight.ImportExport.AirExports.Bookings.CreateUpdateExportBookingDto>();
        // AirImportMawb
        CreateMap<Pages.AirImports.CreateMawbModel.CreateAIMMawbViewModel, CreateUpdateAirImportMawbDto>();
        CreateMap<AirImportMawbDto, Pages.AirImports.EditMawbModel.CreateAIMMawbViewModel>();
        CreateMap<Pages.AirImports.EditMawbModel.CreateAIMMawbViewModel, CreateUpdateAirImportMawbDto>();
        CreateMap<AirImportMawbDto, CreateAIMMawbViewModel>();

        // 發票
        CreateMap<CreateUpdateOceanExportMblDto, InvoiceMblDto>();
        CreateMap<CreateUpdateOceanImportMblDto, InvoiceMblDto>();
        CreateMap<CreateUpdateOceanExportHblDto, InvoiceMblDto>();
        CreateMap<CreateUpdateOceanImportHblDto, InvoiceMblDto>();
        CreateMap<ExportBookingDto, InvoiceMblDto>();
        CreateMap<OceanExportMblDto, InvoiceMblDto>();
        CreateMap<CreateUpdateContainerDto, Container>();
        // 國家管理
        //CreateMap<CountryDisplayName, CreateUpdateCountryDisplayNameDto>();
		//DisplaySetting
        CreateMap<CreateUpdateDisplaySettingDTO, DisplaySettingDTO>();
        //貨幣表
        CreateMap<CurrencySettingDTO, CreateUpdateCurrencySettingDTO>();

    }
}
