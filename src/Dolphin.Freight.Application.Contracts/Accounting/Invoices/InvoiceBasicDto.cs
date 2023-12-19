using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.Accounting.Invoices
{
    public class InvoiceBasicDto
    {
        /// <summary>
        /// 託運人
        /// </summary>
        public string ShipperName { get; set; } = "";
        /// <summary>
        /// 收貨人
        /// </summary>
        public string MblConsigneeName { get; set; } = "";
        /// <summary>
        /// 分站縮寫
        /// </summary>
        public string AbbreviationName { get; set; } = "";
        /// <summary>
        /// 通知方
        /// </summary>
        public string MblNotifyName { get; set; } = "";
        /// <summary>
        /// 通知方
        /// </summary>
        public string VesselNameVoyage { get; set; } = "";
        /// <summary>
        /// 負責操作人員
        /// </summary>
        public string MblOperatorName { get; set; } = "";
        public string PackageUnitName { get; set; } = "";
    }
}
