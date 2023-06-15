using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Account.Languages
{
    public class LanguagesDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
