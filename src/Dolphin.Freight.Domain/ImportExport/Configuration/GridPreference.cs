using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Dolphin.Freight.ImportExport.Configuration
{
    public class GridPreference : BasicAggregateRoot<Guid>
    {
        public string Preference { get; set; }
        public string PreferenceSrc { get; set; }
        public Guid UserId { get; set; }
    }
}
