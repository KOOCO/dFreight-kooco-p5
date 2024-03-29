using Dolphin.Freight.ImportExport.AirImports;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Web.Pages.AirImports;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Dolphin.Freight.ImportExport.Containers;

namespace Dolphin.Freight.Web.ViewModels.ImportExport
{
    public class HawbHblViewModel
    {
        public bool IsHblHaveInvoice { get; set; } = false;
        private OceanImportHblDto _oceanImportHbl;
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ShowMsg { get; set; }
        public string CabinateSize { get; set; }
        public int Quantity { get; set; }
        public int Index { get; set; }
        [BindProperty]
        public AirImportHawbDto HawbModel { get; set; }
        public OceanExportHblDto OceanExportHbl { get; set; }
        public List<SelectListItem> RateUnitTypeLookupList { get; set; }
        public List<SelectListItem> UnitTypeLookupList { get; set; }
        [BindProperty]
        public CreateUpdateExportBookingDto ExportBookingDto { get; set; }
        [BindProperty]
        public CreateUpdateContainerDto CreateUpdateContainerBooking { get; set; }
        public OceanImportHblDto OceanImportHbl { get { return _oceanImportHbl; } set
            {
                _oceanImportHbl = value;
                _oceanImportHbl.ShipTypeId = AirImports.ShipType.Normal;   } }
        public CreateUpdateOceanImportHblDto OceanImportHblDto { get; set; }
        
        public AirExportHawbDto AirExportHawbDto { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid Hid { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<AttachmentDto> FileList { get; set; }

        public OceanExportHblDto OceanExportHblDto { get; set; }
        public AirImportHawbDto AirImportHawbDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m0invoiceDtos { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m1invoiceDtos { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> m2invoiceDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> h0invoiceDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> h1invoiceDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<InvoiceDto> h2invoiceDtos { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> PortsManagementLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> CountryName { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }
        public List<SelectListItem> OtherList { get; set; }

        public virtual string GetFileSize(string filename)
        {
            string uploadsFolder = Path.Combine("mediaUpload", "AirImports", "DocCenter", Id.ToString());

            try
            {
                long bytes = new FileInfo(Path.Combine(uploadsFolder, filename)).Length;

                return string.Format("{0,2} MB", (bytes / 1024f) / 1024f);
            }
            catch (Exception)
            {
                return "";
            }
        }
        public string Title { get; set; }
        public string CurrentHawbNo { get; set; }

    }
}
