using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Settinngs.ContainerSizes
{
    public class ContainerSizeDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 集裝箱清單的ID
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
        public string ContainerGroup { get; set; }
        public string AmsTypeCode { get; set; }
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

