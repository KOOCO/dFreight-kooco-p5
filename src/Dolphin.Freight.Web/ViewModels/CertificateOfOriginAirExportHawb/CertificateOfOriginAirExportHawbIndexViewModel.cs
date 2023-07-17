using System;

namespace Dolphin.Freight.Web.ViewModels.CertificateOfOriginAirExportHawb
{
    public class CertificateOfOriginAirExportHawbIndexViewModel
    {
        public string SHIPPER_EXPORTER { get; set; }
        public string CONSIGNEE { get; set; }
        public string NOTIFY { get; set; }
        public string DOCUMENT_NO { get; set; }
        public string BL_NO_CHECK { get; set; }
        public string BL_NO { get; set; }
        public string EXPORT_FILE_NO { get; set; }
        public string FORWARDING_AGENT { get; set; }
        public string POINT_AND_COUNTRY_OF_ORIGIN { get; set; }
        public string EXPORT_CARRIER { get; set; }
        public string PORT_OF_LOADING { get; set; }
        public string PORT_OF_DISCHARGE { get; set; }
        public string PLACE_OF_DELIVERY { get; set; }
        public string SHIPPING_MARKS { get; set; }
        public string QTY { get; set; }
        public string DESCRIPTION_OF_GOODS { get; set; }
        public string WEIGHT_G { get; set; }
        public string WEIGHT_C { get; set; }
        public string MEASUREMENT { get; set; }
        public string Show_Container_Information { get; set; }
        public string bl_date { get; set; }
        public string sworn_date { get; set; }
        public string name_of_chamber { get; set; }
        public string state_of_chamber { get; set; }
        public string name_of_country { get; set; }

        public string CONTAINER_NO { get; set; }
        public string TYPE { get; set; }
        public string SEAL_NO { get; set; }
        public string PKG { get; set; }
        public string KG_LB { get; set; }
        public string CBM_CFT { get; set; }

        public string BaseUrl { get; set; }

        //ReportLog
        public Guid ReportId { get; set; }
    }
}
