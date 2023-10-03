
using Dolphin.Freight.Accounting;
using Dolphin.Freight.Common;
using Dolphin.Freight.Common.Memos;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.OceanImports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dolphin.Freight.Web.Pages.OceanImports
{
    public class CreateMblModel : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }
        public List<SelectListItem> SubstationLookupList { get; set; }
        public List<SelectListItem> TradePartnerLookupList { get; set; }
        public List<SelectListItem> PortsManagementLookupList { get; set; }
        public List<SelectListItem> SalesTypeLookupList { get; set; }
        public List<SelectListItem> CargoTypeLookupList { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportMblDto? OceanImportMbl { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportHblDto? OceanImportHbl { get; set; }
        [BindProperty]
        public int AddHbl { get; set; } = 0;
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IMemoAppService _memoAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IPortsManagementAppService _portsManagementAppService;
        private readonly ICurrentUser _currentUser;
        public List<SelectListItem> ItIssuedLocationList { get; set; }
        public List<SelectListItem> RailCodeList { get; set; }
        public CreateMblModel( IOceanImportMblAppService oceanImportMblAppService,IOceanImportHblAppService oceanImportHblAppService, ISysCodeAppService sysCodeAppService, IPortsManagementAppService portsManagementAppService, ITradePartnerAppService tradePartnerAppService, ISubstationAppService substationAppService, ICurrentUser currentUser, IMemoAppService memoAppService)
        {
            _oceanImportMblAppService = oceanImportMblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _sysCodeAppService = sysCodeAppService;
            _substationAppService = substationAppService;
            _tradePartnerAppService = tradePartnerAppService;
            _portsManagementAppService = portsManagementAppService;
            _currentUser = currentUser;
            _memoAppService = memoAppService;
        }
        public async Task OnGetAsync()
        {
            OceanImportHbl = new CreateUpdateOceanImportHblDto();
            OceanImportMbl = new CreateUpdateOceanImportMblDto();
          var blTypeLookup=  await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "BlTypeId" });

             
            OceanImportMbl.BlTypeId=blTypeLookup.Where(x=>x.CodeValue=="NR").Select(x=>x.Id).FirstOrDefault();
            var shipModeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "ShipModeId" });


            OceanImportMbl.ShipModeId = shipModeLookup.Where(x => x.CodeValue == "FCL").Select(x => x.Id).FirstOrDefault();
            OceanImportHbl.ShipModeId = OceanImportMbl.ShipModeId;
            await FillSubstationAsync();
            await FillTradePartnerAsync();
            await FillPortAsync();
            ItIssuedLocationList = GetItIssuedLocationSelectList();
            RailCodeList = GetRailCodeSelectList();
        }
        public async Task<JsonResult> OnPostAsync()
        {
            if (OceanImportMbl.PolEtd == null) OceanImportMbl.PostDate = DateTime.Now;
            else OceanImportMbl.PostDate = OceanImportMbl.PolEtd;
            OceanImportMbl.FilingNo = await _sysCodeAppService.GetSystemNoAsync(new (){ QueryType= "OceanImportMbl_FilingNo" });
            var mbl = await _oceanImportMblAppService.CreateAsync(OceanImportMbl);
            OceanImportHbl.MblId = mbl.Id;
            Id = mbl.Id;
            if (AddHbl == 1)
            {
                //if (OceanImportHbl.IsCreateBySystem)
                //{
                //    OceanImportHbl.HblNo = await _sysCodeAppService.GetSystemNoAsync(new() { QueryType = "OceanImportHbl_HblNo" });
                //}
                QueryDto query  = new QueryDto();
                query.QueryType = "CardColorId";
                var syscodes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(query);
                if (syscodes != null && syscodes.Count >0) 
                {
                    var syscode = syscodes[0];
                    OceanImportHbl.CardColorId = syscode.Id;
                }
               var hbl= await _oceanImportHblAppService.CreateAsync(OceanImportHbl);
                var emptyGuid = Guid.Empty;
                var memo = await _memoAppService.GetListByQueryAsync(new QueryListDto() { SourceId = emptyGuid,FType=FreightPageType.OIHBL}) ;
                if (memo.Count > 0)
                {
                    await _memoAppService.UpdateSourceIdAsync(hbl.Id, FreightPageType.OIHBL);
                
                }
            
            }

            Dictionary<string, Guid> rs = new()
            {
                { "id", Id.Value }
            };
            return new JsonResult(rs);
        }
        #region FillSubstationAsync()
        private async Task FillSubstationAsync()
        {
            var substationLookup = await _substationAppService.GetSubstationsLookupAsync();
            SubstationLookupList = substationLookup.Items
                                                .Select(x => new SelectListItem(x.SubstationName + "  (" + x.AbbreviationName + ")", x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillTradePartnerAsync()
        private async Task FillTradePartnerAsync()
        {
            var tradePartnerLookup = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            TradePartnerLookupList = tradePartnerLookup.Items
                                                .Select(x => new SelectListItem(x.TPName + " / " + x.TPCode, x.Id.ToString(), false))
                                                .ToList();
        }
        #endregion

        #region FillPortAsync()
        private async Task FillPortAsync()
        {
            var lookup = await _portsManagementAppService.QueryListAsync();

            PortsManagementLookupList = lookup.Select(x => new SelectListItem(x.PortName, x.Id.ToString(), false))
                                   .ToList();
        }
        #endregion
        private List<SelectListItem> GetItIssuedLocationSelectList()
        {
            var enumValues = Enum.GetValues(typeof(ItIssuedLocation))
                                  .Cast<ItIssuedLocation>()
                                  .Select(e => new SelectListItem
                                  {
                                      Text = L["Enum:ItIssuedLocation." + e.ToString()],
                                      Value = e.ToString(),

                                  })
                                  .ToList();

            return enumValues;
        }
        private List<SelectListItem> GetRailCodeSelectList()
        {
            var enumValues = Enum.GetValues(typeof(RailCode))
                                  .Cast<RailCode>()
                                  .Select(e => new SelectListItem
                                  {
                                      Text = L["Enum:RailCode." + e.ToString()],
                                      Value = e.ToString(),

                                  })
                                  .ToList();

            return enumValues;
        }
    }
}


