using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
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
        private readonly IPortsManagementAppService _portRepository;
        private readonly IRepository<AirExportMawb, Guid> _mawbRepository;
        private IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;

        public AirExportHawbAppService(IRepository<AirExportHawb, Guid> repository,
            IRepository<SysCode, Guid> sysCodeRepository,
            IRepository<AirExportHawb, Guid> mblRepository,
            IRepository<Substation, Guid> substationRepository,
            IPortsManagementAppService portRepository,
            IRepository<Airport, Guid> airportRepository,
            IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository,
            IRepository<AirExportMawb, Guid> mawbRepository) : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _mblRepository = mblRepository;
            _substationRepository = substationRepository;
            _portRepository = portRepository;
            _airportRepository = airportRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _mawbRepository = mawbRepository;
        }

        public async Task<PagedResultDto<AirExportHawbDto>> QueryListAsync(QueryHblDto query)
        {
            var airExportHawbs = await _repository.GetQueryableAsync();
            airExportHawbs = airExportHawbs.WhereIf(query.MblId != null && query.MblId != Guid.Empty, x => x.Id
                                           .Equals(query.MblId.Value))
                                           .WhereIf(!string.IsNullOrWhiteSpace(query.Search), x => x.HawbNo
                                           .Contains(query.Search) || x.MawbId.ToString()
                                           .Contains(query.Search));
            List<AirExportHawb> rs = airExportHawbs.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<AirExportHawbDto> list = new List<AirExportHawbDto>();

            if (rs.Any())
            {
                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<AirExportHawb, AirExportHawbDto>(pu);
                    list.Add(dto);
                }
            }
            PagedResultDto<AirExportHawbDto> listDto = new PagedResultDto<AirExportHawbDto>();
            listDto.Items = list;
            listDto.TotalCount = airExportHawbs.Count();
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

        public async Task<AirExportHawbDto> GetHawbWithDetailsById(Guid Id)
        {
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            var portMangements = await _portRepository.QueryListAsync();

            var hawb = await GetHawbCardById(Id);

            if (hawb.SalesId != null)
            {
                var salesPerson = tradePartners.Where(w => w.Id == hawb.SalesId).FirstOrDefault();
                hawb.Sales = new Volo.Abp.Users.UserData() { Id = salesPerson.Id, Name = salesPerson?.TPName + " / " + salesPerson?.TPCode };
            }

            if (hawb.ConsigneeId != null)
            {
                var consignee = tradePartners.Where(w => w.Id == hawb.ConsigneeId).FirstOrDefault();
                hawb.Consignee = string.Concat(consignee.TPName, "/", consignee.TPCode);
            }

            if (hawb.OPId != null)
            {
                var op = tradePartners.Where(w => w.Id == hawb.OPId).FirstOrDefault();
                hawb.OP = new Volo.Abp.Users.UserData() { Id = op.Id, Name = op.TPName + " / " + op.TPCode };
            }

            if (hawb.Customer != null)
            {
                var customer = tradePartners.Where(w => w.Id == Guid.Parse(hawb.Customer)).FirstOrDefault();
                hawb.Customer = string.Concat(customer.TPName, "/", customer.TPCode);
            }

            if (hawb.Trucker != null)
            {
                var trucker = tradePartners.Where(w => w.Id == Guid.Parse(hawb.Trucker)).FirstOrDefault();
                hawb.Trucker = string.Concat(trucker.TPName, "/", trucker.TPCode);
            }

            if (hawb.CargoPickup != null)
            {
                var cargoPickup = tradePartners.Where(w => w.Id == Guid.Parse(hawb.CargoPickup)).FirstOrDefault();
                hawb.CargoPickupName = string.Concat(cargoPickup.TPName, "/", cargoPickup.TPCode);
            }

            if (hawb.OverseaAgent != null)
            {
                var overSeaAgent = tradePartners.Where(w => w.Id == Guid.Parse(hawb.OverseaAgent)).FirstOrDefault();
                hawb.OverseaAgent = string.Concat(overSeaAgent.TPName, "/", overSeaAgent.TPCode);
            }

            if (hawb.DepartureId != null)
            {
                var departure = portMangements.Where(w => Id == hawb.DepartureId).FirstOrDefault();
                hawb.DepartureName = departure?.PortName;
            }

            if (hawb.DestinationId != null)
            {
                var destination = portMangements.Where(w => Id == hawb.DestinationId).FirstOrDefault();
                hawb.DestinationName = destination?.PortName;
            }

            if (hawb.IssuingCarrier != null)
            {
                var issuingCarrier = tradePartners.Where(w => w.Id == Guid.Parse(hawb.IssuingCarrier)).FirstOrDefault();
                hawb.IssuingCarrierName = string.Concat(issuingCarrier.TPName, "/", issuingCarrier.TPCode);
                hawb.IATA = issuingCarrier.IataCode;
            }

            if (hawb.Notify != null)
            {
                var notify = tradePartners.Where(w => w.Id == Guid.Parse(hawb.Notify)).FirstOrDefault();
                hawb.NotifyName = string.Concat(notify.TPName, "/", notify.TPCode);
            }

            if (hawb.ActualShippedr != null)
            {
                var shipper = tradePartners.Where(w => w.Id == Guid.Parse(hawb.ActualShippedr)).FirstOrDefault();
                hawb.ActualShipperName = string.Concat(shipper.TPName, "/", shipper.TPCode);
            }

            if (hawb.BillTo != null)
            {
                var billTo = tradePartners.Where(w => w.Id == Guid.Parse(hawb.BillTo)).FirstOrDefault();
                hawb.BillToName = string.Concat(billTo.TPName, "/", billTo.TPCode);
            }

            if (hawb.ActualShippedr != null)
            {
                var shipper = tradePartners.Where(w => w.Id.ToString() == hawb.ActualShippedr).FirstOrDefault();
                hawb.ActualShippedr = shipper.TPName;
            }

            if (hawb.Notify is not null)
            {
                var notify = tradePartners.Where(w => w.Id.ToString() == hawb.Notify).FirstOrDefault();
                hawb.Notify = string.Concat(notify.TPName, "/", notify.TPCode);
            }

            return hawb;
        }

        public async Task<AirExportDetails> GetAirExportDetailsById(Guid Id)
        {
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            var portMangements = await _portRepository.QueryListAsync();
            var airExportDetails = new AirExportDetails();

            var data = await GetHawbCardById(Id);

            if (data != null)
            {
                airExportDetails = ObjectMapper.Map<AirExportHawbDto, AirExportDetails>(data);

                var mawb = await _mawbRepository.GetAsync(data.MawbId.GetValueOrDefault());

                if (data.SalesId != null)
                {
                    var salesPerson = tradePartners.Where(w => w.Id == data.SalesId).FirstOrDefault();
                    airExportDetails.SalesName = string.Concat(salesPerson.TPName, "/", salesPerson.TPCode);
                }

                if (data.ConsigneeId != null)
                {
                    var consignee = tradePartners.Where(w => w.Id == data.ConsigneeId).FirstOrDefault();
                    airExportDetails.ConsigneeName = string.Concat(consignee.TPName, "/", consignee.TPCode);
                }

                if (data.OPId != null)
                {
                    var op = tradePartners.Where(w => w.Id == data.OPId).FirstOrDefault();
                    airExportDetails.Operator = string.Concat(op.TPName, "/", op.TPCode);
                }

                if (data.Customer != null)
                {
                    var customer = tradePartners.Where(w => w.Id == Guid.Parse(data.Customer)).FirstOrDefault();
                    airExportDetails.CustomerName = string.Concat(customer.TPName, "/", customer.TPCode);
                }

                if (data.Trucker != null)
                {
                    var trucker = tradePartners.Where(w => w.Id == Guid.Parse(data.Trucker)).FirstOrDefault();
                    airExportDetails.TruckerName = string.Concat(trucker.TPName, "/", trucker.TPCode);
                }

                if (data.CargoPickup != null)
                {
                    var cargoPickup = tradePartners.Where(w => w.Id == Guid.Parse(data.CargoPickup)).FirstOrDefault();
                    airExportDetails.CargoPickupName = string.Concat(cargoPickup.TPName, "/", cargoPickup.TPCode);
                }

                if (data.OverseaAgent != null)
                {
                    var overSeaAgent = tradePartners.Where(w => w.Id == Guid.Parse(data.OverseaAgent)).FirstOrDefault();
                    airExportDetails.OverSeaAgentName = string.Concat(overSeaAgent.TPName, "/", overSeaAgent.TPCode);
                }

                if (data.DepartureId != null)
                {
                    var departure = portMangements.Where(w => w.Id == data.DepartureId).FirstOrDefault();
                    airExportDetails.DepartureName = departure?.PortName;
                }

                if (data.DestinationId != null)
                {
                    var destination = portMangements.Where(w => w.Id == data.DestinationId).FirstOrDefault();
                    airExportDetails.DestinationName = destination?.PortName;
                }

                if (data.IssuingCarrier != null)
                {
                    var issuingCarrier = tradePartners.Where(w => w.Id == Guid.Parse(data.IssuingCarrier)).FirstOrDefault();
                    airExportDetails.IssuingCarrierName = string.Concat(issuingCarrier.TPName, "/", issuingCarrier.TPCode);
                    airExportDetails.IATA = issuingCarrier.IataCode;
                }

                if (data.Notify != null)
                {
                    var notify = tradePartners.Where(w => w.Id == Guid.Parse(data.Notify)).FirstOrDefault();
                    airExportDetails.NotifyName = string.Concat(notify.TPName, "/", notify.TPCode);
                }

                if (data.ActualShippedr != null)
                {
                    var shipper = tradePartners.Where(w => w.Id == Guid.Parse(data.ActualShippedr)).FirstOrDefault();
                    airExportDetails.ShippperName = string.Concat(shipper.TPName, "/", shipper.TPCode);
                }

                if (data.BillTo != null)
                {
                    var billTo = tradePartners.Where(w => w.Id == Guid.Parse(data.BillTo)).FirstOrDefault();
                    airExportDetails.BillToName = string.Concat(billTo.TPName, "/", billTo.TPCode);
                }

                airExportDetails.AirWayBillNo = data.HawbNo;
                airExportDetails.MawbNo = mawb.MawbNo ?? "";
                airExportDetails.DocNumber = mawb.FilingNo;
                airExportDetails.GrossWeight = data.GrossWeightCneeKG;
                airExportDetails.HandlingInformation = data.HandlingInformation;
                airExportDetails.ArrivalDate = mawb.ArrivalDate?.ToShortDateString();
                airExportDetails.NVD = data.DVCarriage;
                airExportDetails.NCV = data.DVCustoms;
                airExportDetails.ChargableWeight = string.Concat(data.ChargeableWeightCneeKG, " ", data.ChargeableWeightCneeLB);
                airExportDetails.Package = data.Package;
                airExportDetails.ChargeableWeightCneeKG = data.ChargeableWeightCneeKG;
                airExportDetails.ChargeableWeightCneeLB = data.ChargeableWeightCneeLB;
                airExportDetails.DepatureDate = mawb.DepatureDate;
            }

            return airExportDetails;
        }

        public async Task LockedOrUnLockedAirExportHawbAsync(Guid id)
        {
            try
            {
                var mbl = await _repository.GetAsync(id);
                mbl.IsLocked = !mbl.IsLocked;
                await _repository.UpdateAsync(mbl);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }
    }
}
