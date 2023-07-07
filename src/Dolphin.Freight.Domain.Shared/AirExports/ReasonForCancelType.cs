using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.AirExports
{
    public enum ReasonForCancelType
    {
        ByCustomer = 1,
        NoActivity,
        NoLateGate,
        CarrierIssue,
        Others
    }
}
