using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settings.PortsManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Dolphin.Freight.TradePartners;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public class AirImportMawbAppService :
        CrudAppService<
            AirImportMawb,
            AirImportMawbDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateAirImportMawbDto>,
        IAirImportMawbAppService
    {
        private IRepository<AirImportMawb, Guid> _repository;
        private IRepository<Airport, Guid> _airportRepository;
        private IRepository<PortsManagement, Guid> _portsManagementAppService;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IRepository<AirImportHawb, Guid> _airImportHawbAppService;

        public AirImportMawbAppService(
            IRepository<AirImportMawb, Guid> repository,
            IRepository<Airport, Guid> airportRepository,
            IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository,
            IRepository<PortsManagement, Guid> portsManagementAppService,
            IRepository<AirImportHawb, Guid> airImportHawbAppService
            ) : base(repository)
        {
            _repository = repository;
            _airportRepository = airportRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _portsManagementAppService = portsManagementAppService;
            _airImportHawbAppService = airImportHawbAppService;
        }

        public override async Task<PagedResultDto<AirImportMawbDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            // port management
            Dictionary<Guid, string> portManagementDictionary = new Dictionary<Guid, string>();
            var portManagementList = await _portsManagementAppService.GetListAsync();
            if (null != portManagementList) 
            {
                foreach (var portsManagement in portManagementList)
                {
                    portManagementDictionary.Add(portsManagement.Id, portsManagement.PortName);
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
                .OrderBy(x=>x.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);
            var airImportMawbList = await AsyncExecuter.ToListAsync(query);
            List<AirImportMawbDto> airImportMawbDtoList = new List<AirImportMawbDto>();
            if (null != airImportMawbList && airImportMawbList.Count > 0)
            {
                foreach (var airImportMawb in airImportMawbList)
                {
                    var airImportMawbDto = ObjectMapper.Map<AirImportMawb, AirImportMawbDto>(airImportMawb);
                    if (airImportMawb.DepatureId != null)
                    {
                        airImportMawbDto.DepatureAirportName = portManagementDictionary[airImportMawb.DepatureId.Value];
                    }
                    else
                    {
                        airImportMawbDto.DepatureAirportName = null;
                    }
                    if (airImportMawb.DestinationId != null)
                    {
                        airImportMawbDto.DestinationAirportName = portManagementDictionary[airImportMawb.DestinationId.Value];
                    }
                    else
                    {
                        airImportMawbDto.DestinationAirportName = null;
                    }
                    if (airImportMawb.OverseaAgentId != null)
                    {
                        airImportMawbDto.OverseaAgentTPName = tradePartnerDictionary[airImportMawb.OverseaAgentId.Value];
                    }
                    else
                    {
                        airImportMawbDto.OverseaAgentTPName = null;
                    }
                    if (airImportMawb.CarrierId != null)
                    {
                        airImportMawbDto.CarrierTPName = tradePartnerDictionary[airImportMawb.CarrierId.Value];
                    }
                    else
                    {
                        airImportMawbDto.CarrierTPName = null;
                    }

                    airImportMawbDtoList.Add(airImportMawbDto);
                }
            }

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<AirImportMawbDto>(
                totalCount,
                airImportMawbDtoList
            );
        }

        public override async Task DeleteAsync(Guid Id)
        {
            var hawbs = await _airImportHawbAppService.GetListAsync();
            var ids = hawbs.Where(h => h.MawbId == Id).Select(s => s.Id);

            await Repository.DeleteAsync(Id);
            await _airImportHawbAppService.DeleteManyAsync(ids);
        }

        public async Task<AirImportDetails> GetAirImportDetailsById(Guid Id)
        {
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            var portMangements = await _portsManagementAppService.GetListAsync();

            var data = await Repository.GetAsync(Id);

            var airImportDetails = ObjectMapper.Map<AirImportMawb, AirImportDetails>(data);

            if (data.ConsigneeId != null)
            {
                var consignee = tradePartners.Where(w => w.Id == data.ConsigneeId).FirstOrDefault();
                airImportDetails.ConsigneeName = string.Concat(consignee.TPName, "/", consignee.TPCode);
            }

            if (data.DepatureId != null)
            {
                var departure = portMangements.Where(w => w.Id == data.DepatureId).FirstOrDefault();
                airImportDetails.DepatureName = departure?.PortName;
            }

            if (data.DestinationId != null)
            {
                var destination = portMangements.Where(w => w.Id == data.DestinationId).FirstOrDefault();
                airImportDetails.DestinationAirportName = destination?.PortName;
                airImportDetails.DestinationCountry = destination?.Country;
            }

            if (data.NotifyId != null)
            {
                var notify = tradePartners.Where(w => w.Id == data.NotifyId).FirstOrDefault();
                airImportDetails.NotifyName = string.Concat(notify.TPName, "/", notify.TPCode);
            }

            if (data.ShipperId != null)
            {
                var shipper = tradePartners.Where(w => w.Id == data.ShipperId).FirstOrDefault();
                airImportDetails.ShipperName = string.Concat(shipper.TPName, "/", shipper.TPCode);
            }

            if (data.OverseaAgentId != null)
            {
                var overseaAgent = tradePartners.Where(w => w.Id == data.OverseaAgentId).FirstOrDefault();
                airImportDetails.OverseaAgentTPName = string.Concat(overseaAgent.TPName, "/", overseaAgent.TPCode);
                airImportDetails.IATA = overseaAgent.IataCode;
            }

            if (data.CarrierId != null)
            {
                var carrier = tradePartners.Where(w => w.Id == data.CarrierId).FirstOrDefault();
                airImportDetails.CarrierTPName = string.Concat(carrier.TPName, "/", carrier.TPCode);
            }

            if (data.FreightLocationId != null)
            {
                var freightLocation = tradePartners.Where(w => w.Id == data.FreightLocationId).FirstOrDefault();
                airImportDetails.FreightLocationName = string.Concat(freightLocation.TPName, "/", freightLocation.TPCode);
            }

            airImportDetails.AirWayBillNo = data.MawbNo;
            airImportDetails.MawbNo = airImportDetails.MawbNo;
            airImportDetails.DocNumber = data.FilingNo;
            airImportDetails.ChargableWeight = string.Concat(data.ChargeableWeightKg, " ", data.ChargeableWeightLb);
            airImportDetails.DepatureDate = data.DepatureDate;
            airImportDetails.OPName = string.Concat(CurrentUser.Name, " ", CurrentUser.SurName);
            airImportDetails.MawbId = data.Id;


            return airImportDetails;
        }
        public async Task<AirImportMawbDto> GetAirImportMawbDetailsById(Guid Id)
        {
            var tradePartner = await _tradePartnerRepository.GetListAsync();
            var portManagement = await _portsManagementAppService.GetListAsync();

            var data = await Repository.GetAsync(Id);

            var airImportDetails = ObjectMapper.Map<AirImportMawb, AirImportMawbDto>(data);

            if(data.DepatureId is not null)
            {
                var departure = portManagement.Where(w => w.Id == data.DepatureId).FirstOrDefault();
                airImportDetails.DepartureName = departure?.PortName;
            }

            if(data.DestinationId is not null)
            {
                var destination = portManagement.Where(w => w.Id == data.DestinationId).FirstOrDefault();
                airImportDetails.DestinationName = destination?.PortName;
            }
             
            if (data.CarrierId is not null)
            {
                var carrier = tradePartner.Where(w => w.Id == data.CarrierId).FirstOrDefault();
                airImportDetails.CarrierName = string.Concat(carrier.TPName, "/", carrier.TPCode);
            }

            airImportDetails.MawbNo = data.MawbNo;
            airImportDetails.FilingNo = data.FilingNo;
            airImportDetails.FlightNo = data.FlightNo;
            airImportDetails.DepatureDate = data.DepatureDate;
            airImportDetails.ArrivalDate = data.ArrivalDate;

            return airImportDetails;
        }

    }
}
