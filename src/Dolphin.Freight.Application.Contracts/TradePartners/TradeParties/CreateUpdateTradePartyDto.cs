using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Dolphin.Freight.TradePartners.TradeParties
{
    public class CreateUpdateTradePartyDto : AuditedEntityDto<Guid>
    {
        public Guid TradePartnerId { get; set; }
        public Guid TargetTradePartnerId { get; set; }
        public TradePartyType TradePartyType { get; set; }
        public string TradePartyDescription { get; set; }
        public bool? IsDefault { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
        public TradePartyInfo TradePartyListDto { get; set; } = new TradePartyInfo();
    }
}
