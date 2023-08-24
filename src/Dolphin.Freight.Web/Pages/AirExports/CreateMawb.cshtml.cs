using Dolphin.Freight.AirExports;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.Web.Pages.Sales.TradePartner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Dolphin.Freight.Accounting;
using Volo.Abp.Data;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Accounting.InvoiceBills;
using static Dolphin.Freight.Permissions.AccountingPermissions;

namespace Dolphin.Freight.Web.Pages.AirExports
{
    public class CreateMawbModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsCopyFlightInfo { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CopyHawb { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool CopyCommodity { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool MawbCopyCommodity { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool AccountingInfo { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool MawbAccountingInfo { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsAPCopy { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsARCopy { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsDCCopy { get; set; }

        public Guid MawbId { get; set; }
        public ILogger<CreateMawbModel> Logger { get; set; }
       
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IAirExportMawbAppService _airExportMawbAppService;
        private readonly IAjaxDropdownAppService ajaxDropdownAppService;
        private readonly IAirExportHawbAppService _airExportHawbAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IInvoiceBillAppService _invoiceBillAppService;

        [BindProperty]
        public CreateMawbViewModel MawbModel { get; set; }
        [BindProperty]
        public List<Commodity> Commodities { get; set; }
        [BindProperty]
        public List<AccountingInformation> AccountingInformation { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        [BindProperty]
        public List<MoreInformation> MoreInformations { get; set; }
        [BindProperty]
        public MoreInformation MoreInformationPrepaid { get; set; }
        [BindProperty]
        public MoreInformation MoreInformationCollect { get; set; }

        [BindProperty]
        public AirExportHawbDto AirExportHawbDto { get; set; }
        [BindProperty]
        public AirExportMawbDto AirExportMawbDto { get; set; }
        public IList<InvoiceDto> h0invoiceDtos { get; set; }

       
        public IList<InvoiceDto> h1invoiceDtos { get; set; }

        
        public IList<InvoiceDto> h2invoiceDtos { get; set; }


        public CreateMawbModel(ITradePartnerAppService tradePartnerAppService,
            ISubstationAppService substationAppService,
            IAirportAppService airportAppService,
            IPackageUnitAppService packageUnitAppService,
            IAirExportMawbAppService airExportMawbAppService,
            IAirExportHawbAppService airExportHawbAppService,
             IInvoiceAppService invoiceAppService,
             IInvoiceBillAppService invoiceBillAppService
            )
        {
            Logger = NullLogger<CreateMawbModel>.Instance;
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airExportMawbAppService = airExportMawbAppService;
            _airExportHawbAppService = airExportHawbAppService;
            _invoiceAppService= invoiceAppService;
            _invoiceBillAppService= invoiceBillAppService;
        }
        [BindProperty]
        public List<SelectListItem> EnumList { get; set; }
        public List<SelectListItem> ChargeItemList { get; set; }
        Guid CopyId = Guid.Empty;
        public async Task OnGetAsync()
        {
            if (Id != Guid.Empty)
            {
                AirExportMawbDto = await _airExportMawbAppService.GetAsync(Id);
                MawbModel =ObjectMapper.Map<AirExportMawbDto,CreateMawbViewModel >(AirExportMawbDto);
                MawbModel.MawbNo = null;
                MawbModel.FilingNo = null;
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
                if (!MawbCopyCommodity)
                {
                    MawbModel.ExtraProperties.Remove("Commodities");


                }
            }
            else
            {
               
                MawbModel = new CreateMawbViewModel
                {
                    // set default value
                    AwbDate = Clock.Now.ClearTime(),
                    ItnNo = "NO EEI 30.37(a)",
                    AwbType = AWBType.Normal,
                    DVCarriage = "NVD",
                    DVCustomer = "NCV",
                    Insurance = "XXX",
                    WtVal = "PPD",
                    Other = "PPD"
                };
            }

               

                await FillTradePartnerAsync();
                await FillSubstationAsync();
                await FillAirportAsync();
                FillWtValOther();
                await FillPackageUnitAsync();
            EnumList = GetIncotermsSelectList();
            ChargeItemList = GetChargeItemSelectList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Logger.LogDebug("Depature Date:" + MawbModel.DepatureDate);
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Id != Guid.Empty)
            {
                MawbModel.FilingNo = SetAirExportFileNo();
                MawbModel.PostDate = Clock.Now.ClearTime();
                var NewMawab = ObjectMapper.Map<CreateMawbViewModel, CreateUpdateAirExportMawbDto>(MawbModel);

                if (NewMawab.ExtraProperties == null)
                {
                    NewMawab.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                }
                if (MoreInformationPrepaid != null || MoreInformationCollect != null)
                {
                    MoreInformations.Add(MoreInformationPrepaid);
                    MoreInformations.Add(MoreInformationCollect);


                    NewMawab.ExtraProperties.Add("MoreInformation", MoreInformations);

                }
                if (NewMawab.OtherCharges != null)
                {

                    NewMawab.ExtraProperties.Add("OtherCharges", NewMawab.OtherCharges);

                }
                if (Commodities != null)
                {
                    NewMawab.ExtraProperties.Add("Commodities", Commodities);
                }
                if (AccountingInformation != null)
                {
                    NewMawab.ExtraProperties.Add("AccountingInformation", AccountingInformation);
                }

                var inputDto = await _airExportMawbAppService.CreateAsync(NewMawab);
                if (MawbAccountingInfo)
                {
                    QueryInvoiceDto q1idto = new QueryInvoiceDto() { QueryType = 0, ParentId = AirExportMawbDto.Id };
                    var invoiceDtos1 = await _invoiceAppService.QueryInvoicesAsync(q1idto);

                    if (invoiceDtos1 != null && invoiceDtos1.Count > 0)
                    {
                        if (IsAPCopy)
                        {
                            var invoiceAp = invoiceDtos1.Where(x => x.InvoiceType == 0).ToList();
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
                            var invoiceDc = invoiceDtos1.Where(x => x.InvoiceType == 1).ToList();
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
                            var invoiceAr = invoiceDtos1.Where(x => x.InvoiceType == 2).ToList();
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
                 if (AirExportHawbDto is not null && !string.IsNullOrEmpty(AirExportHawbDto.HawbNo))
                        {

                            if (AirExportHawbDto.ExtraProperties == null)
                            {
                                AirExportHawbDto.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                            }
                            if (CopyCommodity)
                            {
                                if (AirExportHawbDto.Commodities != null)
                                {
                                    AirExportHawbDto.ExtraProperties.Add("Commodities", AirExportHawbDto.Commodities);
                                }
                            }
                            if (AirExportHawbDto.OtherCharges != null)
                            {
                                AirExportHawbDto.ExtraProperties.Add("OtherCharges", AirExportHawbDto.OtherCharges);
                            }


                            var addHawb = ObjectMapper.Map<AirExportHawbDto, CreateUpdateAirExportHawbDto>(AirExportHawbDto);
                            addHawb.MawbId = MawbId;

                           var newHbl= await _airExportHawbAppService.CreateAsync(addHawb);

                        if (AccountingInfo)
                        {
                            QueryInvoiceDto q1idto = new QueryInvoiceDto() { QueryType = 4, ParentId = AirExportHawbDto.Id };
                            var invoiceDtos1 = await _invoiceAppService.QueryInvoicesAsync(q1idto);
                          
                            if (invoiceDtos1 != null && invoiceDtos1.Count > 0)
                            {
                                if (IsAPCopy)
                                {
                                    var invoiceAp = invoiceDtos1.Where(x => x.InvoiceType == 0).ToList();
                                    foreach (var invoice in invoiceAp)
                                    {

                                        var newInvoiceAp = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                        newInvoiceAp.HawbId = newHbl.Id;
                                        newInvoiceAp.Id = Guid.Empty;
                                      var createInvoice=  await _invoiceAppService.CreateAsync(newInvoiceAp);
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
                                    var invoiceDc = invoiceDtos1.Where(x => x.InvoiceType == 1).ToList();
                                foreach (var invoice in invoiceDc)
                                    {

                                        var newInvoiceDc = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                        newInvoiceDc.HawbId = newHbl.Id;
                                        newInvoiceDc.Id = Guid.Empty;
                                      var createInvoice=  await _invoiceAppService.CreateAsync(newInvoiceDc);
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
                                    var invoiceAr = invoiceDtos1.Where(x => x.InvoiceType == 2).ToList();
                                    foreach (var invoice in invoiceAr)
                                    {

                                        var newInvoiceAr = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                        newInvoiceAr.HawbId = newHbl.Id;
                                        newInvoiceAr.Id = Guid.Empty;
                                       var createInvoice= await _invoiceAppService.CreateAsync(newInvoiceAr);
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

                    }

                if (CopyHawb == "AllHAWB")
                {
                        var hbls = await _airExportHawbAppService.GetHblCardsById(AirExportMawbDto.Id);
                        var newHbls = hbls.Where(x => x.Id != AirExportHawbDto.Id).ToList();
                        foreach (var hbl in newHbls)
                        {
                            if (hbl is not null && !string.IsNullOrEmpty(hbl.HawbNo))
                            {

                                if (hbl.ExtraProperties == null)
                                {
                                    hbl.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                                }
                                if (!CopyCommodity)
                                {

                                    hbl.ExtraProperties.Remove("Commodities");

                                }



                                var addHawb = ObjectMapper.Map<AirExportHawbDto, CreateUpdateAirExportHawbDto>(AirExportHawbDto);
                                addHawb.MawbId = MawbId;

                             var  newHbl= await _airExportHawbAppService.CreateAsync(addHawb);
                            if (AccountingInfo)
                            {
                                QueryInvoiceDto q1idto = new QueryInvoiceDto() { QueryType = 4, ParentId = hbl.Id };
                                var invoiceDtos1 = await _invoiceAppService.QueryInvoicesAsync(q1idto);
                               
                                if (invoiceDtos1 != null && invoiceDtos1.Count > 0)
                                {
                                    if (IsAPCopy)
                                    {
                                        var invoiceAp = invoiceDtos1.Where(x => x.InvoiceType == 0).ToList();
                                        foreach (var invoice in invoiceAp)
                                        {

                                            var newInvoiceAp = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                            newInvoiceAp.HawbId = newHbl.Id;
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
                                        var invoiceDc = invoiceDtos1.Where(x => x.InvoiceType == 1).ToList();
                                        foreach (var invoice in invoiceDc)
                                        {

                                            var newInvoiceDc = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                            newInvoiceDc.HawbId = newHbl.Id;
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
                                        var invoiceAr = invoiceDtos1.Where(x => x.InvoiceType == 2).ToList();
                                        foreach (var invoice in invoiceAr)
                                        {

                                            var newInvoiceAr = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                                            newInvoiceAr.HawbId = newHbl.Id;
                                            newInvoiceAr.Id = Guid.Empty;
                                          var createInvoice=  await _invoiceAppService.CreateAsync(newInvoiceAr);
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
                        }

                        }
                    }
                
            }
            else
            {
                MawbModel.FilingNo = SetAirExportFileNo();
                MawbModel.PostDate = Clock.Now.ClearTime();
                var NewMawab = ObjectMapper.Map<CreateMawbViewModel, CreateUpdateAirExportMawbDto>(MawbModel);

                if (NewMawab.ExtraProperties == null)
                {
                    NewMawab.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                }
                if (MoreInformationPrepaid != null || MoreInformationCollect != null)
                {
                    MoreInformations.Add(MoreInformationPrepaid);
                    MoreInformations.Add(MoreInformationCollect);


                    NewMawab.ExtraProperties.Add("MoreInformation", MoreInformations);

                }
                if (NewMawab.OtherCharges != null)
                {

                    NewMawab.ExtraProperties.Add("OtherCharges", NewMawab.OtherCharges);

                }
                if (Commodities != null)
                {
                    NewMawab.ExtraProperties.Add("Commodities", Commodities);
                }
                if (AccountingInformation != null)
                {
                    NewMawab.ExtraProperties.Add("AccountingInformation", AccountingInformation);
                }

                var inputDto = await _airExportMawbAppService.CreateAsync(NewMawab);
                MawbId = inputDto.Id;

                if (AirExportHawbDto is not null && !string.IsNullOrEmpty(AirExportHawbDto.HawbNo))
                {
                    if (AirExportHawbDto.ExtraProperties == null)
                    {
                        AirExportHawbDto.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                    }

                    if (AirExportHawbDto.Commodities != null)
                    {
                        AirExportHawbDto.ExtraProperties.Add("Commodities", AirExportHawbDto.Commodities);
                    }

                    if (AirExportHawbDto.OtherCharges != null)
                    {
                        AirExportHawbDto.ExtraProperties.Add("OtherCharges", AirExportHawbDto.OtherCharges);
                    }


                    var addHawb = ObjectMapper.Map<AirExportHawbDto, CreateUpdateAirExportHawbDto>(AirExportHawbDto);
                    addHawb.MawbId = MawbId;

                    await _airExportHawbAppService.CreateAsync(addHawb);
                }
            }
            Dictionary<string, Guid> rs = new()
            {
                { "id", MawbId}
            };
            return new JsonResult(rs);

        }

        #region FillTradePartnerAsync()
        private async Task FillTradePartnerAsync() {
            var tradePartnerLookup = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            TradePartnerLookupList = tradePartnerLookup.Items
                                                .Select(x => new SelectListItem(x.TPName + " / " + x.TPCode, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillSubstationAsync()
        private async Task FillSubstationAsync() {
            var substationLookup = await _substationAppService.GetSubstationsLookupAsync();
            SubstationLookupList = substationLookup.Items
                                                .Select(x => new SelectListItem(x.SubstationName + "  (" + x.AbbreviationName + ")", x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillAirportAsync()
        private async Task FillAirportAsync()
        {
            var airportLookup = await _airportAppService.GetAirportLookupAsync();
            AirportLookupList = airportLookup.Items
                                                .Select(x => new SelectListItem(x.AirportIataCode + " " + x.AirportName, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillWtVal()
        private void FillWtValOther()
        {
            WtValOtherList = new List<SelectListItem>
            {
                new SelectListItem { Value = "PPD", Text = L["PPD"]},
                new SelectListItem { Value = "COLL", Text = L["COLL"]}
            };
        }
        #endregion

        #region FillPackageUnitAsync()
        private async Task FillPackageUnitAsync()
        {
            var packageUnitLookup = await _packageUnitAppService.GetPackageUnitsLookupAsync();
            PackageUnitLookupList = packageUnitLookup.Items
                                                .Select(x => new SelectListItem(x.PackageName, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion


        #region SetAirExportFileNo() 
        /// <summary>
        /// ³]©wAir ExportªºFile No
        /// </summary>
        private string SetAirExportFileNo()
        {
            string today = DateTime.Now.ToString("yyyyMMddhhmmss");
            string AEFileNo = "AXE-" + today;
            return AEFileNo;
        }
        #endregion


        #region CreateMawbViewModel
        public class CreateMawbViewModel
        {
            //public Guid Id { get; set; }
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
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans1ArrivalDate { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans1DepatureDate { get; set; }
            public string RouteTrans1FlightNo { get; set; }
            public String RouteTrans1CarrierId { get; set; }

            public String RouteTrans2Id { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans2ArrivalDate { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans2DepatureDate { get; set; }
            public string RouteTrans2FlightNo { get; set; }
            public String RouteTrans2CarrierId { get; set; }

            public String RouteTrans3Id { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans3ArrivalDate { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? RouteTrans3DepatureDate { get; set; }
            public string RouteTrans3FlightNo { get; set; }
            public String RouteTrans3CarrierId { get; set; }

            public double Package { get; set; }
            public String MawbPackageUnitId { get; set; }
            public double BuyingRate { get; set; }
            public RateUnitType? BuyingRateUnitType { get; set; }
            public double SellingRate { get; set; }
            public RateUnitType? SellingRateUnitType { get; set; }
            public double VolumeWeightKg { get; set; }
            public double VolumeWeightCbm { get; set; }

            public double GrossWeightKg { get; set; }
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
           
            [DataType(DataType.Date)]
            public DateTime? AwbCancelledDate { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String AwbCancelledOpId { get; set; }
            public ReasonForCancelType? ReasonForCancel { get; set; }
            [SelectItems(nameof(TradePartnerLookupList))]
            public String BusinessReferredId { get; set; }
            public bool IsECom { get; set; }
            public string PONo { get; set; }
            public List<OtherCharges> OtherCharges { get; set; }
            public ExtraPropertyDictionary ExtraProperties { get; set; }
        }
        #endregion
        private List<SelectListItem> GetIncotermsSelectList()
        {
            var enumValues = Enum.GetValues(typeof(AccountingInformationCode))
                                  .Cast<AccountingInformationCode>()
                                  .Select(e => new SelectListItem
                                  {
                                      Text = L["Enum:" + e.ToString()],
                                      Value = e.ToString(),

                                  })
                                  .ToList();

            return enumValues;
        }
        private List<SelectListItem> GetChargeItemSelectList()
        {
            var enumValues = Enum.GetValues(typeof(ChargeItems))
                                  .Cast<ChargeItems>()
                                  .Select(e => new SelectListItem
                                  {
                                      Text = L["Enum:ItemCharge." + e.ToString()],
                                      Value = e.ToString(),

                                  })
                                  .ToList();

            return enumValues;
        }
    }
}
