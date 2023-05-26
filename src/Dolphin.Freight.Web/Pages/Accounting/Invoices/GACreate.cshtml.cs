using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Settinngs.Ports;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
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
        public Guid? InvoiceId { get; set; }
        [BindProperty]
        public IList<CreateUpdateInvoiceBillDto> InvoiceBillDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public CreateUpdateInvoiceDto InvoiceDto { get; set; }
        public InvoiceBasicDto InvoiceBasicDto { get; set; }
        [BindProperty]
        public InvoiceMblDto InvoiceMblDto { get; set; }

        public string backUrl { get; set; }
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IInvoiceBillAppService _invoiceBillAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IPortAppService _portAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        public GACreateModel(ITradePartnerAppService tradePartnerAppService,
                            ISubstationAppService substationAppService,
                            IInvoiceAppService invoiceAppService,
                            IInvoiceBillAppService invoiceBillAppService,
                            IPortAppService portAppService,
                            ISysCodeAppService sysCodeAppService
                            )
        {
            _invoiceAppService = invoiceAppService;
            _invoiceBillAppService = invoiceBillAppService;
            _substationAppService = substationAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _portAppService = portAppService;
            _sysCodeAppService = sysCodeAppService;
        }
        public async Task OnGetAsync()
        {
            InvoiceBasicDto = new InvoiceBasicDto();

            if (InvoiceId != null)
            {
                var invoice = await _invoiceAppService.GetAsync(InvoiceId.Value);
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
        }
        
        public async Task<JsonResult> OnPostAsync()
        {
            
            if (InvoiceDto.InvoiceNo == null)
            {
                if (InvoiceType == 3)
                {
                    InvoiceDto.InvoiceType = InvoiceType;
                    InvoiceDto.InvoiceNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "In_InvoiceNo" });
                }
                else {
                    InvoiceDto.InvoiceType = 4;
                    InvoiceDto.InvoiceNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "OUT_InvoiceNo" });
                }
            }
            var invoice = await _invoiceAppService.CreateAsync(InvoiceDto);
            InvoiceDto.Id = invoice.Id;
            if (InvoiceBillDtos != null && InvoiceBillDtos.Count > 0)
            {
                foreach (var dto in InvoiceBillDtos)
                {
                    dto.InvoiceId = invoice.Id;
                    await _invoiceBillAppService.CreateAsync(dto);
                }
            }
            Dictionary<string, object> rs = new Dictionary<string, object>();
            rs.Add("a1", "ejo");
            return new JsonResult(rs);
        }
    }
}
