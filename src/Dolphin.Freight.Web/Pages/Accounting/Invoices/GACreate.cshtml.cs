using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Settings.CurrencySetting;
using Dolphin.Freight.Settinngs.Ports;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Dolphin.Freight.Web.Pages.Accounting.Invoices
{
    public class GACreateModel : FreightPageModel
    {
        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public int InvoiceType { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid InvoiceId { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }
        [BindProperty]
        public IList<CreateUpdateInvoiceBillDto> InvoiceBillDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public CreateUpdateInvoiceDto InvoiceDto { get; set; }
        public InvoiceBasicDto InvoiceBasicDto { get; set; }
        [BindProperty]
        public InvoiceMblDto InvoiceMblDto { get; set; }
        public Dictionary<string, float> DictCurrencyConversion { get; set; }
        public string CurrencyConversionJSON { get; set; }

        public string backUrl { get; set; }
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IInvoiceBillAppService _invoiceBillAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IPortAppService _portAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly ICurrencySettingAppService _currencySettingAppService;
        public GACreateModel(ITradePartnerAppService tradePartnerAppService,
                            ISubstationAppService substationAppService,
                            IInvoiceAppService invoiceAppService,
                            IInvoiceBillAppService invoiceBillAppService,
                            IPortAppService portAppService,
                            ISysCodeAppService sysCodeAppService,
                            ICurrencySettingAppService currencySettingAppService
                            )
        {
            _invoiceAppService = invoiceAppService;
            _invoiceBillAppService = invoiceBillAppService;
            _substationAppService = substationAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _portAppService = portAppService;
            _sysCodeAppService = sysCodeAppService;
            _currencySettingAppService = currencySettingAppService;
        }
        public async Task OnGetAsync()
        {
            InvoiceBasicDto = new InvoiceBasicDto();

            if (InvoiceId != Guid.Empty || Id != null)
            {
                var invoice = await _invoiceAppService.GetAsync(InvoiceId);
                InvoiceDto = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
            }

            if (InvoiceMblDto != null)
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

            await FillCurrencySettingsDictionary();
        }

        public async Task<JsonResult> OnPostAsync()
        {
            if (InvoiceDto.Id == Guid.Empty)
            {
                if (InvoiceType == 3)
                {
                    if (InvoiceDto.InvoiceNo == null)
                    {
                        Random rnd = new Random(Guid.NewGuid().GetHashCode());
                        int ai = rnd.Next(20230000);
                        var s = ai.ToString("00000000");
                        InvoiceDto.InvoiceNo = "AP" + s;
                    }
                    InvoiceDto.InvoiceType = InvoiceType;
                }
                else {
                    if (InvoiceDto.InvoiceNo == null)
                    {
                        Random rnd = new Random(Guid.NewGuid().GetHashCode());
                        int ai = rnd.Next(20230000);
                        var s = ai.ToString("00000000");
                        InvoiceDto.InvoiceNo = s;
                    }
                    InvoiceDto.InvoiceType = 4;
                }
                var invoice = await _invoiceAppService.CreateAsync(InvoiceDto);
                InvoiceDto.Id = invoice.Id;
            } else if (InvoiceDto.Id != Guid.Empty)
            {
                InvoiceDto.InvoiceType = InvoiceType;
                await _invoiceAppService.UpdateAsync(InvoiceDto.Id, InvoiceDto);
            }

            if (InvoiceBillDtos != null && InvoiceBillDtos.Count > 0)
            {
                foreach (var dto in InvoiceBillDtos)
                {
                    dto.InvoiceId = InvoiceDto.Id;
                    if (dto.Id != Guid.Empty)
                    {
                        await _invoiceBillAppService.UpdateAsync(dto.Id, dto);
                    } else
                    {
                        await _invoiceBillAppService.CreateAsync(dto);
                    }
                }
            }
            Dictionary<string, object> rs = new Dictionary<string, object>();
            rs.Add("a1", "ejo");
            return new JsonResult(rs);
        }

        public async Task FillCurrencySettingsDictionary()
        {
            var currencySettings = await _currencySettingAppService.GetCurrenciesAsync();

            DictCurrencyConversion = new Dictionary<string, float>();
            foreach (var currencySetting in currencySettings)
            {
                DictCurrencyConversion.Add($"{currencySetting.StartingCurrency}-{currencySetting.EndCurrency}", currencySetting.ExChangeRate);
            }

            CurrencyConversionJSON = JsonConvert.SerializeObject(DictCurrencyConversion);
        }
    }
}
