using Dolphin.Freight.Accounting.Inv;
using Dolphin.Freight.Accounting.Payment;
using Dolphin.Freight.AccountingSettings.CurrencyTables;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.Accounting.Payment.CustomerPayment
{
    public class DocCenterModel : FreightPageModel
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
        public readonly IAttachmentAppService _attachmentAppService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocCenterModel(ICustomerPaymentAppService customerPaymentAppService, ICurrencyTableAppService currencyAppService, ISysCodeAppService sysCodeRepository,
            ISubstationAppService substationAppService, IAjaxDropdownAppService ajaxDropdownAppService, IInvAppService invAppService, IAttachmentAppService attachmentAppService,
            IWebHostEnvironment webHostEnvironment)
        {
            _customerPaymentAppService = customerPaymentAppService;
            _currencyAppService = currencyAppService;
            _sysCodeAppService = sysCodeRepository;
            _substationAppService = substationAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _invAppService = invAppService;
            _attachmentAppService = attachmentAppService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGetAsync(Guid id, string edit, Guid copyId)
        {
            CustomerPayment = new CreateUpdateCustomerPaymentDto();
            QueryCurrency = new QueryCurrencyTableDto();

            if (id.ToString() != "00000000-0000-0000-0000-000000000000" || copyId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                var newId = copyId != Guid.Empty ? copyId : id;
                CustomerPaymentDto = await _customerPaymentAppService.GetDataAsync(newId);
                CustomerPayment.Id = copyId == Guid.Empty ? id.ToString() : null;
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

                CustomerPayment.GU = copyId == Guid.Empty ? CustomerPaymentDto.PaymentId : Guid.NewGuid();
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

            if (list.Count > 0)
            {
                PLList = new List<SelectListItem>();
                if (id.ToString() == "00000000-0000-0000-0000-000000000000" && CopyId.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    foreach (var pl in list.DistinctBy(x => x.CodeValue))
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
                if (id.ToString() == "00000000-0000-0000-0000-000000000000" && copyId.ToString() == "00000000-0000-0000-0000-000000000000")
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
                            SubstationList.Add(new SelectListItem() { Text = substation.SubstationName + "(" + substation.AbbreviationName + ")", Value = substation.Id.ToString(), Selected = true });
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
                        RSList.Add(new SelectListItem() { Text = receivablessource.TPName + "\r\n" + (receivablessource.TPAliasName == null ? "null" : receivablessource.TPAliasName) + "\r\n" + receivablessource.TPCode, Value = receivablessource.Id.ToString() });
                    }
                }
                else
                {
                    RSList.Add(new SelectListItem() { Text = "", Value = "" });
                    foreach (var receivablessource in receivablessources)
                    {
                        if (receivablessource.Id.ToString().ToUpper() == CustomerPaymentDto.ReceivablesSources.ToString().ToUpper())
                        {
                            RSList.Add(new SelectListItem() { Text = receivablessource.TPName + "\r\n" + (receivablessource.TPAliasName == null ? "null" : receivablessource.TPAliasName) + "\r\n" + receivablessource.TPCode, Value = receivablessource.Id.ToString(), Selected = true });
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

        public async Task<IActionResult> OnPostAsync()
        {
            await _customerPaymentAppService.UpdateAsync(Id, CustomerPayment);
            return NoContent();
        }
        public async Task<IActionResult> OnPostMyUploader(List<IFormFile> MyUploader, Guid fid, int ftype)
        {
            string fname = "";
            if (MyUploader != null)
            {
                foreach (var file in MyUploader)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        DirectoryInfo folder = Directory.CreateDirectory(uploadsFolder);
                    }
                    string filePath = Path.Combine(uploadsFolder, file.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    fname = file.FileName;
                    CreateUpdateAttachmentDto dto = new CreateUpdateAttachmentDto() { FileName = fname, ShowName = fname, Ftype = ftype, Fid = fid, Size = file.Length / 1024 };
                    await _attachmentAppService.CreateAsync(dto);
                }
                return new ObjectResult(new { status = "success", fname = fname, udate = DateTime.Now.ToString("yyyy-MM-dd"), size = 1024 });
            }
            return new ObjectResult(new { status = "fail" });

        }
        public async Task<IActionResult> OnGetDownload(string filename)
        {
            if (filename == null)
                return Content("filename is not availble");
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
            var path = Path.Combine(uploadsFolder, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        // Get content type
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
                {
                    {".txt", "text/plain"},
                    {".pdf", "application/pdf"},
                    {".doc", "application/vnd.ms-word"},
                    {".docx", "application/vnd.ms-word"},
                    {".xls", "application/vnd.ms-excel"},
                    {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                    {".png", "image/png"},
                    {".jpg", "image/jpeg"},
                    {".jpeg", "image/jpeg"},
                    {".gif", "image/gif"},
                    {".csv", "text/csv"}
                };
        }
    }
}
