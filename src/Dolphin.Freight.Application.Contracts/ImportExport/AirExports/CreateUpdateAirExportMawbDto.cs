using Dolphin.Freight.AirExports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class CreateUpdateAirExportMawbDto
    {
        public Guid Id { get; set; }
        public string FilingNo { get; set; }

        public String MawbCarrierId { get; set; }
       
        public String IssuingCarrierId { get; set; }
        public AWBType AwbType { get; set; }

        public string MawbNo { get; set; }
        [DataType(DataType.Date)]
        public DateTime AwbDate { get; set; }
        public string ItnNo { get; set; }
       
        public String ShipperId { get; set; }
       

        public String ConsigneeId { get; set; }
        
        public String NotifyId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? PostDate { get; set; }
        [Required]
        
        public String OfficeId { get; set; }

        public bool IsDirectMaster { get; set; }
        
        public String AwbAcctCarrierId { get; set; }
        
        public String CoLoaderId { get; set; }
        
        public String MawbOperatorId { get; set; }

        
        public String DepatureId { get; set; }
        [Required]
        [DisplayName("")]
        [DataType(DataType.DateTime)]
        public DateTime? DepatureDate { get; set; }
        public string FlightNo { get; set; }

        
        public string DestinationId { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string DVCarriage { get; set; }
        public string DVCustomer { get; set; }

        public string Insurance { get; set; }
        public string CarrierSpotNo { get; set; }
        
        public string WtVal { get; set; }
        [Display(Name = "Other")]
        
        public string Other { get; set; }

        public String DeliveryId { get; set; }
        public string ShippingInfo { get; set; }
        public string ShipperLoad { get; set; }
        public string Sci { get; set; }

        
        public String RouteTrans1Id { get; set; }
        [DisplayName("")]
        public DateTime? RouteTrans1ArrivalDate { get; set; }
        [DisplayName("")]
        public DateTime? RouteTrans1DepatureDate { get; set; }
        [DisplayName("")]
        public string RouteTrans1FlightNo { get; set; }
        [DisplayName("")]
        
        public String RouteTrans1CarrierId { get; set; }

        
        public String RouteTrans2Id { get; set; }
        [DisplayName("")]
        public DateTime? RouteTrans2ArrivalDate { get; set; }
        [DisplayName("")]
        public DateTime? RouteTrans2DepatureDate { get; set; }
        [DisplayName("")]
        public string RouteTrans2FlightNo { get; set; }
        [DisplayName("")]
        
        public String RouteTrans2CarrierId { get; set; }

        
        public String RouteTrans3Id { get; set; }
        [DisplayName("")]
        public DateTime? RouteTrans3ArrivalDate { get; set; }
        [DisplayName("")]
        public DateTime? RouteTrans3DepatureDate { get; set; }
        [DisplayName("")]
        public string RouteTrans3FlightNo { get; set; }
        [DisplayName("")]
        
        public String RouteTrans3CarrierId { get; set; }

        public double Package { get; set; }
        [DisplayName("")]
        public String MawbPackageUnitId { get; set; }
        public double BuyingRate { get; set; }
        public RateUnitType BuyingRateUnitType { get; set; }
        public double SellingRate { get; set; }
        public RateUnitType SellingRateUnitType { get; set; }
        public double VolumeWeightKg { get; set; }
        public double VolumeWeightCbm { get; set; }

        public double GrossWeightKg { get; set; }
        [DisplayName("")]
        public double GrossWeightLb { get; set; }
        public double GrossWeightAmount { get; set; }

        public double AwbGrossWeightKg { get; set; }
        public double AwbGrossWeightLb { get; set; }
        public double AwbGrossWeightAmount { get; set; }

        public double ChargeableWeightKg { get; set; }
        public double ChargeableWeightLb { get; set; }
        public double ChargeableWeightAmount { get; set; }

        public double AwbChargeableWeightKg { get; set; }
        public double AwbChargeableWeightLb { get; set; }
        public double AwbChargeableWeightAmount { get; set; }

        public IncotermsType IncotermsType { get; set; }
        public ServiceTermType ServiceTermTypeFrom { get; set; }
        [DisplayName("~")]
        public ServiceTermType ServiceTermTypeTo { get; set; }
        public DisplayUnitType DisplayUnit { get; set; }

        public bool IsAwbCancelled { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? AwbCancelledDate { get; set; }
        
        public String AwbCancelledOpId { get; set; }
        public ReasonForCancelType ReasonForCancel { get; set; }
        
        public String BusinessReferredId { get; set; }
        public bool IsECom { get; set; }
        public string PONo { get; set; }
        public List<OtherCharges> OtherCharges { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
        public string Mark { get; set; }
        public string NatureAndQuantityOfGoods { get; set; }
        public string ManifestNatureAndQuantityOfGoods { get; set; }
        public string HandlingInformation { get; set; }
    }
}
