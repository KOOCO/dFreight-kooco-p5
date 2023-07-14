using System;

namespace Dolphin.Freight.Web.ViewModels.Reports
{
    public class DangerousGoodsViewModel
    {
        public Guid HawbId { get; set; }
        public Guid MawbId { get; set; }
        public string Shipper { get; set; }
        public string AirWayBillNo { get; set; }
        public string Consignee { get; set; }
        public string DepartureName { get; set; }
        public string DestinationName { get; set; }
        public string NameOfSignatory { get; set; }
        public string PodEtd { get; set; }
    }
}
