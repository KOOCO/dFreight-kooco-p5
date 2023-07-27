using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;

namespace Dolphin.Freight.Settings.SysCodes
{
    /// <summary>
    /// 系統用參數，如那些下拉選單
    /// </summary>
    public class SysCode : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        /// <summary>
        /// 類別，下拉選單名稱
        /// </summary>
        public string CodeType { get; set; }
        /// <summary>
        /// 選單值
        /// </summary>
        public string CodeValue { get; set; }
        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string ShowName { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public Guid? ParentId { get; set; }
    }
}
