using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.AccountingSettings.BillingCodes;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settings.CurrencySetting;
using Dolphin.Freight.Settings.Substations;
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
using QueryInvoiceDto = Dolphin.Freight.Accounting.Invoices.QueryInvoiceDto;

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
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly ICurrencySettingAppService _currencySettingAppService;
        public InvoiceModel(ITradePartnerAppService tradePartnerAppService,
                            ISubstationAppService substationAppService,
                            IInvoiceAppService invoiceAppService, 
                            IOceanExportMblAppService oceanExportMblAppService, 
                            IOceanExportHblAppService oceanExportHblAppService, 
                            IInvoiceBillAppService invoiceBillAppService, 
                            IPortAppService portAppService, 
                            OceanImportHblAppService oceanImportHblAppService,
                            OceanImportMblAppService oceanImportMblAppService,
                            IAjaxDropdownAppService ajaxDropdownAppService,
                            ICurrencySettingAppService currencySettingAppService
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
        }
        public async Task OnGetAsync()
        {
            InvoiceBasicDto = new InvoiceBasicDto();

            // 0：海運進口、1：海運出口、2：空運進口、3：空運出口、4：SO清單、5：船期
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
                default:
                    await InitOceanExport();
                    break;
            }
            if (InvoiceId != null)
            {
                var invoice = await _invoiceAppService.GetAsync(InvoiceId.Value);
                InvoiceDto = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
            }

            if (InvoiceMblDto != null ) 
            {
                if (InvoiceMblDto.OfficeId != Guid.Empty) 
                {
                    var substation = await _substationAppService.GetAsync(InvoiceMblDto.OfficeId);
                    InvoiceBasicDto.AbbreviationName = substation.AbbreviationName;
                }
                if (InvoiceMblDto.MblConsigneeId != null && InvoiceMblDto.MblConsigneeId.Value != Guid.Empty) 
                {
                    var tradePartner = await _tradePartnerAppService.GetAsync(InvoiceMblDto.MblConsigneeId.Value);
                    InvoiceBasicDto.MblConsigneeName = tradePartner.TPName;
                }
                if (InvoiceMblDto.MblNotifyId != null && InvoiceMblDto.MblNotifyId.Value != Guid.Empty)
                {
                    var tradePartner = await _tradePartnerAppService.GetAsync(InvoiceMblDto.MblNotifyId.Value);
                    InvoiceBasicDto.MblNotifyName = tradePartner.TPName;
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
                //如果沒有Mid，表示這是Hbl的
                var oceanExportHbl = await _oceanExportHblAppService.GetAsync(Hid.Value);
                if (oceanExportHbl != null)
                {
                    query.ParentId = Hid;
                    query.QueryType = 1;
                }
            }
            else
            {
                //如果有Mid，表示這是Mbl的
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
            var createUpdateOceanImportMblDto = await _oceanImportMblAppService.GetCreateUpdateOceanImportMblDtoById(Mid.Value);

            InvoiceMblDto = ObjectMapper.Map<CreateUpdateOceanImportMblDto, InvoiceMblDto>(createUpdateOceanImportMblDto);

            backUrl = "/OceanImports/EditModal3?Id=" + Mid;
        }
        private async Task InitAirImport()
        {
            QueryInvoiceDto query = new QueryInvoiceDto() { QueryInvoiceType = InvoiceType };
            var oceanExportMbl = new OceanExportMblDto();
            InvoiceMblDto = ObjectMapper.Map<OceanExportMblDto, InvoiceMblDto>(oceanExportMbl);

            backUrl = "/AirImports/EditModal3?Id=" + Mid;
        }
        private async Task InitAirExport()
        {
            QueryInvoiceDto query = new QueryInvoiceDto() { QueryInvoiceType = InvoiceType };
            var oceanExportMbl = new OceanExportMblDto();
            InvoiceMblDto = ObjectMapper.Map<OceanExportMblDto, InvoiceMblDto>(oceanExportMbl);

            backUrl = "/AirExports/EditModal3?Id=" + Mid;
        }
        private async Task InitExportBooking() 
        {
            QueryInvoiceDto query = new QueryInvoiceDto() { QueryInvoiceType = InvoiceType };
            var oceanExportMbl = new OceanExportMblDto();
            InvoiceMblDto = ObjectMapper.Map<OceanExportMblDto, InvoiceMblDto>(oceanExportMbl);

            backUrl = "/OceanExports/ExportBookings/Edit2?Id=" + Bid;
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
            if (InvoiceDto.InvoiceNo == null) 
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

            var invoice = new InvoiceDto();

            if (InvoiceDto.Id == Guid.Empty)
            {
                invoice = await _invoiceAppService.CreateAsync(InvoiceDto);
            }
            else
            {
                invoice = await _invoiceAppService.UpdateAsync(InvoiceDto.Id, InvoiceDto);
            }
            InvoiceDto.Id = invoice.Id;

            if (InvoiceBillDtos != null && InvoiceBillDtos.Count > 0) 
            {
                foreach (var dto in InvoiceBillDtos) 
                { 
                    dto.InvoiceId = invoice.Id;
                    if(dto.Id == Guid.Empty) 
                    {
                        await _invoiceBillAppService.CreateAsync(dto);
                    }
                    else
                    {
                        await _invoiceBillAppService.UpdateAsync(dto.Id, dto);
                    }
                }
            }
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
    }
}
