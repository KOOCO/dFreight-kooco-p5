using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Common;
using Dolphin.Freight.Common.Memos;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ImportExport.OceanExports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dolphin.Freight.Web.Pages.AirImports
{
    public class CopyHawbModal : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid PrevMawbId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool isOceanExportHbl { get; set; }
        [BindProperty]
        public CopyModelInfo CopyModel { get; set; }
        public List<SelectListItem> HblOptions { get; set; }
        [BindProperty]
        public string MawbDocNo { get; set; }

        private readonly IAirImportHawbAppService _airImportHawbAppService;
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IInvoiceBillAppService _invoiceBillAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
       
        public CopyHawbModal(IAirImportHawbAppService airImportHawbAppService, IAirImportMawbAppService airImportMawbAppService,
                            IInvoiceAppService invoiceAppService,IInvoiceBillAppService invoiceBillAppService,
                            IOceanExportMblAppService oceanExportMblAppService, IOceanExportHblAppService oceanExportHblAppService)
        {
            _airImportHawbAppService = airImportHawbAppService;
            _airImportMawbAppService= airImportMawbAppService;
            _invoiceAppService = invoiceAppService;
            _invoiceBillAppService = invoiceBillAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _oceanExportMblAppService = oceanExportMblAppService;
        }
        public async Task OnGetAsync()
        {
            CopyModel = new CopyModelInfo();
            HblOptions = FillHblOptions();
            CopyModel.CopyAccountingInformation = true;
            CopyModel.AP = true;
            CopyModel.AR = true;
            CopyModel.DC = true;
            CopyModel.CopyHawb = "FirstHawbOnly";
            CopyModel.IsCopyFlightInfo = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (isOceanExportHbl)
            {
                var mbl = await _oceanExportMblAppService.GetAsync(CopyModel.MawbId);
                MawbDocNo = mbl.FilingNo;
                var hbl = ObjectMapper.Map<OceanExportHblDto, CreateUpdateOceanExportHblDto>(await _oceanExportHblAppService.GetAsync(Id));

                hbl.Id = Guid.Empty;
                hbl.MblId = CopyModel.MawbId;
                hbl.HblNo = "Copy-1";

                var newHbl = await _oceanExportHblAppService.CreateAsync(hbl);

                if (CopyModel.CopyAccountingInformation)
                {
                    QueryInvoiceDto q1idto = new QueryInvoiceDto() { QueryType = 1, ParentId = Id };
                    var invoiceDtos1 = await _invoiceAppService.QueryInvoicesAsync(q1idto);

                    if (invoiceDtos1 != null && invoiceDtos1.Count > 0)
                    {
                        if (CopyModel.AR)
                        {
                            var invoiceAp = invoiceDtos1.Where(x => x.InvoiceType == 0).ToList();
                            foreach (var invoice in invoiceAp)
                            {
                                var newInvoiceAp = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                newInvoiceAp.HawbId = newHbl.Id;
                                newInvoiceAp.Id = Guid.Empty;
                                var createInvoice = await _invoiceAppService.CreateAsync(newInvoiceAp);
                                QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                                query.InvoiceNo = invoice.Id.ToString();
                                var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                                foreach (var bill in invoiceBills)
                                {
                                    var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                                    newbill.InvoiceId = createInvoice.Id;
                                    newbill.Id = Guid.Empty;
                                    
                                    await _invoiceBillAppService.CreateAsync(newbill);
                                }
                            }
                        }
                        if (CopyModel.DC)
                        {
                            var invoiceDc = invoiceDtos1.Where(x => x.InvoiceType == 1).ToList();
                            foreach (var invoice in invoiceDc)
                            {
                                var newInvoiceDc = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                newInvoiceDc.HawbId = newHbl.Id;
                                newInvoiceDc.Id = Guid.Empty;
                                var createInvoice = await _invoiceAppService.CreateAsync(newInvoiceDc);
                                QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                                query.InvoiceNo = invoice.Id.ToString();
                                var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                                foreach (var bill in invoiceBills)
                                {
                                    var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                                    newbill.InvoiceId = createInvoice.Id;
                                    newbill.Id = Guid.Empty;

                                    await _invoiceBillAppService.CreateAsync(newbill);
                                }
                            }
                        }
                        if (CopyModel.AP)
                        {
                            var invoiceAr = invoiceDtos1.Where(x => x.InvoiceType == 2).ToList();
                            foreach (var invoice in invoiceAr)
                            {
                                var newInvoiceAr = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                newInvoiceAr.HawbId = newHbl.Id;
                                newInvoiceAr.Id = Guid.Empty;
                                var createInvoice = await _invoiceAppService.CreateAsync(newInvoiceAr);
                                QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                                query.InvoiceNo = invoice.Id.ToString();
                                var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                                foreach (var bill in invoiceBills)
                                {
                                    var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                                    newbill.InvoiceId = createInvoice.Id;
                                    newbill.Id = Guid.Empty;

                                    await _invoiceBillAppService.CreateAsync(newbill);
                                }
                            }
                        }
                    }

                    var rs = new Dictionary<string, object>
                    {
                        { "MawbId", CopyModel.MawbId },
                    };

                    return new JsonResult(rs);
                }

            } 
            else
            {
                var mawb = await _airImportMawbAppService.GetAsync(CopyModel.MawbId);
                MawbDocNo = mawb.FilingNo;
                var Hawb = ObjectMapper.Map<AirImportHawbDto, CreateUpdateAirImportHawbDto>(await _airImportHawbAppService.GetAsync(Id));

                Hawb.Id = Guid.Empty;
                Hawb.MawbId = CopyModel.MawbId;
                Hawb.HawbNo = "Copy-1";

                var newHawb = await _airImportHawbAppService.CreateAsync(Hawb);

                if (CopyModel.CopyAccountingInformation)
                {
                    QueryInvoiceDto q1idto = new QueryInvoiceDto() { QueryType = 4, ParentId = Id };
                    var invoiceDtos1 = await _invoiceAppService.QueryInvoicesAsync(q1idto);

                    if (invoiceDtos1 != null && invoiceDtos1.Count > 0)
                    {
                        if (CopyModel.AR)
                        {
                            var invoiceAp = invoiceDtos1.Where(x => x.InvoiceType == 0).ToList();
                            foreach (var invoice in invoiceAp)
                            {

                                var newInvoiceAp = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                newInvoiceAp.HawbId = newHawb.Id;
                                newInvoiceAp.Id = Guid.Empty;
                                var createInvoice = await _invoiceAppService.CreateAsync(newInvoiceAp);
                                QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                                query.InvoiceNo = invoice.Id.ToString();
                                var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                                foreach (var bill in invoiceBills)
                                {
                                    var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                                    newbill.InvoiceId = createInvoice.Id;
                                    newbill.Id = Guid.Empty;
                                    await _invoiceBillAppService.CreateAsync(newbill);
                                }
                            }
                        }
                        if (CopyModel.DC)
                        {
                            var invoiceDc = invoiceDtos1.Where(x => x.InvoiceType == 1).ToList();
                            foreach (var invoice in invoiceDc)
                            {

                                var newInvoiceDc = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                newInvoiceDc.HawbId = newHawb.Id;
                                newInvoiceDc.Id = Guid.Empty;
                                var createInvoice = await _invoiceAppService.CreateAsync(newInvoiceDc);
                                QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                                query.InvoiceNo = invoice.Id.ToString();
                                var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                                foreach (var bill in invoiceBills)
                                {
                                    var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                                    newbill.InvoiceId = createInvoice.Id;
                                    newbill.Id = Guid.Empty;
                                    await _invoiceBillAppService.CreateAsync(newbill);
                                }
                            }
                        }
                        if (CopyModel.AP)
                        {
                            var invoiceAr = invoiceDtos1.Where(x => x.InvoiceType == 2).ToList();
                            foreach (var invoice in invoiceAr)
                            {
                                var newInvoiceAr = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                newInvoiceAr.HawbId = newHawb.Id;
                                newInvoiceAr.Id = Guid.Empty;
                                var createInvoice = await _invoiceAppService.CreateAsync(newInvoiceAr);
                                QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                                query.InvoiceNo = invoice.Id.ToString();
                                var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                                foreach (var bill in invoiceBills)
                                {
                                    var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                                    newbill.InvoiceId = createInvoice.Id;
                                    newbill.Id = Guid.Empty;
                                    await _invoiceBillAppService.CreateAsync(newbill);
                                }
                            }
                        }
                    }
                }
                var rs = new Dictionary<string, object>
                {
                    { "MawbId", CopyModel.MawbId },
                };

                return new JsonResult(rs);
            }

            return NoContent();
        }
        private List<SelectListItem> FillHblOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "FirstHawbOnly", Text = "First HAWB Only"},
                new SelectListItem { Value = "AllHAWB", Text = "All HAWB"}
            };
        }
    }

}
