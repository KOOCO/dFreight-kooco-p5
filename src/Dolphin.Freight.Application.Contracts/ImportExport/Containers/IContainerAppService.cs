using Dolphin.Freight.ImportExport.Containers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.Containers
{
    public interface IContainerAppService :
        ICrudAppService<
        ContainerDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateContainerDto>
    {
        Task<List<ContainerDto>> QueryListAsync(QueryContainerDto query);
        Task<int> DeleteByMblIdAsync(QueryContainerDto query);

        Task SwitchPP(Guid id);
        Task SwitchCTF(Guid id);

        Task<CreateUpdateContainerDto> GetContainerByHblId(Guid id);
    }
}