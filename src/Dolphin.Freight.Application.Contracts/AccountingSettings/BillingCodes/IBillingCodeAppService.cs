using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.AccountingSettings.BillingCodes
{
    public interface IBillingCodeAppService :
       ICrudAppService<
       BillingCodeDto,
       Guid,
       PagedAndSortedResultRequestDto,
       CreateUpdateBillingCodeDto>
    {
        // 海運進口分單
        Task<List<BillingCodeDto>> GetOIHListAsync();
        // 海運出口分單
        Task<List<BillingCodeDto>> GetOEHListAsync();
        // 空運進口分單
        Task<List<BillingCodeDto>> GetAIHListAsync();
        // 空運出口分單
        Task<List<BillingCodeDto>> GetAEHListAsync();
        // 卡車
        Task<List<BillingCodeDto>> GetTKMListAsync();
        // 綜合業務
        Task<List<BillingCodeDto>> GetMSMListAsync();
        // 倉儲
        Task<List<BillingCodeDto>> GetWHSListAsync();

        Task<PagedResultDto<BillingCodeDto>> QueryListAsync(QueryBillingCodeDto query);
        Task<List<BillingCodeDto>> QueryListForTagAsync(QueryBillingCodeDto query);
        Task<List<BillingCodeDto>> GetLookUpListAsync();
    }
}
