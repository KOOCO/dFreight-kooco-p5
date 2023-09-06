using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Repositories;
using static Volo.Abp.Identity.IdentityPermissions;

namespace Dolphin.Freight.ImportExport.OceanExports.ExportBookings
{
    public class ExportBookingAppService :
        CrudAppService<
            ExportBooking,
            ExportBookingDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateExportBookingDto>,
        IExportBookingAppService
    {
        private IRepository<ExportBooking, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        private IRepository<PortsManagement, Guid> _portsManagementRepository;
        private IRepository<Substation, Guid> _substationRepository;
        private IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        public ExportBookingAppService(IRepository<ExportBooking, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<PortsManagement, 
                                       Guid> portsManagementRepository, IRepository<Substation, Guid> substationRepository, IRepository<TradePartners.TradePartner, 
                                       Guid> tradePartnerRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _portsManagementRepository = portsManagementRepository;
            _substationRepository = substationRepository;
            _tradePartnerRepository = tradePartnerRepository;
            /*
            GetPolicyName = OceanExportPermissions.ExportBookingas.Default;
            GetListPolicyName = OceanExportPermissions.ExportBookingas.Default;
            CreatePolicyName = OceanExportPermissions.ExportBookingas.Create;
            UpdatePolicyName = OceanExportPermissions.ExportBookingas.Edit;
            DeletePolicyName = OceanExportPermissions.ExportBookingas.Delete;*/
        }
        public async Task<PagedResultDto<ExportBookingDto>> QueryListAsync(QueryExportBookingDto query)
        {
            var Ports = await _portsManagementRepository.GetListAsync();
            Dictionary<Guid, string> pdictionary = new();
            if (Ports != null)
            {
                foreach (var port in Ports)
                {
                    pdictionary.Add(port.Id, port.PortName);
                }
            }
            var Substations = await _substationRepository.GetListAsync();
            Dictionary<Guid, string> sdictionary = new();
            if (Substations != null)
            {
                foreach (var substation in Substations)
                {
                    sdictionary.Add(substation.Id, substation.SubstationName);
                }
            }
            var TradePartners = await _tradePartnerRepository.GetListAsync();
            Dictionary<Guid, string> tdictionary = new();
            if (TradePartners != null)
            {
                foreach (var tradePartner in TradePartners)
                {
                    tdictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> sysdictionary = new();
            if (SysCodes != null)
            {
                foreach (var sysCode in SysCodes)
                {
                    sysdictionary.Add(sysCode.Id, sysCode.ShowName);
                }
            }
            var ExportBookings = await _repository.GetQueryableAsync();
            var c = from jobProfile in ExportBookings
                    select jobProfile.Carrier;
            var s = from jobProfile in ExportBookings
                    select jobProfile.Shipper;
            var o = from jobProfile in ExportBookings
                    select jobProfile.Office;

            ExportBookings = ExportBookings.WhereIf(!string.IsNullOrWhiteSpace(query.Search), x => x.HblNo
                                    .Contains(query.Search) || x.SoNo
                                    .Contains(query.Search) || x.CarrierBkgNo
                                    .Contains(query.Search) || x.VesselName
                                    .Contains(query.Search) || x.Voyage
                                    .Contains(query.Search) || x.Shipper.TPName
                                    .Contains(query.Search) || x.Office.SubstationName
                                    .Contains(query.Search) || x.ShippingAgent.TPName
                                    .Contains(query.Search) || x.Carrier.TPName
                                    .Contains(query.Search))
                                .WhereIf(query.CarrierId.HasValue, e => e.CarrierId == query.CarrierId)
                                .WhereIf(query.ShipperId.HasValue, e => e.ShipperId == query.ShipperId)
                                .WhereIf(query.ConsigneeId.HasValue, e => e.ConsigneeId == query.ConsigneeId)
                                   .WhereIf(query.OfficeId.HasValue, e => e.OfficeId == query.OfficeId)
                                   .WhereIf(query.NotifyId.HasValue, e => e.NotifyId == query.NotifyId)
                                   .WhereIf(query.FbaFcId.HasValue, e => e.FbaId == query.FbaFcId)
                                   .WhereIf(query.Pod.HasValue, e => e.PodId == query.Pod)
                                   .WhereIf(query.Del.HasValue, e => e.DelId == query.Del)
                                   .WhereIf(query.DeliverTo.HasValue, e => e.DeliveryToId == query.DeliverTo)
                                   .WhereIf(query.ShipModeId.HasValue, e => e.ShipModeId == query.ShipModeId)
                                   .WhereIf(query.CustomerId.HasValue, e => e.CustomerId == query.CustomerId)
                                   .WhereIf(query.BlCancelled.HasValue, e => e.IsCanceled == query.BlCancelled)
                                   .WhereIf(query.CargoReadyDate.HasValue, e => e.CargoArrivalDate.Value.Date == query.CargoReadyDate.Value.Date.AddDays(1))
                                   .WhereIf(query.Eta.HasValue, e => e.PodEta.Date == query.Eta.Value.Date.AddDays(1))
                                   .WhereIf(query.Etd.HasValue, e => e.PolEtd.Date == query.Etd.Value.Date.AddDays(1))
                                   .WhereIf(query.CreationDate.HasValue, e => e.CreationTime.Date == query.CreationDate.Value.Date.AddDays(1));
            List<ExportBooking> rs = ExportBookings.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<ExportBookingDto> list = new List<ExportBookingDto>();

            if (rs.Any())
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<ExportBooking, ExportBookingDto>(pu);
                    if (pud.PolId != null) pud.PolName = pdictionary[pud.PolId.Value];
                    if (pud.PorId != null) pud.PorName = pdictionary[pud.PorId.Value];
                    if (pud.PodId != null) pud.PodName = pdictionary[pud.PodId.Value];
                    if (pud.DelId != null) pud.DelName = pdictionary[pud.DelId.Value];
                    if (pud.ShipperId != null) pud.ShipperName = tdictionary[pud.ShipperId.Value];
                    if (pud.CustomerId != null) pud.CustomerName = tdictionary[pud.CustomerId.Value];
                    if (pud.CarrierId != null) pud.CarrierName = tdictionary[pud.CarrierId.Value];
                    if (pud.ShippingAgentId != null) pud.ShippingAgentName = tdictionary[pud.ShippingAgentId.Value];
                    string SvcName = "";
                    if (pud.SvcTermFromId != null) SvcName = sysdictionary[pud.SvcTermFromId.Value];
                    if (pud.SvcTermToId != null) {
                        if (SvcName.Length > 0) SvcName = SvcName + " - ";
                        SvcName = SvcName + sysdictionary[pud.SvcTermToId.Value];
                    }
                    if (pud.OfficeId != null) pud.OfficeName = sdictionary[pud.OfficeId.Value];
                    //if (pud.MblOverseaAgentId != null) pud.MblOverseaAgentName = tdictionary[pud.MblOverseaAgentId.Value];
                    list.Add(pud);
                }
            }
            PagedResultDto<ExportBookingDto> listDto = new PagedResultDto<ExportBookingDto>();
            listDto.Items = list;
            listDto.TotalCount = ExportBookings.Count();
            return listDto;
        }

        public async Task<List<ExportBookingDto>> GetSONo()
        {
            var exportBooking = await _repository.GetListAsync();
            var list = ObjectMapper.Map<List<ExportBooking>, List<ExportBookingDto>>(exportBooking);
            return list;
        }

        public async Task<CreateUpdateExportBookingDto> GetBookingCardById(Guid Id)
        {
            if (await _repository.AnyAsync(a => a.Id == Id))
            {
                var data = await _repository.GetAsync(a => a.Id == Id, true);
                var retVal = ObjectMapper.Map<ExportBooking, CreateUpdateExportBookingDto>(data);
                return retVal;
            }
            return new CreateUpdateExportBookingDto();
        }

        public async Task DeleteMultipleBookingsByIdsAsync(Guid[] ids)
        {
            foreach (var id in ids)
            {
                var booking = await _repository.GetAsync(id);

                booking.IsDeleted = true;

                await _repository.UpdateAsync(booking);
            }
        }
    }
}