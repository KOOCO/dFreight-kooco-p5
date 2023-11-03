using Dolphin.Freight.AirExports;
using Dolphin.Freight.AirImports;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public class CreateUpdateAirImportMawbDto
    {
        public Guid Id { get; set; }
        public string FilingNo { get; set; }
        
        public string MawbNo { get; set; }
        
        public String OfficeId { get; set; }

        public AWBType AwbType { get; set; }

        
        public DateTime PostDate { get; set; }

       
        public String OverseaAgentId { get; set; }

        
        public String CarrierId { get; set; }

        
        public Guid? AwbAcctCarrierId { get; set; }

        
        public Guid? CoLoaderId { get; set; }

        
        public String OPId { get; set; }

        public bool IsDirectMaster { get; set; }

        
        public String ShipperId { get; set; }

        
        public String ConsigneeId { get; set; }

        
        public String NotifyId { get; set; }

        
        public String CustomerId { get; set; }

        
        public String BillToId { get; set; }

        
        public String SalesId { get; set; }

        
        public String DepatureId { get; set; }

        
        public DateTime? DepatureDate { get; set; }

        public string FlightNo { get; set; }

        
        public String RouteTrans1Id { get; set; }

        
        public DateTime? RouteTrans1ArrivalDate { get; set; }

        
        public DateTime? RouteTrans1DepatureDate { get; set; }

        public string RouteTrans1FlightNo { get; set; }

        
        public String RouteTrans1CarrierId { get; set; }

        
        public String RouteTrans2Id { get; set; }

        
        public DateTime? RouteTrans2ArrivalDate { get; set; }

        
        public DateTime? RouteTrans2DepatureDate { get; set; }

        public string RouteTrans2FlightNo { get; set; }

       
        public String RouteTrans2CarrierId { get; set; }

  
        public String RouteTrans3Id { get; set; }

       
        public DateTime? RouteTrans3ArrivalDate { get; set; }

       
        public DateTime? RouteTrans3DepatureDate { get; set; }

        public string RouteTrans3FlightNo { get; set; }

        
        public String RouteTrans3CarrierId { get; set; }

        public string FPOEDepature { get; set; }

        public string FPOETrans1 { get; set; }

        public string FPOETrans2 { get; set; }

        public string FPOETrans3 { get; set; }

        public string FPOEDestination { get; set; }

       
        public String DestinationId { get; set; }

        
        public DateTime? ArrivalDate { get; set; }

        
        public String FreightLocationId { get; set; }

        
        public DateTime? StorageStartDate { get; set; }

        public double Package { get; set; }

        
        public String MawbPackageUnitId { get; set; }

        public double GrossWeightKg { get; set; }

        public double GrossWeightLb { get; set; }

        public double ChargeableWeightKg { get; set; }

        public double ChargeableWeightLb { get; set; }

        public double VolumeWeightKg { get; set; }

        public double VolumeWeightCbm { get; set; }

        public FreightType? FreightType { get; set; }

        public IncotermsType? IncotermsType { get; set; }

        public ServiceTermType ServiceTermTypeFrom { get; set; }

        public ServiceTermType ServiceTermTypeTo { get; set; }

        
        public String BusinessReferredId { get; set; }

        public bool IsECom { get; set; }

        public DisplayUnitType DisplayUnit { get; set; }
        public string Mark { get; set; }
        public string NatureAndQuantityOfGoods { get; set; }
        public string ManifestNatureAndQuantityOfGoods { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
        public string HandlingInformation { get; set; }
    }
}
