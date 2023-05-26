﻿using Dolphin.Freight.Common;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public interface IOceanExportMblAppService :
        ICrudAppService< 
        OceanExportMblDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateOceanExportMblDto> 
    {
        Task<PagedResultDto<OceanExportMblDto>> QueryListAsync(QueryMblDto query);
        void LockedOrUnLockedOceanExportMblAsync(QueryMblDto query);
        Task<CreateUpdateOceanExportMblDto> GetCreateUpdateOceanExportMblDtoById(Guid Id);
    }
}
