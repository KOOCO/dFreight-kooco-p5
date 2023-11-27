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
using Microsoft.AspNetCore.Mvc.Rendering;
using Dolphin.Freight.ImportExport.OceanImports;
using static Dolphin.Freight.Permissions.OceanImportPermissions;
using Volo.Abp.ObjectMapping;
using Newtonsoft.Json;
using Dolphin.Freight.Settinngs.PackageUnits;

namespace Dolphin.Freight.Web.Pages.OceanExports
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
        public CreateUpdateOceanExportMblDto OceanExportMbl { get; set; }
        [BindProperty]
        public CreateUpdateOceanExportHblDto OceanExportHbl { get; set; }
        [BindProperty]
        public List<CreateUpdateOceanExportHblDto> OceanExportHblContainer { get; set; }
        [BindProperty]
        public List<CreateUpdateContainerDto> CreateUpdateContainerDtos { get; set; }

        [BindProperty]
        public CreateUpdateContainerDto CreateUpdateContainerHawb { get; set; }

        [BindProperty]
        public List<ManifestCommodity> Commodities { get; set; }

        public List<SelectListItem> RateUnitTypeLookupList { get; set; }
        public List<SelectListItem> UnitTypeLookupList { get; set; }


        [BindProperty(SupportsGet = true)]
        public int NewHbl { get; set; } = 0;
        public string CardClass { get; set; }
        public bool IsShowHbl { get; set; } = false;
        public IList<OceanExportHblDto> OceanExportHbls { get; set; }
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly IContainerAppService _containerAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IOceanImportMblAppService _oceanImportMblAppService;
        public EditModal2Model(IContainerAppService containerAppService,
            IOceanExportMblAppService oceanExportMblAppService, 
            IOceanExportHblAppService oceanExportHblAppService, 
            ISysCodeAppService sysCodeAppService,
            IOceanImportMblAppService oceanImportMblAppService)
        {
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportHblAppService = oceanExportHblAppService;
            _containerAppService = containerAppService;
            _sysCodeAppService = sysCodeAppService;
            _oceanImportMblAppService=oceanImportMblAppService;
        }
        public async Task OnGetAsync()
        {
            OceanExportMbl = await _oceanExportMblAppService.GetCreateUpdateOceanExportMblDtoById(Id);
            ImportExport.OceanExports.QueryHblDto query = new ImportExport.OceanExports.QueryHblDto() { MblId = Id };
            OceanExportHbls = await _oceanExportHblAppService.QueryListByMidAsync(query);
            ImportExport.OceanExports.QueryHblDto queryHbl = new ImportExport.OceanExports.QueryHblDto();
            RateUnitTypeLookupList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "KG", Value = "KG" },
                new SelectListItem() { Text = "LG", Value = "LG" }
            };
            UnitTypeLookupList = new List<SelectListItem>
            {
                new SelectListItem() { Text = "CBM", Value = "CBM" },
                new SelectListItem() { Text = "CFT", Value = "CFT" }
            };
            Commodities = new List<ManifestCommodity>();
            ISToolTipShow = await _oceanImportMblAppService.GetCardSettings();
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
                        OceanExportHbl.PackageNo = OceanExportHbls[0].CreateUpdateHBLContainerDto?.PackageNum ?? 0;
                        OceanExportHbl.PackageWeight = OceanExportHbls[0].CreateUpdateHBLContainerDto?.PackageWeight ?? 0;
                        OceanExportHbl.PackageMeasurement = OceanExportHbls[0].CreateUpdateHBLContainerDto?.PackageMeasure ?? 0;
                        OceanExportHbl.ContainerId = OceanExportHbls[0].CreateUpdateHBLContainerDto?.Id;
                        Hid = OceanExportHbl.Id;
                        IsShowHbl = true;
                    }
                }

            }
            else
            {
                queryHbl.Id = Hid;

                var hbl = await _oceanExportHblAppService.GetAsync(Hid);

                var container = await _containerAppService.GetContainerByHblId(Hid);

                OceanExportHbl = new CreateUpdateOceanExportHblDto();

                OceanExportHbl = ObjectMapper.Map<OceanExportHblDto, CreateUpdateOceanExportHblDto>(hbl);
                if (container is not null)
                {
                    OceanExportHbl.PackageNo = container.PackageNum;
                    OceanExportHbl.PackageWeight = container.PackageWeight;
                    OceanExportHbl.PackageMeasurement = container.PackageMeasure;
                    OceanExportHbl.ContainerId = container.Id;
                }
                IsShowHbl = true;
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (OceanExportHbl.Id != Guid.Empty)
            {
                ImportExport.OceanExports.QueryHblDto queryHbl = new ImportExport.OceanExports.QueryHblDto();
                queryHbl.Id = OceanExportHbl.Id;
                var OceanExportHb2 = await _oceanExportHblAppService.GetHblById(queryHbl);
                //OceanExportHb2.PackageNo = OceanExportHbl.PackageNo;
                //OceanExportHb2.PackageWeight = OceanExportHbl.PackageWeight;
                //OceanExportHb2.PackageMeasurement = OceanExportHbl.PackageMeasurement;
                if (OceanExportHb2.Id != Guid.Empty)
                {
                    foreach (var item in OceanExportHblContainer)
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
                OceanExportHb2.Mark = OceanExportHbl.Mark;
                OceanExportHb2.Description = OceanExportHbl.Description;
                OceanExportHb2.DomesticInstructions = OceanExportHbl.DomesticInstructions;
                OceanExportHb2.PoNo = OceanExportHbl.PoNo;

                if (OceanExportHb2.ExtraProperties == null)
                {
                    OceanExportHb2.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
                }

                if (Commodities != null && Commodities.Any())
                {
                    OceanExportHb2.ExtraProperties.Remove("Commodities");
                    OceanExportHb2.ExtraProperties.Add("Commodities", Commodities);
                }

                await _oceanExportHblAppService.UpdateAsync(OceanExportHb2.Id, OceanExportHb2);

                //if (CreateUpdateContainerHawb is not null)
                //{
                //    CreateUpdateContainerHawb.HblId = OceanExportHbl.Id;
                //    if (CreateUpdateContainerHawb.Id != Guid.Empty)
                //    {
                //        await _containerAppService.UpdateAsync(CreateUpdateContainerHawb.Id, CreateUpdateContainerHawb);
                //    }
                //    else
                //    {
                //        await _containerAppService.CreateAsync(CreateUpdateContainerHawb);
                //    }

                //}
            }

            var OceanExportMb2 = ObjectMapper.Map<OceanExportMblDto, CreateUpdateOceanExportMblDto>(await _oceanExportMblAppService.GetAsync(Id));
            OceanExportMb2.Mark = OceanExportMbl.Mark;
            OceanExportMb2.Description = OceanExportMbl.Description;
            OceanExportMb2.DomesticInstructions = OceanExportMbl.DomesticInstructions;
            await _oceanExportMblAppService.UpdateAsync(Id, OceanExportMb2);

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
