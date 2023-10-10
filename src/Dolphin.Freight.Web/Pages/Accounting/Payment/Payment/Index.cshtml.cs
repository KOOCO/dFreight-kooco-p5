using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dolphin.Freight.Accounting.Payment;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Dolphin.Freight.AccountingSettings.CurrencyTables;
using Dolphin.Freight.Accounting.Inv;
using Newtonsoft.Json;
using Dolphin.Freight.ImportExport.Common;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.SysCodes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Volo.Abp.Domain.Repositories;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Common;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using Volo.Abp;

namespace Dolphin.Freight.Web.Pages.Payment
{
    public class IndexModel : AbpPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty]
        public CreateUpdatePaymentDto Payment { get; set; }
        public PaymentDto PaymentDto { get; set; }

        public QueryCurrencyTableDto QueryCurrency { get; set; }
        public List<SelectListItem> PLList { get; set; }
        public List<SelectListItem> SubstationList { get; set; }
        public List<SelectListItem> PTList { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> BankList { get; set; }
        public List<SelectListItem> CurrencyList { get; set; }

        private readonly IPaymentAppService _paymentAppService;
        private readonly ICurrencyTableAppService _currencyAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly IInvAppService _invAppService;

        public IndexModel(IPaymentAppService paymentAppService , ICurrencyTableAppService currencyAppService, ISysCodeAppService sysCodeRepository, ISubstationAppService substationAppService, IAjaxDropdownAppService ajaxDropdownAppService, IInvAppService invAppService)
        {
            _paymentAppService = paymentAppService;
            _currencyAppService = currencyAppService;
            _sysCodeAppService = sysCodeRepository;
            _substationAppService = substationAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _invAppService = invAppService;
        }
        public async Task<IActionResult> OnGetAsync(Guid id,string edit)
        {
            //id = Guid.Parse("9A2B557D-1E43-6504-DECE-3A09461EDB60");
            Payment = new CreateUpdatePaymentDto();
            QueryCurrency = new QueryCurrencyTableDto();

            if (id.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                PaymentDto = await _paymentAppService.GetDataAsync(id);
                Payment.Id = id.ToString();
                Payment.PaymentId = PaymentDto.PaymentId;
                Payment.PaymentLevel = PaymentDto.PaymentLevel;
                Payment.PaidTo = PaymentDto.PaidTo;
                Payment.ShowPartyOnCheck = PaymentDto.ShowPartyOnCheck;
                Payment.ReleaseDate = PaymentDto.ReleaseDate;
                Payment.Category = PaymentDto.Category;
                Payment.CheckNo = PaymentDto.CheckNo;
                Payment.Clear = PaymentDto.Clear;
                Payment.ClearChk = PaymentDto.Clear == null ? false : true;
                Payment.Invalid = PaymentDto.Invalid;
                Payment.InvalidChk = PaymentDto.Invalid == null ? false : true;
                Payment.OfficeId = PaymentDto.OfficeId;
                Payment.Bank = PaymentDto.Bank;
                Payment.BankCurrency = PaymentDto.BankCurrency;
                Payment.U2T = PaymentDto.U2T;
                Payment.R2T = PaymentDto.R2T;
                Payment.H2T = PaymentDto.H2T;
                Payment.Memo = PaymentDto.Memo;

                Payment.GU = PaymentDto.PaymentId;
                Payment.Edit = edit;
                ViewData["PId"] = PaymentDto.PaymentId;
            }
            else
            {   Payment.ReleaseDate = DateTime.Now.Date;
                QueryCurrency.Ccy1Id = "19B90321-C852-451D-A1C0-5FA47373ED55";
                QueryCurrency.Ccy2Id = "9D571C85-3C78-41B1-A098-BBB22E8D159B";
                Payment.U2T = await _currencyAppService.QueryRateInternalAsync(QueryCurrency);
                QueryCurrency.Ccy1Id = "3676C643-B105-4654-9492-332607E7C195";
                Payment.R2T = await _currencyAppService.QueryRateInternalAsync(QueryCurrency);
                QueryCurrency.Ccy1Id = "7A780112-9D67-4BFE-AC8C-C494AB47FEF4";
                Payment.H2T = await _currencyAppService.QueryRateInternalAsync(QueryCurrency);

                Payment.GU = Guid.NewGuid();
            }                       

            #region 取收付款級別
            QueryDto queryDto = new QueryDto();
            queryDto.QueryType = "PaymentLevel_1";
            var sysCodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(queryDto);
            var rs = sysCodes.Where(x => x.CodeType.Equals("PaymentLevel_1")).ToList();
            List<SysCodeDto> list = new List<SysCodeDto>();
            list = rs;

            if (list.Count > 0 )
            { 
                PLList = new List<SelectListItem>();
                if (id.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    foreach (var pl in list.DistinctBy(x=>x.CodeValue))
                    {
                        if (pl.CodeValue == "1")
                        {
                            PLList.Add(new SelectListItem() { Text = pl.ShowName, Value = pl.CodeValue, Selected = true });
                        }
                        else
                        {
                            PLList.Add(new SelectListItem() { Text = pl.ShowName, Value = pl.CodeValue });
                        }
                    }
                }
                else 
                {
                    foreach (var pl in list.DistinctBy(x => x.CodeValue))
                    {
                        if (pl.CodeValue == PaymentDto.PaymentLevel)
                        {
                            PLList.Add(new SelectListItem() { Text = pl.ShowName, Value = pl.CodeValue, Selected = true });
                        }
                        else
                        {
                            PLList.Add(new SelectListItem() { Text = pl.ShowName, Value = pl.CodeValue });
                        }
                    }
                }                    
            }
            #endregion

            #region 取類別
            queryDto = new QueryDto();
            queryDto.QueryType = "Category";
            sysCodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(queryDto);
            rs = sysCodes.Where(x => x.CodeType.Equals("Category")).ToList();
            list = rs;

            if (list.Count > 0)
            {
                CategoryList = new List<SelectListItem>();
                if (id.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    foreach (var pl in list.DistinctBy(x => x.CodeValue))
                    {
                        if (pl.CodeValue == "10")
                        {
                            CategoryList.Add(new SelectListItem() { Text = pl.ShowName, Value = pl.CodeValue, Selected = true });
                        }
                        else
                        {
                            CategoryList.Add(new SelectListItem() { Text = pl.ShowName, Value = pl.CodeValue });
                        }
                    }
                }
                else 
                {
                    foreach (var pl in list.DistinctBy(x => x.CodeValue))
                    {
                        if (pl.CodeValue == PaymentDto.Category)
                        {
                            CategoryList.Add(new SelectListItem() { Text = pl.ShowName, Value = pl.CodeValue, Selected = true });
                        }
                        else
                        {
                            CategoryList.Add(new SelectListItem() { Text = pl.ShowName, Value = pl.CodeValue });
                        }
                    }
                }                    
            }
            #endregion

            #region 取分站
            queryDto = new QueryDto();
            queryDto.QueryType = "";

            var substations = await _substationAppService.GetSubstationsAsync(queryDto);
            if (substations != null)
            {
                SubstationList = new List<SelectListItem>();
                if (id.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    SubstationList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
                    foreach (var substation in substations)
                    {
                        SubstationList.Add(new SelectListItem() { Text = substation.SubstationName + "(" + substation.AbbreviationName + ")", Value = substation.Id.ToString() });
                    }
                }
                else
                {
                    SubstationList.Add(new SelectListItem() { Text = "", Value = "" });
                    foreach (var substation in substations)
                    {
                        if (substation.Id.ToString().ToUpper() == PaymentDto.OfficeId.ToString().ToUpper())
                        {
                            SubstationList.Add(new SelectListItem() { Text = substation.SubstationName + "(" + substation.AbbreviationName + ")", Value = substation.Id.ToString() ,Selected = true});
                        }
                        else 
                        { 
                            SubstationList.Add(new SelectListItem() { Text = substation.SubstationName + "(" + substation.AbbreviationName + ")", Value = substation.Id.ToString() });
                        }                        
                    }
                }
                
            }
            #endregion

            #region 取收款人
            queryDto = new QueryDto();
            queryDto.QueryType = "";
            var paidto = await _ajaxDropdownAppService.GetAllTradePartners(queryDto);
            if (paidto != null)
            {
                PTList = new List<SelectListItem>();
                if (id.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    PTList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
                    foreach (var paidtoitem in paidto)
                    {
                        PTList.Add(new SelectListItem() { Text = paidtoitem.TPName + "\r\n" + (paidtoitem.TPAliasName == null ? "null" : paidtoitem.TPAliasName) + "\r\n" + paidtoitem.TPCode , Value = paidtoitem.Id.ToString() });
                    }
                }
                else
                {
                    PTList.Add(new SelectListItem() { Text = "", Value = ""});
                    foreach (var paidtoitem in paidto)
                    {
                        if (paidtoitem.Id.ToString().ToUpper() == PaymentDto.PaidTo.ToString().ToUpper())
                        {
                            PTList.Add(new SelectListItem() { Text = paidtoitem.TPName + "\r\n" + (paidtoitem.TPAliasName == null ? "null" : paidtoitem.TPAliasName) + "\r\n" + paidtoitem.TPCode, Value = paidtoitem.Id.ToString() ,Selected = true});
                        }
                        else 
                        {
                            PTList.Add(new SelectListItem() { Text = paidtoitem.TPName + "\r\n" + (paidtoitem.TPAliasName == null ? "null" : paidtoitem.TPAliasName) + "\r\n" + paidtoitem.TPCode, Value = paidtoitem.Id.ToString() });
                        }                       
                    }
                }               
            }
            #endregion

            //之後改為讀取DB，暫時寫死
            BankList = new List<SelectListItem>
            {
               new SelectListItem { Value = "1", Text = "UOB" },
new SelectListItem { Value = "2", Text = "\u83EF\u5357\u9280\u884C-USD",Selected=true },
new SelectListItem { Value = "3", Text = "Bank3" },
new SelectListItem { Value = "4", Text = "Bank4" },
new SelectListItem { Value = "5", Text = "Bank5" },
new SelectListItem { Value = "6", Text = "Bank6" },
new SelectListItem { Value = "7", Text = "Bank7" },
new SelectListItem { Value = "8", Text = "Bank8" },
new SelectListItem { Value = "9", Text = "Bank9" },
new SelectListItem { Value = "10", Text = "Chase Credit Card Example" },
new SelectListItem { Value = "11", Text = "Bank11" },
new SelectListItem { Value = "12", Text = "Bank12" },
new SelectListItem { Value = "13", Text = "Bank13" },
new SelectListItem { Value = "14", Text = "BANK1234" },
new SelectListItem { Value = "15", Text = "Bank15" },
new SelectListItem { Value = "16", Text = "Bank16" },
new SelectListItem { Value = "17", Text = "Bank17" },
new SelectListItem { Value = "18", Text = "Bank18" },
new SelectListItem { Value = "19", Text = "Bank19" },
new SelectListItem { Value = "20", Text = "Bank20" },
new SelectListItem { Value = "21", Text = "Bank21" },
new SelectListItem { Value = "22", Text = "Credit Card Example" },
new SelectListItem { Value = "23", Text = "Petty Cash" },
new SelectListItem { Value = "24", Text = "Petty Cash" },
new SelectListItem { Value = "25", Text = "testing" },
new SelectListItem { Value = "26", Text = "OCBC" },
new SelectListItem { Value = "27", Text = "HSBC" },
new SelectListItem { Value = "28", Text = "UOB" },
new SelectListItem { Value = "29", Text = "\u96F6\u7528\u91D1" }
            };

            if (id.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                foreach (var item in BankList)
                {
                    if (item.Value == PaymentDto.Bank)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            
            return Page();
        }

        public async Task<JsonResult> OnPostAsync(string datatablelist, CreateUpdatePaymentDto payment)
        {
            PaymentDto = await _paymentAppService.CheckByPaymentIdAsync(Payment.GU);

            if (PaymentDto != null)
            {
                if (Payment.Edit != "Y")
                {
                    throw new BusinessException(FreightDomainErrorCodes.PaymentAlreadyExists);
                }
                payment.PaymentId = Payment.GU;
                PaymentDto= await _paymentAppService.UpdateAsync(Guid.Parse(payment.Id), payment);
            }
            else
            {
                payment.PaymentId = Payment.GU;
                PaymentDto= await _paymentAppService.CreateAsync(payment);
            }

            List<CreateUpdateInvDto> list = JsonConvert.DeserializeObject<List<CreateUpdateInvDto>>(datatablelist);
            await _invAppService.UpdateList(Payment.GU, list);
            Dictionary<string, Guid> rs = new Dictionary<string, Guid>
            {
                { "id", PaymentDto.Id }
            };
            return new JsonResult(rs);
        }
    }
}
