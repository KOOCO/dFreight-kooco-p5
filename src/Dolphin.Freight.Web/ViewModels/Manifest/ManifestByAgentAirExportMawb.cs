using Org.BouncyCastle.Asn1.BC;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.Manifest
{
    public class ManifestByAgentAirExportMawb
    {
        public List<OverSeaAgent> OverSeaAgents { get; set; }
    }

    public class OverSeaAgent
    {
        public string Name { get; set; }
    }
}
