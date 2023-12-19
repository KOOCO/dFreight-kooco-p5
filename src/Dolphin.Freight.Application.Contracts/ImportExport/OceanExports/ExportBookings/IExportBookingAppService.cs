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
        Task<List<ExportBookingDto>> GetBookingCardsById(Guid Id, bool isAsc = true, int sortType = 1);
        Task<CreateUpdateExportBookingDto> GetBookingCardById(Guid Id);
        Task<bool> IsBookingContainsByVesselId(Guid id);
    }
}