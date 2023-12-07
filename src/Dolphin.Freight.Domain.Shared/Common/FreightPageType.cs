using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.Common
{
    public enum FreightPageType
    {
        OIMBL = 1, // 海運進口MBL
        OIHBL, // 海運進口HBL
        OEMBL, // 海運出口MBL
        OEHBL, // 海運出口HBL
        AIMBL, // 空運進口MBL
        AIHBL, // 空運進口HBL
        AEMBL, // 空運出口MBL
        AEHBL, // 空運出口HBL
        SO, // S/O
        VS, // 船期
        TP, // 貿易夥伴
        GA,
        RP, // Receive Payment
        AR, // Invoice AR
        AP, // Invoice AP
        DC // Invoice DC
    }
}
