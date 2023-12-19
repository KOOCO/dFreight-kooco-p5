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
using Microsoft.AspNetCore.Mvc;
using Dolphin.Freight.Accounting.Invoices;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.Settings.PackageUnits;
using Microsoft.EntityFrameworkCore;

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
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IRepository<PackageUnit, Guid> _packageUnitRepository;

        public AirExportMawbAppService(
            IRepository<AirExportMawb, Guid> repository,
            IRepository<SysCode, Guid> sysCodeRepository,
            IRepository<AirExportMawb, Guid> mblRepository,
            IRepository<Substation, Guid> substationRepository,
            IPortsManagementAppService portRepository,
            IRepository<Airport, Guid> airportRepository,
            IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository,
            IRepository<AirExportHawb, Guid> airExportHawbRepository,
            IInvoiceAppService invoiceAppService,
            IRepository<PackageUnit, Guid> packageUnitRepository
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
            _invoiceAppService = invoiceAppService;
            _packageUnitRepository = packageUnitRepository;
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
                                           .ToString().Contains(query.Search))
                                           .WhereIf(query.CarrierId.HasValue, e => e.AwbAcctCarrierId == query.CarrierId)
                                           .WhereIf(query.ConsigneeId.HasValue, e => e.ConsigneeId == query.ConsigneeId)
                                           .WhereIf(query.ShipperId.HasValue, e => e.ShipperId == query.ShipperId)
                                           .WhereIf(query.DestinationId.HasValue, e => e.DestinationId == query.DestinationId)
                                           .WhereIf(query.DestinationId.HasValue, e => e.DepatureId == query.DepatureId)
                                           .WhereIf(!string.IsNullOrWhiteSpace(query.FlightNo), x => x.FlightNo == query.FlightNo)
                                           .WhereIf(query.OfficeId.HasValue, e => e.OfficeId == query.OfficeId)
                                           .WhereIf(query.IncotermsType.HasValue,e=>e.IncotermsType==query.IncotermsType)
                                           .WhereIf(query.AwbCancelled.HasValue, e => e.IsAwbCancelled == query.AwbCancelled)
                                           .WhereIf(query.Block.HasValue, e => e.IsLocked == query.Block)
                                           .WhereIf(query.AwbType.HasValue, e => e.AwbType == query.AwbType)
                                           .WhereIf(query.DirectMaster.HasValue, e => e.IsDirectMaster == query.DirectMaster)
                                           .WhereIf(query.PostDate.HasValue, e => e.PostDate.Date == query.PostDate.Value.Date.AddDays(1))
                                           .WhereIf(query.DepatureDate.HasValue, e => e.DepatureDate.Date == query.DepatureDate.Value.Date.AddDays(1))
                                           .WhereIf(query.ArrivalDate.HasValue, e => e.ArrivalDate.Value.Date == query.ArrivalDate.Value.Date.AddDays(1))
                                           .WhereIf(query.CreationDate.HasValue, e => e.CreationTime.Date == query.CreationDate.Value.Date.AddDays(1))
                                           .OrderByDescending(x => x.CreationTime);

            List<AirExportMawb> rs = airExportMawbs.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<AirExportMawbDto> list = new List<AirExportMawbDto>();
            if (rs.Any())
            {
                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<AirExportMawb, AirExportMawbDto>(pu);
                    if (dto.ShipperId != null) dto.Shipper = tdictionary[dto.ShipperId.Value];
                    var hawbs = await _airExportHawbRepository.GetListAsync();
                    dto.HawbNos = string.Join(", ", hawbs.Where(w => w.MawbId == dto.Id).Select(s => s.HawbNo));
                    var queryType = 0;

                    QueryInvoiceDto queryDto = new QueryInvoiceDto() { QueryType = queryType, ParentId = dto.Id };

                    dto.Invoices = (await _invoiceAppService.QueryInvoicesAsync(queryDto)).ToList();

                    if (dto.Invoices is not null && dto.Invoices.Count > 0)
                    {
                        dto.AR = new List<InvoiceDto>();
                        dto.DC = new List<InvoiceDto>();
                        dto.AP = new List<InvoiceDto>();
                        foreach (var dto1 in dto.Invoices)
                        {
                            switch (dto1.InvoiceType)
                            {
                                default:
                                    dto.AR.Add(dto1);
                                    break;
                                case 1:
                                    dto.DC.Add(dto1);
                                    break;
                                case 2:
                                    dto.AP.Add(dto1);
                                    break;
                            }
                        }

                        if (dto.AR.Any())
                        {
                            double arTotal = 0;
                            foreach (var ar in dto.AR)
                            {
                                arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }
                            dto.ARTotal = arTotal;
                        }
                        if (dto.AP.Any())
                        {
                            double apTotal = 0;
                            foreach (var ap in dto.AP)
                            {
                                apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }

                            dto.APTotal = apTotal;
                        }
                        if (dto.DC.Any())
                        {
                            double dcTotal = 0;
                            foreach (var dc in dto.DC)
                            {
                                dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }
                            dto.DCTotal = dcTotal;
                        }
                        dto.Total = dto.ARTotal - dto.APTotal + dto.DCTotal;
                        dto.InvoicesJson = JsonConvert.SerializeObject(dto.Invoices);
                    }
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

            // substation
            Dictionary<Guid, string> substationDictionary = new Dictionary<Guid, string>();
            var substationList = await _substationRepository.GetListAsync();
            if (null != substationList && substationList.Count > 0)
            {
                foreach (var substation in substationList)
                {
                    substationDictionary.Add(substation.Id, substation.SubstationName);
                }
            }

            var queryable = await Repository.GetQueryableAsync();
            queryable = queryable.WhereIf(input.MblId != null && input.MblId != Guid.Empty, x => x.Id
                                           .Equals(input.MblId))
                                           .WhereIf(!string.IsNullOrWhiteSpace(input.Search), x => x.MawbNo
                                           .Contains(input.Search) || x.Shipper.TPName
                                           .Contains(input.Search) || x.FilingNo
                                           .Contains(input.Search) || x.ConsigneeId
                                           .ToString().Contains(input.Search))
                                           .WhereIf(input.CarrierId.HasValue, e => e.AwbAcctCarrierId == input.CarrierId)
                                   .WhereIf(input.ConsigneeId.HasValue, e => e.ConsigneeId == input.ConsigneeId)
                                   .WhereIf(input.ShipperId.HasValue, e => e.ShipperId == input.ShipperId)
                                   .WhereIf(input.DestinationId.HasValue, e => e.DestinationId == input.DestinationId)
                                   .WhereIf(input.DestinationId.HasValue, e => e.DepatureId == input.DepatureId)
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.FlightNo), x => x.FlightNo == input.FlightNo)
                                   .WhereIf(input.OfficeId.HasValue, e => e.OfficeId == input.OfficeId)
                                   .WhereIf(input.AwbCancelled.HasValue, e => e.IsAwbCancelled == input.AwbCancelled)
                                   .WhereIf(input.DirectMaster.HasValue, e => e.IsDirectMaster == input.DirectMaster)
                                   .WhereIf(input.PostDate.HasValue, e => e.PostDate.Date == input.PostDate.Value.Date.AddDays(1))
                                   .WhereIf(input.DepatureDate.HasValue, e => e.DepatureDate.Date == input.DepatureDate.Value.Date.AddDays(1))
                                   .WhereIf(input.ArrivalDate.HasValue, e => e.ArrivalDate.Value.Date == input.ArrivalDate.Value.Date.AddDays(1))
                                   .WhereIf(input.CreationDate.HasValue, e => e.CreationTime.Date == input.CreationDate.Value.Date.AddDays(1))
                                          .OrderByDescending(x => x.CreationTime);
            var query = queryable
                .OrderBy(x => x.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);
            Volo.Abp.Linq.IAsyncQueryableExecuter asyncExecuter = AsyncExecuter;
            var airExportMawbList = await asyncExecuter.ToListAsync(query);
            List<AirExportMawbDto> airExportMawbDtoList = new List<AirExportMawbDto>();

            string shipperName, awbAccountCarrier, consingeeName, carrierName ,departureName, destinationName, office;

            if (null != airExportMawbList && airExportMawbList.Count > 0)
            {
                foreach (var airExportMawb in airExportMawbList)
                {
                    var airExportMawbDto = ObjectMapper.Map<AirExportMawb, AirExportMawbDto>(airExportMawb);

                    tradePartnerDictionary.TryGetValue(airExportMawb.ShipperId.GetValueOrDefault(), out shipperName);
                    airExportMawbDto.Shipper = shipperName;
                    
                    tradePartnerDictionary.TryGetValue(airExportMawb.AwbAcctCarrierId.GetValueOrDefault(), out awbAccountCarrier);
                    airExportMawbDto.AwbAccountCarrierName = awbAccountCarrier;
                    
                    tradePartnerDictionary.TryGetValue(airExportMawb.ConsigneeId.GetValueOrDefault(), out consingeeName);
                    airExportMawbDto.ConsigneeName = consingeeName;
                    
                    tradePartnerDictionary.TryGetValue(airExportMawb.MawbCarrierId.GetValueOrDefault(), out carrierName);
                    airExportMawbDto.CarrierTPName = carrierName;

                    pdictionary.TryGetValue(airExportMawb.DepatureId.GetValueOrDefault(), out departureName);
                    airExportMawbDto.DepatureAirportName = departureName;

                    pdictionary.TryGetValue(airExportMawb.DestinationId.GetValueOrDefault(), out destinationName);
                    airExportMawbDto.DestinationAirportName = destinationName;

                    substationDictionary.TryGetValue(airExportMawb.OfficeId.GetValueOrDefault(), out office);
                    airExportMawbDto.OfficeName = office;

                    var hawbs = await _airExportHawbRepository.GetListAsync();
                    airExportMawbDto.HawbNos = string.Join(", ", hawbs.Where(w => w.MawbId == airExportMawbDto.Id).Select(s => s.HawbNo));

                    var queryType = 0;

                    QueryInvoiceDto queryDto = new QueryInvoiceDto() { QueryType = queryType, ParentId = airExportMawb.Id };

                    airExportMawbDto.Invoices = (await _invoiceAppService.QueryInvoicesAsync(queryDto)).ToList();

                    if (airExportMawbDto.Invoices is not null && airExportMawbDto.Invoices.Count > 0)
                    {
                        airExportMawbDto.AR = new List<InvoiceDto>();
                        airExportMawbDto.DC = new List<InvoiceDto>();
                        airExportMawbDto.AP = new List<InvoiceDto>();
                        foreach (var dto in airExportMawbDto.Invoices)
                        {
                            switch (dto.InvoiceType)
                            {
                                default:
                                    airExportMawbDto.AR.Add(dto);
                                    break;
                                case 1:
                                    airExportMawbDto.DC.Add(dto);
                                    break;
                                case 2:
                                    airExportMawbDto.AP.Add(dto);
                                    break;
                            }
                        }

                        if (airExportMawbDto.AR.Any())
                        {
                            double arTotal = 0;
                            foreach (var ar in airExportMawbDto.AR)
                            {
                                arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }
                            airExportMawbDto.ARTotal = arTotal;
                        }
                        if (airExportMawbDto.AP.Any())
                        {
                            double apTotal = 0;
                            foreach (var ap in airExportMawbDto.AP)
                            {
                                apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }

                            airExportMawbDto.APTotal = apTotal;
                        }
                        if (airExportMawbDto.DC.Any())
                        {
                            double dcTotal = 0;
                            foreach (var dc in airExportMawbDto.DC)
                            {
                                dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }
                            airExportMawbDto.DCTotal = dcTotal;
                        }
                        airExportMawbDto.Total = airExportMawbDto.ARTotal - airExportMawbDto.APTotal + airExportMawbDto.DCTotal;
                        airExportMawbDto.InvoicesJson = JsonConvert.SerializeObject(airExportMawbDto.Invoices);
                    }

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
            var packageUnits = await _packageUnitRepository.GetListAsync();
            var airPorts = await _airportRepository.GetListAsync();

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

            if (data.MawbPackageUnitId is not null)
            {
                var mawbPackageUnit = packageUnits.Where(w => w.Id == data.MawbPackageUnitId).FirstOrDefault();
                airExportDetails.MawbPackageUnit = mawbPackageUnit.PackageName;
            }

            if (data.RouteTrans1Id is not null)
            {
                var routeTrans1 = airPorts.Where(w => w.Id == data.RouteTrans1Id).FirstOrDefault();
                airExportDetails.RouteTrans1 = routeTrans1.AirportName;
            }

            if (data.RouteTrans2Id is not null)
            {
                var routeTrans2 = airPorts.Where(w => w.Id == data.RouteTrans2Id).FirstOrDefault();
                airExportDetails.RouteTrans2 = routeTrans2.AirportName;
            }
            if (data.RouteTrans3Id is not null)
            {
                var routeTrans3 = airPorts.Where(w => w.Id == data.RouteTrans3Id).FirstOrDefault();
                airExportDetails.RouteTrans3 = routeTrans3.AirportName;
            }

            if (data.RouteTrans1CarrierId is not null)
            {
                var routeTrans1Carrier = tradePartners.Where(w => w.Id == data.RouteTrans1CarrierId).FirstOrDefault();
                airExportDetails.RouteTrans1Carrier = string.Concat(routeTrans1Carrier.TPName, "/", routeTrans1Carrier.TPCode);
            }

            if (data.RouteTrans2CarrierId is not null)
            {
                var routeTrans2Carrier = tradePartners.Where(w => w.Id == data.RouteTrans2CarrierId).FirstOrDefault();
                airExportDetails.RouteTrans2Carrier = string.Concat(routeTrans2Carrier.TPName, "/", routeTrans2Carrier.TPCode);
            }

            if (data.RouteTrans3CarrierId is not null)
            {
                var routeTrans3Carrier = tradePartners.Where(w => w.Id == data.RouteTrans3CarrierId).FirstOrDefault();
                airExportDetails.RouteTrans3Carrier = string.Concat(routeTrans3Carrier.TPName, "/", routeTrans3Carrier.TPCode);
            }

            airExportDetails.LastFlight = data.RouteTrans3FlightNo is not null ? data.RouteTrans3FlightNo :
                                          data.RouteTrans2FlightNo is not null ? data.RouteTrans2FlightNo :
                                          data.RouteTrans1FlightNo is not null ? data.RouteTrans1FlightNo : "";

            airExportDetails.LastFlightDate = data.RouteTrans3FlightNo is not null ? data.RouteTrans3ArrivalDate.ToShortDateString() :
                                              data.RouteTrans2FlightNo is not null ? data.RouteTrans2ArrivalDate.ToShortDateString() :
                                              data.RouteTrans1ArrivalDate.ToShortDateString();

            airExportDetails.DepartureFlight = data.RouteDepartureFlightNo;

            airExportDetails.DepartureFlightDate = data.RouteDepatureDate?.ToShortDateString();

            airExportDetails.AirWayBillNo = data.MawbNo;
            airExportDetails.MawbNo = data.MawbNo;
            airExportDetails.DocNumber = data.FilingNo;
            airExportDetails.GrossWeight = Convert.ToString(data.AwbChargeableWeightKg);
            airExportDetails.ArrivalDate = data.ArrivalDate?.ToShortDateString();
            airExportDetails.NVD = data.DVCarriage;
            airExportDetails.NCV = data.DVCustomer;
            airExportDetails.ChargableWeight = string.Concat(data.ChargeableWeightKg, " ", data.ChargeableWeightLb);
            airExportDetails.Package = Convert.ToString(data.Package);
            airExportDetails.DepatureDate = data.DepatureDate;
            airExportDetails.ChargeableWeightCneeKG = Convert.ToString(data.ChargeableWeightKg + " KGS");
            airExportDetails.ChargeableWeightCneeLB = Convert.ToString(data.ChargeableWeightLb + " LBS");
            airExportDetails.ChargeableWeightKG = data.ChargeableWeightKg;
            airExportDetails.ChargeableWeightLB = data.ChargeableWeightLb;
            airExportDetails.Operator = string.Concat(CurrentUser.Name, " ", CurrentUser.SurName);
            if (data.ExtraProperties.Count > 0)
            {
                if (data.ExtraProperties.ContainsKey("OtherCharges"))
                {
                    airExportDetails.OtherCharges = JsonConvert.DeserializeObject<List<OtherCharges>>(data.ExtraProperties?.GetValueOrDefault("OtherCharges").ToString());
                    airExportDetails.OtherChargesJSON = data.ExtraProperties.GetValueOrDefault("OtherCharges").ToString();
                }
            }

            return airExportDetails;
        }

        public async Task LockedOrUnLockedAirExportMawbAsync(Guid id)
        {
            try
            {
                var mbl = await _repository.GetAsync(id);
                if (mbl.IsLocked == true)
                {
                    mbl.IsLocked = false;
                    var query = await _airExportHawbRepository.GetQueryableAsync();
                    var hbls = query.Where(x => x.MawbId == id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = false;

                        await _airExportHawbRepository.UpdateAsync(hbl,true);
                    }
                    await _repository.UpdateAsync(mbl);
                }
                else {

                    mbl.IsLocked = true;
                    var query = await _airExportHawbRepository.GetQueryableAsync();
                    var hbls = query.Where(x => x.MawbId == id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = true;

                        await _airExportHawbRepository.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);


                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task SelectedLockedAirExportMawbAsync(Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var mbl = await _repository.GetAsync(id);

                    mbl.IsLocked = true;
                    var query = await _airExportHawbRepository.GetQueryableAsync();
                    var hbls = query.Where(x => x.MawbId == id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = true;

                        await _airExportHawbRepository.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);

                }
            
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task SelectedUnLockedAirExportMawbAsync(Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var mbl = await _repository.GetAsync(id);

                    mbl.IsLocked = false;
                    var query = await _airExportHawbRepository.GetQueryableAsync();
                    var hbls = query.Where(x => x.MawbId == id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = false;

                        await _airExportHawbRepository.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);

                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task<List<AirExportMawbDto>> GetMawbListAsync()
        {
            var Mawbs = await _repository.GetQueryableAsync();

            return ObjectMapper.Map<List<AirExportMawb>, List<AirExportMawbDto>>(Mawbs.ToList());
        }
    }
}
