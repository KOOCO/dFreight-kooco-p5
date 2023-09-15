using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.ImportExport.OceanExports.ExportBookings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.OceanExports.ExportBookings
{
    public class Edit3Model : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty]
        public string SoNo { get; set; }
        private readonly IExportBookingAppService _exportBookingAppService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly IAttachmentAppService _attachmentAppService;
        public Edit3Model( IExportBookingAppService exportBookingAppService, IWebHostEnvironment webHostEnvironment, IAttachmentAppService attachmentAppService)
        {
            _exportBookingAppService = exportBookingAppService;
            _attachmentAppService = attachmentAppService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task OnGetAsync()
        {
            var exportBooking = await _exportBookingAppService.GetAsync(Id);
            SoNo = exportBooking.SoNo;
        }
        public async Task<IActionResult> OnPostMyUploader(List<IFormFile> MyUploader, Guid fid)
        {
            string fname = "";
            if (MyUploader != null)
            {
                foreach (var file in MyUploader)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        DirectoryInfo folder = Directory.CreateDirectory(uploadsFolder);
                    }
                    string filePath = Path.Combine(uploadsFolder, file.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    fname = file.FileName;
                    CreateUpdateAttachmentDto dto = new CreateUpdateAttachmentDto() { FileName = fname, ShowName = fname, Ftype = 8, Fid = fid, Size = file.Length / 1024 };
                    await _attachmentAppService.CreateAsync(dto);
                    
                }
                return new ObjectResult(new { status = "success", fname = fname, udate = DateTime.Now.ToString("yyyy-MM-dd"), size = 1024 });
            }
            return new ObjectResult(new { status = "fail" });

        }
        public async Task<IActionResult> OnGetDownload(string filename)
        {
            if (filename == null)
                return Content("filename is not availble");
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
            var path = Path.Combine(uploadsFolder, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        // Get content type
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
                {
                    {".txt", "text/plain"},
                    {".pdf", "application/pdf"},
                    {".doc", "application/vnd.ms-word"},
                    {".docx", "application/vnd.ms-word"},
                    {".xls", "application/vnd.ms-excel"},
                    {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                    {".png", "image/png"},
                    {".jpg", "image/jpeg"},
                    {".jpeg", "image/jpeg"},
                    {".gif", "image/gif"},
                    {".csv", "text/csv"}
                };
        }

    }
}
