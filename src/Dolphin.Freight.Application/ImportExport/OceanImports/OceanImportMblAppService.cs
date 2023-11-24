using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Common;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Dolphin.Freight.ImportExport.OceanImports
{
    public class OceanImportMblAppService :
        CrudAppService<
            OceanImportMbl, //IT號碼管理entity
            OceanImportMblDto, //顯示IT號碼管理用
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateOceanImportMblDto>, //新增修改IT號碼管理用
        IOceanImportMblAppService //實作IOceanImportMblAppService
    {
        private readonly ISettingProvider _settingProvider;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<OceanImportMbl, Guid> _repository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly PortsManagementAppService _portRepository1;
        private readonly IRepository<PortsManagement, Guid> _portRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IRepository<OceanImportHbl, Guid> _oceanImportHblRepository;
        private readonly IRepository<Country, Guid> _countryRepository;
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IContainerAppService _containerAppService;
        public OceanImportMblAppService(IRepository<OceanImportMbl, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<Substation, Guid> substationRepository,
                                         PortsManagementAppService portRepository1, IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository,
                                        IRepository<OceanImportHbl, Guid> oceanImportHblRepository, IIdentityUserAppService identityUserAppService, IContainerAppService containerAppService,
                                        IRepository<Country, Guid> countryRepository, IIdentityUserRepository identityUserRepository, IRepository<PortsManagement, Guid> portRepository,
                                        IOceanImportHblAppService oceanImportHblAppService, ISettingManager settingManager, ISettingProvider settingProvider)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _substationRepository = substationRepository;
            _portRepository1 = portRepository1;
            _tradePartnerRepository = tradePartnerRepository;
            _oceanImportHblRepository = oceanImportHblRepository;
            _identityUserAppService=identityUserAppService;
            _portRepository = portRepository;
            _countryRepository = countryRepository;
            _oceanImportHblAppService = oceanImportHblAppService;
            _identityUserRepository = identityUserRepository;
            _containerAppService = containerAppService;
            _settingManager = settingManager;
            _settingProvider = settingProvider;

            /*
            GetPolicyName = OceanImportPermissions.OceanImportMbls.Default;
            GetListPolicyName = OceanImportPermissions.OceanImportMbls.Default;
            CreatePolicyName = OceanImportPermissions.OceanImportMbls.Create;
            UpdatePolicyName = OceanImportPermissions.OceanImportMbls.Edit;
            DeletePolicyName = OceanImportPermissions.OceanImportMbls.Delete;*/
        }
        public async Task<PagedResultDto<OceanImportMblDto>> QueryListAsync(QueryMblDto query)
        {
            var substations = await _substationRepository.GetListAsync();
            Dictionary<Guid, string> substationsDictionary = new Dictionary<Guid, string>();
            if (substations != null)
            {
                foreach (var substation in substations)
                {
                    substationsDictionary.Add(substation.Id, substation.SubstationName + "(" + substation.AbbreviationName + ")");
                }
            }
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            Dictionary<Guid, string> tradePartnerDictionary = new Dictionary<Guid, string>();
            if (tradePartners != null)
            {
                foreach (var tradePartner in tradePartners)
                {
                    tradePartnerDictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }
            var portManagements = await _portRepository.GetListAsync();
            Dictionary<Guid, string> portManagementsDictionary = new Dictionary<Guid, string>();
            if (portManagements != null)
            {
                foreach (var portManagement in portManagements)
                {
                    portManagementsDictionary.Add(portManagement.Id, portManagement.PortName);
                }
            }
            var SysCodes = await _sysCodeRepository.GetListAsync();
            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (SysCodes != null)
            {
                foreach (var syscode in SysCodes)
                {
                    dictionary.Add(syscode.Id, syscode.CodeValue);
                }
            }
            var Users = await _identityUserRepository.GetListAsync();
            Dictionary<Guid, string> userDictionary = new Dictionary<Guid, string>();
            if (Users != null)
            {
                foreach (var user in Users)
                {
                    dictionary.Add(user.Id, user.Name + " " + user.Surname);
                }
            }
            var OceanImportMbls = await _repository.GetQueryableAsync();
            OceanImportMbls = OceanImportMbls.WhereIf(!string.IsNullOrWhiteSpace(query.Search), x => x.MblNo
                                            .Contains(query.Search) || x.Office.SubstationName
                                            .Contains(query.Search) || x.Office.AbbreviationName
                                            .Contains(query.Search) || x.SoNo
                                            .Contains(query.Search))
                                            .WhereIf(query.CarrierId.HasValue, e => e.MblCarrierId == query.CarrierId)
                                            .WhereIf(query.ShippingAgentId.HasValue, e => e.ShippingAgentId == query.ShippingAgentId)
                                            .WhereIf(query.CyLocationId.HasValue, e => e.CyLocationId == query.CyLocationId)
                                            .WhereIf(query.CyLocationId.HasValue, e => e.CfsLocationId == query.CyLocationId)
                                            .WhereIf(query.Pol.HasValue, e => e.PolId == query.Pol)
                                            .WhereIf(query.Pod.HasValue, e => e.PodId == query.Pod)
                                            .WhereIf(query.Del.HasValue, e => e.DelId == query.Del)
                                            .WhereIf(!string.IsNullOrWhiteSpace(query.Vessel),x=>x.VesselName==query.Vessel)
                                            .WhereIf(query.SaleId.HasValue, e => e.MblSaleId == query.SaleId)
                                            .WhereIf(query.OvearseaAgentId.HasValue, e => e.MblOverseaAgentId == query.OvearseaAgentId)
                                            .WhereIf(query.OfficeId.HasValue, e => e.OfficeId == query.OfficeId)
                                            .WhereIf(query.OpId.HasValue, e => e.CreatorId == query.OpId)
                                            .WhereIf(query.BlTypeId.HasValue, e => e.BlTypeId == query.BlTypeId)
                                            .WhereIf(query.SvcTermFrom.HasValue, e => e.SvcTermFromId == query.SvcTermFrom)
                                            .WhereIf(query.SvcTermTo.HasValue, e => e.SvcTermToId == query.SvcTermTo)
                                            .WhereIf(query.Block.HasValue, e => e.IsLocked == query.Block)
                                            .WhereIf(query.CoLoaderId.HasValue, e => e.CoLoaderId == query.CoLoaderId)
                                            .WhereIf(query.PostDate.HasValue, e => e.PostDate.Date == query.PostDate.Value.Date.AddDays(1))
                                            .WhereIf(query.CreationDate.HasValue, e => e.CreationTime.Date == query.CreationDate.Value.Date.AddDays(1))
                                              .WhereIf(query.ReleaseDate.HasValue, e => e.MblReleaseDate == query.ReleaseDate.Value.Date.AddDays(1))
                                            .OrderByDescending(x => x.CreationTime);
            List<OceanImportMbl> rs = OceanImportMbls.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<OceanImportMblDto> list = new List<OceanImportMblDto>();

            if (rs.Any())
            {
                foreach (var r in rs)
                {
                    var item = ObjectMapper.Map<OceanImportMbl, OceanImportMblDto>(r);
                    item.OfficeName = substationsDictionary[r.OfficeId.Value];
                    if (r.FdestId is not null) item.FinalDestName = portManagementsDictionary[r.FdestId.Value];
                    if (r.MblCarrierId is not null) item.MblCarrierName = tradePartnerDictionary[r.MblCarrierId.Value];
                    if (r.MblOverseaAgentId is not null) item.MblOverseaAgentName = tradePartnerDictionary[r.MblOverseaAgentId.Value];
                    if (r.MblSaleId is not null) item.MblSaleName = tradePartnerDictionary[r.MblSaleId.Value];
                    if (r.MblOperatorId is not null) item.MblOperatorName = userDictionary[r.MblOperatorId.Value];
                    list.Add(item);
                }
            }
            PagedResultDto<OceanImportMblDto> listDto = new PagedResultDto<OceanImportMblDto>();
            listDto.Items = list;
            listDto.TotalCount = OceanImportMbls.Count();
            return listDto;
        }
        public async Task LockedOrUnLockedOceanImportMblAsync(QueryMblDto query)
        {
            var mbl = await _repository.GetAsync(query.MbId.Value);
            if (mbl.IsLocked == true)
            {
                mbl.IsLocked = false;
                var queryHbl = await _oceanImportHblRepository.GetQueryableAsync();
                var hbls = queryHbl.Where(w => w.MblId == mbl.Id).ToList();
                foreach (var hbl in hbls)
                {
                    hbl.IsLocked = false;

                    await _oceanImportHblRepository.UpdateAsync(hbl);
                }
                await _repository.UpdateAsync(mbl);
            } else
            {
                mbl.IsLocked = true;
                var queryHbl = await _oceanImportHblRepository.GetQueryableAsync();
                var hbls = queryHbl.Where(w => w.MblId == mbl.Id).ToList();
                foreach (var hbl in hbls)
                {
                    hbl.IsLocked = true;

                    await _oceanImportHblRepository.UpdateAsync(hbl);
                }
                await _repository.UpdateAsync(mbl);
            }
        }
        public async Task<CreateUpdateOceanImportMblDto> GetCreateUpdateOceanImportMblDtoById(Guid Id)
        {
            var oceanImportMbl = await _repository.GetAsync(Id, true);
            var dto = ObjectMapper.Map<OceanImportMbl, CreateUpdateOceanImportMblDto>(oceanImportMbl);
            var ports = await _portRepository.GetListAsync();
            Dictionary<Guid, string> pdictionary = new();
            if (ports != null && ports.Count > 0)
            {
                foreach (var port in ports)
                {
                    pdictionary.Add(port.Id, port.SubDiv + " " + port.PortName + " ( " + port.Locode + " ) ");
                }
            }
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            Dictionary<Guid, string> tdictionary = new();
            if (tradePartners != null && tradePartners.Count > 0)
            {
                foreach (var tradePartner in tradePartners)
                {
                    tdictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }
            if (dto != null)
            {
                if (dto.PodId != null) dto.PodName = pdictionary[dto.PodId.Value];
                if (dto.PolId != null) dto.PolName = pdictionary[dto.PolId.Value];
                if (dto.PorId != null) dto.PorName = pdictionary[dto.PorId.Value];
                if (dto.DelId != null) dto.DelName = pdictionary[dto.DelId.Value];
                if (dto.FdestId != null) dto.FdestName = pdictionary[dto.FdestId.Value];
                if (dto.MblCarrierId != null) dto.MblCarrierName = tdictionary[dto.MblCarrierId.Value];
                if (dto.MblOverseaAgentId != null) dto.MblOverseaAgentName = tdictionary[dto.MblOverseaAgentId.Value];
            }
            return dto;
        }
        public override async Task DeleteAsync(Guid Id)
        {
            var hbls = await _oceanImportHblRepository.GetListAsync();
            var ids = hbls.Where(w => w.MblId == Id).Select(s => s.Id);

            await Repository.DeleteAsync(Id);
            await _oceanImportHblRepository.DeleteManyAsync(ids);
        }
        public async Task<OceanImportDetails> GetOceanImportDetailsById(Guid Id)
        {
            var oceanImportDetails = new OceanImportDetails();
            var tradePartners = ObjectMapper.Map<List<TradePartners.TradePartner>, List<TradePartnerDto>>(await _tradePartnerRepository.GetListAsync());
            var portMangements = await _portRepository1.QueryListAsync();
            var sysCodes = ObjectMapper.Map<List<SysCode>, List<SysCodeDto>>(await _sysCodeRepository.GetListAsync());
            var substations = ObjectMapper.Map<List<Substation>, List<SubstationDto>>(await _substationRepository.GetListAsync());
            var countries = await _countryRepository.GetListAsync();

            var data = await Repository.GetAsync(Id);

            if (data != null)
            {
                oceanImportDetails = ObjectMapper.Map<OceanImportMbl, OceanImportDetails>(data);

                if (data.BlAcctCarrierId != null)
                {
                    var BlAcctCarrier = tradePartners.Where(w => w.Id == data.BlAcctCarrierId).FirstOrDefault();
                    oceanImportDetails.BlAcctCarrierName = string.Concat(BlAcctCarrier?.TPName, "/", BlAcctCarrier?.TPCode);
                }
                if (data.BlTypeId != null)
                {
                    oceanImportDetails.BlType = sysCodes.Where(w => w.Id == data.BlTypeId).FirstOrDefault();
                }
                if (data.CancelById != null)
                {
                    oceanImportDetails.CancelBy = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(data.CancelById.GetValueOrDefault()));
                }
                if (data.CareOfId != null)
                {
                    oceanImportDetails.CareOf = tradePartners.Where(w => w.Id == data.CareOfId).FirstOrDefault();
                }
                if (data.CargoTypeId != null)
                {
                    var CargoType = sysCodes.Where(w => w.Id == data.CargoTypeId).FirstOrDefault();
                    oceanImportDetails.CargoTypeName = CargoType?.ShowName;
                }
                if (data.CoLoaderId != null)
                {
                    var CoLoader = tradePartners.Where(w => w.Id == data.CoLoaderId).FirstOrDefault();
                    oceanImportDetails.CargoTypeName = string.Concat(CoLoader?.TPName, "/", CoLoader?.TPCode);
                }
                if (data.CyLocationId != null)
                {
                    var CY = countries.Where(w => w.Id == data.CyLocationId).FirstOrDefault();
                    oceanImportDetails.CyLocation = string.Concat(CY?.CountryName);
                }
                if (data.DelId != null)
                {
                    var Del = portMangements.Where(w => w.Id == data.DelId).FirstOrDefault();
                    oceanImportDetails.DelName = Del?.PortName;
                }
                if (data.DeliveryToId != null)
                {
                    var DeliveryTo = tradePartners.Where(w => w.Id == data.DeliveryToId).FirstOrDefault();
                    oceanImportDetails.DeliveryToName = string.Concat(DeliveryTo?.TPName, "/", DeliveryTo?.TPCode)?.TrimStart('/');
                }
                if (data.EmptyPickupId != null)
                {
                    var EmptyPickup = tradePartners.Where(w => w.Id == data.EmptyPickupId).FirstOrDefault();
                    oceanImportDetails.EmptyPickupName = string.Concat(EmptyPickup?.TPName, "/", EmptyPickup?.TPCode)?.TrimStart('/');
                }
                if (data.CfsLocationId != null)
                {
                    var cfsLocation = countries.Where(w => w.Id == data.CfsLocationId).FirstOrDefault();
                    oceanImportDetails.CfsLocation = string.Concat(cfsLocation?.CountryName)?.TrimStart('/');
                }
                if (data.FdestId != null)
                {
                    var Fdest = portMangements.Where(w => w.Id == data.FdestId).FirstOrDefault();
                    oceanImportDetails.FdestName = Fdest?.PortName;
                }
                if (data.ForwardingAgentId != null)
                {
                    var ForwardingAgent = tradePartners.Where(w => w.Id == data.ForwardingAgentId).FirstOrDefault();
                    oceanImportDetails.ForwardingAgentName = string.Concat(ForwardingAgent?.TPName, "/", ForwardingAgent?.TPCode);
                }
                if (data.FreightTermId != null)
                {
                    var FreightTerm = sysCodes.Where(w => w.Id == data.FreightTermId).FirstOrDefault();
                    oceanImportDetails.FreightTermName = FreightTerm?.ShowName;
                }
                if (data.MblBillToId != null)
                {
                    var MblBillTo = tradePartners.Where(w => w.Id == data.MblBillToId).FirstOrDefault();
                    oceanImportDetails.MblBillToName = string.Concat(MblBillTo?.TPName, "/", MblBillTo?.TPCode)?.TrimStart('/');
                }
                if (data.MblCarrierId != null)
                {
                    var MblCarrier = tradePartners.Where(w => w.Id == data.MblCarrierId).FirstOrDefault();
                    oceanImportDetails.MblCarrierName = string.Concat(MblCarrier?.TPName);
                }
                if (data.MblConsigneeId != null)
                {
                    var MblConsignee = tradePartners.Where(w => w.Id == data.MblConsigneeId).FirstOrDefault();
                    oceanImportDetails.MblConsigneeName = string.Concat(MblConsignee?.TPName, "/", MblConsignee?.TPCode)?.TrimStart('/');
                }
                if (data.MblCustomerId != null)
                {
                    var MblCustomer = tradePartners.Where(w => w.Id == data.MblCustomerId).FirstOrDefault();
                    oceanImportDetails.MblCustomerName = string.Concat(MblCustomer?.TPName, "/", MblCustomer?.TPCode)?.TrimStart('/');
                }
                if (data.MblNotifyId != null)
                {
                    var MblNotify = tradePartners.Where(w => w.Id == data.MblNotifyId).FirstOrDefault();
                    oceanImportDetails.MblNotifyName = string.Concat(MblNotify?.TPName, "/", MblNotify?.TPCode);
                }
                if (data.MblOperatorId != null || data.CreatorId != null)
                {
                    oceanImportDetails.MblOperator = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(data.MblOperatorId != null ? data.MblOperatorId.GetValueOrDefault() : data.CreatorId.GetValueOrDefault()));
                    oceanImportDetails.MblOperatorName = string.Concat(oceanImportDetails.MblOperator.UserName, " ", oceanImportDetails.MblOperator.Surname);
                }
                if (data.MblOverseaAgentId != null)
                {
                    var MblOverseaAgent = tradePartners.Where(w => w.Id == data.MblOverseaAgentId).FirstOrDefault();
                    oceanImportDetails.MblOverseaAgentName = string.Concat(MblOverseaAgent.TPName, "/", MblOverseaAgent.TPCode);
                }
                if (data.MblReferralById != null)
                {
                    var MblReferralBy = tradePartners.Where(w => w.Id == data.FdestId).FirstOrDefault();
                    oceanImportDetails.MblReferralByName = string.Concat(MblReferralBy?.TPName, "/", MblReferralBy?.TPCode);
                }
                if (data.MblSaleId != null)
                {
                    var MblSale = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(data.MblSaleId.GetValueOrDefault()));
                    oceanImportDetails.MblSaleName = string.Concat(MblSale?.Name, "/", MblSale?.Surname)?.TrimStart('/');
                }
                if (data.OblTypeId != null)
                {
                    var OblType = sysCodes.Where(w => w.Id == data.OblTypeId).FirstOrDefault();
                    oceanImportDetails.OblTypeName = OblType?.ShowName;
                }

                if (data.OfficeId != null)
                {
                    var Office = substations.Where(w => w.Id == data.OfficeId).FirstOrDefault();
                }
                if (data.PackageCategoryId != null)
                {
                    var PackageCategory = sysCodes.Where(w => w.Id == data.PackageCategoryId).FirstOrDefault();
                    oceanImportDetails.PackageCategoryName = PackageCategory?.ShowName;
                }
                if (data.PackageMeasureId != null)
                {
                    oceanImportDetails.PackageMeasure = sysCodes.Where(w => w.Id == data.PackageMeasureId).FirstOrDefault();
                }
                if (data.PackageWeightId != null)
                {
                    var PackageWeight = sysCodes.Where(w => w.Id == data.PackageWeightId).FirstOrDefault();
                    oceanImportDetails.PackageWeightName = PackageWeight?.ShowName;
                }
                if (data.PodId != null)
                {
                    var Pod = portMangements.Where(w => w.Id == data.PodId).FirstOrDefault();
                    oceanImportDetails.PodName = Pod?.PortName;
                }
                if (data.PolId != null)
                {
                    var Pol = portMangements.Where(w => w.Id == data.PolId).FirstOrDefault();
                    oceanImportDetails.PolName = Pol?.PortName;
                }
                if (data.PorId != null)
                {
                    var Por = portMangements.Where(w => w.Id == data.PorId).FirstOrDefault();
                    oceanImportDetails.PorName = Por?.PortName;
                }
                if (data.PreCarriageVesselNameId != null)
                {
                    var PreCarriageVesselName = sysCodes.Where(w => w.Id == data.PreCarriageVesselNameId).FirstOrDefault();
                    oceanImportDetails.PreCarriageVesselNameValue = PreCarriageVesselName?.ShowName;
                }
                if (data.ReleaseById != null)
                {
                    var ReleaseBy = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(data.ReleaseById.GetValueOrDefault()));
                    oceanImportDetails.ReleaseBy = string.Concat(ReleaseBy.UserName, " ", ReleaseBy.Surname);
                }
                if (data.ShipModeId != null)
                {
                    var ShipMode = sysCodes.Where(w => w.Id == data.ShipModeId).FirstOrDefault();
                    oceanImportDetails.ShipModeName = ShipMode?.ShowName;
                }
                if (data.ShippingAgentId != null)
                {
                    var ShippingAgent = tradePartners.Where(w => w.Id == data.ShippingAgentId).FirstOrDefault();
                    oceanImportDetails.ShippingAgentName = string.Concat(ShippingAgent?.TPName, "/", ShippingAgent?.TPCode);
                }
                if (data.SvcTermFromId != null)
                {
                    var SvcTermFrom = sysCodes.Where(w => w.Id == data.SvcTermFromId).FirstOrDefault();
                    oceanImportDetails.SvcTermFromName = SvcTermFrom?.ShowName;
                }
                if (data.SvcTermToId != null)
                {
                    var SvcTermTo = sysCodes.Where(w => w.Id == data.SvcTermToId).FirstOrDefault();
                    oceanImportDetails.SvcTermToName = SvcTermTo?.ShowName;
                }
                if (data.TransPort1Id != null)
                {
                    var TransPort1 = portMangements.Where(w => w.Id == data.TransPort1Id).FirstOrDefault();
                    oceanImportDetails.TransPort1Name = TransPort1?.PortName;
                }
                if (data.CyLocationId != null)
                {
                    var cyLocation = countries.Where(w => w.Id == data.CyLocationId).FirstOrDefault();
                    oceanImportDetails.CyLocationName = cyLocation?.CountryName;
                }
                if (data.ItLocation != null)
                {
                    var itLocation = portMangements.Where(w => w.Id == Guid.Parse(data.ItLocation)).FirstOrDefault();
                    oceanImportDetails.ItLocationName = itLocation?.PortName;
                }
            }

            oceanImportDetails.Commodity = data.GetProperty<List<ManifestCommodity>>("Commodities");
            oceanImportDetails.MblNo = data.MblNo;
            oceanImportDetails.SoNo = data.SoNo;
            oceanImportDetails.PodEta = data.PodEta;
            oceanImportDetails.DocNo = data.FilingNo;
            oceanImportDetails.Mark = data.Mark;
            oceanImportDetails.Description = data.Description;
            oceanImportDetails.DomesticInstructions = data.DomesticInstructions;
            oceanImportDetails.CurrentDate = DateTime.Now;
            oceanImportDetails.VesselName = data.VesselName;
            oceanImportDetails.Voyage = data.Voyage;
            oceanImportDetails.AgentRefNo = data.AgentRefNo;
            oceanImportDetails.ItNo = data.ItNo;
            oceanImportDetails.ItDate = data.ItDate;

            return oceanImportDetails;
        }

        public async Task SetLockOrUnlockStatusOceanImportMblAsync(Guid[] Ids, bool IsLocked)
        {
            foreach (var Id in Ids)
            {
                var Mbl = await _repository.GetAsync(Id);
                Mbl.IsLocked = IsLocked;
                var QueryHbls = await _oceanImportHblRepository.GetQueryableAsync();
                var Hbls = QueryHbls.Where(w => w.MblId == Id).ToList();
                foreach (var Hbl in Hbls)
                {
                    Hbl.IsLocked = IsLocked;

                    await _oceanImportHblRepository.UpdateAsync(Hbl);
                }
                await _repository.UpdateAsync(Mbl);
            }
        }

        public async Task DeleteMultipleMblsAsync(Guid[] Ids)
        {
            var Hbls = await _oceanImportHblRepository.GetListAsync();

            foreach (var Id in Ids)
            {
                var HblIds = Hbls.Where(w => w.MblId == Id).Select(s => s.Id).ToList();           

                await _oceanImportHblRepository.DeleteManyAsync(HblIds);

                await _repository.DeleteAsync(Id);
            }
        }
        public async Task SaveDimensionsAsync(DimensionDataModel DataModel)
        {
            var Dimensions = DataModel.Dimensions;
            var ContainerId = DataModel.ContainerId;

            var container = await _containerAppService.GetAsync(ContainerId);

            if (container.ExtraProperties == null)
            {
                container.ExtraProperties = new Volo.Abp.Data.ExtraPropertyDictionary();
            }

            if (Dimensions != null && Dimensions.Any())
            {
                container.ExtraProperties.Remove("Dimensions");
                container.ExtraProperties.Add("Dimensions", Dimensions);
            }

            await _containerAppService.UpdateAsync(ContainerId, ObjectMapper.Map<ContainerDto, CreateUpdateContainerDto>(container));
        }

        public async Task SetCardSetting(bool IsShowDetail)
        {
            try
            {
                await _settingManager.SetGlobalAsync("ShowHblDetails", IsShowDetail.ToString().ToPascalCase());
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<bool> GetCardSettings() {

          var result=  await _settingProvider.GetAsync<bool>("ShowHblDetails");
            return result;
        }
    }
}