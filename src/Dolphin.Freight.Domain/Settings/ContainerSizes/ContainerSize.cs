using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using Dolphin.Freight.Settings.SysCodes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dolphin.Freight.Settings.ContainerSizes
{
    /// <summary>
    /// 集裝箱櫃型/尺寸
    /// </summary>
    public class ContainerSize : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        /// <summary>
        /// 集裝箱清單代碼
        /// </summary>
        public string ContainerCode { get; set; }
        /// <summary>
        /// 貨描
        /// </summary>
        public string SizeDescription { get; set; }
        /// <summary>
        /// 類別
        /// </summary>
        public Guid? ContainerGroupId { get; set; }
        public Guid? AmsTypeCodeId { get; set; }
        /// <summary>
        /// 類別
        /// </summary>
        [ForeignKey("ContainerGroupId")]
        public SysCode ContainerGroup { get; set; }
        [ForeignKey("AmsTypeCodeId")]
        public SysCode AmsTypeCode { get; set; }
        /// <summary>
        /// 長度
        /// </summary>
        public double Teu { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUseed { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
