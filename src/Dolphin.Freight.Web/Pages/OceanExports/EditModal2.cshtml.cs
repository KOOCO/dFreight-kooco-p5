using AutoMapper.Internal.Mappers;
using Dolphin.Freight.ImportExport.OceanExports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Dolphin.Freight.ImportExport.Containers;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.SysCodes;
using System.Linq;

namespace Dolphin.Freight.Web.Pages.OceanExports
{
    public class EditModal2Model : FreightPageModel
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
        [BindProperty]
        public List<CreateUpdateContainerDto> CreateUpdateContainerDtos { get; set; }
        [BindProperty(SupportsGet = true)]
        public int NewHbl { get; set; } = 0;
        public string CardClass { get; set; }
        public bool IsShowHbl { get; set; } = false;
        public IList<OceanExportHblDto> OceanExportHbls { get; set; }
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly IContainerAppService _containerAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        public EditModal2Model(IContainerAppService containerAppService,IOceanExportMblAppService oceanExportMblAppService, IOceanExportHblAppService oceanExportHblAppService, ISysCodeAppService sysCodeAppService)
        {
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _containerAppService = containerAppService;
            _sysCodeAppService = sysCodeAppService;
        }
        public async Task OnGetAsync()
        {
            OceanExportMbl = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(Id);
            QueryHblDto query = new QueryHblDto() { MblId = Id };
            OceanExportHbls = await _oceanExportHblAppService.QueryListByMidAsync(query);
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
                        Hid = OceanExportHbl.Id;
                        IsShowHbl = true;
                    }
                }

            }
            else
            {
                queryHbl.Id = Hid;
                OceanExportHbl = await _oceanExportHblAppService.GetHblById(queryHbl);
                IsShowHbl = true;


            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (Hid != Guid.Empty)
            {
                QueryHblDto queryHbl = new QueryHblDto();
                queryHbl.Id = Hid;
                var OceanExportHb2 = await _oceanExportHblAppService.GetHblById(queryHbl);
                
                OceanExportHb2.Mark = OceanExportHbl.Mark;
                OceanExportHb2.Description = OceanExportHbl.Description;
                OceanExportHb2.DomesticInstructions = OceanExportHbl.DomesticInstructions;
                await _oceanExportHblAppService.UpdateAsync(Hid, OceanExportHb2);
                //await _oceanExportMblAppService.UpdateAsync(Id, OceanExportMbl);
                //await _oceanExportHblAppService.UpdateAsync(Hid, OceanExportHbl);
                QueryContainerDto query1 = new QueryContainerDto() { QueryId = Hid };
                var rs1 = await _containerAppService.DeleteByMblIdAsync(query1);
                foreach (var dto in CreateUpdateContainerDtos)
                {
                    var a = dto.IsDeleted;
                    if (dto.Status == 0) await _containerAppService.CreateAsync(dto);
                }


            }

            var OceanExportMb2 = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(Id);
            OceanExportMb2.Mark = OceanExportMbl.Mark;
            OceanExportMb2.Description = OceanExportMbl.Description;
            OceanExportMb2.DomesticInstructions = OceanExportMbl.DomesticInstructions;
            await _oceanExportMblAppService.UpdateAsync(Id, OceanExportMb2) ;
            //await _oceanExportMblAppService.UpdateAsync(Id, OceanExportMbl);
            //await _oceanExportHblAppService.UpdateAsync(Hid, OceanExportHbl);
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
