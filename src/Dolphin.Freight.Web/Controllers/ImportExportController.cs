using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.Web.Pages.AirImports;
using Dolphin.Freight.Web.Pages.Sales.TradePartner;
using Dolphin.Freight.Web.ViewModels.ImportExport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Dolphin.Freight.Accounting.Invoices;

namespace Dolphin.Freight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportExportController : AbpController
    {
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly IAirImportHawbAppService _airImportHawbAppService;
        private readonly IAirExportHawbAppService _airExportHawbAppService;
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IAttachmentAppService _attachmentAppService;
        private readonly IInvoiceAppService _invoiceAppService;

        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }

        private readonly int fileType = 10;
        public ImportExportController(ITradePartnerAppService tradePartnerAppService,
            ISubstationAppService substationAppService,
            IAirportAppService airportAppService,
            IPackageUnitAppService packageUnitAppService,
            IAirImportHawbAppService airImportHawbAppService,
            IAirExportHawbAppService airExportHawbAppService,
            IOceanExportHblAppService oceanExportHblAppService,
            IOceanImportHblAppService oceanImportHblAppService,
            IAttachmentAppService attachmentAppService,
            IInvoiceAppService invoiceAppService
            )
        {
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airImportHawbAppService = airImportHawbAppService;
            _airExportHawbAppService = airExportHawbAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _attachmentAppService = attachmentAppService;
            _invoiceAppService = invoiceAppService;

            FillTradePartnerAsync().Wait();
            FillSubstationAsync().Wait();
            FillAirportAsync().Wait();
            FillPackageUnitAsync().Wait();
        }



        #region AirImports

        [HttpGet]
        [Route("AirImportBasicHawb")]
        public async Task<PartialViewResult> GetAirImportBasicHawb(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;

            model.HawbModel = await _airImportHawbAppService.GetHawbCardById(Id);

            return PartialView("~/Pages/AirImports/_AirImportBasicHawb.cshtml", model);
        }

        [HttpGet]
        [Route("AirImportAccountHawb")]
        public async Task<PartialViewResult> GetAirImportAccountHawb(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;

            model.AirImportHawbDto = await _airImportHawbAppService.GetHawbCardById(Id);

            QueryInvoiceDto qidto = new QueryInvoiceDto() { QueryType = 3, ParentId = Id };
            var invoiceDtos = await _invoiceAppService.QueryInvoicesAsync(qidto);
            model.m0invoiceDtos =  new List<InvoiceDto>();
            model.m1invoiceDtos = new List<InvoiceDto>();
            model.m2invoiceDtos = new List<InvoiceDto>();
            if (invoiceDtos != null && invoiceDtos.Count > 0)
            {
                foreach (var dto in invoiceDtos)
                {
                    switch (dto.InvoiceType)
                    {
                        default:
                            model.m0invoiceDtos.Add(dto);
                            break;
                        case 1:
                            model.m1invoiceDtos.Add(dto);
                            break;
                        case 2:
                            model.m2invoiceDtos.Add(dto);
                            break;
                    }
                }
            }
            qidto.ParentId = Id;
            

            return PartialView("~/Pages/AirImports/_AirImportAccountHawb.cshtml", model);

        }

        [HttpGet]
        [Route("AirImportDocCenterHawb")]
        public async Task<PartialViewResult> GetAirImportDocCenterHawb(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;


            model.AirImportHawbDto = await _airImportHawbAppService.GetDocCenterCardById(Id);

            QueryAttachmentDto dto = new()
            {
                QueryId = Id,
                QueryType = fileType,
            };

            model.FileList = await _attachmentAppService.QueryListAsync(dto);

            return PartialView("~/Pages/AirImports/DocCenter/_AirImportDocCenterHawb.cshtml", model);
        }

        #endregion

        #region AirExports

        [HttpGet]
        [Route("AirExportBasicHawb")]
        public async Task<PartialViewResult> GetAirExportBasicHawb(Guid Id)
        {
            FillWtValOther();

            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;
            model.WtValOtherList = WtValOtherList;

            model.AirExportHawbDto = await _airExportHawbAppService.GetHawbCardById(Id);
            

            return PartialView("~/Pages/AirExports/_AirExportBasicHawb.cshtml", model);
        }

        #endregion


        [Route("OceanHBL")]
        public async Task<PartialViewResult> GetOceanExportHBL(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;

            model.OceanExportHblDto = await _oceanExportHblAppService.GetHblCardById(Id);

            return PartialView("~/Pages/OceanExports/_OceanExportAccountingHawb.cshtml", model);
        }

        

        

        [HttpGet]
        [Route("AirDocCenterHBL")]
        public async Task<PartialViewResult> GetDocCenterHBL(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;

            FillWtValOther();

            model.WtValOtherList = WtValOtherList;

            model.AirExportHawbDto = await _airExportHawbAppService.GetHawbCardById(Id);

            return PartialView("~/Pages/AirExports/_AirExportHawbHBL.cshtml", model);
        }
        

        [HttpGet]
        [Route("OceanExportHawb")]
        public async Task<PartialViewResult> GetOceanExportHawbHBL(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;

            model.OceanExportHbl = await _oceanExportHblAppService.GetHblCardById(Id);
            return PartialView("~/Pages/OceanExports/_OceanExportHawb.cshtml", model);
        }

        [HttpGet]
        [Route("OceanImportHawb")]
        public async Task<PartialViewResult> GetOceanImportHawbHBL(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;

            model.OceanImportHbl = await _oceanImportHblAppService.GetHawbCardById(Id);

            return PartialView("~/Pages/OceanImports/_OceanImportHawb.cshtml", model);

        }

        [HttpGet]
        [Route("AirExportDocCenterHawb")]
        public async Task<PartialViewResult> GetAirExportDocCenterHawb(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;


            model.AirExportHawbDto = await _airExportHawbAppService.GetDocCenterCardById(Id);

            QueryAttachmentDto dto = new()
            {
                QueryId = Id,
                QueryType = fileType,
            };

            model.FileList = await _attachmentAppService.QueryListAsync(dto);

            return PartialView("~/Pages/AirExports/DocCenter/_AirEportDocCenterHawb.cshtml", model);
        }

        #region FillTradePartnerAsync()
        private async Task FillTradePartnerAsync()
        {
            var tradePartnerLookup = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            TradePartnerLookupList = tradePartnerLookup.Items
                                                .Select(x => new SelectListItem(x.TPName + " / " + x.TPCode, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillSubstationAsync()
        private async Task FillSubstationAsync()
        {
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

        #region FillPackageUnitAsync()
        private async Task FillPackageUnitAsync()
        {
            var packageUnitLookup = await _packageUnitAppService.GetPackageUnitsLookupAsync();
            PackageUnitLookupList = packageUnitLookup.Items
                                                .Select(x => new SelectListItem(x.PackageName, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillWtVal()
        private void FillWtValOther()
        {
            WtValOtherList = new List<SelectListItem>
            {
                new SelectListItem { Value = "PPD", Text = "PPD"},
                new SelectListItem { Value = "COLL", Text = "COLL"}
            };
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
    }
}
