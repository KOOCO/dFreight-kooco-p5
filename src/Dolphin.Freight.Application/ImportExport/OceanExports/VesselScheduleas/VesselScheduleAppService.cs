using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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
        public VesselScheduleAppService(IRepository<VesselSchedule, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<Port, Guid> portRepository, IRepository<Substation, Guid> substationRepository, 
                                        IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository, IPortsManagementAppService portsManagementAppService)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _portRepository = portRepository;
            _substationRepository = substationRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _portsManagementAppService = portsManagementAppService;
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
            PagedResultDto<VesselScheduleDto> listDto = new PagedResultDto<VesselScheduleDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
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
    }
}
