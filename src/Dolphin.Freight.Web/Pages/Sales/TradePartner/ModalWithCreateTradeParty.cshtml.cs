using Dolphin.Freight.TradePartners;
using Dolphin.Freight.TradePartners.TradeParties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using System;

namespace Dolphin.Freight.Web.Pages.Sales.TradePartner
{
    public class ModalWithCreateTradePartyModel : AbpPageModel
    {


        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public TradePartyType TradePartyType { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid TradePartnerId { get; set; }

        public Guid TargetTradePartnerId { get; set; }

        public string TradePartyDescription { get; set; }

        public bool IsDefault { get; set; }


        [BindProperty]
        public CreateUpdateTradePartyDto CreateUpdateTradePartyDto { get; set; }


        public List<TradePartnerLookupDto> TradePartnerLookupList { get; set; }

        public readonly ITradePartnerAppService _tradePartnerAppService;
        public readonly ITradePartyAppService _tradePartyAppService;
        
        public ModalWithCreateTradePartyModel(ITradePartnerAppService tradePartnerAppService, ITradePartyAppService tradePartyAppService)
        {
            _tradePartnerAppService = tradePartnerAppService;
            _tradePartyAppService = tradePartyAppService;
        }

        public async Task OnGetAsync()
        {
            if (Id != null)
            {
                TradePartyDto dto = await _tradePartyAppService.GetAsync(Id.Value);
                TradePartyType = dto.TradePartyType;
                TradePartnerId = dto.TradePartnerId;
                TargetTradePartnerId = dto.TargetTradePartnerId;
                TradePartyDescription = dto.TradePartyDescription;
                IsDefault = dto.IsDefault;
            }

            //TradePartnerLookupList = (await _tradePartnerAppService.GetTradePartnersLookupAsync()).Items.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _tradePartyAppService.SaveAsync(CreateUpdateTradePartyDto);

            return NoContent();
        }
    }
}
