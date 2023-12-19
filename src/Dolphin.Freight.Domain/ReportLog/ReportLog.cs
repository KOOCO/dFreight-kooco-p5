using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NPOI.HSSF.Record.PivotTable;
using Dolphin.Freight.Accounting.Invoices;

namespace Dolphin.Freight.ReportLog
{
    public class ReportLog : AuditedAggregateRoot<Guid>
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

    public class MawbReport
    {
        public Guid Id { get; set; }
        public string ReportType { get; set; }
        public int No { get; set; }
        public string ConsigneeId { get; set; }
        public string OverseaAgentId { get; set; }
        public string OfficeId { get; set; } = "";
        public string Free { get; set; } = "";
        public string Nomi { get; set; } = "";
        public string Col { get; set; } = "";
        public string SUM { get; set; } = "";
        public string CNTR { get; set; } = "";
        public string OverseaAgent { get; set; } = "";
        public string Office { get; set; } = "";
        public string Consignee { get; set; } = "";
        public string FreightTermId { get; set; }
        public string SalesId { get; set; }
        public string SalesTypeId { get; set; } = "";
        public int? ServiceTermTypeFrom { get; set; }
        public int? ServiceTermTypeTo { get; set; }
        public string IsEcommerce { get; set; } = "";
        public string ShipperId { get; set; } = "";
        public string Shipper { get; set; } = "";
        public string CarrierId { get; set; } = "";
        public string CarrierName { get; set; } = "";
        public string CustomerId { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string BillToId { get; set; } = "";
        public string BillToName { get; set; } = "";
        public string CustomerRefNo { get; set; } = "";
        public string OpId { get; set; } = "";
        public string OpName { get; set; } = "";
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
        public string CargoType { get; set; }
        public string SalesPerson { get; set; } // Sales Person but don't get table
        public DateTime BLPostDate { get; set; } //  Mawb post date
        public VolumeReport Volume { get; set; }
        public ProfitReport Profit { get; set; }
        public ShipModeVolumeReport ShipModeVolume {get;set;}
        public SaleVolumeReport SaleVolume { get; set; }
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
        public Guid? ShipModeId { get; set; }
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
        public HblVolumeReport HblVolumeReport { get; set; }
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

    public class VolumeReport
    {
        public int V20 { get; set; }
        public int V40 { get; set; }
        public int HC { get; set; }
        public int V45 { get; set; }
        public int RF { get; set; }
        public int SOC { get; set; }
      
        public int ETC { get; set; }
    }
    public class HblVolumeReport
    {
        public int V20 { get; set; }
        public int V40 { get; set; }
        public int HC { get; set; }
        public int V45 { get; set; }
        public int RF { get; set; }
        public int SOC { get; set; }

        public int ETC { get; set; }
    }
    public class SaleVolumeReport
    {
        public double Free { get; set; }
        public double Nomi { get; set; }
        public double Co { get; set; }
       

        public double ETC { get; set; }
        public double HblFree { get; set; }
        public double HblNomi { get; set; }
        public double HblCo { get; set; }


        public double HblETC { get; set; }
    }
    public class ShipModeVolumeReport
    {
        public double Lcl { get; set; }
        public double Fcl { get; set; }
        public double CoLoaded { get; set; }
        public double Bulk { get; set; }
        public double HblLcl { get; set; }
        public double HblFcl { get; set; }
        public double HblCoLoaded { get; set; }
        public double HblBulk { get; set; }


    }

    public class PackageSizeReport
    {

        public Guid ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public Guid MblId { get; set; }
    }
    

    public class ProfitReport
    {
        public double ARTotal { get; set; }
        public double APTotal { get; set; }
        public double DCTotal { get; set; }
        public double ProfitMargin { get; set; }
        public double ProfitAmt { get; set; }
        public double Avg_Profit_Per_Cntr { get; set; }
        public List<InvoiceDto> AR { get; set; }
        public List<InvoiceDto> DC { get; set; }
        public List<InvoiceDto> AP { get; set; }
        public List<InvoiceDto> Invoices { get; set; }
        public double NomiProfit { get; set; }
        public double FreeProfit { get; set; }
        public double CoProfit { get; set; }
        public double ETCProfit { get; set; }
        public double HblNomiProfit { get; set; }
        public double HblFreeProfit { get; set; }
        public double HblCoProfit { get; set; }
        public double HblETCProfit { get; set; }
    }
    public class SysCodeForReport
    {
        public string   Id  { get; set; }
        public string ShowName { get; set; }
        public string CodeType { get; set; }
    }
}
