using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.AccountingSettings.BillingCodes;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settings.CurrencySetting;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Ports;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using static Dolphin.Freight.Permissions.OceanExportPermissions;
using QueryInvoiceDto = Dolphin.Freight.Accounting.Invoices.QueryInvoiceDto;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.Common.Memos;

namespace Dolphin.Freight.Web.Pages.Accounting
{
    public class InvoiceModel : FreightPageModel
    {
        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public int InvoiceType { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? InvoiceId { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid? CopyInvoiceId { get; set; }
        /// <summary>
        /// 0：海運進口、1：海運出口、2：空運進口、3：空運出口、4：SO清單、5：船期
        /// </summary>
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public int MethodType { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? Mid { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? Hid { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? Bid { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? MawbId { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? HawbId { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? VesselId { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<OceanExportHblDto> OceanExportHblDtos { get; set; }
        public string Hblno { get; set; }
        [BindProperty]
        public IList<CreateUpdateInvoiceBillDto> InvoiceBillDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public CreateUpdateInvoiceDto InvoiceDto { get; set; }
        public InvoiceBasicDto InvoiceBasicDto { get; set; }
        [BindProperty]
        public InvoiceMblDto InvoiceMblDto { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SysCodeLookupList { get; set; }
        public Dictionary<string, float> DictCurrencyConversion { get; set; }
        public string CurrencyConversionJson { get; set; }
        public FreightPageType FType { get; set; }
        public string backUrl { get; set; }

        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        private readonly IInvoiceBillAppService _invoiceBillAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IPortAppService _portAppService;
        private readonly IAirExportHawbAppService _airExportHawbAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly ICurrencySettingAppService _currencySettingAppService;
        private readonly IExportBookingAppService _exportBookingAppService;
        private readonly IAirExportMawbAppService _airExportMawbAppService;
        private readonly IAirImportHawbAppService _airImportHawbAppService;
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        private readonly IPortsManagementAppService _portsManagementAppService;
        private readonly IRepository<PackageUnit, Guid> _packageUnitAppService;
        private readonly IMemoAppService _memosAppService;

        public InvoiceModel(ITradePartnerAppService tradePartnerAppService, ISubstationAppService substationAppService,
                            IInvoiceAppService invoiceAppService, IOceanExportMblAppService oceanExportMblAppService,
                            IOceanExportHblAppService oceanExportHblAppService, IInvoiceBillAppService invoiceBillAppService,
                            IPortAppService portAppService, OceanImportHblAppService oceanImportHblAppService, 
                            OceanImportMblAppService oceanImportMblAppService, IAjaxDropdownAppService ajaxDropdownAppService,
                            ICurrencySettingAppService currencySettingAppService, IAirExportHawbAppService airExportHawbAppService,
                            IExportBookingAppService exportBookingAppService, IAirExportMawbAppService airExportMawbAppService,
                            IPortsManagementAppService portsManagementAppService, IRepository<PackageUnit, Guid> packageUnitAppService,
                            IAirImportHawbAppService airImportHawbAppService, IAirImportMawbAppService airImportMawbAppService,
                            IMemoAppService memosAppService
                           )
        {
            _invoiceAppService = invoiceAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _oceanImportMblAppService = oceanImportMblAppService;
            _invoiceBillAppService = invoiceBillAppService;
            _substationAppService = substationAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _portAppService = portAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _currencySettingAppService = currencySettingAppService;
            _airExportHawbAppService = airExportHawbAppService;
            _exportBookingAppService = exportBookingAppService;
            _airExportMawbAppService = airExportMawbAppService;
            _portsManagementAppService = portsManagementAppService;
            _packageUnitAppService = packageUnitAppService;
            _airImportHawbAppService = airImportHawbAppService;
            _airImportMawbAppService = airImportMawbAppService;
            _memosAppService = memosAppService;
        }
        public async Task OnGetAsync()
        {
            InvoiceBasicDto = new InvoiceBasicDto();

            switch (MethodType)
            {
                case 0:
                    await InitOceanImport();
                    break;
                case 1:
                    await InitOceanExport();
                    break;
                case 2:
                    await InitAirImport();
                    break;
                case 3:
                    await InitAirExport();
                    break;
                case 4:
                    await InitExportBooking();
                    break;
                case 5:
                    await InitVesselSchedule();
                    break;
                case 6:
                    await InitAirImport();
                    break;
                case 7:
                    await InitAirImport();
                    break;
                default:
                    await InitOceanExport();
                    break;
            }
            
            if (InvoiceId != null||CopyInvoiceId!=null)
            {
                var id = InvoiceId != null ? InvoiceId : CopyInvoiceId;
                var invoice = await _invoiceAppService.GetAsync((Guid)id);
                InvoiceDto = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                InvoiceDto.Id = CopyInvoiceId != null ? Guid.Empty : InvoiceDto.Id;
            }

            if (InvoiceMblDto != null ) 
            {
                if (InvoiceMblDto.OfficeId != Guid.Empty) 
                {
                    var substation = await _substationAppService.GetAsync(InvoiceMblDto.OfficeId);
                    InvoiceBasicDto.AbbreviationName = substation.SubstationName + " (" + substation.AbbreviationName + ")";
                }
                if (InvoiceMblDto.MblConsigneeId != null && InvoiceMblDto.MblConsigneeId.Value != Guid.Empty) 
                {
                    var tradePartner = await _tradePartnerAppService.GetAsync(InvoiceMblDto.MblConsigneeId.Value);
                    InvoiceBasicDto.MblConsigneeName = tradePartner.TPName;
                    InvoiceMblDto.MblOverseaAgentName = tradePartner.TPName;
                }
                if (InvoiceMblDto.HblConsigneeId is not null && InvoiceMblDto.HblConsigneeId.Value != Guid.Empty)
                {
                    var tradePartner = await _tradePartnerAppService.GetAsync(InvoiceMblDto.HblConsigneeId.Value);
                    InvoiceBasicDto.MblConsigneeName = tradePartner.TPName;
                }
                if (InvoiceMblDto.ShipperId is not null && InvoiceMblDto.ShipperId.Value != Guid.Empty)
                {
                    var tradePartner = await _tradePartnerAppService.GetAsync(InvoiceMblDto.ShipperId.Value);
                    InvoiceBasicDto.ShipperName = tradePartner.TPName;
                }
                if (InvoiceMblDto.MblNotifyId != null && InvoiceMblDto.MblNotifyId.Value != Guid.Empty)
                {
                    var tradePartner = await _tradePartnerAppService.GetAsync(InvoiceMblDto.MblNotifyId.Value);
                    InvoiceBasicDto.MblNotifyName = tradePartner.TPName;
                }
                if (InvoiceMblDto.DepatureId is not null && InvoiceMblDto.DepatureId.Value != Guid.Empty)
                {
                    var portManagement = await _portsManagementAppService.QueryListAsync();
                    InvoiceMblDto.PolName = portManagement.Where(w => w.Id == InvoiceMblDto.DepatureId.Value).Select(s => s.PortName).FirstOrDefault();
                }
                if (InvoiceMblDto.DestinationId is not null && InvoiceMblDto.DestinationId.Value != Guid.Empty)
                {
                    var portManagement = await _portsManagementAppService.QueryListAsync();
                    InvoiceMblDto.PodName = portManagement.Where(w => w.Id == InvoiceMblDto.DestinationId.Value).Select(s => s.PortName).FirstOrDefault();
                }
                if (InvoiceMblDto.PackageCategoryId is not null && InvoiceMblDto.PackageCategoryId.Value != Guid.Empty)
                {
                    var packageUnit = await _packageUnitAppService.GetQueryableAsync();
                    InvoiceBasicDto.PackageUnitName = packageUnit.Where(w => w.Id == InvoiceMblDto.PackageCategoryId).Select(s => s.PackageName).FirstOrDefault();
                }
                InvoiceBasicDto.VesselNameVoyage = "";
                if (InvoiceMblDto.VesselName != null) InvoiceBasicDto.VesselNameVoyage = InvoiceMblDto.VesselName;
                if (InvoiceMblDto.Voyage != null) 
                {
                    if (InvoiceBasicDto.VesselNameVoyage.Length > 0) InvoiceBasicDto.VesselNameVoyage = InvoiceBasicDto.VesselNameVoyage + "/";
                    InvoiceBasicDto.VesselNameVoyage = InvoiceBasicDto.VesselNameVoyage + InvoiceMblDto.Voyage;
                }
            }

            await FillTradePartnerAsync();
            await FillSysCodeAsync();
            await FillCurrencySettingsDictionary();

        }

        public async Task FillCurrencySettingsDictionary()
        {
            var currencySettings = await _currencySettingAppService.GetCurrenciesAsync();
            DictCurrencyConversion = new ();
            foreach (var currencySetting in currencySettings)
            {
                DictCurrencyConversion.Add($"{currencySetting.StartingCurrency}-{currencySetting.EndCurrency}", currencySetting.ExChangeRate);
            }

            CurrencyConversionJson = JsonConvert.SerializeObject(DictCurrencyConversion);
        }

        private async Task InitOceanExport() 
        {
            QueryInvoiceDto query = new QueryInvoiceDto() { QueryInvoiceType = InvoiceType };
            if (Mid == null)
            {
                var oceanExportHbl = await _oceanExportHblAppService.GetAsync(Hid.Value);
                if (oceanExportHbl != null)
                {
                    query.ParentId = Hid;
                    query.QueryType = 1;
                }
            }
            else
            {
                query.ParentId = Mid;
                query.QueryType = 0;
            }
            if (Hid == null)
            {
                var createUpdateOceanExportMblDto = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(Mid.Value);

                InvoiceMblDto = ObjectMapper.Map<CreateUpdateOceanExportMblDto, InvoiceMblDto>(createUpdateOceanExportMblDto);

                backUrl = "/OceanExports/EditModal3?Id=" + Mid;
                
            }
            else {

                var oceanExportHbl =ObjectMapper.Map <OceanExportHblDto, CreateUpdateOceanExportHblDto>(await _oceanExportHblAppService.GetAsync(Hid.Value));

                InvoiceMblDto = ObjectMapper.Map<CreateUpdateOceanExportHblDto, InvoiceMblDto>(oceanExportHbl);

                backUrl = "/OceanExports/EditModal3?Id=" + oceanExportHbl.MblId;
                Mid = oceanExportHbl.MblId;
            }
        }
        private async Task InitOceanImport()
        {
            QueryInvoiceDto query = new QueryInvoiceDto() { QueryInvoiceType = InvoiceType };
            if (Mid == null)
            {
                //如果沒有Mid，表示這是Hbl的
                var oceanImportHbl = await _oceanImportHblAppService.GetAsync(Hid.Value);
                if (oceanImportHbl != null)
                {
                    query.ParentId = Hid;
                    query.QueryType = 1;
                }
            }
            else
            {
                //如果有Mid，表示這是Mbl的
                query.ParentId = Mid;
                query.QueryType = 3;
            }
            if (Hid == null)
            {
               
                var createUpdateOceanImportMblDto = await _oceanImportMblAppService.GetCreateUpdateOceanImportMblDtoById(Mid.Value);

                InvoiceMblDto = ObjectMapper.Map<CreateUpdateOceanImportMblDto, InvoiceMblDto>(createUpdateOceanImportMblDto);

                backUrl = "/OceanImports/EditModal3?Id=" + Mid;
            }
            else
            {
                var oceanImportHbl = ObjectMapper.Map<OceanImportHblDto, CreateUpdateOceanImportHblDto>(await _oceanImportHblAppService.GetAsync(Hid.Value));

                InvoiceMblDto = ObjectMapper.Map<CreateUpdateOceanImportHblDto, InvoiceMblDto>(oceanImportHbl);

                backUrl = "/OceanImports/EditModal3?Id=" + oceanImportHbl.MblId;
                Mid = oceanImportHbl.MblId;
            }
        }
        private async Task InitAirImport()
        {
            QueryInvoiceDto query = new QueryInvoiceDto() { QueryInvoiceType = InvoiceType };
            
            if (MawbId != null)
            {
                var AirImportMbl = await _airImportMawbAppService.GetAsync((Guid)MawbId);

                InvoiceMblDto = ObjectMapper.Map<AirImportMawbDto, InvoiceMblDto>(AirImportMbl);
 
                InvoiceMblDto.MblConsigneeId = AirImportMbl.ConsigneeId;
                InvoiceMblDto.MblNotifyId = AirImportMbl.NotifyId;
                InvoiceMblDto.PolEtd = AirImportMbl.DepatureDate;
                InvoiceMblDto.PodEta = AirImportMbl.ArrivalDate;
                InvoiceMblDto.PackageCategoryId = AirImportMbl.MawbPackageUnitId;
                InvoiceMblDto.MblNo = AirImportMbl.MawbNo;

                backUrl = "/AirImports/EditModal3?Id=" + MawbId;
            }
            else { 
                var AirImportHawb = await _airImportHawbAppService.GetAsync(HawbId.Value);

                var AirImportMawb = await _airImportMawbAppService.GetAsync(AirImportHawb.MawbId.Value);

                InvoiceMblDto = ObjectMapper.Map<AirImportHawbDto, InvoiceMblDto>(AirImportHawb);

                InvoiceMblDto.MblConsigneeId = AirImportHawb.ConsigneeId;
                InvoiceMblDto.Package = Convert.ToDouble(AirImportHawb.Package);
                Hblno = AirImportHawb.HawbNo;
                InvoiceMblDto.MblNo = AirImportMawb.MawbNo;
                InvoiceMblDto.GrossWeightKg = Convert.ToDouble(AirImportHawb.GrossWeightKG);
                InvoiceMblDto.GrossWeightLb = Convert.ToDouble(AirImportHawb.GrossWeightLB);
                InvoiceMblDto.PolEtd = AirImportMawb.DepatureDate;
                InvoiceMblDto.PodEta = AirImportMawb.ArrivalDate;
                InvoiceMblDto.VolumeWeightKg = Convert.ToDouble(AirImportHawb.VolumeWeightKG);
                InvoiceMblDto.VolumeWeightCbm = Convert.ToDouble(AirImportHawb.VolumeWeightCBM);
                if (AirImportHawb.ShipperId is not null) InvoiceMblDto.ShipperId = AirImportHawb.ShipperId;
                if (AirImportHawb.Notify is not null) InvoiceMblDto.MblNotifyId = Guid.Parse(AirImportHawb.Notify);
                if (AirImportHawb.PackageUnit is not null) InvoiceMblDto.PackageCategoryId = Guid.Parse(AirImportHawb.PackageUnit);

                backUrl = "/AirImports/EditModal3?Id=" + AirImportHawb.MawbId + "&Hid=" + HawbId;
            }
        }
        private async Task InitAirExport()
        {
            QueryInvoiceDto query = new QueryInvoiceDto() { QueryInvoiceType = InvoiceType };
            if (MawbId is not null)
            {
                var airExportMbl = await _airExportMawbAppService.GetAsync(MawbId.Value);

                InvoiceMblDto = ObjectMapper.Map<AirExportMawbDto, InvoiceMblDto>(airExportMbl);
                InvoiceMblDto.MblConsigneeId = airExportMbl.ConsigneeId;
                InvoiceMblDto.MblNotifyId = airExportMbl.NotifyId;
                InvoiceMblDto.PolEtd = airExportMbl.DepatureDate;
                InvoiceMblDto.PodEta = airExportMbl.ArrivalDate;
                InvoiceMblDto.PackageCategoryId = airExportMbl.MawbPackageUnitId;
                InvoiceMblDto.MblNo = airExportMbl.MawbNo;

                backUrl = "/AirExports/EditModal3?Id=" + MawbId + "&Hid=" + HawbId;
            }
            else if (HawbId is not null)
            {
                var airexportHawb = await _airExportHawbAppService.GetAsync(HawbId.Value);
                var airExportMawb = await _airExportMawbAppService.GetAsync(airexportHawb.MawbId.Value);

                InvoiceMblDto = ObjectMapper.Map<AirExportMawbDto, InvoiceMblDto>(airExportMawb);
                InvoiceMblDto.Package = Convert.ToDouble(airexportHawb.Package);
                InvoiceMblDto.MblNo = airExportMawb.MawbNo;
                Hblno = airexportHawb.HawbNo;
                InvoiceMblDto.MblConsigneeId = airExportMawb.ConsigneeId;
                InvoiceMblDto.GrossWeightKg = Convert.ToDouble(airexportHawb.GrossWeightShprKG);
                InvoiceMblDto.GrossWeightLb = Convert.ToDouble(airexportHawb.GrossWeightShprLB);
                InvoiceMblDto.PolEtd = airExportMawb.DepatureDate;
                InvoiceMblDto.PodEta = airExportMawb.ArrivalDate;
                InvoiceMblDto.VolumeWeightKg = Convert.ToDouble(airexportHawb.VolumeWeightKG);
                InvoiceMblDto.VolumeWeightCbm = Convert.ToDouble(airexportHawb.VolumeWeightCBM);
                if (airexportHawb.ActualShippedr is not null) InvoiceMblDto.ShipperId = Guid.Parse(airexportHawb.ActualShippedr);
                if (airexportHawb.Notify is not null) InvoiceMblDto.MblNotifyId = Guid.Parse(airexportHawb.Notify);
                if (airexportHawb.OverseaAgent is not null) InvoiceMblDto.HblConsigneeId = Guid.Parse(airexportHawb.OverseaAgent);
                if (airexportHawb.PackageUnit is not null) InvoiceMblDto.PackageCategoryId = Guid.Parse(airexportHawb.PackageUnit);

                backUrl = "/AirExports/EditModal3?Id=" + airexportHawb.MawbId + "&Hid=" + HawbId;
                MawbId = airexportHawb.MawbId;
            }
        }
        private async Task InitExportBooking() 
        {
            QueryInvoiceDto query = new QueryInvoiceDto() { QueryInvoiceType = InvoiceType };
            if (Bid is not null)
            {
                var exportBooking = await _exportBookingAppService.GetAsync(Bid.Value);

                InvoiceMblDto = ObjectMapper.Map<ExportBookingDto, InvoiceMblDto>(exportBooking);

                backUrl = "/OceanExports/VesselSchedules/Edit2?Id=" + exportBooking.VesselScheduleId;

                VesselId = exportBooking.VesselScheduleId;
            }
        }
        private async Task InitVesselSchedule()
        {
            QueryInvoiceDto query = new QueryInvoiceDto() { QueryInvoiceType = InvoiceType };
            var oceanExportMbl = new OceanExportMblDto();
            InvoiceMblDto = ObjectMapper.Map<OceanExportMblDto, InvoiceMblDto>(oceanExportMbl);

           backUrl = "/OceanExports/VesselSchedules/Edit2?Id=" + VesselId;
        }
        public async Task<JsonResult> OnPostAsync()
        {
            InvoiceDto.MblId = Mid;
            InvoiceDto.HblId = Hid;
            InvoiceDto.BookingId = Bid;
            InvoiceDto.MawbId = MawbId;
            InvoiceDto.HawbId = HawbId;
            InvoiceDto.VesselScheduleId = VesselId;

            if (InvoiceDto.InvoiceNo == null) GetInvoiceNo(InvoiceType);

            var invoice = new InvoiceDto();

            if (InvoiceDto.Id == Guid.Empty) {
                invoice = await _invoiceAppService.CreateAsync(InvoiceDto);
            }
            else {
                invoice = await _invoiceAppService.UpdateAsync(InvoiceDto.Id, InvoiceDto); 
            }

            InvoiceDto.Id = invoice.Id;

            if (InvoiceBillDtos != null && InvoiceBillDtos.Count > 0) await SaveInvoiceBillsAsync(invoice.Id, InvoiceBillDtos);

            await SaveMemoSourceId(InvoiceDto.Id, InvoiceType);

            Dictionary<string, object> rs = new Dictionary<string, object>();
            rs.Add("a1", "ejo");

            return new JsonResult(rs);
        }

        #region FillTradePartnerAsync()
        private async Task FillTradePartnerAsync()
        {
            var tradePartnerLookup = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            TradePartnerLookupList = tradePartnerLookup.Items
                                                .Select(x => new SelectListItem(x.TPName + " / " + x.TPCode, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillSysCodeAsync()
        private async Task FillSysCodeAsync()
        {
            var sysLookup = await _ajaxDropdownAppService.GetSysCodeByTypeAsync();
            SysCodeLookupList = sysLookup
                                         .Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                         .ToList();
        }
        #endregion

        #region Private Functions
        private async Task SaveInvoiceBillsAsync(Guid InvoiceId, IList<CreateUpdateInvoiceBillDto> InvoiceBillDto)
        {
            foreach (var InvoiceBill in InvoiceBillDto)
            {
                InvoiceBill.InvoiceId = InvoiceId;
                
                InvoiceBill.Id = CopyInvoiceId != null ? Guid.Empty : InvoiceBill.Id;
                
                if (InvoiceBill.Id == Guid.Empty)
                {
                    await _invoiceBillAppService.CreateAsync(InvoiceBill);
                }
                else
                {
                    await _invoiceBillAppService.UpdateAsync(InvoiceBill.Id, InvoiceBill);
                }
            }
        }

        private void GetInvoiceNo(int InvoiceType)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int ai = rnd.Next(20230000);
            var s = ai.ToString("00000000");
            
            switch (InvoiceType)
            {
                default:
                    InvoiceDto.InvoiceNo = "AR" + s;
                    break;
                case 1:
                    InvoiceDto.InvoiceNo = "DC" + s;
                    break;
                case 2:
                    InvoiceDto.InvoiceNo = "AP" + s;
                    break;
                case 3:
                    InvoiceDto.InvoiceNo = "AP" + s;
                    break;
                case 4:
                    InvoiceDto.InvoiceNo = "DC" + s;
                    break;
            }
        }

        private async Task SaveMemoSourceId(Guid InvoiceId, int InvoiceType)
        {
            if (InvoiceType == 0) FType = FreightPageType.AR;
            if (InvoiceType == 1) FType = FreightPageType.DC;
            if (InvoiceType == 2) FType = FreightPageType.AP;

            var Memos = await _memosAppService.GetListByQueryAsync(new QueryListDto() { SourceId = Guid.Empty, FType = FType });

            if (Memos is not null && Memos.Count > 0) await _memosAppService.UpdateSourceIdAsync(InvoiceId, FType);
        }
        #endregion
    }
}
