using Dolphin.Freight.ImportExport.OceanExports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using static Dolphin.Freight.Permissions.AccountingPermissions;

namespace Dolphin.Freight.Accounting.Payment
{
    public interface IPaymentAppService :
        ICrudAppService<
        PaymentDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdatePaymentDto>,
        IApplicationService
    {
        Task<PaymentDto> GetDataAsync(Guid id);
        Task<PagedResultDto<PaymentDto>> GetDataList(QueryPaymentDto query);
        Task<PaymentDto> CheckByPaymentIdAsync(Guid PaymentId);
    }
}
