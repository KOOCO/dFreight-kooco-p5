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
        public string CargoType { get; set; }
        public string SalesPerson { get; set; } // Sales Person but don't get table 
        public DateTime BLPostDate { get; set; } //  Mawb post date
        public bool IsNA { get; set; }
        public bool IsCOLoad { get; set; }
        public bool IsFreeCargo { get; set; }
        public bool IsNomi { get; set; }
        public bool IsCargoType { get; set; }
        public bool IsSalesPerson { get; set; }
        public bool IsBLPostDate { get; set; }
        public string[] FreightCode { get; set; }
        public decimal? MinProfit { get; set; }
        public decimal? MaxProfit { get; set; }
        public string Profit { get; set; }
        public Guid? ShipModeId { get; set; }
        public bool IsOceanImport { get; set; }
        public bool IsOceanExport { get; set; }
        public bool IsAirImport { get; set; }
        public bool IsAirExport { get; set; }
        public bool IsTruck { get; set; }
        public bool IsMisc { get; set; }
        public bool IsWarehouse { get; set; }
        public bool IsVolumeDetail { get; set; }
        public bool IsProfitDetail { get; set; }
        public bool IsContainerDetail { get; set; }
        public bool IsVolByCntrAndSalesType { get; set; }
        public int V20 { get; set; }
        public int V40 { get; set; }
        public int HC { get; set; }
        public int V45 { get; set; }
        public int RF { get; set; }
        public int SOC { get; set; }
        public int ETC { get; set; }
        public double ARTotal { get; set; }
        public double APTotal { get; set; }
        public double DCTotal { get; set; }
        public double ProfitMargin { get; set; }
        public double ProfitAmt { get; set; }
        public double Avg_Profit_Per_Cntr { get; set; }
        public double GrossWeightKG { get; set; }
        public double GrossWeightLB { get; set; }
        public double ChargeableWeightKG { get; set; }
        public double ChargeableWeightLB { get; set; }
        public double FCL { get; set; }
        public double LCL { get; set; }
        public double CoLoaded { get; set; }
        public double Bulk { get; set; }
        public double HblFCL { get; set; }
        public double HblLCL { get; set; }
        public double HblCoLoaded { get; set; }
        public double HblBulk { get; set; }

        public double SaleFree { get; set; }
        public double SaleNomi { get; set; }
        public double SaleCo { get; set; }


        public double SaleETC { get; set; }
        public double HblFree { get; set; }
        public double HblNomi { get; set; }
        public double HblCo { get; set; }


        public double HblETC { get; set; }

        public int HblV20 { get; set; }
        public int HblV40 { get; set; }
        public int HblHC { get; set; }
        public int HblV45 { get; set; }
        public int HblRF { get; set; }
        public int HblSOC { get; set; }

        public int HblCntETC { get; set; }

        public double NomiProfit { get; set; }
        public double FreeProfit { get; set; }
        public double CoProfit { get; set; }
        public double ETCProfit { get; set; }
        public double HblNomiProfit { get; set; }
        public double HblFreeProfit { get; set; }
        public double HblCoProfit { get; set; }
        public double HblETCProfit { get; set; }
    }

}

