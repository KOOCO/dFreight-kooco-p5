using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.Settings.Countries
{
    public interface ICountryAppService : IApplicationService
    {
        Task<List<CountryDto>> GetListAsync();
        Task<ListResultDto<CountryDto>> GetCountryLookupAsync();
    }
}