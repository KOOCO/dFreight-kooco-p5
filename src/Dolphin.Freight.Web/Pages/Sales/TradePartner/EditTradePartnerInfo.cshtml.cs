using Dolphin.Freight.TradePartner;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.TradePartners.Credits;
using Dolphin.Freight.Web.Pages.Sales.TradePartner.Credit;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Dolphin.Freight.TradePartners.TradeParties;
using Microsoft.AspNetCore.Components.Forms;
using static Dolphin.Freight.Web.Pages.Sales.TradePartner.TradePartnerInfoModel;

namespace Dolphin.Freight.Web.Pages.Sales.TradePartner
{
    public class EditTradePartnerInfoModel : AbpPageModel
    {
        public ILogger<EditTradePartnerInfoModel> Logger { get; set; }

        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ITradePartnerMemoAppService _tradePartnerMemoAppService;
        private readonly IAccountGroupAppService _accountGroupAppService;
        private readonly ICreditLimitGroupAppService _creditLimitGroupAppService;
        private readonly ITradePartyAppService _tradePartyAppService;
        private readonly IContactPersonAppService _contactPersonAppService;

        [BindProperty]
        public CreateTradePartnerInfoViewModel TPInfoModel { get; set; }

        public List<SelectListItem> CreditLimitGroupNameLookupList { get; set; }
        public List<SelectListItem> CountryLookupList { get; set; }
        public List<SelectListItem> AccountGroupNameLookupList { get; set; }
        public List<SelectListItem> CurrencyLookupList { get; set; }

        public bool HasHighlight { get; set; }

        public Guid Id { get; set; }

        [BindProperty]
        public List<CreateUpdateTradePartyDto> TradeParties { get; set; }

        [BindProperty]
        public List<CreateUpdateContactPersonDto> ContactPersonModel { get; set; }

        [BindProperty]
        public List<CreateContactInfoChildren> ContactPersonChildren { get; set; }

        public EditTradePartnerInfoModel(ITradePartnerAppService tradePartnerAppService, ITradePartnerMemoAppService tradePartnerMemoAppService,
            IAccountGroupAppService accountGroupAppService, ICreditLimitGroupAppService creditLimitGroupAppService,
            ITradePartyAppService tradePartyAppService, IContactPersonAppService contactPersonAppService)
        {
            Logger = NullLogger<EditTradePartnerInfoModel>.Instance;
            _tradePartnerAppService = tradePartnerAppService;
            _tradePartnerMemoAppService = tradePartnerMemoAppService;
            _accountGroupAppService = accountGroupAppService;
            _creditLimitGroupAppService = creditLimitGroupAppService;
            _tradePartyAppService = tradePartyAppService;
            _contactPersonAppService = contactPersonAppService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            TradePartnerDto tradePartnerDto = await _tradePartnerAppService.GetTPAsync(id);
            Id = id;

            if (tradePartnerDto == null)
            {
                return NotFound();
            }

            HasHighlight = await _tradePartnerMemoAppService.HasHighlightAsync(id);

            var creditLimitGroupNameLookup = await _creditLimitGroupAppService.GetCreditLimitGroupNameLookupAsync();
            CreditLimitGroupNameLookupList = creditLimitGroupNameLookup.Items
                                                .Select(x => new SelectListItem(x.CreditLimitGroupName, x.Id.ToString(), false))
                                                .ToList();
            //if (tradePartnerDto.CreditLimitGroupId != Guid.Empty) {
            //    CreditLimitGroupNameLookupList.Where(q => q.Value == tradePartnerDto.CreditLimitGroupId.ToString()).First().Selected = true;
            //}
            var countryNameLookup = await _tradePartnerAppService.GetCountriesLookupAsync();
            CountryLookupList = countryNameLookup.Items
                                                .Select(x => new SelectListItem(x.ShowName + " " + x.CountryName, x.Id.ToString(), false))
                                                .ToList();
            var accountGroupNameLookup = await _accountGroupAppService.GetAccountGroupNameLookupAsync();
            AccountGroupNameLookupList = accountGroupNameLookup.Items
                                                .Select(x => new SelectListItem(x.AccountGroupName, x.Id.ToString(), false))
                                                .ToList<SelectListItem>();
            //if (tradePartnerDto.AccountGroupId != Guid.Empty) {
            //    AccountGroupNameLookupList.Where(q => q.Value == tradePartnerDto.AccountGroupId.ToString()).First().Selected = true;
            //}
            var currencyNameLookup = await _tradePartnerAppService.GetCurrenciesLookupAsync();
            CurrencyLookupList = currencyNameLookup.Items
                                                .Select(x => new SelectListItem(x.CurrencyName, x.Id.ToString(), false))
                                                .ToList();

            TPInfoModel = ObjectMapper.Map<TradePartnerDto, CreateTradePartnerInfoViewModel>(tradePartnerDto);
            Logger.LogDebug("PopUpTips:" + TPInfoModel.PopUpTips);
            PopUpTipsObj popObj = JsonSerializer.Deserialize<PopUpTipsObj>(TPInfoModel.PopUpTips);

            TPInfoModel.DoorToDoor = popObj.DoorToDoor;
            TPInfoModel.BadCustomer = popObj.BadCustomer;
            TPInfoModel.ImportOnly = popObj.ImportOnly;
            TPInfoModel.ExportOnly = popObj.ExportOnly;
            TPInfoModel.CoLoader = popObj.CoLoader;
            TPInfoModel.CustomClear = popObj.CustomClear;
            TPInfoModel.Warehouse = popObj.Warehouse;
            TPInfoModel.ISFCharges = popObj.ISFCharges;
            TPInfoModel.FreeHandCargo = popObj.FreeHandCargo;
            TPInfoModel.Nomination = popObj.Nomination;
            TPInfoModel.SeeMemoRemark = popObj.SeeMemoRemark;


            TradeParties = await _tradePartyAppService.GetListByTradePartnerId(id);

            ContactPersonModel = await _contactPersonAppService.GetListByTradePartnerId(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
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
            Logger.LogDebug("jsonString:" + jsonString);

            TPInfoModel.PopUpTips = jsonString;
            try
            {
                await _tradePartnerAppService.UpdateTPAsync(
                TPInfoModel.Id,
                ObjectMapper.Map<CreateTradePartnerInfoViewModel, CreateUpdateTradePartnerDto>(TPInfoModel)
                );

                var currentTradeParties = await _tradePartyAppService.GetListByTradePartnerId(TPInfoModel.Id);
                //var deleted =  currentTradeParties.ExceptBy(TradeParties, x => x);
                var deleted =  currentTradeParties.Where(x => !TradeParties.Any(y=>y.Id == x.Id)).ToList();

                if (TradeParties != null && TradeParties.Any())
                {
                    foreach (var tradeParty in TradeParties)
                    {
                        if (tradeParty.ExtraProperties == null)
                        {
                            tradeParty.ExtraProperties = new();
                        }

                        if (tradeParty.TradePartyListDto != null)
                        {
                            tradeParty.ExtraProperties.Remove("TradePartyList");
                            tradeParty.ExtraProperties.Add("TradePartyList", tradeParty.TradePartyListDto);
                        }

                        tradeParty.TradePartnerId = TPInfoModel.Id;
                        if(tradeParty.Id != Guid.Empty)
                        {
                            await _tradePartyAppService.UpdateAsync(tradeParty.Id, tradeParty);
                        }
                        else
                        {
                            await _tradePartyAppService.CreateAsync(tradeParty);
                        }

                        
                    }
                }

                foreach (var item in deleted)
                {
                    await _tradePartyAppService.DeleteAsync(item.Id);
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new AbpDbConcurrencyException(ex.Message, ex);
            }

            return NoContent();

        }

        public class CreateTradePartnerInfoViewModel
        {
            [HiddenInput]
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
            public string CityCode { get; set; } // �����N�X
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
            public Guid AccountGroupId { get; set; }
            public string Ceo { get; set; }

            [SelectItems(nameof(CreditLimitGroupNameLookupList))]
            //public string CreditLimitGroupName { get; set; }
            public Guid CreditLimitGroupId { get; set; }

            public string CorpNo { get; set; }
            public string IataCode { get; set; }
            [StringLength(5, ErrorMessage = "Length must less than {0} characters")]
            public string IataPrefix { get; set; }
            public string ScacCode { get; set; }
            public string FirmsCode { get; set; } // ����FIRMS�s��
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
