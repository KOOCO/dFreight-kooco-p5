using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Data;

namespace Dolphin.Freight.TradePartners.TradeParties
{
    public class TradeParty: AuditedAggregateRoot<Guid>, ISoftDelete
    {   
        public TradePartyType TradePartyType { get; set; }
        public bool IsDefault { get; set; }
        public string TradePartyDescription { get; set; }
        public Guid TradePartnerId { get; set; }
        [ForeignKey("TradePartnerId")]
        public TradePartner TradePartner { get; set; }
        public Guid TargetTradePartnerId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
