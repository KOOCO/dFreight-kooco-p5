using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.Ports;
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
    public class OceanExportHblAppService :
        CrudAppService<
            OceanExportHbl, 
            OceanExportHblDto, 
            Guid, 
            PagedAndSortedResultRequestDto, 
            CreateUpdateOceanExportHblDto>, 
        IOceanExportHblAppService 
    {
        private readonly IRepository<OceanExportHbl, Guid> _repository;
        private readonly IRepository<OceanExportMbl, Guid> _mblRepository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly IRepository<Port, Guid> _portRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        public OceanExportHblAppService(IRepository<OceanExportHbl, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<OceanExportMbl, Guid> mblRepository, IRepository<Substation, Guid> substationRepository, IRepository<Port, Guid>  portRepository, IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _mblRepository = mblRepository;
            _substationRepository = substationRepository;
            _portRepository = portRepository;
            _tradePartnerRepository = tradePartnerRepository;
            /*
            GetPolicyName = OceanExportPermissions.OceanExportHbls.Default;
            GetListPolicyName = OceanExportPermissions.OceanExportHbls.Default;
            CreatePolicyName = OceanExportPermissions.OceanExportHbls.Create;
            UpdatePolicyName = OceanExportPermissions.OceanExportHbls.Edit;
            DeletePolicyName = OceanExportPermissions.OceanExportHbls.Delete;*/
        }
        public async Task<PagedResultDto<OceanExportHblDto>> QueryListAsync(QueryHblDto query)
        {
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.CodeValue);
                }
            }
            var Substations = await _substationRepository.GetListAsync();
            Dictionary<Guid, string> sdictionary = new();
            if (Substations != null) 
            {
                foreach (var sub in Substations) 
                {
                    sdictionary.Add(sub.Id, sub.SubstationName);
                }
            }
            //貿易夥伴
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            Dictionary<Guid, string> tdictionary = new();
            if (tradePartners != null && tradePartners.Count > 0)
            {
                foreach (var tradePartner in tradePartners)
                {
                    tdictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }
            //Mbls
            var mbls = await _mblRepository.GetListAsync();
            Dictionary<Guid, OceanExportMbl> mdictionary = new();
            if (mbls != null) 
            { 
                foreach(var mbl in mbls) 
                {
                    mdictionary.Add(mbl.Id, mbl);
                }
            }
            //港口
            var ports = await _portRepository.GetListAsync();
            Dictionary<Guid, string> pdictionary = new();
            if (ports != null && ports.Count > 0)
            {
                foreach (var port in ports)
                {
                    pdictionary.Add(port.Id, port.SubDiv + " " + port.PortName + " ( " + port.Locode + " ) ");
                }
            }
            var OceanExportHbls = await _repository.GetListAsync();
            List<OceanExportHbl> rs;
            List<OceanExportHblDto> list = new List<OceanExportHblDto>();
            if (query != null && query.MblId != null)
            {
                rs = OceanExportHbls.Where(x=>x.MblId.Equals(query.MblId.Value)).ToList();
            }
            else
            {
                rs = OceanExportHbls;
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<OceanExportHbl, OceanExportHblDto>(pu);
                    if (mdictionary.ContainsKey(dto.MblId))
                    {
                        dto.FilingNo = mdictionary[dto.MblId].FilingNo;
                        dto.MblNo = mdictionary[dto.MblId].SoNo;
                        if (mdictionary[dto.MblId].OfficeId.HasValue && sdictionary.ContainsKey(mdictionary[dto.MblId].OfficeId.Value))
                            dto.OfficeName = sdictionary[mdictionary[dto.MblId].OfficeId.Value];

                        //SysCode
                        if (mdictionary[dto.MblId].OblTypeId != null) dto.OblTypeName = dictionary[mdictionary[dto.MblId].OblTypeId.Value];
                    }

                    //人
                    if (dto.AgentId.HasValue && tdictionary.ContainsKey(dto.AgentId.Value))
                        dto.AgentName = tdictionary[dto.AgentId.Value];
                    if (dto.HblShipperId.HasValue && tdictionary.ContainsKey(dto.HblShipperId.Value))
                        dto.HblShipperName = tdictionary[dto.HblShipperId.Value];
                    if (dto.HblConsigneeId.HasValue && tdictionary.ContainsKey(dto.HblConsigneeId.Value)) 
                        dto.HblConsigneeName = tdictionary[dto.HblConsigneeId.Value];
                    if (mdictionary.ContainsKey(dto.MblId))
                    {
                        if (mdictionary[dto.MblId].MblCarrierId.HasValue && tdictionary.ContainsKey(mdictionary[dto.MblId].MblCarrierId.Value))
                            dto.MblCarrierName = tdictionary[mdictionary[dto.MblId].MblCarrierId.Value];
                    }
                    //if (mdictionary[dto.MblId].shipModeId != null) dto.shipModeName = tdictionary[mdictionary[dto.MblId].shipModeId.Value];


                    //港口
                    if (dto.PodId.HasValue && pdictionary.ContainsKey(dto.PodId.Value))
                        dto.PodName = pdictionary[dto.PodId.Value];
                    if (dto.PolId.HasValue && pdictionary.ContainsKey(dto.PolId.Value))
                        dto.PolName = pdictionary[dto.PolId.Value];
                    if (dto.PorId.HasValue && pdictionary.ContainsKey(dto.PorId.Value))
                        dto.PorName = pdictionary[dto.PorId.Value];
                    if (dto.DelId.HasValue && pdictionary.ContainsKey(dto.DelId.Value))
                        dto.DelName = pdictionary[dto.DelId.Value];
                    if (dto.FdestId.HasValue && pdictionary.ContainsKey(dto.FdestId.Value))
                        dto.FdestName = pdictionary[dto.FdestId.Value];

                    list.Add(dto);
                }
            }
            PagedResultDto<OceanExportHblDto> listDto = new PagedResultDto<OceanExportHblDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }
        public async Task<IList<OceanExportHblDto>> QueryListByMidAsync(QueryHblDto query) {
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.CodeValue);
                }
            }
            var OceanExportHbls = await _repository.GetListAsync();
            List<OceanExportHbl> rs;
            List<OceanExportHblDto> list = new List<OceanExportHblDto>();
            if (query != null && query.MblId != null)
            {
                rs = OceanExportHbls.Where(x => x.MblId.Equals(query.MblId.Value)).ToList();
            }
            else
            {
                rs = OceanExportHbls;
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<OceanExportHbl, OceanExportHblDto>(pu);
                    if (pud.CardColorId != null) pud.CardColorValue = dictionary[pud.CardColorId.Value];
                    if(pud.ColorRemarkId != null)pud.ColorRemarkValue = dictionary[pud.ColorRemarkId.Value];
                    list.Add(pud);
                }
            }
            return list;
        }

        public async Task<CreateUpdateOceanExportHblDto> GetHblById(QueryHblDto query)
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
            var oceanExportHbl = await _repository.GetAsync(query.Id.Value);
            var rs = ObjectMapper.Map<OceanExportHbl, CreateUpdateOceanExportHblDto>(oceanExportHbl);
            if (rs.CardColorId != null) rs.CardColorValue = dictionary[rs.CardColorId.Value];
            return rs;
        }

        public async Task<List<OceanExportHblDto>> GetHblCardsById(Guid Id) {
            var data = await _repository.GetListAsync(f => f.MblId == Id);
            var retVal = ObjectMapper.Map<List<OceanExportHbl>, List<OceanExportHblDto>>(data);

            return retVal;
        }

        public async Task<OceanExportHblDto> GetHblCardById(Guid Id)
        {
            if (await _repository.AnyAsync(f => f.Id == Id))
            {
                var data = await _repository.GetAsync(f => f.Id == Id);
                var retVal = ObjectMapper.Map<OceanExportHbl, OceanExportHblDto>(data);
                return retVal;
            }
            return new OceanExportHblDto();
        }
        public async void LockedOrUnLockedOceanExportHblAsync(QueryHblDto query)
        {
            var Hbl = await _repository.GetAsync(query.HblId.Value);
            Hbl.IsLocked = !Hbl.IsLocked;
            await _repository.UpdateAsync(Hbl);
        }

        public async Task<OceanExportHblDto> GetHawbCardById(Guid Id)
        {
            if (await _repository.AnyAsync(f => f.Id == Id))
            {
                var data = await _repository.GetAsync(f => f.Id == Id);
                var retVal = ObjectMapper.Map<OceanExportHbl, OceanExportHblDto>(data);
                return retVal;
            }

            return new OceanExportHblDto();
        }
    }
}