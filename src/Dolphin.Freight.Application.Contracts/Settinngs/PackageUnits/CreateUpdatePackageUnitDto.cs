using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dolphin.Freight.Settinngs.PackageUnits
{
    public class CreateUpdatePackageUnitDto
    {
        [MaxLength(3)]
        [Required]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Code must be numeric")]
        /// <summary>
        /// 代碼
        /// </summary>
        public string PackageCode { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        public string PackageName { get; set; }
        /// <summary>
        /// Ams編號ID
        /// </summary>
        public Guid? AmsNoId { get; set; }
        /// <summary>
        /// EManifest ID
        /// </summary>
        public Guid? EManifestId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
