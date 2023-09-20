using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settings.PackageUnits;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.Substations;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

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
        private readonly IRepository<PackageUnit, Guid> _packageUnitRepository;
        private readonly IRepository<Port, Guid> _portRepository;
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IRepository<PortsManagement, Guid> _portsManagementAppService;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IRepository<AirImportMawb, Guid> _mawbRepository;
        private readonly IInvoiceAppService _invoiceAppService;
        public AirImportHawbAppService(IRepository<AirImportHawb, Guid> repository,
            IRepository<PackageUnit, Guid> packageUnitRepository,
            IRepository<Airport, Guid> airportRepository,
            IRepository<Substation, Guid> substationRepository,
            IRepository<AirImportHawb, Guid> mblRepository,
             IRepository<SysCode, Guid> sysCodeRepository,
            IRepository<Port, Guid> portRepository,
            IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository,
            IRepository<PortsManagement, Guid> portsManagementAppService,
            IRepository<AirImportMawb, Guid> mawbRepository,
            IIdentityUserAppService identityUserAppService,
            IInvoiceAppService invoiceAppService) : base(repository)
        {
            _repository = repository;
            _airportRepository = airportRepository;
            _mblRepository = mblRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _substationRepository = substationRepository;
            _sysCodeRepository = sysCodeRepository;
            _airportRepository = airportRepository;
            _portRepository = portRepository;
            _portsManagementAppService = portsManagementAppService;
            _mawbRepository = mawbRepository;
            _packageUnitRepository = packageUnitRepository;
            _identityUserAppService = identityUserAppService;
            _invoiceAppService = invoiceAppService;
        }

        public async Task<List<AirImportHawbDto>> GetHawbCardsByMawbId(Guid Id)
        {
            var data = await _repository.GetListAsync(f => f.MawbId == Id);
            var retVal = ObjectMapper.Map<List<AirImportHawb>, List<AirImportHawbDto>>(data);

            return retVal;
        }

        public async Task<AirImportHawbDto> GetHawbCardById(Guid Id)
        {
            if (await _repository.AnyAsync(f => f.Id == Id))
            {
                var data = await _repository.GetAsync(f => f.Id == Id);
                var retVal = ObjectMapper.Map<AirImportHawb, AirImportHawbDto>(data);

                return retVal;
            }
            return new AirImportHawbDto();
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

        public async Task<PagedResultDto<AirImportHawbDto>> QueryListAsync(OceanImports.QueryHblDto query)
        {
            var portMangements = await _portsManagementAppService.GetListAsync();
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
            var airImportHawbs = await _repository.GetQueryableAsync();
            airImportHawbs = airImportHawbs.WhereIf(query.MblId != null && query.MblId != Guid.Empty, x=>x.Id
                                           .Equals(query.MblId))
                                           .WhereIf(!string.IsNullOrWhiteSpace(query.Search), x => x.HawbNo
                                           .Contains(query.Search) || x.MawbId.ToString()
                                           .Contains(query.Search))
                                            .WhereIf(query.ShipperId.HasValue, e => e.ShipperId == query.ShipperId)
                                   .WhereIf(query.ConsigneeId.HasValue, e => e.ConsigneeId == query.ConsigneeId)
                                   .WhereIf(query.NotifyId.HasValue, e => e.ConsigneeId == query.NotifyId)
                                   .WhereIf(query.DestinationId.HasValue, e => e.FinalDestination == query.DestinationId.ToString())
                                   .WhereIf(query.CustomerId.HasValue, e => e.Customer == query.CustomerId.ToString())
                                  .WhereIf(query.SaleId.HasValue, e => e.SalesId == query.SaleId)


                                   .WhereIf(query.FreightLocationId.HasValue, e => e.FreightLocation == query.FreightLocationId.ToString())
                                   
                                  
                                   
                                   
                                   .WhereIf(query.CreationDate.HasValue, e => e.CreationTime.Date == query.CreationDate.Value.Date.AddDays(1))
                                          .OrderByDescending(x => x.CreationTime);
            List<AirImportHawb> rs = airImportHawbs.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<AirImportHawbDto> list = new List<AirImportHawbDto>();
            if (rs.Any())
            {

                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<AirImportHawb, AirImportHawbDto>(pu);
                    if (dto.ConsigneeId != null)
                    {
                        var consignee = tradePartners.Where(w => w.Id == dto.ConsigneeId).FirstOrDefault();
                        dto.ConsigneeName = string.Concat(consignee.TPName, "/", consignee.TPCode);
                    }

                    if (dto.FinalDestination != null)
                    {
                        var destination = portMangements.Where(w => w.Id.ToString() ==dto.FinalDestination).FirstOrDefault();
                        dto.DestinationName = destination?.PortName;
                       
                    }
                    if (dto.DeliveryLocation != null)
                    {
                        var destination = portMangements.Where(w => w.Id.ToString() == dto.DeliveryLocation).FirstOrDefault();
                        dto.DeliveryLocationName = destination?.PortName;

                    }
                    if (dto.FreightLocation != null)
                    {
                        var destination = portMangements.Where(w => w.Id.ToString() == dto.DeliveryLocation).FirstOrDefault();
                        dto.FreightLocationName = destination?.PortName;


                    }

                    if (dto.ShipperId != null)
                    {
                        var shipper = tradePartners.Where(w => w.Id == dto.ShipperId).FirstOrDefault();
                        dto.ShipperName = string.Concat(shipper.TPName, "/", shipper.TPCode);
                    }
                    
                    if (dto.MawbId != null)
                    {

                        var mawb = await _mawbRepository.GetAsync((Guid)dto.MawbId);
                        dto.DocNo = mawb.FilingNo;
                        dto.MawbNo = mawb.MawbNo;
                        if (mawb.OfficeId != null)
                        {
                            var substations = ObjectMapper.Map<List<Substation>, List<SubstationDto>>(await _substationRepository.GetListAsync());
                            var Office = substations.Where(w => w.Id == mawb.OfficeId).FirstOrDefault();
                            dto.OfficeName = Office.SubstationName;

                        }
                    
                    }
                  
                    if (dto.OPId != null || dto.CreatorId != null)
                    {
                        dto.OP = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(dto.OPId != null ? dto.OPId.GetValueOrDefault() : dto.CreatorId.GetValueOrDefault()));
                        dto.OperatorName = string.Concat(dto.OP.UserName, " ", dto.OP.Surname);
                    }
                    if (dto.SalesId != null)
                    {
                        var MblSale = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(dto.SalesId.GetValueOrDefault()));
                        dto.SalesName = string.Concat(MblSale?.Name, "/", MblSale?.Surname)?.TrimStart('/');
                    }
                    var queryType = 4;

                    QueryInvoiceDto queryDto = new QueryInvoiceDto() { QueryType = queryType, ParentId = dto.Id };

                    var Invoices = (await _invoiceAppService.QueryInvoicesAsync(queryDto)).ToList();

                    if (Invoices is not null && Invoices.Count > 0)
                    {
                        var AR = new List<InvoiceDto>();
                        var DC = new List<InvoiceDto>();
                        var  AP = new List<InvoiceDto>();
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
                          dto.ARBalance = arTotal.ToString("N2");
                        }
                        if (AP.Any())
                        { 
                            double apTotal = 0;
                            foreach (var ap in AP)
                            {
                                apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }

                            dto.APBalance = apTotal.ToString("N2");
                        }
                        if (DC.Any())
                        {
                            double dcTotal = 0;
                            foreach (var dc in DC)
                            {
                                dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                            }
                            dto.DCBalance = dcTotal.ToString("N2");
                        }
                    //    Total = airExportMawbDto.ARTotal - airExportMawbDto.APTotal + airExportMawbDto.DCTotal;
                    //    airExportMawbDto.InvoicesJson = JsonConvert.SerializeObject(airExportMawbDto.Invoices);
                    }
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
            listDto.TotalCount = airImportHawbs.Count();
            return listDto;
        }

        public async Task<AirImportDetails> GetAirImportDetailsById(Guid Id)
        {
            var airImportDetails = new AirImportDetails();
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            var portMangements = await _portsManagementAppService.GetListAsync();
            var packageUnits = await _packageUnitRepository.GetListAsync();
            var sysCodes = await _sysCodeRepository.GetListAsync();

            var data = await Repository.GetAsync(Id);

            if (data != null)
            {
                airImportDetails = ObjectMapper.Map<AirImportHawb, AirImportDetails>(data);

                var mawb = await _mawbRepository.GetAsync(data.MawbId.GetValueOrDefault());

                if (data.ConsigneeId != null)
                {
                    var consignee = tradePartners.Where(w => w.Id == data.ConsigneeId).FirstOrDefault();
                    airImportDetails.ConsigneeName = string.Concat(consignee.TPName, "/", consignee.TPCode);
                }

                if (mawb.DepatureId != null)
                {
                    var departure = portMangements.Where(w => w.Id == mawb.DepatureId).FirstOrDefault();
                    airImportDetails.DepatureName = departure?.PortName;
                }

                if (mawb.DestinationId != null)
                {
                    var destination = portMangements.Where(w => w.Id == mawb.DestinationId).FirstOrDefault();
                    airImportDetails.DestinationAirportName = destination?.PortName;
                    airImportDetails.DestinationCountry = destination?.Country;
                }
                
                if (data.Notify != null)
                {
                    var notify = tradePartners.Where(w => w.Id == Guid.Parse(data.Notify)).FirstOrDefault();
                    airImportDetails.NotifyName = string.Concat(notify.TPName, "/", notify.TPCode);
                }
               
                if (data.ShipperId != null)
                {
                    var shipper = tradePartners.Where(w => w.Id == data.ShipperId).FirstOrDefault();
                    airImportDetails.ShipperName = string.Concat(shipper.TPName, "/", shipper.TPCode);
                }

                if (mawb.OverseaAgentId != null)
                {
                    var overseaAgent = tradePartners.Where(w => w.Id == mawb.OverseaAgentId).FirstOrDefault();
                    airImportDetails.OverseaAgentTPName = string.Concat(overseaAgent.TPName, "/", overseaAgent.TPCode);
                    airImportDetails.IATA = overseaAgent.IataCode;
                }
                
                if (mawb.CarrierId != null)
                {
                    var carrier = tradePartners.Where(w => w.Id == mawb.CarrierId).FirstOrDefault();
                    airImportDetails.CarrierTPName = string.Concat(carrier.TPName, "/", carrier.TPCode);
                }

                if (data.BillToId != null)
                {
                    var billTo = tradePartners.Where(w => w.Id == Guid.Parse(data.BillToId)).FirstOrDefault();
                    airImportDetails.BillToName = string.Concat(billTo.TPName, "/", billTo.TPCode);
                }

                if (data.SalesType != null)
                {
                    var salesType = sysCodes.Where(w => w.Id == Guid.Parse(data.SalesType)).FirstOrDefault();
                    airImportDetails.SalesType = salesType.ShowName;
                }

                if (mawb.FreightLocationId != null)
                {
                    var freightLocation = tradePartners.Where(w => w.Id == mawb.FreightLocationId).FirstOrDefault();
                    airImportDetails.MFreightLocationName = string.Concat(freightLocation.TPName, "/", freightLocation.TPCode);
                }

                if (mawb.FlightNo != null)
                {
                    airImportDetails.FlightNo = mawb.FlightNo;
                }

                if (data.CustomsBroker != null)
                {
                    var customBroker = tradePartners.Where(w => w.Id == Guid.Parse(data.CustomsBroker)).FirstOrDefault();
                    airImportDetails.CustomBrokerName = string.Concat(customBroker.TPName, "/", customBroker.TPCode);
                }

                if (data.PackageUnit != null)
                {
                    var packageUnit = packageUnits.Where(w => w.Id == Guid.Parse(data.PackageUnit)).FirstOrDefault();
                    airImportDetails.HPackageUnitName = string.Concat(packageUnit.PackageName);
                }

                if (data.FreightLocation != null)
                {
                    var freightLocation = tradePartners.Where(w => w.Id == Guid.Parse(data.FreightLocation)).FirstOrDefault();
                    airImportDetails.FreightLocationName = string.Concat(freightLocation.TPName, "/", freightLocation.TPCode);
                }

                if (data.Trucker != null)
                {
                    var trucker = tradePartners.Where(w => w.Id == Guid.Parse(data.Trucker)).FirstOrDefault();
                    airImportDetails.HTruckerName = string.Concat(trucker.TPName, "/", trucker.TPCode);
                }

                if (data.FinalDestination != null)
                {
                    var finalDestination = tradePartners.Where(w => w.Id == Guid.Parse(data.FinalDestination)).FirstOrDefault();
                    airImportDetails.FinalDestination = string.Concat(finalDestination.TPName, "/", finalDestination.TPCode);
                }

                if (data.DeliveryLocation != null)
                {
                    var deliveryLocation = tradePartners.Where(w => w.Id == Guid.Parse(data.DeliveryLocation)).FirstOrDefault();
                    airImportDetails.DeliveryLocationName = string.Concat(deliveryLocation.TPName, "/", deliveryLocation.TPCode);
                }

                var subHawbs = new List<SubHawbs>();

                object subHawbsStr;

                data.ExtraProperties.TryGetValue("SubHawbs", out subHawbsStr);

                subHawbs = JsonConvert.DeserializeObject<List<SubHawbs>>(Convert.ToString(subHawbsStr));

                airImportDetails.GrossWeightStr = data.GrossWeightKG == null ? "" : data.GrossWeightKG + " KGS " + (double.Parse(data.GrossWeightKG) * 2.20462).ToString("0.00") + " LBS";
                airImportDetails.ChargableWeightStr = data.ChargeableWeightKG == null ? "" : data.ChargeableWeightKG + " KGS " + (double.Parse(data.ChargeableWeightKG) * 2.20462).ToString("0.00") + " LBS";
                airImportDetails.MeasurementStr = data.VolumeWeightCBM == null ? "" : data.VolumeWeightCBM + " CBM " + (double.Parse(data.VolumeWeightCBM) * 35.315).ToString("0.00") + " CFT";
                airImportDetails.CurrentDate = DateTime.Now.ToString("MM/dd/yyyy");
                airImportDetails.LastFreeDay = data.LastFreeDay;
                airImportDetails.FDestETA = string.Concat(data.FinalETA);
                airImportDetails.FilingNo = mawb.FilingNo;
                airImportDetails.HItNo = data.ITNo;
                airImportDetails.HItDate = string.Concat(data.ITDate);
                airImportDetails.HItLocation = data.ITIssuedLocation;
                airImportDetails.TotalPackage = string.Concat(data.Package) + " " + airImportDetails.HPackageUnitName;
                airImportDetails.HMark = data.Mark;
                airImportDetails.AirWayBillNo = data.HawbNo;
                airImportDetails.MawbNo = airImportDetails.MawbNo;
                airImportDetails.DocNumber = mawb.FilingNo;
                airImportDetails.ChargableWeight = string.Concat(mawb.ChargeableWeightKg, " ", mawb.ChargeableWeightLb);
                airImportDetails.DepatureDate = mawb.DepatureDate;
                airImportDetails.ArrivalDate = mawb.ArrivalDate;
                airImportDetails.OPName = string.Concat(CurrentUser.Name, " ", CurrentUser.SurName);
                airImportDetails.MawbId = mawb.Id;
                airImportDetails.HawbId = data.Id;
                airImportDetails.HawbNo = data.HawbNo;
                airImportDetails.MawbNo = mawb.MawbNo;
                airImportDetails.CustomerName = airImportDetails.BillToName;
                airImportDetails.SalesType = data.SalesType;
                airImportDetails.SubHawbs = subHawbs;
            }

            return airImportDetails;
        }
        public async Task LockedOrUnLockedAirImportHawbAsync(Guid id)
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

        public async Task UpdateMawbIdOfHawbAsync(Guid hawbId, Guid newMawbId)
        {
            try
            {
                var hawb = await _repository.GetAsync(hawbId);
                hawb.MawbId = newMawbId;
                await _repository.UpdateAsync(hawb);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

    }
}
