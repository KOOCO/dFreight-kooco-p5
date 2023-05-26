﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.Containers
{
    public class ContainerDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 集裝箱號
        /// </summary>
        public string ContainerNo { get; set; }
        /// <summary>
        /// 櫃型/尺寸ID
        /// </summary>
        public Guid ContainerSizeId { get; set; }

        /// <summary>
        /// 封條號碼
        /// </summary>
        public string SealNo { get; set; }
        /// <summary>
        /// 包裝種類數量
        /// </summary>
        public int PackageNum { get; set; }
        /// <summary>
        /// 包裝重量
        /// </summary>
        public double PackageWeight { get; set; }
        /// <summary>
        /// 包裝材積
        /// </summary>
        public double PackageMeasure { get; set; }
        /// <summary>
        /// 封條號碼2
        /// </summary>
        public string SealNo2 { get; set; }
        /// <summary>
        /// 提單號碼
        /// </summary>
        public string PicupNo { get; set; }
        /// <summary>
        /// 是否危險物品
        /// </summary>
        public bool IsDanger { get; set; }
        /// <summary>
        /// 儲存開始日期
        /// </summary>
        public DateTime StorageStartDate { get; set; }
        /// <summary>
        /// 儲存結束日期
        /// </summary>
        public DateTime StorageEndDate { get; set; }
        /// <summary>
        /// 溫度單位
        /// </summary>
        public string TemperatureUnit { set; get; }
        /// <summary>
        /// 溫度
        /// </summary>
        public double Temperature { set; get; }
        /// <summary>
        /// 通風ID
        /// </summary>
        public Guid? VentilationId { get; set; }
        
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 貨櫃進場日期時間
        /// </summary>
        public DateTime GateInDate { get; set; }
        /// <summary>
        /// 結關日期時間
        /// </summary>
        public DateTime CutOffDate { get; set; }
        /// <summary>
        /// 裝櫃日期
        /// </summary>
        public DateTime LoadingDate { get; set; }
        /// <summary>
        ///  已裝船日期時間
        /// </summary>
        public DateTime LoadedOnVesselDate { get; set; }
        /// <summary>
        /// 船公司是否放貨
        /// </summary>
        public bool IsCarrierRelease { get; set; }
        /// <summary>
        /// 海關是否放貨
        /// </summary>
        public bool IsCustomsClearance { get; set; }
        /// <summary>
        /// 已從船隻卸貨日期時間
        /// </summary>
        public DateTime UvDate { get; set; }
        /// <summary>
        /// 場內延滯免費期
        /// </summary>
        public DateTime LastFreeDate { get; set; }
        /// <summary>
        /// 櫃場地點
        /// </summary>
        public string YardLocation { get; set; }
        /// <summary>
        /// 預計提貨(櫃)時間
        /// </summary>
        public DateTime ApptDate { get; set; }
        /// <summary>
        /// 提出碼頭時間
        /// </summary>
        public DateTime GateOutDate { get; set; }
        /// <summary>
        /// 場外滯留免費期
        /// </summary>
        public DateTime FreeDetentionDate { get; set; }
        /// <summary>
        /// 送貨到府ETA 
        /// </summary>
        public DateTime EstimateDeliveryDate { get; set; }
        /// <summary>
        /// ATA(已送貨到府) 
        /// </summary>
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// 空櫃返還日期
        /// </summary>
        public DateTime EmptyReturnDate { get; set; }
        /// <summary>
        /// 是否可提櫃
        /// </summary>
        public bool IsAvailableForPickup { get; set; }
        public bool IsPP { get; set; }
        public bool IsCTF { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
