using Dolphin.Freight.ImportExport.AirImports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.Configuration
{
    public interface IGridPreferenceAppService :
        ICrudAppService<
            GridPreferenceDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateGridPreferenceDto
        >
    {
        Task<GridPreferenceDto> GetGridPreferenceBySrcAsync(string src, Guid userId);

        Task SaveGridPreferenceById(GridPreferenceDto gridPreferenceDto);
    }
}
