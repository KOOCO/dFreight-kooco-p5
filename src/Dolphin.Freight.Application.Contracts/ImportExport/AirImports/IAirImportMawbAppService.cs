﻿using Dolphin.Freight.Common;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public interface IAirImportMawbAppService :
        ICrudAppService<
            AirImportMawbDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateAirImportMawbDto
        >
    {
        Task<AirImportMawbDto> GetAirImportDetailsById(Guid Id);
    }
}
