using Dolphin.Freight.ImportExport.AirImports;
﻿using Dolphin.Freight.ImportExport.AirImports;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Web.Pages.AirImports;
using Dolphin.Freight.ImportExport.Attachments;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Dolphin.Freight.Web.ViewModels.ImportExport
{
    public class HawbHblViewModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ShowMsg { get; set; }
        [BindProperty]
        public AirImportHawbDto HawbModel { get; set; }
        public OceanExportHblDto OceanExportHbl { get; set; }
        public OceanImportHblDto OceanImportHbl { get; set; }
        public AirExportHawbDto AirExportHawbDto { get; set; }
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
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> AirportLookupList { get; set; }
        public List<SelectListItem> PackageUnitLookupList { get; set; }
        public List<SelectListItem> WtValOtherList { get; set; }
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
    }
}
