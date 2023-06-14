using Dolphin.Freight.ImportExport.AirImports;
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

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class AirExportHawbAppService :
        CrudAppService<
            AirExportHawb,
            AirExportHawbDto,
            Guid, //Primary key 
            PagedAndSortedResultRequestDto, //Used for paging & sorting
            CreateUpdateAirExportHawbDto>, // Create & Update
        IAirExportHawbAppService // implement airport service
    {
        private IRepository<AirExportHawb, Guid> _repository;
        private IRepository<Airport, Guid> _airportRepository;
        private readonly IRepository<AirExportHawb, Guid> _mblRepository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly IRepository<Port, Guid> _portRepository;
        private IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        public AirExportHawbAppService(IRepository<AirExportHawb, Guid> repository,
            IRepository<SysCode, Guid> sysCodeRepository,
            IRepository<AirExportHawb, Guid> mblRepository,
            IRepository<Substation, Guid> substationRepository,
            IRepository<Port, Guid> portRepository,
            IRepository<Airport, Guid> airportRepository,
            IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository) : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _mblRepository = mblRepository;
            _substationRepository = substationRepository;
            _portRepository = portRepository;
            _airportRepository = airportRepository;
            _tradePartnerRepository = tradePartnerRepository;
        }

        public async Task<PagedResultDto<AirExportHawbDto>> QueryListAsync(QueryHblDto query)
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
            Dictionary<Guid, AirExportHawb> mdictionary = new();
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
            var airExportHawbs = await _repository.GetListAsync();
            List<AirExportHawb> rs;
            List<AirExportHawbDto> list = new List<AirExportHawbDto>();
            if (query != null && query.MblId != null)
            {
                rs = airExportHawbs.Where(x => x.Id.Equals(query.MblId.Value)).ToList();
            }
            else
            {
                rs = airExportHawbs;
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<AirExportHawb, AirExportHawbDto>(pu);
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
            PagedResultDto<AirExportHawbDto> listDto = new PagedResultDto<AirExportHawbDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }

        public override async Task<PagedResultDto<AirExportHawbDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            // airport
            Dictionary<Guid, string> airportDictionary = new Dictionary<Guid, string>();
            var airportList = await _airportRepository.GetListAsync();
            if (null != airportList)
            {
                foreach (var airport in airportList)
                {
                    airportDictionary.Add(airport.Id, airport.AirportName);
                }
            }

            // tradepartner
            Dictionary<Guid, string> tradePartnerDictionary = new Dictionary<Guid, string>();
            var tradePartnerList = await _tradePartnerRepository.GetListAsync();
            if (null != tradePartnerList && tradePartnerList.Count > 0)
            {
                foreach (var tradePartner in tradePartnerList)
                {
                    tradePartnerDictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }

            var queryable = await Repository.GetQueryableAsync();
            var query = queryable
                .OrderBy(x => x.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var airExportHawbList = await AsyncExecuter.ToListAsync(query);
            List<AirExportHawbDto> airExportHawbDtoList = new List<AirExportHawbDto>();

            if (null != airExportHawbList && airExportHawbList.Count > 0)
            {
                foreach (var airExportHawb in airExportHawbList)
                {
                    var airExportHawbDto = ObjectMapper.Map<AirExportHawb, AirExportHawbDto>(airExportHawb);

                    airExportHawbDtoList.Add(airExportHawbDto);
                }
            }

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<AirExportHawbDto>(
                totalCount,
                airExportHawbDtoList
            );
        }

        public async Task<AirExportHawbDto> GetDocCenterCardById(Guid Id)
        {
            if (await _repository.AnyAsync(f => f.Id == Id))
            {
                var data = await _repository.GetAsync(f => f.Id == Id);
                var retVal = ObjectMapper.Map<AirExportHawb, AirExportHawbDto>(data);
                return retVal;
            }
            return new AirExportHawbDto();
        }

        public async Task<List<AirExportHawbDto>> GetDocCenterCardsById(Guid Id)
        {
            var data = await _repository.GetListAsync(f => f.MawbId == Id);
            var retVal = ObjectMapper.Map<List<AirExportHawb>, List<AirExportHawbDto>>(data);

            return retVal;
        }

        public async Task<List<AirExportHawbDto>> GetHblCardsById(Guid Id)
        {
            var data = await _repository.GetListAsync(f => f.MawbId == Id);
            var retVal = ObjectMapper.Map<List<AirExportHawb>, List<AirExportHawbDto>>(data);

            return retVal;
        }

        public async Task<AirExportHawbDto> GetHawbCardById(Guid Id)
        {
            if (await _repository.AnyAsync(f => f.Id == Id))
            {
                var data = await _repository.GetAsync(f => f.Id == Id);
                var retVal = ObjectMapper.Map<AirExportHawb, AirExportHawbDto>(data);
                return retVal;
            }

            return new AirExportHawbDto();
        }
    }
}
