using Dolphin.Freight.Common;
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
            QueryDto,
            CreateUpdateAirImportMawbDto
        >
    {
        Task<AirImportDetails> GetAirImportDetailsById(Guid Id);
        Task<AirImportMawbDto> GetAirImportMawbDetailsById(Guid Id);
        Task LockedOrUnLockedAirImportMawbAsync(Guid id);

    }
}
