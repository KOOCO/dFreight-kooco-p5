
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin.Freight.Common.Memos
{
    public interface IMemoAppService
    {
        Task<MemoDto> GetAsync(Guid id);
        Task<List<MemoDto>> GetListByQueryAsync(QueryListDto dto);
        Task<bool> HasHighlightByQueryAsync(QueryHighlightDto dto);
        Task SaveAsync(CreateUpdateMemoDto dto);
        Task SwitchHighlightAsync(SwitchHighlightDto dto);
        Task DeleteAsync(Guid id);
        Task UpdateSourceIdAsync(Guid SourceId, FreightPageType fpageType);
    }
}
