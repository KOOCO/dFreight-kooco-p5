using Dolphin.Freight.ImportExport.OceanImports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Dolphin.Freight.Accounting.Invoices;
using System.Collections.Generic;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.SysCodes;

namespace Dolphin.Freight.Web.Pages.OceanImports
{
    public class EditModal3Model : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid Hid { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportMblDto OceanImportMbl { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportHblDto OceanImportHbl { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ISToolTipShow { get; set; }
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
        public IList<OceanImportHblDto> OceanImportHbls { get; set; }
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        public EditModal3Model(IOceanImportMblAppService oceanImportMblAppService, IOceanImportHblAppService oceanImportHblAppService, IInvoiceAppService invoiceAppService, ISysCodeAppService sysCodeAppService)
        {
            _oceanImportMblAppService = oceanImportMblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _invoiceAppService = invoiceAppService;
            _sysCodeAppService = sysCodeAppService;
        }
        public async Task OnGetAsync()
        {
            OceanImportMbl = await _oceanImportMblAppService.GetCreateUpdateOceanImportMblDtoById(Id);
            OceanImportMbl.Mid = Id;
            ISToolTipShow = await _oceanImportMblAppService.GetCardSettings();
            QueryHblDto query = new QueryHblDto() { MblId = OceanImportMbl.Id };
            OceanImportHbls = await _oceanImportHblAppService.QueryListByMidAsync(query);
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
            
            QueryHblDto queryHbl = new QueryHblDto();
            if (Hid == Guid.Empty)
            {
                if (NewHbl == 1)
                {
                    OceanImportHbl = new CreateUpdateOceanImportHblDto();
                    QueryDto cquery = new QueryDto();
                    cquery.QueryType = "CardColorId";
                    var syscodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(cquery);
                    if (OceanImportHbls != null && OceanImportHbls.Count > 0)
                    {
                        int index = OceanImportHbls.Count % syscodes.Count;
                        OceanImportHbl.CardColorId = syscodes[index].Id;
                        OceanImportHbl.CardColorValue = syscodes[index].CodeValue;
                        CardClass = syscodes[index].CodeValue;
                    }
                    else
                    {
                        OceanImportHbl.CardColorId = syscodes[0].Id;
                        OceanImportHbl.CardColorValue = syscodes[0].CodeValue;
                        CardClass = syscodes[0].CodeValue;
                    }

                }
                else
                {
                    OceanImportHbl = new CreateUpdateOceanImportHblDto();
                    if (OceanImportHbls != null && OceanImportHbls.Count > 0)
                    {
                        OceanImportHbl = ObjectMapper.Map<OceanImportHblDto, CreateUpdateOceanImportHblDto>(OceanImportHbls[0]);
                        Hid = OceanImportHbl.Id;
                        IsShowHbl = true;
                    }
                }

            }
            else
            {
                queryHbl.Id = Hid;
                OceanImportHbl = new();
                IsShowHbl = true;


            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _oceanImportMblAppService.UpdateAsync(Id, OceanImportMbl);
            await _oceanImportHblAppService.UpdateAsync(Hid, OceanImportHbl);
            return NoContent();
        }
    }
}
