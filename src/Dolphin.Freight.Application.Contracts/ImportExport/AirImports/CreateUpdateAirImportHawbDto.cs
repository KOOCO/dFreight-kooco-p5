using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public class CreateUpdateAirImportHawbDto : AuditedEntityDto<Guid>
    {
        public Guid? MawbId { get; set; }
                                    
        /// <summary>
        /// Hawb 號碼
        /// </summary>
        public string HawbNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? QuotationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Hsn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? ShipperId { get; set; }
        // <summary>
        /// 操作員Id
        /// </summary>
        public Guid? SalesId { get; set; }
        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("SalesId")]
        public virtual UserData Sales { get; set; }
        /// <summary>
        /// 操作員Id
        /// </summary>
        public Guid? OPId { get; set; }
        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("OPId")]
        public virtual UserData OP { get; set; }

        public string FreightLocation { get; set; }
        public string FinalDestination { get; set; }
        public DateTime FinalETA { get; set; }
        public String DeliveryLocation { get; set; }
        public string Trucker { get; set; }
        public DateTime LastFreeDay { get; set; }
        public DateTime StorageStartDate { get; set; }
        public String Freight { get; set; }
        public string SalesType { get; set; }
        public string Package { get; set; }
        public string PackageUnit { get; set; }
        public string GrossWeightKG { get; set; }
        public string GrossWeightLB { get; set; }
        public string ChargeableWeightKG { get; set; }
        public string ChargeableWeightLB { get; set; }
        public string VolumeWeightKG { get; set; }
        public string VolumeWeightCBM { get; set; }
        public string ITNo { get; set; }
        public string ClassOfEntry { get; set; }
        public string CardColorValue { get; set; }
        public DateTime ITDate { get; set; }
        public string ITIssuedLocation { get; set; }
        public DateTime FrtRelease { get; set; }
        public string ReleasedBy { get; set; }
        public string CargoReleasedto { get; set; }
        public DateTime CReleasedDate { get; set; }
        public DateTime DoorDelivered { get; set; }
        public string ShipType { get; set; }
        public string Incoterms { get; set; }
        public string ServiceTermStart { get; set; }
        public string ServiceTermTo { get; set; }
        public string PONo { get; set; }
        public string Mark { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }



        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
