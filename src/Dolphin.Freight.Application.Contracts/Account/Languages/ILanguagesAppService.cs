using Dolphin.Freight.ImportExport.AirExports;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.Account.Languages
{
    public interface ILanguagesAppService: ICrudAppService<
            LanguagesDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateLanguageDto
        >
    {

    }
}
