using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanExports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public interface IAirExportMawbAppService :
        ICrudAppService<
            AirExportMawbDto,
            Guid,
            QueryHblDto,
            CreateUpdateAirExportMawbDto
        >
    {
        Task<AirExportDetails> GetAirExportDetailsById(Guid Id);
        Task LockedOrUnLockedAirExportMawbAsync(Guid id);
        Task SelectedLockedAirExportMawbAsync(Guid[] ids);
        Task SelectedUnLockedAirExportMawbAsync(Guid[] ids);
    }
}
