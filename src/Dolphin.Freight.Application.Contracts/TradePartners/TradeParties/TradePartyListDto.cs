using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.TradePartners.TradeParties
{
    public class TradePartyListDto : AuditedEntityDto<Guid>
    {
        public Guid TradePartnerId { get; set; }
        public string TradePartyDescription { get; set; }
        public Guid TargetTradePartnerId { get; set; }
        public bool IsDefault { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public Guid? ContactPersonId { get; set; }
        public bool? IsRep { get; set; }
        public string Contact { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
}
