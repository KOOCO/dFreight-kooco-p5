using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.TradePartners.TradeParties
{
    public interface ITradePartyAppService : ICrudAppService<TradePartyDto, Guid,  PagedAndSortedResultRequestDto, CreateUpdateTradePartyDto>
    {
        Task<TradePartyDto> GetAsync(Guid id);
        Task<List<TradePartyListDto>> GetListByPartnerIdAndTypeAsync(Guid id, TradePartyType type);
        Task SaveAsync(CreateUpdateTradePartyDto dto);
        Task DeleteAsync(Guid id);
        Task SwitchDefaultAsync(SwitchDefaultTradePartyDto dto);
        Task<List<CreateUpdateTradePartyDto>> GetListByTradePartnerId(Guid id);
    }
}
