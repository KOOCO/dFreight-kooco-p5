﻿using AutoMapper.Internal.Mappers;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.Countries;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using NPOI.POIFS.Crypt.Dsig.Facets;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.OceanImports
{
    public class OceanImportHblAppService :
        CrudAppService<
            OceanImportHbl,
            OceanImportHblDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateOceanImportHblDto>,
        IOceanImportHblAppService
    {
        private readonly IRepository<OceanImportHbl, Guid> _repository;
        private readonly IRepository<OceanImportMbl, Guid> _mblRepository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly IRepository<Port, Guid> _portRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IRepository<PortsManagement, Guid> _portsManagementRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Container, Guid> _containerRepository;
        private readonly IRepository<Country, Guid> _countryRepository;

        public OceanImportHblAppService(IRepository<OceanImportHbl, Guid> repository, IRepository<Container, Guid> containerRepository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<OceanImportMbl, Guid> mblRepository, IRepository<Substation, Guid> substationRepository, IRepository<Port, Guid> portRepository, IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository, IRepository<PortsManagement, Guid> portsManagementRepository, ICurrentUser currentUser, IRepository<Country, Guid> countryRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _mblRepository = mblRepository;
            _substationRepository = substationRepository;
            _portRepository = portRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _portsManagementRepository = portsManagementRepository;
            _currentUser = currentUser;
            _containerRepository = containerRepository;
            _countryRepository = countryRepository;
            /*
            GetPolicyName = OceanImportPermissions.OceanImportHbls.Default;
            GetListPolicyName = OceanImportPermissions.OceanImportHbls.Default;
            CreatePolicyName = OceanImportPermissions.OceanImportHbls.Create;
            UpdatePolicyName = OceanImportPermissions.OceanImportHbls.Edit;
            DeletePolicyName = OceanImportPermissions.OceanImportHbls.Delete;*/
        }
        public async Task<PagedResultDto<OceanImportHblDto>> QueryListAsync(QueryHblDto query)
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
            Dictionary<Guid, OceanImportMbl> mdictionary = new();
            if (mbls != null)
            {
                foreach (var mbl in mbls)
                {
                    mdictionary.Add(mbl.Id, mbl);
                }
            }
            //港口
            var ports = await _portsManagementRepository.GetListAsync();
            Dictionary<Guid, string> pdictionary = new();
            if (ports != null && ports.Count > 0)
            {
                foreach (var port in ports)
                {
                    pdictionary.Add(port.Id, port.SubDiv + " " + port.PortName + " ( " + port.Locode + " ) ");
                }
            }
            var OceanImportHbls = await _repository.GetQueryableAsync();
            OceanImportHbls = OceanImportHbls.WhereIf(!string.IsNullOrWhiteSpace(query.Search), x => x.HblNo
                                    .Contains(query.Search) || x.Mbl.SoNo
                                    .Contains(query.Search) || x.Mbl.Office.SubstationName
                                    .Contains(query.Search) || x.HblShipper.TPName
                                    .Contains(query.Search) || x.HblConsignee.TPName
                                    .Contains(query.Search))
                                    .WhereIf(query.MblId != null && query.MblId == Guid.Empty, x => x.MblId.Value
                                    .Equals(query.MblId))

                                    .WhereIf(query.ShipperId.HasValue, e => e.HblShipperId == query.ShipperId)
                                    
                                    
                                   .WhereIf(query.NotifyId.HasValue, e => e.HblNotifyId == query.NotifyId)
                                   .WhereIf(query.ConsigneeId.HasValue, e => e.HblConsigneeId == query.ConsigneeId)
                                   .WhereIf(query.Pol.HasValue, e => e.PolId == query.Pol)
                                   .WhereIf(query.PorId.HasValue, e => e.PorId == query.PorId)
                                   .WhereIf(query.TruckerId.HasValue, e => e.TruckerId == query.TruckerId)
                                   .WhereIf(query.SaleId.HasValue, e => e.HblSaleId == query.SaleId)
                                   .WhereIf(query.CustomerId.HasValue, e => e.HblCustomerId == query.CustomerId)

                                   .WhereIf(query.Pod.HasValue, e => e.PodId == query.Pod)
                                   .WhereIf(query.Del.HasValue, e => e.DelId == query.Del)
                                   .WhereIf(query.FinalDestId.HasValue, e => e.FdestId == query.FinalDestId)
                                   .WhereIf(query.ShipModeId.HasValue, e => e.ShipModeId == query.ShipModeId)
                                   .WhereIf(query.OvearseaAgentId.HasValue, e => e.AgentId == query.OvearseaAgentId)
                                   .WhereIf(query.SvcTermId.HasValue, e => e.SvcTermToId == query.SvcTermId)
                                 .WhereIf(query.DelEta.HasValue, e => e.DelEta.Value.Date == query.DelEta.Value.Date.AddDays(1))
                                   .WhereIf(query.FinalDestEta.HasValue, e => e.FdestEta.Value.Date == query.FinalDestEta.Value.Date.AddDays(1))
                                   .WhereIf(query.Eta.HasValue, e => e.PodEta.Value.Date == query.Eta.Value.Date.AddDays(1))
                                   .WhereIf(query.Etd.HasValue, e => e.PolEtd.Value.Date == query.Etd.Value.Date.AddDays(1))
                                   .WhereIf(query.CreationDate.HasValue, e => e.CreationTime.Date == query.CreationDate.Value.Date.AddDays(1));
            List<OceanImportHbl> rs = OceanImportHbls.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<OceanImportHblDto> list = new List<OceanImportHblDto>();
            
            if (rs.Any())
            {

                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<OceanImportHbl, OceanImportHblDto>(pu);
                    dto.FilingNo = mdictionary[dto.MblId].FilingNo;
                    dto.MblNo = mdictionary[dto.MblId].SoNo;
                    dto.OfficeName = sdictionary[mdictionary[dto.MblId].OfficeId.Value];
                    dto.Voyage = mdictionary[dto.MblId].Voyage;
                    //SysCode
                    if (mdictionary[dto.MblId].OblTypeId != null) dto.OblTypeName = dictionary[mdictionary[dto.MblId].OblTypeId.Value];
                    if (dto.SvcTermFromId is not null) dto.SvcTermFromName = dictionary[dto.SvcTermFromId.Value];
                    //人
                    if (dto.AgentId != null) dto.AgentName = tdictionary[dto.AgentId.Value];
                    if (dto.CyCfsLocationId is not null) dto.CyCfsLocationName = tdictionary[dto.CyCfsLocationId.Value];
                    if (dto.TruckerId is not null) dto.TruckerName = tdictionary[dto.TruckerId.Value];
                    if (dto.HblShipperId != null) dto.HblShipperName = tdictionary[dto.HblShipperId.Value];
                    if (dto.HblConsigneeId != null) dto.HblConsigneeName = tdictionary[dto.HblConsigneeId.Value];
                    if (mdictionary[dto.MblId].MblCarrierId != null) dto.MblCarrierName = tdictionary[mdictionary[dto.MblId].MblCarrierId.Value];
                    if (mdictionary[dto.MblId].ShipModeId != null) dto.shipModeName = dictionary[mdictionary[dto.MblId].ShipModeId.Value];

                    //港口
                    if (dto.PodId != null) dto.PodName = pdictionary[dto.PodId.Value];
                    if (dto.PolId != null) dto.PolName = pdictionary[dto.PolId.Value];
                    if (dto.PorId != null) dto.PorName = pdictionary[dto.PorId.Value];
                    if (dto.DelId != null) dto.DelName = pdictionary[dto.DelId.Value];
                    if (dto.FdestId != null) dto.FdestName = pdictionary[dto.FdestId.Value];
                    list.Add(dto);
                }
            }
            PagedResultDto<OceanImportHblDto> listDto = new PagedResultDto<OceanImportHblDto>();
            listDto.Items = list;
            listDto.TotalCount = OceanImportHbls.Count();
            return listDto;
        }
        public async Task<IList<OceanImportHblDto>> QueryListByMidAsync(QueryHblDto query)
        {
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.CodeValue);
                }
            }
            var OceanImportHbls = await _repository.GetListAsync();
            List<OceanImportHbl> rs;
            List<OceanImportHblDto> list = new List<OceanImportHblDto>();
            if (query != null && query.MblId != null)
            {
                rs = OceanImportHbls.Where(x => x.MblId.Equals(query.MblId.Value)).ToList();
            }
            else
            {
                rs = OceanImportHbls;
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<OceanImportHbl, OceanImportHblDto>(pu);
                    if (pud.CardColorId != null) pud.CardColorValue = dictionary[pud.CardColorId.Value];
                    if (pud.ColorRemarkId != null) pud.ColorRemarkValue = dictionary[pud.ColorRemarkId.Value];

                    if (await _containerRepository.AnyAsync(a => a.HblId == pud.Id))
                    {
                        var container = await _containerRepository.FirstOrDefaultAsync(a => a.HblId == pud.Id);
                        pud.CreateUpdateHBLContainerDto = ObjectMapper.Map<Container, CreateUpdateContainerDto>(container);
                    }

                    list.Add(pud);
                }
            }
            return list;
        }
        public async Task<CreateUpdateOceanImportHblDto> GetHblById(QueryHblDto query)
        {
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.CodeValue);
                }
            }
            var oceanImportHbl = await _repository.GetAsync(query.Id.Value);
            var rs = ObjectMapper.Map<OceanImportHbl, CreateUpdateOceanImportHblDto>(oceanImportHbl);
            if (rs.CardColorId != null) rs.CardColorValue = dictionary[rs.CardColorId.Value];
            return rs;
        }
        public async void LockedOrUnLockedOceanImportHblAsync(QueryHblDto query)
        {
            var Hbl = await _repository.GetAsync(query.HblId.Value);
            Hbl.IsLocked = !Hbl.IsLocked;
            await _repository.UpdateAsync(Hbl);
        }
        public async Task<List<OceanImportHblDto>> GetHblCardsById(Guid Id, bool isAsc = true, int sortType = 1)
        {
            var data = await _repository.GetListAsync(f => f.MblId == Id);
            var tradePartners = ObjectMapper.Map<List<TradePartners.TradePartner>, List<TradePartnerDto>>(await _tradePartnerRepository.GetListAsync());
            var sysCodes = await _sysCodeRepository.GetListAsync();

            if (!isAsc)
            {
                if (sortType == 1)
                {
                    data = data.OrderByDescending(x => x.HblNo).ToList();
                }
                else
                {
                    data = data.OrderByDescending(x => x.CreationTime).ToList();

                }
            }
            else
            {

                if (sortType == 1)
                {
                    data = data.OrderBy(x => x.HblNo).ToList();
                }
                else
                {
                    data = data.OrderBy(x => x.CreationTime).ToList();

                }
            }

            var retVal = ObjectMapper.Map<List<OceanImportHbl>, List<OceanImportHblDto>>(data);
            foreach (var item in retVal)
            {
                if (item.HblShipperId != null)
                {
                    var shipper = tradePartners.Where(w => w.Id == item.HblShipperId).FirstOrDefault();
                    item.HblShipperName = shipper.TPName;
                }

                if (item.HblConsigneeId != null)
                {
                    var consignee = tradePartners.Where(w => w.Id == item.HblConsigneeId).FirstOrDefault();
                    item.HblConsigneeName = consignee.TPName;
                }

                if (item.CardColorId is not null)
                {
                    var colorValue = sysCodes.Where(w => w.Id == item.CardColorId).FirstOrDefault();
                    item.CardColorValue = colorValue.CodeValue;
                }
            }
            return retVal;
        }
        public async Task<OceanImportHblDto> GetHblCardById(Guid Id)
        {
            if (Id != Guid.Empty && await _repository.AnyAsync(f => f.Id == Id))
            {
                var data = await _repository.GetAsync(f => f.Id == Id);
                var retVal = ObjectMapper.Map<OceanImportHbl, OceanImportHblDto>(data);

                return retVal;
            }

            return new OceanImportHblDto();
        }
        public async Task<OceanImportDetails> GetOceanImportDetailsById(Guid Id)
        {
            var oceanImportDetails = new OceanImportDetails();
            var tradePartners = ObjectMapper.Map<List<TradePartners.TradePartner>, List<TradePartnerDto>>(await _tradePartnerRepository.GetListAsync());
            var portMangements = ObjectMapper.Map<List<PortsManagement>, List<PortsManagementDTO>>(await _portsManagementRepository.GetListAsync());
            var sysCodes = ObjectMapper.Map<List<SysCode>, List<SysCodeDto>>(await _sysCodeRepository.GetListAsync());
            var substations = ObjectMapper.Map<List<Substation>, List<SubstationDto>>(await _substationRepository.GetListAsync());
            var countries = await _countryRepository.GetListAsync();
            var data = await _repository.GetAsync(Id);

            if (data != null)
            {
                oceanImportDetails = ObjectMapper.Map<OceanImportHbl, OceanImportDetails>(data);

                var mbl = await _mblRepository.GetAsync(data.MblId.GetValueOrDefault());
                if (data.HblBillToId != null)
                {
                    var billto = tradePartners.Where(w => w.Id == data.HblBillToId).FirstOrDefault();
                    oceanImportDetails.MblBillToName = string.Concat(billto.TPName, "/", billto.TPCode);
                    oceanImportDetails.MblBillToContent=billto.TPLocalAddress;
                }
                if (data.HblCustomerId != null)
                {
                    var customer = tradePartners.Where(w => w.Id == data.HblCustomerId).FirstOrDefault();
                    oceanImportDetails.HblCustomerName = string.Concat(customer.TPName, "/", customer.TPCode);
                   
                }
                if (data.HblShipperId != null)
                {
                    var shipper = tradePartners.Where(w => w.Id == data.HblShipperId).FirstOrDefault();
                    oceanImportDetails.ShippingAgentName = string.Concat(shipper.TPName, "/", shipper.TPCode);
                }

                if (data.HblConsigneeId != null)
                {
                    var consignee = tradePartners.Where(w => w.Id == data.HblConsigneeId).FirstOrDefault();
                    oceanImportDetails.HblConsigneeName = string.Concat(consignee.TPName, "/", consignee.TPCode);
                }

                if (data.HblNotifyId != null)
                {
                    var notify = tradePartners.Where(w => w.Id == data.HblNotifyId).FirstOrDefault();
                    oceanImportDetails.HblNotifyName = string.Concat(notify.TPName, "/", notify.TPCode);
                }

                if (data.FdestId != null)
                {
                    var FDest = portMangements.Where(w => w.Id == data.FdestId).FirstOrDefault();
                    oceanImportDetails.FdestName = FDest?.PortName;
                }

                if (mbl.FdestId != null)
                {
                    var FDest = portMangements.Where(w => w.Id == mbl.FdestId).FirstOrDefault();
                    oceanImportDetails.MblFdestName = FDest?.PortName;
                }
                if (data.EmptyPickupId != null)
                {
                    var EmptyPickup = tradePartners.Where(w => w.Id == data.EmptyPickupId).FirstOrDefault();
                    oceanImportDetails.EmptyPickupName = string.Concat(EmptyPickup?.TPName, "/", EmptyPickup?.TPCode)?.TrimStart('/');
                  
                }
                if (data.DeliveryToId != null)
                {
                    var DeliveryTo = tradePartners.Where(w => w.Id == data.DeliveryToId).FirstOrDefault();
                    oceanImportDetails.DeliveryToName = string.Concat(DeliveryTo?.TPName, "/", DeliveryTo?.TPCode)?.TrimStart('/');
                }
                if (data.FbaFcId != null)
                {

                    var Fbafc = tradePartners.Where(w => w.Id == data.FbaFcId).FirstOrDefault();
                    oceanImportDetails.DeliveryToName = string.Concat(Fbafc?.TPName, "/", Fbafc?.TPCode)?.TrimStart('/');

                }
                if (data.PorId != null)
                {
                    var por = portMangements.Where(w => w.Id == data.PorId).FirstOrDefault();
                    oceanImportDetails.PorName = por?.PortName;
                }

                if (mbl.PorId != null)
                {
                    var por = portMangements.Where(w => w.Id == mbl.PorId).FirstOrDefault();
                    oceanImportDetails.MPorName = por?.PortName;
                }
                if (mbl.PolId != null)
                {
                    var por = portMangements.Where(w => w.Id == mbl.PolId).FirstOrDefault();
                    oceanImportDetails.MPolName = por?.PortName;
                }
                if (data.PolId != null)
                {
                    var pol = portMangements.Where(w => w.Id == data.PolId).FirstOrDefault();
                    oceanImportDetails.PolName = pol?.PortName;
                }

                if (data.DelId != null)
                {
                    var del = portMangements.Where(w => w.Id == data.DelId).FirstOrDefault();
                    oceanImportDetails.DelName = del?.PortName;
                }
                if (data.CargoPickupId != null)
                {
                    oceanImportDetails.CargoPickUp = new TradePartnerDto();
                    var del = tradePartners.Where(w => w.Id == data.CargoPickupId).FirstOrDefault();
                    oceanImportDetails.CargoPickUp = del;
                }
                if (mbl.FreightTermId != null)
                {
                    var freightTerm = sysCodes.Where(w => w.Id == mbl.FreightTermId).FirstOrDefault();
                    oceanImportDetails.FreightTermName = freightTerm?.ShowName;
                }

                if (mbl.MblOverseaAgentId != null)
                {
                    var overSeaAgent = tradePartners.Where(w => w.Id == mbl.MblOverseaAgentId).FirstOrDefault();
                    oceanImportDetails.MblOverseaAgentName = string.Concat(overSeaAgent.TPName, "/", overSeaAgent.TPCode);
                }

                if (data.SvcTermToId != null)
                {
                    var svcTo = sysCodes.Where(w => w.Id == mbl.SvcTermToId).FirstOrDefault();
                    oceanImportDetails.SvcTermToName = svcTo?.ShowName;
                }

                if (data.SvcTermFromId != null)
                {
                    var svcTo = sysCodes.Where(w => w.Id == mbl.SvcTermFromId).FirstOrDefault();
                    oceanImportDetails.SvcTermFromName = svcTo?.ShowName;
                }

                if (mbl.MblCarrierId != null)
                {
                    var carrier = tradePartners.Where(w => w.Id == mbl.MblCarrierId).FirstOrDefault();
                    oceanImportDetails.MblCarrierName = carrier.TPName + "/" + carrier.TPCode;
                }
                if (mbl.CyLocationId != null)
                {
                    var cyLocation = countries.Where(w => w.Id == mbl.CyLocationId).FirstOrDefault();
                    oceanImportDetails.CyLocationName = cyLocation?.CountryName;
                }
                if (data.PodId != null)
                {
                    var pod = portMangements.Where(w => w.Id.Equals(data.PodId)).FirstOrDefault();
                    oceanImportDetails.PodName = pod?.PortName;
                }

                if (mbl.PodId != null)
                {
                    var pod = portMangements.Where(w => w.Id.Equals(mbl.PodId)).FirstOrDefault();
                    oceanImportDetails.MPodName = pod?.PortName;
                }

                if (mbl.ForwardingAgentId != null)
                {
                    var ForwardingAgent = tradePartners.Where(w => w.Id.Equals(mbl.ForwardingAgentId)).FirstOrDefault();
                    oceanImportDetails.ForwardingAgentName = ForwardingAgent.TPName + "/" + ForwardingAgent.TPCode;
                }

                if (mbl.MblCarrierId != null)
                {
                    var MblCarrier = tradePartners.Where(w => w.Id.Equals(mbl.MblCarrierId)).FirstOrDefault();
                    oceanImportDetails.MblCarrierName = MblCarrier.TPName + "/" + MblCarrier.TPCode;
                }
                if (data.TruckerId != null)
                {
                    var trucker = tradePartners.Where(w => w.Id.Equals(data.TruckerId)).FirstOrDefault();
                    oceanImportDetails.TruckerName = trucker.TPName + "/" + trucker.TPCode;
                }

                if (data.AgentId != null)
                {
                    var agent = tradePartners.Where(w => w.Id.Equals(data.AgentId)).FirstOrDefault();
                    oceanImportDetails.HblAgentName = agent.TPName + "/" + agent.TPCode;
                }

                oceanImportDetails.HblOperatorName = _currentUser.Name + " " + _currentUser.SurName;
                oceanImportDetails.CurrentDate = DateTime.Now;
                oceanImportDetails.HblNo = data.HblNo;
                oceanImportDetails.SoNo = mbl.SoNo;
                oceanImportDetails.HblSoNo = data.SoNo;
                oceanImportDetails.MblFillingNo = mbl.FilingNo;
                //oceanExportDetails.ItnNo = data.ItnNo;
                oceanImportDetails.MblDel = mbl.Del?.PortName;
                oceanImportDetails.LCNo = data.LcNo;
                oceanImportDetails.LCIssueBankName = data.LcIssueBank;
                oceanImportDetails.LCIssueDate = data.LcIssueDate;
                oceanImportDetails.FdestEta = data.FdestEta;
                oceanImportDetails.PolEtd = mbl.PolEtd;

           
                oceanImportDetails.PorEtd = data.PorEtd;
                oceanImportDetails.MPorEtd = mbl.PorEtd;
                oceanImportDetails.PodEta = data.PodEta;
                oceanImportDetails.MPodEta = mbl.PodEta;
                oceanImportDetails.DelEta = data.DelEta;
                oceanImportDetails.VesselName = mbl.VesselName;
                oceanImportDetails.Voyage = mbl.Voyage;
                oceanImportDetails.Mark = data.Mark;
                oceanImportDetails.MblNo = mbl.MblNo;
                oceanImportDetails.MblPostDate = mbl.PostDate;
                oceanImportDetails.EmptyPickUpDate = data.CargoArrivalDate;
                oceanImportDetails.EarlyReturnDateTime = data.EarlyReturnDateTime;
                oceanImportDetails.AgentRefNo = mbl.AgentRefNo;
                oceanImportDetails.Description = data.Description;
                oceanImportDetails.ExtraProperties = data.ExtraProperties;
             
            }

            return oceanImportDetails;
        }
        public async Task DeleteMultipleHblsAsync(Guid[] Ids)
        {
            if (Ids.Length > 1) {
                await _repository.DeleteManyAsync(Ids);
            } else if (Ids.Length == 1) {
                await _repository.DeleteAsync(Ids[0]);
            }
        }
        public async Task SetLockOrUnlockStatusOceanImportHblAsync(Guid[] Ids, bool IsLock)
        {
            foreach (var Id in Ids)
            {
                var Hbl = await _repository.GetAsync(Id);

                Hbl.IsLocked = IsLock;

                await _repository.UpdateAsync(Hbl);
            }
        }

        public async Task SaveAssignContainerToHblAsync(OceanImportHblAppModel AppModel)
        {
            var Ids = AppModel.Ids;
            var Containers = AppModel.Containers;

            foreach (var Id in Ids)
            {
                foreach (var Container in Containers)
                {
                    QueryHblDto queryHblDto = new QueryHblDto() { Id = Id };
                    var OceanImportHbl = await this.GetHblById(queryHblDto);

                    if (OceanImportHbl.Id != Guid.Empty)
                    {
                        CreateUpdateContainerDto containerDto = new CreateUpdateContainerDto()
                        {
                            ContainerNo = Container
                        };

                        await _containerRepository.InsertAsync(ObjectMapper.Map<CreateUpdateContainerDto, Container>(containerDto));
                    }
                }
            }
        }
    }
}