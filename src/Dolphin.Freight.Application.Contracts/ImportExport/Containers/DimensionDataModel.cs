using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ImportExport.Containers
{
    public class DimensionDataModel
    {
        public List<Dimension> Dimensions { get; set; }
        public Guid ContainerId { get; set; }
    }

    public class Dimension
    {
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Pcs { get; set; }
    }
}
