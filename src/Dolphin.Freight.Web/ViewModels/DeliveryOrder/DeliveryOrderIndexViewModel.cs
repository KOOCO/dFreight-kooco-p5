using System;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.DeliveryOrder
{
    public class DeliveryOrderIndexViewModel
    {
        
        public string BaseUrl { get; set; }
        public string Office { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Date { get; set; }
        public string DateTime { get; set; }
        public string CurrentDate { get; set; }
        public bool IsPDF { get; set; }
        public string form_title { get; set; }
        public string issue_at { get; set; }
        public string issue_by { get; set; }
        public string trucker_area { get; set; }
        public string HBL_LcNo { get; set; }
        public string is_show_empty_pickup_area { get; set; }
        public string empty_pickup_area { get; set; }
        public string empty_pickup_ref_no { get; set; }
        public string empty_pickup_date { get; set; }
        public string empty_pickup_time { get; set; }
        public string cargo_pickup_area { get; set; }
        public string cargo_pickup_ref_no { get; set; }
        public string cargo_pickup_date { get; set; }
        public string cargo_pickup_time { get; set; }
        public string is_show_delivery_to_area { get; set; }
        public string MBL_NO { get; set; }
        public string HBL_NO { get; set; }
        public string OUR_REF_NO { get; set; }
        public string carrier_bkg_no { get; set; }
        public string carrier { get; set; }
        public string VESSEL_INFO { get; set; }
        public string VOVAGE_INFO { get; set; }
        public string POR_location { get; set; }
        public string POR_location_ETD { get; set; }
        public string POL_location { get; set; }
        public string POL_location_ETD { get; set; }
        public string POD_location { get; set; }
        public string POD_location_ETD { get; set; }
        public string container_qty { get; set; }
        public string total_packages_count { get; set; }
        public string package_unit { get; set; }
        public string gross_weight_kgs { get; set; }
        public string gross_weight_lbs { get; set; }
        public string measurement_cbm { get; set; }
        public string measurement_cft { get; set; }
        public string LcIssueBank { get; set; }
        public string LcIssueDate { get; set; }
        public string COMMODITY { get; set; }
        public string PO_NO { get; set; }
        public string is_show_bill_to { get; set; }
        public string billing_to_area { get; set; }
        public string billing_to_ref_no { get; set; }
        public string instruction_description { get; set; }
        public string do_not_break_pallet { get; set; }
        public string id_is_show_bill_to { get; set; }
        public string delivery_to_ref_no { get; set; }
        public string delivery_to_date { get; set; }
        public string delivery_to_time { get; set; }
        public string delivery_to_area { get; set; }
        public string FdestName { get; set; }
        public string CyLocation { get; set; }
        public string PodEta { get; set; }
        public string description { get; set; }
        public string marks { get; set; }
        public List<DeliveryOrderContainerList> ContainerList { get; set; }
        //ReportLog
        public Guid ReportId { get; set; }
    }
    public class DeliveryOrderContainerList
    {
        public string chk { get; set; }
        public string CONTAINER_NO { get; set; }
        public string TYPE { get; set; }
        public string SEAL_NO { get; set; }
        public string PACKAGE { get; set; }
        public string WEIGHT { get; set; }
        public string PICKUP_NO { get; set; }
        public string LFD { get; set; }
    }
}

