﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.AccountingSettings.CurrencyTables
{
    public class CreateUpdateCurrencyTableDto
    {
        /// <summary>
        /// 來源幣種
        /// </summary>
        public string Ccy1Id { get; set; }
        /// <summary>
        /// 來源幣種
        /// </summary>
        public string Ccy2Id { get; set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 匯率(內部)
        /// </summary>
        public double RateInternal { get; set; }
        /// <summary>
        /// 匯率(外部)
        /// </summary>
        public double RateInterna2 { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
