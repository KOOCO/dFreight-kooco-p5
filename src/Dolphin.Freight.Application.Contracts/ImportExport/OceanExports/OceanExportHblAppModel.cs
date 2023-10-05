using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ImportExport.OceanExports;

public class OceanExportHblAppModel
{
    public Guid[] Ids { get; set; }
    public string[] Containers { get; set; }
    public Guid[] ContainerId { get; set; }
}
