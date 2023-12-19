using Dolphin.Freight.Web.ViewModels.BookingPackingList;
using System;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.PackagingListAirExportHawb
{
    public class PackagingListAirExportHawb
    {
        public Guid ReportId { get; set; }
        public string BaseUrl { get; set; }
        public string Shipper_name { get; set; }
        public string Notify_party { get; set; }
        public string Consignee_address { get; set; }
        public string Inv_no { get; set; }
        public string Inv_date { get; set; }
        public string POL_location { get; set; }
        public string POD_location { get; set; }
        public string Issue_date { get; set; }
        public string Departure_date { get; set; }
        public string Issue_bank { get; set; }
        public string LC_no { get; set; }
        public string Flight_no { get; set; }
        public string Term { get; set; }

        public List<PackagingListAirExportHawbList> ContainerList { get; set; }
    }

    public class PackagingListAirExportHawbList
    {
        public string Chk { get; set; }
        public string Booking_no { get; set; }
        public string Package { get; set; }
        public string Pkg { get; set; }
        public string Description { get; set; }
        public string Hts { get; set; }
        public string Pcs { get; set; }
        public string Net_weight { get; set; }
        public string Net_weight_kg { get; set; }
        public string Net_weight_lb { get; set; }
        public string Gross_weight { get; set; }
        public string Gross_weight_kg { get; set; }
        public string Gross_weight_lb { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public string Details { get; set; }
        public string Volumns { get; set; }
        public string Quantity { get; set; }
        public string Shipping_mark { get; set; }
    }
}
