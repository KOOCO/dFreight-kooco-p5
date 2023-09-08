using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using Dolphin.Freight.TradePartner;
using Dolphin.Freight.TradePartners;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Dolphin.Freight
{
    public class CreateUpdateContactPersonDto : AuditedEntityDto<Guid>
    {
        public bool IsRep { get; set; }
        public bool IsEmailNotification { get; set; }
        [Required]
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactDivision { get; set; }
        public string ContactCellPhone { get; set; }
        public string ContactPhone { get; set; }
        public string ContactFax { get; set; }
        public string ContactEmailAddress { get; set; }
        public string ContactRemark { get; set; }
        public GenderType? ContactGender { get; set; }
        public MarriageType? ContactMarriage { get; set; }
        public SmokesType? ContactSmokes { get; set; }
        public DrinkType? ContactDrink { get; set; }
        public int? ContactAge { get; set; }
        public string ContactGarment { get; set; }
        public string ContactHobby { get; set; }
        public string ContactInterest { get; set; }
        public ConstellationType? ContactConstellation { get; set; }
        public DateTime? ContactMemorialDay { get; set; }
        public DateTime? ContactBirthday { get; set; }
        public Guid? ContactCountryId { get; set; }
        public string ContactCityCode { get; set; }
        public string ContactStateCode { get; set; }
        public string ContactPostCode { get; set; }
        public string ContactAddress { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public Guid TradePartnerId { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
    }
}
