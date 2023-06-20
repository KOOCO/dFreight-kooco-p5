using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.Account.Languages
{
    public class LanguagesAppService :
        CrudAppService<
            Language,
            LanguagesDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateLanguageDto>,
        ILanguagesAppService
    {
        public LanguagesAppService(IRepository<Language, Guid> repository) : base(repository) { }


    }
}
