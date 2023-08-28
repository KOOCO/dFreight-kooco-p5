using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.AirExports;
using Dolphin.Freight.AirImports;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.Web.Pages.AirImports
{
    public class CreateMawbModel : AbpPageModel
    {
        [BindProperty(SupportsGet =true)]
        public Guid Id { get; set; }
        public Guid? MawbId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsCopyFlightInfo { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public bool AccountingInfo { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsAPCopy { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsARCopy { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsDCCopy { get; set; }
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        private readonly IAirImportHawbAppService _airImportHawbAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IInvoiceBillAppService _invoiceBillAppService;

        [BindProperty]
        public CreateAIMMawbViewModel MawbModel { get; set; }
        [BindProperty]
        public AirImportHawbDto HawbModel { get; set; }
        [BindProperty]
        public AirImportMawbDto AirImportMawbDto { get; set; }

        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }

        public CreateMawbModel(ITradePartnerAppService tradePartnerAppService,
            ISubstationAppService substationAppService,
            IAirportAppService airportAppService,
            IPackageUnitAppService packageUnitAppService,
            IAirImportMawbAppService airImportMawbAppService,
            IAirImportHawbAppService airImportHawbAppService,
             IInvoiceAppService invoiceAppService,
             IInvoiceBillAppService invoiceBillAppService
            )
        {
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airImportMawbAppService = airImportMawbAppService;
            _airImportHawbAppService = airImportHawbAppService;
            _invoiceAppService = invoiceAppService;
            _invoiceBillAppService= invoiceBillAppService;
        }


        #region OnGetAsync()
        public async Task OnGetAsync()
        {
            if (Id != Guid.Empty)
            {
                AirImportMawbDto = await _airImportMawbAppService.GetAsync(Id);

                MawbModel=ObjectMapper.Map<AirImportMawbDto,CreateAIMMawbViewModel>(AirImportMawbDto);
                MawbModel.MawbNo = null;
                MawbModel.FilingNo = null;
                MawbModel.AwbType = AWBType.Normal;
                MawbModel.ServiceTermTypeFrom = ServiceTermType.Airport;
                MawbModel.ServiceTermTypeTo = ServiceTermType.Airport;
                MawbModel.DisplayUnit = DisplayUnitType.Both;
                MawbModel.FreightType = FreightType.Collect;
                MawbModel.IncotermsType = IncotermsType.Cfr;
                //MawbModel.Id = Guid.Empty;
                if (!IsCopyFlightInfo)
                {

                    MawbModel.RouteTrans1Id = null;
                    MawbModel.RouteTrans1ArrivalDate = null;
                    MawbModel.RouteTrans1DepatureDate = null;
                    MawbModel.RouteTrans1FlightNo = null;
                    MawbModel.RouteTrans1CarrierId = null;

                    MawbModel.RouteTrans2Id = null;
                    MawbModel.RouteTrans2ArrivalDate = null;
                    MawbModel.RouteTrans2DepatureDate = null;
                    MawbModel.RouteTrans2FlightNo = null;
                    MawbModel.RouteTrans2CarrierId = null;

                    MawbModel.RouteTrans3Id = null;
                    MawbModel.RouteTrans3ArrivalDate = null;
                    MawbModel.RouteTrans3DepatureDate = null;
                    MawbModel.RouteTrans3FlightNo = null;
                    MawbModel.RouteTrans3CarrierId = null;



                }


            }
            else
            {



                MawbModel = new CreateAIMMawbViewModel
                {
                    // set default value
                    AwbType = AWBType.Normal,
                    ServiceTermTypeFrom = ServiceTermType.Airport,
                    ServiceTermTypeTo = ServiceTermType.Airport,
                    DisplayUnit = DisplayUnitType.Both
                };
            }

        }
        #endregion

        #region OnPostAsync()
        public async Task<IActionResult> OnPostAsync()
        {
            MawbModel.FilingNo = SetAirImportFileNo();
            MawbModel.PostDate = Clock.Now.ClearTime();

            if (Id != Guid.Empty)
            {
                MawbModel.Id = Guid.Empty;
                var inputDto = await _airImportMawbAppService.CreateAsync(
                  ObjectMapper.Map<CreateAIMMawbViewModel, CreateUpdateAirImportMawbDto>(MawbModel)
               );
                if (AccountingInfo)
                {
                    QueryInvoiceDto qidto = new QueryInvoiceDto() { QueryType = 0, ParentId = AirImportMawbDto.Id };
                    var invoiceDtos = await _invoiceAppService.QueryInvoicesAsync(qidto);
                    if (invoiceDtos != null && invoiceDtos.Count > 0)
                    {
                        if (IsAPCopy)
                        {
                            var invoiceAp = invoiceDtos.Where(x => x.InvoiceType == 0).ToList();
                            foreach (var invoice in invoiceAp)
                            {
                                var newInvoiceAp = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                newInvoiceAp.MawbId = inputDto.Id;
                                newInvoiceAp.Id = Guid.Empty;
                                var createInvoice = await _invoiceAppService.CreateAsync(newInvoiceAp);
                                QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                                query.InvoiceNo = invoice.Id.ToString();
                                var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                                foreach (var bill in invoiceBills)
                                {
                                    var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                                    newbill.InvoiceId = createInvoice.Id;
                                    newbill.Id = Guid.Empty;
                                    await _invoiceBillAppService.CreateAsync(newbill);



                                }



                            }

                            }

                        if (IsDCCopy)
                        {
                            var invoiceDc = invoiceDtos.Where(x => x.InvoiceType == 1).ToList();
                            foreach (var invoice in invoiceDc)
                            {

                                var newInvoiceDc = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                newInvoiceDc.MawbId = inputDto.Id;
                                newInvoiceDc.Id = Guid.Empty;
                                var createInvoice = await _invoiceAppService.CreateAsync(newInvoiceDc);
                                QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                                query.InvoiceNo = invoice.Id.ToString();
                                var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                                foreach (var bill in invoiceBills)
                                {
                                    var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                                    newbill.InvoiceId = createInvoice.Id;
                                    newbill.Id = Guid.Empty;
                                    await _invoiceBillAppService.CreateAsync(newbill);



                                }

                            }


                        }
                        if (IsARCopy)
                        {
                            var invoiceAr = invoiceDtos.Where(x => x.InvoiceType == 2).ToList();
                            foreach (var invoice in invoiceAr)
                            {

                                var newInvoiceAr = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                newInvoiceAr.MawbId = inputDto.Id;
                                newInvoiceAr.Id = Guid.Empty;
                                var createInvoice = await _invoiceAppService.CreateAsync(newInvoiceAr);
                                QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                                query.InvoiceNo = invoice.Id.ToString();
                                var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                                foreach (var bill in invoiceBills)
                                {
                                    var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                                    newbill.InvoiceId = createInvoice.Id;
                                    newbill.Id = Guid.Empty;
                                    await _invoiceBillAppService.CreateAsync(newbill);



                                }


                            }


                        }

                    }


                    }


                MawbId = inputDto.Id;

                if (HawbModel is not null && !string.IsNullOrEmpty(HawbModel.HawbNo))
                {
                    HawbModel.Id = Guid.Empty;
                    if (HawbModel.ExtraProperties == null)
                    {
                        HawbModel.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                    }

                    if (HawbModel.Commodities != null)
                    {
                        HawbModel.ExtraProperties.Add("Commodities", HawbModel.Commodities);
                    }

                    if (HawbModel.SubHawbs != null)
                    {
                        HawbModel.ExtraProperties.Add("SubHawbs", HawbModel.SubHawbs);
                    }

                    var addHawb = ObjectMapper.Map<AirImportHawbDto, CreateUpdateAirImportHawbDto>(HawbModel);
                    addHawb.MawbId = MawbId;

                    await _airImportHawbAppService.CreateAsync(addHawb);

                }

            }

            else
            {

                var inputDto = await _airImportMawbAppService.CreateAsync(
                       ObjectMapper.Map<CreateAIMMawbViewModel, CreateUpdateAirImportMawbDto>(MawbModel)
                    );

                MawbId = inputDto.Id;

                if (HawbModel is not null && !string.IsNullOrEmpty(HawbModel.HawbNo))
                {
                    if (HawbModel.ExtraProperties == null)
                    {
                        HawbModel.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                    }

                    if (HawbModel.Commodities != null)
                    {
                        HawbModel.ExtraProperties.Add("Commodities", HawbModel.Commodities);
                    }

                    if (HawbModel.SubHawbs != null)
                    {
                        HawbModel.ExtraProperties.Add("SubHawbs", HawbModel.SubHawbs);
                    }

                    var addHawb = ObjectMapper.Map<AirImportHawbDto, CreateUpdateAirImportHawbDto>(HawbModel);
                    addHawb.MawbId = MawbId;

                    await _airImportHawbAppService.CreateAsync(addHawb);
                }
            }

            Dictionary<string, Guid> rs = new()
            {
                { "id", MawbId.Value }
            };
            return new JsonResult(rs);
        }
        #endregion

        #region SetAirImportFileNo() 
        /// <summary>
        /// ³]©wAir ImportªºFile No
        /// </summary>
        private string SetAirImportFileNo()
        {
            string today = DateTime.Now.ToString("yyyyMMddhhmmss");
            string AIFileNo = "AIM-" + today;
            return AIFileNo;
        }
        #endregion

        #region CreateMawbViewModel
        public class CreateAIMMawbViewModel
        {
            [HiddenInput]
            [BindProperty(SupportsGet = true)]
            public Guid Id { get; set; }
            public string FilingNo { get; set; }
            [Required]
            [MaxLength(12)]
            [RegularExpression("^[0-9]{3}-[0-9]{1,8}$", ErrorMessage = "Invalid Mawb No.")]
            public string MawbNo { get; set; }
            [Required]
            [SelectItems(nameof(SubstationLookupList))]
            public String OfficeId { get; set; }
            
            public AWBType AwbType { get; set; }
            public DateTime? PostDate { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String OverseaAgentId { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String CarrierId { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String? AwbAcctCarrierId { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String? CoLoaderId { get; set; }
            
            [SelectItems(nameof(TradePartnerLookupList))]
            public String OPId { get; set; }
            
            public bool IsDirectMaster { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String ShipperId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String ConsigneeId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String NotifyId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String CustomerId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String BillToId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String SalesId { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String DepatureId { get; set; }
            public DateTime? DepatureDate { get; set; }
            
            public string FlightNo { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String RouteTrans1Id { get; set; }
            public DateTime? RouteTrans1ArrivalDate { get; set; }
            public DateTime? RouteTrans1DepatureDate { get; set; }
            
            public string RouteTrans1FlightNo { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String RouteTrans1CarrierId { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String RouteTrans2Id { get; set; }
            public DateTime? RouteTrans2ArrivalDate { get; set; }
            public DateTime? RouteTrans2DepatureDate { get; set; }
            
            public string RouteTrans2FlightNo { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String RouteTrans2CarrierId { get; set; }

            [SelectItems(nameof(AirportLookupList))]
            public String RouteTrans3Id { get; set; }
            public DateTime? RouteTrans3ArrivalDate { get; set; }
            public DateTime? RouteTrans3DepatureDate { get; set; }

            public string RouteTrans3FlightNo { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String RouteTrans3CarrierId { get; set; }

            public string FPOEDepature { get; set; }
            
            public string FPOETrans1 { get; set; }
            
            public string FPOETrans2 { get; set; }
            
            public string FPOETrans3 { get; set; }
            
            public string FPOEDestination { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String DestinationId { get; set; }

            [Required]
            public DateTime? ArrivalDate { get; set; }

            [SelectItems(nameof(TradePartnerLookupList))]
            public String FreightLocationId { get; set; }
            public DateTime? StorageStartDate { get; set; }
            
            public double Package { get; set; }

            [SelectItems(nameof(PackageUnitLookupList))]
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

            [SelectItems(nameof(TradePartnerLookupList))]
            public String BusinessReferredId { get; set; }
            
            public bool IsECom { get; set; }
            
            public DisplayUnitType DisplayUnit { get; set; }
        }
        #endregion
    }
}
