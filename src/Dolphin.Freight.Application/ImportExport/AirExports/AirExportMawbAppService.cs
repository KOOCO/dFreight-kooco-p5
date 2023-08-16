using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Common;
using Volo.Abp;

namespace Dolphin.Freight.ImportExport.AirExports
{
    public class AirExportMawbAppService :
        CrudAppService<
            AirExportMawb, // AirExportMawb entity
            AirExportMawbDto, // show
            Guid, //Primary key 
            QueryHblDto, //Used for paging & sorting
            CreateUpdateAirExportMawbDto>, // Create & Update
        IAirExportMawbAppService // implement airport service
    {
        private IRepository<AirExportMawb, Guid> _repository;
        private IRepository<Airport, Guid> _airportRepository;
        private readonly IRepository<AirExportMawb, Guid> _mblRepository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly IPortsManagementAppService _portRepository;
        private IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IRepository<AirExportHawb, Guid> _airExportHawbRepository;

        public AirExportMawbAppService(
            IRepository<AirExportMawb, Guid> repository,
            IRepository<SysCode, Guid> sysCodeRepository,
            IRepository<AirExportMawb, Guid> mblRepository,
            IRepository<Substation, Guid> substationRepository,
            IPortsManagementAppService portRepository,
            IRepository<Airport, Guid> airportRepository,
            IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository,
            IRepository<AirExportHawb, Guid> airExportHawbRepository
        ) : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _mblRepository = mblRepository;
            _substationRepository = substationRepository;
            _portRepository = portRepository;
            _airportRepository = airportRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _airExportHawbRepository = airExportHawbRepository; 
        }

        public async Task<PagedResultDto<AirExportMawbDto>> QueryListAsync(QueryHblDto query)
        {
           
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
           
            var airExportMawbs = await _repository.GetQueryableAsync();
            airExportMawbs = airExportMawbs.WhereIf(query.MblId != null && query.MblId != Guid.Empty, x => x.Id
                                           .Equals(query.MblId))
                                           .WhereIf(!string.IsNullOrWhiteSpace(query.Search), x => x.MawbNo
                                           .Contains(query.Search) || x.Shipper.TPName
                                           .Contains(query.Search) || x.FilingNo
                                           .Contains(query.Search) || x.ConsigneeId
                                           .ToString().Contains(query.Search));

            List<AirExportMawb> rs = airExportMawbs.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<AirExportMawbDto> list = new List<AirExportMawbDto>();
            if (rs.Any())
            {
                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<AirExportMawb, AirExportMawbDto>(pu);
                    if (dto.ShipperId != null) dto.Shipper = tdictionary[dto.ShipperId.Value];
                    list.Add(dto);
                }
            }
            PagedResultDto<AirExportMawbDto> listDto = new PagedResultDto<AirExportMawbDto>();
            listDto.Items = list;
            listDto.TotalCount = airExportMawbs.Count();
            return listDto;
        }

        public override async Task<PagedResultDto<AirExportMawbDto>> GetListAsync(QueryHblDto input)
        {
            // ports management
            var ports = await _portRepository.QueryListAsync();
            Dictionary<Guid, string> pdictionary = new();
            if (ports != null && ports.Count > 0)
            {
                foreach (var port in ports)
                {
                    pdictionary.Add(port.Id, port.SubDiv + " " + port.PortName + " ( " + port.Locode + " ) ");
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
            queryable = queryable.WhereIf(input.MblId != null && input.MblId != Guid.Empty, x => x.Id
                                           .Equals(input.MblId))
                                           .WhereIf(!string.IsNullOrWhiteSpace(input.Search), x => x.MawbNo
                                           .Contains(input.Search) || x.Shipper.TPName
                                           .Contains(input.Search) || x.FilingNo
                                           .Contains(input.Search) || x.ConsigneeId
                                           .ToString().Contains(input.Search));
            var query = queryable
                .OrderBy(x => x.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);
            Volo.Abp.Linq.IAsyncQueryableExecuter asyncExecuter = AsyncExecuter;
            var airExportMawbList = await asyncExecuter.ToListAsync(query);
            List<AirExportMawbDto> airExportMawbDtoList = new List<AirExportMawbDto>();

            string departureName, destinationName;

            if (null != airExportMawbList && airExportMawbList.Count > 0)
            {
                foreach (var airExportMawb in airExportMawbList)
                {
                    var airExportMawbDto = ObjectMapper.Map<AirExportMawb, AirExportMawbDto>(airExportMawb);

                    pdictionary.TryGetValue(airExportMawb.DepatureId.GetValueOrDefault(), out departureName);
                    airExportMawbDto.DepatureAirportName = departureName;

                    pdictionary.TryGetValue(airExportMawb.DestinationId.GetValueOrDefault(), out destinationName);
                    airExportMawbDto.DestinationAirportName = destinationName;

                    airExportMawbDtoList.Add(airExportMawbDto);
                }
            }

            //Get the total count with another query
            var totalCount = queryable.Count();

            return new PagedResultDto<AirExportMawbDto>(
                totalCount,
                airExportMawbDtoList
            );
        }

        public override async Task DeleteAsync(Guid Id)
        {
            var hawbs = await _airExportHawbRepository.GetListAsync();
            var ids = hawbs.Where(w => w.MawbId == Id).Select(s => s.Id);

            await Repository.DeleteAsync(Id);

            await _airExportHawbRepository.DeleteManyAsync(ids);
        }

        public async Task<AirExportDetails> GetAirExportDetailsById(Guid Id)
        {
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            var portMangements = await _portRepository.QueryListAsync();

            var data = await Repository.GetAsync(Id);

            var airExportDetails = ObjectMapper.Map<AirExportMawb, AirExportDetails>(data);

            if (data.ConsigneeId != null)
            {
                var consignee = tradePartners.Where(w => w.Id == data.ConsigneeId).FirstOrDefault();
                airExportDetails.ConsigneeName = string.Concat(consignee.TPName, "/", consignee.TPCode);
            }

            if (data.DepatureId != null)
            {
                var departure = portMangements.Where(w => w.Id == data.DepatureId).FirstOrDefault();
                airExportDetails.DepartureName = departure?.PortName;
            }

            if (data.DestinationId != null)
            {
                var destination = portMangements.Where(w => w.Id == data.DestinationId).FirstOrDefault();
                airExportDetails.DestinationName = destination?.PortName;
                airExportDetails.DestinationCountry = destination?.Country;
            }

            if (data.NotifyId != null)
            {
                var notify = tradePartners.Where(w => w.Id == data.NotifyId).FirstOrDefault();
                airExportDetails.NotifyName = string.Concat(notify.TPName, "/", notify.TPCode);
            }

            if (data.ShipperId != null)
            {
                var shipper = tradePartners.Where(w => w.Id == data.ShipperId).FirstOrDefault();
                airExportDetails.ShippperName = string.Concat(shipper.TPName, "/", shipper.TPCode);
            }

            if (data.IssuingCarrierId != null)
            {
                var issuingCarrier = tradePartners.Where(w => w.Id == data.IssuingCarrierId).FirstOrDefault();
                airExportDetails.IssuingCarrierName = string.Concat(issuingCarrier.TPName, "/", issuingCarrier.TPCode);
                airExportDetails.IATA = issuingCarrier.IataCode;
            }

            if (data.MawbCarrierId != null)
            {
                var carrier = tradePartners.Where(w => w.Id == data.MawbCarrierId).FirstOrDefault();
                airExportDetails.CarrierName = string.Concat(carrier.TPName, "/", carrier.TPCode);
            }

            airExportDetails.AirWayBillNo = data.MawbNo;
            airExportDetails.MawbNo = airExportDetails.MawbNo;
            airExportDetails.DocNumber = data.FilingNo;
            airExportDetails.GrossWeight = Convert.ToString(data.AwbChargeableWeightKg);
            airExportDetails.ArrivalDate = data.ArrivalDate?.ToShortDateString();
            airExportDetails.NVD = data.DVCarriage;
            airExportDetails.NCV = data.DVCustomer;
            airExportDetails.ChargableWeight = string.Concat(data.ChargeableWeightKg, " ", data.ChargeableWeightLb);
            airExportDetails.Package = Convert.ToString(data.Package);
            airExportDetails.DepatureDate = data.DepatureDate;
            airExportDetails.ChargeableWeightCneeKG = Convert.ToString(data.ChargeableWeightKg);
            airExportDetails.ChargeableWeightCneeLB = Convert.ToString(data.ChargeableWeightLb);
            airExportDetails.Operator = string.Concat(CurrentUser.Name, " ", CurrentUser.SurName);

            return airExportDetails;
        }

        public async Task LockedOrUnLockedAirExportMawbAsync(Guid id)
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
