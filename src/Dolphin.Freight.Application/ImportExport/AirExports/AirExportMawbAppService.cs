﻿
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class AirExportMawbAppService :
        CrudAppService<
            AirExportMawb, // AirExportMawb entity
            AirExportMawbDto, // show
            Guid, //Primary key 
            PagedAndSortedResultRequestDto, //Used for paging & sorting
            CreateUpdateAirExportMawbDto>, // Create & Update
        IAirExportMawbAppService // implement airport service
    {
        private IRepository<AirExportMawb, Guid> _repository;
        private IRepository<Airport, Guid> _airportRepository;
         private readonly IRepository<AirExportMawb, Guid> _mblRepository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly IRepository<Port, Guid> _portRepository;
        private IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;

        public AirExportMawbAppService(
            IRepository<AirExportMawb, Guid> repository,
            IRepository<SysCode, Guid> sysCodeRepository, 
            IRepository<AirExportMawb, Guid> mblRepository, 
            IRepository<Substation, Guid> substationRepository, 
            IRepository<Port, Guid> portRepository, 
            IRepository<Airport, Guid> airportRepository,
            IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository
        ) : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _mblRepository = mblRepository;
            _substationRepository = substationRepository;
            _portRepository = portRepository;
            _airportRepository = airportRepository;
            _tradePartnerRepository = tradePartnerRepository;
        }

    public async Task<PagedResultDto<AirExportMawbDto>> QueryListAsync(QueryHblDto query)
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
            Dictionary<Guid, AirExportMawb> mdictionary = new();
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
            var airExportMawbs = await _repository.GetListAsync();
            List<AirExportMawb> rs;
            List<AirExportMawbDto> list = new List<AirExportMawbDto>();
            if (query != null && query.MblId != null)
            {
                rs = airExportMawbs.Where(x => x.Id.Equals(query.MblId.Value)).ToList();
            }
            else
            {
                rs = airExportMawbs;
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<AirExportMawb, AirExportMawbDto>(pu);
                    //dto.FilingNo = mdictionary[dto.FilingNo].FilingNo;
                    //dto.MblNo = mdictionary[dto.MblId].SoNo;
                    //dto.OfficeName = sdictionary[mdictionary[dto.OfficeId.GetValueOrDefault()].OfficeId.Value];
                    ////SysCode
                    //if (mdictionary[dto.MblId].OblTypeId != null) dto.OblTypeName = dictionary[mdictionary[dto.MblId].OblTypeId.Value];

                    ////人
                    //if (dto.AgentId != null) dto.AgentName = tdictionary[dto.AgentId.Value];
                    if (dto.ShipperId != null) dto.Shipper = tdictionary[dto.ShipperId.Value];
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
            PagedResultDto<AirExportMawbDto> listDto = new PagedResultDto<AirExportMawbDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }

        public override async Task<PagedResultDto<AirExportMawbDto>> GetListAsync(PagedAndSortedResultRequestDto input)
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
            var airExportMawbList = await AsyncExecuter.ToListAsync(query);
            List<AirExportMawbDto> airExportMawbDtoList = new List<AirExportMawbDto>();
            if (null != airExportMawbList && airExportMawbList.Count > 0)
            {
                foreach (var airExportMawb in airExportMawbList)
                {
                    var airExportMawbDto = ObjectMapper.Map<AirExportMawb, AirExportMawbDto>(airExportMawb);
                    if (airExportMawb.DepatureId != null)
                    {
                        airExportMawbDto.DepatureAirportName = airportDictionary[airExportMawb.DepatureId.Value];
                    }
                    else
                    {
                        airExportMawbDto.DepatureAirportName = null;
                    }
                    if (airExportMawb.DestinationId != null)
                    {
                        airExportMawbDto.DestinationAirportName = airportDictionary[airExportMawb.DestinationId.Value];
                    }
                    else
                    {
                        airExportMawbDto.DestinationAirportName = null;
                    }

                    airExportMawbDtoList.Add(airExportMawbDto);
                }
            }

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<AirExportMawbDto>(
                totalCount,
                airExportMawbDtoList
            );
        }
    }
}
