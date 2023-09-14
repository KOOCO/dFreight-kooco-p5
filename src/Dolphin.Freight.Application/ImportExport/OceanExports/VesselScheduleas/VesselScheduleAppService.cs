using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using Dolphin.Freight.ImportExport.Containers;
using JetBrains.Annotations;
using Dolphin.Freight.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dolphin.Freight.Settinngs.ContainerSizes;
using Dolphin.Freight.Settings.ContainerSizes;

namespace Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas
{
    public class VesselScheduleAppService :
        CrudAppService<
            VesselSchedule,
            VesselScheduleDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateVesselScheduleDto>,
        IVesselScheduleAppService
    {
        private IRepository<VesselSchedule, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        private IRepository<Port, Guid> _portRepository;
        private readonly IPortsManagementAppService _portsManagementAppService;
        private IRepository<Substation, Guid> _substationRepository;
        private IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IContainerAppService _containerAppService;
        private readonly IOceanExportMblAppService _oceanExportMblAppService;
        private readonly IRepository<OceanExportMbl, Guid> _oceanExportMblRepository;
        private readonly IContainerSizeAppService _containerSizeAppService;
        private readonly IRepository<ContainerSize, Guid> _containerSizeRepository;
        public VesselScheduleAppService(IRepository<VesselSchedule, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<Port, Guid> portRepository, IRepository<Substation, Guid> substationRepository, 
                                        IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository, IPortsManagementAppService portsManagementAppService, IContainerAppService containerAppService,
                                        IOceanExportMblAppService oceanExportMblAppService, IRepository<OceanExportMbl, Guid> oceanExportMblRepository, IContainerSizeAppService containerSizeAppService,
                                        IRepository<ContainerSize, Guid> containerSizeRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _portRepository = portRepository;
            _substationRepository = substationRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _portsManagementAppService = portsManagementAppService;
            _containerAppService = containerAppService;
            _oceanExportMblAppService = oceanExportMblAppService;
            _oceanExportMblRepository = oceanExportMblRepository;
            _containerSizeAppService = containerSizeAppService;
            _containerSizeRepository = containerSizeRepository;
            /*
            GetPolicyName = OceanExportPermissions.VesselScheduleas.Default;
            GetListPolicyName = OceanExportPermissions.VesselScheduleas.Default;
            CreatePolicyName = OceanExportPermissions.VesselScheduleas.Create;
            UpdatePolicyName = OceanExportPermissions.VesselScheduleas.Edit;
            DeletePolicyName = OceanExportPermissions.VesselScheduleas.Delete;*/
        }
        public async Task<PagedResultDto<VesselScheduleDto>> QueryListAsync(QueryVesselScheduleDto query)
        {
            var Ports = await _portsManagementAppService.QueryListAsync();
            Dictionary<Guid, string> pdictionary = new Dictionary<Guid, string>();
            if (Ports != null)
            {
                foreach (var port in Ports)
                {
                    pdictionary.Add(port.Id, port.PortName);
                }
            }
            var Substations = await _substationRepository.GetListAsync();
            Dictionary<Guid, string> sdictionary = new Dictionary<Guid, string>();
            if (Substations != null)
            {
                foreach (var substation in Substations)
                {
                    sdictionary.Add(substation.Id, substation.SubstationName);
                }
            }
            var TradePartners = await _tradePartnerRepository.GetListAsync();
            Dictionary<Guid, string> tdictionary = new Dictionary<Guid, string>();
            if (TradePartners != null)
            {
                foreach (var tradePartner in TradePartners)
                {
                    tdictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }

            var VesselSchedules = (await _repository.GetQueryableAsync())
                                   .WhereIf(!string.IsNullOrWhiteSpace(query.Search), x=>x.ReferenceNo
                                   .Contains(query.Search) || x.VesselName
                                   .Contains(query.Search) || x.Office.SubstationName
                                   .Contains(query.Search) || x.Voyage
                                   .Contains(query.Search))
                                   .WhereIf(!string.IsNullOrWhiteSpace(query.Vessel),x=>x.VesselName==query.Vessel)
                                   .WhereIf(query.OfficeId.HasValue,e=>e.OfficeId==query.OfficeId)
                                   .WhereIf(query.CarrierId.HasValue,e=>e.MblCarrierId==query.CarrierId)
                                   .WhereIf(query.ShippingAgentId.HasValue,e=>e.ShippingAgentId==query.ShippingAgentId)
                                   .WhereIf(query.ForwardingAgentId.HasValue,e=>e.ForwardingAgentId==query.ForwardingAgentId)
                                   .WhereIf(query.Pol.HasValue,e=>e.PolId==query.Pol)
                                   .WhereIf(query.ShipModeId.HasValue,e=>e.ShipModeId==query.ShipModeId)
                                   .WhereIf(query.OvearseaAgentId.HasValue, e => e.MblOverseaAgentId == query.OvearseaAgentId)
                                   .WhereIf(query.BlTypeId.HasValue,e=>e.BlTypeId==query.BlTypeId)
                                   .WhereIf(query.PostDate.HasValue,e=>e.PostDate.Date==query.PostDate.Value.Date.AddDays(1))
                                   .WhereIf(query.Eta.HasValue,e=>e.PodEta.Value.Date==query.Eta.Value.Date.AddDays(1))
                                   .WhereIf(query.Etd.HasValue,e=>e.PolEtd.Value.Date==query.Etd.Value.Date.AddDays(1));
                                    
            List<VesselSchedule> rs = VesselSchedules.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<VesselScheduleDto> list = new List<VesselScheduleDto>();

            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<VesselSchedule, VesselScheduleDto>(pu);
                    if (pud.PolId != null) pud.PolName = pdictionary[pud.PolId.Value];
                    if (pud.PorId != null) pud.PorName = pdictionary[pud.PorId.Value];
                    if (pud.PodId != null) pud.PodName = pdictionary[pud.PodId.Value];
                    if (pud.DelId != null) pud.DelName = pdictionary[pud.DelId.Value];
                    if (pud.OfficeId != null) pud.OfficeName = sdictionary[pud.OfficeId.Value];
                    if (pud.MblOverseaAgentId != null) pud.MblOverseaAgentName = tdictionary[pud.MblOverseaAgentId.Value];
                    list.Add(pud);
                }
            }
            PagedResultDto<VesselScheduleDto> listDto = new PagedResultDto<VesselScheduleDto>();
            listDto.Items = list;
            listDto.TotalCount = VesselSchedules.Count();
            return listDto;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<List<VesselScheduleDto>> GetListAsync(QueryVesselScheduleDto query)
        {
            var Ports = await _portsManagementAppService.QueryListAsync();
            Dictionary<Guid, string> pdictionary = new Dictionary<Guid, string>();
            if (Ports != null)
            {
                foreach (var port in Ports)
                {
                    pdictionary.Add(port.Id, port.PortName);
                }
            }
            var Substations = await _substationRepository.GetListAsync();
            Dictionary<Guid, string> sdictionary = new Dictionary<Guid, string>();
            if (Substations != null)
            {
                foreach (var substation in Substations)
                {
                    sdictionary.Add(substation.Id, substation.SubstationName);
                }
            }
            var TradePartners = await _tradePartnerRepository.GetListAsync();
            Dictionary<Guid, string> tdictionary = new Dictionary<Guid, string>();
            if (TradePartners != null)
            {
                foreach (var tradePartner in TradePartners)
                {
                    tdictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }

            var VesselSchedules = await _repository.GetListAsync();
            List<VesselSchedule> rs = VesselSchedules;
            List<VesselScheduleDto> list = new List<VesselScheduleDto>();

            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<VesselSchedule, VesselScheduleDto>(pu);
                    if (pud.PolId != null) pud.PolName = pdictionary[pud.PolId.Value];
                    if (pud.PorId != null) pud.PorName = pdictionary[pud.PorId.Value];
                    if (pud.PodId != null) pud.PodName = pdictionary[pud.PodId.Value];
                    if (pud.DelId != null) pud.DelName = pdictionary[pud.DelId.Value];
                    if (pud.OfficeId != null) pud.OfficeName = sdictionary[pud.OfficeId.Value];
                    if (pud.MblOverseaAgentId != null) pud.MblOverseaAgentName = tdictionary[pud.MblOverseaAgentId.Value];
                    list.Add(pud);
                }
            }
            return list;
        }
        public async Task DeleteMultipleVesselAsync(Guid[] ids)
        {
            foreach (var id in ids)
            {
                await _repository.DeleteAsync(id);

              
               
             
            }
        }

        public async Task<JsonResult> GetMblContainersByVesselIdAsync(Guid id)
        {
            var containersList = await _containerAppService.QueryListVesselAsync(id);
            var containerSizeList = await _containerSizeRepository.GetListAsync();
            var containersData = new List<CreateUpdateContainerDto>();

            foreach (var container in containersList)
            {
                var containerData = new CreateUpdateContainerDto()
                {
                    Id = container.Id,
                    MblId = (Guid)container.MblId,
                    ContainerNo = container.ContainerNo,
                    ContainerSizeId = container.ContainerSizeId,
                    ContainerSizeName = containerSizeList.Where(w => w.Id == container.ContainerSizeId).Select(s => s.ContainerCode).FirstOrDefault(),
                    SealNo = container.SealNo,
                    CreationTime = container.CreationTime,
                };
                containersData.Add(containerData);
            }

            var mbls = new List<CreateUpdateOceanExportMblDto>();

            foreach (var container in containersList)
            {
                var mblid = container.MblId;
                var mbl = await _oceanExportMblAppService.GetAsync((Guid)mblid);

                var mblData = new CreateUpdateOceanExportMblDto()
                {
                    Id = mbl.Id,
                    IsLocked = mbl.IsLocked,
                    FilingNo = mbl.FilingNo,
                    MblNo = mbl.MblNo
                };

                mbls.Add(mblData);
            }

            return new JsonResult(new { containerList = containersData, mblList = mbls });
        }
    }
}
