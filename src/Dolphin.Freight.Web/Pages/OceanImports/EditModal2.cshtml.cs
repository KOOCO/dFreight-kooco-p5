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
using Dolphin.Freight.ImportExport.OceanExports;
using QueryHblDto = Dolphin.Freight.ImportExport.OceanImports.QueryHblDto;
using static Dolphin.Freight.Permissions.OceanExportPermissions;
using System.Linq;
using Volo.Abp.Domain.Repositories;
using Newtonsoft.Json;

namespace Dolphin.Freight.Web.Pages.OceanImports
{
    public class EditModal2Model : FreightPageModel
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
        [BindProperty]
        public List<CreateUpdateOceanExportHblDto> OceanImportHblContainer { get; set; }
        [BindProperty]
        public List<CreateUpdateContainerDto> CreateUpdateContainerDtos { get; set; }

        [BindProperty]
        public List<ManifestCommodity> Commodities { get; set; }

        [BindProperty(SupportsGet = true)]
        public int NewHbl { get; set; } = 0;
        public string CardClass { get; set; }
        public bool IsShowHbl { get; set; } = false;
        public IList<OceanImportHblDto> OceanImportHbls { get; set; }
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        private readonly IContainerAppService _containerAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IRepository<Container, Guid> _containerRepository;
        public EditModal2Model(IContainerAppService containerAppService, IRepository<Container, Guid> containerRepository, IOceanImportMblAppService oceanImportMblAppService, IOceanImportHblAppService oceanImportHblAppService, ISysCodeAppService sysCodeAppService)
        {
            _oceanImportMblAppService = oceanImportMblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _containerAppService = containerAppService;
            _sysCodeAppService = sysCodeAppService;
            _containerRepository = containerRepository;
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
                        OceanImportHbl.PackageNo = OceanImportHbls[0].CreateUpdateHBLContainerDto?.PackageNum ?? 0;
                        OceanImportHbl.PackageWeight = OceanImportHbls[0].CreateUpdateHBLContainerDto?.PackageWeight ?? 0;
                        OceanImportHbl.PackageMeasurement = OceanImportHbls[0].CreateUpdateHBLContainerDto?.PackageMeasure ?? 0;
                        OceanImportHbl.ContainerId = OceanImportHbls[0].CreateUpdateHBLContainerDto?.Id;
                        Hid = OceanImportHbl.Id;
                        IsShowHbl = true;
                    }
                }

            }
            else
            {
                queryHbl.Id = Hid;
                OceanImportHbl = new CreateUpdateOceanImportHblDto();
                if (OceanImportHbls != null && OceanImportHbls.Count > 0)
                {
                    OceanImportHbl = ObjectMapper.Map<OceanImportHblDto, CreateUpdateOceanImportHblDto>(OceanImportHbls.Where(w => w.Id == Hid).FirstOrDefault());
                    OceanImportHbl.PackageNo = OceanImportHbls.Where(w => w.Id == Hid).Select(s => s.CreateUpdateHBLContainerDto?.PackageNum ?? 0).FirstOrDefault();
                    OceanImportHbl.PackageWeight = OceanImportHbls.Where(w => w.Id == Hid).Select(s => s.CreateUpdateHBLContainerDto?.PackageWeight ?? 0).FirstOrDefault();
                    OceanImportHbl.PackageMeasurement = OceanImportHbls.Where(w => w.Id == Hid).Select(s => s.CreateUpdateHBLContainerDto?.PackageMeasure ?? 0).FirstOrDefault();
                    OceanImportHbl.ContainerId = OceanImportHbls.Where(w => w.Id == Hid).Select(s => s.CreateUpdateHBLContainerDto?.Id ?? Guid.Empty).FirstOrDefault();
                }
                IsShowHbl = true;


            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var OceanImportMb2 = await _oceanImportMblAppService.GetCreateUpdateOceanImportMblDtoById(Id);
            OceanImportMb2.Mark = OceanImportMbl.Mark;
            OceanImportMb2.Description = OceanImportMbl.Description;
            OceanImportMb2.DomesticInstructions = OceanImportMbl.DomesticInstructions;
            await _oceanImportMblAppService.UpdateAsync(Id, OceanImportMb2);
            QueryHblDto queryHblDto = new QueryHblDto() { Id = OceanImportHbl.Id };
            var oceanImportHbl = await _oceanImportHblAppService.GetHblById(queryHblDto);
            //oceanImportHbl.PackageNo = OceanImportHbl.PackageNo;
            //oceanImportHbl.PackageWeight = OceanImportHbl.PackageWeight;
            //oceanImportHbl.PackageMeasurement = OceanImportHbl.PackageMeasurement;
            if (oceanImportHbl.Id != Guid.Empty)
            {
                foreach (var item in OceanImportHblContainer)
                {
                    CreateUpdateContainerDto containerDto = new CreateUpdateContainerDto()
                    {
                        ContainerNo = item.ContainerNo,
                        PackageNum = item.PackageNo,
                        PackageWeight = item.PackageWeight,
                        PackageMeasure = item.PackageMeasurement,
                        HblId = item.Id,
                        Id = (Guid)item.ContainerId
                    };
                    if (item.ContainerId is not null && item.ContainerId != Guid.Empty)
                    {
                        await _containerAppService.UpdateAsync(item.ContainerId.GetValueOrDefault(), containerDto);
                    }
                    else
                    {
                        await _containerAppService.CreateAsync(containerDto);
                    }
                }
            }
            oceanImportHbl.Mark = OceanImportHbl.Mark;
            oceanImportHbl.Description = OceanImportHbl.Description;
            oceanImportHbl.DomesticInstructions = OceanImportHbl.DomesticInstructions;
            oceanImportHbl.DomesticInstructionsDelOrder = OceanImportHbl.DomesticInstructionsDelOrder;
            oceanImportHbl.PoNo = OceanImportHbl.PoNo;

            if (oceanImportHbl.ExtraProperties == null)
            {
                oceanImportHbl.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
            }

            if (Commodities != null && Commodities.Any())
            {
                oceanImportHbl.ExtraProperties.Remove("Commodities");
                oceanImportHbl.ExtraProperties.Add("Commodities", Commodities);
            }

            await _oceanImportHblAppService.UpdateAsync(oceanImportHbl.Id, oceanImportHbl);
            
            foreach (var dto in CreateUpdateContainerDtos) 
            {
                if (dto.Id != Guid.Empty)
                {
                    if (dto.Status == 0)
                    {
                        var container = await _containerAppService.GetAsync(dto.Id);

                        if (container.ExtraProperties.GetValueOrDefault("Dimensions") != null && container.ExtraProperties.Count > 0)
                        {
                            var dimensions = container.ExtraProperties.GetValueOrDefault("Dimensions").ToString();

                            var dimensionList = JsonConvert.DeserializeObject<List<Dimension>>(dimensions);

                            dto.ExtraProperties = container.ExtraProperties;

                            dto.ExtraProperties.Remove("Dimensions");
                            dto.ExtraProperties.Add("Dimensions", dimensionList);
                        }

                        await _containerAppService.UpdateAsync(dto.Id, dto);
                    }
                } else
                {
                    await _containerAppService.CreateAsync(dto);
                }
            }
            return NoContent();
        }
    }
}
