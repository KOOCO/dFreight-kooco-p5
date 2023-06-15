using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Dolphin.Freight.Account.Languages
{
    public class Language : BasicAggregateRoot<Guid>
    {
        public string Name { get; set; }
    }
}
