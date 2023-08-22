﻿using Dolphin.Freight.AirExports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;
using System.ComponentModel;
using Dolphin.Freight.ImportExport.AirExports;
using Volo.Abp.Data;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public class AirImportHawbDto : AuditedEntityDto<Guid>
    {

        /// </summary>
        public Guid? MawbId { get; set; }
        /// <summary>
        /// Mawb Entity
        /// </summary>
        //[ForeignKey("MawbId")]
        //public virtual AirImportHawb Mawb { get; set; }
        /// <summary>
        /// Hawb 號碼
        /// </summary>
        [Required]
        public string HawbNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? QuotationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HSN")]
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
        /// SalesId
       
        public Guid? OPId { get; set; }
        /// <summary>
        /// 操作員
        /// </summary>
        [ForeignKey("OPId")]
        public virtual UserData OP { get; set; }

        public Guid? ConsigneeId { get; set; }
        public string ConsigneeName { get; set; }
        public string FreightLocation { get; set; }
        public string FinalDestination { get; set; }
        public DateTime? FinalETA { get; set; }
        public String DeliveryLocation { get; set; }
        public string Trucker { get; set; }
        public DateTime? LastFreeDay { get; set; }
        public DateTime? StorageStartDate { get; set; }
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
        public DateTime? ITDate { get; set; }
        public string ITIssuedLocation { get; set; }
        public DateTime? FrtRelease { get; set; }
        public string ReleasedBy { get; set; }
        public string CargoReleasedto { get; set; }
        public DateTime? CReleasedDate { get; set; }
        public DateTime? DoorDelivered { get; set; }
        public string ShipType { get; set; }
        public string Incoterms { get; set; }
        public string ServiceTermStart { get; set; }
        public string ServiceTermTo { get; set; }
        public string PONo { get; set; }
        public string Mark { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string Notify { get; set; }
        public string Customer { get; set; }
        public string BillToId { get; set; }
        public string CustomsBroker { get; set; }
        public List<Commodity> Commodities { get; set; }
        public List<SubHawbs> SubHawbs { get; set; }
        public bool IsLocked { get; set; }
        public string SalesName { get; set; }
        public string DocNo { get; set; }
        public string DestinationName { get; set; }
        public string ShipperName { get; set; }
        public string OfficeName { get; set; }
        public string MawbNo { get; set; }
        public string FreightLocationName { get; set; }
        public string DeliveryLocationName { get; set; }
        public string DepatureName { get; set; }
        public string OperatorName { get; set; }

        /// <summary>
        /// 是否刪除
        /// </summary>
        public bool IsDeleted { get; set; }
        public ExtraPropertyDictionary ExtraProperties { get; set; }
    }
}
