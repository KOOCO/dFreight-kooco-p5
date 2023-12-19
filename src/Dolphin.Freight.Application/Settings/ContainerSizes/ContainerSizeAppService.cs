using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Common;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.ContainerSizes;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.ContainerSizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.Settings.ContainerSizes
{
    public class ContainerSizeAppService :
        CrudAppService<
            ContainerSize, //IT號碼管理entity
            ContainerSizeDto, //顯示IT號碼管理用
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateContainerSizeDto>, //新增修改IT號碼管理用
        IContainerSizeAppService //實作IContainerSizeAppService
    {
        private IRepository<ContainerSize, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        public ContainerSizeAppService(IRepository<ContainerSize, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            GetPolicyName = SettingsPermissions.ContainerSizes.Default;
            GetListPolicyName = SettingsPermissions.ContainerSizes.Default;
            CreatePolicyName = SettingsPermissions.ContainerSizes.Create;
            UpdatePolicyName = SettingsPermissions.ContainerSizes.Edit;
            DeletePolicyName = SettingsPermissions.ContainerSizes.Delete;
        }
        public async Task<PagedResultDto<ContainerSizeDto>> QueryListAsync(QueryDto query)
        {
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.ShowName);
                }
            }
            //var ContainerSizes = await _repository.GetListAsync();
            var ContainerSizes = await _repository.GetQueryableAsync();
            List<ContainerSize> rs;
            List<ContainerSizeDto> list = new List<ContainerSizeDto>();

            ContainerSizes = ContainerSizes.WhereIf(query.Active != null, x => x.IsUseed == query.Active)
                                           .WhereIf(!string.IsNullOrWhiteSpace(query.Filter), x => x.SizeDescription.ToLower()
                                           .Contains(query.Filter.ToLower()) || x.ContainerCode.Contains(query.Filter.ToLower()));


            if (query != null && query.QueryKey != null)
            {
                rs = ContainerSizes.Where(x => x.SizeDescription.Contains(query.QueryKey) || x.ContainerCode.Contains(query.QueryKey) || x.ContainerGroup.CodeValue.Contains(query.QueryKey)).ToList();
            }
            else
            {
                rs = ContainerSizes.ToList();
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<ContainerSize, ContainerSizeDto>(pu);
                    if (pu.ContainerGroupId != null) pud.ContainerGroup = dictionary[pu.ContainerGroupId.Value];
                    if (pu.AmsTypeCodeId is not null) pud.AmsTypeCode = dictionary[pu.AmsTypeCodeId.Value];
                    list.Add(pud);
                }
            }
            PagedResultDto<ContainerSizeDto> listDto = new PagedResultDto<ContainerSizeDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }
        public async Task<List<ContainerSizeDto>> QueryAllListAsync(QueryDto query) 
        {
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.ShowName);
                }
            }
            var ContainerSizes = await _repository.GetListAsync();
            List<ContainerSize> rs;
            List<ContainerSizeDto> list = new List<ContainerSizeDto>();
            if (query != null && query.QueryKey != null)
            {
                rs = ContainerSizes.Where(x => x.SizeDescription.Contains(query.QueryKey) || x.ContainerCode.Contains(query.QueryKey) || x.ContainerGroup.CodeValue.Contains(query.QueryKey)).ToList();
            }
            else
            {
                rs = ContainerSizes;
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<ContainerSize, ContainerSizeDto>(pu);
                    if (pu.ContainerGroupId != null) pud.ContainerGroup = dictionary[pu.ContainerGroupId.Value];
                    list.Add(pud);
                }
            }
            return list;
        }

    }
}
