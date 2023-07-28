using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.TradePartners
{
    public class CreateUpdateTradePartnerMemoDto : AuditedEntityDto<Guid>
    {
        public new Guid? Id { get; set; }
        public Guid TradePartnerId { get; set; }
        [Required(ErrorMessage = "Subject is Required")]
        [Display(Name = "Display:Memo:THead:Title")]
        public string Title { get; set; }
        public string Memo { get; set; }
    }
}
