using AutoMapper.Internal.Mappers;
using Dolphin.Freight.ImportExport.OceanExports;
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
using System.Linq;

namespace Dolphin.Freight.Web.Pages.OceanExports
{
    public class EditModal4Model :  FreightPageModel
    {
        [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }
    [BindProperty(SupportsGet = true)]
    public Guid Hid { get; set; }
    [BindProperty]
    public CreateUpdateOceanExportMblDto OceanExportMbl { get; set; }
    [BindProperty]
    public CreateUpdateOceanExportHblDto OceanExportHbl { get; set; }
    [BindProperty(SupportsGet = true)]
     public int NewHbl { get; set; } = 0;
     public string CardClass { get; set; }
     public bool IsShowHbl { get; set; } = false;
     public IList<OceanExportHblDto> OceanExportHbls { get; set; }
     private readonly IOceanExportHblAppService _oceanExportHblAppService;
    private readonly IOceanExportMblAppService _oceanExportMblAppService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public readonly IAttachmentAppService _attachmentAppService;
    private readonly ISysCodeAppService _sysCodeAppService;

    public EditModal4Model(IOceanExportMblAppService oceanExportMblAppService, IOceanExportHblAppService oceanExportHblAppService, IWebHostEnvironment webHostEnvironment, IAttachmentAppService attachmentAppService, ISysCodeAppService sysCodeAppService)
    {
        _oceanExportMblAppService = oceanExportMblAppService;
        _oceanExportHblAppService = oceanExportHblAppService;
        _webHostEnvironment = webHostEnvironment;
        _attachmentAppService = attachmentAppService;
        _sysCodeAppService = sysCodeAppService;
    }
    public async Task OnGetAsync()
    {
            OceanExportMbl = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(Id);
            QueryHblDto query = new QueryHblDto() { MblId = Id };
            OceanExportHbls = await _oceanExportHblAppService.QueryListByMidAsync(query);
            OceanExportHbls = OceanExportHbls.Reverse().ToList();
            QueryHblDto queryHbl = new QueryHblDto();
            if (Hid == Guid.Empty)
            {
                if (NewHbl == 1)
                {
                    OceanExportHbl = new CreateUpdateOceanExportHblDto();
                    QueryDto cquery = new QueryDto();
                    cquery.QueryType = "CardColorId";
                    var syscodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(cquery);
                    if (OceanExportHbls != null && OceanExportHbls.Count > 0)
                    {
                        int index = OceanExportHbls.Count % syscodes.Count;
                        OceanExportHbl.CardColorId = syscodes[index].Id;
                        OceanExportHbl.CardColorValue = syscodes[index].CodeValue;
                        CardClass = syscodes[index].CodeValue;
                    }
                    else
                    {
                        OceanExportHbl.CardColorId = syscodes[0].Id;
                        OceanExportHbl.CardColorValue = syscodes[0].CodeValue;
                        CardClass = syscodes[0].CodeValue;
                    }

                }
                else
                {
                    OceanExportHbl = new CreateUpdateOceanExportHblDto();
                    if (OceanExportHbls != null && OceanExportHbls.Count > 0)
                    {
                        OceanExportHbl = ObjectMapper.Map<OceanExportHblDto, CreateUpdateOceanExportHblDto>(OceanExportHbls[0]);
                        OceanExportHbl.CardColorValue = "0";
                        Hid = OceanExportHbl.Id;
                        IsShowHbl = true;
                    }
                }

            }
            else
            {
                queryHbl.Id = Hid;

                int index = OceanExportHbls.IndexOf(OceanExportHbls.FirstOrDefault(x => x.Id == Hid));
                OceanExportHbl = ObjectMapper.Map<OceanExportHblDto, CreateUpdateOceanExportHblDto>(OceanExportHbls.Where(x => x.Id == Hid).FirstOrDefault());
                OceanExportHbl.CardColorValue = index.ToString();
                IsShowHbl = true;


            }
        }
    public async Task<IActionResult> OnPostAsync()
    {
        await _oceanExportMblAppService.UpdateAsync(Id, OceanExportMbl);
        await _oceanExportHblAppService.UpdateAsync(Hid, OceanExportHbl);
        return NoContent();
    }
        public async Task<IActionResult> OnPostMyUploader(List<IFormFile> MyUploader,Guid fid,int ftype)
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
                    CreateUpdateAttachmentDto dto = new CreateUpdateAttachmentDto() { FileName = fname, ShowName = fname, Ftype = ftype, Fid = fid, Size = file.Length / 1024 };
                    await _attachmentAppService.CreateAsync(dto);
                }
                    return new ObjectResult(new { status = "success",fname = fname ,udate=DateTime.Now.ToString("yyyy-MM-dd"),size=1024 });
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
