using Dolphin.Freight.Common;
using Volo.Abp.Users;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ReportLog;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.Web.ViewModels.BankDraft;
using Dolphin.Freight.Web.ViewModels.BookingConfirmation;
using Dolphin.Freight.Web.ViewModels.BookingPackingList;
using Dolphin.Freight.Web.ViewModels.CertificateOfOrigin;
using Dolphin.Freight.Web.ViewModels.CommercialInvoice;
using Dolphin.Freight.Web.ViewModels.DockRecepit;
using Dolphin.Freight.Web.ViewModels.ForwarderCargoReceipt;
using Dolphin.Freight.Web.ViewModels.Hbl;
using Dolphin.Freight.Web.ViewModels.HblClauses;
using Dolphin.Freight.Web.ViewModels.HblPackageLabel;
using Dolphin.Freight.Web.ViewModels.Manifest;
using Dolphin.Freight.Web.ViewModels.Package;
using Dolphin.Freight.Web.ViewModels.PackageLabel;
using Dolphin.Freight.Web.ViewModels.PickupDeliveryOrder;
using Dolphin.Freight.Web.ViewModels.PreAlert;
using Dolphin.Freight.Web.ViewModels.Reports;
using Dolphin.Freight.Web.ViewModels.Telexrelease;
using Dolphin.Freight.Web.ViewModels.UsdaHeatTreatment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Wkhtmltopdf.NetCore;
using static Dolphin.Freight.Permissions.OceanExportPermissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Dolphin.Freight.Web.CommonService;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.Web.ViewModels.PackagingListAirExportHawb;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Accounting.InvoiceBills;

using Dolphin.Freight.Web.ViewModels.CertificateOfOriginAirExportHawb;
using NPOI.OpenXmlFormats.Wordprocessing;
using JetBrains.Annotations;
using System.Runtime.Intrinsics.Arm;
using Microsoft.CodeAnalysis.CSharp;
using System.Configuration;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.AirImports;
using Dolphin.Freight.ImportExport;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.Web.Pages.OceanExports;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NPOI.SS.Formula.Functions;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.ImportExport.OceanImports;
using QueryHblDto = Dolphin.Freight.ImportExport.OceanExports.QueryHblDto;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text;
using Dolphin.Freight.Web.ViewModels.DeliveryOrder;
using Dolphin.Freight.Settinngs.ContainerSizes;
using Dolphin.Freight.ImportExport.OceanImports;


namespace Dolphin.Freight.Web.Controllers
{
    public class DocsController : AbpController
    {
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly ImportExport.OceanImports.IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IGeneratePdf _generatePdf;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly IReportLogAppService _reportLogAppService;
        private readonly ICurrentUser _currentUser;
        private readonly IDropdownService _dropdownService;
        private readonly IExportBookingAppService _exportBookingAppService;
        private readonly IAirExportMawbAppService _airExportMawbAppService;
        private readonly IAirExportHawbAppService _airExportHawbAppService;
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        private readonly IAirImportHawbAppService _airImportHawbAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IContainerAppService _containerAppService;
        private readonly IContainerSizeAppService _containerSizeAppService;
      
        private readonly IOceanImportMblAppService _oceanImportMblAppService;

        private Dolphin.Freight.ReportLog.ReportLogDto ReportLog;
        public IList<OceanExportHblDto> OceanExportHbls { get; set; }
        public DocsController(IOceanExportMblAppService oceanExportMblAppService, IOceanExportHblAppService oceanExportHblAppService, ISysCodeAppService sysCodeAppService, IGeneratePdf generatePdf, IAjaxDropdownAppService ajaxDropdownAppService, IReportLogAppService reportLogAppService,
          ICurrentUser currentUser, IDropdownService dropdownService, IExportBookingAppService exportBookingAppService,
          IAirExportMawbAppService airExportMawbAppService,
          IAirExportHawbAppService airExportHawbAppService,
          IInvoiceAppService invoiceAppService,
          IAirImportMawbAppService airImportMawbAppService,
          IAirImportHawbAppService airImportHawbAppService,
          IContainerAppService containerAppService,
          ImportExport.OceanImports.IOceanImportHblAppService oceanImportHblAppService,
          IContainerSizeAppService containerSizeAppService,
        
          
          IOceanImportMblAppService oceanImportMblAppService)
        {
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _sysCodeAppService = sysCodeAppService;
            _generatePdf = generatePdf;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _reportLogAppService = reportLogAppService;
            _currentUser = currentUser;
            _dropdownService = dropdownService;
            _exportBookingAppService = exportBookingAppService;
            _airExportHawbAppService = airExportHawbAppService;
            _airExportMawbAppService = airExportMawbAppService;
            _invoiceAppService = invoiceAppService;
            _airImportMawbAppService = airImportMawbAppService;
            _airImportHawbAppService = airImportHawbAppService;
            _containerAppService = containerAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _oceanImportMblAppService = oceanImportMblAppService;

          
            _containerSizeAppService = containerSizeAppService;
            ReportLog = new ReportLog.ReportLogDto();

        }

        [HttpGet]
        public async Task<IActionResult> PreAlert(string id)
        {
            PreAlertIndexViewModel InfoViewModel = new PreAlertIndexViewModel();
            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetOceanExportDetailsById(Guid.Parse(id));
            var OceanExportMbl = await _oceanExportMblAppService.GetOceanExportDetailsById(OceanExportHbl.MblId);
            var container = await _containerAppService.GetContainerByHblId(Guid.Parse(id));
            #region
            //https://dlihq.gofreight.co/ocean/export/shipment/container/OE-23020004/?hbl=56
            InfoViewModel.Office = OceanExportMbl.Office.AbbreviationName; ;
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = OceanExportHbl.MblCustomerName;
            InfoViewModel.LastName = "";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.TO = "";
            InfoViewModel.JOBNO = OceanExportMbl.DocNo;
            InfoViewModel.ATTN = "";
            InfoViewModel.MBL = OceanExportHbl.MblNo;
            InfoViewModel.HBL = OceanExportHbl.HblNo;
            InfoViewModel.SHPR = OceanExportHbl.ShippingAgentName;
            InfoViewModel.CNEE = OceanExportHbl.HblConsigneeName;
            InfoViewModel.CARR = OceanExportHbl.MblCarrierName;
            InfoViewModel.VSLVOV = OceanExportHbl.VesselName + "" + OceanExportHbl.Voyage;
            InfoViewModel.POR = OceanExportMbl.PorName;
            InfoViewModel.POL = OceanExportMbl.PolName;
            InfoViewModel.ETD = OceanExportMbl.PolEtd?.ToString("dd-MM-yyyy");
            InfoViewModel._2NDVV = "";
            InfoViewModel.ETDKAO = "";
            InfoViewModel.POD = OceanExportMbl.PodName;
            InfoViewModel.ETA = OceanExportMbl.PodEta?.ToString("dd-MM-yyyy");
            InfoViewModel.DEST = OceanExportMbl.FdestName;
            InfoViewModel.DESTETA = OceanExportMbl.FdestEta?.ToString("dd-MM-yyyy");
            InfoViewModel.VOL = "";
            InfoViewModel.CNTRNO = container?.ContainerNo;/* "2 /  / 48HC" + Environment.NewLine + "4 /  / 40FR" + Environment.NewLine + "5 /  / 40";*/
            InfoViewModel.COMM = "";
            InfoViewModel.PONBR = "";
            InfoViewModel.PKGS = OceanExportHbl.TotalPackage.ToString();
            InfoViewModel.GWT = OceanExportHbl.TotalWeight.ToString();
            InfoViewModel.MSRMT = OceanExportHbl.TotalMeasure.ToString(); ;
            InfoViewModel.RMK = "";

            InfoViewModel.ReportId = Guid.Parse(id);

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PreAlert(PreAlertIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "PreAlert";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/PreAlert/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> Telexrelease(Guid oceanExportMblId)
        {
            TelexreleaseIndexViewModel InfoViewModel = new TelexreleaseIndexViewModel();

            OceanExportDetails oceanExportMbl = TempData["PrintData"] is not null
                                                                                ? JsonConvert.DeserializeObject<OceanExportDetails>(TempData["PrintData"].ToString())
                                                                                : await _oceanExportMblAppService.GetOceanExportDetailsById(oceanExportMblId);

            if (oceanExportMblId != Guid.Empty)
            {
                oceanExportMbl = await _oceanExportMblAppService.GetOceanExportDetailsById(oceanExportMblId);
            }

            InfoViewModel.Office = oceanExportMbl.Office.AbbreviationName; ;
            InfoViewModel.Address = "地址";
            InfoViewModel.Tel = "電話";
            InfoViewModel.Fax = "傳真";
            InfoViewModel.Email = _currentUser.Email;
            InfoViewModel.Email_U = _currentUser.Email.ToUpper();
            InfoViewModel.FirstName = oceanExportMbl.MblOperatorName;
            InfoViewModel.LastName = _currentUser.SurName;
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");
            InfoViewModel.Date_M = DateTime.Now.ToString("MMMM dd, yyyy", new CultureInfo("en-US"));
            InfoViewModel.To = oceanExportMbl.MblCarrierName;
            InfoViewModel.Attn = "EXPORT FREIGHT CASHER";
            InfoViewModel.From = "分站名稱";
            InfoViewModel.Vessel = Convert.ToString(oceanExportMbl.VesselName);
            InfoViewModel.Voyage = Convert.ToString(oceanExportMbl.Voyage);
            InfoViewModel.Mbl = Convert.ToString(oceanExportMbl.MblNo);
            InfoViewModel.Pol = oceanExportMbl.PolName;
            InfoViewModel.Pod = oceanExportMbl.PodName;
            InfoViewModel.Cnee = oceanExportMbl.MblOverseaAgentName;
            InfoViewModel.Consignee = Convert.ToString(oceanExportMbl.MblOverseaAgentName);

            InfoViewModel.TextArea = InfoViewModel.FirstName;

            InfoViewModel.ReportId = oceanExportMblId;

            TempData["PrintData"] = JsonConvert.SerializeObject(oceanExportMbl);

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Telexrelease(TelexreleaseIndexViewModel InfoModel)
        {
            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "Telexrelease";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/Telexrelease/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public IActionResult TelexreleaseCosco()
        {
            TelexreleaseIndexViewModel InfoViewModel = new TelexreleaseIndexViewModel();

            var OceanExportMbl = new CreateUpdateOceanExportMblDto();
            OceanExportMbl = JsonConvert.DeserializeObject<CreateUpdateOceanExportMblDto>(TempData["PrintData"].ToString());

            #region
            InfoViewModel.Office = "分站名稱";
            InfoViewModel.Address = "地址";
            InfoViewModel.Tel = "電話";
            InfoViewModel.Fax = "傳真";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.Email_U = InfoViewModel.Email.ToUpper();
            InfoViewModel.FirstName = "萬泰";
            InfoViewModel.LastName = "資訊部";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");
            InfoViewModel.Date_M = DateTime.Now.ToString("MMMM dd, yyyy", new CultureInfo("en-US"));
            InfoViewModel.To = OceanExportMbl.MblCarrierName == null ? "" : OceanExportMbl.MblCarrierName;
            InfoViewModel.Attn = "EXPORT FREIGHT CASHER";
            InfoViewModel.From = "分站名稱";
            InfoViewModel.Vessel = OceanExportMbl.VesselName == null ? "" : OceanExportMbl.VesselName;
            InfoViewModel.Voyage = OceanExportMbl.Voyage == null ? "" : OceanExportMbl.Voyage;
            InfoViewModel.Mbl = OceanExportMbl.FilingNo == null ? "" : OceanExportMbl.FilingNo;
            InfoViewModel.Pol = "裝貨港";
            InfoViewModel.Pod = "卸貨港";
            InfoViewModel.Cnee = "收貨人";
            InfoViewModel.Consignee = "海外代理";

            InfoViewModel.TextArea = InfoViewModel.LastName;

            InfoViewModel.ReportId = OceanExportMbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion
            TempData["PrintData"] = JsonConvert.SerializeObject(OceanExportMbl);

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TelexreleaseCosco(TelexreleaseIndexViewModel InfoModel)
        {
            return await _generatePdf.GetPdf("Views/Docs/Pdf/TelexreleaseCosco/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> PackageLabel(string datasource, Guid oceanExportMblId)
        {
            PackageLabelIndexViewModel InfoViewModel = new PackageLabelIndexViewModel();

            OceanExportMblDto oceanExportMbl = TempData["PrintData"] is not null ? JsonConvert.DeserializeObject<OceanExportMblDto>(TempData["PrintData"].ToString())
                                                                                 : await _oceanExportMblAppService.GetAsync(oceanExportMblId);

            if (oceanExportMbl.Mid != oceanExportMblId)
            {
                oceanExportMbl = await _oceanExportMblAppService.GetAsync(oceanExportMblId);
            }

            if (datasource == null || datasource == "shipment")
            {
                InfoViewModel.Office = oceanExportMbl.OfficeName;//待改
                InfoViewModel.To = "AER LINGUS\r\n6555 W. IMPERIAL HWY\r\nLOS ANGELES, CA 90045, UNITED STATES";//待改
                InfoViewModel.MblNo = oceanExportMbl.MblNo;
                InfoViewModel.CarrierBookingNo = oceanExportMbl.SoNo;
                InfoViewModel.Pieces = "1";//待改
                InfoViewModel.Destination = oceanExportMbl.PodName;//待改
                InfoViewModel.TotalPieces = "1";    //待改         
            }
            else if (datasource == "lastmodified")
            {
                ReportLog = new ReportLogDto();
                ReportLog.ReportId = oceanExportMblId;
                ReportLog.ReportName = "PackageLabel";
                var lastmodified = await _reportLogAppService.QueryReportLog(ReportLog);

                if (lastmodified == null)
                {
                    InfoViewModel.Office = oceanExportMbl.OfficeName;//待改
                    InfoViewModel.To = "AER LINGUS\r\n6555 W. IMPERIAL HWY\r\nLOS ANGELES, CA 90045, UNITED STATES";//待改
                    InfoViewModel.MblNo = oceanExportMbl.MblNo;
                    InfoViewModel.CarrierBookingNo = oceanExportMbl.SoNo;
                    InfoViewModel.Pieces = "1";//待改
                    InfoViewModel.Destination = oceanExportMbl.PodName;//待改
                    InfoViewModel.TotalPieces = "1";//待改
                }
                else
                {
                    var last = JsonConvert.DeserializeObject<PackageLabelIndexViewModel>(lastmodified.ReportData.ToString());

                    InfoViewModel.Office = last.Office;
                    InfoViewModel.To = last.To;
                    InfoViewModel.MblNo = last.MblNo;
                    InfoViewModel.CarrierBookingNo = last.CarrierBookingNo;
                    InfoViewModel.Pieces = last.Pieces;
                    InfoViewModel.Destination = last.Destination;
                    InfoViewModel.TotalPieces = last.TotalPieces;
                }
            }
            else if (datasource == "loademptyfieldsfromlastmodified")
            {
                ReportLog = new ReportLogDto();
                ReportLog.ReportId = oceanExportMblId;
                ReportLog.ReportName = "PackageLabel";
                var lastmodified = await _reportLogAppService.QueryReportLog(ReportLog);

                if (lastmodified == null)
                {
                    InfoViewModel.Office = oceanExportMbl.OfficeName;//待改
                    InfoViewModel.To = "AER LINGUS\r\n6555 W. IMPERIAL HWY\r\nLOS ANGELES, CA 90045, UNITED STATES";//待改
                    InfoViewModel.MblNo = oceanExportMbl.MblNo;
                    InfoViewModel.CarrierBookingNo = oceanExportMbl.SoNo;
                    InfoViewModel.Pieces = "1";//待改
                    InfoViewModel.Destination = oceanExportMbl.PodName;//待改
                    InfoViewModel.TotalPieces = "1";//待改
                }
                else
                {
                    var last = JsonConvert.DeserializeObject<PackageLabelIndexViewModel>(lastmodified.ReportData.ToString());

                    InfoViewModel.Office = last.Office;//待改
                    InfoViewModel.To = last.To;//待改
                    InfoViewModel.MblNo = oceanExportMbl.MblNo == null ? last.MblNo : oceanExportMbl.MblNo;
                    InfoViewModel.CarrierBookingNo = oceanExportMbl.SoNo == null ? last.CarrierBookingNo : oceanExportMbl.SoNo;
                    InfoViewModel.Pieces = last.Pieces;//待改
                    InfoViewModel.Destination = oceanExportMbl.PodName == null ? last.Destination : oceanExportMbl.PodName;//待改
                    InfoViewModel.TotalPieces = last.TotalPieces;//待改
                }
            }

            InfoViewModel.ReportId = oceanExportMbl.Id;
            InfoViewModel.DataSource = datasource;

            TempData["PrintData"] = JsonConvert.SerializeObject(oceanExportMbl);

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PackageLabel(PackageLabelIndexViewModel InfoModel)
        {
            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "PackageLabel";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/PackageLabel/PackageLabel.cshtml", InfoModel);
        }

        //[HttpGet]
        //public async Task<IActionResult> Package(string target, string oldhblno, string oldto, string oldpieces, string olddestination)
        //{
        //    var HblList = new List<PackageIndexViewModel>();

        //    if (TempData["PrintDataHBL"] != null && target != null)
        //    {
        //        HblList = JsonConvert.DeserializeObject<List<PackageIndexViewModel>>(TempData["PrintDataHBL"].ToString());
        //    }
        //    else
        //    {
        //        var OceanExportMbl = new CreateUpdateOceanExportMblDto();
        //        OceanExportMbl = JsonConvert.DeserializeObject<CreateUpdateOceanExportMblDto>(TempData["PrintData"].ToString());
        //        QueryHblDto query = new QueryHblDto() { MblId = OceanExportMbl.Id };
        //        IList<OceanExportHblDto> OceanExportHbls = await _oceanExportHblAppService.QueryListByMidAsync(query);

        //        if (OceanExportHbls != null && OceanExportHbls.Count > 1)
        //        {
        //            for (int i = 0; i < OceanExportHbls.Count; i++)
        //            {
        //                HblList.Add(new PackageIndexViewModel()
        //                {
        //                    Office = OceanExportHbls[i].OfficeName,
        //                    To = OceanExportHbls[i].AgentName,
        //                    MblNo = OceanExportMbl.MblNo,
        //                    HblNo = OceanExportHbls[i].HblNo,
        //                    Pieces = "1",
        //                    Destination = OceanExportHbls[i].PodName,
        //                    TotalPieces = "1",
        //                    ReportId = OceanExportHbls[i].Id
        //                });
        //            }
        //        }
        //        else
        //        {
        //            HblList.Add(new PackageIndexViewModel()
        //            {
        //                Office = OceanExportHbls[0].OfficeName,
        //                To = OceanExportHbls[0].AgentName,
        //                MblNo = OceanExportMbl.MblNo,
        //                HblNo = OceanExportHbls[0].HblNo,
        //                Pieces = "1",
        //                Destination = OceanExportHbls[0].PodName,
        //                TotalPieces = "1",
        //                ReportId = OceanExportHbls[0].Id
        //            });
        //        }
        //        TempData["PrintData"] = JsonConvert.SerializeObject(OceanExportMbl);
        //    }

        //    if (oldto != null)
        //    {
        //        oldto = oldto.Replace("%0D%0A", "\r\n");
        //    }

        //    HblList.Where(x => x.HblNo == oldhblno && (x.To != oldto || x.Pieces != oldpieces || x.Destination != olddestination)).ToList()
        //    .ForEach(x =>
        //    {
        //        x.To = oldto;
        //        x.Pieces = oldpieces;
        //        x.Destination = olddestination;
        //        x.TotalPieces = oldpieces;
        //    });

        //    ViewBag.HblList = HblList.Select(x => x.HblNo).Select(x => new SelectListItem() { Text = x, Value = x, Selected = x == target }).ToList();

        //    var hbl = string.IsNullOrEmpty(target) ? HblList[0] : (HblList.Any(x => x.HblNo == target) ? HblList.First(x => x.HblNo == target) : HblList[0]);
        //    hbl.HblList = JsonConvert.SerializeObject(HblList);
        //    TempData["PrintDataHBL"] = JsonConvert.SerializeObject(HblList);
        //    return View(hbl);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Package(string hbllist, string hblno, string to, string pieces, string destination, string totalpieces)
        //{

        //    var HblList = new List<PackageIndexViewModel>();
        //    HblList = JsonConvert.DeserializeObject<List<PackageIndexViewModel>>(hbllist);

        //    HblList.Where(x => x.HblNo == hblno && (x.To != to || x.Pieces != pieces || x.Destination != destination)).ToList()
        //    .ForEach(x =>
        //    {
        //        x.To = to;
        //        x.Pieces = pieces;
        //        x.Destination = destination;
        //        x.TotalPieces = totalpieces;
        //    });

        //    string Input = JsonConvert.SerializeObject(HblList);
        //    TempData["PrintDataHBL"] = JsonConvert.SerializeObject(HblList);

        //    ReportLog.ReportId = HblList[0].ReportId;
        //    ReportLog.ReportName = "Package";
        //    ReportLog.ReportData = Input;
        //    ReportLog.LastUpdateTime = DateTime.Now;
        //    await _reportLogAppService.UpdateReportLog(ReportLog);

        //    PackageIndexViewModel InfoModel = new PackageIndexViewModel();

        //    InfoModel.HblPDFList = new List<HblPDFListModel>();

        //    for (int i = 0; i < HblList.Count; i++)
        //    {
        //        InfoModel.HblPDFList.Add(new HblPDFListModel()
        //        {
        //            Office = HblList[i].Office == null ? "" : HblList[i].Office,
        //            To = HblList[i].To == null ? "" : HblList[i].To,
        //            MblNo = HblList[i].MblNo == null ? "" : HblList[i].MblNo,
        //            HblNo = HblList[i].HblNo == null ? "" : HblList[i].HblNo,
        //            Pieces = HblList[i].Pieces == null ? "0" : HblList[i].Pieces,
        //            Destination = HblList[i].Destination == null ? "" : HblList[i].Destination,
        //            TotalPieces = HblList[i].TotalPieces == null ? "0" : HblList[i].TotalPieces
        //        });
        //    }

        //    return await _generatePdf.GetPdf("Views/Docs/Pdf/Package/Package.cshtml", InfoModel);
        //}

        [HttpGet]
        public async Task<IActionResult> HblPackageLabel(string id)
        {
            HblPackageLabelIndexViewModel InfoViewModel = new HblPackageLabelIndexViewModel();

            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

            #region
            InfoViewModel.Office = "分站名稱";
            InfoViewModel.To = "AER LINGUS\r\n6555 W. IMPERIAL HWY\r\nLOS ANGELES, CA 90045, UNITED STATES";
            InfoViewModel.MblNo = "123AAABBBA";
            InfoViewModel.HblNo = "OH-23010006";
            InfoViewModel.Pieces = "1";
            InfoViewModel.Destination = "SOUTH OGDEN, UT (UNITED STATES)";
            InfoViewModel.TotalPieces = InfoViewModel.Pieces;

            InfoViewModel.ReportId = OceanExportHbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> HblPackageLabel(HblPackageLabelIndexViewModel InfoModel)
        {
            //InfoModel.TotalPieces = InfoModel.Pieces;
            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "HblPackageLabel";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/HblPackageLabel/HblPackageLabel.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> BookingConfirmation(string id)
        {
            BookingConfirmationIndexViewModel InfoViewModel = new BookingConfirmationIndexViewModel();

            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetOceanExportDetailsById(Guid.Parse(id));
            var OceanExportMbl = await _oceanExportMblAppService.GetOceanExportDetailsById(OceanExportHbl.MblId);
            var container = await _containerAppService.GetContainerByHblId(Guid.Parse(id));
            #region
            InfoViewModel.Office = "Dolphin Logistics Taipei Office";
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = OceanExportHbl.HblOperatorName;
            InfoViewModel.LastName = "";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");
            //InfoViewModel.ReportTitleChoice = "BC";
            //InfoViewModel.TradePartnerLayoutChoice = "default";
            InfoViewModel.CarrierBookingNo = OceanExportMbl.SoNo;
            InfoViewModel.ActualShipper = OceanExportHbl.ShippingAgentName;
            InfoViewModel.Consignee = OceanExportHbl.HblConsigneeName;
            InfoViewModel.Shipping = OceanExportHbl.ShippingAgentName;
            InfoViewModel.OverseaAgent = OceanExportHbl.MblOverseaAgentName;
            InfoViewModel.HblNo = OceanExportHbl.HblNo;
            InfoViewModel.OutBookingNo = "";
            InfoViewModel.BookingDate = "";
            InfoViewModel.ExportRefNo = OceanExportHbl.CustomerRefNo;
            InfoViewModel.PoNo = "";
            InfoViewModel.ItnNo = "";
            InfoViewModel.Agent = "AER LINGUS";
            InfoViewModel.Notify = OceanExportHbl.HblNotifyName;
            InfoViewModel.Vessel_Voyage = OceanExportHbl.VesselName + "" + OceanExportHbl.Voyage;
            InfoViewModel.Carrier = OceanExportHbl.MblCarrierName;
            InfoViewModel.PlaceOfReceipt = OceanExportHbl.PorName;
            InfoViewModel.PortOfLoading = OceanExportHbl.PolName;
            InfoViewModel.ETD = OceanExportHbl.PolEtd?.ToString("dd-MM-yyyy");
            InfoViewModel.PortOfTransshipment = OceanExportHbl.TransPort1Name;
            InfoViewModel.TsETA = OceanExportHbl.Trans1Eta?.ToString("dd-MM-yyyy");
            InfoViewModel.PortOfDischarge = OceanExportHbl.PodName;
            InfoViewModel.PodETA = OceanExportHbl.PodEta?.ToString("dd-MM-yyyy");
            InfoViewModel.PlaceOfDelivery = OceanExportHbl.DelName; ;
            InfoViewModel.DelETA = OceanExportHbl.DelEta?.ToString("dd-MM-yyyy");
            InfoViewModel.FinalDestination = OceanExportHbl.FdestName;
            InfoViewModel.FinalETA = OceanExportHbl.FdestEta?.ToString("dd-MM-yyyy");
            InfoViewModel.MoveType = OceanExportMbl.SvcTermFromName + "-" + OceanExportMbl.SvcTermToName;
            InfoViewModel.EarlyReturn = OceanExportHbl.EarlyReturnDateTime?.ToString("dd-MM-yyyy");
            InfoViewModel.Commodity = OceanExportHbl.Commodity?[0].Description;
            InfoViewModel.Container = container?.ContainerNo;
            InfoViewModel.Weight = container?.PackageWeight + "" + container?.PackageWeightUnit;
            InfoViewModel.Dangerous = false;
            InfoViewModel.Measurement = container?.PackageMeasure + "" + container?.PackageMeasureUnit;
            InfoViewModel.LC = true;
            InfoViewModel.PKG = container?.PackageNum.ToString();
            InfoViewModel.Stackable = true;
            InfoViewModel.CargoDeliveryLocation_1 = OceanExportHbl.DeliveryToName;
            InfoViewModel.CargoDeliveryLocation_2 = "AERO TRANSCOLOMBIANA DE CARGA";
            InfoViewModel.Port_Cutoff_Date = OceanExportHbl.PortCutOffTime?.ToString("dd-MM-yyyy");
            InfoViewModel.Rail_Cutoff_Date = OceanExportHbl.RailCutOffTime?.ToString("dd-MM-yyyy");
            InfoViewModel.Warehouse_Cutoff_Date = OceanExportHbl.VgmCutOffTime?.ToString("dd-MM-yyyy");
            InfoViewModel.Doc_Cutoff_Date = OceanExportHbl.DocCutOffTime?.ToString("dd-MM-yyyy");
            InfoViewModel.EmptyPickUp = OceanExportHbl.EmptyPickupName;
            InfoViewModel.CargoPickUp = OceanExportHbl.CargoPickUp?.TPName;
            InfoViewModel.Trucker = OceanExportHbl.TruckerName;
            InfoViewModel.Remark = OceanExportHbl.ColorRemarkName;

            InfoViewModel.ReportId = Guid.Parse(id);

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BookingConfirmation(BookingConfirmationIndexViewModel InfoModel)
        {
            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "BookingConfirmation";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/BookingConfirmation/BookingConfirmation.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> BankDraft(string id)
        {
            BankDraftIndexViewModel InfoViewModel = new BankDraftIndexViewModel();

            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetOceanExportDetailsById(Guid.Parse(id));

            #region 取貿易夥伴
            List<SelectListItem> GentlemenList = new List<SelectListItem>();
            QueryDto queryDto = new QueryDto();
            queryDto.QueryType = "";
            var gentlemen = await _ajaxDropdownAppService.GetAllTradePartners(queryDto);
            if (gentlemen != null)
            {
                GentlemenList = new List<SelectListItem>();
                GentlemenList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
                foreach (var receivablessource in gentlemen)
                {
                    GentlemenList.Add(new SelectListItem() { Text = receivablessource.TPName + "\r\n" + (receivablessource.TPAliasName == null ? "null" : receivablessource.TPAliasName) + "\r\n" + receivablessource.TPCode, Value = receivablessource.Id.ToString() });
                }
            }
            ViewBag.GentlemenList = GentlemenList;
            #endregion

            #region
            InfoViewModel.Amount = ".00";
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");
            InfoViewModel.AtSight = "AT SIGHT";
            InfoViewModel.ShipperName = OceanExportHbl.ShippingAgentName;
            InfoViewModel.DrawnDocCredit = "\"DRAWN UNDER DOCUMENTARY CREDIT NO. : " + OceanExportHbl.LCNo;
            InfoViewModel.IssueDate = OceanExportHbl.LCIssueDate?.ToString("dd-MM-yyyy");
            InfoViewModel.LCIssueBank = OceanExportHbl.LCIssueBankName; /*"PPP";*/
            InfoViewModel.ToLCIssueBank = OceanExportHbl.LCIssueBankName;
            InfoViewModel.ToTradePartnerLoadFrom = "CONSIGNEE"; //NOTIFY
            InfoViewModel.Consignee = OceanExportHbl.HblConsigneeName;
            InfoViewModel.NotifyParty = OceanExportHbl.HblNotifyName;
            InfoViewModel.ToTradePartnerName = OceanExportHbl.HblConsigneeName; /*OceanExportHbl.mbl;*/
            InfoViewModel.DraftNo = "";
            InfoViewModel.BankPreference = "";
            InfoViewModel.ShipperName2 = OceanExportHbl.ShippingAgentName;
            InfoViewModel.Gentlemen = "";
            InfoViewModel.GentlemenNameAddress = "";
            InfoViewModel.EncloseDate = DateTime.Now.ToString("yyyy-MM-dd");
            InfoViewModel.EncloseDraftNo = "";
            InfoViewModel.EncloseForCollection = "";
            InfoViewModel.EncloseForOther = "";
            InfoViewModel.EncloseForPayment = "";
            InfoViewModel.ExtraBl = "";
            InfoViewModel.ExtraBlCopy = "";
            InfoViewModel.ExtraCommInv = "";
            InfoViewModel.ExtraInsCtf = "";
            InfoViewModel.ExtraCtfOrig = "";
            InfoViewModel.ExtraConsInv = "";
            InfoViewModel.ExtraPkngList = "";
            InfoViewModel.ExtraWgtCtf = "";
            InfoViewModel.DeliverInOneMailing = "";
            InfoViewModel.DeliverInTwoMailing = "";
            InfoViewModel.DeliverIfDraft = "";
            InfoViewModel.AllChargesForAccount = "";
            InfoViewModel.DoNotWaiveCharges = "";
            InfoViewModel.ProtestForNonPayment = "";
            InfoViewModel.DoNotProtest = "";
            InfoViewModel.PresentOnArrival = "";
            InfoViewModel.AdviseNonPaymentBy = "";
            InfoViewModel.AdvisePaymentBy = "";
            InfoViewModel.ReferToName = "HARD CORE TECHNOLOGY(GST/ VAT)";
            InfoViewModel.ReferToAddress = "";
            InfoViewModel.ToActFullyOnOurBehalf = "";
            InfoViewModel.ToAssistNotToAlter = "";
            InfoViewModel.OtherInstructions = "";
            InfoViewModel.ReferQuestionTo = "";
            InfoViewModel.ReferQuestionName = "HARD CORE TECHNOLOGY(GST/VAT)";
            InfoViewModel.ReferQuestionPhone = "TEL: " + "08417606080";
            InfoViewModel.ReferQuestionShipperName = OceanExportHbl.ShippingAgentName;
            InfoViewModel.ReferQuestionShipperPhone = "TEL: " + "(358)459-8430";
            InfoViewModel.ReferQuestionFreightForwarderName = "HARD CORE TECHNOLOGY(GST/VAT)";
            InfoViewModel.ReferQuestionFreightForwarderPhone = "TEL: " + "08417606080";
            InfoViewModel.ShipperName3 = OceanExportHbl.ShippingAgentName;
            InfoViewModel.AuthorizedSignatureInput = "";
            InfoViewModel.UserCompany = "HANJUN LIN" + " / " + "HARD CORE TECHNOLOGY(GST/VAT)";

            InfoViewModel.ReportId = Guid.Parse(id);

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View("Views/Docs/Pdf/BankDraft/BankDraft.cshtml", InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BankDraft(BankDraftIndexViewModel InfoModel)
        {
            InfoModel.AmountToString = ConvertAmount(Convert.ToDouble(InfoModel.Amount)).ToString() + " AND 00/100";

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "BankDraft";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/BankDraft/BankDraft.cshtml", InfoModel);
        }

        private static String[] units = { "Zero", "One", "Two", "Three",
            "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
            "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
            "Seventeen", "Eighteen", "Nineteen" };
        private static String[] tens = { "", "", "Twenty", "Thirty", "Forty",
            "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public static String ConvertAmount(double amount)
        {
            try
            {
                Int64 amount_int = (Int64)amount;
                Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    return ConvertInt(amount_int);
                }
                else
                {
                    return ConvertInt(amount_int) + " Point " + ConvertInt(amount_dec);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            return "";
        }

        public static String ConvertInt(Int64 i)
        {
            if (i < 20)
            {
                return units[i];
            }
            if (i < 100)
            {
                return tens[i / 10] + ((i % 10 > 0) ? " " + ConvertInt(i % 10) : "");
            }
            if (i < 1000)
            {
                return units[i / 100] + " Hundred"
                        + ((i % 100 > 0) ? " And " + ConvertInt(i % 100) : "");
            }
            if (i < 100000)
            {
                return ConvertInt(i / 1000) + " Thousand "
                        + ((i % 1000 > 0) ? " " + ConvertInt(i % 1000) : "");
            }
            if (i < 10000000)
            {
                return ConvertInt(i / 100000) + " Lakh "
                        + ((i % 100000 > 0) ? " " + ConvertInt(i % 100000) : "");
            }
            if (i < 1000000000)
            {
                return ConvertInt(i / 10000000) + " Crore "
                        + ((i % 10000000 > 0) ? " " + ConvertInt(i % 10000000) : "");
            }
            return ConvertInt(i / 1000000000) + " Arab "
                    + ((i % 1000000000 > 0) ? " " + ConvertInt(i % 1000000000) : "");
        }

        public async Task<IActionResult> HblClauses()
        {
            var OutModel = new HblClausesIndexViewModel();
            return await _generatePdf.GetPdf("Views/HblClauses/HblClauses.cshtml", OutModel);
        }

        [HttpGet]
        public async Task<IActionResult> CertificateOfOrigin(string id)
        {
            CertificateOfOriginIndexViewModel InfoViewModel = new CertificateOfOriginIndexViewModel();

            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetOceanExportDetailsById(Guid.Parse(id));
            var OceanExportMbl = await _oceanExportMblAppService.GetOceanExportDetailsById(OceanExportHbl.MblId);
            //var shipper=await 

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030002/?hbl=47120&hide_mbl=false
            InfoViewModel.SHIPPER_EXPORTER = OceanExportHbl.ShippingAgentName + Environment.NewLine + OceanExportHbl.ShippingAgentContent; /*"3 FL., NO. 215, SEC. 1, FU XING S. RD., TAIPEI, TAIWAN" + Environment.NewLine + OceanExportHbl. + Environment.NewLine + "FAX : 02-87732222" + Environment.NewLine + "TAIWAN";*/
            InfoViewModel.CONSIGNEE = OceanExportHbl.HblConsigneeName;
            InfoViewModel.NOTIFY = OceanExportHbl.HblNotifyName + Environment.NewLine;
            InfoViewModel.DOCUMENT_NO = OceanExportHbl.DocNo;
            InfoViewModel.BL_NO = OceanExportHbl.SubBlNo;
            InfoViewModel.EXPORT_FILE_NO = OceanExportHbl.FilingNo;
            InfoViewModel.FORWARDING_AGENT = OceanExportHbl.ForwardingAgentName + Environment.NewLine + OceanExportHbl.ForwardingAgentContent /*+ Environment.NewLine + "UNITED STATES"*/;
            InfoViewModel.POINT_AND_COUNTRY_OF_ORIGIN = "";
            InfoViewModel.EXPORT_CARRIER = OceanExportHbl.VesselName + OceanExportHbl.Voyage;
            InfoViewModel.PORT_OF_LOADING = OceanExportMbl.PolName;
            InfoViewModel.PORT_OF_DISCHARGE = OceanExportMbl.PodName;
            InfoViewModel.PLACE_OF_DELIVERY = OceanExportMbl.DelName;

            InfoViewModel.SHIPPING_MARKS = OceanExportHbl.Mark; /*"CONTAINER NO./SEAL NO./P.O. NO." + Environment.NewLine + " /  / " + Environment.NewLine + Environment.NewLine + Environment.NewLine + "SHIEHN HAIPHONG PTE";*/
            InfoViewModel.QTY = OceanExportHbl.TotalPackage.ToString();
            InfoViewModel.DESCRIPTION_OF_GOODS = OceanExportHbl.Description; /*"ELECTRONIC COMPONENT" + Environment.NewLine + "20 CARTONS";*/
            InfoViewModel.WEIGHT_G = OceanExportHbl.TotalWeight + "" + OceanExportHbl.PackageWeightName; /* "300.00 KGS" + Environment.NewLine + Environment.NewLine + "661.39 LBS";*/
            InfoViewModel.WEIGHT_C = "";
            InfoViewModel.MEASUREMENT = OceanExportHbl.TotalMeasure + "" + OceanExportHbl.PackageMeasure; /*"4.50 CBM" + Environment.NewLine + Environment.NewLine + "158.92 CFT";*/
            InfoViewModel.Show_Container_Information = "True";
            InfoViewModel.CONTAINER_NO = "";
            InfoViewModel.TYPE = "";
            InfoViewModel.SEAL_NO = "";
            InfoViewModel.PKG = OceanExportHbl.TotalPackage.ToString();
            InfoViewModel.KG_LB = OceanExportHbl.TotalWeight.ToString();
            InfoViewModel.CBM_CFT = OceanExportHbl.TotalMeasure.ToString();
            InfoViewModel.bl_date = OceanExportHbl.OnBoardDate?.ToString("dd/MM/yyyy");
            InfoViewModel.sworn_date = OceanExportHbl.OnBoardDate?.ToString("MMMM/dd/yyyy"); ;

            InfoViewModel.name_of_chamber = "REGIONAL";
            InfoViewModel.state_of_chamber = "CA";
            InfoViewModel.name_of_country = "UNITED STATES";

            InfoViewModel.ReportId = OceanExportHbl.MblId;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View("Views/Docs/Pdf/CertificateOfOrigin/Default.cshtml", InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CertificateOfOrigin(CertificateOfOriginIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "CertificateOfOrigin";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/CertificateOfOrigin/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> DockRecepit(string id)
        {
            DockRecepitIndexViewModel InfoViewModel = new DockRecepitIndexViewModel();

            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030003/?hbl=47125&hide_mbl=false
            InfoViewModel.Office = "Dolphin Logistics Taipei Office";
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = "萬泰";
            InfoViewModel.LastName = "資訊部";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.SHIPPER_EXPORTER = "CHIYODA\r\nPODIUM BUILDING\r\n159883, SINGAPORE\r\nTEL: 62786666";
            InfoViewModel.CONSIGNEE = "ASTI\r\nHAIPHONG\r\nVIETNAM\r\nATTN:";
            InfoViewModel.NOTIFY = "CHIYODA\r\nPODIUM BUILDING\r\n159883, SINGAPORE\r\nTEL: 62786666";
            InfoViewModel.DELIVERY_TO = "NAX\r\nTOH GUAN ROAD LEVEL 2\r\n123456, SINGAPORE\r\nATTN:";
            InfoViewModel.CARRIER_BOOKING_NO = "APS44431-23";
            InfoViewModel.EXPORT_FILE_NO = "OEX-23030003";
            InfoViewModel.INVOICE_NO = "";
            InfoViewModel.PO_NO = "";
            InfoViewModel.EXPORT_CARRIER = "ALPINE AVIATION INC.";
            InfoViewModel.VESSEL_VOYAGE_NO = "ASIATIC BAY N097";
            InfoViewModel.CUT_OFF_PORT = "";
            InfoViewModel.CUT_OFF_RAIL = "";
            InfoViewModel.CUT_OFF_WAREHOUSE = "03-29-2023 16:00";
            InfoViewModel.CUT_OFF_SED_DOC = "03-28-2023 00:00";

            InfoViewModel.MARK_NUMBER = "CONTAINER NO./SEAL NO./P.O. NO.";
            InfoViewModel.NO_OF_PKGS = "0 CARTON(S)";
            InfoViewModel.DESCRIPTION_OF_GOODS = "";
            InfoViewModel.WEIGHT = "";
            InfoViewModel.MEASUREMENT = "";

            InfoViewModel.DELIVERY_BY = "";
            InfoViewModel.TRUCKING_CO = "";
            InfoViewModel.ARRIVED_DATE = "";
            InfoViewModel.DELIVERY_BY = "";
            InfoViewModel.UNLOADED_DATE = "";
            InfoViewModel.UNLOADED_TIME = "";
            InfoViewModel.CHECK_BY = "";
            InfoViewModel.INSHIP = "";
            InfoViewModel.ON_DOCK = "";
            InfoViewModel.LOCATION = "";

            InfoViewModel.ReportId = OceanExportHbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DockRecepit(DockRecepitIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "DockRecepit";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/DockRecepit/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> UsdaHeatTreatment(string id)
        {
            UsdaHeatTreatmentIndexViewModel InfoViewModel = new UsdaHeatTreatmentIndexViewModel();

            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetOceanExportDetailsById(Guid.Parse(id));

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030003/?hbl=47125&hide_mbl=false
            InfoViewModel.NameAndAddressOfExporte = OceanExportHbl.ShippingAgentName + Environment.NewLine + OceanExportHbl.ShippingAgentContent; /*"CHIYODA" + Environment.NewLine + "PODIUM BUILDING" + Environment.NewLine + "159883, SINGAPORE" + Environment.NewLine + "TEL: 62786666"*/;
            InfoViewModel.NameAndAddressOfConsignee = OceanExportHbl.HblConsigneeName;/* + Environment.NewLine + "PODIUM BUILDING" + Environment.NewLine + "159883, SINGAPORE" + Environment.NewLine + "TEL: 62786666";*/
            InfoViewModel.SignatureOfExporter = "";
            InfoViewModel.DescriptionOfConsignment1 = "";
            InfoViewModel.DescriptionOfConsignment2 = "";

            InfoViewModel.ReportId = Guid.Parse(id);

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UsdaHeatTreatment(UsdaHeatTreatmentIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "UsdaHeatTreatment";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/UsdaHeatTreatment/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> ForwarderCargoReceipt(string id)
        {
            ForwarderCargoReceiptIndexViewModel InfoViewModel = new ForwarderCargoReceiptIndexViewModel();

            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030002/?hbl=47120&hide_mbl=false
            InfoViewModel.Show_Container_Rider = "true";

            InfoViewModel.SUPPLIER_VENDOR = "123" + Environment.NewLine + "3 FL., NO. 215, SEC. 1, FU XING S. RD., TAIPEI, TAIWAN" + Environment.NewLine + "TEL : 02-87721111" + Environment.NewLine + "FAX : 02-87732222" + Environment.NewLine + "TAIWAN";
            InfoViewModel.CONSIGNEE = "CHIYODA";
            InfoViewModel.NOTIFY_PARTY = "1231231" + Environment.NewLine + "ATTN: SDSDSD";
            InfoViewModel.FCR_No = "SINHPH23030002";
            InfoViewModel.PORT_AND_COUNTRY_ORIGIN = "";
            InfoViewModel.DATE_OF_RECEIPT_OF_GOODS = "";

            InfoViewModel.VESSEL_VOYAGE = "XIN WEN ZHOU / 149E";
            InfoViewModel.SAILING_DATE = "03-27-2023";
            InfoViewModel.NUMBER_OF_ORIGINAL_FCR = "";
            InfoViewModel.PLACE_OF_RECEIPT = "";
            InfoViewModel.PORT_OF_LOADING = "SINGAPORE (SINGAPORE)";
            InfoViewModel.PORT_OF_DISCHARGE = "HAIPHONG (VIETNAM)";
            InfoViewModel.PLACE_OF_DELIVERY = "";

            InfoViewModel.MARKS_AND_NUMBERS = " /  / " + Environment.NewLine + Environment.NewLine + Environment.NewLine + "SHIEHN HAIPHONG PTE";
            InfoViewModel.NUMBER_AND_KIND_OF_PACKAGES = "3 PALLET(S)";
            InfoViewModel.DESCRIPTION_AND_GOODS = "ELECTRONIC COMPONENT" + Environment.NewLine + "20 CARTONS";
            InfoViewModel.GROSS_WEIGHT = "300.00 KGS" + Environment.NewLine + Environment.NewLine + "661.39 LBS";
            InfoViewModel.MEASUREMENT = "4.50 CBM" + Environment.NewLine + Environment.NewLine + "158.92 CFT";

            InfoViewModel.addition_mark = "DRAFT";
            InfoViewModel.signature = "HANJUN LIN";
            InfoViewModel.current_date = "04-10-2023";

            InfoViewModel.ContainerList = new List<ForwarderCargoReceiptContainerList>()
            {
            new ForwarderCargoReceiptContainerList()
            {
                PKGS = "3",
                UNIT = "PALLET(S)",
                WEIGHT = "300.00 / 661.39",
                MEASUREMENT = "4.50 / 158.92"
            }
            };

            InfoViewModel.TOTAL_PKGS = "3";
            InfoViewModel.TOTAL_UNIT = "PALLET(S)";
            InfoViewModel.TOTAL_WEIGHT = "300.00 / 661.39";
            InfoViewModel.TOTAL_MEASUREMENT = "4.50 / 158.92";

            InfoViewModel.ReportId = OceanExportHbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ForwarderCargoReceipt(ForwarderCargoReceiptIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "ForwarderCargoReceipt";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/ForwarderCargoReceipt/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public IActionResult Car()
        {
            CarViewModel InfoViewModel = new CarViewModel();

            var OceanExportMbl = new CreateUpdateOceanExportMblDto();
            OceanExportMbl = JsonConvert.DeserializeObject<CreateUpdateOceanExportMblDto>(TempData["PrintData"].ToString());

            #region
            InfoViewModel.Office = "Dolphin Logistics Taipei Office";
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = "萬泰";
            InfoViewModel.LastName = "資訊部";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.TO = "AER LINGUS";
            InfoViewModel.JOBNO = "OE-23020004";
            InfoViewModel.ATTN = "";
            InfoViewModel.MBL = "TEST00000002";
            InfoViewModel.HBL = "OH-23020009";
            InfoViewModel.SHPR = "123552133441";
            InfoViewModel.CNEE = "3891 DELTA PORT 809";
            InfoViewModel.CARR = "A CUSTOMS BROKERAGE, INC.";
            InfoViewModel.VSLVOV = ". ABHIJEET / CCC";
            InfoViewModel.POR = "HERMITAGE, AR (UNITED STATES)";
            InfoViewModel.POL = "BURLINGTON, KY (UNITED STATES)";
            InfoViewModel.ETD = "02-16-2023";
            InfoViewModel._2NDVV = "";
            InfoViewModel.ETDKAO = "";
            InfoViewModel.POD = "SOUTH OGDEN, UT (UNITED STATES)";
            InfoViewModel.ETA = "03-02-2023";
            InfoViewModel.DEST = "AMBIA, IN (UNITED STATES)";
            InfoViewModel.DESTETA = "03-04-2023";
            InfoViewModel.VOL = "40 X 1, 40FR X 1, 20TK X 1, 48HC X 1, 20 X 1";
            InfoViewModel.CNTRNO = "2 /  / 48HC" + Environment.NewLine + "4 /  / 40FR" + Environment.NewLine + "5 /  / 40";
            InfoViewModel.COMM = "";
            InfoViewModel.PONBR = "";
            InfoViewModel.PKGS = "7 CARTON(S)";
            InfoViewModel.GWT = "7 KGS / 15.43 LBS";
            InfoViewModel.MSRMT = "7 CBM / 247.2 CFT";
            InfoViewModel.RMK = "";

            InfoViewModel.ReportId = OceanExportMbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion
            TempData["PrintData"] = JsonConvert.SerializeObject(OceanExportMbl);

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Car(CarViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "Car";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/Car/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public IActionResult Cargo()
        {
            CargoViewModel InfoViewModel = new CargoViewModel();

            var OceanExportMbl = new CreateUpdateOceanExportMblDto();
            OceanExportMbl = JsonConvert.DeserializeObject<CreateUpdateOceanExportMblDto>(TempData["PrintData"].ToString());

            #region
            InfoViewModel.Office = "Dolphin Logistics Taipei Office";
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = "萬泰";
            InfoViewModel.LastName = "資訊部";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.TO = "AER LINGUS";
            InfoViewModel.JOBNO = "OE-23020004";
            InfoViewModel.ATTN = "";
            InfoViewModel.MBL = "TEST00000002";
            InfoViewModel.HBL = "OH-23020009";
            InfoViewModel.SHPR = "123552133441";
            InfoViewModel.CNEE = "3891 DELTA PORT 809";
            InfoViewModel.CARR = "A CUSTOMS BROKERAGE, INC.";
            InfoViewModel.VSLVOV = ". ABHIJEET / CCC";
            InfoViewModel.POR = "HERMITAGE, AR (UNITED STATES)";
            InfoViewModel.POL = "BURLINGTON, KY (UNITED STATES)";
            InfoViewModel.ETD = "02-16-2023";
            InfoViewModel._2NDVV = "";
            InfoViewModel.ETDKAO = "";
            InfoViewModel.POD = "SOUTH OGDEN, UT (UNITED STATES)";
            InfoViewModel.ETA = "03-02-2023";
            InfoViewModel.DEST = "AMBIA, IN (UNITED STATES)";
            InfoViewModel.DESTETA = "03-04-2023";
            InfoViewModel.VOL = "40 X 1, 40FR X 1, 20TK X 1, 48HC X 1, 20 X 1";
            InfoViewModel.CNTRNO = "2 /  / 48HC" + Environment.NewLine + "4 /  / 40FR" + Environment.NewLine + "5 /  / 40";
            InfoViewModel.COMM = "";
            InfoViewModel.PONBR = "";
            InfoViewModel.PKGS = "7 CARTON(S)";
            InfoViewModel.GWT = "7 KGS / 15.43 LBS";
            InfoViewModel.MSRMT = "7 CBM / 247.2 CFT";
            InfoViewModel.RMK = "";

            InfoViewModel.ReportId = OceanExportMbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion
            TempData["PrintData"] = JsonConvert.SerializeObject(OceanExportMbl);

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Cargo(CargoViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "Cargo";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/Cargo/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public IActionResult Box()
        {
            BoxViewModel InfoViewModel = new BoxViewModel();

            var OceanExportMbl = new CreateUpdateOceanExportMblDto();
            OceanExportMbl = JsonConvert.DeserializeObject<CreateUpdateOceanExportMblDto>(TempData["PrintData"].ToString());

            #region
            InfoViewModel.Office = "Dolphin Logistics Taipei Office";
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = "萬泰";
            InfoViewModel.LastName = "資訊部";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.TO = "AER LINGUS";
            InfoViewModel.JOBNO = "OE-23020004";
            InfoViewModel.ATTN = "";
            InfoViewModel.MBL = "TEST00000002";
            InfoViewModel.HBL = "OH-23020009";
            InfoViewModel.SHPR = "123552133441";
            InfoViewModel.CNEE = "3891 DELTA PORT 809";
            InfoViewModel.CARR = "A CUSTOMS BROKERAGE, INC.";
            InfoViewModel.VSLVOV = ". ABHIJEET / CCC";
            InfoViewModel.POR = "HERMITAGE, AR (UNITED STATES)";
            InfoViewModel.POL = "BURLINGTON, KY (UNITED STATES)";
            InfoViewModel.ETD = "02-16-2023";
            InfoViewModel._2NDVV = "";
            InfoViewModel.ETDKAO = "";
            InfoViewModel.POD = "SOUTH OGDEN, UT (UNITED STATES)";
            InfoViewModel.ETA = "03-02-2023";
            InfoViewModel.DEST = "AMBIA, IN (UNITED STATES)";
            InfoViewModel.DESTETA = "03-04-2023";
            InfoViewModel.VOL = "40 X 1, 40FR X 1, 20TK X 1, 48HC X 1, 20 X 1";
            InfoViewModel.CNTRNO = "2 /  / 48HC" + Environment.NewLine + "4 /  / 40FR" + Environment.NewLine + "5 /  / 40";
            InfoViewModel.COMM = "";
            InfoViewModel.PONBR = "";
            InfoViewModel.PKGS = "7 CARTON(S)";
            InfoViewModel.GWT = "7 KGS / 15.43 LBS";
            InfoViewModel.MSRMT = "7 CBM / 247.2 CFT";
            InfoViewModel.RMK = "";

            InfoViewModel.ReportId = OceanExportMbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion
            TempData["PrintData"] = JsonConvert.SerializeObject(OceanExportMbl);

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Box(BoxViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "Box";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/Box/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> ProfitSummary(Guid oceanExportMblId)
        {
            ProfitSummaryViewModel InfoViewModel = new ProfitSummaryViewModel();

            OceanExportMblDto oceanExportMbl = TempData["PrintData"] is not null ? JsonConvert.DeserializeObject<OceanExportMblDto>(TempData["PrintData"].ToString())
                                                                                 : await _oceanExportMblAppService.GetAsync(oceanExportMblId);

            if (oceanExportMbl.Id != oceanExportMblId)
            {
                oceanExportMbl = await _oceanExportMblAppService.GetAsync(oceanExportMblId);
            }

            #region
            InfoViewModel.Office = oceanExportMbl.OfficeName;
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = _currentUser.Email;
            InfoViewModel.FirstName = _currentUser.Name;
            InfoViewModel.LastName = _currentUser.SurName;
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.TO = Convert.ToString(oceanExportMbl.OfficeName);
            InfoViewModel.JOBNO = "OE-23020004";
            InfoViewModel.ATTN = "";
            InfoViewModel.MBL = oceanExportMbl.MblNo;
            InfoViewModel.HBL = "OH-23020009";
            InfoViewModel.SHPR = oceanExportMbl.ShipmentNo;
            InfoViewModel.CNEE = "3891 DELTA PORT 809";
            InfoViewModel.CARR = oceanExportMbl.MblCarrierName;
            InfoViewModel.VSLVOV = ". ABHIJEET / CCC";
            InfoViewModel.POR = oceanExportMbl.PorName;
            InfoViewModel.POL = oceanExportMbl.PolName;
            InfoViewModel.ETD = Convert.ToString(oceanExportMbl.PolEtd);
            InfoViewModel._2NDVV = "";
            InfoViewModel.ETDKAO = "";
            InfoViewModel.POD = oceanExportMbl.PodName;
            InfoViewModel.ETA = Convert.ToString(oceanExportMbl.PodEta);
            InfoViewModel.DEST = "AMBIA, IN (UNITED STATES)";
            InfoViewModel.DESTETA = Convert.ToString(oceanExportMbl.FdestEta);
            InfoViewModel.VOL = "40 X 1, 40FR X 1, 20TK X 1, 48HC X 1, 20 X 1";
            InfoViewModel.CNTRNO = "2 /  / 48HC" + Environment.NewLine + "4 /  / 40FR" + Environment.NewLine + "5 /  / 40";
            InfoViewModel.COMM = "";
            InfoViewModel.PONBR = "";
            InfoViewModel.PKGS = "7 CARTON(S)";
            InfoViewModel.GWT = "7 KGS / 15.43 LBS";
            InfoViewModel.MSRMT = "7 CBM / 247.2 CFT";
            InfoViewModel.RMK = "";

            InfoViewModel.ReportId = oceanExportMbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion
            TempData["PrintData"] = JsonConvert.SerializeObject(oceanExportMbl);

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ProfitSummary(ProfitSummaryViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "ProfitSummary";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/ProfitSummary/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> ProfitDetailAsync(Guid oceanExportMblId)
        {
            ProfitDetailViewModel InfoViewModel = new ProfitDetailViewModel();

            OceanExportMblDto oceanExportMbl = TempData["PrintData"] is not null ? JsonConvert.DeserializeObject<OceanExportMblDto>(TempData["PrintData"].ToString())
                                                                                 : await _oceanExportMblAppService.GetAsync(oceanExportMblId);

            if (oceanExportMbl.Id != oceanExportMblId)
            {
                oceanExportMbl = await _oceanExportMblAppService.GetAsync(oceanExportMblId);
            }

            #region
            InfoViewModel.Office = "Dolphin Logistics Taipei Office";
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = _currentUser.Email;
            InfoViewModel.FirstName = _currentUser.Name;
            InfoViewModel.LastName = _currentUser.SurName;
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.TO = "AER LINGUS";
            InfoViewModel.JOBNO = "OE-23020004";
            InfoViewModel.ATTN = "";
            InfoViewModel.MBL = oceanExportMbl.MblNo;
            InfoViewModel.HBL = "OH-23020009";
            InfoViewModel.SHPR = oceanExportMbl.ShipmentNo;
            InfoViewModel.CNEE = "3891 DELTA PORT 809";
            InfoViewModel.CARR = oceanExportMbl.MblCarrierName;
            InfoViewModel.VSLVOV = ". ABHIJEET / CCC";
            InfoViewModel.POR = oceanExportMbl.PorName;
            InfoViewModel.POL = oceanExportMbl.PolName;
            InfoViewModel.ETD = Convert.ToString(oceanExportMbl.PolEtd);
            InfoViewModel._2NDVV = "";
            InfoViewModel.ETDKAO = "";
            InfoViewModel.POD = oceanExportMbl.PodName;
            InfoViewModel.ETA = Convert.ToString(oceanExportMbl.PodEta);
            InfoViewModel.DEST = "AMBIA, IN (UNITED STATES)";
            InfoViewModel.DESTETA = "03-04-2023";
            InfoViewModel.VOL = "40 X 1, 40FR X 1, 20TK X 1, 48HC X 1, 20 X 1";
            InfoViewModel.CNTRNO = "2 /  / 48HC" + Environment.NewLine + "4 /  / 40FR" + Environment.NewLine + "5 /  / 40";
            InfoViewModel.COMM = "";
            InfoViewModel.PONBR = "";
            InfoViewModel.PKGS = "7 CARTON(S)";
            InfoViewModel.GWT = "7 KGS / 15.43 LBS";
            InfoViewModel.MSRMT = "7 CBM / 247.2 CFT";
            InfoViewModel.RMK = "";

            InfoViewModel.ReportId = oceanExportMbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion
            TempData["PrintData"] = JsonConvert.SerializeObject(oceanExportMbl);

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ProfitDetail(ProfitDetailViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "ProfitDetail";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/ProfitDetail/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> Shipment(string id)
        {
            ShipmentViewModel InfoViewModel = new ShipmentViewModel();

            ImportExport.OceanExports.QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetOceanExportDetailsById(Guid.Parse(id));
            var OceanExportMbl = await _oceanExportMblAppService.GetOceanExportDetailsById(OceanExportHbl.MblId);
            #region
            InfoViewModel.Office = "Dolphin Logistics Taipei Office";
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = OceanExportHbl.HblOperatorName;
            InfoViewModel.LastName = "";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");
            InfoViewModel.SteamShip = OceanExportMbl.ShippingAgentName;
            InfoViewModel.TO = "";
            InfoViewModel.JOBNO = OceanExportHbl.DocNo;
            InfoViewModel.ATTN = "";
            InfoViewModel.MBL = OceanExportHbl.MblNo;
            InfoViewModel.HBL = OceanExportHbl.HblNo;
            InfoViewModel.SHPR = OceanExportHbl.ShippingAgentName;
            InfoViewModel.CNEE = OceanExportHbl.HblConsigneeName;
            InfoViewModel.CARR = OceanExportHbl.MblCarrierName;
            InfoViewModel.VSLVOV = OceanExportHbl.VesselName + "" + OceanExportHbl.Voyage;
            InfoViewModel.POR = OceanExportHbl.PorName;
            InfoViewModel.POL = OceanExportMbl.PolName;
            InfoViewModel.ETD = OceanExportMbl.PolEtd?.ToString("dd-MM-yyyy");
            InfoViewModel._2NDVV = "";
            InfoViewModel.ETDKAO = "";
            InfoViewModel.POD = OceanExportMbl.PodName;
            InfoViewModel.ETA = OceanExportMbl.PodEta?.ToString("dd-MM-yyyy");
            InfoViewModel.DEST = OceanExportMbl.FdestName;
            InfoViewModel.DESTETA = OceanExportMbl.FdestEta?.ToString("dd-MM-yyyy");
            InfoViewModel.VOL = "";
            InfoViewModel.CNTRNO = "";
            InfoViewModel.COMM = "";
            InfoViewModel.PONBR = "";
            InfoViewModel.PKGS = OceanExportHbl.TotalPackage.ToString();
            InfoViewModel.GWT = OceanExportHbl.TotalWeight.ToString();
            InfoViewModel.MSRMT = OceanExportHbl.TotalMeasure.ToString();
            InfoViewModel.RMK = "";
            InfoViewModel.BookingNo = OceanExportMbl.SoNo;
            InfoViewModel.Xtn = OceanExportHbl.SoNo;
            InfoViewModel.SteamShip = OceanExportMbl.MblCarrierName;
            InfoViewModel.PlaceOfLoading = OceanExportHbl.CargoPickUp?.TPName;
            InfoViewModel.PolAddress = OceanExportHbl.CargoPickUp?.TPLocalAddress + "," + OceanExportHbl.CargoPickUp?.CityCode + "," + OceanExportHbl.CargoPickUp?.CountryName;
            InfoViewModel.ReportId = Guid.Parse(id);

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Shipment(ShipmentViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "Shipment";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/Shipment/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> OBL(string id)
        {
            OBLViewModel InfoViewModel = new OBLViewModel();

            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

            #region
            InfoViewModel.Office = "Dolphin Logistics Taipei Office";
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = "萬泰";
            InfoViewModel.LastName = "資訊部";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.TO = "AER LINGUS";
            InfoViewModel.JOBNO = "OE-23020004";
            InfoViewModel.ATTN = "";
            InfoViewModel.MBL = "TEST00000002";
            InfoViewModel.HBL = "OH-23020009";
            InfoViewModel.SHPR = "123552133441";
            InfoViewModel.CNEE = "3891 DELTA PORT 809";
            InfoViewModel.CARR = "A CUSTOMS BROKERAGE, INC.";
            InfoViewModel.VSLVOV = ". ABHIJEET / CCC";
            InfoViewModel.POR = "HERMITAGE, AR (UNITED STATES)";
            InfoViewModel.POL = "BURLINGTON, KY (UNITED STATES)";
            InfoViewModel.ETD = "02-16-2023";
            InfoViewModel._2NDVV = "";
            InfoViewModel.ETDKAO = "";
            InfoViewModel.POD = "SOUTH OGDEN, UT (UNITED STATES)";
            InfoViewModel.ETA = "03-02-2023";
            InfoViewModel.DEST = "AMBIA, IN (UNITED STATES)";
            InfoViewModel.DESTETA = "03-04-2023";
            InfoViewModel.VOL = "40 X 1, 40FR X 1, 20TK X 1, 48HC X 1, 20 X 1";
            InfoViewModel.CNTRNO = "2 /  / 48HC" + Environment.NewLine + "4 /  / 40FR" + Environment.NewLine + "5 /  / 40";
            InfoViewModel.COMM = "";
            InfoViewModel.PONBR = "";
            InfoViewModel.PKGS = "7 CARTON(S)";
            InfoViewModel.GWT = "7 KGS / 15.43 LBS";
            InfoViewModel.MSRMT = "7 CBM / 247.2 CFT";
            InfoViewModel.RMK = "";

            InfoViewModel.ReportId = OceanExportHbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> OBL(OBLViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "OBL";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/OBL/Default.cshtml", InfoModel);
        }

        public async Task<IActionResult> Hbl(Guid id)
        {
            HblIndexViewModel InfoViewModel = new HblIndexViewModel();

            var Ocean = await _oceanExportHblAppService.GetHblCardById(id);

            //var OceanExportMbl = new CreateUpdateOceanExportMblDto();

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030002/?hbl=47120&hide_mbl=false

            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;
            var exportBooking = await _exportBookingAppService.GetSONo();

            InfoViewModel.Header_Consignee = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(Ocean.HblConsigneeId)).Select(s => s.Text));
            InfoViewModel.Header_Notify = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(Ocean.HblNotifyId)).Select(s => s.Text));
            InfoViewModel.Header_Shipper = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(Ocean.HblShipperId)).Select(s => s.Text));
            InfoViewModel.Office = Ocean.OfficeName;
            InfoViewModel.Address = "77792 COBB CAPE" + Environment.NewLine + "TRISTANCHESTER, NE 47478";
            InfoViewModel.Tel = "08417606080";
            InfoViewModel.Fax = "08417606080";
            InfoViewModel.OTI_No = "123456N";
            InfoViewModel.SHIPPER_EXPORTER = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(Ocean.HblShipperId)).Select(s => s.Text));
            InfoViewModel.CONSIGNEE = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(Ocean.HblConsigneeId)).Select(s => s.Text));
            InfoViewModel.NOTIFY_PARTY = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(Ocean.HblNotifyId)).Select(s => s.Text));
            InfoViewModel.DOCUMENT_NO = Ocean.DocNo;
            InfoViewModel.BL_NO = Ocean.HblNo;
            InfoViewModel.EXPORT_REFERENCES = "S/O # " + Ocean.SoNo + Environment.NewLine + Ocean.CustomerRefNo;
            InfoViewModel.FORWARDING_AGENT_REFERENCES = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(Ocean.ForwardingAgentId)).Select(s => s.Text));
            InfoViewModel.domestic_instructions = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(Ocean.AgentId)).Select(s => s.Text));
            InfoViewModel.EXPORTING_CARRIER = string.Concat(exportBooking.Where(w => w.SoNo == Ocean.SoNo).Select(s => s.VesselName)) + " " + string.Concat(exportBooking.Where(w => w.SoNo == Ocean.SoNo).Select(s => s.Voyage));
            InfoViewModel.PLACE_OF_RECEIPT = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(Ocean.PorId)).Select(s => s.Text));
            InfoViewModel.PORT_OF_LOADING = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(Ocean.PolId)).Select(s => s.Text));
            InfoViewModel.PORT_OF_DISCHARGE = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(Ocean.PodId)).Select(s => s.Text));
            InfoViewModel.PLACE_OF_DELIVERY = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(Ocean.DelId)).Select(s => s.Text));
            InfoViewModel.CARGO_INSURANCE_THRU_CARRIER = "True";

            InfoViewModel.MARKS_AND_NUMBERS = " /  / " + Environment.NewLine + Environment.NewLine + Environment.NewLine + "SHIEHN HAIPHONG PTE";
            InfoViewModel.NO_OF_PKGS = "3 PALLET(S)";
            InfoViewModel.DESCRIPTION_OF_PACKAGES_AND_GOODS = "ELECTRONIC COMPONENT" + Environment.NewLine + "20 CARTONS";

            InfoViewModel.show_on_board_watermark = "True";
            InfoViewModel.on_board_watermark = "03-27-2023" + Environment.NewLine + "--------------------------" + Environment.NewLine + "XIN WEN ZHOU 149E" + Environment.NewLine + "SINGAPORE";

            InfoViewModel.Show_Container_Information = "True";
            InfoViewModel.ContainerList = new List<HblContainerList>()
            {
            new HblContainerList()
            {
                PKGS = "3",
                UNIT = "PALLET(S)",
                WEIGHT_KGS = "300.00",
                WEIGHT_LBS = "661.39",
                MEASUREMENT_CBM ="4.50",
                MEASUREMENT_CFT = "158.92"
            }
            };

            InfoViewModel.no_of_original_bill = "NO. OF ORIGINAL BILL(S): 0";

            InfoViewModel.FreightList = new List<HblFreightList>()
            {
            new HblFreightList()
            {
            },
            new HblFreightList()
            {
            },
            new HblFreightList()
            {
            },
            new HblFreightList()
            {
            },
            new HblFreightList()
            {
            }
            };

            InfoViewModel.show_hbl_export_remark = "True";
            InfoViewModel.hbl_export_remark = "THESE COMMODITIES, TECHNOLOGY OR SOFTWARE WERE EXPORTED FROM THE UNITED STATES IN ACCORDANCE WITH THE EXPORT ADMINISTRATION REGULATIONS, DIVERSION CONTRARY TO U.S LAW PROHIBITED";
            InfoViewModel.by_who = "AS AGENT FOR, THE CARRIER, OCEAN NETWORK EXPRESS (NORTH AMERICA) INC.";
            InfoViewModel.issue_date = "03-27-2023";
            InfoViewModel.declared_value = "8";


            InfoViewModel.TOTAL_PKGS = "3";
            InfoViewModel.TOTAL_UNIT = "PALLET(S)";
            InfoViewModel.TOTAL_WEIGHT_KGS = "300.00";
            InfoViewModel.TOTAL_WEIGHT_LBS = "661.39";
            InfoViewModel.TOTAL_MEASUREMENT_CBM = "4.50";
            InfoViewModel.TOTAL_MEASUREMENT_CFT = "158.92";

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion
            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> HBL(HblIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/Hbl/Default.cshtml", InfoModel);
        }


        [HttpGet]
        public async Task<IActionResult> CommercialInvoice(string id)
        {
            CommercialInvoiceIndexViewModel InfoViewModel = new CommercialInvoiceIndexViewModel();
            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetOceanExportDetailsById(Guid.Parse(id));
            #region
            //https://dlihq.gofreight.co/ocean/export/shipment/container/OE-23020004/?hbl=56
            InfoViewModel.shipper_area = OceanExportHbl.ShippingAgentName + Environment.NewLine + OceanExportHbl.ShippingAgentContent /*+ Environment.NewLine + "TEL : 02-87721111" + Environment.NewLine + "FAX : 02-87732222" + Environment.NewLine + "TAIWAN"*/;
            InfoViewModel.consignee_area = OceanExportHbl.HblConsigneeName;
            InfoViewModel.notify_area = OceanExportHbl.HblNotifyName;
            InfoViewModel.invoice_no_and_date = "";
            InfoViewModel.LC_NO = OceanExportHbl.LCNo;
            InfoViewModel.LC_issue_bank = OceanExportHbl.LCIssueBankName;
            InfoViewModel.ETD_date = OceanExportHbl.DelEta?.ToString("dd/mm/yyyy");
            InfoViewModel.POR_location = OceanExportHbl.PorName;
            InfoViewModel.POL_location = OceanExportHbl.PolName;
            InfoViewModel.FDEST_location = OceanExportHbl.FdestName;
            InfoViewModel.vessel_voyage = OceanExportHbl.VesselName;
            InfoViewModel.freight_term = OceanExportHbl.FreightTermName;

            InfoViewModel.shipping_marks = OceanExportHbl.Mark /*+ Environment.NewLine + " /  / " + Environment.NewLine + Environment.NewLine + Environment.NewLine + "SHIEHN HAIPHONG PTE"*/;
            InfoViewModel.packages = OceanExportHbl.TotalPackage.ToString();
            InfoViewModel.good_items = OceanExportHbl.Description;
            InfoViewModel.unit_price = "";
            InfoViewModel.total_amount = "";
            InfoViewModel.shipper_name = OceanExportHbl.ShippingAgentName;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View("Views/Docs/CommercialInvoice.cshtml", InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CommercialInvoice(CommercialInvoiceIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/CommercialInvoice/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> PickupDeliveryOrder(string id)
        {
            PickupDeliveryOrderIndexViewModel InfoViewModel = new PickupDeliveryOrderIndexViewModel();

            //QueryHblDto queryHbl = new QueryHblDto();
            //queryHbl.Id = Guid.Parse(id);
            QueryContainerDto query = new QueryContainerDto() { QueryId = Guid.Parse(id), MaxResultCount = 1000 };
            var OceanExportMbl = await _oceanExportMblAppService.GetOceanExportDetailsById(Guid.Parse(id));
            var containers = await _containerAppService.QueryListAsync(query);

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030003/?hbl=47125&hide_mbl=false
            InfoViewModel.Office = OceanExportMbl.Office.AbbreviationName;
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = OceanExportMbl.MblCustomerName; /*"萬泰"*/;
            InfoViewModel.LastName = "" /*"資訊部"*/;
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.trucker_area = "";
            InfoViewModel.empty_pickup_area = OceanExportMbl.EmptyPickupName;
            InfoViewModel.issue_at = OceanExportMbl.PostDate.ToString("dd-MM-yyyy");/* "05-09-2023";*/
            InfoViewModel.issue_by = OceanExportMbl.MblOperatorName;
            InfoViewModel.MBL_NO = OceanExportMbl.MblNo;
            InfoViewModel.carrier = OceanExportMbl.MblCarrierName;
            InfoViewModel.VESSEL_INFO = OceanExportMbl.VesselName + OceanExportMbl.Voyage;
            InfoViewModel.POR_location = OceanExportMbl.PorName;
            InfoViewModel.POR_location_ETD = OceanExportMbl.PorEtd?.ToString("dd-MM-yyyy");
            InfoViewModel.POL_location = OceanExportMbl.PolName;
            InfoViewModel.POL_location_ETD = OceanExportMbl.PolEtd?.ToString("dd-MM-yyyy");
            InfoViewModel.POD_location = OceanExportMbl.PodName;
            InfoViewModel.POD_location_ETD = OceanExportMbl.PodEta?.ToString("dd-MM-yyyy") /*"04-03-2023"*/;
            InfoViewModel.total_packages_count = OceanExportMbl.TotalPackage.ToString();
            InfoViewModel.gross_weight_kgs = OceanExportMbl.TotalWeight.ToString();
            InfoViewModel.gross_weight_lbs = "0.00";
            InfoViewModel.measurement_cbm = OceanExportMbl.TotalMeasure.ToString();
            InfoViewModel.measurement_cft = "0.00";
            InfoViewModel.COMMODITY = OceanExportMbl.Commodity?.ToString();
            InfoViewModel.carrier_bkg_no = OceanExportMbl.SoNo;
            InfoViewModel.delivery_to_area = OceanExportMbl.DeliveryToName;
            InfoViewModel.billing_to_area = OceanExportMbl.MblBillToName + "\r\n" + OceanExportMbl.MblBillToContent; /*"HARD CORE TECHNOLOGY\r\n198 PEARSON GATEWAY APT. 555\r\nNORTH JAMES, KY 98809-9933\r\nWALNUT, CA 91789, UNITED STATES\r\nATTN: JENNIFER JIMENEZ TEL: 585.592.4848 FAX: 649-277-5122"*/;
            InfoViewModel.ContainerList = new List<PickupDeliveryOrderContainerList>();
            foreach (var container in containers)
            {
                var con = new
                  PickupDeliveryOrderContainerList
                {
                    PACKAGE = container.PackageNum.ToString(),
                    WEIGHT = container.PackageWeight + " " + container.PackageWeightUnit,
                    CONTAINER_NO = container.ContainerNo,
                    PICKUP_NO = container.PicupNo,
                    SEAL_NO = container.SealNo,
                    LFD = container.LastFreeDate.ToString(),

                };
                InfoViewModel.ContainerList.Add(con);
            };

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PickupDeliveryOrder(PickupDeliveryOrderIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "PickupDeliveryOrder";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/PickupDeliveryOrder/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> BookingPackingList(/*string id*/)
        {
            BookingPackingListIndexViewModel InfoViewModel = new BookingPackingListIndexViewModel();

            //QueryHblDto queryHbl = new QueryHblDto();
            //queryHbl.Id = Guid.Parse(id);
            //var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

            #region
            //https://eval-asia.gofreight.co/reports/ocean/mbl/booking-packing-list/OEX-23030003/


            InfoViewModel.shipper_name = "";
            InfoViewModel.shipper_address = "";
            InfoViewModel.consignee_name = "";
            InfoViewModel.consignee_address = "";
            InfoViewModel.inv_no = "";
            InfoViewModel.issue_date = "05-15-2023";
            InfoViewModel.POL_location = "SINGAPORE (SINGAPORE)";
            InfoViewModel.POD_location = "BANGKOK (THAILAND)";

            InfoViewModel.ContainerList = new List<BookingPackingListContainerList>()
            {
                new
                BookingPackingListContainerList
                {
                    booking_no = "SINBKK23030003",
                    package = "6",
                    pkg = "PALLET(S)",
                    description = "ELECTRONIC COMPONENT",
                    hts = "",
                    pcs = "",
                    net_weight_kg = "950.00",
                    net_weight_lb = "2,094.39",
                    gross_weight_kg = "1,000.00",
                    gross_weight_lb = "2,204.62",
                    price = "",
                    amount = "0.00",
                    details = ""
                },

                new
                BookingPackingListContainerList
                {
                    booking_no = "SINBKK23030003",
                    package = "6",
                    pkg = "PALLET(S)",
                    description = "ELECTRONIC COMPONENT",
                    hts = "",
                    pcs = "",
                    net_weight_kg = "950.00",
                    net_weight_lb = "2,094.39",
                    gross_weight_kg = "1,000.00",
                    gross_weight_lb = "2,204.62",
                    price = "",
                    amount = "0.00",
                    details = ""
                }
            };
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public IActionResult BookingPackingList(BookingPackingListIndexViewModel InfoModel)
        {
            var reportTemplatePath = Path.Combine("Views/Docs/Booking_packing_list.xlsx");

            var file = new FileInfo(reportTemplatePath);

            using (var excel = new ExcelPackage(file))
            {
                var sheet = excel.Workbook.Worksheets.First();

                sheet.Cells[2, 1].Value = InfoModel.shipper_name ?? "";
                sheet.Cells[3, 1].Value = InfoModel.shipper_address ?? "";
                sheet.Cells[4, 2].Value = InfoModel.consignee_name ?? "";
                sheet.Cells[5, 2].Value = InfoModel.consignee_address ?? "";
                sheet.Cells[4, 8].Value = InfoModel.inv_no ?? "";
                sheet.Cells[5, 8].Value = InfoModel.issue_date ?? "";
                sheet.Cells[6, 2].Value = InfoModel.POL_location ?? "";
                sheet.Cells[6, 8].Value = InfoModel.POD_location ?? "";

                sheet.Cells[7, 7].Value = "Net Weight (" + (InfoModel.net_weight_unit ?? "") + ")";
                sheet.Cells[7, 8].Value = "Gross Weight (" + (InfoModel.gross_weight_unit ?? "") + ")";

                var rowIndex = 8;
                if (InfoModel.ContainerList != null)
                {
                    foreach (var item in InfoModel.ContainerList)
                    {
                        sheet.Cells[rowIndex, 1].Value = item.booking_no ?? "";
                        sheet.Cells[rowIndex, 2].Value = item.package ?? "";
                        sheet.Cells[rowIndex, 3].Value = item.pkg ?? "";
                        sheet.Cells[rowIndex, 4].Value = item.description ?? "";
                        sheet.Cells[rowIndex, 5].Value = item.hts ?? "";
                        sheet.Cells[rowIndex, 6].Value = item.pcs ?? "";
                        sheet.Cells[rowIndex, 7].Value = item.net_weight ?? "";
                        sheet.Cells[rowIndex, 8].Value = item.gross_weight ?? "";
                        sheet.Cells[rowIndex, 9].Value = item.price ?? "";
                        sheet.Cells[rowIndex, 10].Value = item.amount ?? "";
                        sheet.Cells[rowIndex, 11].Value = item.details ?? "";
                        rowIndex++;
                    }
                }

                sheet.Cells[rowIndex, 2].Value = InfoModel.total_package ?? "";
                sheet.Cells[rowIndex, 6].Value = InfoModel.total_pcs ?? "";
                sheet.Cells[rowIndex, 7].Value = InfoModel.total_net_weight ?? "";
                sheet.Cells[rowIndex, 8].Value = InfoModel.total_gross_weight ?? "";
                sheet.Cells[rowIndex, 10].Value = InfoModel.total_amount ?? "";

                sheet.Cells[8, 2, rowIndex, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[8, 5, rowIndex, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                sheet.Cells[8, 1, rowIndex, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                sheet.Cells[8, 1, rowIndex, 11].Style.Font.Name = "Helvetica";
                sheet.Cells[8, 1, rowIndex, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[8, 1, rowIndex, 11].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells[8, 1, rowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells[8, 1, rowIndex, 11].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells[rowIndex, 1, rowIndex, 11].Style.Font.Bold = true;

                var filecontent = excel.GetAsByteArray();

                return File(filecontent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "booking_packing_list_" + DateTime.Now.ToString("MM-dd-yyyy") + ".xlsx");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Manifest(string id, string report, string reportType)
        {
            //QueryHblDto queryHbl = new QueryHblDto();
            //queryHbl.Id = Guid.Parse(id);
            //var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

            ManifestIndexViewModel InfoViewModel = new ManifestIndexViewModel();

            #region
            //https://eval-asia.gofreight.co/reports/ocean/mbl/export-manifest-by-mbl/OEX-23030003/
            InfoViewModel.Office = "Dolphin Logistics Taipei Office";
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = "萬泰";
            InfoViewModel.LastName = "資訊部";
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");

            InfoViewModel.from_trade_partner = "Dolphin Logistics Taipei Office";
            InfoViewModel.to_trade_partner_area = "11";
            InfoViewModel.MblNo = "SEBKK23030141-06";
            InfoViewModel.filing_no = "OEX-23030003";
            InfoViewModel.BookingNo = "APS44431-23";
            InfoViewModel.Carrier = "ALPINE AVIATION INC.";
            InfoViewModel.vessel_name = "ASIATIC BAY";
            InfoViewModel.voyage = "N097";
            InfoViewModel.POL_location = "SINGAPORE (SINGAPORE)";
            InfoViewModel.ETD = "03-30-2023";
            InfoViewModel.POD_location = "BANGKOK (THAILAND)";
            InfoViewModel.ETA = "04-03-2023";
            InfoViewModel.po_nos_text = "";
            InfoViewModel.CargoReadyDate = "03-28-2023";
            InfoViewModel.remarks = "";
            InfoViewModel.total_count = "1";
            InfoViewModel.total_PACKAGE = "6";
            InfoViewModel.total_MEASUREMENT_CBM = "4.50";
            InfoViewModel.total_MEASUREMENT_CFT = "158.92";
            InfoViewModel.total_GROSS_WEIGHT_KGS = "1,000.00";
            InfoViewModel.total_GROSS_WEIGHT_LBS = "2,204.62";
            InfoViewModel.CNTR_NO_SEAL_SIZE = "None /";

            InfoViewModel.ContainerList = new List<ManifestContainerList>()
            {
                new
                ManifestContainerList
                {
                    CONTAINER_NO = "None",
                    SEAL_NO = "None",
                    CONTAINER_SIZE = "1*None",
                    SHIPPER = "CHIYODA",
                    CONSIGNEE = "ASTI",
                    BL_NO = "SINBKK23030003",
                    PACKAGE = "6",
                    GROSS_WEIGHT_KGS = "1,000.00",
                    GROSS_WEIGHT_LBS = "2,204.62",
                    MEASUREMENT_CBM = "4.50",
                    MEASUREMENT_CFT = "158.92",
                    shipper = "CHIYODA" + Environment.NewLine + "PODIUM BUILDING" + Environment.NewLine + "159883, SINGAPORE" + Environment.NewLine + "TEL: 62786666",
                    consignee = "ASTI" + Environment.NewLine + "HAIPHONG" + Environment.NewLine + "VIETNAM" + Environment.NewLine + "ATTN:",
                    good_items = "ELECTRONIC COMPONENT",
                    good_items_without_pcs_and_amount = "ELECTRONIC COMPONENT",
                    good_items_with_pcs_and_amount = "ELECTRONIC COMPONENT / PCS:  / AMOUNT: "
                }
            };
            #endregion

            if (report == "CARRIER")
            {
                return View("ManifestForSteamshipLine", InfoViewModel);
            }
            else
            {
                if (reportType == "BY_CONTAINER")
                    return View("ManifestByContainer", InfoViewModel);
                else
                    return View("ManifestByMbl", InfoViewModel);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Manifest(ManifestIndexViewModel InfoModel, string report, string reportType)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);


            if (report == "CARRIER")
            {
                return await _generatePdf.GetPdf("Views/Docs/Pdf/Manifest/ManifestForSteamshipLine.cshtml", InfoModel);
            }
            else
            {
                if (reportType == "BY_CONTAINER")
                    return await _generatePdf.GetPdf("Views/Docs/Pdf/Manifest/ManifestByContainer.cshtml", InfoModel);
                else
                    return await _generatePdf.GetPdf("Views/Docs/Pdf/Manifest/ManifestByMbl.cshtml", InfoModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> PackagingListAirExportHawb(Guid hawbId)
        {
            PackagingListAirExportHawb InfoViewModel = new PackagingListAirExportHawb();

            var data = await _airExportHawbAppService.GetAsync(hawbId);
            var mawb = await _airExportMawbAppService.GetAsync(data.MawbId.GetValueOrDefault());
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;
            var incoTerms = _dropdownService.IncotermsLookupList;

            InfoViewModel.Shipper_name = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.ActualShippedr)).Select(s => s.Text));
            InfoViewModel.Notify_party = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.Notify)).Select(s => s.Text));
            InfoViewModel.Consignee_address = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.Consignee)).Select(s => s.Text));
            InfoViewModel.Inv_no = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.BookingNo)).Select(s => s.Text));
            InfoViewModel.Inv_date = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.BookingDate)).Select(s => s.Text));
            InfoViewModel.POL_location = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(data.DepartureId)).Select(s => s.Text));
            InfoViewModel.POD_location = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(data.DestinationId)).Select(s => s.Text));
            InfoViewModel.Issue_date = "";
            InfoViewModel.Departure_date = string.Concat(mawb.DepatureDate);
            InfoViewModel.Issue_bank = "";
            InfoViewModel.LC_no = "";
            InfoViewModel.Term = string.Concat(incoTerms.Where(w => w.Value == Convert.ToString(data.Incoterms)).Select(s => s.Text));
            InfoViewModel.Flight_no = string.Concat(mawb.FlightNo);

            InfoViewModel.ContainerList = new List<PackagingListAirExportHawbList>()
            {
                new
                PackagingListAirExportHawbList
                {
                    Booking_no = "",
                    Package = "",
                    Pkg = "",
                    Description = "",
                    Hts = "",
                    Pcs = "",
                    Net_weight_kg = string.Concat(data.ChargeableWeightShprKG),
                    Net_weight_lb = string.Concat(data.ChargeableWeightShprLB),
                    Gross_weight_kg = string.Concat(data.GrossWeightShprKG),
                    Gross_weight_lb = string.Concat(data.GrossWeightShprLB),
                    Price = "",
                    Details = "",
                    Volumns = string.Concat(data.ChargeableWeightCneeLB),
                    Amount = data.ChargeableWeightCneeLB == null ? "" : (double.Parse(data.ChargeableWeightCneeLB) * 35.315).ToString("0.00"),
                    Quantity = string.Concat(data.Package + "/" + data.PackageUnit)
                }
            };

            return View(InfoViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> HawbProfitReport(Guid hawbId)
        {
            var hawb = await _airExportHawbAppService.GetHawbWithDetailsById(hawbId);

            var mawb = await _airExportMawbAppService.GetAsync(hawb.MawbId.GetValueOrDefault());

            var measurement = Convert.ToDouble(hawb.ChargeableWeightCneeLB) * 35.315;

            var hawbProfitReport = new ProfitReportViewModel()
            {
                AgentName = hawb.OverseaAgent,
                Consignee = hawb.Consignee,
                Currency = "USD",
                Customer = hawb.Customer,
                HawbNo = hawb.HawbNo,
                Operator = hawb.OP?.Name,
                Measurement = measurement.ToString("0.00"),
                PolEtd = string.Concat(hawb.DepartureName, "/", mawb.DepatureDate),
                PodEtd = string.Concat(hawb.DestinationName, "/", mawb.ArrivalDate),
                Sales = hawb.Sales?.Name,
                Shipper = hawb.Trucker,
                MawbNo = mawb.MawbNo,
                ChargableWeight = string.Concat(hawb.ChargeableWeightCneeKG, "/", hawb.ChargeableWeightCneeLB),
                PostDate = DateTime.Now
            };

            QueryInvoiceDto queryDto = new QueryInvoiceDto() { QueryType = 4, ParentId = hawbId };
            hawbProfitReport.Invoices = await _invoiceAppService.QueryInvoicesAsync(queryDto);
            hawbProfitReport.AR = new List<InvoiceDto>();
            hawbProfitReport.AP = new List<InvoiceDto>();
            hawbProfitReport.DC = new List<InvoiceDto>();

            if (hawbProfitReport.Invoices != null && hawbProfitReport.Invoices.Count > 0)
            {
                foreach (var dto in hawbProfitReport.Invoices)
                {
                    switch (dto.InvoiceType)
                    {
                        default:
                            hawbProfitReport.AR.Add(dto);
                            break;
                        case 1:
                            hawbProfitReport.AP.Add(dto);
                            break;
                        case 2:
                            hawbProfitReport.DC.Add(dto);
                            break;
                    }
                }
            }

            if (hawbProfitReport.AR.Any())
            {
                double arTotal = 0;
                foreach (var ar in hawbProfitReport.AR)
                {
                    arTotal += Convert.ToInt64(ar.InvoiceAmount);
                }
                hawbProfitReport.ARTotal = arTotal;
            }
            if (hawbProfitReport.AP.Any())
            {
                double apTotal = 0;
                foreach (var ap in hawbProfitReport.AP)
                {
                    apTotal += Convert.ToInt64(ap.InvoiceAmount);
                }

                hawbProfitReport.APTotal = apTotal;
            }
            if (hawbProfitReport.DC.Any())
            {
                double dcTotal = 0;
                foreach (var dc in hawbProfitReport.DC)
                {
                    dcTotal += Convert.ToInt64(dc.InvoiceAmount);
                }
                hawbProfitReport.DCTotal = dcTotal;
            }

            hawbProfitReport.Total = hawbProfitReport.ARTotal + hawbProfitReport.APTotal + hawbProfitReport.DCTotal;

            return View(hawbProfitReport);
        }

        [HttpPost]
        public async Task<IActionResult> HawbProfitReport(ProfitReportViewModel model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/HawbProfitReport.cshtml", model);
        }

        [HttpGet]
        public async Task<ActionResult> DangerousGoods(Guid hawbId)
        {
            var hawb = await _airExportHawbAppService.GetHawbWithDetailsById(hawbId);

            var mawb = await _airExportMawbAppService.GetAsync(hawb.MawbId.GetValueOrDefault());

            var viewModel = new DangerousGoodsViewModel()
            {
                HawbId = hawbId,
                MawbId = mawb.Id,
                AirWayBillNo = hawb.HawbNo,
                Consignee = hawb.Consignee,
                DepartureName = hawb.DepartureName,
                DestinationName = hawb.DestinationName,
                NameOfSignatory = string.Concat(hawb.OP?.Name, " ", hawb.OP?.Surname),
                PodEtd = string.Concat(hawb.DepartureName, ",", mawb.DepatureDate).TrimStart(','),
                Shipper = hawb.CargoPickupName
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> DangerousGoods(DangerousGoodsViewModel model)
        {
            model.IsPDF = true;
            return await _generatePdf.GetPdf("Views/Docs/DangerousGoods.cshtml", model);
        }

        public async Task<IActionResult> CertificateOfOriginAirExportHawb(Guid id)
        {
            CertificateOfOriginAirExportHawbIndexViewModel InfoViewModel = new CertificateOfOriginAirExportHawbIndexViewModel();

            var hawb = await _airExportHawbAppService.GetAsync(id);
            var mawb = await _airExportMawbAppService.GetAsync(hawb.MawbId.GetValueOrDefault());
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030002/?hbl=47120&hide_mbl=false
            InfoViewModel.SHIPPER_EXPORTER = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.ActualShippedr)).Select(s => s.Text));
            InfoViewModel.CONSIGNEE = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.ConsigneeId)).Select(s => s.Text));
            InfoViewModel.NOTIFY = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.Notify)).Select(s => s.Text));
            InfoViewModel.DOCUMENT_NO = mawb.FilingNo;
            InfoViewModel.BL_NO = hawb.BookingNo;
            InfoViewModel.EXPORT_FILE_NO = "BOOKING # " + hawb.BookingNo;
            InfoViewModel.FORWARDING_AGENT = "";
            InfoViewModel.POINT_AND_COUNTRY_OF_ORIGIN = "";
            InfoViewModel.EXPORT_CARRIER = mawb.FlightNo;
            InfoViewModel.PORT_OF_LOADING = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(hawb.DepartureId)).Select(s => s.Text));
            InfoViewModel.PORT_OF_DISCHARGE = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(hawb.DestinationId)).Select(s => s.Text));
            InfoViewModel.PLACE_OF_DELIVERY = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.DeliveryTo)).Select(s => s.Text));

            InfoViewModel.SHIPPING_MARKS = hawb.Mark;
            InfoViewModel.QTY = hawb.Package + "/" + hawb.PackageUnit;
            InfoViewModel.DESCRIPTION_OF_GOODS = hawb.NatureAndQuantityOfGoods;
            InfoViewModel.WEIGHT_G = hawb.GrossWeightShprKG + " KG" + Environment.NewLine + hawb.GrossWeightShprLB + " LBS";
            InfoViewModel.WEIGHT_C = hawb.ChargeableWeightShprKG + " KG" + Environment.NewLine + hawb.ChargeableWeightShprLB + " LBS";
            InfoViewModel.MEASUREMENT = hawb.ChargeableWeightCneeKG + " CBM" + Environment.NewLine + (double.Parse(hawb.ChargeableWeightCneeKG) * 35.315).ToString("0.00") + " CFT";
            InfoViewModel.Show_Container_Information = "true";
            InfoViewModel.CONTAINER_NO = "";
            InfoViewModel.TYPE = "";
            InfoViewModel.SEAL_NO = "";
            InfoViewModel.PKG = "";
            InfoViewModel.KG_LB = "";
            InfoViewModel.CBM_CFT = "";
            InfoViewModel.bl_date = "";
            InfoViewModel.sworn_date = "";

            InfoViewModel.name_of_chamber = "";
            InfoViewModel.state_of_chamber = "";
            InfoViewModel.name_of_country = "";

            InfoViewModel.ReportId = hawb.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CertificateOfOriginAirExportHawb(CertificateOfOriginAirExportHawbIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoModel);

            ReportLog.ReportId = InfoModel.ReportId;
            ReportLog.ReportName = "CertificateOfOriginAirExportHawb";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;

            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/CertificateOfOriginAirExportHawb/Default.cshtml", InfoModel);
        }

        public async Task<IActionResult> PackageLabelAirExportHawb(Guid hawbId)
        {
            PackageLabelAirExportHawbIndexViewModel InfoModel = new();

            var data = await _airExportHawbAppService.GetAsync(hawbId);
            var mawb = await _airExportMawbAppService.GetAsync(data.MawbId.GetValueOrDefault());
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;

            InfoModel.HawbNo = data.HawbNo;
            InfoModel.Hawb_Pc = data.Package;
            InfoModel.Delivery_To = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.DeliveryTo)).Select(s => s.Text));
            InfoModel.Destination = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(data.DestinationId)).Select(s => s.Text));
            InfoModel.Total_no_of_pieces = data.Package;
            InfoModel.Number_of_forwarder = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.IssuingCarrier)).Select(s => s.Text));
            InfoModel.Origin = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(data.DepartureId)).Select(s => s.Text));
            InfoModel.Air_way_bill_no = mawb.MawbNo;
            InfoModel.ReportId = data.Id;

            return View(InfoModel);
        }

        [HttpPost]
        public async Task<IActionResult> PackageLabelAirExportHawb(PackageLabelAirExportHawbIndexViewModel model)
        {
            model.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(model);

            ReportLog.ReportId = model.ReportId;
            ReportLog.ReportName = "PackageLabelAirExportHawb";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;

            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/PackageLabel/PackageLabelAirExportHawb.cshtml", model);
        }

        public async Task<IActionResult> PreAlertAirExportHawb(Guid hawbId)
        {
            PreAlertAirExportHawbIndexViewModel InfoModel = new();

            var hawb = await _airExportHawbAppService.GetHawbWithDetailsById(hawbId);
            var mawb = await _airExportMawbAppService.GetAsync(hawb.MawbId.GetValueOrDefault());
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;

            InfoModel.Oversea_Agent = hawb.OverseaAgent;
            InfoModel.Hawb_No = hawb.HawbNo;
            InfoModel.Mawb_No = mawb.MawbNo;
            InfoModel.Consignee = hawb.OverseaAgent;
            InfoModel.Shipper_Ref = hawb.ActualShippedr;
            InfoModel.Carrier = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.IssuingCarrier)).Select(s => s.Text));
            InfoModel.Flight_No = mawb.FlightNo;
            InfoModel.Origin = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(hawb.DepartureId)).Select(s => s.Text));
            InfoModel.Destination = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(hawb.DestinationId)).Select(s => s.Text));
            InfoModel.Flight_Date = string.Concat(mawb.DepatureDate);
            InfoModel.Filing_No = mawb.FilingNo;
            InfoModel.ETA = string.Concat(mawb.ArrivalDate);
            InfoModel.Gross_Weight = hawb.GrossWeightShprKG;
            InfoModel.Total_Package = hawb.Package + " " + hawb.PackageUnit;

            return View(InfoModel);
        }

        [HttpPost]
        public async Task<IActionResult> PreAlertAirExportHawb(PreAlertAirExportHawbIndexViewModel model)
        {
            model.IsPDF = true;
            return await _generatePdf.GetPdf("Views/Docs/PreAlertAirExportHawb.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> PackageLabelListAirExportHawb(Guid id, FreightPageType pageType)
        {
            var airExportDetails = await GetAirExportDetailsByPageType(id, pageType);

            return View(airExportDetails);
        }

        [HttpPost]
        public async Task<IActionResult> PackageLabelListAirExportHawb(AirExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/PackageLabelListAirExportHawb.cshtml", model);
        }

        public async Task<IActionResult> BankDraftAirExportHawb(Guid hawbId)
        {
            BankDraftAirExportHawbModel InfoViewModel = new();

            var data = await _airExportHawbAppService.GetAsync(hawbId);
            var tradePartner = _dropdownService.TradePartnerLookupList;

            InfoViewModel.Amount = ".00";
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");
            InfoViewModel.AtSight = "AT SIGHT";
            InfoViewModel.ShipperName = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.ActualShippedr)).Select(s => s.Text));
            InfoViewModel.DrawnDocCredit = "\"DRAWN UNDER DOCUMENTARY CREDIT NO. : OOO";
            InfoViewModel.IssueDate = string.Concat(data.BookingDate);
            InfoViewModel.LCIssueBank = "PPP";
            InfoViewModel.ToLCIssueBank = "PPP";
            InfoViewModel.ToTradePartnerLoadFrom = "CONSIGNEE"; //NOTIFY
            InfoViewModel.Consignee = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.ConsigneeId)).Select(s => s.Text));
            InfoViewModel.NotifyParty = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.Notify)).Select(s => s.Text));
            InfoViewModel.ToTradePartnerName = "GALLAGHER-CANNON";
            InfoViewModel.DraftNo = "";
            InfoViewModel.BankPreference = "";
            InfoViewModel.ShipperName2 = "LONG BEACH CONTAINER PIER F";
            InfoViewModel.Gentlemen = "";
            InfoViewModel.GentlemenNameAddress = "";
            InfoViewModel.EncloseDate = DateTime.Now.ToString("yyyy-MM-dd");
            InfoViewModel.EncloseDraftNo = "";
            InfoViewModel.EncloseForCollection = "";
            InfoViewModel.EncloseForOther = "";
            InfoViewModel.EncloseForPayment = "";
            InfoViewModel.ExtraBl = "";
            InfoViewModel.ExtraBlCopy = "";
            InfoViewModel.ExtraCommInv = "";
            InfoViewModel.ExtraInsCtf = "";
            InfoViewModel.ExtraCtfOrig = "";
            InfoViewModel.ExtraConsInv = "";
            InfoViewModel.ExtraPkngList = "";
            InfoViewModel.ExtraWgtCtf = "";
            InfoViewModel.DeliverInOneMailing = "";
            InfoViewModel.DeliverInTwoMailing = "";
            InfoViewModel.DeliverIfDraft = "";
            InfoViewModel.AllChargesForAccount = "";
            InfoViewModel.DoNotWaiveCharges = "";
            InfoViewModel.ProtestForNonPayment = "";
            InfoViewModel.DoNotProtest = "";
            InfoViewModel.PresentOnArrival = "";
            InfoViewModel.AdviseNonPaymentBy = "";
            InfoViewModel.AdvisePaymentBy = "";
            InfoViewModel.ReferToName = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.IssuingCarrier)).Select(s => s.Text));
            InfoViewModel.ReferToAddress = "";
            InfoViewModel.ToActFullyOnOurBehalf = "";
            InfoViewModel.ToAssistNotToAlter = "";
            InfoViewModel.OtherInstructions = "";
            InfoViewModel.ReferQuestionTo = "";
            InfoViewModel.ReferQuestionName = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.IssuingCarrier)).Select(s => s.Text));
            InfoViewModel.ReferQuestionPhone = "TEL: " + "08417606080";
            InfoViewModel.ReferQuestionShipperName = "LONG BEACH CONTAINER PIER F";
            InfoViewModel.ReferQuestionShipperPhone = "TEL: " + "(358)459-8430";
            InfoViewModel.ReferQuestionFreightForwarderName = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.IssuingCarrier)).Select(s => s.Text));
            InfoViewModel.ReferQuestionFreightForwarderPhone = "TEL: " + "08417606080";
            InfoViewModel.ShipperName3 = "LONG BEACH CONTAINER PIER F";
            InfoViewModel.AuthorizedSignatureInput = "";
            InfoViewModel.UserCompany = CurrentUser.Name + " " + CurrentUser.SurName + "/" + string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.IssuingCarrier)).Select(s => s.Text));

            InfoViewModel.ReportId = data.Id;

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BankDraftAirExportHawb(BankDraftAirExportHawbModel model)
        {
            model.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(model);

            ReportLog.ReportId = model.ReportId;
            ReportLog.ReportName = "BankDraftAirExportHawb";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;

            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/BankDraft/BankDraftAirExportHawb.cshtml", model);
        }

        public async Task<IActionResult> PickupDeliveryOrderAirExportHawb(Guid hawbId)
        {
            PickupDeliveryOrderAirExportHawbModel InfoModel = new();

            var hawb = await _airExportHawbAppService.GetHawbWithDetailsById(hawbId);
            var mawb = await _airExportMawbAppService.GetAsync(hawb.MawbId.GetValueOrDefault());
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;

            InfoModel.Issued_At = string.Concat(DateTime.UtcNow);
            InfoModel.Issued_By = _currentUser.Name + " " + _currentUser.SurName;
            InfoModel.Trucker = hawb.Trucker;
            InfoModel.Mawb_No = mawb.MawbNo;
            InfoModel.Hawb_No = hawb.HawbNo;
            InfoModel.Our_Ref_No = mawb.FilingNo;
            InfoModel.Shipper = hawb.CargoPickupName;
            InfoModel.Consignee = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.ConsigneeId)).Select(s => s.Text));
            InfoModel.Carrier = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.DeliveryTo)).Select(s => s.Text));
            InfoModel.Flight_No = mawb.FlightNo;
            InfoModel.Pickup_Location = hawb.CargoPickupName;
            InfoModel.Port_Of_Loading = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(hawb.DepartureId)).Select(s => s.Text));
            InfoModel.ETD = string.Concat(mawb.DepatureDate);
            InfoModel.Port_Of_Discharge = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(hawb.DestinationId)).Select(s => s.Text));
            InfoModel.ETA = string.Concat(mawb.ArrivalDate);
            InfoModel.Delivery_To = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.DeliveryTo)).Select(s => s.Text));
            InfoModel.Packages = hawb.Package;
            InfoModel.Measurement = hawb.ChargeableWeightCneeLB;
            InfoModel.MeasurementWithCFT = hawb.ChargeableWeightCneeLB == null ? "" : (double.Parse(hawb.ChargeableWeightCneeLB) * 35.315).ToString("0.00");
            InfoModel.Bill_To = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.IssuingCarrier)).Select(s => s.Text));

            return View(InfoModel);
        }


        [HttpPost]
        public async Task<IActionResult> PickupDeliveryOrderAirExportHawb(PickupDeliveryOrderAirExportHawbModel model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/PickupDeliveryOrderAirExportHawb.cshtml", model);
        }

        public async Task<IActionResult> CommercialInvoiceAirExportHawb(Guid hawbId)
        {
            CommercialInvoiceAirExportHawbModel InfoModel = new();

            var hawb = await _airExportHawbAppService.GetHawbWithDetailsById(hawbId);
            var mawb = await _airExportMawbAppService.GetAsync(hawb.MawbId.GetValueOrDefault());
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManangement = _dropdownService.PortsManagementLookupList;

            InfoModel.Shipper = hawb.ActualShippedr;
            InfoModel.Departure_Date = string.Concat(mawb.DepatureDate);
            InfoModel.Consignee = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.ConsigneeId)).Select(s => s.Text));
            InfoModel.Port_Of_Loading = string.Concat(portManangement.Where(w => w.Value == Convert.ToString(hawb.DepartureId)).Select(s => s.Text));
            InfoModel.Notify = hawb.Notify;
            InfoModel.Final_Destination = string.Concat(portManangement.Where(w => w.Value == Convert.ToString(hawb.DestinationId)).Select(s => s.Text));
            InfoModel.Flight_No = mawb.FlightNo;
            InfoModel.Shipping_Mark = hawb.Mark;
            InfoModel.Package_Quantity = hawb.Package + " " + hawb.PackageUnit;
            InfoModel.Description = hawb.NatureAndQuantityOfGoods;
            if (hawb.WTVAL == "PPD")
            {
                InfoModel.Term = "FREIGHT PREPAID";
            }
            else if (hawb.WTVAL == "COLL")
            {
                InfoModel.Term = "FREIGHT COLLECT";
            }
            else
            {
                InfoModel.Term = "";
            }

            return View(InfoModel);
        }

        [HttpPost]
        public async Task<IActionResult> CommercialInvoiceAirExportHawb(CommercialInvoiceAirExportHawbModel model)
        {
            model.IsPDF = true;
            return await _generatePdf.GetPdf("Views/Docs/CommercialInvoiceAirExportHawb.cshtml", model);
        }

        public async Task<IActionResult> BookingConfirmationAirExportHawb(Guid hawbId)
        {
            BookingConfirmationAirExportHawbModel InfoModel = new();

            var data = await _airExportHawbAppService.GetAsync(hawbId);
            var mawb = await _airExportMawbAppService.GetAsync(data.MawbId.GetValueOrDefault());
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;

            InfoModel.HblNo = string.Concat(data.HawbNo);
            InfoModel.ActualShipper = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.ActualShippedr)).Select(s => s.Text));
            InfoModel.File_No = string.Concat(mawb.MawbNo);
            InfoModel.BookingDate = string.Concat(data.BookingDate);
            InfoModel.PoNo = string.Concat(data.PONo);
            InfoModel.ItnNo = string.Concat(data.ITNNo);
            InfoModel.Consignee = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.ConsigneeId)).Select(s => s.Text));
            InfoModel.Agent = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.OverseaAgent)).Select(s => s.Text));
            InfoModel.Notify = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.Notify)).Select(s => s.Text));
            InfoModel.Flight_No = string.Concat(mawb.FlightNo);
            InfoModel.Departure = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(data.DepartureId)).Select(s => s.Text));
            InfoModel.Destination = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(data.DestinationId)).Select(s => s.Text));
            InfoModel.Carrier = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.DeliveryTo)).Select(s => s.Text));
            InfoModel.ETD = string.Concat(mawb.DepatureDate);
            InfoModel.ETA = string.Concat(mawb.ArrivalDate);
            InfoModel.Measurement = data.ChargeableWeightCneeLB == null ? "" : string.Concat(data.ChargeableWeightCneeLB) + "CBM" + " " + (double.Parse(data.ChargeableWeightCneeLB) * 35.315).ToString("0.00") + "CFT";
            InfoModel.PKG = string.Concat(data.Package) + " " + string.Concat(data.PackageUnit);
            InfoModel.Deliver_To = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.DeliveryTo)).Select(s => s.Text));
            InfoModel.CargoPickUp = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.CargoPickup)).Select(s => s.Text));
            InfoModel.Trucker = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data.Trucker)).Select(s => s.Text));

            return View(InfoModel);
        }

        [HttpPost]
        public async Task<IActionResult> BookingConfirmationAirExportHawb(BookingConfirmationAirExportHawbModel model)
        {
            model.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(model);

            ReportLog.ReportId = model.ReportId;
            ReportLog.ReportName = "BookingConfirmationAirExportHawb";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;

            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/BookingConfirmation/BookingConfirmationAirExportHawb.cshtml", model);
        }
        public async Task<IActionResult> HawbPrintAirExportHawb()
        {




            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PackagingListAirExportHawb(PackagingListAirExportHawb InfoViewModel, Guid guid)
        {

            InfoViewModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            string Input = JsonConvert.SerializeObject(InfoViewModel);

            ReportLog.ReportId = InfoViewModel.ReportId;
            ReportLog.ReportName = "PackagingListAirExportHawb";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/PackagingListAirExportHawb/Default.cshtml", InfoViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> DocumentPackage(Guid id, FreightPageType pageType)
        {
            var airExportDetails = await GetAirExportDetailsByPageType(id, pageType);

            return View(airExportDetails);
        }

        [HttpPost]
        public async Task<IActionResult> DocumentPackage(AirExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/DocumentPackage.cshtml", model);
        }

        public async Task<IActionResult> PackageLabelAirExportMawb(Guid mawbId)
        {
            PackageLabelAirExportMawbIndexViewModel InfoModel = new();

            var mawb = await _airExportMawbAppService.GetAsync(mawbId);
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;

            InfoModel.Carrier = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.MawbCarrierId)).Select(s => s.Text));
            InfoModel.Air_Way_Bill_No = mawb.MawbNo;
            InfoModel.Destination = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DestinationId)).Select(s => s.Text));
            InfoModel.Total_No_Of_Pieces = string.Concat(mawb.Package);
            InfoModel.Consignee = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.ConsigneeId)).Select(s => s.Text));
            InfoModel.Origin = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DepatureId)).Select(s => s.Text));

            return View(InfoModel);
        }
        [HttpPost]
        public async Task<IActionResult> PackageLabelAirExportMawb(PackageLabelAirExportMawbIndexViewModel model)
        {
            model.IsPDF = true;
            return await _generatePdf.GetPdf("Views/Docs/PackageLabelAirExportMawb.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> ProfitReport(Guid id, FreightPageType pageType, string reportType)
        {
            string returnUrl = string.Empty;

            var airExportDetails = await GetAirExportDetailsByPageType(id, pageType);

            var measurement = Convert.ToDouble(airExportDetails.ChargeableWeightCneeLB) * 35.315;

            var profitReport = new ProfitReportViewModel()
            {
                AgentName = airExportDetails.OverSeaAgentName,
                Consignee = airExportDetails.ConsigneeName,
                Currency = "USD",
                Customer = airExportDetails.CustomerName,
                HawbNo = airExportDetails.HawbNo,
                Operator = airExportDetails.Operator,
                Measurement = measurement.ToString("0.00"),
                PolEtd = string.Concat(airExportDetails.DepartureName, " / ", airExportDetails.DepatureDate),
                PodEtd = string.Concat(airExportDetails.DestinationName, " / ", airExportDetails.ArrivalDate),
                Sales = airExportDetails.SalesName,
                Shipper = airExportDetails.CarrierName,
                MawbNo = airExportDetails.MawbNo,
                ChargableWeight = string.Concat(airExportDetails.ChargeableWeightCneeKG, " / ", airExportDetails.ChargeableWeightCneeLB),
                PageType = pageType,
                ReportType = reportType,
                FileNo = airExportDetails.DocNumber
            };

            var queryType = pageType == FreightPageType.AEMBL ? 5 : 4;

            QueryInvoiceDto queryDto = new QueryInvoiceDto() { QueryType = queryType, ParentId = id };
            profitReport.Invoices = await _invoiceAppService.QueryInvoicesAsync(queryDto);
            profitReport.AR = new List<InvoiceDto>();
            profitReport.AP = new List<InvoiceDto>();
            profitReport.DC = new List<InvoiceDto>();

            if (profitReport.Invoices != null && profitReport.Invoices.Count > 0)
            {
                foreach (var dto in profitReport.Invoices)
                {
                    switch (dto.InvoiceType)
                    {
                        default:
                            profitReport.AR.Add(dto);
                            break;
                        case 1:
                            profitReport.DC.Add(dto);
                            break;
                        case 2:
                            profitReport.AP.Add(dto);
                            break;
                    }
                }
                profitReport.InvoicesJson = JsonConvert.SerializeObject(profitReport.Invoices);
            }

            if (profitReport.AR.Any())
            {
                double arTotal = 0;
                foreach (var ar in profitReport.AR)
                {
                    arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                }
                profitReport.ARTotal = arTotal;
            }
            if (profitReport.AP.Any())
            {
                double apTotal = 0;
                foreach (var ap in profitReport.AP)
                {
                    apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                }

                profitReport.APTotal = apTotal;
            }
            if (profitReport.DC.Any())
            {
                double dcTotal = 0;
                foreach (var dc in profitReport.DC)
                {
                    dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                }
                profitReport.DCTotal = dcTotal;
            }

            profitReport.Total = profitReport.ARTotal - profitReport.APTotal + profitReport.DCTotal;

            returnUrl = pageType == FreightPageType.AEMBL
                ? (reportType == "Summary") ? "Views/Docs/MawbProfitReport.cshtml" : "Views/Docs/MawbProfitReportDetailed.cshtml"
                : "Views/Docs/HawbProfitReport.cshtml";

            return View(returnUrl, profitReport);
        }

        [HttpPost]
        public async Task<IActionResult> ProfitReport(ProfitReportViewModel model)
        {
            string returnUrl = GetViewUrlByPageType(model.PageType, model.ReportType);

            model.IsPDF = true;

            model.Invoices = JsonConvert.DeserializeObject<IList<InvoiceDto>>(model.InvoicesJson);

            return await _generatePdf.GetPdf(returnUrl, model);
        }

        public async Task<IActionResult> BookingConfirmationAirExportMawb(Guid mawbId)
        {
            BookingConfirmationAirExportMawbModel InfoModel = new();

            var mawb = await _airExportMawbAppService.GetAsync(mawbId);
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;
            var packageUnit = _dropdownService.PackageUnitLookupList;

            InfoModel.ActualShipper = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.ShipperId)).Select(s => s.Text));
            InfoModel.MawbNo = mawb.MawbNo;
            InfoModel.File_No = mawb.FilingNo;
            InfoModel.ItnNo = mawb.ItnNo;
            InfoModel.Consignee = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.ConsigneeId)).Select(s => s.Text));
            InfoModel.Notify = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.NotifyId)).Select(s => s.Text));
            InfoModel.Flight_No = mawb.FlightNo;
            InfoModel.Carrier = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.MawbCarrierId)).Select(s => s.Text));
            InfoModel.Departure = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DepatureId)).Select(s => s.Text));
            InfoModel.ETD = string.Concat(mawb.DepatureDate);
            InfoModel.Destination = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DestinationId)).Select(s => s.Text));
            InfoModel.ETA = string.Concat(mawb.ArrivalDate);
            InfoModel.Measurement = mawb.VolumeWeightCbm == 0 ? "" : string.Concat(mawb.VolumeWeightCbm) + " CBM" + " " + (mawb.VolumeWeightCbm * 35.315).ToString("0.00") + " CFT";
            InfoModel.PKG = string.Concat(mawb.Package) + " " + string.Concat(packageUnit.Where(w => w.Value == Convert.ToString(mawb.MawbPackageUnitId)).Select(s => s.Text));
            InfoModel.Deliver_To = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.DeliveryId)).Select(s => s.Text));

            return View(InfoModel);
        }

        [HttpPost]
        public async Task<IActionResult> BookingConfirmationAirExportMawb(BookingConfirmationAirExportMawbModel model)
        {
            model.IsPDF = true;
            return await _generatePdf.GetPdf("Views/Docs/BookingConfirmationAirExportMawb.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> AllHawbPackageLabelAirExportMawb(Guid mawbId)
        {
            AllHawbPackageLabelModel InfoModel = new();
            var mawb = await _airExportMawbAppService.GetAsync(mawbId);
            var hawb = await _airExportHawbAppService.GetHblCardsById(mawbId);
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;
            var allHawbLists = await GetAllHawbLists(mawbId);

            InfoModel.AllHawbLists = allHawbLists;
            InfoModel.Air_WayBill_No = mawb.MawbNo;
            InfoModel.Destination = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DestinationId)).Select(s => s.Text));
            InfoModel.Name_Of_Forwarder = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.IssuingCarrierId)).Select(s => s.Text));
            InfoModel.Origin = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DepatureId)).Select(s => s.Text));
            InfoModel.Hawb_No = hawb[0].HawbNo;
            InfoModel.Hawb_Pc = hawb[0].Package;
            InfoModel.Total_No_Of_Pieces = string.Concat(mawb.Package);

            return View(InfoModel);
        }
        [HttpPost]
        public async Task<IActionResult> AllHawbPackageLabelAirExportMawb(AllHawbPackageLabelModel model)
        {
            model.IsPDF = true;
            return await _generatePdf.GetPdf("Views/Docs/AllHawbPackageLabelAirExportMawb.cshtml", model);
        }

        public async Task<IActionResult> PickupDeliveryOrderAirExportMawb(Guid mawbId)
        {
            PickupDeliveryOrderAirExportMawbModel InfoModel = new();

            var mawb = await _airExportMawbAppService.GetAsync(mawbId);
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;
            var packageUnit = _dropdownService.PackageUnitLookupList;

            InfoModel.Issued_At = string.Concat(DateTime.UtcNow);
            InfoModel.Issued_By = _currentUser.Name + " " + _currentUser.SurName;
            InfoModel.Mawb_No = mawb.MawbNo;
            InfoModel.Our_Ref_No = mawb.FilingNo;
            InfoModel.Shipper = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.ShipperId)).Select(s => s.Text));
            InfoModel.Consignee = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.ConsigneeId)).Select(s => s.Text));
            InfoModel.Carrier = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.MawbCarrierId)).Select(s => s.Text));
            InfoModel.Flight_No = mawb.FlightNo;
            InfoModel.Port_Of_Loading = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DepatureId)).Select(s => s.Text));
            InfoModel.ETD = string.Concat(mawb.DepatureDate);
            InfoModel.Port_Of_Discharge = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DestinationId)).Select(s => s.Text));
            InfoModel.ETA = string.Concat(mawb.ArrivalDate);
            InfoModel.Packages = string.Concat(mawb.Package);
            InfoModel.Package_Unit = string.Concat(packageUnit.Where(w => w.Value == Convert.ToString(mawb.MawbPackageUnitId)).Select(s => s.Text));
            InfoModel.Measurement = string.Concat(mawb.VolumeWeightCbm);
            InfoModel.MeasurementWithCFT = mawb.VolumeWeightCbm == 0.00 ? "" : (mawb.VolumeWeightCbm * 35.315).ToString("0.00");
            InfoModel.Bill_To = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.IssuingCarrierId)).Select(s => s.Text));

            return View(InfoModel);
        }
        [HttpPost]
        public async Task<IActionResult> PickupDeliveryOrderAirExportMawb(PickupDeliveryOrderAirExportMawbModel model)
        {
            model.IsPDF = true;
            return await _generatePdf.GetPdf("Views/Docs/PickupDeliveryOrderAirExportMawb.cshtml", model);
        }

        public async Task<IActionResult> OnHandReportAirExportMawb(Guid mawbId)
        {
            OnHandReportAirExportMawbModel InfoModel = new();

            var mawb = await _airExportMawbAppService.GetAsync(mawbId);
            var allHawbLists = await GetAllHawbLists(mawbId);

            InfoModel.AllHawbLists = allHawbLists;

            InfoModel.Mawb_No = mawb.MawbNo;

            InfoModel.HawbListsJson = JsonConvert.SerializeObject(allHawbLists);

            return View(InfoModel);
        }
        [HttpPost]
        public async Task<IActionResult> OnHandReportAirExportMawb(OnHandReportAirExportMawbModel model)
        {
            model.IsPDF = true;

            model.AllHawbLists = JsonConvert.DeserializeObject<List<AllHawbList>>(model.HawbListsJson);

            return await _generatePdf.GetPdf("Views/Docs/OnHandReportAirExportMawb.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> PrintMawb(Guid id, FreightPageType pageType)
        {
            var airExportDetails = await GetAirExportDetailsByPageType(id, pageType);

            return View(airExportDetails);
        }

        [HttpPost]
        public async Task<IActionResult> PrintMawb(AirExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/PrintMawb.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> SecurityEndorsement(Guid id, FreightPageType pageType)
        {
            var airExportDetails = await GetAirExportDetailsByPageType(id, pageType);

            return View(airExportDetails);
        }

        [HttpPost]
        public async Task<IActionResult> SecurityEndorsement(AirExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/SecurityEndorsement.cshtml", model);
        }

        public async Task<IActionResult> ManifestByAgentAirExportMawbPartial(Guid mawbId)
        {
            ManifestByAgentAirExportMawb InfoModel = new();

            var mawb = await _airExportMawbAppService.GetAsync(mawbId);
            var datas = _dropdownService.TradePartnerLookupList;
            var overSeaAgents = new List<OverSeaAgent>();

            foreach (var overSeaAgent in datas)
            {
                var data = new OverSeaAgent
                {
                    Name = overSeaAgent.Text
                };
                overSeaAgents.Add(data);
            }

            InfoModel.Agent = string.Concat(datas.Where(w => w.Value == Convert.ToString(mawb.ConsigneeId)).Select(s => s.Text));
            InfoModel.OverSeaAgents = overSeaAgents;

            return PartialView("Pages/Shared/_ManifestByAgent.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> ManifestByAgentAirExportMawb(Guid mawbId, string agent)
        {
            ManifestByAgentAirExportMawb InfoModel = new ManifestByAgentAirExportMawb();

            var mawb = await _airExportMawbAppService.GetAsync(mawbId);
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;
            var packageUnit = _dropdownService.PackageUnitLookupList;

            InfoModel.Mawb_No = mawb.MawbNo;
            InfoModel.Carrier_Agent = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.IssuingCarrierId)).Select(s => s.Text));
            InfoModel.File_No = mawb.FilingNo;
            InfoModel.Flight_No = mawb.FlightNo;
            InfoModel.Carrier = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.MawbCarrierId)).Select(s => s.Text));
            InfoModel.Departure = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DepatureId)).Select(s => s.Text));
            InfoModel.ETD = string.Concat(mawb.DepatureDate);
            InfoModel.Destination = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mawb.DestinationId)).Select(s => s.Text));
            InfoModel.ETA = string.Concat(mawb.ArrivalDate);
            InfoModel.From = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.ShipperId)).Select(s => s.Text));
            InfoModel.Shipper = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.ShipperId)).Select(s => s.Text));
            InfoModel.To = agent;
            InfoModel.Total_Package = string.Concat(mawb.Package) + " " + string.Concat(packageUnit.Where(w => w.Value == Convert.ToString(mawb.MawbPackageUnitId)).Select(s => s.Text));
            InfoModel.Total_Pc = string.Concat(mawb.Package);
            InfoModel.Gross_Weight = string.Concat(mawb.GrossWeightKg) + " KG" + " " + string.Concat(mawb.GrossWeightLb) + " LB";
            InfoModel.Chargable_Weight = string.Concat(mawb.ChargeableWeightKg) + " KG" + " " + string.Concat(mawb.ChargeableWeightLb) + " LB";
            InfoModel.Itn_No = mawb.ItnNo;
            InfoModel.Measurement = string.Concat(mawb.VolumeWeightCbm) + " CBM" + " " + (mawb.VolumeWeightCbm * 35.315).ToString("0.00") + " CFT";
            if (mawb.WtVal == "PPD")
            {
                InfoModel.Term = "PP";
            }
            else if (mawb.WtVal == "COLL")
            {
                InfoModel.Term = "CC";
            }

            return View(InfoModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManifestByAgentAirExportMawb(ManifestByAgentAirExportMawb model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/ManifestByAgentAirExportMawb.cshtml", model);
        }

        public async Task<IActionResult> ConsolidatedManifestByAgentAirExportMawbPartial(Guid mawbId)
        {
            ManifestByAgentAirExportMawb InfoModel = new();

            var mawb = await _airExportMawbAppService.GetAsync(mawbId);
            var datas = _dropdownService.TradePartnerLookupList;
            var overSeaAgents = new List<OverSeaAgent>();

            foreach (var overSeaAgent in datas)
            {
                var data = new OverSeaAgent
                {
                    Name = overSeaAgent.Text
                };
                overSeaAgents.Add(data);
            }

            InfoModel.Mawb_No = mawb.MawbNo;
            InfoModel.Agent = string.Concat(datas.Where(w => w.Value == Convert.ToString(mawb.ConsigneeId)).Select(s => s.Text));
            InfoModel.OverSeaAgents = overSeaAgents;

            return PartialView("Pages/Shared/_ConsolidatedManifestByAgent.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> MblProfitReportSummary(Guid id, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType, true);

            return View(oceanExportDetails);
        }

        [HttpPost]
        public async Task<IActionResult> MblProfitReportSummary(OceanExportDetails model)
        {
            model.IsPDF = true;

            model.Invoices = JsonConvert.DeserializeObject<List<InvoiceDto>>(model.InvoicesJson);

            return await _generatePdf.GetPdf("Views/Docs/MblProfitReportSummary.cshtml", model);
        }
        public async Task<IActionResult> AllHblPackageLabelOceanExportMBL(Guid mblId)
        {
            AllHblPackageLabelOceanExportMBL InfoModel = new();
            var mbl = await _oceanExportMblAppService.GetAsync(mblId);
            var hbl = await _oceanExportHblAppService.GetHblCardsById(mblId);
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var portManagement = _dropdownService.PortsManagementLookupList;

            InfoModel.AllHblLists = await GetAllHblLists(mblId);
            InfoModel.Mbl_No = mbl.MblNo;
            InfoModel.Hbl_No = hbl[0].HblNo;
            InfoModel.To = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mbl.MblOverseaAgentId)).Select(s => s.Text));
            InfoModel.Carrier = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mbl.ShippingAgentId)).Select(s => s.Text));
            InfoModel.Destination = string.Concat(portManagement.Where(w => w.Value == Convert.ToString(mbl.PodId)).Select(s => s.Text));

            return View(InfoModel);
        }
        [HttpPost]
        public async Task<IActionResult> AllHblPackageLabelOceanExportMBL(AllHblPackageLabelOceanExportMBL model)
        {
            model.IsPDF = true;
            return await _generatePdf.GetPdf("Views/Docs/AllHblPackageLabelOceanExportMBL.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> MblProfitReportDetailed(Guid id, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType, true);

            return View(oceanExportDetails);
        }

        [HttpPost]
        public async Task<IActionResult> MblProfitReportDetailed(OceanExportDetails model)
        {
            model.IsPDF = true;

            model.Invoices = JsonConvert.DeserializeObject<List<InvoiceDto>>(model.InvoicesJson);

            return await _generatePdf.GetPdf("Views/Docs/MblProfitReportDetailed.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> AirImportProfitReport(Guid id, FreightPageType pageType, string reportType)
        {
            string returnUrl = string.Empty;

            QueryInvoiceDto queryDto = new QueryInvoiceDto();

            var airImportDetails = await GetAirImportDetailsByPageType(id, pageType);

            var measurement = Convert.ToDouble(airImportDetails.ChargeableWeightKg) * 35.315;

            var profitReport = new ProfitReportViewModel()
            {
                AgentName = airImportDetails.OverseaAgentTPName,
                Consignee = airImportDetails.ConsigneeName,
                Currency = "USD",
                Customer = airImportDetails.CustomerName,
                Operator = airImportDetails.OPName,
                Measurement = measurement.ToString("0.00"),
                PolEtd = string.Concat(airImportDetails.DepatureAirportName, "/", airImportDetails.DepatureDate).TrimStart('/'),
                PodEtd = string.Concat(airImportDetails.DestinationAirportName, "/", airImportDetails.ArrivalDate).TrimStart('/'),
                Sales = airImportDetails.SalesName,
                Shipper = airImportDetails.ShipperName,
                MawbNo = airImportDetails.MawbNo,
                ChargableWeight = string.Concat(airImportDetails.ChargeableWeightKg, " / ", airImportDetails.ChargeableWeightLb),
                PageType = pageType,
                ReportType = reportType,
                FileNo = airImportDetails.DocNumber,
                HawbNo = airImportDetails.HawbNo,
                SvcTermToName = Convert.ToString(airImportDetails.ServiceTermTypeTo),
                SvcTermFromName = Convert.ToString(airImportDetails.ServiceTermTypeFrom),
                SalesType = airImportDetails.SalesType
            };

            if (pageType == FreightPageType.AIHBL)
            {

                // Hawb invoices
                queryDto = new QueryInvoiceDto() { QueryType = 4, ParentId = id };

                profitReport.Invoices = await _invoiceAppService.QueryInvoicesAsync(queryDto);

                // Mawb invoices
                queryDto = new QueryInvoiceDto() { QueryType = 5, ParentId = airImportDetails.MawbId };

                var mawbInvoices = await _invoiceAppService.QueryInvoicesAsync(queryDto);

                if (mawbInvoices != null && mawbInvoices.Any())
                {
                    profitReport.Invoices ??= new List<InvoiceDto>();

                    foreach (var mawbInvoice in mawbInvoices)
                    {
                        profitReport.Invoices.Add(mawbInvoice);
                    }
                }

            }
            else
            {
                queryDto = new QueryInvoiceDto() { QueryType = 5, ParentId = id };

                profitReport.Invoices = await _invoiceAppService.QueryInvoicesAsync(queryDto);
            }

            profitReport.AR = new List<InvoiceDto>();
            profitReport.AP = new List<InvoiceDto>();
            profitReport.DC = new List<InvoiceDto>();

            if (profitReport.Invoices != null && profitReport.Invoices.Count > 0)
            {
                foreach (var dto in profitReport.Invoices)
                {
                    switch (dto.InvoiceType)
                    {
                        default:
                            profitReport.AR.Add(dto);
                            break;
                        case 1:
                            profitReport.DC.Add(dto);
                            break;
                        case 2:
                            profitReport.AP.Add(dto);
                            break;
                    }
                }
                profitReport.InvoicesJson = JsonConvert.SerializeObject(profitReport.Invoices);
            }

            if (profitReport.AR.Any())
            {
                double arTotal = 0;
                foreach (var ar in profitReport.AR)
                {
                    arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                }
                profitReport.ARTotal = arTotal;
            }
            if (profitReport.AP.Any())
            {
                double apTotal = 0;
                foreach (var ap in profitReport.AP)
                {
                    apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                }

                profitReport.APTotal = apTotal;
            }
            if (profitReport.DC.Any())
            {
                double dcTotal = 0;
                foreach (var dc in profitReport.DC)
                {
                    dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                }
                profitReport.DCTotal = dcTotal;
            }

            profitReport.Total = profitReport.ARTotal - profitReport.APTotal + profitReport.DCTotal;

            returnUrl = pageType == FreightPageType.AIMBL
                ? (reportType == "Summary") ? "Views/Docs/MawbProfitReport.cshtml" : "Views/Docs/MawbProfitReportDetailed.cshtml"
                : "Views/Docs/HawbProfitReport.cshtml";

            return View(returnUrl, profitReport);
        }

        [HttpPost]
        public async Task<IActionResult> AirImportProfitReport(ProfitReportViewModel model)
        {
            string returnUrl = model.PageType == FreightPageType.AIMBL
                ? (model.ReportType == "Summary") ? "Views/Docs/MawbProfitReport.cshtml" : "Views/Docs/MawbProfitReportDetailed.cshtml"
                : "Views/Docs/HawbProfitReport.cshtml";

            model.IsPDF = true;

            model.Invoices = JsonConvert.DeserializeObject<IList<InvoiceDto>>(model.InvoicesJson);

            return await _generatePdf.GetPdf(returnUrl, model);
        }

        public async Task<IActionResult> ManifestAirImport(Guid id, FreightPageType pageType)
        {
            var airImportDetails = await GetAirImportMawbDetailsByPageType(id, pageType);

            return View(airImportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ManifestAirImport(AirImportMawbDto model)
        {
            model.IsPDF = true;

            model.AllHawbListAirImports = JsonConvert.DeserializeObject<List<AllHawbListAirImport>>(model.HawbJson);

            return await _generatePdf.GetPdf("Views/Docs/ManifestAirImport.cshtml", model);
        }

        public async Task<IActionResult> ConsolidatedArrivalNoticePartialView(Guid mawbId, string conId)
        {
            AirImportDetails InfoModel = new();

            var hawb = await _airImportHawbAppService.GetHawbCardsByMawbId(mawbId);
            var tradePartners = _dropdownService.TradePartnerLookupList;
            var overSeaAgents = new List<OverSeaAgentAirImport>();

            foreach (var overSeaAgent in tradePartners)
            {
                var data = new OverSeaAgentAirImport
                {
                    Id = overSeaAgent.Value,
                    Name = overSeaAgent.Text
                };
                overSeaAgents.Add(data);
            }

            InfoModel.CurrentAgentId = string.Concat(conId);
            InfoModel.CurrentAgent = string.Concat(tradePartners.Where(w => w.Value == Convert.ToString(conId)).Select(s => s.Text));
            InfoModel.OverSeaAgents = overSeaAgents;

            return PartialView("Pages/Shared/_ConsolidatedArrivalNotice.cshtml", InfoModel);
        }
        [HttpGet]
        public async Task<IActionResult> ConsolidatedArrivalNoticeAirImport(Guid mawbId, string agent, FreightPageType pageType)
        {
            var data = await _airImportHawbAppService.GetHawbCardsByMawbId(mawbId);
            var tradePartner = _dropdownService.TradePartnerLookupList;
            var packgeUnit = _dropdownService.PackageUnitLookupList;
            var airImportDetails = await GetAirImportDetailsByPageType(mawbId, pageType);
            var hawbNos = new List<HawbNo>();

            foreach (var item in data)
            {
                var hawb = new HawbNo
                {
                    HawbNos = item.HawbNo,
                    OverSeaAgent = string.Concat(item.ConsigneeId),
                    GrossWeightKG = item.GrossWeightKG,
                    ChargableWeightKG = item.ChargeableWeightKG,
                    VolumeWeightKG = item.VolumeWeightKG,
                    MeasurementWeight = item.VolumeWeightCBM,
                    Packages = item.Package + " " + string.Concat(packgeUnit.Where(w => w.Value == Convert.ToString(item.PackageUnit)).Select(s => s.Text))
                };
                hawbNos.Add(hawb);
            }
            airImportDetails.HawbNos = hawbNos;
            airImportDetails.PackagesStr = string.Join("\n", hawbNos.Where(w => w.OverSeaAgent == agent).Select(s => s.Packages));
            airImportDetails.ChargableWeightStr = string.Concat(hawbNos.Where(w => w.OverSeaAgent == agent).Sum(s => Convert.ToInt64(s.ChargableWeightKG))) + " KGS " + (hawbNos.Where(w => w.OverSeaAgent == agent).Sum(s => Convert.ToInt64(s.ChargableWeightKG)) * 2.20462).ToString("0.00") + " LBS";
            airImportDetails.GrossWeightStr = string.Concat(hawbNos.Where(w => w.OverSeaAgent == agent).Sum(s => Convert.ToInt64(s.GrossWeightKG))) + " KGS " + (hawbNos.Where(w => w.OverSeaAgent == agent).Sum(s => Convert.ToInt64(s.GrossWeightKG)) * 2.20462).ToString("0.00") + " LBS";
            airImportDetails.VolumeWeightStr = string.Concat(hawbNos.Where(w => w.OverSeaAgent == agent).Sum(s => Convert.ToInt64(s.VolumeWeightKG))) + " KGS " + (hawbNos.Where(w => w.OverSeaAgent == agent).Sum(s => Convert.ToInt64(s.VolumeWeightKG)) * 2.20462).ToString("0.00") + " LBS";
            airImportDetails.MeasurementStr = string.Concat(hawbNos.Where(w => w.OverSeaAgent == agent)
                                                                   .Where(w => !string.IsNullOrEmpty(w.MeasurementWeight) && double.TryParse(w.MeasurementWeight, out _))
                                                                   .Sum(s => double.Parse(s.MeasurementWeight)))
                                                                   + " CBM " +
                                                                   (hawbNos.Where(w => w.OverSeaAgent == agent)
                                                                   .Where(w => !string.IsNullOrEmpty(w.MeasurementWeight) && double.TryParse(w.MeasurementWeight, out _))
                                                                   .Sum(s => double.Parse(s.MeasurementWeight) * 35.315)).ToString("0.00") + " CFT";
            airImportDetails.HawbString = "Hawb Nos:- " + string.Join(", ", hawbNos.Where(w => w.OverSeaAgent == agent).Select(s => s.HawbNos));
            airImportDetails.Hawb_Nos = JsonConvert.SerializeObject(hawbNos);
            airImportDetails.CurrentAgent = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(agent)).Select(s => s.Text));
            airImportDetails.ShipperName = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data[0].ShipperId)).Select(s => s.Text));
            airImportDetails.NotifyName = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data[0].Notify)).Select(s => s.Text));
            airImportDetails.CustomBroker = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(data[0].CustomsBroker)).Select(s => s.Text));

            return View(airImportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ConsolidatedArrivalNoticeAirImport(AirImportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/ConsolidatedArrivalNoticeAirImport.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> ArrivalNoticeAirImportHawb(Guid id, FreightPageType pageType)
        {
            var airImportDetails = await GetAirImportDetailsByPageType(id, pageType);

            airImportDetails.GrossWeightStr = airImportDetails.GrossWeightKg == 0 ? "" : string.Concat(airImportDetails.GrossWeightKg) + " KGS " + (double.Parse(string.Concat(airImportDetails.GrossWeightKg)) * 2.20462).ToString("0.00") + " LBS";
            airImportDetails.ChargableWeightStr = airImportDetails.ChargeableWeightKg == 0 ? "" : string.Concat(airImportDetails.ChargeableWeightKg) + " KGS " + (double.Parse(string.Concat(airImportDetails.ChargeableWeightKg)) * 2.20462).ToString("0.00") + " LBS";
            airImportDetails.VolumeWeightStr = airImportDetails.VolumeWeightKg == 0 ? "" : string.Concat(airImportDetails.VolumeWeightKg) + " KGS " + (double.Parse(string.Concat(airImportDetails.VolumeWeightKg)) * 2.20462).ToString("0.00") + " LBS";
            airImportDetails.MeasurementStr = airImportDetails.VolumeWeightCbm == 0 ? "" : string.Concat(airImportDetails.VolumeWeightCbm) + " CBM " + (double.Parse(string.Concat(airImportDetails.VolumeWeightCbm)) * 35.315).ToString("0.00") + " CFT";

            return View(airImportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ArrivalNoticeAirImportHawb(AirImportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/ArrivalNoticeAirImportHawb.cshtml", model);
        }

        public async Task<IActionResult> HawbAuthority(Guid id, FreightPageType pageType)
        {
            var data = await GetAirImportDetailsByPageType(id, pageType);
            var viewModel = new AuthorityViewModel()
            {
                AgentName = data.OverseaAgentTPName,
                Consignee = data.ConsigneeName,
                CustomsBroker = data.CustomerName,
                Date = DateTime.Now.ToShortDateString(),
                DepAirport = data.DepatureName,
                DescriptionITNO = data.ITNo,
                DestAirport = data.DestinationAirportName,
                DocumentPickedBy = data.ReleasedBy,
                EntryPort = data.DestinationAirportName,
                ETA1 = string.Concat(data.ArrivalDate),
                ETA2 = string.Concat(data.ArrivalDate),
                ETA3 = string.Concat(data.ArrivalDate),
                ETD = data.DepatureDate,
                FileNo = data.FilingNo,
                FinalDest = data.FinalDestination,
                FlightNo = data.FlightNo,
                HawbNo = data.HawbNo,
                MawbNo = data.MawbNo,
                FrightLoc = data.FreightLocationName,
                ITNO = data.ITNo,
                ITDate = string.Concat(data.ITDate),
                ITIssuePlace = data.ITIssuedLocation,
                LastFreeDay = data.LastFreeDay,
                MerchandiseImportedAt = data.DestinationAirportName,
                NotifyParty = data.NotifyName,
                On = string.Concat(data.ArrivalDate),
                PCS = data.Package,
                Remark = data.Remark,
                Shipper = data.ShipperName,
                //SubHawb = string.Join(",", data.SubHawbs?.Select(s => s.SubHAWB)),
                Trucker = data.HTruckerName,
                //Amount = data.SubHawbs?.Sum(s => Convert.ToDouble(s.Amount)),
                VIA = data.DeliveryLocationName,
                PrepBy = data.OPName
            };

            if (data.SubHawbs != null && data.SubHawbs.Any())
            {
                viewModel.SubHawb = string.Join(",", data.SubHawbs?.Select(s => s.SubHAWB));
                viewModel.Amount = data.SubHawbs?.Sum(s => Convert.ToDouble(s.Amount));
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> HawbAuthority(AuthorityViewModel model)
        {
            model.IsPDF = true;
            return await _generatePdf.GetPdf("Views/Docs/HawbAuthority.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> BatchPrintingPartialView(Guid mawbId, Guid hawbId)
        {
            AirImportDetails airImportDetails = new AirImportDetails();

            var tradePartners = _dropdownService.TradePartnerLookupList;
            var hawb = await _airImportHawbAppService.GetHawbCardsByMawbId(mawbId);

            var hawbLists = new List<HawbNo>();

            foreach (var item in hawb)
            {
                hawbLists.Add(new HawbNo()
                {
                    Id = string.Concat(item.Id),
                    HawbNos = item.HawbNo,
                    Consignee = string.Concat(tradePartners.Where(w => w.Value == Convert.ToString(item.ConsigneeId)).Select(s => s.Text)),
                    Notify = string.Concat(tradePartners.Where(w => w.Value == Convert.ToString(item.Notify)).Select(s => s.Text)),
                    Customer = string.Concat(tradePartners.Where(w => w.Value == Convert.ToString(item.Customer)).Select(s => s.Text))
                });
            }

            airImportDetails.HawbId = hawbId;
            airImportDetails.HawbNos = hawbLists;

            return PartialView("Pages/Shared/_BatchPrinting.cshtml", airImportDetails);
        }
        [HttpGet]
        public async Task<IActionResult> BatchPrintingAirImport(Guid mawbId, FreightPageType pageType, Guid hawbId, string hawbIdJson)
        {
            var airImportDetails = await GetAirImportDetailsByPageType(hawbId, pageType);

            var ids = JsonConvert.DeserializeObject<List<string>>(hawbIdJson);
            var hawbsList = new List<HawbNo>();

            foreach (var item in ids)
            {
                var hawbs = new HawbNo
                {
                    Id = item
                };
                hawbsList.Add(hawbs);
            }

            var newHawbs = new List<Hawb>();

            foreach (var hawb in hawbsList)
            {
                var data = await _airImportHawbAppService.GetHawbCardById(Guid.Parse(hawb.Id));
                var newHawb = new Hawb
                {
                    Id = string.Concat(data.Id),
                    HawbNo = data.HawbNo
                };

                var airImportSubHawbs = await GetAirImportDetailsByPageType(Guid.Parse(hawb.Id), pageType);

                newHawb.Shipper = airImportSubHawbs.ShipperName;
                newHawb.Consignee = airImportSubHawbs.ConsigneeName;
                newHawb.Notify = airImportSubHawbs.NotifyName;
                newHawb.FinalDestName = airImportSubHawbs.FinalDestination;
                newHawb.FDestETA = airImportSubHawbs.FDestETA;
                newHawb.LastFreeDay = airImportSubHawbs.LastFreeDay;
                newHawb.FreightLocation = airImportSubHawbs.FreightLocationName;
                newHawb.ITNo = airImportSubHawbs.HItNo;
                newHawb.ITIssuePlace = airImportSubHawbs.HItLocation;
                newHawb.ITDate = airImportSubHawbs.HItDate;
                newHawb.SubHawbJson = JsonConvert.SerializeObject(airImportSubHawbs.SubHawbs);

                newHawbs.Add(newHawb);
            }

            airImportDetails.HawbList = newHawbs;
            airImportDetails.HawbListJson = JsonConvert.SerializeObject(newHawbs);

            return View(airImportDetails);
        }

        [HttpPost]
        public async Task<IActionResult> BatchPrintingAirImport(AirImportDetails model)
        {
            var data = await _airImportHawbAppService.GetHawbCardById(Guid.Parse(model.Hawb_Id));

            model.IsPDF = true;
            model.HawbList = JsonConvert.DeserializeObject<List<Hawb>>(model.HawbListJson);
            model.SubHawbs = JsonConvert.DeserializeObject<List<SubHawbs>>(model.HawbList.FirstOrDefault(f => f.Id == model.Hawb_Id)?.SubHawbJson);
            model.Hawb_No = data.HawbNo;

            return await _generatePdf.GetPdf("Views/Docs/BatchPrintingAirImport.cshtml", model);
        }
        public async Task<IActionResult> DeliveryOrderAirImportHawb(Guid id, FreightPageType pageType)
        {
            var airImportDetails = await GetAirImportDetailsByPageType(id, pageType);

            return View(airImportDetails);
        }
        public async Task<IActionResult> DeliveryOrderOceanImportMawb(Guid id)
        {
            DeliveryOrderIndexViewModel InfoViewModel = new DeliveryOrderIndexViewModel();

            //QueryHblDto queryHbl = new QueryHblDto();
            //queryHbl.Id = Guid.Parse(id);
            QueryContainerDto query = new QueryContainerDto() { QueryId = id, MaxResultCount = 1000 };
            var OceanExportMbl = await _oceanImportMblAppService.GetOceanImportDetailsById(id);
            var containers = await _containerAppService.QueryListAsync(query);

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030003/?hbl=47125&hide_mbl=false
            InfoViewModel.Office = OceanExportMbl.Office.AbbreviationName;
            InfoViewModel.Address = "";
            InfoViewModel.Tel = "+886-2-2545-9900#8671";
            InfoViewModel.Fax = "";
            InfoViewModel.Email = "it@dolphin-gp.com";
            InfoViewModel.FirstName = OceanExportMbl.MblCustomerName; /*"萬泰"*/;
            InfoViewModel.LastName = "" /*"資訊部"*/;
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");
            InfoViewModel.HBL_LcNo = OceanExportMbl.LCNo;
            InfoViewModel.trucker_area = "";
            InfoViewModel.FirstName = OceanExportMbl.MblOperatorName;
            InfoViewModel.empty_pickup_area = OceanExportMbl.EmptyPickupName;
            InfoViewModel.issue_at = OceanExportMbl.PostDate.ToString("dd-MM-yyyy");/* "05-09-2023";*/
            InfoViewModel.issue_by = OceanExportMbl.MblOperatorName;
            InfoViewModel.MBL_NO = OceanExportMbl.MblNo;
            InfoViewModel.carrier = OceanExportMbl.MblCarrierName;
            InfoViewModel.VESSEL_INFO = OceanExportMbl.VesselName + OceanExportMbl.Voyage;
            InfoViewModel.POR_location = OceanExportMbl.PorName;
            InfoViewModel.POR_location_ETD = OceanExportMbl.PorEtd?.ToString("dd-MM-yyyy");
            InfoViewModel.POL_location = OceanExportMbl.PolName;
            InfoViewModel.POL_location_ETD = OceanExportMbl.PolEtd?.ToString("dd-MM-yyyy");
            InfoViewModel.POD_location = OceanExportMbl.PodName;
            InfoViewModel.FdestName = OceanExportMbl.FdestName;
            InfoViewModel.POD_location_ETD = OceanExportMbl.PodEta?.ToString("dd-MM-yyyy") /*"04-03-2023"*/;
            InfoViewModel.total_packages_count = OceanExportMbl.TotalPackage.ToString();
            InfoViewModel.gross_weight_kgs = OceanExportMbl.TotalWeight.ToString();
            InfoViewModel.gross_weight_lbs = "0.00";
            InfoViewModel.measurement_cbm = OceanExportMbl.TotalMeasure.ToString();
            InfoViewModel.measurement_cft = "0.00";
            InfoViewModel.COMMODITY = OceanExportMbl.Commodity?.ToString();
            InfoViewModel.LcIssueBank = OceanExportMbl.LCIssueBankName;
            InfoViewModel.LcIssueDate = OceanExportMbl.LCIssueDate?.ToString("dd-MM-yyyy");
            InfoViewModel.carrier_bkg_no = OceanExportMbl.SoNo;
            InfoViewModel.delivery_to_area = OceanExportMbl.DeliveryToName;
            InfoViewModel.CyLocation = OceanExportMbl.CyLocation;
            InfoViewModel.delivery_to_date = OceanExportMbl.DelEta?.ToString("dd-MM-yyyy");
            InfoViewModel.billing_to_area = OceanExportMbl.MblBillToName + "\r\n" + OceanExportMbl.MblBillToContent; /*"HARD CORE TECHNOLOGY\r\n198 PEARSON GATEWAY APT. 555\r\nNORTH JAMES, KY 98809-9933\r\nWALNUT, CA 91789, UNITED STATES\r\nATTN: JENNIFER JIMENEZ TEL: 585.592.4848 FAX: 649-277-5122"*/;

            InfoViewModel.ContainerList = new List<DeliveryOrderContainerList>();
            foreach (var container in containers)
            {
                var con = new
                  DeliveryOrderContainerList
                {
                    PACKAGE = container.PackageNum.ToString(),
                    WEIGHT = container.PackageWeight + " " + container.PackageWeightUnit,
                    CONTAINER_NO = container.ContainerNo,
                    PICKUP_NO = container.PicupNo,
                    SEAL_NO = container.SealNo,
                    LFD = container.LastFreeDate.ToString(),

                };
                InfoViewModel.ContainerList.Add(con);
            };
            #endregion
            return View(InfoViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> DeliveryOrderAirImportHawb(AirImportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/DeliveryOrderAirImportHawb.cshtml", model);
        }

        public async Task<IActionResult> PreliminaryClaimAirImportHawb(Guid id, FreightPageType pageType)
        {
            var airImportDetails = await GetAirImportDetailsByPageType(id, pageType);

            return View(airImportDetails);
        }
        public async Task<IActionResult> PreliminaryClaimOceanExport(Guid id, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);
            var oceanExportMbl = await _oceanExportMblAppService.GetOceanExportDetailsById(oceanExportDetails.MblId);
            var container = await _containerAppService.GetContainerByHblId(id);
            oceanExportDetails.PackageMeasureName = container?.PackageMeasure.ToString();
            oceanExportDetails.PackageMeasureName = container?.PackageMeasureUnit == "CBM" ? container?.PackageMeasure + " CBM/" + (container?.PackageMeasure * 35.315) + " CFT" : container?.PackageMeasure * 0.0283 + " CBM/" + container?.PackageMeasure + " CFT";
            oceanExportDetails.PackageWeightName = container?.PackageWeightUnit == "KG" ? container?.PackageWeight + " KGS/" + (container?.PackageWeight * 2.20462) + " LBS" : container?.PackageWeight * 0.453592 + " KGS/" + (container?.PackageWeight) + " LBS";
            oceanExportDetails.TotalMeasure = (double)container?.PackageMeasure;
            oceanExportDetails.TotalWeight = (double)container?.PackageWeight;
            oceanExportDetails.TotalPackage = (int)container?.PackageNum;
            oceanExportDetails.VesselName = oceanExportMbl.VesselName;

            oceanExportDetails.Voyage = oceanExportMbl.Voyage;
            oceanExportDetails.DocNo = oceanExportMbl.DocNo;
            oceanExportDetails.PolName = oceanExportMbl.PolName;
            oceanExportDetails.PodName = oceanExportMbl.PodName;
            oceanExportDetails.PolEtd = oceanExportMbl.PolEtd;
            oceanExportDetails.PodEta = oceanExportMbl.PodEta;
            oceanExportDetails.ContainerNo = container.ContainerNo;
            return View(oceanExportDetails);
        }
        public async Task<IActionResult> PreliminaryClaimOceanImport(Guid id, FreightPageType pageType)
        {
            var oceanImportDetails = await GetOceanImportDetailsByPageType(id, pageType);
            var oceanExportMbl = await _oceanImportMblAppService.GetOceanImportDetailsById(oceanImportDetails.MblId);
            var container = await _containerAppService.GetContainerByHblId(id);
            oceanImportDetails.PackageMeasureName = container?.PackageMeasure.ToString();
            oceanImportDetails.PackageMeasureName = container?.PackageMeasureUnit == "CBM" ? container?.PackageMeasure + " CBM/" + (container?.PackageMeasure * 35.315) + " CFT" : container?.PackageMeasure * 0.0283 + " CBM/" + container?.PackageMeasure + " CFT";
            oceanImportDetails.PackageWeightName = container?.PackageWeightUnit == "KG" ? container?.PackageWeight + " KGS/" + (container?.PackageWeight * 2.20462) + " LBS" : container?.PackageWeight * 0.453592 + " KGS/" + (container?.PackageWeight) + " LBS";
            oceanImportDetails.TotalMeasure = container?.PackageMeasure != null ? (double)container?.PackageMeasure : 0;
            oceanImportDetails.TotalWeight = container?.PackageMeasure != null ? (double)container?.PackageWeight : 0;
            oceanImportDetails.TotalPackage = container?.PackageMeasure != null ? (int)container?.PackageNum : 0;
            oceanImportDetails.VesselName = oceanExportMbl.VesselName;

            oceanImportDetails.Voyage = oceanExportMbl.Voyage;
            oceanImportDetails.DocNo = oceanExportMbl.DocNo;
            oceanImportDetails.PolName = oceanExportMbl.PolName;
            oceanImportDetails.PodName = oceanExportMbl.PodName;
            oceanImportDetails.PolEtd = oceanExportMbl.PolEtd;
            oceanImportDetails.PodEta = oceanExportMbl.PodEta;
            oceanImportDetails.ContainerNo = container?.ContainerNo;
            return View(oceanImportDetails);
        }


        [HttpPost]
        public async Task<IActionResult> PreliminaryClaimAirImportHawb(AirImportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/PreliminaryClaimAirImportHawb.cshtml", model);
        }

        public async Task<IActionResult> CarrierCertificateAirImportHawb(Guid id, FreightPageType pageType)
        {
            var airImportDetails = await GetAirImportDetailsByPageType(id, pageType);

            return View(airImportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> CarrierCertificateAirImportHawb(AirImportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/CarrierCertificateAirImportHawb.cshtml", model);
        }
        public async Task<IActionResult> CarrierCertificateOceanImportHbl(Guid id, FreightPageType pageType)
        {
            var airImportDetails = await GetOceanImportDetailsByPageType(id, pageType);

            return View(airImportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> CarrierCertificateOceanImportHbl(OceanImportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/CarrierCertificateOceanImportHbl.cshtml", model);
        }
        public async Task<IActionResult> ReleaseOrderAirImportHawb(Guid id, FreightPageType pageType)
        {
            var airImportDetails = await GetAirImportDetailsByPageType(id, pageType);

            return View(airImportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ReleaseOrderAirImportHawb(AirImportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/ReleaseOrderAirImportHawb.cshtml", model);
        }
        public async Task<IActionResult> ReleaseOrderOceanImportHbl(Guid id, FreightPageType pageType)
        {
            var airImportDetails = await GetOceanImportDetailsByPageType(id, pageType);

            return View(airImportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ReleaseOrderOceanImportHbl(AirImportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/ReleaseOrderOceanImportHbl.cshtml", model);
        }


        public async Task<IActionResult> MBLPackageLabelOceanExportMBL(Guid id, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);

            return View(oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> MBLPackageLabelOceanExportMBL(OceanExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/MBLPackageLabelOceanExportMBL.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> ITTE(Guid id, FreightPageType pageType)
        {
            var airImportDetails = await GetAirImportDetailsByPageType(id, pageType);

            var iTTEViewModel = new ITTEViewModel()
            {
                Port = airImportDetails.ITIssuedLocation,
                EntryNo = airImportDetails.ITNo,
                ClassOfEntry = airImportDetails.ClassOfEntry,
                PortOfLoading = airImportDetails.DestinationAirportName,
                PortOf = airImportDetails.DestinationAirportName,
                CustomPort = airImportDetails.DestinationAirportName,
                ForeignPort = airImportDetails.DepatureName,
                AWBNo = airImportDetails.MawbNo,
                DateOfSailing = airImportDetails.DepatureDate?.ToShortDateString(),
                VesselOrCarrier = airImportDetails.CarrierTPName,
                DateImported = airImportDetails.ArrivalDate?.ToShortDateString(),
                ExportedOn = airImportDetails.DepatureDate?.ToShortDateString(),
                GoodsNowAt = airImportDetails.AwbAcctCarrierName,
                Consignee = airImportDetails.ConsigneeName,
                ForeignDestination = airImportDetails.FinalDestination,
                Package = Convert.ToString(airImportDetails.Package),
                WeightKG = (airImportDetails.GrossWeightKg + airImportDetails.ChargeableWeightKg),
                WeightLG = (airImportDetails.GrossWeightLb + airImportDetails.ChargeableWeightLb),
                MawbNo = airImportDetails.MawbNo,
                HawbNo = airImportDetails?.HawbNo,
            };

            return View(iTTEViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ITTE(ITTEViewModel model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/ITTE.cshtml", model);
        }

        public async Task<IActionResult> HBLPackingListOceanExport(Guid id, FreightPageType pageType, bool isPartialView = false)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);
            oceanExportDetails.IsPartialView = isPartialView;

            return View("Views/Docs/HBLPackingListOceanExport.cshtml", oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> HBLPackingListOceanExport(OceanExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/HBLPackingListOceanExport.cshtml", model);
        }

        public async Task<IActionResult> PrintMBLInstruction(Guid id, FreightPageType pageType, bool isPartialView = false)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);
            var containerName = _dropdownService.ContainerLookupList;

            QueryContainerDto query = new QueryContainerDto() { QueryId = id };
            var containers = await _containerAppService.QueryListAsync(query);

            var list = new List<CreateUpdateContainerDto>();

            double totalPackageWeight = 0;
            double totalPackageMeasure = 0;

            foreach (var item in containers)
            {
                var containerSizeName = string.Concat(containerName.Where(w => w.Value == string.Concat(item.ContainerSizeId)).Select(s => s.Text));

                var items = new CreateUpdateContainerDto
                {
                    ContainerNo = item.ContainerNo,
                    SealNo = item.SealNo,
                    ContainerSizeName = containerSizeName,
                    PackageWeight = item.PackageWeight,
                    PackageMeasure = item.PackageMeasure
                };

                totalPackageWeight += item.PackageWeight;
                totalPackageMeasure += item.PackageMeasure;

                list.Add(items);
            }

            oceanExportDetails.TotalWeightStr = string.Concat(totalPackageWeight + " KGS " + Math.Round(totalPackageWeight * 2.20462, 2) + " LBS");
            oceanExportDetails.TotalMeasureStr = string.Concat(totalPackageMeasure + " CBM " + Math.Round(totalPackageMeasure * 35.315, 2) + " CFT");
            oceanExportDetails.CreateUpdateContainerDtos = list;
            oceanExportDetails.CreateUpdateContainerDtosJson = JsonConvert.SerializeObject(list);
            oceanExportDetails.IsPartialView = isPartialView;

            return View("Views/Docs/PrintMBLInstruction.cshtml", oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> PrintMBLInstruction(OceanExportDetails model)
        {
            model.IsPDF = true;
            model.CreateUpdateContainerDtos = JsonConvert.DeserializeObject<List<CreateUpdateContainerDto>>(model.CreateUpdateContainerDtosJson);

            return await _generatePdf.GetPdf("Views/Docs/PrintMBLInstruction.cshtml", model);
        }

        public async Task<IActionResult> ForwarderCargoReceiptOceanExportHBL(Guid id, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);
            var packageName = _dropdownService.PackageUnitLookupList;
            var containers = await _containerAppService.QueryListHblAsync(id);

            var containerlists = new List<CreateUpdateContainerDto>();

            foreach (var item in containers)
            {
                var containerSizeName = string.Concat(packageName.Where(w => w.Value == string.Concat(item.PackageUnitId)).Select(s => s.Text));

                var container = new CreateUpdateContainerDto()
                {
                    PackageNum = item.PackageNum,
                    PackageUnitName = containerSizeName,
                    PackageWeightStr = string.Concat(item.PackageWeight) + " KGS",
                    PackageWeightStrLBS = string.Concat(Math.Round(item.PackageWeight * 2.204, 2)) + " LBS",
                    PackageMeasureStr = string.Concat(item.PackageMeasure) + " CBM",
                    PackageMeasureStrLBS = string.Concat(Math.Round(item.PackageMeasure * 35.315, 2)) + " CFT"
                };
                containerlists.Add(container);
            }

            oceanExportDetails.CreateUpdateContainer = containerlists;
            oceanExportDetails.CreateUpdateContainerJson = JsonConvert.SerializeObject(containerlists);

            return View(oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ForwarderCargoReceiptOceanExportHBL(OceanExportDetails model)
        {
            model.IsPDF = true;

            model.CreateUpdateContainer = JsonConvert.DeserializeObject<List<CreateUpdateContainerDto>>(model.CreateUpdateContainerJson);

            return await _generatePdf.GetPdf("Views/Docs/ForwarderCargoReceiptOceanExportHBL.cshtml", model);
        }
        public async Task<IActionResult> ForwarderCargoReceiptOceanImportHBL(Guid id, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanImportDetailsByPageType(id, pageType);
            var packageName = _dropdownService.PackageUnitLookupList;
            var containers = await _containerAppService.QueryListHblAsync(id);

            var containerlists = new List<CreateUpdateContainerDto>();

            foreach (var item in containers)
            {
                var containerSizeName = string.Concat(packageName.Where(w => w.Value == string.Concat(item.PackageUnitId)).Select(s => s.Text));

                var container = new CreateUpdateContainerDto()
                {
                    PackageNum = item.PackageNum,
                    PackageUnitName = containerSizeName,
                    PackageWeightStr = string.Concat(item.PackageWeight) + " KGS",
                    PackageWeightStrLBS = string.Concat(Math.Round(item.PackageWeight * 2.204, 2)) + " LBS",
                    PackageMeasureStr = string.Concat(item.PackageMeasure) + " CBM",
                    PackageMeasureStrLBS = string.Concat(Math.Round(item.PackageMeasure * 35.315, 2)) + " CFT"
                };
                containerlists.Add(container);
            }

            oceanExportDetails.CreateUpdateContainer = containerlists;
            oceanExportDetails.CreateUpdateContainerJson = JsonConvert.SerializeObject(containerlists);

            return View(oceanExportDetails);
        }
        public async Task<IActionResult> ExamHoldNoticeOceanExportHBL(Guid id, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);
            oceanExportDetails.HblOperatorName = _currentUser.Name + " " + _currentUser.SurName;

            return View(oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ExamHoldNoticeOceanExportHBL(OceanExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/ExamHoldNoticeOceanExportHBL.cshtml", model);
        }
        public async Task<IActionResult> ExamHoldNoticeOceanImportHBL(Guid id, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanImportDetailsByPageType(id, pageType);
            oceanExportDetails.HblOperatorName = _currentUser.Name + " " + _currentUser.SurName;

            return View(oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ExamHoldNoticeOceanImportHBL(OceanExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/ExamHoldNoticeOceanImportHBL.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> OceanExportProfitReport(Guid id, FreightPageType pageType, string reportType)
        {
            string returnUrl = string.Empty;

            QueryInvoiceDto queryDto = new QueryInvoiceDto();

            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);

            var containers = await _containerAppService.QueryListHblAsync(id);

            var list = new List<CreateUpdateContainerDto>();

            double totalPackageWeight = 0;
            double totalPackageMeasure = 0;

            foreach (var item in containers)
            {
                var items = new CreateUpdateContainerDto
                {
                    ContainerNo = item.ContainerNo,
                    SealNo = item.SealNo,
                    PackageWeight = item.PackageWeight,
                    PackageMeasure = item.PackageMeasure
                };

                totalPackageWeight += item.PackageWeight;
                totalPackageMeasure += item.PackageMeasure;

                list.Add(items);
            }

            var measurement = Convert.ToDouble(oceanExportDetails.TotalWeight) * 35.315;

            var profitReport = new ProfitReportViewModel()
            {
                AgentName = oceanExportDetails.HblAgentName,
                Consignee = oceanExportDetails.HblConsigneeName,
                Currency = "USD",
                Customer = oceanExportDetails.MblCustomerName,
                Operator = oceanExportDetails.MblOperatorName,
                Measurement = string.Concat(totalPackageMeasure + " CBM " + Math.Round(totalPackageMeasure * 35.315, 2) + " CFT"),
                PolEtd = string.Concat(oceanExportDetails.PolName, "/", oceanExportDetails.PolEtd).TrimStart('/'),
                PodEtd = string.Concat(oceanExportDetails.PodName, "/", oceanExportDetails.PodEta).TrimStart('/'),
                PorEtd = string.Concat(oceanExportDetails.PorName, "/", oceanExportDetails.PorEtd).TrimStart('/'),
                Del = string.Concat(oceanExportDetails.DelName, "/", oceanExportDetails.DelEta).TrimStart('/'),
                Sales = oceanExportDetails.MblSaleName,
                Shipper = oceanExportDetails.ShippingAgentName,
                MawbNo = oceanExportDetails.MblNo,
                ChargableWeight = string.Concat(totalPackageWeight + " KGS " + Math.Round(totalPackageWeight * 2.20462, 2) + " LBS"),
                PageType = pageType,
                ReportType = reportType,
                FileNo = oceanExportDetails.FilingNo,
                HawbNo = oceanExportDetails.HblNo,
                SvcTermToName = Convert.ToString(oceanExportDetails.SvcTermToName),
                SvcTermFromName = Convert.ToString(oceanExportDetails.SvcTermFromName),
                SalesType = oceanExportDetails.MblSalesTypeName,

            };

            if (pageType == FreightPageType.OEHBL)
            {

                // hbl invoices
                queryDto = new QueryInvoiceDto() { QueryType = 1, ParentId = id };

                profitReport.Invoices = await _invoiceAppService.QueryInvoicesAsync(queryDto);

                // Mbl invoices
                queryDto = new QueryInvoiceDto() { QueryType = 3, ParentId = oceanExportDetails.MblId };

                var mawbInvoices = await _invoiceAppService.QueryInvoicesAsync(queryDto);

                if (mawbInvoices != null && mawbInvoices.Any())
                {
                    profitReport.Invoices ??= new List<InvoiceDto>();

                    foreach (var mawbInvoice in mawbInvoices)
                    {
                        profitReport.Invoices.Add(mawbInvoice);
                    }
                }

            }
            else
            {
                queryDto = new QueryInvoiceDto() { QueryType = 3, ParentId = id };

                profitReport.Invoices = await _invoiceAppService.QueryInvoicesAsync(queryDto);
            }

            profitReport.AR = new List<InvoiceDto>();
            profitReport.AP = new List<InvoiceDto>();
            profitReport.DC = new List<InvoiceDto>();

            if (profitReport.Invoices != null && profitReport.Invoices.Count > 0)
            {
                foreach (var dto in profitReport.Invoices)
                {
                    switch (dto.InvoiceType)
                    {
                        default:
                            profitReport.AR.Add(dto);
                            break;
                        case 1:
                            profitReport.DC.Add(dto);
                            break;
                        case 2:
                            profitReport.AP.Add(dto);
                            break;
                    }
                }
                profitReport.InvoicesJson = JsonConvert.SerializeObject(profitReport.Invoices);
            }

            if (profitReport.AR.Any())
            {
                double arTotal = 0;
                foreach (var ar in profitReport.AR)
                {
                    arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                }
                profitReport.ARTotal = arTotal;
            }
            if (profitReport.AP.Any())
            {
                double apTotal = 0;
                foreach (var ap in profitReport.AP)
                {
                    apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                }

                profitReport.APTotal = apTotal;
            }
            if (profitReport.DC.Any())
            {
                double dcTotal = 0;
                foreach (var dc in profitReport.DC)
                {
                    dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                }
                profitReport.DCTotal = dcTotal;
            }

            profitReport.Total = profitReport.ARTotal - profitReport.APTotal + profitReport.DCTotal;

            returnUrl = pageType == FreightPageType.OIMBL
                ? (reportType == "Summary") ? "Views/Docs/MblProfitReportSummary.cshtml" : "Views/Docs/MblProfitReportDetailed.cshtml"
                : "Views/Docs/HblProfitReport.cshtml";

            return View(returnUrl, profitReport);
        }

        [HttpPost]
        public async Task<IActionResult> OceanExportProfitReport(ProfitReportViewModel model)
        {
            string returnUrl = model.PageType == FreightPageType.AIMBL
                ? (model.ReportType == "Summary") ? "Views/Docs/MblProfitReportSummary.cshtml" : "Views/Docs/MblProfitReportDetailed.cshtml"
                : "Views/Docs/HblProfitReport.cshtml";

            model.IsPDF = true;

            model.Invoices = JsonConvert.DeserializeObject<IList<InvoiceDto>>(model.InvoicesJson);

            return await _generatePdf.GetPdf(returnUrl, model);
        }

        public async Task<IActionResult> DangerousGoodsOceanExportHBL(Guid id, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);

            return View(oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> DangerousGoodsOceanExportHBL(OceanExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/DangerousGoodsOceanExportHBL.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> ManifestOceanExportMBLPartial(Guid mblId)
        {
            var data = await _oceanExportMblAppService.GetAsync(mblId);
            var tradePartners = _dropdownService.TradePartnerLookupList;

            OceanExportDetails oceanExportDetails = new();

            oceanExportDetails.MblNo = data.MblNo;
            oceanExportDetails.MblOverseaAgentName = string.Concat(tradePartners.Where(w => w.Value.Equals(Convert.ToString(data.MblOverseaAgentId))).Select(s => s.Text));

            return PartialView("Pages/Shared/_ManifestOceanExportMBL.cshtml", oceanExportDetails);
        }

        public async Task<IActionResult> ManifestOceanExportMBL(Guid mblId, string agent, FreightPageType pageType, bool isPartialView = false)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(mblId, pageType);
            var containerName = _dropdownService.ContainerLookupList;

            QueryContainerDto query = new QueryContainerDto() { QueryId = mblId };
            var containers = await _containerAppService.QueryListAsync(query);

            var list = new List<CreateUpdateContainerDto>();

            foreach (var item in containers)
            {
                var containerSizeName = string.Concat(containerName.Where(w => w.Value == string.Concat(item.ContainerSizeId)).Select(s => s.Text));

                var items = new CreateUpdateContainerDto
                {
                    ContainerNo = item.ContainerNo,
                    SealNo = item.SealNo,
                    ContainerSizeName = containerSizeName,
                    PackageNum = item.PackageNum,
                    PackageWeightStr = $"{item.PackageWeight} KGS",
                    PackageWeightStrLBS = $"{item.PackageWeight * 2.20462:F2} LBS",
                    PackageMeasureStr = $"{item.PackageMeasure} CBM",
                    PackageMeasureStrLBS = $"{item.PackageMeasure * 35.315:F2} CFT"
                };
                list.Add(items);
            }

            oceanExportDetails.CreateUpdateContainer = list;
            oceanExportDetails.CreateUpdateContainerJson = JsonConvert.SerializeObject(list);
            oceanExportDetails.IsPartialView = isPartialView;

            return View("Views/Docs/ManifestOceanExportMBL.cshtml",oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ManifestOceanExportMBL(OceanExportDetails model)
        {
            model.IsPDF = true;

            model.CreateUpdateContainer = JsonConvert.DeserializeObject<List<CreateUpdateContainerDto>>(model.CreateUpdateContainerJson);

            return await _generatePdf.GetPdf("Views/Docs/ManifestOceanExportMBL.cshtml", model);
        }

        public async Task<IActionResult> ManifestOceanExportContainer(Guid mblId, FreightPageType pageType, bool isPartialView = false)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(mblId, pageType);
            var containerName = _dropdownService.ContainerLookupList;

            QueryContainerDto query = new QueryContainerDto() { QueryId = mblId };
            var containers = await _containerAppService.QueryListAsync(query);

            var list = new List<CreateUpdateContainerDto>();

            foreach (var item in containers)
            {
                var containerSizeName = string.Concat(containerName.Where(w => w.Value == string.Concat(item.ContainerSizeId)).Select(s => s.Text));

                var items = new CreateUpdateContainerDto
                {
                    Id = item.Id,
                    ContainerNo = item.ContainerNo,
                    SealNo = item.SealNo,
                    ContainerSizeName = containerSizeName
                };
                list.Add(items);
            }

            var hblsList = await _oceanExportHblAppService.GetHblCardsById(mblId);

            var hbllists = new List<Hbl>();

            foreach (var item in hblsList)
            {
                var abc = item.Id;
                var hblcontainers = await _containerAppService.QueryListHblAsync(item.Id);
                var hblData = await GetOceanExportDetailsByPageType(item.Id, FreightPageType.OEHBL);
                var test = hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageWeight).FirstOrDefault();
                var weight = hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageWeight).FirstOrDefault();
                var res = Convert.ToDouble(weight) * 2.204;
                var measurement = hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageMeasure).FirstOrDefault();
                var ress = Convert.ToDouble(measurement) * 35.315;
                var containerid = Convert.ToString(hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageUnitId).FirstOrDefault());
                string commodityDesc = string.Empty;

                object commodities;
                item.ExtraProperties.TryGetValue("Commodities", out commodities);

                List<Commodity> commoditiesList = new List<Commodity>();

                if (commodities != null)
                {
                    commoditiesList = JsonConvert.DeserializeObject<List<Commodity>>(Convert.ToString(commodities));

                    if (commoditiesList != null && commoditiesList.Any())
                        commodityDesc = string.Join("\n", commoditiesList.Select(s => s.Description));
                }


                var Hbl = new Hbl
                {
                    HblNo = item.HblNo,
                    HblPcs = string.Concat(hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageNum)) + " " + string.Concat(containerName.Where(w => w.Value == containerid).Select(s => s.Text)),
                    Weight = weight + " KGS",
                    WeightLBS = res.ToString("0.00") + " LBS",
                    Measurement = measurement + " CBM",
                    MeasurementLBS = ress.ToString("0.00") + " CFT",
                    Shipper = hblData.ShippingAgentName,
                    Consignee = hblData.HblConsigneeName,
                    Mark = hblData.Mark,
                    Description = hblData.Description,
                    CommodityDesc = commodityDesc
                };


                hbllists.Add(Hbl);
            }

            oceanExportDetails.Hbls = hbllists;
            oceanExportDetails.HblsJson = JsonConvert.SerializeObject(hbllists);
            oceanExportDetails.CreateUpdateContainer = list;
            oceanExportDetails.CreateUpdateContainerJson = JsonConvert.SerializeObject(list);
            oceanExportDetails.IsPartialView = isPartialView;

            return View("Views/Docs/ManifestOceanExportContainer.cshtml", oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ManifestOceanExportContainer(OceanExportDetails models)
        {
            models.IsPDF = true;

            models.CreateUpdateContainer = JsonConvert.DeserializeObject<List<CreateUpdateContainerDto>>(models.CreateUpdateContainerJson);

            models.Hbls = JsonConvert.DeserializeObject<List<Hbl>>(models.HblsJson);

            return await _generatePdf.GetPdf("Views/Docs/ManifestOceanExportContainer.cshtml", models);
        }

        public async Task<IActionResult> ManifestOceanExportCarrier(Guid mblId, FreightPageType pageType)
        {
            var oceanExportDetails = await GetOceanExportDetailsByPageType(mblId, pageType);
            var containerName = _dropdownService.ContainerLookupList;

            QueryContainerDto query = new QueryContainerDto() { QueryId = mblId };
            var containers = await _containerAppService.QueryListAsync(query);

            var list = new List<CreateUpdateContainerDto>();

            foreach (var item in containers)
            {
                var containerSizeName = string.Concat(containerName.Where(w => w.Value == string.Concat(item.ContainerSizeId)).Select(s => s.Text));

                var items = new CreateUpdateContainerDto
                {
                    Id = item.Id,
                    ContainerNo = item.ContainerNo,
                    SealNo = item.SealNo,
                    ContainerSizeName = containerSizeName
                };
                list.Add(items);
            }
            var hblsList = await _oceanExportHblAppService.GetHblCardsById(mblId);

            var hbllists = new List<Hbl>();

            foreach (var item in hblsList)
            {
                var abc = item.Id;
                var hblcontainers = await _containerAppService.QueryListHblAsync(item.Id);
                var hblData = await GetOceanExportDetailsByPageType(item.Id, FreightPageType.OEHBL);
                var test = hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageWeight).FirstOrDefault();
                var weight = hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageWeight).FirstOrDefault();
                var res = Convert.ToDouble(weight) * 2.204;
                var measurement = hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageMeasure).FirstOrDefault();
                var ress = Convert.ToDouble(measurement) * 35.315;
                var containerid = Convert.ToString(hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageUnitId).FirstOrDefault());
                string commodityDesc = string.Empty;

                object commodities;
                item.ExtraProperties.TryGetValue("Commodities", out commodities);

                List<Commodity> commoditiesList = new List<Commodity>();

                if (commodities != null)
                {
                    commoditiesList = JsonConvert.DeserializeObject<List<Commodity>>(Convert.ToString(commodities));

                    if (commoditiesList != null && commoditiesList.Any())
                        commodityDesc = string.Join("\n", commoditiesList.Select(s => s.Description));
                }


                var Hbl = new Hbl
                {
                    HblNo = item.HblNo,
                    HblPcs = string.Concat(hblcontainers.Where(w => w.HblId == abc).Select(s => s.PackageNum)) + " " + string.Concat(containerName.Where(w => w.Value == containerid).Select(s => s.Text)),
                    Weight = weight + " KGS",
                    WeightLBS = res.ToString("0.00") + " LBS",
                    Measurement = measurement + " CBM",
                    MeasurementLBS = ress.ToString("0.00") + " CFT",
                    Shipper = hblData.ShippingAgentName,
                    Consignee = hblData.HblConsigneeName,
                    Mark = hblData.Mark,
                    Description = hblData.Description,
                    CommodityDesc = commodityDesc,
                    Remark = hblData.DomesticInstructions
                };


                hbllists.Add(Hbl);
            }

            oceanExportDetails.Hbls = hbllists;
            oceanExportDetails.HblsJson = JsonConvert.SerializeObject(hbllists);
            oceanExportDetails.CreateUpdateContainer = list;
            oceanExportDetails.CreateUpdateContainerJson = JsonConvert.SerializeObject(list);

            return View(oceanExportDetails);
        }
        [HttpPost]
        public async Task<IActionResult> ManifestOceanExportCarrier(OceanExportDetails model)
        {
            model.IsPDF = true;

            model.CreateUpdateContainer = JsonConvert.DeserializeObject<List<CreateUpdateContainerDto>>(model.CreateUpdateContainerJson);

            model.Hbls = JsonConvert.DeserializeObject<List<Hbl>>(model.HblsJson);

            return await _generatePdf.GetPdf("Views/Docs/ManifestOceanExportCarrier.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> HBLPrintOceanExport(Guid id, FreightPageType pageType, bool isPartialView = false)
        {

            var oceanExportPrintDetails = await GetOceanExportDetailsByPageType(id, pageType);

            QueryInvoiceDto queryDto = new QueryInvoiceDto();

            var containers = await _containerAppService.QueryListHblAsync(id);
            //var container = await _containerAppService.GetContainerByHblId(Guid.Parse(id));

            var list = new List<CreateUpdateContainerDto>();

            if (containers != null && containers.Any())
            {
                //int totalPKGs = 0;
                //double totalPackageWeight = 0;
                //double totalPackageMeasure = 0;
                string packageUnitName = _dropdownService.PackageUnitLookupList.Where(w => w.Value == Convert.ToString(containers[0].PackageUnitId)).FirstOrDefault().Text;
                foreach (var item in containers)
                {
                    var items = new CreateUpdateContainerDto
                    {
                        ContainerNo = item.ContainerNo,
                        PackageNum = item.PackageNum,
                        PackageWeight = item.PackageWeight,
                        PackageMeasure = item.PackageMeasure
                    };

                    //totalPackageWeight += item.PackageWeight;
                    //totalPackageMeasure += item.PackageMeasure;
                    //totalPKGs += item.PackageNum;

                    list.Add(items);
                }
                oceanExportPrintDetails.TotalWeight = containers[0].PackageWeight;
                oceanExportPrintDetails.TotalMeasure = containers[0].PackageMeasure;
                oceanExportPrintDetails.TotalPackage = containers[0].PackageNum;
                oceanExportPrintDetails.PackageWeightName = containers[0].PackageWeightUnit;
                oceanExportPrintDetails.PackageMeasureName = containers[0].PackageMeasureUnit;
                oceanExportPrintDetails.ContainerNo = containers[0].ContainerNo;
                oceanExportPrintDetails.Mark = containers[0].SealNo;
                oceanExportPrintDetails.PackageUnitName = packageUnitName;
            }

            oceanExportPrintDetails.DisplayUnit = "KGBM";
            oceanExportPrintDetails.IsPartialView = isPartialView;

            return View("Views/Docs/HBLPrintOceanExport.cshtml", oceanExportPrintDetails);

        }

        [HttpPost]
        public async Task<IActionResult> HBLPrintOceanExport(OceanExportDetails model)
        {
            model.IsPDF = true;

            return await _generatePdf.GetPdf("Views/Docs/HBLPrintOceanExport.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> HblShippingAdvice(Guid id, FreightPageType pageType)
        {
            var data = await GetOceanExportDetailsByPageType(id, pageType);
            var packageUnit = _dropdownService.PackageUnitLookupList;

            var containers = await _containerAppService.QueryListHblAsync(id)
;
            var list = new List<CreateUpdateContainerDto>();

            double totalPackageWeight = 0;
            double totalPackageMeasure = 0;
            double totalPackage = 0;

            foreach (var item in containers)
            {

                var items = new CreateUpdateContainerDto
                {
                    PackageNum = item.PackageNum,
                    ContainerNo = item.ContainerNo,
                    SealNo = item.SealNo,
                    PackageWeight = item.PackageWeight,
                    PackageMeasure = item.PackageMeasure,
                    PackageUnitName = item.PackageUnitId == null ? ""
                                        : string.Concat(packageUnit.Where(w => w.Value == string.Concat(item.PackageUnitId)).Select(s => s.Text))
                };

                totalPackage += item.PackageNum;
                totalPackageWeight += item.PackageWeight;
                totalPackageMeasure += item.PackageMeasure;

                list.Add(items);
            }

            data.TotalPackage = (int)totalPackage;
            data.PackageUnitName = list[0].PackageUnitName;
            data.TotalWeight = totalPackageWeight;
            data.TotalMeasure = totalPackageMeasure;
            data.TotalWeightStr = (Convert.ToDouble(data.TotalWeight) * 2.20).ToString("0.00");
            data.TotalMeasureStr = (Convert.ToDouble(data.TotalMeasure) * 35.31).ToString("0.00");
            data.CreateUpdateContainerDtos = list;
            data.CreateUpdateContainerDtosJson = JsonConvert.SerializeObject(list);

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> HblShippingAdvice(OceanExportDetails model)
        {
            model.IsPDF = true;
            model.CreateUpdateContainerDtos = JsonConvert.DeserializeObject<List<CreateUpdateContainerDto>>(model.CreateUpdateContainerDtosJson);

            return await _generatePdf.GetPdf("Views/Docs/HblShippingAdvice.cshtml", model);
        }

        public async Task<IActionResult> PackagingListOceanExportMBL(Guid id, FreightPageType pageType)
        {
            var hblLists = await _oceanExportHblAppService.GetHblCardsById(id);
            List<Commodity> commoditiesList = new List<Commodity>();

            foreach (var item in hblLists)
            {
                object commodities;
                item.ExtraProperties.TryGetValue("Commodities", out commodities);

                if (commodities is not null)
                {
                    foreach (var subItem in JsonConvert.DeserializeObject<List<Commodity>>(Convert.ToString(commodities)))
                    {
                        var commodity = new Commodity
                        {
                            BookingNo = item.SoNo,
                            PackagingType = subItem.PackagingType,
                            Description = subItem.Description,
                            HTSCode = subItem.HTSCode,
                            NoOfPcs = subItem.NoOfPcs,
                            NetWeight = subItem.NetWeight,
                            GrossWeight = subItem.GrossWeight,
                            UnitPrice = subItem.UnitPrice,
                            Amount = subItem.Amount
                        };
                        commoditiesList.Add(commodity);
                    }
                }
            }

            var oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);

            oceanExportDetails.Commodities = commoditiesList;
            oceanExportDetails.CommoditiesJson = JsonConvert.SerializeObject(commoditiesList);

            return View(oceanExportDetails);
        }
        [HttpPost]
        public IActionResult PackagingListOceanExportMBL(OceanExportDetails InfoModel)
        {
            try
            {
                var reportTemplatePath = Path.Combine("Views/Docs/Booking_packing_list.xlsx");
                if (System.IO.File.Exists(reportTemplatePath))
                {
                    var file = new FileInfo(reportTemplatePath);

                    using (var excel = new ExcelPackage(file))
                    {

                        var sheet = excel.Workbook.Worksheets.First();

                        sheet.Cells[5, 8].Value = InfoModel?.CurrentDate?.ToShortDateString() ?? "";
                        sheet.Cells[6, 2].Value = InfoModel?.PolName ?? "";
                        sheet.Cells[6, 8].Value = InfoModel?.PodName ?? "";

                        sheet.Cells[7, 7].Value = "Net Weight (" + (InfoModel?.Net_weight_unit ?? "") + ")";
                        sheet.Cells[7, 8].Value = "Gross Weight (" + (InfoModel?.Gross_weight_unit ?? "") + ")";

                        InfoModel.Commodities = JsonConvert.DeserializeObject<List<Commodity>>(InfoModel.CommoditiesJson);

                        var rowIndex = 8;
                        if (InfoModel?.Commodities != null && InfoModel.Commodities.Any())
                        {
                            foreach (var item in InfoModel.Commodities)
                            {
                                sheet.Cells[rowIndex, 1].Value = item.BookingNo ?? "";
                                sheet.Cells[rowIndex, 2].Value = item.PackagingType ?? "";
                                sheet.Cells[rowIndex, 3].Value = "";
                                sheet.Cells[rowIndex, 4].Value = item.Description ?? "";
                                sheet.Cells[rowIndex, 5].Value = item.HTSCode ?? "";
                                sheet.Cells[rowIndex, 6].Value = item.NoOfPcs ?? "";
                                var netweight = string.Empty;
                                if (item.NetWeight != null)
                                    netweight = InfoModel.Net_weight_unit == "LB" ? (double.Parse(item.NetWeight) * 2.20462).ToString("0.00") : item.NetWeight ?? "";
                                sheet.Cells[rowIndex, 7].Value = netweight;
                                var grossweight = string.Empty;
                                if (item.GrossWeight != null)
                                    grossweight = InfoModel.Gross_weight_unit == "LB" ? (double.Parse(item.GrossWeight) * 2.20462).ToString("0.00") : item.GrossWeight ?? "";
                                sheet.Cells[rowIndex, 8].Value = grossweight;
                                sheet.Cells[rowIndex, 9].Value = item.UnitPrice ?? "";
                                sheet.Cells[rowIndex, 10].Value = item.Amount ?? "";
                                sheet.Cells[rowIndex, 11].Value = item.Details ?? "";
                                rowIndex++;
                            }
                        }

                        sheet.Cells[rowIndex, 2].Value = InfoModel.TotalPackagesStr ?? "";
                        sheet.Cells[rowIndex, 6].Value = InfoModel.TotalPCSStr ?? "";
                        sheet.Cells[rowIndex, 7].Value = InfoModel.TotalNetWeightStr ?? "";
                        sheet.Cells[rowIndex, 8].Value = InfoModel.TotalGrossWeightStr ?? "";
                        sheet.Cells[rowIndex, 10].Value = InfoModel.TotalAmountStr ?? "";

                        sheet.Cells[8, 2, rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        sheet.Cells[8, 5, rowIndex, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        sheet.Cells[8, 1, rowIndex, 11].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sheet.Cells[8, 1, rowIndex, 11].Style.Font.Name = "Helvetica";
                        sheet.Cells[8, 1, rowIndex, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        sheet.Cells[8, 1, rowIndex, 11].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        sheet.Cells[8, 1, rowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        sheet.Cells[8, 1, rowIndex, 11].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        sheet.Cells[rowIndex, 1, rowIndex, 11].Style.Font.Bold = true;

                        var filecontent = excel.GetAsByteArray();

                        return File(filecontent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "booking_packing_list_" + DateTime.Now.ToString("MM-dd-yyyy") + ".xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            return Ok("File not found");
        }

        [HttpGet]
        public async Task<IActionResult> HBLPrintOceanExportOBL(Guid id, FreightPageType pageType)
        {
            var oceanExportPrintDetails = await GetOceanExportDetailsByPageType(id, pageType);
            var list = new List<CreateUpdateContainerDto>();
            var packageUnit = _dropdownService.PackageUnitLookupList;

            QueryInvoiceDto queryDto = new QueryInvoiceDto();

            QueryContainerDto query = new QueryContainerDto() { QueryId = oceanExportPrintDetails.MblId };
            var containers = await _containerAppService.QueryListAsync(query);

            if (containers != null && containers.Any())
            {
                string packageUnitName = "";

                int totalPKGs = 0;
                double totalPackageWeight = 0;
                double totalPackageMeasure = 0;

                foreach (var item in containers)
                {
                    if (item.PackageUnitId != null)
                    {
                        packageUnitName = _dropdownService.PackageUnitLookupList.Where(w => w.Value == Convert.ToString(containers[0].PackageUnitId)).FirstOrDefault().Text;
                    }
                    var items = new CreateUpdateContainerDto
                    {
                        ContainerNo = item.ContainerNo,
                        SealNo = item.SealNo,
                        PackageNum = item.PackageNum,
                        PackageWeight = item.PackageWeight,
                        PackageMeasure = item.PackageMeasure,
                        PackageUnitName = item.PackageUnitId == null ? ""
                                        : string.Concat(packageUnit.Where(w => w.Value == string.Concat(item.PackageUnitId)).Select(s => s.Text))
                    };

                    totalPackageWeight += item.PackageWeight;
                    totalPackageMeasure += item.PackageMeasure;
                    totalPKGs += item.PackageNum;

                    list.Add(items);
                }
                oceanExportPrintDetails.TotalWeight = totalPackageWeight;
                oceanExportPrintDetails.TotalMeasure = totalPackageMeasure;
                oceanExportPrintDetails.TotalPackage = totalPKGs;
                oceanExportPrintDetails.PackageWeightName = containers[0].PackageWeightUnit;
                oceanExportPrintDetails.PackageMeasureName = containers[0].PackageMeasureUnit;
                oceanExportPrintDetails.ContainerNo = containers[0].ContainerNo;
                oceanExportPrintDetails.Mark = containers[0].SealNo;
                oceanExportPrintDetails.PackageUnitName = list[0].PackageUnitName;
            }

            oceanExportPrintDetails.CreateUpdateContainer = list;
            oceanExportPrintDetails.CreateUpdateContainerJson = JsonConvert.SerializeObject(list);
            oceanExportPrintDetails.DisplayUnit = "KGBM";


            return View(oceanExportPrintDetails);
        }

        [HttpPost]
        public async Task<IActionResult> HBLPrintOceanExportOBL(OceanExportDetails model)
        {
            model.IsPDF = true;
            model.CreateUpdateContainer = JsonConvert.DeserializeObject<List<CreateUpdateContainerDto>>(model.CreateUpdateContainerJson);
            return await _generatePdf.GetPdf("Views/Docs/HBLPrintOceanExportOBL.cshtml", model);
        }

        [HttpGet]
        public IActionResult DocumentPackageHBLPopupPartial(Guid id)
        {
            OceanExportDetails oceanExportDetails = new OceanExportDetails() { HblId = id };

            return PartialView("Pages/Shared/_HBLDocumentPackagePopup.cshtml", oceanExportDetails);
        }

        [HttpGet]
        public async Task<IActionResult> HBLDocumentPackageOceanExport(Guid id, FreightPageType pageType, string reportType)
        {

            OceanExportDetails oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);
            oceanExportDetails.DDLItems = reportType.Split(',').ToList();
            oceanExportDetails.HblId = id;

            return View(oceanExportDetails);
        }

        [HttpGet]
        public async Task<IActionResult> GetDocPKGReportPartial(Guid id, FreightPageType pageType, string reportType)
        {
            switch (reportType)
            {
                case "HBLPrint":
                    return await HBLPrintOceanExport(id, pageType, true);
                case "CommercialInvoice":
                    return await CommercialInvoice(id.ToString());
                case "PackingList":
                    return await HBLPackingListOceanExport(id, pageType, true);
                case "CertificateOfOrigin":
                    return await CertificateOfOrigin(id.ToString());
                default:
                    return await BankDraft(id.ToString());
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> HBLDocumentPackageOceanExport(Guid id, FreightPageType pageType, string reportType)
        //{
           
        //    OceanExportDetails oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);
        //    oceanExportDetails.DDLItems = reportType.Split(',').ToList();
        //    oceanExportDetails.HblId = id;

        //    return View(oceanExportDetails);
        //}
        
        [HttpPost]
        public async Task<IActionResult> HBLDocumentPackageOceanExport(string reportType, OceanExportDetails model, CommercialInvoiceIndexViewModel InfoModel
            , CertificateOfOriginIndexViewModel certiModel, BankDraftIndexViewModel BDModel)
        {
            switch (reportType)
            {
                case "HBLPrint":
                    return await HBLPrintOceanExport(model);
                case "CommercialInvoice":
                    return await CommercialInvoice(InfoModel);
                case "PackingList":
                    return await HBLPackingListOceanExport(model);
                case "CertificateOfOrigin":
                    return await CertificateOfOrigin(certiModel);
                default:
                    return await BankDraft(BDModel);
            }
        }

        [HttpGet]
        public IActionResult DocumentPackageMBLPopupPartial(Guid id)
        {
            OceanExportDetails oceanExportDetails = new OceanExportDetails() {MblId = id };

            return PartialView("Pages/Shared/_MBLDocumentPackagePopup.cshtml", oceanExportDetails);
        }
        [HttpGet]
        public async Task<IActionResult> MBLDocumentPackageOceanExport(Guid id, FreightPageType pageType, string reportType, string displayBy)
        {
           
            OceanExportDetails oceanExportDetails = await GetOceanExportDetailsByPageType(id, pageType);

            QueryContainerDto query = new QueryContainerDto() { QueryId = id };
            var containers = await _containerAppService.QueryListAsync(query);
            var hblLists = await _oceanExportHblAppService.GetHblCardsById(id);
            var containersList = new List<CreateUpdateContainerDto>();
            var containerList = _dropdownService.ContainerLookupList;
            var listOfHBL = new List<Hbl>();

            if (containers.Any())
            {
                foreach(var item in containers)
                {
                    var container = new CreateUpdateContainerDto();
                    if(item.ContainerSizeId != null)
                        container.ContainerSizeName = string.Concat(containerList.Where(w => w.Value == Convert.ToString(containers[0].ContainerSizeId)).Select(s => s.Text));
                    if (item.ContainerNo != null)
                        container.ContainerNo = item.ContainerNo;
                    if (item.SealNo != null)
                        container.SealNo = item.SealNo;
                     
                    containersList.Add(container);
                }
            }

            foreach (var item in hblLists)
            {
                var hbl = new Hbl
                {
                    Id = string.Concat(item.Id),
                    HblNo = item.HblNo
                };
                listOfHBL.Add(hbl);      
            }

            var ddlList = new List<string>();
            foreach (var item in reportType.Split(','))
            {
                if(item == "MBL Export Manifest")
                {
                    foreach (var contr in containers)
                    {
                        ddlList.Add(item + " - " + contr.ContainerNo);
                    }
                }
                else if ( item == "Print HBL" || item == "Commercial Invoice")
                {
                    foreach (var hbl in listOfHBL)
                    {
                        ddlList.Add(item + " - " + hbl.HblNo);
                    }
                }
                else 
                {
                    ddlList.Add(item);
                }

            }

            

            oceanExportDetails.Hbls = listOfHBL;
            oceanExportDetails.HblsJson = JsonConvert.SerializeObject(listOfHBL);
            oceanExportDetails.DDLItems = ddlList;
            //oceanExportDetails.DDLItems = reportType.Split(',').ToList();
            oceanExportDetails.DisplayReportBy = displayBy;
            oceanExportDetails.MblId = id;
            oceanExportDetails.CreateUpdateContainer = containersList;
            oceanExportDetails.CreateUpdateContainerJson = JsonConvert.SerializeObject(containersList);

            return View(oceanExportDetails);
        }

        [HttpGet]
        public async Task<IActionResult> GetDocPKGReportPartialMBL(Guid id, FreightPageType pageType, string reportType, string displayBy)
        {
            //switch (reportType.Split('-')[0].TrimEnd())

            switch (reportType)
            {
                case "MBL Export Manifest":
                    if(displayBy == "Display by container")
                    {
                        return await ManifestOceanExportContainer(id,pageType,true);
                    }
                    else
                    {
                        return await ManifestOceanExportMBL(id,null,pageType,true);
                    }
                    
                case "Print HBL BLInstruction":
                    return await PrintMBLInstruction(id,pageType, true);
                case "Print HBL":
                  return await HBLPrintOceanExport(id,pageType,true);
                default:
                    return await CommercialInvoice(id.ToString());
            }
        }


        [HttpPost]
        public async Task<IActionResult> MBLDocumentPackageOceanExport(string reportType, string displayBy, string mblSizeQtyInfo, OceanExportDetails model, CommercialInvoiceIndexViewModel InfoModel)
        {
            switch (reportType)
            {
                case "MBL Export Manifest":
                    if(displayBy == "Container")
                    {
                        model.DisplayMblContainerSizeQtyInfo = mblSizeQtyInfo;
                        return await ManifestOceanExportContainer(model);
                    }
                    else
                    {
                        return await ManifestOceanExportMBL(model);
                    }
                    
                case "Print HBL BLInstruction":
                    return await PrintMBLInstruction(model);
                case "HBLPrint":
                  return await HBLPrintOceanExport(model);
                default:
                    return await CommercialInvoice(InfoModel);
            }
        }

        #region Private Functions

        private async Task<AirExportDetails> GetAirExportDetailsByPageType(Guid Id, FreightPageType pageType)
        {
            var data = new AirExportDetails();

            switch (pageType)
            {
                case FreightPageType.AEMBL:
                    data = await _airExportMawbAppService.GetAirExportDetailsById(Id);
                    break;
                case FreightPageType.AEHBL:
                    data = await _airExportHawbAppService.GetAirExportDetailsById(Id);
                    break;
                default:
                    break;
            }

            data.PageType = pageType;

            return data;
        }
        private async Task<AirImportDetails> GetAirImportDetailsByPageType(Guid Id, FreightPageType pageType)
        {
            var data = new AirImportDetails();

            switch (pageType)
            {
                case FreightPageType.AIMBL:
                    data = await _airImportMawbAppService.GetAirImportDetailsById(Id);
                    break;
                case FreightPageType.AIHBL:
                    data = await _airImportHawbAppService.GetAirImportDetailsById(Id);
                    break;
                default:
                    break;
            }

            data.PageType = pageType;

            return data;
        }
        private async Task<OceanExportDetails> GetOceanExportDetailsByPageType(Guid Id, FreightPageType pageType, bool isIncludeInvoices = false)
        {
            var data = new OceanExportDetails();
            QueryContainerDto query = new QueryContainerDto() { QueryId = Id, MaxResultCount = 1000 };
           var containers=new List<ContainerDto>();
            switch (pageType)
            {
                case FreightPageType.OEMBL:
                    data = await _oceanExportMblAppService.GetOceanExportDetailsById(Id);
                    break;
                case FreightPageType.OEHBL:
                    data = await _oceanExportHblAppService.GetOceanExportDetailsById(Id);
                    break;
                case FreightPageType.OIHBL:
                
                    break;
                default:
                    break;
            }
            data.ContainerList = new List<ImportExport.OceanExports.ContainerList>();
            foreach (var container in containers)
            {
                var con = new
                  ImportExport.OceanExports.ContainerList
                {
                    PACKAGE = container.PackageNum.ToString(),
                    WEIGHT = container.PackageWeight + " " + container.PackageWeightUnit,
                    CONTAINER_NO = container.ContainerNo,
                    PICKUP_NO = container.PicupNo,
                    SEAL_NO = container.SealNo,
                    LFD = container.LastFreeDate.ToString(),

                };
                data.ContainerList.Add(con);
            };
            if (data != null && isIncludeInvoices)
            {
                var queryType = pageType == FreightPageType.OEMBL ? 3 : 1;

                QueryInvoiceDto queryDto = new QueryInvoiceDto() { QueryType = queryType, ParentId = Id };

                data.Invoices = (await _invoiceAppService.QueryInvoicesAsync(queryDto)).ToList();

                if (data.Invoices != null && data.Invoices.Count > 0)
                {
                    data.AR = new List<InvoiceDto>();
                    data.DC = new List<InvoiceDto>();
                    data.AP = new List<InvoiceDto>();
                    foreach (var dto in data.Invoices)
                    {
                        switch (dto.InvoiceType)
                        {
                            default:
                                data.AR.Add(dto);
                                break;
                            case 1:
                                data.DC.Add(dto);
                                break;
                            case 2:
                                data.AP.Add(dto);
                                break;
                        }
                    }

                    if (data.AR.Any())
                    {
                        double arTotal = 0;
                        foreach (var ar in data.AR)
                        {
                            arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                        }
                        data.ARTotal = arTotal;
                    }
                    if (data.AP.Any())
                    {
                        double apTotal = 0;
                        foreach (var ap in data.AP)
                        {
                            apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                        }

                        data.APTotal = apTotal;
                    }
                    if (data.DC.Any())
                    {
                        double dcTotal = 0;
                        foreach (var dc in data.DC)
                        {
                            dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                        }
                        data.DCTotal = dcTotal;
                    }

                    data.Total = data.ARTotal - data.APTotal + data.DCTotal;

                    data.InvoicesJson = JsonConvert.SerializeObject(data.Invoices);
                }
            }

            data.PageType = pageType;

            if (string.IsNullOrEmpty(data.MblOperatorName)) data.MblOperatorName = string.Concat(CurrentUser.Name, " ", CurrentUser.SurName);

            return data;
        }

        private async Task<OceanImportDetails> GetOceanImportDetailsByPageType(Guid Id, FreightPageType pageType, bool isIncludeInvoices = false)
        {
            var data = new OceanImportDetails();
            QueryContainerDto query = new QueryContainerDto() { QueryId = Id, MaxResultCount = 1000 };
            var containers = new List<ContainerDto>();
            switch (pageType)
            {
                case FreightPageType.OEMBL:
                   
                    break;
                case FreightPageType.OEHBL:
                   
                    break;
                case FreightPageType.OIMBL:
                    data = await _oceanImportMblAppService.GetOceanImportDetailsById(Id);
                    break;
                case FreightPageType.OIHBL:
                    data = await _oceanImportHblAppService.GetOceanImportDetailsById(Id);
                    containers = await _containerAppService.QueryListHblAsync(Id);
                    break;
                default:
                    break;
            }
            data.ContainerList = new List<ImportExport.OceanImports.ContainerList>();
            foreach (var container in containers)
            {
                var con = new
                  ImportExport.OceanImports.ContainerList
                {
                    PACKAGE = container.PackageNum.ToString(),
                    WEIGHT = container.PackageWeight + " " + container.PackageWeightUnit,
                    CONTAINER_NO = container.ContainerNo,
                    PICKUP_NO = container.PicupNo,
                    SEAL_NO = container.SealNo,
                    LFD = container.LastFreeDate.ToString(),

                };
                data.ContainerList.Add(con);
            };
            if (data != null && isIncludeInvoices)
            {
                var queryType = pageType == FreightPageType.OIHBL ? 3 : 1;

                QueryInvoiceDto queryDto = new QueryInvoiceDto() { QueryType = queryType, ParentId = Id };

                data.Invoices = (await _invoiceAppService.QueryInvoicesAsync(queryDto)).ToList();

                if (data.Invoices != null && data.Invoices.Count > 0)
                {
                    data.AR = new List<InvoiceDto>();
                    data.DC = new List<InvoiceDto>();
                    data.AP = new List<InvoiceDto>();
                    foreach (var dto in data.Invoices)
                    {
                        switch (dto.InvoiceType)
                        {
                            default:
                                data.AR.Add(dto);
                                break;
                            case 1:
                                data.DC.Add(dto);
                                break;
                            case 2:
                                data.AP.Add(dto);
                                break;
                        }
                    }

                    if (data.AR.Any())
                    {
                        double arTotal = 0;
                        foreach (var ar in data.AR)
                        {
                            arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                        }
                        data.ARTotal = arTotal;
                    }
                    if (data.AP.Any())
                    {
                        double apTotal = 0;
                        foreach (var ap in data.AP)
                        {
                            apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                        }

                        data.APTotal = apTotal;
                    }
                    if (data.DC.Any())
                    {
                        double dcTotal = 0;
                        foreach (var dc in data.DC)
                        {
                            dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                        }
                        data.DCTotal = dcTotal;
                    }

                    data.Total = data.ARTotal - data.APTotal + data.DCTotal;

                    data.InvoicesJson = JsonConvert.SerializeObject(data.Invoices);
                }
            }

            data.PageType = pageType;

            if (string.IsNullOrEmpty(data.MblOperatorName)) data.MblOperatorName = string.Concat(CurrentUser.Name, " ", CurrentUser.SurName);

            return data;
        }
        //private async Task<OceanImportDetails> GetOceanImportDetailsByPageType(Guid Id, FreightPageType pageType, bool isIncludeInvoices = false)
        //{
        //    var data = new OceanImportDetails();

        //    switch (pageType)
        //    {
        //        case FreightPageType.OIMBL:
        //            data = await _oceanImportMblAppService.GetOceanImportDetailsById(Id);
        //            break;
        //        case FreightPageType.OIHBL:
        //            data = await _oceanImportHblAppService.GetOceanImportDetailsById(Id);
        //            break;
        //        default:
        //            break;
        //    }

        //    if (data != null && isIncludeInvoices)
        //    {
        //        var queryType = pageType == FreightPageType.OEMBL ? 3 : 1;

        //        QueryInvoiceDto queryDto = new QueryInvoiceDto() { QueryType = queryType, ParentId = Id };

        //        data.Invoices = (await _invoiceAppService.QueryInvoicesAsync(queryDto)).ToList();

        //        if (data.Invoices != null && data.Invoices.Count > 0)
        //        {
        //            data.AR = new List<InvoiceDto>();
        //            data.DC = new List<InvoiceDto>();
        //            data.AP = new List<InvoiceDto>();
        //            foreach (var dto in data.Invoices)
        //            {
        //                switch (dto.InvoiceType)
        //                {
        //                    default:
        //                        data.AR.Add(dto);
        //                        break;
        //                    case 1:
        //                        data.DC.Add(dto);
        //                        break;
        //                    case 2:
        //                        data.AP.Add(dto);
        //                        break;
        //                }
        //            }

        //            if (data.AR.Any())
        //            {
        //                double arTotal = 0;
        //                foreach (var ar in data.AR)
        //                {
        //                    arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
        //                }
        //                data.ARTotal = arTotal;
        //            }
        //            if (data.AP.Any())
        //            {
        //                double apTotal = 0;
        //                foreach (var ap in data.AP)
        //                {
        //                    apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
        //                }

        //                data.APTotal = apTotal;
        //            }
        //            if (data.DC.Any())
        //            {
        //                double dcTotal = 0;
        //                foreach (var dc in data.DC)
        //                {
        //                    dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
        //                }
        //                data.DCTotal = dcTotal;
        //            }

        //            data.Total = data.ARTotal - data.APTotal + data.DCTotal;

        //            data.InvoicesJson = JsonConvert.SerializeObject(data.Invoices);
        //        }
        //    }

        //    data.PageType = pageType;

        //    if (string.IsNullOrEmpty(data.MblOperatorName)) data.MblOperatorName = string.Concat(CurrentUser.Name, " ", CurrentUser.SurName);

        //    return data;
        //}


        private async Task<List<AllHawbList>> GetAllHawbLists(Guid mawbId)
        {
            var data = await _airExportHawbAppService.GetHblCardsById(mawbId);
            var allHawbLists = new List<AllHawbList>();

            foreach (var hawb in data)
            {
                var mawb = await _airExportMawbAppService.GetAsync(mawbId);
                var tradePartner = _dropdownService.TradePartnerLookupList;
                var portManagement = _dropdownService.PortsManagementLookupList;

                var allHawbs = new AllHawbList
                {
                    Id = string.Concat(hawb.Id),
                    Hawb_No = hawb.HawbNo,
                    Hawb_Pc = hawb.Package,
                    Weight_LB = hawb.GrossWeightShprLB,
                    Weight_KG = hawb.GrossWeightShprKG,
                    Shipper = string.Concat(tradePartner.Where(w => w.Value == hawb.ActualShippedr).Select(s => s.Text)),
                    Consignee = string.Concat(tradePartner.Where(w => w.Value == hawb.OverseaAgent).Select(s => s.Text)),
                    Carrier = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(mawb.MawbCarrierId)).Select(s => s.Text)),
                    Destination = string.Concat(portManagement.Where(s => s.Value == Convert.ToString(mawb.DestinationId)).Select(s => s.Text))
                };
                allHawbLists.Add(allHawbs);
            }

            return allHawbLists;
        }
        private async Task<List<AllHblList>> GetAllHblLists(Guid mblId)
        {
            var data = await _oceanExportHblAppService.GetHblCardsById(mblId);
            var allHblLists = new List<AllHblList>();

            foreach (var hbl in data)
            {
                var allHbls = new AllHblList
                {
                    Id = string.Concat(hbl.Id),
                    Hbl_No = hbl.HblNo
                };
                allHblLists.Add(allHbls);
            }

            return allHblLists;
        }
        private string GetViewUrlByPageType(FreightPageType pageType, string reportType)
        {
            string viewUrl = string.Empty;

            viewUrl = pageType == FreightPageType.AEMBL
                ? (reportType == "Summary") ? "Views/Docs/MawbProfitReport.cshtml" : "Views/Docs/MawbProfitReportDetailed.cshtml"
                : "Views/Docs/HawbProfitReport.cshtml";

            return viewUrl;
        }
        private async Task<List<AllHawbListAirImport>> GetAllHawbListsAirImport(Guid mawbId)
        {
            var data = await _airImportHawbAppService.GetHawbCardsByMawbId(mawbId);
            var allHawbLists = new List<AllHawbListAirImport>();

            foreach (var hawb in data)
            {
                var tradePartner = _dropdownService.TradePartnerLookupList;
                var packageUnit = _dropdownService.PackageUnitLookupList;

                var allHawbs = new AllHawbListAirImport
                {
                    HawbNo = hawb.HawbNo,
                    Packages = hawb.Package + " " + string.Concat(packageUnit.Where(w => w.Value == Convert.ToString(hawb.PackageUnit)).Select(s => s.Text)),
                    Chargeable_Weight = hawb.ChargeableWeightKG,
                    Customer = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.ConsigneeId)).Select(s => s.Text)),
                    Shipper = string.Concat(tradePartner.Where(w => w.Value == Convert.ToString(hawb.ShipperId)).Select(s => s.Text))
                };
                allHawbLists.Add(allHawbs);
            }

            return allHawbLists;
        }
        private async Task<AirImportMawbDto> GetAirImportMawbDetailsByPageType(Guid id, FreightPageType pageType)
        {
            var data = new AirImportMawbDto();

            switch (pageType)
            {
                case FreightPageType.AIMBL:
                    data = await _airImportMawbAppService.GetAirImportMawbDetailsById(id);
                    break;
                default:
                    break;
            }

            data.PageType = pageType;
            data.AllHawbListAirImports = await GetAllHawbListsAirImport(id);
            data.HawbJson = JsonConvert.SerializeObject(data.AllHawbListAirImports);

            return data;
        }

        #endregion
    }
} 
