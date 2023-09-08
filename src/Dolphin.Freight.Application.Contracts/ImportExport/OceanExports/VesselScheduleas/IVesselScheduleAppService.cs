using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas
{
    public interface IVesselScheduleAppService :
        ICrudAppService<
        VesselScheduleDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateVesselScheduleDto>
    {
        Task<PagedResultDto<VesselScheduleDto>> QueryListAsync(QueryVesselScheduleDto query);
        Task<List<VesselScheduleDto>> GetListAsync(QueryVesselScheduleDto query);
        Task DeleteMultipleVesselAsync(Guid[] ids);
    }
}