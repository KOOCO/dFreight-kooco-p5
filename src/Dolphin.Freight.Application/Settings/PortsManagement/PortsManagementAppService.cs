using Dolphin.Freight.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.Settings.PortsManagement
{
    public class PortsManagementAppService :
        CrudAppService<
            PortsManagement, //港口管理entity
            PortsManagementDTO, //顯示港口管理用
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdatePortsManagementDto>, //新增修改港口管理用
        IPortsManagementAppService //實作PortsManagementAppService
    {
        public PortsManagementAppService(IRepository<PortsManagement, Guid> repository)
            : base(repository)
        {
            GetPolicyName = SettingsPermissions.PortsManagement.Default;
            GetListPolicyName = SettingsPermissions.PortsManagement.Default;
            CreatePolicyName = SettingsPermissions.PortsManagement.Create;
            UpdatePolicyName = SettingsPermissions.PortsManagement.Edit;
            DeletePolicyName = SettingsPermissions.PortsManagement.Delete;
        }

        public async Task<List<PortsManagementDTO>> QueryListAsync()
        {
            var ports = await Repository.GetListAsync();
            var list = ObjectMapper.Map<List<PortsManagement>, List< PortsManagementDTO >> (ports);

            return list;
        }
    }
}
