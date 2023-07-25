using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using static Dolphin.Freight.Web.Pages.Sales.TradePartner.EditTradePartnerInfoModel;

namespace Dolphin.Freight.Web.Pages.Sales.TradePartner.DocCenter
{
    public class IndexModel : AbpPageModel
    {
        public Guid Id { get; set; }
        public List<AttachmentDto> FileList { get; set; }
        [BindProperty]
        public CreateTradePartnerInfoViewModel TPInfoModel { get; set; }
        private readonly int fileType = 10;
        private readonly string url = "/Sales/TradePartner/DocCenter/";

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAttachmentAppService _attachmentAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;

        public IndexModel(IWebHostEnvironment webHostEnvironment, IAttachmentAppService attachmentAppService,  ITradePartnerAppService tradePartnerAppService)
        {
            _webHostEnvironment = webHostEnvironment;
            _attachmentAppService = attachmentAppService;
            _tradePartnerAppService = tradePartnerAppService;
        }

        public virtual string GetFileSize(string filename)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload", "TradePartner", "DocCenter", Id.ToString());

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

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            Id = id;
            TradePartnerDto tradePartnerDto = await _tradePartnerAppService.GetTPAsync(id);
            QueryAttachmentDto dto = new() {
                QueryId = Id,
                QueryType = fileType,
            };

            FileList = await _attachmentAppService.QueryListAsync(dto);

            return Page();
        }

        //public async Task<IActionResult> OnPostUploader(IFormFile formFile, Guid id)
        //{
        //    if (formFile == null)
        //    {
        //        return Redirect(url + id);
        //    }

        //    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload", "TradePartner", "DocCenter", id.ToString());
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        DirectoryInfo folder = Directory.CreateDirectory(uploadsFolder);
        //    }

        //    string filePath = Path.Combine(uploadsFolder, formFile.FileName);
        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        formFile.CopyTo(fileStream);
        //    }

        //    string filename = formFile.FileName;
        //    CreateUpdateAttachmentDto dto = new CreateUpdateAttachmentDto() { 
        //        FileName = filename, 
        //        ShowName = filename, 
        //        Ftype = fileType, 
        //        Fid = id, 
        //        Size = formFile.Length / 1024,
        //    };

        //    await _attachmentAppService.CreateAsync(dto);
        //    return Redirect(url + id);
        //}
        public async Task<IActionResult> OnPostUploader(IFormFile Uploader, Guid id, int ftype)
        {
            string fname = "";
            if (Uploader != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload", "TradePartner", "DocCenter", id.ToString());
                if (!Directory.Exists(uploadsFolder))
                {
                    DirectoryInfo folder = Directory.CreateDirectory(uploadsFolder);
                }
                string filePath = Path.Combine(uploadsFolder, Uploader.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Uploader.CopyTo(fileStream);
                }
                fname = Uploader.FileName;
                CreateUpdateAttachmentDto dto = new CreateUpdateAttachmentDto() { FileName = fname, ShowName = fname, Ftype = ftype, Fid = id, Size = Uploader.Length / 1024 };
                await _attachmentAppService.CreateAsync(dto);
                return new ObjectResult(new { status = "success", fname = fname, udate = DateTime.Now.ToString("yyyy-MM-dd"), size = Uploader.Length / 1024 });
            }
            return new ObjectResult(new { status = "fail" });

        }
        public async Task<IActionResult> OnGetDownload(Guid id, string filename)
        {
            if (filename == null)
            {
                return Content("filename is not availble");
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload", "TradePartner", "DocCenter", id.ToString());
            var path = Path.Combine(uploadsFolder, filename);

            try
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch (Exception)
            {
                return new ObjectResult(new { status = "fail", message = "File Not Found" });
            }
         
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id, Guid fileId, string filename)
        {
            if (filename == null)
            {
                return Redirect(url + id);
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload", "TradePartner", "DocCenter", id.ToString());

            try
            {
                await _attachmentAppService.DeleteAsync(fileId);
                System.IO.File.Delete(Path.Combine(uploadsFolder, filename));
            }
            catch (IOException)
            {
                
            }

            return Redirect(url + id);
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
