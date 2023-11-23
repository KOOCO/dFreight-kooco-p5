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
using Volo.Abp;
using Dolphin.Freight.Accounting.Invoices;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settinngs.Substations;

namespace Dolphin.Freight.ImportExport.AirImports
{
    public class AirImportMawbAppService :
        CrudAppService<
            AirImportMawb,
            AirImportMawbDto,
            Guid,
            QueryDto,
            CreateUpdateAirImportMawbDto>,
        IAirImportMawbAppService
    {
        private IRepository<AirImportMawb, Guid> _repository;
        private IRepository<Airport, Guid> _airportRepository;
        private IRepository<PortsManagement, Guid> _portsManagementAppService;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IRepository<AirImportHawb, Guid> _airImportHawbAppService;
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IRepository<Substation, Guid> _substationRepository;
       

        public AirImportMawbAppService(
            IRepository<AirImportMawb, Guid> repository,
            IRepository<Airport, Guid> airportRepository,
            IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository,
            IRepository<PortsManagement, Guid> portsManagementAppService,
            IRepository<AirImportHawb, Guid> airImportHawbAppService,
            IInvoiceAppService invoiceAppService,
             IIdentityUserAppService identityUserAppService,
             IRepository<Substation, Guid> substationRepository
            ) : base(repository)
        {
            _repository = repository;
            _airportRepository = airportRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _portsManagementAppService = portsManagementAppService;
            _airImportHawbAppService = airImportHawbAppService;
            _invoiceAppService = invoiceAppService;
            _substationRepository = substationRepository;
            _identityUserAppService=identityUserAppService;
        }

        public override async Task<PagedResultDto<AirImportMawbDto>> GetListAsync(QueryDto input)
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
            queryable = queryable.WhereIf(!string.IsNullOrWhiteSpace(input.Search), x => x.MawbNo
                                 .Contains(input.Search) || x.FilingNo
                                 .Contains(input.Search))
                                  .WhereIf(input.CarrierId.HasValue, e => e.AwbAcctCarrierId == input.CarrierId)
                                   .WhereIf(input.ConsigneeId.HasValue, e => e.ConsigneeId == input.ConsigneeId)
                                    .WhereIf(input.OverseaAgentId.HasValue, e => e.OverseaAgentId == input.OverseaAgentId)
                                   .WhereIf(input.DestinationId.HasValue, e => e.DestinationId == input.DestinationId)
                                   .WhereIf(input.DepatureId.HasValue, e => e.DepatureId == input.DepatureId)
                                   .WhereIf(!string.IsNullOrWhiteSpace(input.FlightNo), x => x.FlightNo == input.FlightNo)
                                   .WhereIf(input.OfficeId.HasValue, e => e.OfficeId == input.OfficeId)
                                   .WhereIf(input.AwbType.HasValue, e => e.AwbType == input.AwbType)
                                   .WhereIf(input.Block.HasValue, e => e.IsLocked == input.Block)
                                   .WhereIf(input.CreatorId.HasValue, e => e.CreatorId == input.CreatorId)
                                   .WhereIf(input.FreightLocationId.HasValue, e => e.FreightLocationId == input.FreightLocationId)
                                   .WhereIf(input.DirectMaster.HasValue, e => e.IsDirectMaster == input.DirectMaster)
                                   .WhereIf(input.PostDate.HasValue, e => e.PostDate.Date == input.PostDate.Value.Date.AddDays(1))
                                   .WhereIf(input.DepatureDate.HasValue, e => e.DepatureDate.Date == input.DepatureDate.Value.Date.AddDays(1))
                                   .WhereIf(input.ArrivalDate.HasValue, e => e.ArrivalDate.Date == input.ArrivalDate.Value.Date.AddDays(1))
                                   .WhereIf(input.CreationDate.HasValue, e => e.CreationTime.Date == input.CreationDate.Value.Date.AddDays(1))
                                          .OrderByDescending(x => x.CreationTime);

            var query = queryable
                .OrderBy(x => x.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);
            var airImportMawbList = await AsyncExecuter.ToListAsync(query);
            List<AirImportMawbDto> airImportMawbDtoList = new List<AirImportMawbDto>();
            if (null != airImportMawbList && airImportMawbList.Count > 0)
            {
                foreach (var airImportMawb in airImportMawbList)
                {
                    var airImportMawbDto = ObjectMapper.Map<AirImportMawb, AirImportMawbDto>(airImportMawb);
                    if (airImportMawb.Id != Guid.Empty)
                    {
                        airImportMawbDto.Id = airImportMawb.Id;
                    }
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
                    if (airImportMawb.SalesId != null)
                    {
                        var MblSale = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(airImportMawb.SalesId.GetValueOrDefault()));
                        airImportMawbDto.SalesName = string.Concat(MblSale?.Name, "/", MblSale?.Surname)?.TrimStart('/');
                    }
                    if (airImportMawb.CreatorId != null)
                    {
                        var MblSale = await _identityUserAppService.GetAsync(airImportMawb.CreatorId.GetValueOrDefault());
                        airImportMawbDto.OpName = MblSale?.Name;
                    }
                    else
                    {
                        airImportMawbDto.SalesName = null;
                    }
                    if (airImportMawb.OfficeId != null)
                    {
                        var substations = ObjectMapper.Map<List<Substation>, List<SubstationDto>>(await _substationRepository.GetListAsync());
                        var Office = substations.Where(w => w.Id == airImportMawb.OfficeId).FirstOrDefault();
                        airImportMawbDto.OfficeName = Office.SubstationName;
                    }
                    else
                    {
                        airImportMawbDto.OfficeName = null;
                    }
                    var queryType = 0;

                    QueryInvoiceDto queryDto = new QueryInvoiceDto() { QueryType = queryType, ParentId = airImportMawbDto.Id };

                    var Invoices = (await _invoiceAppService.QueryInvoicesAsync(queryDto)).ToList();

                    if (Invoices is not null && Invoices.Count > 0)
                    {
                        var AR = new List<InvoiceDto>();
                        var DC = new List<InvoiceDto>();
                        var AP = new List<InvoiceDto>();
                        foreach (var dto1 in Invoices)
                        {
                            switch (dto1.InvoiceType)
                            {
                                default:
                                    AR.Add(dto1);
                                    break;
                                case 1:
                                    DC.Add(dto1);
                                    break;
                                case 2:
                                    AP.Add(dto1);
                                    break;
                            }
                        }

                        if (AR.Any())
                        {
                            double arTotal = 0;
                            foreach (var ar in AR)
                            {
                                arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }
                            airImportMawbDto.ARBalance = arTotal.ToString("N2");
                        }
                        if (AP.Any())
                        {
                            double apTotal = 0;
                            foreach (var ap in AP)
                            {
                                apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }

                            airImportMawbDto.APBalance = apTotal.ToString("N2");
                        }
                        if (DC.Any())
                        {
                            double dcTotal = 0;
                            foreach (var dc in DC)
                            {
                                dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }
                            airImportMawbDto.DCBalance = dcTotal.ToString("N2");
                        }

                    }
                    airImportMawbDtoList.Add(airImportMawbDto);
                }
            }
                //Get the total count with another query
                var totalCount = queryable.Count();

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
        public  async Task DeleteMultipalAsync(List<Guid> Ids)
        {
            foreach (var id in Ids)
            {
                
               

                await base.DeleteAsync(id);
               
            }
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

            airImportDetails.FlightNo = data.FlightNo;
            airImportDetails.AirWayBillNo = data.MawbNo;
            airImportDetails.MawbNo = airImportDetails.MawbNo;
            airImportDetails.DocNumber = data.FilingNo;
            airImportDetails.ChargableWeight = string.Concat(data.ChargeableWeightKg, " ", data.ChargeableWeightLb);
            airImportDetails.DepatureDate = data.DepatureDate;
            airImportDetails.ArrivalDate = data.ArrivalDate;
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
                airImportDetails.CarrierName = string.Concat(carrier.TPName);
            }

            airImportDetails.MawbNo = data.MawbNo;
            airImportDetails.FilingNo = data.FilingNo;
            airImportDetails.FlightNo = data.FlightNo;
            airImportDetails.DepatureDate = data.DepatureDate;
            airImportDetails.ArrivalDate = data.ArrivalDate;

            return airImportDetails;
        }
        public async Task LockedOrUnLockedAirImportMawbAsync(Guid id)
        {
            try
            {
                var mbl = await _repository.GetAsync(id);
               
                    mbl.IsLocked = !mbl.IsLocked;
                    var query = await _airImportHawbAppService.GetQueryableAsync();
                    var hbls = query.Where(x => x.MawbId == id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = mbl.IsLocked;

                        await _airImportHawbAppService.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);
             
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<List<AirImportMawbDto>> GetMawbListAsync() {

            var query = await _repository.GetQueryableAsync();
            return ObjectMapper.Map<List<AirImportMawb>, List<AirImportMawbDto>>(query.ToList());
        
        }
        public async Task SelectedUnLockedAirImportMawbAsync(Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var mbl = await _repository.GetAsync(id);

                    mbl.IsLocked = false;
                    var query = await _airImportHawbAppService.GetQueryableAsync();
                    var hbls = query.Where(x => x.MawbId == id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = false;

                        await _airImportHawbAppService.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);

                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }
        public async Task SelectedLockedAirImportMawbAsync(Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var mbl = await _repository.GetAsync(id);

                    mbl.IsLocked = true;
                    var query = await _airImportHawbAppService.GetQueryableAsync();
                    var hbls = query.Where(x => x.MawbId == id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = true;

                        await _airImportHawbAppService.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);

                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }
    }
}
