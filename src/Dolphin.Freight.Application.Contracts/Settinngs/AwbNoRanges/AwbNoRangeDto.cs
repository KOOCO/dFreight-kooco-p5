using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Settings.AwbNoRanges
{
    /// <summary>
    /// Awb號碼管理DTO
    /// </summary>
    public class AwbNoRangeDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 起始號碼
        /// </summary>
        public string StartNo { get; set; }

        /// <summary>
        /// 結束號碼
        /// </summary>
        public string EndNo { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 當前分配的號碼
        /// </summary>
        public string CurrentNo { get; set; }
        public string Carrier { get; set; }

        public Guid CompanyId { get; set; } 
    }
}