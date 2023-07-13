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
using Volo.Abp.Users;
using Wkhtmltopdf.NetCore;
using static Dolphin.Freight.Permissions.OceanExportPermissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Dolphin.Freight.Web.CommonService;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Accounting.InvoiceBills;

namespace Dolphin.Freight.Web.Controllers
{
    public class DocsController : AbpController
    {
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IGeneratePdf _generatePdf;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly IReportLogAppService _reportLogAppService;
        private readonly ICurrentUser _currentUser;
        private readonly IDropdownService _dropdownService;
        private readonly IExportBookingAppService _exportBookingAppService;
        private readonly IAirExportHawbAppService _airExportHawbAppService;
        private readonly IAirExportMawbAppService _airExportMawbAppService;
        private readonly IInvoiceAppService _invoiceAppService;

        private Dolphin.Freight.ReportLog.ReportLogDto ReportLog;
        public IList<OceanExportHblDto> OceanExportHbls { get; set; }
        public DocsController(IOceanExportMblAppService oceanExportMblAppService, IOceanExportHblAppService oceanExportHblAppService, ISysCodeAppService sysCodeAppService, IGeneratePdf generatePdf, IAjaxDropdownAppService ajaxDropdownAppService, IReportLogAppService reportLogAppService,
          ICurrentUser currentUser, IDropdownService dropdownService, IExportBookingAppService exportBookingAppService,
          IAirExportHawbAppService airExportHawbAppService,
          IAirExportMawbAppService airExportMawbAppService,
          IInvoiceAppService invoiceAppService)
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

            ReportLog = new ReportLog.ReportLogDto();
        }

        [HttpGet]
        public async Task<IActionResult> PreAlert(string id)
        {
            PreAlertIndexViewModel InfoViewModel = new PreAlertIndexViewModel();
            QueryHblDto queryHbl = new QueryHblDto();
            queryHbl.Id = Guid.Parse(id);
            var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

            #region
            //https://dlihq.gofreight.co/ocean/export/shipment/container/OE-23020004/?hbl=56
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

            OceanExportMblDto oceanExportMbl = TempData["PrintData"] is not null
                                                                                ? JsonConvert.DeserializeObject<OceanExportMblDto>(TempData["PrintData"].ToString())
                                                                                : await _oceanExportMblAppService.GetAsync(oceanExportMblId);

            if(oceanExportMbl.Id != oceanExportMblId && oceanExportMblId != Guid.Empty)
            {
                oceanExportMbl = await _oceanExportMblAppService.GetAsync(oceanExportMblId);
            }

            InfoViewModel.Office = oceanExportMbl.OfficeName;
            InfoViewModel.Address = "地址";
            InfoViewModel.Tel = "電話";
            InfoViewModel.Fax = "傳真";
            InfoViewModel.Email = _currentUser.Email;
            InfoViewModel.Email_U = _currentUser.Email.ToUpper();
            InfoViewModel.FirstName = _currentUser.Name;
            InfoViewModel.LastName = _currentUser.SurName;
            InfoViewModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InfoViewModel.Date = DateTime.Now.ToString("yyyy-MM-dd");
            InfoViewModel.Date_M = DateTime.Now.ToString("MMMM dd, yyyy", new CultureInfo("en-US"));
            InfoViewModel.To = Convert.ToString(oceanExportMbl.OfficeName);
            InfoViewModel.Attn = "EXPORT FREIGHT CASHER";
            InfoViewModel.From = "分站名稱";
            InfoViewModel.Vessel = Convert.ToString(oceanExportMbl.OfficeName);
            InfoViewModel.Voyage = Convert.ToString(oceanExportMbl.OfficeName);
            InfoViewModel.Mbl = Convert.ToString(oceanExportMbl.OfficeName);
            InfoViewModel.Pol = oceanExportMbl.PolName;
            InfoViewModel.Pod = oceanExportMbl.PodName;
            InfoViewModel.Cnee = oceanExportMbl.MblCarrierName;
            InfoViewModel.Consignee = Convert.ToString(oceanExportMbl.MblConsigneeId);

            InfoViewModel.TextArea = InfoViewModel.LastName;

            InfoViewModel.ReportId = oceanExportMbl.Id;

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

        [HttpGet]
        public async Task<IActionResult> Package(string target, string oldhblno, string oldto, string oldpieces, string olddestination)
        {
            var HblList = new List<PackageIndexViewModel>();

            if (TempData["PrintDataHBL"] != null && target != null)
            {
                HblList = JsonConvert.DeserializeObject<List<PackageIndexViewModel>>(TempData["PrintDataHBL"].ToString());
            }
            else
            {
                var OceanExportMbl = new CreateUpdateOceanExportMblDto();
                OceanExportMbl = JsonConvert.DeserializeObject<CreateUpdateOceanExportMblDto>(TempData["PrintData"].ToString());
                QueryHblDto query = new QueryHblDto() { MblId = OceanExportMbl.Id };
                IList<OceanExportHblDto> OceanExportHbls = await _oceanExportHblAppService.QueryListByMidAsync(query);

                if (OceanExportHbls != null && OceanExportHbls.Count > 1)
                {
                    for (int i = 0; i < OceanExportHbls.Count; i++)
                    {
                        HblList.Add(new PackageIndexViewModel()
                        {
                            Office = OceanExportHbls[i].OfficeName,
                            To = OceanExportHbls[i].AgentName,
                            MblNo = OceanExportMbl.MblNo,
                            HblNo = OceanExportHbls[i].HblNo,
                            Pieces = "1",
                            Destination = OceanExportHbls[i].PodName,
                            TotalPieces = "1",
                            ReportId = OceanExportHbls[i].Id
                        });
                    }
                }
                else
                {
                    HblList.Add(new PackageIndexViewModel()
                    {
                        Office = OceanExportHbls[0].OfficeName,
                        To = OceanExportHbls[0].AgentName,
                        MblNo = OceanExportMbl.MblNo,
                        HblNo = OceanExportHbls[0].HblNo,
                        Pieces = "1",
                        Destination = OceanExportHbls[0].PodName,
                        TotalPieces = "1",
                        ReportId = OceanExportHbls[0].Id
                    });
                }
                TempData["PrintData"] = JsonConvert.SerializeObject(OceanExportMbl);
            }

            if (oldto != null)
            {
                oldto = oldto.Replace("%0D%0A", "\r\n");
            }

            HblList.Where(x => x.HblNo == oldhblno && (x.To != oldto || x.Pieces != oldpieces || x.Destination != olddestination)).ToList()
            .ForEach(x =>
            {
                x.To = oldto;
                x.Pieces = oldpieces;
                x.Destination = olddestination;
                x.TotalPieces = oldpieces;
            });

            ViewBag.HblList = HblList.Select(x => x.HblNo).Select(x => new SelectListItem() { Text = x, Value = x, Selected = x == target }).ToList();

            var hbl = string.IsNullOrEmpty(target) ? HblList[0] : (HblList.Any(x => x.HblNo == target) ? HblList.First(x => x.HblNo == target) : HblList[0]);
            hbl.HblList = JsonConvert.SerializeObject(HblList);
            TempData["PrintDataHBL"] = JsonConvert.SerializeObject(HblList);
            return View(hbl);
        }

        [HttpPost]
        public async Task<IActionResult> Package(string hbllist, string hblno, string to, string pieces, string destination, string totalpieces)
        {

            var HblList = new List<PackageIndexViewModel>();
            HblList = JsonConvert.DeserializeObject<List<PackageIndexViewModel>>(hbllist);

            HblList.Where(x => x.HblNo == hblno && (x.To != to || x.Pieces != pieces || x.Destination != destination)).ToList()
            .ForEach(x =>
            {
                x.To = to;
                x.Pieces = pieces;
                x.Destination = destination;
                x.TotalPieces = totalpieces;
            });

            string Input = JsonConvert.SerializeObject(HblList);
            TempData["PrintDataHBL"] = JsonConvert.SerializeObject(HblList);

            ReportLog.ReportId = HblList[0].ReportId;
            ReportLog.ReportName = "Package";
            ReportLog.ReportData = Input;
            ReportLog.LastUpdateTime = DateTime.Now;
            await _reportLogAppService.UpdateReportLog(ReportLog);

            PackageIndexViewModel InfoModel = new PackageIndexViewModel();

            InfoModel.HblPDFList = new List<HblPDFListModel>();

            for (int i = 0; i < HblList.Count; i++)
            {
                InfoModel.HblPDFList.Add(new HblPDFListModel()
                {
                    Office = HblList[i].Office == null ? "" : HblList[i].Office,
                    To = HblList[i].To == null ? "" : HblList[i].To,
                    MblNo = HblList[i].MblNo == null ? "" : HblList[i].MblNo,
                    HblNo = HblList[i].HblNo == null ? "" : HblList[i].HblNo,
                    Pieces = HblList[i].Pieces == null ? "0" : HblList[i].Pieces,
                    Destination = HblList[i].Destination == null ? "" : HblList[i].Destination,
                    TotalPieces = HblList[i].TotalPieces == null ? "0" : HblList[i].TotalPieces
                });
            }

            return await _generatePdf.GetPdf("Views/Docs/Pdf/Package/Package.cshtml", InfoModel);
        }

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
            //InfoViewModel.ReportTitleChoice = "BC";
            //InfoViewModel.TradePartnerLayoutChoice = "default";
            InfoViewModel.CarrierBookingNo = "A1234";
            InfoViewModel.ActualShipper = "3891 DELTA PORT 809\r\nTSI DELTAPORT\r\n2 ROBERTS BANK\r\nDELTA, BC, CANADA";
            InfoViewModel.Consignee = "A CUSTOMS BROKERAGE, INC.\r\n5400 NW 84TH AVE\r\nDORAL, FL 33166, UNITED STATES";
            InfoViewModel.Shipping = "DOLPHIN LOGISTICS TAIPEI OFFICE\r\nC/O AER LINGUS LIMITED P.L.C.";
            InfoViewModel.OverseaAgent = "AER LINGUS\r\n6555 W. IMPERIAL HWY\r\nLOS ANGELES, CA 90045, UNITED STATES";
            InfoViewModel.HblNo = "OH-23010006";
            InfoViewModel.OutBookingNo = "DDD";
            InfoViewModel.BookingDate = "";
            InfoViewModel.ExportRefNo = "GGG";
            InfoViewModel.PoNo = "";
            InfoViewModel.ItnNo = "";
            InfoViewModel.Agent = "AER LINGUS";
            InfoViewModel.Notify = "123552133441";
            InfoViewModel.Vessel_Voyage = ". BELO GOA 444";
            InfoViewModel.Carrier = "3891 DELTA PORT 809";
            InfoViewModel.PlaceOfReceipt = "HERMITAGE, AR (UNITED STATES)";
            InfoViewModel.PortOfLoading = "BALLSTON LAKE, NY (UNITED STATES)";
            InfoViewModel.ETD = "01-12-2023";
            InfoViewModel.PortOfTransshipment = "ADAIR, OK (UNITED STATES)";
            InfoViewModel.TsETA = "03-14-2023";
            InfoViewModel.PortOfDischarge = "SOUTH OGDEN, UT (UNITED STATES)";
            InfoViewModel.PodETA = "02-21-2023";
            InfoViewModel.PlaceOfDelivery = "AMBIA, IN (UNITED STATES)";
            InfoViewModel.DelETA = "03-30-2023";
            InfoViewModel.FinalDestination = "ALDAN, PA (UNITED STATES)";
            InfoViewModel.FinalETA = "03-31-2023";
            InfoViewModel.MoveType = "BT/BT";
            InfoViewModel.EarlyReturn = "03-29-2023 00:00";
            InfoViewModel.Commodity = "測試";
            InfoViewModel.Container = "測試";
            InfoViewModel.Weight = "1";
            InfoViewModel.Dangerous = false;
            InfoViewModel.Measurement = "1";
            InfoViewModel.LC = true;
            InfoViewModel.PKG = "1";
            InfoViewModel.Stackable = true;
            InfoViewModel.CargoDeliveryLocation_1 = "AERO TRANSCOLOMBIANA DE CARGA";
            InfoViewModel.CargoDeliveryLocation_2 = "AERO TRANSCOLOMBIANA DE CARGA";
            InfoViewModel.Port_Cutoff_Date = "03-12-2023 00:00";
            InfoViewModel.Rail_Cutoff_Date = "03-10-2023 00:00";
            InfoViewModel.Warehouse_Cutoff_Date = "03-29-2023 00:00";
            InfoViewModel.Doc_Cutoff_Date = "03-11-2023 00:00";
            InfoViewModel.EmptyPickUp = "AERO EJECUTIVOS C.A.";
            InfoViewModel.CargoPickUp = "AEROCHAGO AIRLINES S.A.";
            InfoViewModel.Trucker = "ADDICTED DEALS, LLC";
            InfoViewModel.Remark = "";

            InfoViewModel.ReportId = OceanExportHbl.Id;

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
            var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

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
            InfoViewModel.ShipperName = "LONG BEACH CONTAINER PIER F";
            InfoViewModel.DrawnDocCredit = "\"DRAWN UNDER DOCUMENTARY CREDIT NO. : OOO";
            InfoViewModel.IssueDate = "04-18-2023";
            InfoViewModel.LCIssueBank = "PPP";
            InfoViewModel.ToLCIssueBank = "PPP";
            InfoViewModel.ToTradePartnerLoadFrom = "CONSIGNEE"; //NOTIFY
            InfoViewModel.Consignee = "GALLAGHER-CANNON";
            InfoViewModel.NotifyParty = "RAMIREZ-LEWIS";
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
            InfoViewModel.ReferToName = "HARD CORE TECHNOLOGY(GST/VAT)";
            InfoViewModel.ReferToAddress = "";
            InfoViewModel.ToActFullyOnOurBehalf = "";
            InfoViewModel.ToAssistNotToAlter = "";
            InfoViewModel.OtherInstructions = "";
            InfoViewModel.ReferQuestionTo = "";
            InfoViewModel.ReferQuestionName = "HARD CORE TECHNOLOGY(GST/VAT)";
            InfoViewModel.ReferQuestionPhone = "TEL: " + "08417606080";
            InfoViewModel.ReferQuestionShipperName = "LONG BEACH CONTAINER PIER F";
            InfoViewModel.ReferQuestionShipperPhone = "TEL: " + "(358)459-8430";
            InfoViewModel.ReferQuestionFreightForwarderName = "HARD CORE TECHNOLOGY(GST/VAT)";
            InfoViewModel.ReferQuestionFreightForwarderPhone = "TEL: " + "08417606080";
            InfoViewModel.ShipperName3 = "LONG BEACH CONTAINER PIER F";
            InfoViewModel.AuthorizedSignatureInput = "";
            InfoViewModel.UserCompany = "HANJUN LIN" + " / " + "HARD CORE TECHNOLOGY(GST/VAT)";

            InfoViewModel.ReportId = OceanExportHbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
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
            var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030002/?hbl=47120&hide_mbl=false
            InfoViewModel.SHIPPER_EXPORTER = "123" + Environment.NewLine + "3 FL., NO. 215, SEC. 1, FU XING S. RD., TAIPEI, TAIWAN" + Environment.NewLine + "TEL : 02-87721111" + Environment.NewLine + "FAX : 02-87732222" + Environment.NewLine + "TAIWAN";
            InfoViewModel.CONSIGNEE = "CHIYODA";
            InfoViewModel.NOTIFY = "1231231" + Environment.NewLine + "ATTN: SDSDSD";
            InfoViewModel.DOCUMENT_NO = "CMS/E/HPG118814";
            InfoViewModel.BL_NO = "SINHPH23030002";
            InfoViewModel.EXPORT_FILE_NO = "BOOKING # SINHPH23030002";
            InfoViewModel.FORWARDING_AGENT = "EVA AIRWAYS CORPORATION (BR)" + Environment.NewLine + "200 NORTH SEPULVEDA BLVD., SUITE 1600" + Environment.NewLine + "UNITED STATES";
            InfoViewModel.POINT_AND_COUNTRY_OF_ORIGIN = "";
            InfoViewModel.EXPORT_CARRIER = "XIN WEN ZHOU / 149E";
            InfoViewModel.PORT_OF_LOADING = "SINGAPORE (SINGAPORE)";
            InfoViewModel.PORT_OF_DISCHARGE = "HAIPHONG (VIETNAM)";
            InfoViewModel.PLACE_OF_DELIVERY = "";

            InfoViewModel.SHIPPING_MARKS = "CONTAINER NO./SEAL NO./P.O. NO." + Environment.NewLine + " /  / " + Environment.NewLine + Environment.NewLine + Environment.NewLine + "SHIEHN HAIPHONG PTE";
            InfoViewModel.QTY = "3 PALLET(S)";
            InfoViewModel.DESCRIPTION_OF_GOODS = "ELECTRONIC COMPONENT" + Environment.NewLine + "20 CARTONS";
            InfoViewModel.WEIGHT_G = "300.00 KGS" + Environment.NewLine + Environment.NewLine + "661.39 LBS";
            InfoViewModel.WEIGHT_C = "";
            InfoViewModel.MEASUREMENT = "4.50 CBM" + Environment.NewLine + Environment.NewLine + "158.92 CFT";
            InfoViewModel.Show_Container_Information = "true";
            InfoViewModel.CONTAINER_NO = "";
            InfoViewModel.TYPE = "";
            InfoViewModel.SEAL_NO = "";
            InfoViewModel.PKG = "3 PALLET(S)";
            InfoViewModel.KG_LB = "300.00 / 661.39";
            InfoViewModel.CBM_CFT = "4.50 / 158.92";
            InfoViewModel.bl_date = "03-27-2023";
            InfoViewModel.sworn_date = "MARCH 27, 2023";

            InfoViewModel.name_of_chamber = "REGIONAL";
            InfoViewModel.state_of_chamber = "CA";
            InfoViewModel.name_of_country = "UNITED STATES";

            InfoViewModel.ReportId = OceanExportHbl.Id;

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
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
            var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

            #region
            //https://eval-asia.gofreight.co/ocean/export/shipment/OEX-23030003/?hbl=47125&hide_mbl=false
            InfoViewModel.NameAndAddressOfExporte = "CHIYODA" + Environment.NewLine + "PODIUM BUILDING" + Environment.NewLine + "159883, SINGAPORE" + Environment.NewLine + "TEL: 62786666";
            InfoViewModel.NameAndAddressOfConsignee = "CHIYODA" + Environment.NewLine + "PODIUM BUILDING" + Environment.NewLine + "159883, SINGAPORE" + Environment.NewLine + "TEL: 62786666";
            InfoViewModel.SignatureOfExporter = "";
            InfoViewModel.DescriptionOfConsignment1 = "";
            InfoViewModel.DescriptionOfConsignment2 = "";

            InfoViewModel.ReportId = OceanExportHbl.Id;

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
        public IActionResult CommercialInvoice()
        {
            CommercialInvoiceIndexViewModel InfoViewModel = new CommercialInvoiceIndexViewModel();

            #region
            //https://dlihq.gofreight.co/ocean/export/shipment/container/OE-23020004/?hbl=56
            InfoViewModel.shipper_area = "123" + Environment.NewLine + "3 FL., NO. 215, SEC. 1, FU XING S. RD., TAIPEI, TAIWAN" + Environment.NewLine + "TEL : 02-87721111" + Environment.NewLine + "FAX : 02-87732222" + Environment.NewLine + "TAIWAN";
            InfoViewModel.consignee_area = "CHIYODA";
            InfoViewModel.notify_area = "1231231" + Environment.NewLine + "ATTN: SDSDSD";
            InfoViewModel.invoice_no_and_date = "";
            InfoViewModel.LC_NO = "";
            InfoViewModel.LC_issue_bank = "";
            InfoViewModel.ETD_date = "03-27-2023";
            InfoViewModel.POR_location = "";
            InfoViewModel.POL_location = "SINGAPORE (SINGAPORE)";
            InfoViewModel.FDEST_location = "";
            InfoViewModel.vessel_voyage = "XIN WEN ZHOU / 149E";
            InfoViewModel.freight_term = "FREIGHT PREPAID";

            InfoViewModel.shipping_marks = "CONTAINER NO./SEAL NO./P.O. NO." + Environment.NewLine + " /  / " + Environment.NewLine + Environment.NewLine + Environment.NewLine + "SHIEHN HAIPHONG PTE";
            InfoViewModel.packages = "3 PALLET(S)";
            InfoViewModel.good_items = "ELECTRONIC COMPONENT" + Environment.NewLine + "20 CARTONS";
            InfoViewModel.unit_price = "";
            InfoViewModel.total_amount = "";
            InfoViewModel.shipper_name = "123";

            //string Input = JsonConvert.SerializeObject(InfoViewModel);
            #endregion

            return View(InfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CommercialInvoice(CommercialInvoiceIndexViewModel InfoModel)
        {
            InfoModel.BaseUrl = string.Format("{0}://{1}/", HttpContext.Request.Scheme, HttpContext.Request.Host);

            return await _generatePdf.GetPdf("Views/Docs/Pdf/CommercialInvoice/Default.cshtml", InfoModel);
        }

        [HttpGet]
        public async Task<IActionResult> PickupDeliveryOrder(/*string id*/)
        {
            PickupDeliveryOrderIndexViewModel InfoViewModel = new PickupDeliveryOrderIndexViewModel();

            //QueryHblDto queryHbl = new QueryHblDto();
            //queryHbl.Id = Guid.Parse(id);
            //var OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);

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

            InfoViewModel.trucker_area = "";
            InfoViewModel.empty_pickup_area = "NIPPON CARGO AIRLINES";
            InfoViewModel.issue_at = "05-09-2023";
            InfoViewModel.issue_by = "HANJUN LIN";
            InfoViewModel.MBL_NO = "OEX-23030003";
            InfoViewModel.carrier = "ALPINE AVIATION INC.";
            InfoViewModel.VESSEL_INFO = "ASIATIC BAY N097";
            InfoViewModel.POL_location = "SINGAPORE (SINGAPORE)";
            InfoViewModel.POL_location_ETD = "03-30-2023";
            InfoViewModel.POD_location = "BANGKOK (THAILAND)";
            InfoViewModel.POD_location_ETD = "04-03-2023";
            InfoViewModel.total_packages_count = "6";
            InfoViewModel.gross_weight_kgs = "1,000.00";
            InfoViewModel.gross_weight_lbs = "0.00";
            InfoViewModel.measurement_cbm = "4.50";
            InfoViewModel.measurement_cft = "0.00";
            InfoViewModel.COMMODITY = "ELECTRONIC COMPONENT";
            InfoViewModel.billing_to_area = "HARD CORE TECHNOLOGY\r\n198 PEARSON GATEWAY APT. 555\r\nNORTH JAMES, KY 98809-9933\r\nWALNUT, CA 91789, UNITED STATES\r\nATTN: JENNIFER JIMENEZ TEL: 585.592.4848 FAX: 649-277-5122";
            InfoViewModel.ContainerList = new List<PickupDeliveryOrderContainerList>()
            {
                new
                PickupDeliveryOrderContainerList
                {
                    PACKAGE = "6 PALLET(S)",
                    WEIGHT = "1,000.00 KGS / 0.00 LBS"
                }
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
        public async Task<IActionResult> HawbProfitReport(Guid hawbId)
        {
            var hawb = await _airExportHawbAppService.GetHawbWithDetailsById(hawbId);

            var mawb = await _airExportMawbAppService.GetAsync(hawb.MawbId.GetValueOrDefault());

            var measurement = Convert.ToDouble(hawb.ChargeableWeightCneeLB) * 35.315;

            var hawbProfitReport = new HawbProfitReportViewModel()
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

            if(hawbProfitReport.AR.Any())
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
    }
}
