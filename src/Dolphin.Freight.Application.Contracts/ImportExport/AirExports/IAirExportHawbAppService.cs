﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public interface IAirExportHawbAppService :
        ICrudAppService<
            AirExportHawbDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateAirExportHawbDto
        >
    {
        Task<AirExportHawbDto> GetHblById(Guid Id);
    }
}
