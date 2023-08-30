using Dolphin.Freight.iFreightDB.FreightCenters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ReportLog;

public class VolumeReportSummaryViewModel
{
    public List<ShippingTypeList> ShippingTypeList { get; set; }
    public List<ContainerList> ContainerList { get; set; }
    public List<ShipModeList> ShipModeList { get; set; }
    public string Operator { get; set; }
}

public class ShippingTypeList
{
    public string Name { get; set; }
}

public class ContainerList
{
    public string Name { get;set;}
}
public class ShipModeList
{
    public string Name { get;set;}
}

