using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ImportExport.OceanExports;

public class OceanExportHblAppModel
{
    public Guid MblId { get; set; }
    public Guid HblId { get; set; }
    public Guid[] HblIds { get; set; }
    public Guid[] Ids { get; set; }
    public string[] Containers { get; set; }
    public Guid[] ContainerId { get; set; }
    public List<Guid> ContainerIds { get; set; }
    public List<string> ContainerStrIds { get; set; }
    public List<int> ContainerNos { get; set; }
    public string Containersid { get; set; }
    public string ContainerNo { get; set; }
}
