using Dolphin.Freight.AirExports;
using Dolphin.Freight.TradePartner;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.Common
{
    public class QueryDto : PagedAndSortedResultRequestDto
    {
        public int Qtype { get; set; }
        public string QueryType { get; set; }
        public string QueryName { get; set; }
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string QueryKey { get; set; }
        public string Filter { get; set; }
        public bool? Active { get; set; }
        public string Search { get; set; }
       

     
        public Guid? ConsigneeId { get; set; }

       
        public DateTime? CreationDate { get; set; }

        public string FlightNo { get; set; }
        public Guid? DepatureId { get; set; }
        public Guid? DestinationId { get; set; }

        public Guid? CarrierId { get; set; }
        public Guid? FreightLocationId { get; set; }

        public Guid? OfficeId { get; set; }
        public AWBType? AwbType { get; set; }
        public bool? DirectMaster { get; set; }
        public DateTime? DepatureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? PostDate { get; set; }

        public TPType? TpType { get; set; }
        public Guid? TpAccountGroupName { get; set; }
        public Guid? TpCountryCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string TaxId { get; set; }
        public string SalePerson { get; set; }
        public string SaleOffice { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set;}

    }
}
