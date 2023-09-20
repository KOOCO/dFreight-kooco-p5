using Dolphin.Freight.Common;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.OceanImports
{
    public interface IOceanImportMblAppService :
        ICrudAppService< 
        OceanImportMblDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateOceanImportMblDto> 
    {
        Task<PagedResultDto<OceanImportMblDto>> QueryListAsync(QueryMblDto query);
        Task LockedOrUnLockedOceanImportMblAsync(QueryMblDto query);
        Task<CreateUpdateOceanImportMblDto> GetCreateUpdateOceanImportMblDtoById(Guid Id);
        Task<OceanImportDetails> GetOceanImportDetailsById(Guid Id);
        Task SetLockOrUnlockStatusOceanImportMblAsync(Guid[] Ids, bool IsLocked);
    }
}
