using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.Common
{
    public  interface IAjaxDropdownAppService:IApplicationService
    {
        Task<List<TradePartnerDto>> GetAllTradePartners(QueryDto query);
        Task<List<SysCodeDto>> GetSysCodeDtosByTypeAsync(QueryDto query);
        Task<List<SysCodeDto>> GetSysCodeByTypeAsync();
        Task<List<ReferenceItemDto>> GetReferenceItemsByTypeAsync(QueryDto query);
        Task<List<ReferenceItemDto>> GetReferenceMblAsync(QueryDto query);
        Task<List<ReferenceItemDto>> GetReferenceMawabAsync(QueryDto query);

    }
}
