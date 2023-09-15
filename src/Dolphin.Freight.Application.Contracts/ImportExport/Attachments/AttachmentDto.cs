using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.Attachments
{
    public class AttachmentDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string ShowName { get; set; }
        /// <summary>
        /// 檔案大小
        /// </summary>
        public double Size { get; set; }
        /// <summary>
        /// 對應的文件編號
        /// </summary>
        public Guid Fid { get; set; }
        /// <summary>
        /// 對應的文件類別
        /// 0：海運進口MBL，1：海運進口HBL、2海運出口MBL、3海運出口HBL，4空運進口MBL，5空運進口HBL，6空運出口MBL，7空運出口HBL，8S/O，9船期，10貿易夥伴
        /// </summary>
        public int Ftype { get; set; }
        /// <summary>
        /// 是否備忘錄的附件
        /// </summary>
        public bool IsMemo { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public string CreatorName { get; set; }
    }
}
