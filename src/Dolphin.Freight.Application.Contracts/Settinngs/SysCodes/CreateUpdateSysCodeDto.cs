using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Settinngs.SysCodes
{
    public class CreateUpdateSysCodeDto : AuditedEntityDto<Guid>
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
