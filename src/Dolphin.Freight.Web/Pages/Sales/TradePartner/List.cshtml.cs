using Dolphin.Freight.ImportExport.Attachments;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.Web.Pages.Sales.TradePartner
{
    public class ListModel : PageModel
    {
        public List<SelectListItem> AccountGroupNameLookupList { get; set; }
        public List<SelectListItem> CountryLookupList { get; set; }
             public List<SelectListItem> TradePartnerList { get; set; }
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IAccountGroupAppService _accountGroupAppService;
        private readonly ITradePartnerRepository _tradePartnerRepository;
        private readonly ITradePartnerMemoAppService _tradePartnerMemoAppService;
        private readonly IAttachmentAppService _attachmentAppService;

        public string Search { get; set; }
        public ListModel(IAccountGroupAppService accountGroupAppService, ITradePartnerAppService tradePartnerAppService, 
            ITradePartnerRepository tradePartnerRepository, ITradePartnerMemoAppService tradePartnerMemoAppService,
            IAttachmentAppService attachmentAppService) { 
        _accountGroupAppService= accountGroupAppService;
            _tradePartnerAppService= tradePartnerAppService;
            _tradePartnerRepository= tradePartnerRepository;
            _tradePartnerMemoAppService= tradePartnerMemoAppService;
            _attachmentAppService= attachmentAppService;
        }
        public async Task OnGetAsync()
        {
            var accountGroupNameLookup = await _accountGroupAppService.GetAccountGroupNameLookupAsync();
            AccountGroupNameLookupList = accountGroupNameLookup.Items
                                                .Select(x => new SelectListItem(x.AccountGroupName, x.Id.ToString(), false))
                                                .ToList<SelectListItem>();
            var countryNameLookup = await _tradePartnerAppService.GetCountriesLookupAsync();
            CountryLookupList = countryNameLookup.Items
                                                .Select(x => new SelectListItem(x.ShowName + " " + x.CountryName, x.Id.ToString(), false))
                                                .ToList();
            var tradePartnerList = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            TradePartnerList = tradePartnerList.Items
                                                .Select(x => new SelectListItem(x.TPName + " " + x.TPCode, x.Id.ToString(), false))
                                                .ToList();
        }
        public async Task<JsonResult> OnPostAssignGroup(List<Guid> ids, Guid groupId)
        {

            foreach (var id in ids)
            {
             var result=   await _tradePartnerRepository.GetAsync(id);
            result.AccountGroupId = groupId;
                await _tradePartnerRepository.UpdateAsync(result);
            
            }






            Dictionary<string, Guid> rs = new()
            {
                { "id", groupId }
            };
            return new JsonResult(rs);
        }
        public async Task<JsonResult> OnPostDisableTradePatner(List<Guid> ids)
        {

            foreach (var id in ids)
            {
                var result = await _tradePartnerRepository.GetAsync(id);
                result.IsActive = false;
                await _tradePartnerRepository.UpdateAsync(result);

            }






            Dictionary<string, Guid> rs = new()
            {
                { "id", ids[0] }
            };
            return new JsonResult(rs);
        }
        public async Task<JsonResult> OnPostMargeTradePatner(Guid fromId,Guid toId,bool Cp,bool Tp,bool Mm,bool Md)
        {

            var from = await _tradePartnerRepository.GetAsync(fromId);

            var to = await _tradePartnerRepository.GetAsync(toId);
            if (Cp)
            {
                if (from.ExtraProperties != null)
                {

                    to.ExtraProperties.Add("Children", from.ExtraProperties.Where(x => x.Key == "Children").Select(x => x.Value));
                    await _tradePartnerRepository.UpdateAsync(to);
                }
            
            }
            if (Tp)
            {
                if (from.ExtraProperties != null)
                {

                    to.ExtraProperties.Add("TradePartyList", from.ExtraProperties.Where(x => x.Key == "Children").Select(x => x.Value));
                    await _tradePartnerRepository.UpdateAsync(to);
                }

            }
            if (Mm) {

                var memo = await _tradePartnerMemoAppService.GetListByTradePartnerIdAsync(from.Id);
                foreach (var item in memo)
                {
                    CreateUpdateTradePartnerMemoDto input=new CreateUpdateTradePartnerMemoDto();
                    input.TradePartnerId = to.Id;
                    input.Memo = item.Memo;
                    input.Title=item.Title;
                 
                    await _tradePartnerMemoAppService.SaveAsync(input);

                }
          
            
            }
            if (Md)
            {
                QueryAttachmentDto dto = new()
                {
                    QueryId = fromId,
                    QueryType = 10,
                };

               var FileList = await _attachmentAppService.QueryListAsync(dto);
                foreach (var file in FileList) {
                    CreateUpdateAttachmentDto fileInput=new CreateUpdateAttachmentDto();
                    fileInput.Ftype = file.Ftype;
                    fileInput.FileName = file.FileName;
                    fileInput.Fid = to.Id;
                    fileInput.IsMemo = file.IsMemo;
                    fileInput.ShowName = file.ShowName;
                    fileInput.Size = file.Size;

                    await _attachmentAppService.CreateAsync(fileInput);
                
                }


            }
            from.IsActive = false;
            await _tradePartnerRepository.UpdateAsync(from);

            Dictionary<string, Guid> rs = new()
            {
                { "id", fromId }
            };
            return new JsonResult(rs);
        }
    }
}
