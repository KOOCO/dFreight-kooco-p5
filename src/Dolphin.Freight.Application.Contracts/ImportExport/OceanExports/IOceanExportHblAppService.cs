using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public interface IOceanExportHblAppService :
        ICrudAppService< 
        OceanExportHblDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateOceanExportHblDto> 
    {
        Task<List<CreateUpdateContainerDto>> GetContainerByHblExtraProperties(Guid Id);
        Task<PagedResultDto<OceanExportHblDto>> QueryListAsync(QueryHblDto query);
        Task<IList<OceanExportHblDto>> QueryListByMidAsync(QueryHblDto query);
        Task<CreateUpdateOceanExportHblDto> GetHblById(QueryHblDto query);
        Task<OceanExportHblDto> GetHblCardById(Guid Id);
        Task<List<OceanExportHblDto>> GetHblCardsById(Guid Id, bool isAsc = true, int sortType = 1, string ContainerId = "");
        Task<OceanExportDetails> GetOceanExportDetailsById(Guid Id);
        Task LockedOrUnLockedOceanExportHblAsync(QueryHblDto query);
        Task SetLockStatusOnOceanExportHblAsync(Guid[] ids, bool isLocked);
        Task DeleteMultipleHblsAsync(Guid[] ids);
        Task SaveAssignContainerToHblAsync(OceanExportHblAppModel AppModel, bool IsSave = true);
        Task SaveAssignContainerNoToHblAsync(OceanExportHblAppModel AppModel);
        Task SaveDeAssignContainerNoFromHblAsync(OceanExportHblAppModel AppModel);
        Task SaveAssignSingleContainerNoToHblAsync(OceanExportHblAppModel AppModel, bool IsSave = true);
        Task UpdateMblIdOfHblAsync(Guid hblId, Guid newMblId);
    }
}
