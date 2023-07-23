using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;

namespace Dolphin.Freight.Settings.AwbNoRanges
{   /// <summary>
    /// 新增修改Awb號碼管理DTO
    /// </summary>
    public class CreateUpdateAwbNoRangeDto
    {
        /// <summary>
        /// 起始號碼
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 起始號碼
        /// </summary>
        [Required]
        [StringLength(8)]
        public string StartNo { get; set; }

        /// <summary>
        /// 結束號碼
        /// </summary>
        [Required]
        [StringLength(8)]
        public string EndNo { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Note { get; set; }
    }
}