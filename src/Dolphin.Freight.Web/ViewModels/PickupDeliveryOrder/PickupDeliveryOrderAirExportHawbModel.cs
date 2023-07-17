using System;

namespace Dolphin.Freight.Web.ViewModels.PickupDeliveryOrder
{
    public class PickupDeliveryOrderAirExportHawbModel
    {
        public string Issued_At { get; set; }
        public string Issued_By { get; set; }
        public string Trucker { get; set; }
        public string Mawb_No { get; set; }
        public string Hawb_No { get; set; }
        public string Our_Ref_No { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Carrier { get; set; }
        public string Flight_No { get; set; }
        public string Pickup_Location { get; set; }
        public string Port_Of_Loading { get; set; }
        public string ETD { get; set; }
        public string Port_Of_Discharge { get; set; }
        public string ETA { get; set; }
        public string Delivery_To { get; set; }
        public string Packages { get; set; }
        public string Measurement { get; set; }
        public string BaseUrl { get; set; }
        public Guid ReportId { get; set; }
    }
}
