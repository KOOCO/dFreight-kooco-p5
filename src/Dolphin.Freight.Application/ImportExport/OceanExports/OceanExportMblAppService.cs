using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Common;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class OceanExportMblAppService :
        CrudAppService<
            OceanExportMbl, //IT號碼管理entity
            OceanExportMblDto, //顯示IT號碼管理用
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateOceanExportMblDto>, //新增修改IT號碼管理用
        IOceanExportMblAppService //實作IOceanExportMblAppService
    {
        private readonly IRepository<OceanExportMbl, Guid> _repository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly PortsManagementAppService _portRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        public OceanExportMblAppService(IRepository<OceanExportMbl, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<Substation, Guid> substationRepository, PortsManagementAppService portRepository, IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _substationRepository =  substationRepository;
            _portRepository = portRepository;
            _tradePartnerRepository = tradePartnerRepository;
            /*
            GetPolicyName = OceanExportPermissions.OceanExportMbls.Default;
            GetListPolicyName = OceanExportPermissions.OceanExportMbls.Default;
            CreatePolicyName = OceanExportPermissions.OceanExportMbls.Create;
            UpdatePolicyName = OceanExportPermissions.OceanExportMbls.Edit;
            DeletePolicyName = OceanExportPermissions.OceanExportMbls.Delete;*/
        }
        public async Task<PagedResultDto<OceanExportMblDto>> QueryListAsync(QueryMblDto query)
        {
            var substations = await _substationRepository.GetListAsync();
            Dictionary<Guid, string> substationsDictionary = new Dictionary<Guid, string>();
            if (substations != null) 
            {
                foreach (var substation in substations) 
                {
                    substationsDictionary.Add(substation.Id, substation.SubstationName + "(" + substation.AbbreviationName + ")");
                }
            }
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.CodeValue);
                }
            }
            var OceanExportMbls = await _repository.GetListAsync();
            List<OceanExportMbl> rs;
            List<OceanExportMblDto> list = new List<OceanExportMblDto>();
            if (query != null && query.QueryKey != null)
            {
                rs = OceanExportMbls.OrderByDescending(x=>x.CreationTime ).ToList();
            }
            else
            {
                rs = OceanExportMbls.OrderByDescending(x => x.CreationTime).ToList();
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var r in rs)
                {
                    var item = ObjectMapper.Map<OceanExportMbl, OceanExportMblDto>(r);
                    item.OfficeName = substationsDictionary[r.OfficeId.Value];
                    list.Add(item);
                }
            }
            PagedResultDto<OceanExportMblDto> listDto = new PagedResultDto<OceanExportMblDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }
        public async void LockedOrUnLockedOceanExportMblAsync(QueryMblDto query) 
        {
            var mbl = await _repository.GetAsync(query.MbId.Value);
            mbl.IsLocked = !mbl.IsLocked;
            await _repository.UpdateAsync(mbl);
        }
        public async Task<CreateUpdateOceanExportMblDto> GetCreateUpdateOceanExportMblDtoById(Guid Id) {
            var oceanExportMbl = await _repository.GetAsync(Id,true);
            var dto = ObjectMapper.Map<OceanExportMbl, CreateUpdateOceanExportMblDto>(oceanExportMbl);
            var ports = await _portRepository.QueryListAsync();
            Dictionary<Guid, string> pdictionary = new();
            if (ports != null && ports.Count > 0)
            {
                foreach (var port in ports)
                {
                    pdictionary.Add(port.Id, port.SubDiv + " " + port.PortName + " ( " + port.Locode + " ) ");
                }
            }
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            Dictionary<Guid, string> tdictionary = new();
            if (tradePartners != null && tradePartners.Count > 0)
            {
                foreach (var tradePartner in tradePartners)
                {
                    tdictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }
            if (dto != null) 
            {
                if (dto.PodId != null) dto.PodName = pdictionary[dto.PodId.Value];
                if (dto.PolId != null) dto.PolName = pdictionary[dto.PolId.Value];
                if (dto.PorId != null) dto.PorName = pdictionary[dto.PorId.Value];
                if (dto.DelId != null) dto.DelName = pdictionary[dto.DelId.Value];
                if (dto.FdestId != null) dto.FdestName = pdictionary[dto.FdestId.Value];
                if (dto.MblCarrierId != null)dto.MblCarrierName = tdictionary[dto.MblCarrierId.Value];
                if(dto.MblOverseaAgentId != null)dto.MblOverseaAgentName = tdictionary[dto.MblOverseaAgentId.Value];
            }
            return dto;
        }

        public async Task<CreateUpdateOceanExportMblDto> GetMblById(QueryMblDto query)
        {
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.CodeValue);
                }
            }
            var oceanExportMbl = await _repository.GetAsync(query.MbId.Value);
            var rs = ObjectMapper.Map<OceanExportMbl, CreateUpdateOceanExportMblDto>(oceanExportMbl);
            
            return rs;
        }
    }
}