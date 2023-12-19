using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dolphin.Freight.Web.Pages.AirExports.ModalPopups
{
    public class CopyToAirExportHawbModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid PrevMawbId { get; set; }
        [BindProperty]
        public string MawbDocNo { get; set; }
        [BindProperty]
        public CopyModelInfo CopyModel { get; set; }

        private readonly IAirExportMawbAppService _airExportMawbAppService;
        private readonly IAirExportHawbAppService _airExportHawbAppService;
        private readonly IInvoiceAppService _invoiceAppService;

        public CopyToAirExportHawbModel(IAirExportMawbAppService airExportMawbAppService, IAirExportHawbAppService airExportHawbAppService,
                                        IInvoiceAppService invoiceAppService)
        {
            _airExportMawbAppService = airExportMawbAppService;
            _airExportHawbAppService = airExportHawbAppService;
            _invoiceAppService = invoiceAppService;
        }

        public async Task OnGetAsync()
        {
            CopyModel = new();

            CopyModel.CopyAccountingInformation = true;
            CopyModel.AP = true;
            CopyModel.AR = true;
            CopyModel.DC = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var mawb = await _airExportMawbAppService.GetAsync(CopyModel.MawbId);
            MawbDocNo = mawb.FilingNo;
            
            var Hawb = ObjectMapper.Map<AirExportHawbDto, CreateUpdateAirExportHawbDto>(await _airExportHawbAppService.GetAsync(Id));

            Hawb.Id = Guid.Empty;
            Hawb.MawbId = CopyModel.MawbId;
            Hawb.HawbNo = "Copy-1";

            var newHawb = await _airExportHawbAppService.CreateAsync(Hawb);

            if (CopyModel.CopyAccountingInformation)
            {
                await _invoiceAppService.CreateReplicaAccounting(Id, newHawb.Id, 4, CopyModel.AP, CopyModel.AR, CopyModel.DC);
            }

            var rs = new Dictionary<string, object>
            {
                { "MawbId", CopyModel.MawbId },
            };

            return new JsonResult(rs);
        }
    }

    public class CopyModelInfo
    {
        public bool CopyAccountingInformation { get; set; }
        public bool AP { get; set; }
        public bool AR { get; set; }
        public bool DC { get; set; }
        public Guid MawbId { get; set; }
    }
}
