using Dolphin.Freight.Common;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.Settings.Ports
{
    public class PortAppService :
        CrudAppService<
            Port, //IT號碼管理entity
            PortDto, //顯示IT號碼管理用
            Guid, //Primary key of the book entity
            QueryDto, //Used for paging/sorting
            CreateUpdatePortDto>, //新增修改IT號碼管理用
        IPortAppService //實作IPortAppService
    {
        private IRepository<Port, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        public PortAppService(IRepository<Port, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
        }

        public override async Task<PagedResultDto<PortDto>> GetListAsync(QueryDto input)
        {
            PagedResultDto<PortDto> listDto = new PagedResultDto<PortDto>();
            var query = await _repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrEmpty(input.Filter), x=>x.PortName
                                   .Contains(input.Filter) || x.SubDiv
                                   .Contains(input.Filter)).OrderBy(input.Sorting);

            var result = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            listDto.Items = ObjectMapper.Map<List<Port>, List<PortDto>>(result);
            listDto.TotalCount = query.Count();
            return listDto;
        }
        public async Task<PagedResultDto<PortDto>> QueryListAsync(QueryDto query)
        {
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.CodeValue);
                }
            }
            var Ports = await _repository.GetListAsync();
            List<Port> rs;
            List<PortDto> list = new List<PortDto>();
            rs = Ports;
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<Port, PortDto>(pu);
                    list.Add(pud);
                }
            }
            PagedResultDto<PortDto> listDto = new PagedResultDto<PortDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }
        public async Task<List<PortDto>> QueryListAllAsync(QueryDto query) 
        {
            var Ports = await _repository.GetListAsync();
            var list = new List<PortDto>();
            return list;
        }

    }
}
