using AutoMapper.Internal.Mappers;
using Dolphin.Freight.ImportExport.OceanImports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Dolphin.Freight.ImportExport.Containers;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.SysCodes;

namespace Dolphin.Freight.Web.Pages.OceanImports
{
    public class EditModal2Model : FreightPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid Hid { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportMblDto OceanImportMbl { get; set; }
        [BindProperty]
        public CreateUpdateOceanImportHblDto OceanImportHbl { get; set; }
        [BindProperty]
        public List<CreateUpdateContainerDto> CreateUpdateContainerDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public int NewHbl { get; set; } = 0;
        public string CardClass { get; set; }
        public bool IsShowHbl { get; set; } = false;
        public IList<OceanImportHblDto> OceanImportHbls { get; set; }
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        private readonly IContainerAppService _containerAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        public EditModal2Model(IContainerAppService containerAppService,IOceanImportMblAppService oceanImportMblAppService, IOceanImportHblAppService oceanImportHblAppService, ISysCodeAppService sysCodeAppService)
        {
            _oceanImportMblAppService = oceanImportMblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _containerAppService = containerAppService;
            _sysCodeAppService = sysCodeAppService;
        }
        public async Task OnGetAsync()
        {
            OceanImportMbl = await _oceanImportMblAppService.GetCreateUpdateOceanImportMblDtoById(Id);
            QueryHblDto query = new QueryHblDto() { MblId = Id };
            OceanImportHbls = await _oceanImportHblAppService.QueryListByMidAsync(query);
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
                OceanImportHbl = new();
                IsShowHbl = true;


            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var OceanImportMb2 = await _oceanImportMblAppService.GetCreateUpdateOceanImportMblDtoById(Id);
            OceanImportMb2.Mark = OceanImportMbl.Mark;
            OceanImportMb2.Description = OceanImportMbl.Description;
            OceanImportMb2.DomesticInstructions = OceanImportMbl.DomesticInstructions;
            await _oceanImportMblAppService.UpdateAsync(Id, OceanImportMb2) ;
            //await _oceanImportMblAppService.UpdateAsync(Id, OceanImportMbl);
            //await _oceanImportHblAppService.UpdateAsync(Hid, OceanImportHbl);
            QueryContainerDto query = new QueryContainerDto() { QueryId=Id };
            var rs = await _containerAppService.DeleteByMblIdAsync(query); 
            foreach (var dto in CreateUpdateContainerDtos) 
            {
                var a = dto.IsDeleted;
                if (dto.Status == 0)await _containerAppService.CreateAsync(dto);
            }
            return NoContent();
        }
    }
}
