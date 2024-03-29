﻿using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.Containers;
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
        Task DeleteMultipleMblsAsync(Guid[] Ids);
        Task SaveDimensionsAsync(DimensionDataModel DataModel);
        Task SetCardSetting(bool IsShowDetail);
        Task<bool> GetCardSettings();
    }
}
