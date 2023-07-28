using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Settings.Countries;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dolphin.Freight.Web.Pages.Sales.TradePartner
{
    public class ModalWithCreateTradePartyMemoModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid TradePartnerId { get; set; }

        public ILogger<ModalWithCreateTradePartyMemoModel> Logger { get; set; }

        [BindProperty]
        public CreateUpdateTradePartnerMemoDto CreateUpdateTradePartnerMemoDto { get; set; }

        private readonly ITradePartnerMemoAppService _tradePartnerMemoAppService;

        public ModalWithCreateTradePartyMemoModel(ITradePartnerMemoAppService tradePartnerMemoAppService)
        {
            _tradePartnerMemoAppService = tradePartnerMemoAppService;
            Logger = NullLogger<ModalWithCreateTradePartyMemoModel>.Instance;
            CreateUpdateTradePartnerMemoDto = new CreateUpdateTradePartnerMemoDto();
        }
        
        public async Task OnGetAsync()
        {
            if (Id != null)
            {
                TradePartnerMemoDto dto = await _tradePartnerMemoAppService.GetAsync(Id.Value);
                CreateUpdateTradePartnerMemoDto.Title = dto.Title;
                CreateUpdateTradePartnerMemoDto.Memo = dto.Memo;
            }
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            await _tradePartnerMemoAppService.SaveAsync(CreateUpdateTradePartnerMemoDto);
            return NoContent();
        }
    }
}
