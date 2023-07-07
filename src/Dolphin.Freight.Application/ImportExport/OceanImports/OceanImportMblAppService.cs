using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Common;
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

namespace Dolphin.Freight.ImportExport.OceanImports
{
    public class OceanImportMblAppService :
        CrudAppService<
            OceanImportMbl, //IT號碼管理entity
            OceanImportMblDto, //顯示IT號碼管理用
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateOceanImportMblDto>, //新增修改IT號碼管理用
        IOceanImportMblAppService //實作IOceanImportMblAppService
    {
        private readonly IRepository<OceanImportMbl, Guid> _repository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly IRepository<Port, Guid> _portRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IRepository<OceanImportHbl, Guid> _oceanImportHblRepository;
        public OceanImportMblAppService(IRepository<OceanImportMbl, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<Substation, Guid> substationRepository, 
                                        IRepository<Port, Guid> portRepository, IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository ,
                                        IRepository<OceanImportHbl, Guid> oceanImportHblRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _substationRepository =  substationRepository;
            _portRepository = portRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _oceanImportHblRepository = oceanImportHblRepository;
            /*
            GetPolicyName = OceanImportPermissions.OceanImportMbls.Default;
            GetListPolicyName = OceanImportPermissions.OceanImportMbls.Default;
            CreatePolicyName = OceanImportPermissions.OceanImportMbls.Create;
            UpdatePolicyName = OceanImportPermissions.OceanImportMbls.Edit;
            DeletePolicyName = OceanImportPermissions.OceanImportMbls.Delete;*/
        }
        public async Task<PagedResultDto<OceanImportMblDto>> QueryListAsync(QueryMblDto query)
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
            var OceanImportMbls = await _repository.GetListAsync();
            List<OceanImportMbl> rs;
            List<OceanImportMblDto> list = new List<OceanImportMblDto>();
            if (query != null && query.QueryKey != null)
            {
                rs = OceanImportMbls.OrderByDescending(x=>x.CreationTime ).ToList();
            }
            else
            {
                rs = OceanImportMbls.OrderByDescending(x => x.CreationTime).ToList();
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var r in rs)
                {
                    var item = ObjectMapper.Map<OceanImportMbl, OceanImportMblDto>(r);
                    item.OfficeName = substationsDictionary[r.OfficeId.Value];
                    list.Add(item);
                }
            }
            PagedResultDto<OceanImportMblDto> listDto = new PagedResultDto<OceanImportMblDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }
        public async void LockedOrUnLockedOceanImportMblAsync(QueryMblDto query) 
        {
            var mbl = await _repository.GetAsync(query.MbId.Value);
            mbl.IsLocked = !mbl.IsLocked;
            await _repository.UpdateAsync(mbl);
        }
        public async Task<CreateUpdateOceanImportMblDto> GetCreateUpdateOceanImportMblDtoById(Guid Id) {
            var oceanImportMbl = await _repository.GetAsync(Id,true);
            var dto = ObjectMapper.Map<OceanImportMbl, CreateUpdateOceanImportMblDto>(oceanImportMbl);
            var ports = await _portRepository.GetListAsync();
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
        public override async Task DeleteAsync(Guid Id)
        {
            var hbls = await _oceanImportHblRepository.GetListAsync();
            var ids = hbls.Where(w => w.MblId == Id).Select(s => s.Id);

            await Repository.DeleteAsync(Id);
            await _oceanImportHblRepository.DeleteManyAsync(ids);
        }
    }
}