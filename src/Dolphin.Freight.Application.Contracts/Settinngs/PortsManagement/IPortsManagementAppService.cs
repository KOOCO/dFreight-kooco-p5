using Dolphin.Freight.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Threading;

namespace Dolphin.Freight.Settings.PortsManagement
{
    /// <summary>
    /// IT號碼管理interface
    /// </summary>
    public interface IPortsManagementAppService :
            ICrudAppService< //Defines CRUD methods
            PortsManagementDTO, //顯示IT號碼管理用
            Guid, //Primary key of the book entity
            QueryDto, //Used for paging/sorting
            CreateUpdatePortsManagementDto> //新增修改IT號碼管理用
    {
        Task<List<PortsManagementDTO>> QueryListAsync();
    }
}
