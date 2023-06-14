using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Dolphin.Freight.Accounting.Invoices;
using System.Collections.Generic;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.ImportExport.AirImports;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dolphin.Freight.Web.Pages.AirImports
{
    public class EditModal3 : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public Guid Hid { get; set; }
        
        [BindProperty]
        public AirImportMawbDto AirImportMawbDto { get; set; }
        [BindProperty]
        public CreateUpdateAirImportMawbDto AirImportMawb { get; set; }
        [BindProperty]
        public CreateUpdateAirImportHawbDto AirImportHawb { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m0invoiceDtos { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m1invoiceDtos { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m2invoiceDtos { get; set; }

        [BindProperty(SupportsGet = true)]
        public int NewHbl { get; set; } = 0;
        
        public string CardClass { get; set; }
        
        public bool IsShowHbl { get; set; } = false;
        
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        private readonly IAirImportHawbAppService _airImportHawbAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly ISysCodeAppService _sysCodeAppService;

        public EditModal3(IAirImportMawbAppService airImportMawbAppService, IInvoiceAppService invoiceAppService, ISysCodeAppService sysCodeAppService
                        ,IAirImportHawbAppService airImportHawbAppService)
        {
            _airImportMawbAppService = airImportMawbAppService;
            _airImportHawbAppService = airImportHawbAppService;
            _invoiceAppService = invoiceAppService;
            _sysCodeAppService = sysCodeAppService;
        }

        public async Task OnGetAsync()
        {
            AirImportMawbDto = await _airImportMawbAppService.GetAsync(Id);

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
            qidto.ParentId = Id;
            AirImportHawb = new();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var updateItem = ObjectMapper.Map<AirImportMawbDto, CreateUpdateAirImportMawbDto>(AirImportMawbDto);
            await _airImportMawbAppService.UpdateAsync(AirImportMawbDto.Id, updateItem);

            if (AirImportHawb is not null)
            {
                AirImportHawb.MawbId = AirImportMawbDto.Id;
                if (AirImportHawb.Id != Guid.Empty)
                {
                    await _airImportHawbAppService.UpdateAsync(AirImportHawb.Id, AirImportHawb);
                }
                else
                {
                    await _airImportHawbAppService.CreateAsync(AirImportHawb);
                }
            }
            AirImportHawb = new();
            return new JsonResult(new { }) { };
        }
    }
}
