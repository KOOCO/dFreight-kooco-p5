using Dolphin.Freight.ImportExport.AirExports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public interface IAirImportHawbAppService : 
        ICrudAppService<
            AirImportHawbDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateAirImportHawbDto
        >
    {
        Task<AirImportHawbDto> GetDocCenterCardById(Guid Id);
        Task<List<AirImportHawbDto>> GetDocCenterCardsById(Guid Id);
        Task<List<AirImportHawbDto>> GetHblCardsById(Guid Id);
        Task<AirImportHawbDto> GetHawbCardById(Guid Id);
    }
}
