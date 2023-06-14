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
        Task<List<AirExportHawbDto>> GetHblCardsById(Guid Id);
        Task<AirExportHawbDto> GetHawbCardById(Guid Id);
    }
}
