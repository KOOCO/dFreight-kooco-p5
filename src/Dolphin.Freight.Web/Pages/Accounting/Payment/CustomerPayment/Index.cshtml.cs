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
using static Dolphin.Freight.Permissions.AccountingPermissions;

namespace Dolphin.Freight.Web.Pages.CustomerPayment
{
    public class IndexModel : AbpPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid CopyId { get; set; }
        [BindProperty]
        public CreateUpdateCustomerPaymentDto CustomerPayment { get; set; }
        public CustomerPaymentDto CustomerPaymentDto { get; set; }

        public QueryCurrencyTableDto QueryCurrency { get; set; }
        public List<SelectListItem> PLList { get; set; }
        public List<SelectListItem> SubstationList { get; set; }
        public List<SelectListItem> RSList { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> BankList { get; set; }
        public List<SelectListItem> CurrencyList { get; set; }

        private readonly ICustomerPaymentAppService _customerPaymentAppService;
        private readonly ICurrencyTableAppService _currencyAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly IInvAppService _invAppService;

        public IndexModel(ICustomerPaymentAppService customerPaymentAppService , ICurrencyTableAppService currencyAppService, ISysCodeAppService sysCodeRepository, ISubstationAppService substationAppService, IAjaxDropdownAppService ajaxDropdownAppService, IInvAppService invAppService)
        {
            _customerPaymentAppService = customerPaymentAppService;
            _currencyAppService = currencyAppService;
            _sysCodeAppService = sysCodeRepository;
            _substationAppService = substationAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _invAppService = invAppService;
        }
        public async Task<IActionResult> OnGetAsync(Guid id,string edit,Guid copyId)
        {
            //id = Guid.Parse("9A2B557D-1E43-6504-DECE-3A09461EDB60");
            CustomerPayment = new CreateUpdateCustomerPaymentDto();
            QueryCurrency = new QueryCurrencyTableDto();

            if (id.ToString() != "00000000-0000-0000-0000-000000000000"|| copyId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                var newId = copyId != Guid.Empty ? copyId : id;
                CustomerPaymentDto = await _customerPaymentAppService.GetDataAsync(newId);
                CustomerPayment.Id = copyId == Guid.Empty? id.ToString():null;
                CustomerPayment.PaymentId = CustomerPaymentDto.PaymentId;
                CustomerPayment.PaymentLevel = CustomerPaymentDto.PaymentLevel;
                CustomerPayment.ReceivablesSources = CustomerPaymentDto.ReceivablesSources;
                CustomerPayment.ReleaseDate = CustomerPaymentDto.ReleaseDate;
                CustomerPayment.Category = CustomerPaymentDto.Category;
                CustomerPayment.CheckNo = CustomerPaymentDto.CheckNo;
                CustomerPayment.Deposit = CustomerPaymentDto.Deposit;
                CustomerPayment.DepositChk = CustomerPaymentDto.Deposit == null ? false : true;
                CustomerPayment.Invalid = CustomerPaymentDto.Invalid;
                CustomerPayment.InvalidChk = CustomerPaymentDto.Invalid == null ? false : true;
                CustomerPayment.OfficeId = CustomerPaymentDto.OfficeId;
                CustomerPayment.Bank = CustomerPaymentDto.Bank;
                CustomerPayment.BankCurrency = CustomerPaymentDto.BankCurrency;
                CustomerPayment.U2T = CustomerPaymentDto.U2T;
                CustomerPayment.R2T = CustomerPaymentDto.R2T;
                CustomerPayment.H2T = CustomerPaymentDto.H2T;
                CustomerPayment.Memo = CustomerPaymentDto.Memo;

                CustomerPayment.GU = copyId == Guid.Empty ? CustomerPaymentDto.PaymentId: Guid.NewGuid();
                CustomerPayment.Edit = edit;
                ViewData["PId"] = CustomerPaymentDto.PaymentId;
            }
            else 
            {
                CustomerPayment.ReleaseDate = DateTime.Now.Date;
                QueryCurrency.Ccy1Id = "19B90321-C852-451D-A1C0-5FA47373ED55";
                QueryCurrency.Ccy2Id = "9D571C85-3C78-41B1-A098-BBB22E8D159B";
                CustomerPayment.U2T = await _currencyAppService.QueryRateInternalAsync(QueryCurrency);
                QueryCurrency.Ccy1Id = "3676C643-B105-4654-9492-332607E7C195";
                CustomerPayment.R2T = await _currencyAppService.QueryRateInternalAsync(QueryCurrency);
                QueryCurrency.Ccy1Id = "7A780112-9D67-4BFE-AC8C-C494AB47FEF4";
                CustomerPayment.H2T = await _currencyAppService.QueryRateInternalAsync(QueryCurrency);

                CustomerPayment.GU = Guid.NewGuid();
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
                if (id.ToString() == "00000000-0000-0000-0000-000000000000" && CopyId.ToString() == "00000000-0000-0000-0000-000000000000")
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
                        if (pl.CodeValue == CustomerPaymentDto.PaymentLevel)
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
                if (id.ToString() == "00000000-0000-0000-0000-000000000000"&& copyId.ToString() == "00000000-0000-0000-0000-000000000000")
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
                        if (pl.CodeValue == CustomerPaymentDto.Category)
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
                if (id.ToString() == "00000000-0000-0000-0000-000000000000" && CopyId.ToString() == "00000000-0000-0000-0000-000000000000")
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
                        if (substation.Id.ToString().ToUpper() == CustomerPaymentDto.OfficeId.ToString().ToUpper())
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

            #region 取收款來源
            queryDto = new QueryDto();
            queryDto.QueryType = "";
            var receivablessources = await _ajaxDropdownAppService.GetAllTradePartners(queryDto);
            if (receivablessources != null)
            {
                RSList = new List<SelectListItem>();
                if (id.ToString() == "00000000-0000-0000-0000-000000000000" && copyId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    RSList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
                    foreach (var receivablessource in receivablessources)
                    {
                        RSList.Add(new SelectListItem() { Text = receivablessource.TPName + "\r\n" + (receivablessource.TPAliasName == null ? "null" : receivablessource.TPAliasName) + "\r\n" + receivablessource.TPCode , Value = receivablessource.Id.ToString() });
                    }
                }
                else
                {
                    RSList.Add(new SelectListItem() { Text = "", Value = ""});
                    foreach (var receivablessource in receivablessources)
                    {
                        if (receivablessource.Id.ToString().ToUpper() == CustomerPaymentDto.ReceivablesSources.ToString().ToUpper())
                        {
                            RSList.Add(new SelectListItem() { Text = receivablessource.TPName + "\r\n" + (receivablessource.TPAliasName == null ? "null" : receivablessource.TPAliasName) + "\r\n" + receivablessource.TPCode, Value = receivablessource.Id.ToString() ,Selected = true});
                        }
                        else 
                        {
                            RSList.Add(new SelectListItem() { Text = receivablessource.TPName + "\r\n" + (receivablessource.TPAliasName == null ? "null" : receivablessource.TPAliasName) + "\r\n" + receivablessource.TPCode, Value = receivablessource.Id.ToString() });
                        }                       
                    }
                }               
            }
            #endregion

            //之後改為讀取DB，暫時寫死
            BankList = new List<SelectListItem>
            {
               new SelectListItem { Value = "UOB", Text = "UOB" },
new SelectListItem { Value = "????-USD", Text = "\u83EF\u5357\u9280\u884C-USD",Selected=true },

new SelectListItem { Value = "Chase Credit Card Example", Text = "Chase Credit Card Example" },

new SelectListItem { Value = "Credit Card Example", Text = "Credit Card Example" },
new SelectListItem { Value = "Petty Cash", Text = "Petty Cash" },
new SelectListItem { Value = "Petty Cash", Text = "Petty Cash" },
new SelectListItem { Value = "testing", Text = "testing" },
new SelectListItem { Value = "OCBC", Text = "OCBC" },
new SelectListItem { Value = "HSBC", Text = "HSBC" },
new SelectListItem { Value = "UOB", Text = "UOB" },
new SelectListItem { Value = "???", Text = "\u96F6\u7528\u91D1" }
            };

            if (id.ToString() != "00000000-0000-0000-0000-000000000000" && copyId.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                foreach (var item in BankList)
                {
                    if (item.Value == CustomerPaymentDto.Bank)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            
            return Page();
        }

        public async Task<JsonResult> OnPostAsync(string datatablelist, CreateUpdateCustomerPaymentDto customerPayment)
        {
            CustomerPaymentDto = await _customerPaymentAppService.CheckByPaymentIdAsync(customerPayment.GU);

            if (CustomerPaymentDto != null)
            {
                //if (customerPayment.Edit != "Y")
                //{
                //    throw new BusinessException(FreightDomainErrorCodes.CustomerPaymentAlreadyExists);
                //}
                customerPayment.PaymentId = customerPayment.GU;
                CustomerPaymentDto= await _customerPaymentAppService.UpdateAsync(Guid.Parse(customerPayment.Id), customerPayment);
            }
            else 
            { 
                customerPayment.PaymentId = customerPayment.GU;
                CustomerPaymentDto= await _customerPaymentAppService.CreateAsync(customerPayment);
            }

            List<CreateUpdateInvDto> list = JsonConvert.DeserializeObject<List<CreateUpdateInvDto>>(datatablelist);
            await _invAppService.UpdateList(customerPayment.GU, list);
            Dictionary<string, Guid> rs = new Dictionary<string, Guid>
            {
                { "id",CustomerPaymentDto.Id }
            };
            return new JsonResult(rs);
        }
    }
}
