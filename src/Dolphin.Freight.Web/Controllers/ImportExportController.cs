using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.Web.Pages.AirImports;
using Dolphin.Freight.Settings.PortsManagement;
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
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas;
using Org.BouncyCastle.Asn1.Mozilla;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.Accounting.InvoiceBills;

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
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IAttachmentAppService _attachmentAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IPortsManagementAppService _portsManagementAppService;
        private readonly IVesselScheduleAppService _vesselScheduleAppService;
        private readonly IExportBookingAppService _exportBookingAppService;
        private readonly IContainerAppService _containerAppService;
        private readonly IInvoiceBillAppService _invoiceBillAppService;

        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        public List<SelectListItem> CountryName { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }
        public List<SelectListItem> PortsManagementLookupList { get; set; }

        private readonly int fileType = 10;
        public ImportExportController(ITradePartnerAppService tradePartnerAppService,
            ISubstationAppService substationAppService,
            IAirportAppService airportAppService,
            IPackageUnitAppService packageUnitAppService,
            IAirImportHawbAppService airImportHawbAppService,
            IAirExportHawbAppService airExportHawbAppService,
            IOceanExportHblAppService oceanExportHblAppService,
            IOceanExportMblAppService oceanExportMblAppService,
            IOceanImportHblAppService oceanImportHblAppService,
            IAttachmentAppService attachmentAppService,
            IInvoiceAppService invoiceAppService,
            IPortsManagementAppService portsManagementAppService,
            IVesselScheduleAppService vesselScheduleAppService,
            IExportBookingAppService exportBookingAppService,
            IContainerAppService containerAppService,
            IInvoiceBillAppService invoiceBillAppService
            )
        {
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _airImportHawbAppService = airImportHawbAppService;
            _airExportHawbAppService = airExportHawbAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _attachmentAppService = attachmentAppService;
            _invoiceAppService = invoiceAppService;
            _portsManagementAppService = portsManagementAppService;
            _vesselScheduleAppService = vesselScheduleAppService;
            _exportBookingAppService = exportBookingAppService;
            _containerAppService = containerAppService;
            _invoiceBillAppService = invoiceBillAppService;


            FillCountryNameAsync().Wait();
            FillTradePartnerAsync().Wait();
            FillSubstationAsync().Wait();
            FillAirportAsync().Wait();
            FillPackageUnitAsync().Wait();
            FillPortAsync().Wait();
        }



        #region AirImports

        [HttpGet]
        [Route("AirImportBasicHawb")]
        public async Task<PartialViewResult> GetAirImportBasicHawb(Guid Id)
        {
            HawbHblViewModel model = new();

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

            QueryInvoiceDto qidto = new QueryInvoiceDto() { QueryType = 4, ParentId = Id };
            var invoiceDtos = await _invoiceAppService.QueryInvoicesAsync(qidto);
            model.h0invoiceDtos = new List<InvoiceDto>();
            model.h1invoiceDtos = new List<InvoiceDto>();
            model.h2invoiceDtos = new List<InvoiceDto>();
            if (invoiceDtos != null && invoiceDtos.Count > 0)
            {
                foreach (var dto in invoiceDtos)
                {
                    switch (dto.InvoiceType)
                    {
                        default:
                            model.h0invoiceDtos.Add(dto);
                            break;
                        case 1:
                            model.h1invoiceDtos.Add(dto);
                            break;
                        case 2:
                            model.h2invoiceDtos.Add(dto);
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
        public async Task<PartialViewResult> GetAirExportBasicHawb(Guid Id, string hawbNo)
        {
            FillWtValOther();

            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;
            model.WtValOtherList = WtValOtherList;
            model.CountryName = CountryName;

            model.AirExportHawbDto = await _airExportHawbAppService.GetHawbCardById(Id);
            model.AirExportHawbDto.CurrentHawbNo = hawbNo;


            if( Id == Guid.Empty)
            {
                model.AirExportHawbDto.BookingDate = DateTime.Now;  
            }
           
            return PartialView("~/Pages/AirExports/_AirExportBasicHawb.cshtml", model);
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

        #endregion

        #region Ocean Imports


        [Route("OceanImportBasicHbl")]
        public async Task<PartialViewResult> GetOceanImportBasicHbl(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;

            model.OceanImportHbl = await _oceanImportHblAppService.GetHblCardById(Id);

            return PartialView("~/Pages/OceanImports/_OceanImportBasicHbl.cshtml", model);
        }

        #endregion

        #region Ocean Exports

        [HttpGet]
        [Route("OceanExportBasicHbl")]
        public async Task<PartialViewResult> GetOceanExportBasicHbl(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;
            model.PortsManagementLookupList = PortsManagementLookupList;

            model.OceanExportHbl = await _oceanExportHblAppService.GetHblCardById(Id);

            return PartialView("~/Pages/OceanExports/_OceanExportBasicHbl.cshtml", model);
        }

        [HttpGet]
        [Route("OceanExportAccountingHbl")]
        public async Task<PartialViewResult> GetOceanExportAccountingHbl(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;

            model.OceanExportHblDto = await _oceanExportHblAppService.GetHblCardById(Id);

            QueryInvoiceDto qidto = new QueryInvoiceDto() { QueryType = 1, ParentId = Id };
            var invoiceDtos = await _invoiceAppService.QueryInvoicesAsync(qidto);
            model.h0invoiceDtos = new List<InvoiceDto>();
            model.h1invoiceDtos = new List<InvoiceDto>();
            model.h2invoiceDtos = new List<InvoiceDto>();
            if (invoiceDtos != null && invoiceDtos.Count > 0)
            {
                foreach (var dto in invoiceDtos)
                {
                     switch (dto.InvoiceType)
                    {
                        default:
                            break;
                        case 0:
                            model.h0invoiceDtos.Add(dto);
                            break;
                        case 1:
                            model.h1invoiceDtos.Add(dto);
                            break;
                        case 2:
                            model.h2invoiceDtos.Add(dto);
                            break;
                    }
                }
            }
            qidto.ParentId = Id;

            return PartialView("~/Pages/OceanExports/_OceanExportAccountingHbl.cshtml", model);
        }

        #endregion


        #region Export Bookings
        [HttpGet]
        [Route("ExportBookingBasic")]
        public async Task<PartialViewResult> GetExportBookingBasic(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;
            model.PortsManagementLookupList = PortsManagementLookupList;
            model.RateUnitTypeLookupList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "KG", Value = "KG" },
                new SelectListItem() { Text = "LG", Value = "LG" }
            };
            model.UnitTypeLookupList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "CBM", Value = "CBM" },
                new SelectListItem() { Text = "CFT", Value = "CFT" }
            };

            model.ExportBookingDto = await _exportBookingAppService.GetBookingCardById(Id);

            return PartialView("~/Pages/OceanExports/VesselSchedules/_ExportBookingBasic.cshtml", model);
        }
        #endregion


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
        [Route("OceanImportHawb")]
        public async Task<PartialViewResult> GetOceanImportHawbHBL(Guid Id)
        {
            HawbHblViewModel model = new();

            model.SubstationLookupList = SubstationLookupList;
            model.AirportLookupList = AirportLookupList;
            model.TradePartnerLookupList = TradePartnerLookupList;
            model.PackageUnitLookupList = PackageUnitLookupList;

            model.OceanImportHbl = await _oceanImportHblAppService.GetHblCardById(Id);

            return PartialView("~/Pages/OceanImports/_OceanImportHawb.cshtml", model);

        }

        #region FillCountryNameAsync()
        private async Task FillCountryNameAsync()
        {
            var countryName = await _tradePartnerAppService.GetCountriesLookupAsync();
            CountryName = countryName.Items
                                     .Select(x => new SelectListItem(x.CountryName, x.Id.ToString(), false))
                                     .ToList();
        }
        #endregion

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
                new SelectListItem { Value = "PPD", Text = L["PPD"]},
                new SelectListItem { Value = "COLL", Text = L["COLL"]}
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

        #region GenerateSerialNumber()
        public string GenerateSerialNumber()
        {
            DateTime now = DateTime.Now;
            string serialNumber = now.ToString("yyyyMMddHHmmss");
            return serialNumber;
        }
        #endregion

        #region FillPortAsync()
        private async Task FillPortAsync()
        {
            var lookup = await _portsManagementAppService.QueryListAsync();

            PortsManagementLookupList = lookup.Select(x => new SelectListItem(x.PortName, x.Id.ToString(), false))
                                   .ToList();
        }
        #endregion

        [HttpGet]
        [Route("GetBookingByReference")]
        public async Task<IActionResult> GetBookingByReference(string refNo)
        {
            CreateUpdateExportBookingDto info = new CreateUpdateExportBookingDto();

            var Vessels = await _vesselScheduleAppService.GetListAsync(new QueryVesselScheduleDto());

            info.CarrierId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.MblCarrierId).FirstOrDefault();
            info.HblAgentId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.MblOverseaAgentId).FirstOrDefault();
            info.FreightTermForBuyerId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.FreightTermId).FirstOrDefault();
            info.FreightTermForSalerId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.FreightTermId).FirstOrDefault();
            info.ShippingAgentId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.ShippingAgentId).FirstOrDefault();
            info.ShipModeId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.ShipModeId).FirstOrDefault();
            info.VesselName = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.VesselName).FirstOrDefault();
            info.Voyage = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.Voyage).FirstOrDefault();
            info.PorId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.PorId).FirstOrDefault();
            info.PorEtd = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.PorEtd).FirstOrDefault();
            info.PolId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.PolId).FirstOrDefault();
            info.PolEtd = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.PolEtd).FirstOrDefault();
            info.PodId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.PodId).FirstOrDefault();
            info.PodEta = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.PodEta).FirstOrDefault();
            info.DelId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.DelId).FirstOrDefault();
            info.DelEta = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.DelEta).FirstOrDefault();
            info.FdestId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.FdestId).FirstOrDefault();
            info.FdestEta = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.FdestEta).FirstOrDefault();
            info.OfficeId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.OfficeId).FirstOrDefault();
            info.EmptyPickupId = Vessels.Where(w => w.ReferenceNo == refNo).Select(s => s.EmptyPickupId).FirstOrDefault();

            return Json(new JsonResult(info));
        }

        [HttpPost]
        public async Task<IActionResult> CopyMblDetails(Guid id, bool allHbl, string containerOption, string invoiceOption)
        {
            var newMblId = await CopyMbl(id);

            if (containerOption == "container")
            {
                await CopyContainers(id, newMblId);
            }

            if (invoiceOption == "invoices")
            {
                await CopyInvoices(id, newMblId);
            }

            if (allHbl)
            {
                await CopyAllHbl(id, newMblId, containerOption, invoiceOption);
            }
            else
            {
                await CopySingleHbl(id, newMblId, containerOption, invoiceOption);
            }

            return Json(new { NewMblId = newMblId });
        }

        [HttpPost]
        [Route("VesselSchedulesCopy")]
        public async Task<IActionResult> VesselSchedulesCopy(Guid id, string copyVesselOnly)
        {
            if (copyVesselOnly == "vesselOnly")
            {
                var vesselData = await _vesselScheduleAppService.GetAsync(id);
                var updatedVessel = ObjectMapper.Map<VesselScheduleDto, CreateUpdateVesselScheduleDto>(vesselData);
                updatedVessel.Id = Guid.Empty;

                var newVessel = await _vesselScheduleAppService.CreateAsync(updatedVessel);
                newVessel.isNewVessel = true;

                return Json(new { NewVesselId = newVessel.Id, status = newVessel.isNewVessel });
            }

            return Ok();
        }

        #region Private Functions

        private async Task<Guid> CopyMbl(Guid id)
        {
            var mbldata = await _oceanExportMblAppService.GetAsync(id);
            var updatedMbl = ObjectMapper.Map<OceanExportMblDto, CreateUpdateOceanExportMblDto>(mbldata);
            updatedMbl.Id = Guid.Empty;

            var newMbl = await _oceanExportMblAppService.CreateAsync(updatedMbl);
            return newMbl.Id;
        }

        private async Task CopyContainers(Guid parentId, Guid newParentId, bool isHbl = false)
        {
            var containerdata = isHbl
                ? await _containerAppService.GetContainerListByHblId(parentId)
                : await _containerAppService.GetContainerByMblId(parentId);

            foreach (var container in containerdata)
            {
                if (isHbl)
                    container.HblId = newParentId;
                else
                    container.MblId = newParentId;

                container.Id = Guid.Empty;
                await _containerAppService.CreateAsync(container);
            }
        }

        private async Task CopyInvoices(Guid parentId, Guid newParentId, int queryType = 3)
        {
            QueryInvoiceDto queryInvoiceDto = new QueryInvoiceDto() { ParentId = parentId, QueryType = queryType };
            var invoicedata = await _invoiceAppService.QueryInvoicesAsync(queryInvoiceDto);

            foreach (var invoice in invoicedata)
            {
                var updatedInvoices = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                updatedInvoices.Id = Guid.Empty;

                if (queryType == 3)
                    updatedInvoices.MblId = newParentId;
                else if (queryType == 1)
                    updatedInvoices.HblId = newParentId;

                var newInvoice = await _invoiceAppService.CreateAsync(updatedInvoices);
                var newInvoiceId = newInvoice.Id;

                foreach (var invoicebilldto in invoice.InvoiceBillDtos)
                {
                    invoicebilldto.InvoiceId = newInvoiceId;
                    invoicebilldto.Id = Guid.Empty;

                    await _invoiceBillAppService.CreateAsync(invoicebilldto);
                }
            }
        }

        private async Task CopyAllHbl(Guid parentId, Guid newMblId, string containerOption, string invoiceOption)
        {
            var hbldata = await _oceanExportHblAppService.GetHblCardsById(parentId);

            foreach (var hbl in hbldata)
            {
                var updatedHbls = ObjectMapper.Map<OceanExportHblDto, CreateUpdateOceanExportHblDto>(hbl);
                updatedHbls.MblId = newMblId;
                updatedHbls.Id = Guid.Empty;

                var newHbl = await _oceanExportHblAppService.CreateAsync(updatedHbls);
                var newHblId = newHbl.Id;

                if (containerOption == "container")
                {
                    await CopyContainers(hbl.Id, newHblId, true);
                }

                if (invoiceOption == "invoices")
                {
                    await CopyInvoices(hbl.Id, newHblId, 1);
                }
            }
        }

        private async Task CopySingleHbl(Guid parentId, Guid newMblId, string containerOption, string invoiceOption)
        {
            var hbldata = await _oceanExportHblAppService.GetHblCardsById(parentId);
            
            if (hbldata.Any())
            {
                var hbl = hbldata[0];

                var updatedHbl = ObjectMapper.Map<OceanExportHblDto, CreateUpdateOceanExportHblDto>(hbl);
                updatedHbl.MblId = newMblId;
                updatedHbl.Id = Guid.Empty;

                var newHbl = await _oceanExportHblAppService.CreateAsync(updatedHbl);
                var newHblId = newHbl.Id;

                if (containerOption == "container")
                {
                    await CopyContainers(hbl.Id, newHblId, true);
                }

                if (invoiceOption == "invoices")
                {
                    await CopyInvoices(hbl.Id, newHblId, 1);
                }
            }
        }

        #endregion
    }
}
