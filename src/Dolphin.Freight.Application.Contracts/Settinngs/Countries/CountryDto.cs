using System;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Settings.Countries
{
    public class CountryDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 國家名稱
        /// </summary>
        public string CountryName { get; set; }
        public string Code { get; set; }
    }
}
