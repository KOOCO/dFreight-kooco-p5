using System;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ReportLog
{
    public class ReportLogDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// MBLId or HBLId
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// 報表名稱
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// 報表資料內容
        /// </summary>
        public string ReportData { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }
    }

    public class MawbReportDto
    {
        public string ReportType { get; set; }
        public int No { get; set; }
        public string ConsigneeId { get; set; }
        public string OverseaAgentId { get; set; }
        public string OfficeId { get; set; }
        public string Free { get; set; }
        public string Nomi { get; set; }
        public string Col { get; set; }
        public string SUM { get; set; }
        public string CNTR { get; set; }
        public string OverseaAgent { get; set; }
        public string Office { get; set; }
        public string Consignee { get; set; }
        public string FreightTermId { get; set; }
        public string SalesId { get; set; }
        public string SalesType { get; set; }
        public int? ServiceTermTypeFrom { get; set; }
        public int? ServiceTermTypeTo { get; set; }
        public string IsEcommerce { get; set; }
        public string ShipperId { get; set; }
        public string Shipper { get; set; }
        public string CarrierId { get; set; }
        public string CarrierName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string BillToId { get; set; }
        public string BillToName { get; set; }
        public string CustomerRefNo { get; set; }
        public string OpId { get; set; }
        public string OpName { get; set; }
        public string POLId { get; set; } = "";
        public string POLName { get; set; } = "";
        public string PODId { get; set; } = "";
        public string PODName { get; set; } = "";
        public string DestinationId { get; set; } = "";
        public string DestinationName { get; set; } = "";
        public string Vessel { get; set; } = "";
        public string MawbNo { get; set; } = "";
        public string CoLoaderId { get; set; } = "";
        public string FileNo { get; set; } = "";
        public string ForwardingAgentId { get; set; } = "";
        public string ForwardingAgentName { get; set; } = "";
        public string ColorRemarkId { get; set; } = "";
        public string ColorRemarkName { get; set; } = "";

        public bool IsShipper { get; set; }
        public bool IsOverseaAgent { get; set; }
        public bool IsConsignee { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsCarrier { get; set; }
        public bool IsCustomsBroker { get; set; }
        public bool IsTrucker { get; set; }
        public bool IsAccountGroup { get; set; }
        public bool IsBillTo { get; set; }
        public bool IsReferredBy { get; set; }
        public bool IsOutputOffice { get; set; }
        public bool IsETD { get; set; }
        public bool IsETA { get; set; }
        public bool IsOutputFreightTerm { get; set; }
        public bool IsIncoterms { get; set; }
        public bool IsServiceTerm { get; set; }
        public bool IsMBLOP { get; set; }
        public bool IsOperation { get; set; }
        public bool IsOPCOOPOP { get; set; }
        public bool IsShipLine { get; set; }
        public bool IsPOL { get; set; }
        public bool IsPOD { get; set; }
        public bool IsCountryOfPOL { get; set; }
        public bool IsCountryOfPOD { get; set; }
        public bool IsDEL { get; set; }
        public bool IsFinalDestination { get; set; }
        public bool IsVesselFlight { get; set; }
        public bool IsMblMawbWarehouse { get; set; }
        public bool IsHblHawb { get; set; }
        public bool IsOutputFile { get; set; }
        public bool IsDoorMove { get; set; }
        public bool IsClearance { get; set; }
        public bool IsISF { get; set; }
        public bool IsFBAFC { get; set; }
        public bool IsOutputSalesType { get; set; }
        public bool IsHblNominatedAgent { get; set; }
        public bool IsOutputECommerce { get; set; }
        public bool IsForwardingAgent { get; set; }
        public bool IsCarrierContractNo { get; set; }
        public bool IsMblColorRemark { get; set; }
        public bool IsHblColorRemark { get; set; }
        public bool IsCoLoader { get; set; }
        public bool IsBlType { get; set; }
        public bool IsLatestGateIn { get; set; }
    }
}

