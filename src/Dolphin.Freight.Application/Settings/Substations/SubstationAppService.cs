using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.Settings.Substations
{
    public class SubstationAppService :
        CrudAppService<
            Substation, //IT號碼管理entity
            SubstationDto, //顯示IT號碼管理用
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateSubstationDto>, //新增修改IT號碼管理用
        ISubstationAppService //實作ISubstationAppService
    {
        private IRepository<Substation, Guid> _repository;
        public SubstationAppService(IRepository<Substation, Guid> repository)
            : base(repository)
        {
            _repository = repository;
        }
        public async Task<List<SubstationDto>> GetSubstationsAsync(QueryDto query)
        {
            var Substations = await _repository.GetListAsync();
            var list = ObjectMapper.Map<List<Substation>, List<SubstationDto>>(Substations);
            return list;
        }
        public async Task<ListResultDto<SubstationLookupDto>> GetSubstationsLookupAsync()
        {
            var substationsQueryable = await _repository.GetQueryableAsync();
            var query = from substation in substationsQueryable
                        orderby substation.SubstationName
                        select substation;
            var substations = await AsyncExecuter.ToListAsync(query);
            return new ListResultDto<SubstationLookupDto>(ObjectMapper.Map<List<Substation>, List<SubstationLookupDto>>(substations));
        }
    }
}
