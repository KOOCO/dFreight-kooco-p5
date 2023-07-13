
using Dolphin.Freight.Accounting.Inv;
using Dolphin.Freight.ImportExport.OceanExports;
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
    }
}
