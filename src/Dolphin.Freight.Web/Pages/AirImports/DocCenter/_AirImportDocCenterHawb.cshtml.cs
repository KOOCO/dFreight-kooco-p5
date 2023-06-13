using Dolphin.Freight.ImportExport.Attachments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Dolphin.Freight.Web.Pages.AirImports.DocCenter
{
    public class _AirIndexHawbModel : PageModel
    {
        private readonly int fileType = 10;
        private readonly string url = "/AirImports/DocCenter/";

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAttachmentAppService _attachmentAppService;
        public _AirIndexHawbModel(IWebHostEnvironment webHostEnvironment, IAttachmentAppService attachmentAppService)
        {
            _webHostEnvironment = webHostEnvironment;
            _attachmentAppService = attachmentAppService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostUploader(IFormFile formFile, Guid id, Guid mawbId)
        {
            if (formFile == null)
            {
                return Redirect(url + id);
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload", "AirImports", "DocCenter", id.ToString());
            if (!Directory.Exists(uploadsFolder))
            {
                DirectoryInfo folder = Directory.CreateDirectory(uploadsFolder);
            }

            string filePath = Path.Combine(uploadsFolder, formFile.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }

            string filename = formFile.FileName;
            CreateUpdateAttachmentDto dto = new CreateUpdateAttachmentDto()
            {
                FileName = filename,
                ShowName = filename,
                Ftype = fileType,
                Fid = id,
                Size = formFile.Length / 1024,
            };

            await _attachmentAppService.CreateAsync(dto);

            if(mawbId != Guid.Empty)
            {
                id = mawbId;
            }

            return Redirect(url + id);
        }
    }
}
