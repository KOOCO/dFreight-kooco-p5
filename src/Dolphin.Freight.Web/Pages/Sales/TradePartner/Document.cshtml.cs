using Dolphin.Freight.TradePartners;
using Dolphin.Freight.Web.Helpers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Users;
using static System.Net.WebRequestMethods;


namespace Dolphin.Freight.Web.Pages.Sales.TradePartner
{
    public class DocumentModel : AbpPageModel
    {
        // �������}�ǨӪ��Ѽ�
        [BindProperty(SupportsGet = true)]
        public string TPCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid TPId { get; set; }

        // �e���W������
        [BindProperty]
        public DocumentUploadViewModel DocumentUploadModel { get; set; }

        private readonly string _folder;
        private readonly ITradePartnerAttachmentAppService _tradePartnerAttachmentAppService;

        public DocumentModel(IWebHostEnvironment env, ITradePartnerAttachmentAppService tradePartnerAttachmentAppService)
        {
            // �W�ǥؿ��]��: wwwroot\mediaUpload\tradepartner\doc
            _folder = $@"{env.WebRootPath}\mediaUpload\tradepartner\doc";
            _tradePartnerAttachmentAppService = tradePartnerAttachmentAppService;
        }

        public void OnGet()
        {
            DocumentUploadModel = new DocumentUploadViewModel();
            DocumentUploadModel.TPId = TPId;
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            //try
            //{
            //    ValidateModel();

                // TODO: �P�_�ɦW�O�_������(�ثe�O�|�\�L)

                // �N�ɮ׼g�J���w���ɮצ�m
                if (DocumentUploadModel.FromFile != null && DocumentUploadModel.FromFile.Length > 0)
                {
                    var path = $@"{_folder}\{DocumentUploadModel.FromFile.FileName}";
                    using var stream = new FileStream(path, FileMode.Create);
                    await DocumentUploadModel.FromFile.CopyToAsync(stream);

                    // �N�ɮ׸�T�g�J��Ʈw
                    var dto = new CreateUpdateTradePartnerAttachmentDto();
                    dto.AttachmentName = DocumentUploadModel.FromFile.FileName;
                    dto.AttachmentSize = DocumentUploadModel.FromFile.Length;
                    dto.AttachmentUploadTime = DateTime.Now;
                    dto.TPId = TPId;
                    await _tradePartnerAttachmentAppService.CreateAsync(dto);

                    return NoContent();

                    //return new OkObjectResult(new
                    //{
                    //    name = DocumentUploadModel.FromFile.FileName, // �ɮצW��
                    //    size = DocumentUploadModel.FromFile.Length,
                    //    date = Clock.Now.ClearTime().ToString("yyyy-MM-dd HH:mm"),
                    //    tpId = TPId
                    //});
                }
                else
                {
                    return Page();
                }
            //}
            //catch (Exception ex)
            //{
            //    return Page();
            //}

        }

        #region DocumentUploadViewModel
        public class DocumentUploadViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            
            [Required]
            [StringLength(TradePartnerAttachmentConsts.MaxNameLength, MinimumLength = TradePartnerAttachmentConsts.MinNameLength)]
            public string AttachmentName { get; set; }

            [CanBeNull]
            [DataType(DataType.Upload)]
            [MaxFileSize(TradePartnerAttachmentConsts.MaxAttachmentFileSize)]
            public IFormFile FromFile { get; set; }

            [DataType(DataType.Date)]
            public DateTime AttachmentUploadTime { get; set; }

            public double AttachmentSize { get; set; }
            public Guid UserId { get; set; }
            public Guid TPId { get; set; }
        }
        #endregion
    }
}
