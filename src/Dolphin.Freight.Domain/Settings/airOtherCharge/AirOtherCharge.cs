﻿using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dolphin.Freight.Settings.AirOtherCharge
{
    /// <summary>
    /// 空運出口其他費用Entity
    /// </summary>
    public class AirOtherCharge : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        /// <summary>
        /// 航空公司/代理
        /// </summary>
        public string companyType { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string paymentType { get; set; }

        /// <summary>
        /// 顯示於MAWB
        /// </summary>
        public Boolean showOnMAWB { get; set; }

        /// <summary>
        /// 顯示於HAWB
        /// </summary>
        public Boolean showOnHAWB { get; set; }

        /// <summary>
        /// 收費項目
        /// </summary>
        public string chargeItem { get; set; }

        /// <summary>
        /// 收費細項
        /// </summary>
        public string chargeItemSubitem { get; set; }

        /// <summary>
        /// 收費費率
        /// </summary>
        public int chargeRate { get; set; }

        /// <summary>
        /// 收費費率單位
        /// </summary>
        public string chargeRateUnit { get; set; }

        /// <summary>
        /// 最低收費
        /// </summary>
        public string minPrice { get; set; }

        /// <summary>
        /// Defined by ISoftDelete
        /// </summary>
        public bool IsDeleted { get; set; }
    
    }
}

