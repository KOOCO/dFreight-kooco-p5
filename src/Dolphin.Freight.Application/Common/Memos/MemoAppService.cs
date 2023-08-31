using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace Dolphin.Freight.Common.Memos
{
    public class MemoAppService : ApplicationService, IMemoAppService
    {
        private readonly IRepository<Memo, Guid> _repository;
        private IRepository<IdentityUser, Guid> _userRepository;

        public MemoAppService(IRepository<Memo, Guid> repository, IRepository<IdentityUser, Guid> userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<MemoDto> GetAsync(Guid id)
        {
            Memo entity = await _repository.GetAsync(id);

            return ObjectMapper.Map<Memo, MemoDto>(entity);
        }

        public async Task<List<MemoDto>> GetListByQueryAsync(QueryListDto dto)
        {
            IQueryable<Memo> query = await _repository.GetQueryableAsync();
            IQueryable<IdentityUser> userQuery = await _userRepository.GetQueryableAsync();

            var rows = from memo in query
                       where memo.SourceId == dto.SourceId && memo.FType.Equals(dto.FType)
                       join user1 in userQuery
                       on memo.CreatorId equals user1.Id
                       into User1
                       from u1 in User1.DefaultIfEmpty()
                       join user2 in userQuery
                       on memo.LastModifierId equals user2.Id
                       into User2
                       from u2 in User2.DefaultIfEmpty()
                       select new { Memo = memo, U1 = u1.Name, U2 = u2.Name };

            List<MemoDto> result = new();
            foreach (var row in rows.ToList())
            {
                MemoDto memoDto = ObjectMapper.Map<Memo, MemoDto>(row.Memo);
                memoDto.CreatedUserName = row.U1;
                memoDto.LastUpdatedUserName = row.U2;

                result.Add(memoDto);
            }

            return result;
        }

        public async Task<bool> HasHighlightByQueryAsync(QueryHighlightDto dto)
        {
            Memo entity = await _repository.FirstOrDefaultAsync(row => 
                row.SourceId.Equals(dto.SourceId) && 
                row.FType == dto.FType && 
                row.Highlight
            );

            return entity != null;
        }

        public async Task SaveAsync(CreateUpdateMemoDto dto)
        {
            Memo entity = dto.Id == null ? new() { Highlight = false } : await _repository.GetAsync(dto.Id.Value);
            entity.FType = dto.FType;
            entity.Subject = dto.Subject;
            entity.Content = dto.Content;
            entity.SourceId = dto.SourceId;

            if (dto.Id == null)
            {
                await _repository.InsertAsync(entity);
            }
            else
            {
                await _repository.UpdateAsync(entity);
            }
        }
        public async Task UpdateSourceIdAsync(Guid SourceId,FreightPageType fpageType)
        {
            var data = await _repository.GetQueryableAsync();
            var list = data.Where(x => x.SourceId == Guid.Empty && x.FType == fpageType).ToList();
            foreach (var item in list)
            { 
            item.SourceId= SourceId;
                await _repository.UpdateAsync(item);
            
            
            }
        }


        public async Task SwitchHighlightAsync(SwitchHighlightDto dto)
        {
            Memo entity = await _repository.GetAsync(dto.Id);
            entity.Highlight = dto.Highlight;

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
