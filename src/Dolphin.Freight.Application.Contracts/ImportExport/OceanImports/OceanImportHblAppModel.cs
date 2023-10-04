using NPOI.SS.Formula.PTG;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ImportExport.OceanImports;
public class OceanImportHblAppModel
{
    public Guid[] Ids { get; set; }
    public string[] Containers { get; set; }
}