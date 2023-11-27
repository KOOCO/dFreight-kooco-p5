using AutoMapper.Internal.Mappers;
using Dolphin.Freight.ImportExport.OceanImports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Dolphin.Freight.ImportExport.Attachments;
using System.Collections.Generic;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.Common;

namespace Dolphin.Freight.Web.Pages.OceanImports
{
    public class EditModal4Model :  FreightPageModel
    {
        [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }
    [BindProperty(SupportsGet = true)]
    public Guid Hid { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ISToolTipShow { get; set; }
        [BindProperty]
    public CreateUpdateOceanImportMblDto OceanImportMbl { get; set; }
    [BindProperty]
    public CreateUpdateOceanImportHblDto OceanImportHbl { get; set; }
    [BindProperty(SupportsGet = true)]
     public int NewHbl { get; set; } = 0;
     public string CardClass { get; set; }
     public bool IsShowHbl { get; set; } = false;
     public IList<OceanImportHblDto> OceanImportHbls { get; set; }
     private readonly IOceanImportHblAppService _oceanImportHblAppService;
    private readonly IOceanImportMblAppService _oceanImportMblAppService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public readonly IAttachmentAppService _attachmentAppService;
    private readonly ISysCodeAppService _sysCodeAppService;

    public EditModal4Model(IOceanImportMblAppService oceanImportMblAppService, IOceanImportHblAppService oceanImportHblAppService, IWebHostEnvironment webHostEnvironment, IAttachmentAppService attachmentAppService, ISysCodeAppService sysCodeAppService)
    {
        _oceanImportMblAppService = oceanImportMblAppService;
        _oceanImportHblAppService = oceanImportHblAppService;
        _webHostEnvironment = webHostEnvironment;
        _attachmentAppService = attachmentAppService;
        _sysCodeAppService = sysCodeAppService;
    }
    public async Task OnGetAsync()
    {
            OceanImportMbl = await _oceanImportMblAppService.GetCreateUpdateOceanImportMblDtoById(Id);
            QueryHblDto query = new QueryHblDto() { MblId = Id };
            OceanImportHbls = await _oceanImportHblAppService.QueryListByMidAsync(query);
            ISToolTipShow = await _oceanImportMblAppService.GetCardSettings();
            QueryHblDto queryHbl = new QueryHblDto();
            if (Hid == Guid.Empty)
            {
                if (NewHbl == 1)
                {
                    OceanImportHbl = new CreateUpdateOceanImportHblDto();
                    QueryDto cquery = new QueryDto();
                    cquery.QueryType = "CardColorId";
                    var syscodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(cquery);
                    if (OceanImportHbls != null && OceanImportHbls.Count > 0)
                    {
                        int index = OceanImportHbls.Count % syscodes.Count;
                        OceanImportHbl.CardColorId = syscodes[index].Id;
                        OceanImportHbl.CardColorValue = syscodes[index].CodeValue;
                        CardClass = syscodes[index].CodeValue;
                    }
                    else
                    {
                        OceanImportHbl.CardColorId = syscodes[0].Id;
                        OceanImportHbl.CardColorValue = syscodes[0].CodeValue;
                        CardClass = syscodes[0].CodeValue;
                    }

                }
                else
                {
                    OceanImportHbl = new CreateUpdateOceanImportHblDto();
                    if (OceanImportHbls != null && OceanImportHbls.Count > 0)
                    {
                        OceanImportHbl = ObjectMapper.Map<OceanImportHblDto, CreateUpdateOceanImportHblDto>(OceanImportHbls[0]);
                        Hid = OceanImportHbl.Id;
                        IsShowHbl = true;
                    }
                }

            }
            else
            {
                queryHbl.Id = Hid;
                OceanImportHbl = await _oceanImportHblAppService.GetHblById(queryHbl);
                IsShowHbl = true;


            }
        }
    public async Task<IActionResult> OnPostAsync()
    {
        await _oceanImportMblAppService.UpdateAsync(Id, OceanImportMbl);
        await _oceanImportHblAppService.UpdateAsync(Hid, OceanImportHbl);
        return NoContent();
    }
        public async Task<IActionResult> OnPostMyUploader(IFormFile MyUploader,Guid fid,int ftype)
        {
            string fname = "";
            if (MyUploader != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
                if (!Directory.Exists(uploadsFolder))
                {
                    DirectoryInfo folder = Directory.CreateDirectory(uploadsFolder);
                }
                string filePath = Path.Combine(uploadsFolder, MyUploader.FileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    MyUploader.CopyTo(fileStream);
                }
                fname = MyUploader.FileName;
                CreateUpdateAttachmentDto dto = new CreateUpdateAttachmentDto() { FileName = fname, ShowName = fname, Ftype = ftype, Fid = fid, Size = MyUploader.Length / 1024 };
                await _attachmentAppService.CreateAsync(dto);
                return new ObjectResult(new { status = "success",fname = fname ,udate=DateTime.Now.ToString("yyyy-MM-dd"),size= MyUploader.Length/1024 });
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
