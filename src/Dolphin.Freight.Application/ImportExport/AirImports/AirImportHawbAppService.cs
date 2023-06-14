using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.OceanExports;
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

namespace Dolphin.Freight.ImportExport.AirImports
{
    public class AirImportHawbAppService :
        CrudAppService<
            AirImportHawb,
            AirImportHawbDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateAirImportHawbDto>,
        IAirImportHawbAppService
    {
        IRepository<AirImportHawb, Guid> _repository;
        private IRepository<Airport, Guid> _airportRepository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<AirImportHawb, Guid> _mblRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly IRepository<Port, Guid> _portRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        public AirImportHawbAppService(IRepository<AirImportHawb, Guid> repository,

            IRepository<Airport, Guid> airportRepository,
            IRepository<Substation, Guid> substationRepository,
            IRepository<AirImportHawb, Guid> mblRepository,
             IRepository<SysCode, Guid> sysCodeRepository,
            IRepository<Port, Guid> portRepository,
            IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository) : base(repository)
        {
            _repository = repository;
            _airportRepository = airportRepository;
            _mblRepository = mblRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _substationRepository = substationRepository;
            _sysCodeRepository = sysCodeRepository;
            _airportRepository = airportRepository;
            _portRepository = portRepository;


        }

        public async Task<List<AirImportHawbDto>> GetHblCardsById(Guid Id)
        {
            var data = await _repository.GetListAsync(f => f.MawbId == Id);
            var retVal = ObjectMapper.Map<List<AirImportHawb>, List<AirImportHawbDto>>(data);

            return retVal;
        }

        public async Task<AirImportHawbDto> GetHawbCardById(Guid Id)
        {
            var data = await _repository.GetAsync(f => f.Id == Id);
            var retVal = ObjectMapper.Map<AirImportHawb, AirImportHawbDto>(data);

            return retVal;
        }

        public async Task<List<AirImportHawbDto>> GetDocCenterCardsById(Guid Id)
        {
            var data = await _repository.GetListAsync(f => f.MawbId == Id);
            var retVal = ObjectMapper.Map<List<AirImportHawb>, List<AirImportHawbDto>>(data);

            return retVal;
        }

        public async Task<AirImportHawbDto> GetDocCenterCardById(Guid Id)
        {
            if (await _repository.AnyAsync(x => x.Id == Id))
            {
                var data = await _repository.GetAsync(f => f.Id == Id);
                var retVal = ObjectMapper.Map<AirImportHawb, AirImportHawbDto>(data);
                return retVal;
            }

            return new AirImportHawbDto();
        }

        public async Task<PagedResultDto<AirImportHawbDto>> QueryListAsync(QueryHblDto query)
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
            Dictionary<Guid, AirImportHawb> mdictionary = new();
            if (mbls != null)
            {
                foreach (var mbl in mbls)
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
            var airImportHawbs = await _repository.GetListAsync();
            List<AirImportHawb> rs;
            List<AirImportHawbDto> list = new List<AirImportHawbDto>();
            if (query != null && query.MblId != null)
            {
                rs = airImportHawbs.Where(x => x.Id.Equals(query.MblId.Value)).ToList();
            }
            else
            {
                rs = airImportHawbs;
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<AirImportHawb, AirImportHawbDto>(pu);
                    //dto.FilingNo = mdictionary[dto.FilingNo].FilingNo;
                    //dto.MblNo = mdictionary[dto.MblId].SoNo;
                    //dto.OfficeName = sdictionary[mdictionary[dto.OfficeId.GetValueOrDefault()].OfficeId.Value];
                    ////SysCode
                    //if (mdictionary[dto.MblId].OblTypeId != null) dto.OblTypeName = dictionary[mdictionary[dto.MblId].OblTypeId.Value];

                    ////人
                    //if (dto.AgentId != null) dto.AgentName = tdictionary[dto.AgentId.Value];
                    //if (dto.ShipperId != null) dto.Shipper = tdictionary[dto.ShipperId.Value];
                    //if (dto.HblConsigneeId != null) dto.HblConsigneeName = tdictionary[dto.HblConsigneeId.Value];
                    //if (mdictionary[dto.MblId].MblCarrierId != null) dto.MblCarrierName = tdictionary[mdictionary[dto.MblId].MblCarrierId.Value];
                    ////if (mdictionary[dto.MblId].shipModeId != null) dto.shipModeName = tdictionary[mdictionary[dto.MblId].shipModeId.Value];


                    ////港口
                    //if (dto.PodId != null) dto.PodName = pdictionary[dto.PodId.Value];
                    //if (dto.PolId != null) dto.PolName = pdictionary[dto.PolId.Value];
                    //if (dto.PorId != null) dto.PorName = pdictionary[dto.PorId.Value];
                    //if (dto.DelId != null) dto.DelName = pdictionary[dto.DelId.Value];
                    //if (dto.FdestId != null) dto.FdestName = pdictionary[dto.FdestId.Value];
                    list.Add(dto);
                }
            }
            PagedResultDto<AirImportHawbDto> listDto = new PagedResultDto<AirImportHawbDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }
    }
}
