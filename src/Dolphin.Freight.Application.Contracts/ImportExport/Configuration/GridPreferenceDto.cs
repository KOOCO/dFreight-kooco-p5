using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.Configuration
{
    public class GridPreferenceDto : AuditedEntityDto<Guid>
    {
        public string Preference { get; set; }
        public string PreferenceSrc { get; set; }
        public Guid UserId { get; set; }
    }
}
