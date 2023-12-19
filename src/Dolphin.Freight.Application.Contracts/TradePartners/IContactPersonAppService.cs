using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.TradePartners
{
    public interface IContactPersonAppService :
        ICrudAppService<
            ContactPersonDto,
            Guid,
            ContactPersonPagedAndSortedResultRequestDto,
            CreateUpdateContactPersonDto>,
        IApplicationService
    {
        Task<ContactPersonDto> CreateContactPersonAsync(CreateUpdateContactPersonDto input);
        Task DeleteContactPersonAsync(Guid id);
        Task UpdateContactPersonAsync(Guid id, CreateUpdateContactPersonDto input);
        Task SwitchRepAsync(SwitchRepContactPersonDto dto);
        Task<List<CreateUpdateContactPersonDto>> GetListByTradePartnerId(Guid id);

    }
}
