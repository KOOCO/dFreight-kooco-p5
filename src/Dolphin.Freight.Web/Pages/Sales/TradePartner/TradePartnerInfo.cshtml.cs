using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Microsoft.Extensions.Logging.Abstractions;
using Dolphin.Freight.TradePartners;
using System.Collections.Generic;
using System;
using Dolphin.Freight.TradePartner;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dolphin.Freight.TradePartners.Credits;
using System.Linq;
using Dolphin.Freight.TradePartners.TradeParties;
using Volo.Abp.Data;

namespace Dolphin.Freight.Web.Pages.Sales.TradePartner
{
    public class TradePartnerInfoModel : AbpPageModel
    {
        public ILogger<TradePartnerInfoModel> Logger { get; set; }

        private readonly ITradePartnerAppService _tradePartnerAppService;

        private readonly ICreditLimitGroupAppService _creditLimitGroupAppService;

        private readonly ICreditLimitGroupRepository _creditLimitGroupRepository;
        private readonly IAccountGroupAppService _accountGroupAppService;
        private readonly ITradePartyAppService _tradePartyAppService;


        [BindProperty]
        public CreateTradePartnerInfoViewModel TPInfoModel { get; set; }

        public List<SelectListItem> CreditLimitGroupNameLookupList { get; set; }
        public List<SelectListItem> CountryLookupList { get; set; }
        public List<SelectListItem> AccountGroupNameLookupList { get; set; }
        public List<SelectListItem> CurrencyLookupList { get; set; }

        [BindProperty]
        public List<CreateUpdateTradePartyDto> TradeParties { get; set; }

        public TradePartnerInfoModel(ITradePartnerAppService tradePartnerAppService,
            ICreditLimitGroupRepository creditLimitGroupRepository,
            ICreditLimitGroupAppService creditLimitGroupAppService,
            IAccountGroupAppService accountGroupAppService,
            ITradePartyAppService tradePartyAppService
            )
        {
            Logger = NullLogger<TradePartnerInfoModel>.Instance;
            _tradePartnerAppService = tradePartnerAppService;
            _creditLimitGroupRepository = creditLimitGroupRepository;
            _creditLimitGroupAppService = creditLimitGroupAppService;
            _accountGroupAppService = accountGroupAppService;
        }

        public async Task OnGetAsync()
        {
            TPInfoModel = new CreateTradePartnerInfoViewModel();
            var creditLimitGroupNameLookup = await _creditLimitGroupAppService.GetCreditLimitGroupNameLookupAsync();
            CreditLimitGroupNameLookupList = creditLimitGroupNameLookup.Items
                                                .Select(x => new SelectListItem(x.CreditLimitGroupName, x.Id.ToString(), false))
                                                .ToList();
            var countryNameLookup = await _tradePartnerAppService.GetCountriesLookupAsync();
            CountryLookupList = countryNameLookup.Items
                                                .Select(x => new SelectListItem(x.ShowName + " " + x.CountryName, x.Id.ToString(), false))
                                                .ToList();
            var accountGroupNameLookup = await _accountGroupAppService.GetAccountGroupNameLookupAsync();
            AccountGroupNameLookupList = accountGroupNameLookup.Items
                                                .Select(x => new SelectListItem(x.AccountGroupName, x.Id.ToString(), false))
                                                .ToList<SelectListItem>();
            var currencyNameLookup = await _tradePartnerAppService.GetCurrenciesLookupAsync();
            CurrencyLookupList = currencyNameLookup.Items
                                                .Select(x => new SelectListItem(x.CurrencyName, x.Id.ToString(), false))
                                                .ToList();

            TradeParties = new List<CreateUpdateTradePartyDto>();
        }

        #region OnPostAsync()
        public async Task<IActionResult> OnPostAsync()
        {
            //Logger.LogDebug("1_Enter into TradePartnerInfo OnPostAsync PrintAddress:" + TPInfoModel.TPPrintAddress);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(TradeParties != null && TradeParties.Any())
            {
                foreach (var tradeParty in TradeParties)
                {
                    if (tradeParty.ExtraProperties == null)
                    {
                        tradeParty.ExtraProperties = new();
                    }

                    if(tradeParty.TradePartyListDto != null)
                    {
                        tradeParty.ExtraProperties.Add("TradePartyList", tradeParty.TradePartyListDto);
                    }

                    await _tradePartyAppService.SaveAsync(tradeParty);
                }
            }

            // 將PopUpTips組成Json字串
            PopUpTipsObj popUpTipsObj = new PopUpTipsObj(
                TPInfoModel.DoorToDoor,
                TPInfoModel.BadCustomer,
                TPInfoModel.ImportOnly,
                TPInfoModel.ExportOnly,
                TPInfoModel.CoLoader,
                TPInfoModel.CustomClear,
                TPInfoModel.Warehouse,
                TPInfoModel.ISFCharges,
                TPInfoModel.FreeHandCargo,
                TPInfoModel.Nomination,
                TPInfoModel.SeeMemoRemark
                );

            string jsonString = System.Text.Json.JsonSerializer.Serialize(popUpTipsObj);
           // Logger.LogDebug("jsonString:" + jsonString);
            TPInfoModel.PopUpTips = jsonString;

            var inputDto = await _tradePartnerAppService.CreateTPAsync(
                ObjectMapper.Map<CreateTradePartnerInfoViewModel, CreateUpdateTradePartnerDto>(TPInfoModel)
                );
            // Logger.LogDebug("inputDtoId:" + inputDto.Id);
            //var result = new CreatedAtActionResult("Post", "", new { Id = inputDto.Id }, inputDto);
            //return result;
            //return RedirectToPage("/Sales/TradePartner/EditTradePartnerInfo", new {id = inputDto.Id});
            return new ObjectResult(new { id = inputDto.Id });
        }
        #endregion

        public async Task<IActionResult> OnGetSearch(string filter) {
            var result = await _tradePartnerAppService.SearchTradePartnersLookupAsync(filter);
            return new JsonResult(result);
        }

        public class CreateTradePartnerInfoViewModel
        {
            public Guid Id { get; set; }
            [Required]
            public TPType? TPType { get; set; }
            public string TPCode { get; set; }
            [Required]
            public string TPName { get; set; }
            [Required]
            public string TPNamePrint { get; set; }
            public string TPNameLocal { get; set; }

            [TextArea]
            public string TPLocalAddress { get; set; }
            public string CityCode { get; set; } // 城市代碼
            public string StateCode { get; set; }
            public string PostCode { get; set; }
            [Required]
            [SelectItems(nameof(CountryLookupList))]
            public string CountryCode { get; set; }


            public string Telephone { get; set; }
            public string Fax { get; set; }
            public string Website { get; set; }
            [TextArea]
            public string TPPrintAddress { get; set; }
            [TextArea]
            public string Remark { get; set; }


            // Other Setting
            public bool IsActive { get; set; }


            public string TPAliasName { get; set; }

            [SelectItems(nameof(AccountGroupNameLookupList))]
            public String AccountGroupId { get; set; }
            public string Ceo { get; set; }

            [SelectItems(nameof(CreditLimitGroupNameLookupList))]
            public String CreditLimitGroupId { get; set; }

            public string CorpNo { get; set; }
            public string IataCode { get; set; }
            [StringLength(5, ErrorMessage = "Length must less than {0} characters")]
            public string IataPrefix { get; set; }
            public string ScacCode { get; set; }
            public string FirmsCode { get; set; } // 美國FIRMS編號
            public string CbsaCode { get; set; }
            public string DutyPaymentType { get; set; }
            public string OpenHours { get; set; }
            public BussinessStatusType BussinessStatusType { get; set; }
            public string CsCode { get; set; }
            public string SalesOfficeCode { get; set; }
            public string SalesCode { get; set; }
            public string SalesCodeOe { get; set; }
            public string SalesCodeOi { get; set; }
            public string SalesCodeAe { get; set; }
            public string SalesCodeAi { get; set; }
            public string SalesCodeCuOe { get; set; }
            public string SalesCodeCuAe { get; set; }
            public string SalesCodeCuOi { get; set; }
            public string SalesCodeCuAi { get; set; }

            // Accounting Setting
            [TextArea]
            public string AccountAddress { get; set; }
            public string TaxId { get; set; }
            public bool TrackPayment { get; set; }
            public PaymentType PaymentType { get; set; }
            public CreditTermType CreditTermType { get; set; }
            [DisplayName("")]
            public int CreditTermDays { get; set; }
            public int CreditLimit { get; set; }
            //public string BillToAgentCode { get; set; }
            public bool BillToAgentCode { get; set; }
            public bool Clm { get; set; }
            public string AccountName { get; set; }
            public string BankName { get; set; }
            public string AccountNo { get; set; }
            [SelectItems(nameof(CurrencyLookupList))]
            public string AccountCurrencyCode { get; set; }
            public string BankName2 { get; set; }
            public string AccountNo2 { get; set; }
            [SelectItems(nameof(CurrencyLookupList))]
            public string AccountCurrencyCode2 { get; set; }
            public string BankName3 { get; set; }
            public string AccountNo3 { get; set; }
            [SelectItems(nameof(CurrencyLookupList))]
            public string AccountCurrencyCode3 { get; set; }
            // ISF Setting
            public string IsfSubmissionName { get; set; }
            public ImporterCodeType ImporterCodeType { get; set; }
            public string ImporterNo { get; set; }
            // Pop-up Tips
            public bool DoorToDoor { get; set; }
            public bool BadCustomer { get; set; }
            public bool ImportOnly { get; set; }
            public bool ExportOnly { get; set; }
            public bool CoLoader { get; set; }
            public bool CustomClear { get; set; }
            public bool Warehouse { get; set; }
            public bool ISFCharges { get; set; }
            public bool FreeHandCargo { get; set; }
            public bool Nomination { get; set; }
            public bool SeeMemoRemark { get; set; }

            public string PopUpTips { get; set; }


        }
    }
}
