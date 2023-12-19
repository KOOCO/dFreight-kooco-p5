using Dolphin.Freight.Common;
using Dolphin.Freight.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;

namespace Dolphin.Freight.Settings.PortsManagement
{
    public class PortsManagementAppService :
        CrudAppService<
            PortsManagement, //港口管理entity
            PortsManagementDTO, //顯示港口管理用
            Guid, //Primary key of the book entity
            QueryDto, //Used for paging/sorting
            CreateUpdatePortsManagementDto>, //新增修改港口管理用
        IPortsManagementAppService //實作PortsManagementAppService
    {
        private readonly IRepository<PortsManagement, Guid> _repository;
        public PortsManagementAppService(IRepository<PortsManagement, Guid> repository)
            : base(repository)
        {
            GetPolicyName = SettingsPermissions.PortsManagement.Default;
            GetListPolicyName = SettingsPermissions.PortsManagement.Default;
            CreatePolicyName = SettingsPermissions.PortsManagement.Create;
            UpdatePolicyName = SettingsPermissions.PortsManagement.Edit;
            DeletePolicyName = SettingsPermissions.PortsManagement.Delete;
            _repository = repository;
        }
        public override async Task<PagedResultDto<PortsManagementDTO>> GetListAsync(QueryDto input)
        {
            var query = await _repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Search), x => x.Country
                         .Contains(input.Search) || x.PortName
                         .Contains(input.Search) || x.Locode
                         .Contains(input.Search) || x.SubDiv
                         .Contains(input.Search));
            var resultList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return new PagedResultDto<PortsManagementDTO>
            {
                Items = ObjectMapper.Map<List<PortsManagement>, List<PortsManagementDTO>>(resultList) ,
                TotalCount = query.Count()
            };
            
        }

        public async Task<List<PortsManagementDTO>> QueryListAsync()
        {
            var ports = await Repository.GetListAsync();
            var list = ObjectMapper.Map<List<PortsManagement>, List< PortsManagementDTO >> (ports);

            return list;
        }
    }
}
