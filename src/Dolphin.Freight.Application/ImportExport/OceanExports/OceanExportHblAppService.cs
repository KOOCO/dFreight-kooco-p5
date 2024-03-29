﻿using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using System.Linq.Dynamic.Core;
using Volo.Abp;
using NPOI.HSSF.Record;
using NPOI.DDF;
using Volo.Abp.Validation.Localization;
using Volo.Abp.Auditing;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanImports;
using Volo.Abp.ObjectMapping;
using NPOI.SS.Formula.Functions;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Dolphin.Freight.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class OceanExportHblAppService :
        CrudAppService<
            OceanExportHbl, 
            OceanExportHblDto, 
            Guid, 
            PagedAndSortedResultRequestDto, 
            CreateUpdateOceanExportHblDto>, 
        IOceanExportHblAppService 
    {
        private readonly IRepository<OceanExportHbl, Guid> _repository;
        private readonly IRepository<OceanExportMbl, Guid> _mblRepository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly IRepository<Port, Guid> _portRepository;
        private readonly IRepository<PortsManagement, Guid> _portsManagementRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IRepository<Container, Guid> _containerRepository;
        private readonly IContainerAppService _containerAppService;
        private readonly ICurrentUser _currentUser;
        public OceanExportHblAppService(IRepository<OceanExportHbl, Guid> repository, ICurrentUser currentUser, IRepository<PortsManagement, Guid> portsManagementRepository,
            IRepository<SysCode, Guid> sysCodeRepository, IRepository<OceanExportMbl, Guid> mblRepository, IRepository<Substation, Guid> substationRepository, IContainerAppService containerAppService, 
            IRepository<Port, Guid>  portRepository, IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository, IRepository<Container, Guid> containerRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _mblRepository = mblRepository;
            _substationRepository = substationRepository;
            _portRepository = portRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _portsManagementRepository = portsManagementRepository;
            _containerRepository = containerRepository;
            _currentUser = currentUser;
            _containerAppService = containerAppService;
            /*
            GetPolicyName = OceanExportPermissions.OceanExportHbls.Default;
            GetListPolicyName = OceanExportPermissions.OceanExportHbls.Default;
            CreatePolicyName = OceanExportPermissions.OceanExportHbls.Create;
            UpdatePolicyName = OceanExportPermissions.OceanExportHbls.Edit;
            DeletePolicyName = OceanExportPermissions.OceanExportHbls.Delete;*/
        }
        public async Task<PagedResultDto<OceanExportHblDto>> QueryListAsync(QueryHblDto query)
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
            Dictionary<Guid, OceanExportMbl> mdictionary = new();
            if (mbls != null) 
            { 
                foreach(var mbl in mbls) 
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
            var OceanExportHbls = await _repository.GetQueryableAsync();
             OceanExportHbls = OceanExportHbls.WhereIf(!string.IsNullOrWhiteSpace(query.Search), x=>x.ItnNo
                                    .Contains(query.Search) || x.HblNo
                                    .Contains(query.Search) || x.Mbl.SoNo
                                    .Contains(query.Search) || x.Mbl.Office.SubstationName
                                    .Contains(query.Search) || x.HblShipper.TPName
                                    .Contains(query.Search) || x.HblConsignee.TPName
                                    .Contains(query.Search))
                                    .WhereIf(query.MblId != null && query.MblId == Guid.Empty, x=>x.MblId.Value
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
            List<OceanExportHbl> rs = OceanExportHbls.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<OceanExportHblDto> list = new List<OceanExportHblDto>();
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var dto = ObjectMapper.Map<OceanExportHbl, OceanExportHblDto>(pu);
                    if (mdictionary.ContainsKey(dto.MblId))
                    {
                        dto.FilingNo = mdictionary[dto.MblId].FilingNo;
                        dto.MblNo = mdictionary[dto.MblId].SoNo;
                        if (mdictionary[dto.MblId].OfficeId.HasValue && sdictionary.ContainsKey(mdictionary[dto.MblId].OfficeId.Value))
                            dto.OfficeName = sdictionary[mdictionary[dto.MblId].OfficeId.Value];

                        //SysCode
                        if (mdictionary[dto.MblId].OblTypeId != null) dto.OblTypeName = dictionary[mdictionary[dto.MblId].OblTypeId.Value];
                    }

                    //人
                    if (dto.AgentId.HasValue && tdictionary.ContainsKey(dto.AgentId.Value))
                        dto.AgentName = tdictionary[dto.AgentId.Value];
                    if (dto.HblShipperId.HasValue && tdictionary.ContainsKey(dto.HblShipperId.Value))
                        dto.HblShipperName = tdictionary[dto.HblShipperId.Value];
                    if (dto.HblConsigneeId.HasValue && tdictionary.ContainsKey(dto.HblConsigneeId.Value)) 
                        dto.HblConsigneeName = tdictionary[dto.HblConsigneeId.Value];
                    if (mdictionary.ContainsKey(dto.MblId))
                    {
                        if (mdictionary[dto.MblId].MblCarrierId.HasValue && tdictionary.ContainsKey(mdictionary[dto.MblId].MblCarrierId.Value))
                            dto.MblCarrierName = tdictionary[mdictionary[dto.MblId].MblCarrierId.Value];
                    }
                    //if (mdictionary[dto.MblId].shipModeId != null) dto.shipModeName = tdictionary[mdictionary[dto.MblId].shipModeId.Value];


                    //港口
                    if (dto.PodId.HasValue && pdictionary.ContainsKey(dto.PodId.Value))
                        dto.PodName = pdictionary[dto.PodId.Value];
                    if (dto.PolId.HasValue && pdictionary.ContainsKey(dto.PolId.Value))
                        dto.PolName = pdictionary[dto.PolId.Value];
                    if (dto.PorId.HasValue && pdictionary.ContainsKey(dto.PorId.Value))
                        dto.PorName = pdictionary[dto.PorId.Value];
                    if (dto.DelId.HasValue && pdictionary.ContainsKey(dto.DelId.Value))
                        dto.DelName = pdictionary[dto.DelId.Value];
                    if (dto.FdestId.HasValue && pdictionary.ContainsKey(dto.FdestId.Value))
                        dto.FdestName = pdictionary[dto.FdestId.Value];

                    list.Add(dto);
                }
            }
            PagedResultDto<OceanExportHblDto> listDto = new PagedResultDto<OceanExportHblDto>();
            listDto.Items = list;
            listDto.TotalCount = OceanExportHbls.Count();
            return listDto;
        }
        public async Task<IList<OceanExportHblDto>> QueryListByMidAsync(QueryHblDto query) {
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.CodeValue);
                }
            }
            var OceanExportHbls = await _repository.GetListAsync();
            List<OceanExportHbl> rs;
            List<OceanExportHblDto> list = new List<OceanExportHblDto>();
            if (query != null && query.MblId != null)
            {
                rs = OceanExportHbls.Where(x => x.MblId.Equals(query.MblId.Value)).ToList();
            }
            else
            {
                rs = OceanExportHbls;
            }
            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<OceanExportHbl, OceanExportHblDto>(pu);
                    if (pud.CardColorId != null) pud.CardColorValue = dictionary[pud.CardColorId.Value];
                    if(pud.ColorRemarkId != null)pud.ColorRemarkValue = dictionary[pud.ColorRemarkId.Value];
                    list.Add(pud);
                }
            }
            return list;
        }
        public async Task<CreateUpdateOceanExportHblDto> GetHblById(QueryHblDto query)
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
            var oceanExportHbl = await _repository.GetAsync(query.Id.GetValueOrDefault());
            var rs = ObjectMapper.Map<OceanExportHbl, CreateUpdateOceanExportHblDto>(oceanExportHbl);
            if (rs.CardColorId != null) rs.CardColorValue = dictionary[rs.CardColorId.Value];
            return rs;
        }
        public async Task<List<OceanExportHblDto>> GetHblCardsById(Guid Id, bool isAsc = true, int sortType = 1, string ContainerId = "") {
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
            else if (isAsc) {
                if (sortType == 0)
                {
                    data = data.ToList();
                }
            }
            else {
                if (sortType == 1)
                {
                    data = data.OrderBy(x => x.HblNo).ToList();
                }
                else
                {
                    data = data.OrderBy(x => x.CreationTime).ToList();

                }
            }

            var retVal = ObjectMapper.Map<List<OceanExportHbl>, List<OceanExportHblDto>>(data);
            
            Dictionary<string, string> dictHblContains = new();
            
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
                    var cardColor = sysCodes.Where(w => w.Id == item.CardColorId).FirstOrDefault();
                    item.CardColorValue = cardColor.CodeValue;
                }

                var HblContainers = await _containerAppService.GetContainersByExtraPropertiesHblIds(item.Id, item.MblId);

                foreach (var items in HblContainers)
                {
                    var hbl = items.ExtraProperties.GetValueOrDefault("ContainerDataForHbls");

                    var HblContainer = new List<CreateUpdateContainerDto>();

                    if (hbl is not null)
                    {
                        HblContainer = JsonConvert.DeserializeObject<List<CreateUpdateContainerDto>>(hbl.ToString());
                    }

                    item.Measurement = item.Measurement + HblContainer.Sum(x => x.PackageMeasure.Value).ToString("N2") + " CBM";
                    item.Weight = item.Weight + HblContainer.Sum(x => x.PackageWeight.Value).ToString("N2") + " KG";
                    item.PackageType = item.PackageType + HblContainer.Sum(x => x.PackageNum.Value).ToString();
                }
                
                if (ContainerId is not null && ContainerId != "")
                {
                    List<CreateUpdateContainerDto> containersMbl = await _containerAppService.GetContainerByMblId(item.MblId);
                    item.ContainerIds = containersMbl.Select(s => s.Id.ToString()).ToArray();

                    List<CreateUpdateContainerDto> containers = await _containerAppService.GetContainerListByHblId(item.Id);
                    item.HblVerticalContainers = string.Join(",", containers.Select(s => s.Id.ToString()));

                    item.HblContainers = string.Join(",", containersMbl.Where(w => w.Id == Guid.Parse(ContainerId)).Select(s => Convert.ToString(s.ExtraProperties.GetValueOrDefault("HblIds"))));

                    string currentContainerHbls = string.Join("", containersMbl.Where(w => w.Id == Guid.Parse(ContainerId)).Select(s => Convert.ToString(s.ExtraProperties.GetValueOrDefault("HblIds"))));
                    if (currentContainerHbls.Contains(item.Id.ToString()))
                    {
                        dictHblContains.Add(item.Id.ToString(), ContainerId);
                    }
                    item.HblContainerIdContains = dictHblContains;
                }
            }
            return retVal;
        }
        public async Task<OceanExportHblDto> GetHblCardById(Guid Id)
        {
            if (await _repository.AnyAsync(f => f.Id == Id))
            {
                var data = await _repository.GetAsync(f => f.Id == Id, true);
                var retVal = ObjectMapper.Map<OceanExportHbl, OceanExportHblDto>(data);
                return retVal;
            }
            return new OceanExportHblDto();
        }
        public async Task LockedOrUnLockedOceanExportHblAsync(QueryHblDto query)
        {
            var Hbl = await _repository.GetAsync(query.HblId.Value);
            if (Hbl.IsLocked == true)
            {
                Hbl.IsLocked = false;

                await _repository.UpdateAsync(Hbl);
            }
            else
            {
                Hbl.IsLocked = true;

                await _repository.UpdateAsync(Hbl);
            }
        }
        public async Task<OceanExportHblDto> GetHawbCardById(Guid Id)
        {
            if (await _repository.AnyAsync(f => f.Id == Id))
            {
                var data = await _repository.GetAsync(f => f.Id == Id);
                var retVal = ObjectMapper.Map<OceanExportHbl, OceanExportHblDto>(data);
                return retVal;
            }

            return new OceanExportHblDto();
        }
        public async Task<OceanExportDetails> GetOceanExportDetailsById(Guid Id)
        {
            var oceanExportDetails = new OceanExportDetails();
            var tradePartners = ObjectMapper.Map<List<TradePartners.TradePartner>, List<TradePartnerDto>>(await _tradePartnerRepository.GetListAsync());
            var portMangements = ObjectMapper.Map<List<PortsManagement>, List<PortsManagementDTO>>(await _portsManagementRepository.GetListAsync());
            var sysCodes = ObjectMapper.Map<List<SysCode>, List<SysCodeDto>>(await _sysCodeRepository.GetListAsync());
            var substations = ObjectMapper.Map<List<Substation>, List<SubstationDto>>(await _substationRepository.GetListAsync());
            
            var data = await Repository.GetAsync(Id);

            if (data != null)
            {
                oceanExportDetails = ObjectMapper.Map<OceanExportHbl, OceanExportDetails>(data);

                var mbl = await _mblRepository.GetAsync(data.MblId.GetValueOrDefault());

                if (data.HblShipperId != null)
                {
                    var shipper = tradePartners.Where(w => w.Id == data.HblShipperId).FirstOrDefault();
                    oceanExportDetails.ShippingAgentName = string.Concat(shipper.TPName, "/", shipper.TPCode);
                }

                if (data.HblConsigneeId != null)
                {
                    var consignee = tradePartners.Where(w => w.Id == data.HblConsigneeId).FirstOrDefault();
                    oceanExportDetails.HblConsigneeName = string.Concat(consignee.TPName, "/", consignee.TPCode);
                }

                if (data.HblNotifyId != null)
                {
                    var notify = tradePartners.Where(w => w.Id == data.HblNotifyId).FirstOrDefault();
                    oceanExportDetails.HblNotifyName = string.Concat(notify.TPName, "/", notify.TPCode);
                }

                if (data.FdestId != null)
                {
                    var FDest = portMangements.Where(w => w.Id == data.FdestId).FirstOrDefault();
                    oceanExportDetails.FdestName = FDest?.PortName;
                }

                 if (mbl.FdestId != null)
                {
                    var FDest = portMangements.Where(w => w.Id == mbl.FdestId).FirstOrDefault();
                    oceanExportDetails.MblFdestName = FDest?.PortName;
                }
                if (data.EmptyPickupId != null)
                {
                    var EmptyPickup = tradePartners.Where(w => w.Id == data.EmptyPickupId).FirstOrDefault();
                    oceanExportDetails.EmptyPickupName = string.Concat(EmptyPickup?.TPName, "/", EmptyPickup?.TPCode)?.TrimStart('/');
                }
                if (data.DeliveryToId != null)
                {
                    var DeliveryTo = tradePartners.Where(w => w.Id == data.DeliveryToId).FirstOrDefault();
                    oceanExportDetails.DeliveryToName = string.Concat(DeliveryTo?.TPName, "/", DeliveryTo?.TPCode)?.TrimStart('/');
                }
                if (data.PorId != null)
                {
                    var por = portMangements.Where(w => w.Id == data.PorId).FirstOrDefault();
                    oceanExportDetails.PorName = por?.PortName;
                }

                if (mbl.PorId != null)
                {
                    var por = portMangements.Where(w => w.Id == mbl.PorId).FirstOrDefault();
                    oceanExportDetails.MPorName = por?.PortName;
                }

                if (data.PolId != null)
                {
                    var pol = portMangements.Where(w => w.Id == data.PolId).FirstOrDefault();
                    oceanExportDetails.PolName = pol?.PortName;
                }

                if (data.DelId != null)
                {
                    var del = portMangements.Where(w => w.Id == data.DelId).FirstOrDefault();
                    oceanExportDetails.DelName = del?.PortName;
                }
                if (data.CargoPickupId != null)
                {
                    oceanExportDetails.CargoPickUp = new TradePartnerDto();
                    var del = tradePartners.Where(w => w.Id == data.CargoPickupId).FirstOrDefault();
                    oceanExportDetails.CargoPickUp = del;
                }
                if (mbl.FreightTermId != null)
                {
                    var freightTerm = sysCodes.Where(w => w.Id == mbl.FreightTermId).FirstOrDefault();
                    oceanExportDetails.FreightTermName = freightTerm?.ShowName;
                }

                if (mbl.MblOverseaAgentId != null)
                {
                    var overSeaAgent = tradePartners.Where(w => w.Id == mbl.MblOverseaAgentId).FirstOrDefault();
                    oceanExportDetails.MblOverseaAgentName = string.Concat(overSeaAgent.TPName, "/", overSeaAgent.TPCode);
                }

                if (data.SvcTermToId != null)
                {
                    var svcTo = sysCodes.Where(w => w.Id == mbl.SvcTermToId).FirstOrDefault();
                    oceanExportDetails.SvcTermToName = svcTo?.ShowName;
                }

                if (data.SvcTermFromId != null)
                {
                    var svcTo = sysCodes.Where(w => w.Id == mbl.SvcTermFromId).FirstOrDefault();
                    oceanExportDetails.SvcTermFromName = svcTo?.ShowName;
                }

                if (mbl.MblCarrierId != null)
                {
                    var carrier = tradePartners.Where(w => w.Id == mbl.MblCarrierId).FirstOrDefault();
                    oceanExportDetails.MblCarrierName = carrier.TPName + "/" + carrier.TPCode;
                }

                if (data.PodId != null)
                {
                    var pod = portMangements.Where(w => w.Id.Equals(data.PodId)).FirstOrDefault();
                    oceanExportDetails.PodName = pod?.PortName;
                }
                
                if (mbl.PodId != null)
                {
                    var pod = portMangements.Where(w => w.Id.Equals(mbl.PodId)).FirstOrDefault();
                    oceanExportDetails.MPodName = pod?.PortName;
                }

                if (mbl.ForwardingAgentId != null)
                {
                    var ForwardingAgent = tradePartners.Where(w => w.Id.Equals(mbl.ForwardingAgentId)).FirstOrDefault();
                    oceanExportDetails.ForwardingAgentName = ForwardingAgent.TPName + "/" + ForwardingAgent.TPCode;
                }
                
                if (mbl.MblCarrierId != null)
                {
                    var MblCarrier = tradePartners.Where(w => w.Id.Equals(mbl.MblCarrierId)).FirstOrDefault();
                    oceanExportDetails.MblCarrierName = MblCarrier.TPName + "/" + MblCarrier.TPCode;
                }
                if (data.TruckerId != null)
                {
                    var trucker = tradePartners.Where(w => w.Id.Equals(data.TruckerId)).FirstOrDefault();
                    oceanExportDetails.TruckerName = trucker.TPName + "/" + trucker.TPCode;
                }

                if (data.AgentId != null)
                {
                    var agent = tradePartners.Where(w => w.Id.Equals(data.AgentId)).FirstOrDefault();
                    oceanExportDetails.HblAgentName = agent.TPName + "/" + agent.TPCode;
                }

                if (data.ForwardingAgentId != null)
                {
                    var ForwardingAgent = tradePartners.Where(w => w.Id.Equals(data.ForwardingAgentId)).FirstOrDefault();
                    oceanExportDetails.ForwardingAgentName = ForwardingAgent.TPName + "/" + ForwardingAgent.TPCode;
                }
                

                oceanExportDetails.HblOperatorName = _currentUser.Name + " " + _currentUser.SurName;
                oceanExportDetails.CurrentDate = DateTime.Now;
                oceanExportDetails.HblNo = data.HblNo;
                oceanExportDetails.SoNo = mbl.SoNo;
                oceanExportDetails.HblSoNo = data.SoNo;
                oceanExportDetails.DocNo = mbl.FilingNo;
                oceanExportDetails.ItnNo = data.ItnNo;
                oceanExportDetails.MblDel = mbl.Del?.PortName;
                oceanExportDetails.LCNo = data.LcNo;
                oceanExportDetails.LCIssueBankName = data.LcIssueBank;
                oceanExportDetails.LCIssueDate = data.LcIssueDate;
                oceanExportDetails.FdestEta = data.FdestEta;
                oceanExportDetails.PolEtd = mbl.PolEtd;
                oceanExportDetails.PorEtd = data.PorEtd;
                oceanExportDetails.MPorEtd = mbl.PorEtd;
                oceanExportDetails.PodEta = data.PodEta;
                oceanExportDetails.MPodEta = mbl.PodEta;
                oceanExportDetails.DelEta = data.DelEta;
                oceanExportDetails.VesselName = mbl.VesselName;
                oceanExportDetails.Voyage = mbl.Voyage;
                oceanExportDetails.Mark = data.Mark;
                oceanExportDetails.MblNo = mbl.MblNo;
                oceanExportDetails.MblPostDate = mbl.PostDate;

                oceanExportDetails.EarlyReturnDateTime= data.EarlyReturnDateTime;

                oceanExportDetails.Description = data.Description;
            }  

            return oceanExportDetails;
        }
        public async Task DeleteMultipleHblsAsync(Guid[] ids)
        {
            foreach (var id in ids)
            {
                var hbl = await _repository.GetAsync(id);

                hbl.IsDeleted = true;

                await _repository.UpdateAsync(hbl);
            }
        }
        public async Task SetLockStatusOnOceanExportHblAsync(Guid[] ids, bool isLocked)
        {
            foreach (var id in ids)
            {
                var hbl = await _repository.GetAsync(id);

                hbl.IsLocked = isLocked;

                await _repository.UpdateAsync(hbl);
            }
        }
        public async void SaveAssignContainerToHblAsync(OceanExportHblAppModel AppModel, bool IsSave = true)
        {
            var Ids = AppModel.HblIds.ToList();
            var ContainerId = AppModel.Containersid;
            var MblId = AppModel.MblId;

            var containers = new List<Container>();

            containers = await _containerRepository.GetListAsync();

            var containerList = containers.Where(w => w.MblId == MblId && w.Id == Guid.Parse(ContainerId)).ToList();

            foreach (var container in containerList)
            {
                if (IsSave)
                {
                    container.ExtraProperties.Remove("HblIds");
                    container.ExtraProperties.Add("HblIds", Ids);
                }
                else
                {
                    container.ExtraProperties.Remove("HblIds");
                }

                IHttpContextAccessor Http = new HttpContextAccessor();

                if (Http.HttpContext.Request.Host.Host == "localhost")
                {
                    using (var dbContext = new FreightDbContextFactory().CreateDbContext(new string[] { }))
                    {
                        dbContext.Update(container);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    await _containerRepository.UpdateAsync(container);
                }
            }
        }
        public async Task<List<CreateUpdateContainerDto>> GetContainerByHblExtraProperties(Guid Id)
        {
            List<CreateUpdateContainerDto> CreateUpdateContainerDtos = new List<CreateUpdateContainerDto>();

            var Hbl = await _repository.GetAsync(Id);

            string ContainerIds = Hbl.ExtraProperties.GetValueOrDefault("ContainerIdList").ToString();

            List<string> ContainerIdsList = new List<string>();

            ContainerIdsList = ContainerIds.Split(",").ToList();

            foreach (var ContainerId in ContainerIdsList)
            {
                var container = await _containerRepository.GetAsync(Guid.Parse(ContainerId));

                CreateUpdateContainerDtos.Add(ObjectMapper.Map<Container, CreateUpdateContainerDto>(container));
            }

            return CreateUpdateContainerDtos;
        }
        public async Task SaveAssignContainerNoToHblAsync(OceanExportHblAppModel AppModel, bool IsSave = true)
        {
            var MblId = AppModel.MblId;
            var HblId = AppModel.HblId;
            var containers = await _containerRepository.GetQueryableAsync();
            var containerList = containers.Where(w => w.MblId == MblId).ToList();

            foreach (var container in containerList)
            {
                if (IsSave)
                {
                    var extraProps = container.ExtraProperties.GetValueOrDefault("HblIds");

                    if (HblId != Guid.Empty)
                    {
                        if (extraProps != null && !extraProps.ToString().Contains(Convert.ToString(HblId)) || extraProps is null)
                        {
                            if (extraProps is null)
                            {
                                List<Guid> existingExtraProps = new List<Guid>();

                                existingExtraProps.Add(HblId);

                                container.ExtraProperties.Remove("HblIds");

                                container.ExtraProperties.Add("HblIds", existingExtraProps);
                            }
                            else
                            {
                                List<Guid> existingExtraProps = JsonConvert.DeserializeObject<List<Guid>>(extraProps.ToString());

                                existingExtraProps.Add(HblId);

                                container.ExtraProperties.Remove("HblIds");

                                container.ExtraProperties.Add("HblIds", existingExtraProps);
                            }

                            var dto = ObjectMapper.Map<Container, CreateUpdateContainerDto>(container);

                            await _containerAppService.UpdateAsync(container.Id, dto);
                        }
                    }
                }
                else
                {
                    var extraProps = container.ExtraProperties.GetValueOrDefault("HblIds");

                    if (HblId != Guid.Empty)
                    {
                        if (extraProps != null && extraProps.ToString().Contains(Convert.ToString(HblId)))
                        {
                            List<string> existingList = JsonConvert.DeserializeObject<List<string>>(extraProps.ToString());

                            existingList.Remove(HblId.ToString());

                            string UpdatedExtraProps = JsonConvert.SerializeObject(existingList);

                            container.ExtraProperties.Remove("HblIds");

                            container.ExtraProperties.Add("HblIds", UpdatedExtraProps);

                            var dto = ObjectMapper.Map<Container, CreateUpdateContainerDto>(container);

                            await _containerAppService.UpdateAsync(container.Id, dto);
                        }
                    }
                }
            }
        }
        public async void SaveAssignSingleContainerNoToHblAsync(OceanExportHblAppModel AppModel, bool IsSave = true)
        {
            var MblId = AppModel.MblId;
            var HblIds = AppModel.HblIds.ToList();
            var UncheckedHblId = AppModel.UncheckedHblId;
            var ContainerId = AppModel.Containersid;
            
            var containers = new List<Container>();

            containers = await _containerRepository.GetListAsync();

            var containerList = containers.Where(w => w.MblId == MblId && w.Id == Guid.Parse(ContainerId)).ToList();

            foreach (var container in containerList)
            {
                container.ExtraProperties.Remove("HblIds");
                container.ExtraProperties.Add("HblIds", HblIds);
                
                if (!IsSave && !UncheckedHblId.Equals(Guid.Empty) && container.HblId.Equals(UncheckedHblId))
                {
                    container.HblId = Guid.Empty;
                }

                IHttpContextAccessor Http = new HttpContextAccessor();

                if (Http.HttpContext.Request.Host.Host == "localhost")
                {
                    using (var dbContext = new FreightDbContextFactory().CreateDbContext(new string[] { }))
                    {
                        dbContext.Update(container);
                        dbContext.SaveChanges();
                    }
                } 
                else
                {
                    await _containerRepository.UpdateAsync(container);
                }
            }
        }
        public async Task SaveDeAssignContainerNoFromHblAsync(OceanExportHblAppModel AppModel)
        {
            var MblId = AppModel.MblId;
            var HblId = AppModel.HblId;
            var containers = await _containerRepository.GetQueryableAsync();
            var containerList = containers.Where(w => w.MblId == MblId).ToList();

            foreach (var container in containerList)
            {
                container.HblId = Guid.Empty;
                var dto = ObjectMapper.Map<Container, CreateUpdateContainerDto>(container);
                await _containerAppService.UpdateAsync(container.Id, dto);
            }
        }
        public async Task UpdateMblIdOfHblAsync(Guid hblId, Guid newMblId)
        {
            try
            {
                var hbl = await _repository.GetAsync(hblId);
                hbl.MblId = newMblId;
                await _repository.UpdateAsync(hbl);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task DeleteHblWithBasicAndContainerDataAsync(Guid HblId)
        {
            var hblBasicData = await GetAsync(HblId);

            hblBasicData.IsDeleted = true;

            var ContainerData = await _containerAppService.GetContainersByExtraPropertiesHblIds(hblBasicData.Id, hblBasicData.MblId);

            if (ContainerData is not null && ContainerData.Count > 0)
            {
                foreach (var Container in ContainerData)
                {
                    var extraProp = Container.ExtraProperties.GetValueOrDefault("HblIds");

                    var listOfExtraProp = JsonConvert.DeserializeObject<List<Guid>>(extraProp.ToString());

                    listOfExtraProp.Remove(HblId);

                    Container.ExtraProperties.Remove("HblIds");

                    Container.ExtraProperties.Add("HblIds", listOfExtraProp);

                    await _containerAppService.UpdateAsync(Container.Id, Container);
                }
            }

            await UpdateAsync(HblId, ObjectMapper.Map<OceanExportHblDto, CreateUpdateOceanExportHblDto>(hblBasicData));
        }
    }
}