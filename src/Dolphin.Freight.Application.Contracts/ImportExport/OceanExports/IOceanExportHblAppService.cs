using Dolphin.Freight.Common;
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
        Task<PagedResultDto<OceanExportHblDto>> QueryListAsync(QueryHblDto query);
        Task<IList<OceanExportHblDto>> QueryListByMidAsync(QueryHblDto query);
        Task<CreateUpdateOceanExportHblDto> GetHblById(QueryHblDto query);
        Task<OceanExportHblDto> GetHblCardById(Guid Id);
        Task<List<OceanExportHblDto>> GetHblCardsById(Guid Id);
    }
}
