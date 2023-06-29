using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.AirExports
{
    public enum ReasonForCancelType
    {
        ByCustomer ,
        NoActivity = 1,
        NoLateGate,
        CarrierIssue,
        Others
    }
}
