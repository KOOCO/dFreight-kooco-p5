using Dolphin.Freight.Common;
using System;
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
        Task<AirExportHawbDto> GetDocCenterCardById(Guid Id);
        Task<List<AirExportHawbDto>> GetDocCenterCardsById(Guid Id);
        Task<List<AirExportHawbDto>> GetHblCardsById(Guid Id, bool isAsc = true, int sortType = 1);
        Task<AirExportHawbDto> GetHawbCardById(Guid Id);
        Task<AirExportHawbDto> GetHawbWithDetailsById(Guid Id);
        Task<AirExportDetails> GetAirExportDetailsById(Guid Id);
        Task LockedOrUnLockedAirExportHawbAsync(Guid id);
        Task SelectedLockedAirExportHawbAsync(Guid[] ids);
        Task SelectedUnLockedAirExportHawbAsync(Guid[] ids);
        Task UpdateMawbIdOfHawbAsync(Guid HawbId, Guid NewMawbId);
    }
}
