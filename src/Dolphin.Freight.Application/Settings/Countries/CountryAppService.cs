using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.Settings.Countries
{
    public class CountryAppService : ApplicationService, ICountryAppService
    {
        private IRepository<Country, Guid> _repository;

        public CountryAppService(IRepository<Country, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<List<CountryDto>> GetListAsync()
        {
            return (await _repository.GetListAsync()).Select(row => ObjectMapper.Map<Country, CountryDto>(row)).ToList();
        }
        public async Task<ListResultDto<CountryDto>> GetCountryLookupAsync()
        {
            IQueryable<Country> countriesQueryable = await _repository.GetQueryableAsync();
            var query = from country in countriesQueryable
                        orderby country.CountryCode
                        select country;
            var countries = query.ToList();
            return new ListResultDto<CountryDto>(
                ObjectMapper.Map<List<Country>, List<CountryDto>>(countries)
            );
        }
    }
}
