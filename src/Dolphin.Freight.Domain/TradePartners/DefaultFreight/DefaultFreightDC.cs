﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using Dolphin.Freight.TradePartners.TradeParties;
using System.ComponentModel.DataAnnotations.Schema;
using Dolphin.Freight.Accounting;
using Dolphin.Freight.AccountingSettings.BillingCodes;

namespace Dolphin.Freight.TradePartners.DefaultFreight
{
    public class DefaultFreightDC : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        
        public Guid TradePartnerId { get; set; }

        public DefaultFreightCategory Category { get; set; }

        public Guid FreightCode { get; set; }

        public string FreightDescription { get; set; }

        public PCType PCType { get; set; }

        public DefaultFreightDCType Type { get; set; }

        public UnitType Unit { get; set; }

        public double Vol { get; set; }

        public double Rate { get; set; }

        public double AgentAmount { get; set; }

        public bool? ShipModeFCL { get; set; }

        public bool? ShipModeLCL { get; set; }

        public bool? ShipModeFAK { get; set; }

        public bool? ShipModeBULK { get; set; }

        [ForeignKey("TradePartnerId")]
        public TradePartner TradePartner { get; set; }

        [ForeignKey("FreightCode")]
        public BillingCode BillingCode { get; set; }

        public bool IsDeleted { get; set; }
    }
}
