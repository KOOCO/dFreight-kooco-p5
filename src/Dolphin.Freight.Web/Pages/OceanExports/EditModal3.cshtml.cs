using Dolphin.Freight.ImportExport.OceanExports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Dolphin.Freight.Accounting.Invoices;
using System.Collections.Generic;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ImportExport.OceanImports;

namespace Dolphin.Freight.Web.Pages.OceanExports
{
    public class EditModal3Model : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid Hid { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ISToolTipShow { get; set; }
        [BindProperty]
        public OceanExportHblDto OceanExportHblDto { get; set; }
        [BindProperty]
        public OceanExportMblDto OceanExportMblDto { get; set; }
        [BindProperty]
        public CreateUpdateOceanExportMblDto OceanExportMbl { get; set; }
        [BindProperty]
        public CreateUpdateOceanExportHblDto OceanExportHbl { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m0invoiceDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m1invoiceDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m2invoiceDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> h0invoiceDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> h1invoiceDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> h2invoiceDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public int NewHbl { get; set; } = 0;
        public string CardClass { get; set; }
        public bool IsShowHbl { get; set; } = false;
        public IList<OceanExportHblDto> OceanExportHbls { get; set; }
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        public EditModal3Model(IOceanExportMblAppService oceanExportMblAppService,
            IOceanExportHblAppService oceanExportHblAppService,
            IInvoiceAppService invoiceAppService,
            ISysCodeAppService sysCodeAppService, IOceanImportMblAppService oceanImportMblAppService)
        {
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _invoiceAppService = invoiceAppService;
            _sysCodeAppService = sysCodeAppService;
            _oceanImportMblAppService=oceanImportMblAppService;
        }
        public async Task OnGetAsync()
        {
            ViewData["HAVEHBL"] = "N";
            OceanExportMbl = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(Id);

            QueryInvoiceDto qidto = new QueryInvoiceDto() { QueryType = 3, ParentId = Id };
            var invoiceDtos = await _invoiceAppService.QueryInvoicesAsync(qidto);
            m0invoiceDtos = new List<InvoiceDto>();
            m1invoiceDtos = new List<InvoiceDto>();
            m2invoiceDtos = new List<InvoiceDto>();
            if (invoiceDtos != null && invoiceDtos.Count > 0)
            {
                foreach (var dto in invoiceDtos)
                {
                    switch (dto.InvoiceType)
                    {
                        default:
                            m0invoiceDtos.Add(dto);
                            break;
                        case 1:
                            m1invoiceDtos.Add(dto);
                            break;
                        case 2:
                            m2invoiceDtos.Add(dto);
                            break;
                    }
                }
            }
            ImportExport.OceanExports.QueryHblDto query = new ImportExport.OceanExports.QueryHblDto() { MblId = Id };
            query.Id = Hid;
            ISToolTipShow = await _oceanImportMblAppService.GetCardSettings();
            //OceanExportHbl = new();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var updateItem = ObjectMapper.Map<OceanExportMblDto, CreateUpdateOceanExportMblDto>(OceanExportMblDto);
            await _oceanExportMblAppService.UpdateAsync(OceanExportMblDto.Id, updateItem);

            if (OceanExportHblDto is not null)
            {
                OceanExportHbl.MblId = OceanExportMblDto.Id;
                if (OceanExportHbl.Id != Guid.Empty)
                {
                    await _oceanExportHblAppService.UpdateAsync(OceanExportHbl.Id, OceanExportHbl);
                }
                else
                {
                    await _oceanExportHblAppService.CreateAsync(OceanExportHbl);
                }
            }
            return new ObjectResult(new { id = OceanExportMblDto.Id });
        }
    }
}
