using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Dolphin.Freight.TradePartners
{
    public class TradePartnerMemoAppService : ApplicationService, ITradePartnerMemoAppService
    {
        private IRepository<TradePartnerMemo, Guid> _repository;
        private IRepository<IdentityUser, Guid> _userRepository;

        public TradePartnerMemoAppService(IRepository<TradePartnerMemo, Guid> repository, IRepository<IdentityUser, Guid> userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<TradePartnerMemoDto> GetAsync(Guid id)
        {
            TradePartnerMemo entity = await _repository.GetAsync(id);

            return ObjectMapper.Map<TradePartnerMemo, TradePartnerMemoDto>(entity);
        }

        public async Task<List<TradePartnerMemoDto>> GetListByTradePartnerIdAsync(Guid id)
        {
            IQueryable<TradePartnerMemo> query = await _repository.GetQueryableAsync();
            IQueryable<IdentityUser> userQuery = await _userRepository.GetQueryableAsync();

            var rows = from memo in query
                       where memo.TradePartnerId == id
                       join user1 in userQuery
                       on memo.CreatorId equals user1.Id
                       into User1
                       from u1 in User1.DefaultIfEmpty()
                       join user2 in userQuery
                       on memo.LastModifierId equals user2.Id
                       into User2
                       from u2 in User2.DefaultIfEmpty()
                       select new { Memo = memo, U1 = u1.Name, U2 = u2.Name };

            List<TradePartnerMemoDto> result = new();
            foreach (var row in rows.ToList())
            {
                TradePartnerMemoDto dto = ObjectMapper.Map<TradePartnerMemo, TradePartnerMemoDto>(row.Memo);
                dto.CreatedUserName = row.U1;
                dto.LastUpdatedUserName = row.U2;

                result.Add(dto);
            }

            return result;
        }

        public async Task<bool> HasHighlightAsync(Guid id)
        {
            TradePartnerMemo entity = await _repository.FirstOrDefaultAsync(row => row.TradePartnerId.Equals(id) && row.Highlight);
            return entity != null;
        }

        public async Task SaveAsync(CreateUpdateTradePartnerMemoDto dto)
        {
            TradePartnerMemo entity = dto.Id == null ? new() { Highlight = false } : await _repository.GetAsync(dto.Id.Value);

            entity.TradePartnerId = dto.TradePartnerId;
            entity.Title = dto.Title;
            entity.Memo = dto.Memo;

            if (dto.Id == null)
            {
                await _repository.InsertAsync(entity);
            }
            else
            {
                await _repository.UpdateAsync(entity);
            }
        }

        public async Task SwitchHighlightAsync(SwitchHighlightTradePartnerMemoDto dto)
        {
            TradePartnerMemo entity = await _repository.GetAsync(dto.Id);
            entity.Highlight = dto.Highlight;

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
