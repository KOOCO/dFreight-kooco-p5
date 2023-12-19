using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dolphin.Freight.Web.Pages.OceanExports.ModalPopups;

public class OceanExportMBLCopyModel : AbpPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }
    [BindProperty]
    public CopyModelInfo CopyModel { get; set; }
    public async Task OnGetAsync()
    {
        CopyModel = new();

        CopyModel.IsCopyContainerInfo = true;
        CopyModel.IsCopyVesselInfoAndShippingSchedule = true;
        CopyModel.CopyAccountingInformation = true;
        CopyModel.AP = true;
        CopyModel.AR = true;
        CopyModel.DC = true;
    }

    public async Task<JsonResult> OnPostAsync()
    {
        var retVal = new Dictionary<string, object>
        {
            { "Id", Id },
            { "IsCopyVesselInfoAndShippingSchedule", CopyModel.IsCopyVesselInfoAndShippingSchedule },
            { "IsCopyContainerInfo", CopyModel.IsCopyContainerInfo },
            { "CopyAccountingInformation", CopyModel.CopyAccountingInformation },
            { "AR", CopyModel.AR },
            { "AP", CopyModel.AP },
            { "DC", CopyModel.DC }
        };

        return new JsonResult(retVal);
    }

    public class CopyModelInfo
    {
        public bool IsCopyVesselInfoAndShippingSchedule { get; set; }
        public bool IsCopyContainerInfo { get; set; }
        public bool CopyAccountingInformation { get; set; }
        public bool AP { get; set; }
        public bool AR { get; set; }
        public bool DC { get; set; }
        public Guid MblId { get; set; }
    }
}
