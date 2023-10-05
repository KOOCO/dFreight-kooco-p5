using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Nito.AsyncEx.Synchronous;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Dolphin.Freight.ImportExport.AirExports.Bookings
{
    public class ExportBookingAppService :
        CrudAppService<
            AirExportBooking,
            ExportBookingDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateExportBookingDto>,
        IExportBookingAppService
    {
        private IRepository<AirExportBooking, Guid> _repository;
        private IRepository<AirExportMawb, Guid> _mawbRepository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        private IRepository<PortsManagement, Guid> _portsManagementRepository;
        private IRepository<Substation, Guid> _substationRepository;
        private IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private IRepository<Airport, Guid> _airPortRepository;
        private IIdentityUserRepository _identityUserRepository;
        private IRepository<AirExportHawb, Guid> _hawbRepositroy;
        public ExportBookingAppService(IRepository<AirExportBooking, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<PortsManagement,
                                       Guid> portsManagementRepository, IRepository<Substation, Guid> substationRepository, IRepository<TradePartners.TradePartner,
                                       Guid> tradePartnerRepository, IRepository<Airport, Guid> airPortRepository, IIdentityUserRepository identityUserRepository,
                                        IRepository<AirExportMawb, Guid> mawbRepository, IRepository<AirExportHawb, Guid> hawbRepositroy)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _portsManagementRepository = portsManagementRepository;
            _substationRepository = substationRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _airPortRepository= airPortRepository;
            _identityUserRepository = identityUserRepository;
            _mawbRepository=mawbRepository;
            _hawbRepositroy = hawbRepositroy;
            /*
            GetPolicyName = OceanExportPermissions.ExportBookingas.Default;
            GetListPolicyName = OceanExportPermissions.ExportBookingas.Default;
            CreatePolicyName = OceanExportPermissions.ExportBookingas.Create;
            UpdatePolicyName = OceanExportPermissions.ExportBookingas.Edit;
            DeletePolicyName = OceanExportPermissions.ExportBookingas.Delete;*/
        }
        public async Task<List<ExportBookingDto>> GetSONo()
        {
            var exportBooking = await _repository.GetListAsync();
            var list = ObjectMapper.Map<List<AirExportBooking>, List<ExportBookingDto>>(exportBooking);
            return list;
        }
        public async Task DeleteAirExportBookingAsync(Guid id)
        {
            await base.DeleteAsync(id);
        }
        public async Task<PagedResultDto<ExportBookingDto>> GetQueryListAsync(QueryExportBookingDto query)
        {
            var airExportMawbs = await _repository.GetQueryableAsync();
            airExportMawbs = airExportMawbs
                                           .WhereIf(!string.IsNullOrWhiteSpace(query.Search),x=> x.Shipper.TPName
                                           .Contains(query.Search) || x.SoNo
                                           .Contains(query.Search) || x.FlightNo
                                           .Contains(query.Search) || x.BookingRemark
                                           .Contains(query.Search) || x.ConsigneeId
                                           .ToString().Contains(query.Search))
                                            .WhereIf(query.CarrierId.HasValue, e => e.CarrierId == query.CarrierId)
                                .WhereIf(query.ShipperId.HasValue, e => e.ShipperId == query.ShipperId)
                                .WhereIf(query.ConsigneeId.HasValue, e => e.ConsigneeId == query.ConsigneeId)
                                   .WhereIf(query.OfficeId.HasValue, e => e.OfficeId == query.OfficeId)
                                   .WhereIf(query.NotifyId.HasValue, e => e.NotifyId == query.NotifyId)
                                .WhereIf(query.SalesId.HasValue, e => e.SalesPersonId == query.SalesId)
                                .WhereIf(query.DepatureId.HasValue, e => e.DepatureId == query.DepatureId)
                                .WhereIf(query.CustomerId.HasValue, e => e.CustomerId == query.CustomerId)
                                .WhereIf(query.DestinationId.HasValue, e => e.DestinationId == query.DestinationId)
                                .WhereIf(query.ForwadingAgentId.HasValue, e => e.ForwardingAgentId == query.ForwadingAgentId)
                                 .WhereIf(!string.IsNullOrWhiteSpace(query.FlightNo), x => x.FlightNo == query.FlightNo)
                                 .WhereIf(!string.IsNullOrWhiteSpace(query.BookingRemarks), x => x.BookingRemark == query.BookingRemarks)
                                .WhereIf(query.DepatureDate.HasValue, e => e.DepatureDate.Date == query.DepatureDate.Value.Date.AddDays(1))
                                .WhereIf(query.ArrivalDate.HasValue, e => e.ArrivalDate.Value.Date == query.ArrivalDate.Value.Date.AddDays(1))
                                   .WhereIf(query.BookingDate.HasValue, e => e.SoNoDate.Date == query.BookingDate.Value.Date.AddDays(1))
                                  
                                     
                                          .OrderByDescending(x => x.CreationTime);

            List<AirExportBooking> rs = airExportMawbs.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<ExportBookingDto> list = new List<ExportBookingDto>();
            if (rs.Any())
            {
                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<AirExportBooking, ExportBookingDto>(pu);
                    if (pu.ReferenceId != null)
                    {
                        var carrier = await _mawbRepository.GetAsync((Guid)pu.ReferenceId);
                        dto.MawbNo = carrier.MawbNo;
                    }
                    if (pu.ReferenceId != null)
                    {
                        var carriers = await _hawbRepositroy.GetQueryableAsync();
                        var carrier = carriers.Where(x => x.MawbId == pu.ReferenceId).FirstOrDefault();
                        dto.HawbNo = carrier.HawbNo;
                    }
                    if (pu.CarrierId != null)
                    {
                     var carrier=   await _tradePartnerRepository.GetAsync((Guid)pu.CarrierId);
                        dto.CarrierName = carrier.TPName;
                    }
                    if (pu.DepatureId != null)
                    {
                        var departure = await _airPortRepository.GetAsync((Guid)pu.DepatureId);
                        dto.DepartureName = departure.AirportName;
                    }
                    if (pu.DestinationId != null)
                    {
                        var destination = await _airPortRepository.GetAsync((Guid)pu.DestinationId);
                        dto.DestinationName = destination.AirportName;
                    }
                    if (pu.ShipperId != null)
                    {
                        var carrier = await _tradePartnerRepository.GetAsync((Guid)pu.ShipperId);
                        dto.ShipperName = carrier.TPName;
                    }
                    if (pu.CustomerId != null)
                    {
                        var carrier = await _tradePartnerRepository.GetAsync((Guid)pu.CustomerId);
                        dto.CustomerName = carrier.TPName;
                    }
                    if (pu.NotifyId != null)
                    {
                        var carrier = await _tradePartnerRepository.GetAsync((Guid)pu.NotifyId);
                        dto.NotifyName = carrier.TPName;
                    }
                    if (pu.ForwardingAgentId != null)
                    {
                        var carrier = await _tradePartnerRepository.GetAsync((Guid)pu.ForwardingAgentId);
                        dto.OverseaAgentName = carrier.TPName;
                    }
                    if (pu.ConsigneeId != null)
                    {
                        var carrier = await _tradePartnerRepository.GetAsync((Guid)pu.ConsigneeId);
                        dto.ConsigneeName = carrier.TPName;
                    }
                    if (pu.CreatorId != null)
                    {
                        var carrier = await _identityUserRepository.GetAsync((Guid)pu.CreatorId);
                        dto.OPName = carrier.Name;
                    }
                   
                    if (pu.SalesPersonId != null)
                    {
                        var carrier = await _tradePartnerRepository.GetAsync((Guid)pu.SalesPersonId);
                        dto.SalesName = carrier.TPName;
                    }
                    if (pu.OfficeId != null)
                    {
                        var carrier = await _substationRepository.GetAsync((Guid)pu.OfficeId);
                        dto.OfficeName = carrier.SubstationName;
                    }
                    list.Add(dto);
                }
            }
            PagedResultDto<ExportBookingDto> listDto = new PagedResultDto<ExportBookingDto>();
            listDto.Items = list;
            listDto.TotalCount = airExportMawbs.Count();
            return listDto;
        }
    }
}
