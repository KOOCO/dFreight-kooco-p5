using Dolphin.Freight.ImportExport.AirImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.ImportExport.Configuration
{
    public class GridPreferenceAppService :
        CrudAppService<
            GridPreference,
            GridPreferenceDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateGridPreferenceDto>,
        IGridPreferenceAppService
    {

        public GridPreferenceAppService(IRepository<GridPreference, Guid> repository) : base(repository)
        {

        }

        public async Task<GridPreferenceDto> GetGridPreferenceBySrcAsync(string src, Guid userId)
        {
            var data = await Repository.FirstOrDefaultAsync(w => w.PreferenceSrc.ToUpper() == src.ToUpper());

            return ObjectMapper.Map<GridPreference, GridPreferenceDto>(data);
        }

        public async Task SaveGridPreferenceById(GridPreferenceDto gridPreferenceDto)
        {
            var data = ObjectMapper.Map<GridPreferenceDto, GridPreference>(gridPreferenceDto);

            await Repository.UpdateAsync(data);
        }
    }
}
