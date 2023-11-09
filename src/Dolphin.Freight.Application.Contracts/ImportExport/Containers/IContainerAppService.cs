using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanImports;
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
        Task<List<CreateUpdateContainerDto>> GetContainersListByBookingId(Guid Id);
        Task SwitchPP(Guid id);
        Task SwitchCTF(Guid id);
        Task<CreateUpdateContainerDto> GetSingleContainerByMblId(Guid id);
        Task<List<CreateUpdateContainerDto>> GetContainerByMblId(Guid id);
        Task<CreateUpdateContainerDto> GetContainerByHblId(Guid id);
        Task<List<ContainerDto>> QueryListHblAsync(Guid hblId);
        Task<int> DeleteByBookingIdAsync(QueryContainerDto query);
        Task<List<ContainerDto>> QueryListBookingAsync(Guid bookingId);
        Task<CreateUpdateContainerDto> GetContainerByBookingId(Guid id);
        Task<int> DeleteByVesselIdAsync(QueryContainerDto query);
        Task<List<ContainerDto>> QueryListVesselAsync(Guid vesselId);
        Task<List<CreateUpdateContainerDto>> GetContainerListByHblId(Guid id);
        Task<List<CreateUpdateContainerDto>> GetContainersByExtraPropertiesHblIds(Guid hblId);
    }
}