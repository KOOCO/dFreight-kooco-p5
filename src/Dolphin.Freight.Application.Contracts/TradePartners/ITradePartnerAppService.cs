using Dolphin.Freight.Common;
using Dolphin.Freight.TradePartners.Credits;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.TradePartners
{
    public interface ITradePartnerAppService :
        ICrudAppService<
            TradePartnerDto, // show
            Guid, // primary key
            QueryDto, // for paging & sorting
            CreateUpdateTradePartnerDto>, 
        IApplicationService
    {
        Task<TradePartnerDto> GetTPAsync(Guid id);
        Task<TradePartnerDto> CreateTPAsync(CreateUpdateTradePartnerDto input);
        Task UpdateTPAsync(Guid id, CreateUpdateTradePartnerDto input);
        Task<TradePartnerDto> FindByTpNameAsync(String tpName);
        Task<ListResultDto<TradePartnerLookupDto>> GetTradePartnersLookupAsync();
        Task<ListResultDto<TradePartnerLookupDto>> SearchTradePartnersLookupAsync(string filter);
        Task<ListResultDto<CountryLookupDto>> GetCountriesLookupAsync();
        Task<ListResultDto<CurrencyLookupDto>> GetCurrenciesLookupAsync();

        Task<string> FindByTpIdAsync(Guid id);


    }
}
