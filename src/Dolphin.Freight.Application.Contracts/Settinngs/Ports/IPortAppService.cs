using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.Ports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.Settinngs.Ports
{
    public interface IPortAppService :
        ICrudAppService< 
        PortDto, 
        Guid,
        QueryDto, 
        CreateUpdatePortDto>  
    {
        Task<PagedResultDto<PortDto>> QueryListAsync(QueryDto query);
        Task<List<PortDto>> QueryListAllAsync(QueryDto query);
    }
}
