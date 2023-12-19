using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;
using static Dolphin.Freight.Permissions.OceanExportPermissions;
using static Volo.Abp.Http.MimeTypes;

namespace Dolphin.Freight.Web.Pages.OceanExports.VesselSchedules
{
    public class Edit2Model : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m0invoiceDtos { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ISToolTipShow { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m1invoiceDtos { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m2invoiceDtos { get; set; }

        public VesselScheduleDto VesselScheduleDto { get; set; }

        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IVesselScheduleAppService _vesselScheduleAppService;

        public ILogger<Edit2Model> Logger { get; set; }

        public Edit2Model(IInvoiceAppService invoiceAppService, IVesselScheduleAppService vesselScheduleAppService)
        {
            _invoiceAppService = invoiceAppService;
            _vesselScheduleAppService = vesselScheduleAppService;

            Logger = NullLogger<Edit2Model>.Instance;
        }

        public async Task OnGetAsync()
        {
            VesselScheduleDto = await _vesselScheduleAppService.GetAsync(Id);

            QueryInvoiceDto qidto = new QueryInvoiceDto() { QueryType = 5, ParentId = Id };
            IList<InvoiceDto> invoiceDtos = await _invoiceAppService.QueryInvoicesAsync(qidto);
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
        }
    }
}
