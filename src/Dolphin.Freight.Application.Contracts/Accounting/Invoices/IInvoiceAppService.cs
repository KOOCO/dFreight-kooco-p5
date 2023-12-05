
using Dolphin.Freight.Accounting.Inv;
using Dolphin.Freight.ImportExport.OceanExports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.Accounting.Invoices
{
    public interface IInvoiceAppService :
        ICrudAppService<
        InvoiceDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateInvoiceDto>
    {
        Task<PagedResultDto<InvoiceDto>> QueryListAsync(QueryInvoiceDto query);
        Task<IList<InvoiceDto>> QueryInvoicesAsync(QueryInvoiceDto query);
        Task<List<CopyIdDto>> CopyByBookingId(QueryInvoiceDto query,int IsAR,int IsAp,int IsDC);
        Task<bool> QueryInvoicesCheckAsync(QueryInvoiceDto query);
        Task DeleteGAInvoicesByIdAsync(Guid[] Ids);
        Task<JsonResult> CopyGAInvoiceAsync(Guid Id);
        Task CreateMblHblAccountingForCopiedOE_OI(Guid OldMblId, Guid NewMblId, bool IsAP = false, bool IsAR = false, bool IsDC = false);
    }
}
