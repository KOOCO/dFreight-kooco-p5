using Dolphin.Freight.ImportExport.OceanExports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using static Dolphin.Freight.Permissions.AccountingPermissions;

namespace Dolphin.Freight.Accounting.Payment
{
    public interface ICustomerPaymentAppService :
        ICrudAppService<
        CustomerPaymentDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCustomerPaymentDto>,
        IApplicationService
    {
        Task<CustomerPaymentDto> GetDataAsync(Guid id);
        Task<PagedResultDto<CustomerPaymentDto>> GetDataList();
        Task<CustomerPaymentDto> CheckByPaymentIdAsync(Guid PaymentId);
        Task DeleteMultiplePaymentsAsync(Guid[] ids);
    }
}
