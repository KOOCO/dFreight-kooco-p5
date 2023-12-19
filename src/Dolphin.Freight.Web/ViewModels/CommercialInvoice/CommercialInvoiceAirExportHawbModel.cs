using System;

namespace Dolphin.Freight.Web.ViewModels.CommercialInvoice
{
    public class CommercialInvoiceAirExportHawbModel
    {
        public bool IsPDF { get; set; }
        public string Shipper { get; set; }
        public string Inv_No_Date { get; set; }
        public string LC_No { get; set; }
        public string LC_Issue_Bank { get; set; }
        public string Departure_Date { get; set; }
        public string Consignee { get; set; }
        public string Port_Of_Loading { get; set; }
        public string Final_Destination { get; set; }
        public string Notify { get; set; }
        public string Flight_No { get; set; }
        public string Term { get; set; }
        public string Shipping_Mark { get; set; }
        public string Package_Quantity { get; set; }
        public string Description { get; set; }
        public string Unit_Price { get; set; }
        public string Total_Amount { get; set; }
        public string BaseUrl { get; set; }
        public Guid ReportId { get; set; }
    }
}
