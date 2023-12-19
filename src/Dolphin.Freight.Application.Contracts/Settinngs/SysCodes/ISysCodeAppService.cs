using Dolphin.Freight.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.Settinngs.SysCodes
{
    public interface ISysCodeAppService :
            ICrudAppService< 
            SysCodeDto, 
            Guid, 
            PagedAndSortedResultRequestDto, 
            CreateUpdateSysCodeDto> 
    {
        Task<List<SysCodeDto>> GetSysCodeDtosByTypeAsync(QueryDto query);
        Task<string> GetSystemNoAsync(QueryDto query);
        Task<string> GetSystemBookingNoAsync(QueryDto query);
        Task<List<SysCodeDto>> FillSysCodesAmsNo();
    }
}
