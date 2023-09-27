using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.AirExports.Bookings
{
    public interface IExportBookingAppService :
       ICrudAppService<
       ExportBookingDto,
       Guid,
       PagedAndSortedResultRequestDto,
       CreateUpdateExportBookingDto>
    {
        Task<PagedResultDto<ExportBookingDto>> GetQueryListAsync(QueryExportBookingDto query);
        Task<List<ExportBookingDto>> GetSONo();
        Task DeleteAirExportBookingAsync(Guid id);
    }
}
