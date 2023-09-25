using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ImportExport.OceanImports
{
    public interface IOceanImportHblAppService :
        ICrudAppService< 
        OceanImportHblDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateOceanImportHblDto> 
    {
        Task<PagedResultDto<OceanImportHblDto>> QueryListAsync(QueryHblDto query);
        Task<IList<OceanImportHblDto>> QueryListByMidAsync(QueryHblDto query);
        Task<CreateUpdateOceanImportHblDto> GetHblById(QueryHblDto query);
        Task<List<OceanImportHblDto>> GetHblCardsById(Guid Id);
        Task<OceanImportHblDto> GetHblCardById(Guid Id);
        Task<OceanImportDetails> GetOceanImportDetailsById(Guid Id);
        Task DeleteMultipleHblsAsync(Guid[] Ids);
    }
}
