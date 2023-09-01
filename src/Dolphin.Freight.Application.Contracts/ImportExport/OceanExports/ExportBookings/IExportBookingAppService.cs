using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.OceanExports.ExportBookings
{
    public interface IExportBookingAppService :
        ICrudAppService<
        ExportBookingDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateExportBookingDto>
    {
        Task<PagedResultDto<ExportBookingDto>> QueryListAsync(QueryExportBookingDto query);
        Task<List<ExportBookingDto>> GetSONo();
        Task<CreateUpdateExportBookingDto> GetBookingCardById(Guid Id);
    }
}