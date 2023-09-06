using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Newtonsoft.Json;
using NPOI.DDF;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using static Dolphin.Freight.Permissions.AccountingPermissions;

namespace Dolphin.Freight.ImportExport.OceanExports
{
    public class OceanExportMblAppService :
        CrudAppService<
            OceanExportMbl, //IT號碼管理entity
            OceanExportMblDto, //顯示IT號碼管理用
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateOceanExportMblDto>, //新增修改IT號碼管理用
        IOceanExportMblAppService //實作IOceanExportMblAppService
    {
        private readonly IRepository<OceanExportMbl, Guid> _repository;
        private readonly IRepository<SysCode, Guid> _sysCodeRepository;
        private readonly IRepository<Substation, Guid> _substationRepository;
        private readonly PortsManagementAppService _portRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IIdentityUserAppService _identityUserAppService;
        private readonly IRepository<OceanExportHbl, Guid> _oceanExportHblRepository;
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly ContainerAppService _containerRepository;
        public OceanExportMblAppService(IRepository<OceanExportMbl, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<Substation, Guid> substationRepository, PortsManagementAppService portRepository, IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository,
            IIdentityUserAppService identityUserAppService, IRepository<OceanExportHbl, Guid> oceanExportHblRepository,
            IInvoiceAppService invoiceAppService,
            ContainerAppService containerRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _substationRepository = substationRepository;
            _portRepository = portRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _identityUserAppService = identityUserAppService;
            _oceanExportHblRepository = oceanExportHblRepository;
            _invoiceAppService = invoiceAppService;
            _containerRepository = containerRepository;
            /*
            GetPolicyName = OceanExportPermissions.OceanExportMbls.Default;
            GetListPolicyName = OceanExportPermissions.OceanExportMbls.Default;
            CreatePolicyName = OceanExportPermissions.OceanExportMbls.Create;
            UpdatePolicyName = OceanExportPermissions.OceanExportMbls.Edit;
            DeletePolicyName = OceanExportPermissions.OceanExportMbls.Delete;*/
        }
        public async Task<PagedResultDto<OceanExportMblDto>> QueryListAsync(QueryMblDto query)
        {
            var dataQuery = await _repository.GetQueryableAsync();

            var substations = await _substationRepository.GetListAsync();
            Dictionary<Guid, string> substationsDictionary = new Dictionary<Guid, string>();
            if (substations != null)
            {
                foreach (var substation in substations)
                {
                    substationsDictionary.Add(substation.Id, substation.SubstationName + "(" + substation.AbbreviationName + ")");
                }
            }

            dataQuery = dataQuery.WhereIf(!string.IsNullOrWhiteSpace(query.Search), x => x.CarrierContractNo
                                          .Contains(query.Search) || x.MblNo
                                          .Contains(query.Search) || x.Office.SubstationName
                                          .Contains(query.Search) || x.Office.AbbreviationName
                                          .Contains(query.Search) || x.SoNo
                                          .Contains(query.Search))
                                             .WhereIf(query.CarrierId.HasValue, e => e.MblCarrierId == query.CarrierId)
                                   .WhereIf(query.ShippingAgentId.HasValue, e => e.ShippingAgentId == query.ShippingAgentId)
                                   .WhereIf(query.ForwardingAgentId.HasValue, e => e.ForwardingAgentId == query.ForwardingAgentId)
                                   .WhereIf(query.Pol.HasValue, e => e.PolId == query.Pol)
                                   .WhereIf(query.Pod.HasValue, e => e.PodId == query.Pod)
                                   .WhereIf(query.Del.HasValue, e => e.DelId == query.Del)
                                   .WhereIf(query.DeliverTo.HasValue, e => e.DeliveryToId == query.DeliverTo)
                                   .WhereIf(query.ShipModeId.HasValue, e => e.ShipModeId == query.ShipModeId)
                                   .WhereIf(query.OvearseaAgentId.HasValue, e => e.MblOverseaAgentId == query.OvearseaAgentId)
                                   .WhereIf(query.BlCancelled.HasValue, e => e.IsCanceled == query.BlCancelled)
                                   .WhereIf(query.PostDate.HasValue, e => e.PostDate.Date == query.PostDate.Value.Date.AddDays(1))
                                   .WhereIf(query.Eta.HasValue, e => e.PodEta.Value.Date == query.Eta.Value.Date.AddDays(1))
                                   .WhereIf(query.Etd.HasValue, e => e.PolEtd.Value.Date == query.Etd.Value.Date.AddDays(1))
                                   .WhereIf(query.CreationDate.HasValue, e => e.CreationTime.Date == query.CreationDate.Value.Date.AddDays(1))
                                          .OrderByDescending(x => x.CreationTime);


            //var SysCodes = await _sysCodeRepository.GetListAsync();
            //Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            //if (SysCodes != null)
            //{
            //    foreach (var syscode in SysCodes)
            //    {
            //        dictionary.Add(syscode.Id, syscode.CodeValue);
            //    }
            //}
            //var OceanExportMbls = await _repository.GetListAsync();
            //List<OceanExportMbl> rs;
            //List<OceanExportMblDto> list = new List<OceanExportMblDto>();
            //if (query != null && query.QueryKey != null)
            //{
            //    rs = OceanExportMbls.OrderByDescending(x=>x.CreationTime ).ToList();
            //}
            //else
            //{
            //    rs = OceanExportMbls.OrderByDescending(x => x.CreationTime).ToList();
            //}
            //if (rs != null && rs.Count > 0)
            //{

            //    foreach (var r in rs)
            //    {
            //        var item = ObjectMapper.Map<OceanExportMbl, OceanExportMblDto>(r);
            //        item.OfficeName = substationsDictionary[r.OfficeId.Value];
            //        list.Add(item);
            //    }
            //}

            List<OceanExportMbl> rs = dataQuery.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            List<OceanExportMblDto> list = new List<OceanExportMblDto>();

            var result = ObjectMapper.Map<List<OceanExportMbl>, List<OceanExportMblDto>>(rs);
            if (result.Any())
                result.ForEach(x => x.OfficeName = substationsDictionary[x.OfficeId.Value]);

            PagedResultDto<OceanExportMblDto> listDto = new PagedResultDto<OceanExportMblDto>();
            listDto.Items = result;
            listDto.TotalCount = dataQuery.Count();
            return listDto;
        }
        public async Task LockedOrUnLockedOceanExportMblAsync(QueryMblDto query) 
        {
            try
            {
                var mbl = await _repository.GetAsync(query.MbId.Value);
                if (mbl.IsLocked == true)
                {
                    mbl.IsLocked = false;
                    var queryHbl = await _oceanExportHblRepository.GetQueryableAsync();
                    var hbls = queryHbl.Where(w => w.MblId == mbl.Id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = false;

                        await _oceanExportHblRepository.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);
                }
                else
                {
                    mbl.IsLocked = true;
                    var queryHbl = await _oceanExportHblRepository.GetQueryableAsync();
                    var hbls = queryHbl.Where(w => w.MblId == mbl.Id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = true;
                        await _oceanExportHblRepository.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
           
        }
        public async Task<CreateUpdateOceanExportMblDto> GetCreateUpdateOceanExportMblDtoById(Guid Id) {
            var oceanExportMbl = await _repository.GetAsync(Id,true);
            var dto = ObjectMapper.Map<OceanExportMbl, CreateUpdateOceanExportMblDto>(oceanExportMbl);
            var ports = await _portRepository.QueryListAsync();
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
                if (dto.MblCarrierId != null)dto.MblCarrierName = tdictionary[dto.MblCarrierId.Value];
                if(dto.MblOverseaAgentId != null)dto.MblOverseaAgentName = tdictionary[dto.MblOverseaAgentId.Value];
                if(dto.ReleaseById != null)
                    dto.ReleaseBy = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(dto.ReleaseById.GetValueOrDefault()));
            }
            return dto;
        }

        public async Task<CreateUpdateOceanExportMblDto> GetMblById(QueryMblDto query)
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
            var oceanExportMbl = await _repository.GetAsync(query.MbId.Value);
            var rs = ObjectMapper.Map<OceanExportMbl, CreateUpdateOceanExportMblDto>(oceanExportMbl);
            
            return rs;
        }

        public override async Task DeleteAsync(Guid Id)
        {
            var hbls = await _oceanExportHblRepository.GetListAsync();
            var ids = hbls.Where(w => w.MblId == Id).Select(s => s.Id);

            await Repository.DeleteAsync(Id);
            await _oceanExportHblRepository.DeleteManyAsync(ids);
        }

        public async Task<OceanExportDetails> GetOceanExportDetailsById(Guid Id)
        {
            var oceanExportDetails = new OceanExportDetails();  
            var tradePartners = ObjectMapper.Map<List<TradePartners.TradePartner>, List<TradePartnerDto>>(await _tradePartnerRepository.GetListAsync());
            var portMangements = await _portRepository.QueryListAsync();
            var sysCodes = ObjectMapper.Map<List<SysCode>, List<SysCodeDto>>(await _sysCodeRepository.GetListAsync());
            var substations = ObjectMapper.Map<List<Substation>, List<SubstationDto>>(await _substationRepository.GetListAsync());

            var data = await Repository.GetAsync(Id);

            if (data != null)
            {
                oceanExportDetails = ObjectMapper.Map<OceanExportMbl, OceanExportDetails>(data);

                if (data.BlAcctCarrierId != null)
                {
                    var BlAcctCarrier = tradePartners.Where(w => w.Id == data.BlAcctCarrierId).FirstOrDefault();
                    oceanExportDetails.BlAcctCarrierName = string.Concat(BlAcctCarrier?.TPName, "/", BlAcctCarrier?.TPCode);
                }
                if (data.BlTypeId != null)
                {
                    oceanExportDetails.BlType = sysCodes.Where(w => w.Id == data.BlTypeId).FirstOrDefault();
                }
                if (data.CancelById != null)
                {
                    oceanExportDetails.CancelBy = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(data.CancelById.GetValueOrDefault()));
                }
                if (data.CareOfId != null)
                {
                    oceanExportDetails.CareOf = tradePartners.Where(w => w.Id == data.CareOfId).FirstOrDefault();
                }
                if (data.CargoTypeId != null)
                {
                    var CargoType = sysCodes.Where(w => w.Id == data.CargoTypeId).FirstOrDefault();
                    oceanExportDetails.CargoTypeName = CargoType?.ShowName;
                }
                if (data.CoLoaderId != null)
                {
                    var CoLoader = tradePartners.Where(w => w.Id == data.CoLoaderId).FirstOrDefault();
                    oceanExportDetails.CargoTypeName = string.Concat(CoLoader?.TPName, "/", CoLoader?.TPCode);
                }
                if (data.DelId != null)
                {
                    var Del = portMangements.Where(w => w.Id == data.DelId).FirstOrDefault();
                    oceanExportDetails.DelName = Del?.PortName;
                }
                if (data.DeliveryToId != null)
                {
                    var DeliveryTo = tradePartners.Where(w => w.Id == data.DeliveryToId).FirstOrDefault();
                    oceanExportDetails.DeliveryToName = string.Concat(DeliveryTo?.TPName, "/", DeliveryTo?.TPCode)?.TrimStart('/');
                }
                if (data.EmptyPickupId != null)
                {
                    var EmptyPickup = tradePartners.Where(w => w.Id == data.EmptyPickupId).FirstOrDefault();
                    oceanExportDetails.EmptyPickupName = string.Concat(EmptyPickup?.TPName, "/", EmptyPickup?.TPCode)?.TrimStart('/');
                }
                if (data.FdestId != null)
                {
                    var Fdest = portMangements.Where(w => w.Id == data.FdestId).FirstOrDefault();
                    oceanExportDetails.FdestName = Fdest?.PortName;
                }
                if (data.ForwardingAgentId != null)
                {
                    var ForwardingAgent = tradePartners.Where(w => w.Id == data.ForwardingAgentId).FirstOrDefault();
                    oceanExportDetails.ForwardingAgentName = string.Concat(ForwardingAgent?.TPName, "/", ForwardingAgent?.TPCode);
                }
                if (data.FreightTermId != null)
                {
                    var FreightTerm = sysCodes.Where(w => w.Id == data.FreightTermId).FirstOrDefault();
                    oceanExportDetails.FreightTermName = FreightTerm?.ShowName;
                }
                if (data.MblBillToId != null)
                {
                    var MblBillTo = tradePartners.Where(w => w.Id == data.MblBillToId).FirstOrDefault();
                    oceanExportDetails.MblBillToName = string.Concat(MblBillTo?.TPName, "/", MblBillTo?.TPCode)?.TrimStart('/');
                }
                if (data.MblCarrierId != null)
                {
                    var MblCarrier = tradePartners.Where(w => w.Id == data.MblCarrierId).FirstOrDefault();
                    oceanExportDetails.MblCarrierName = string.Concat(MblCarrier?.TPName, "/", MblCarrier?.TPCode);
                }
                if (data.MblConsigneeId != null)
                {
                    var MblConsignee = tradePartners.Where(w => w.Id == data.MblConsigneeId).FirstOrDefault();
                    oceanExportDetails.MblConsigneeName = string.Concat(MblConsignee?.TPName, "/", MblConsignee?.TPCode)?.TrimStart('/');
                }
                if (data.MblCustomerId != null)
                {
                    var MblCustomer = tradePartners.Where(w => w.Id == data.MblCustomerId).FirstOrDefault();
                    oceanExportDetails.MblCustomerName = string.Concat(MblCustomer?.TPName, "/", MblCustomer?.TPCode)?.TrimStart('/');
                }
                if (data.MblNotifyId != null)
                {
                    var MblNotify = tradePartners.Where(w => w.Id == data.MblNotifyId).FirstOrDefault();
                    oceanExportDetails.MblNotifyName = string.Concat(MblNotify?.TPName, "/", MblNotify?.TPCode);
                }
                if (data.MblOperatorId != null||data.CreatorId!=null)
                {
                    oceanExportDetails.MblOperator = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(data.MblOperatorId!=null? data.MblOperatorId.GetValueOrDefault():data.CreatorId.GetValueOrDefault()));
                    oceanExportDetails.MblOperatorName = string.Concat(oceanExportDetails.MblOperator.UserName, " ", oceanExportDetails.MblOperator.Surname);
                }
                if (data.MblOverseaAgentId != null)
                {
                    var MblOverseaAgent = tradePartners.Where(w => w.Id == data.MblOverseaAgentId).FirstOrDefault();
                    oceanExportDetails.MblOverseaAgentName = string.Concat(MblOverseaAgent.TPName, "/", MblOverseaAgent.TPCode);
                }
                if (data.MblReferralById != null)
                {
                    var MblReferralBy = tradePartners.Where(w => w.Id == data.FdestId).FirstOrDefault();
                    oceanExportDetails.MblReferralByName = string.Concat(MblReferralBy?.TPName, "/", MblReferralBy?.TPCode);
                }
                if (data.MblSaleId != null)
                {
                    var MblSale = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(data.MblSaleId.GetValueOrDefault()));
                    oceanExportDetails.MblSaleName = string.Concat(MblSale?.Name, "/", MblSale?.Surname)?.TrimStart('/');
                }
                if (data.OblTypeId != null)
                {
                    var OblType = sysCodes.Where(w => w.Id == data.OblTypeId).FirstOrDefault();
                    oceanExportDetails.OblTypeName = OblType?.ShowName;
                }

                if (data.OfficeId != null)
                {
                    var Office = substations.Where(w => w.Id == data.OfficeId).FirstOrDefault();
                }
                if (data.PackageCategoryId != null)
                {
                    var PackageCategory = sysCodes.Where(w => w.Id == data.PackageCategoryId).FirstOrDefault();
                    oceanExportDetails.PackageCategoryName = PackageCategory?.ShowName;
                }
                if (data.PackageMeasureId != null)
                {
                    oceanExportDetails.PackageMeasure = sysCodes.Where(w => w.Id == data.PackageMeasureId).FirstOrDefault();
                }
                if (data.PackageWeightId != null)
                {
                    var PackageWeight = sysCodes.Where(w => w.Id == data.PackageWeightId).FirstOrDefault();
                    oceanExportDetails.PackageWeightName = PackageWeight?.ShowName;
                }
                if (data.PodId != null)
                {
                    var Pod = portMangements.Where(w => w.Id == data.PodId).FirstOrDefault();
                    oceanExportDetails.PodName = Pod?.PortName;
                }
                if (data.PolId != null)
                {
                    var Pol = portMangements.Where(w => w.Id == data.PolId).FirstOrDefault();
                    oceanExportDetails.PolName = Pol?.PortName;
                }
                if (data.PorId != null)
                {
                    var Por = portMangements.Where(w => w.Id == data.PorId).FirstOrDefault();
                    oceanExportDetails.PorName = Por?.PortName;
                }
                if (data.PreCarriageVesselNameId != null)
                {
                    var PreCarriageVesselName = sysCodes.Where(w => w.Id == data.PreCarriageVesselNameId).FirstOrDefault();
                    oceanExportDetails.PreCarriageVesselNameValue = PreCarriageVesselName?.ShowName;
                }
                if (data.ReleaseById != null)
                {
                    var ReleaseBy = ObjectMapper.Map<IdentityUserDto, UserData>(await _identityUserAppService.GetAsync(data.ReleaseById.GetValueOrDefault()));
                    oceanExportDetails.ReleaseBy = string.Concat(ReleaseBy.UserName, " ", ReleaseBy.Surname);
                }
                if (data.ShipModeId != null)
                {
                    var ShipMode = sysCodes.Where(w => w.Id == data.ShipModeId).FirstOrDefault();
                    oceanExportDetails.ShipModeName = ShipMode?.ShowName;
                }
                if (data.ShippingAgentId != null)
                {
                    var ShippingAgent = tradePartners.Where(w => w.Id == data.ShippingAgentId).FirstOrDefault();
                    oceanExportDetails.ShippingAgentName = string.Concat(ShippingAgent?.TPName, "/", ShippingAgent?.TPCode);
                }
                if (data.SvcTermFromId != null)
                {
                    var SvcTermFrom = sysCodes.Where(w => w.Id == data.SvcTermFromId).FirstOrDefault();
                    oceanExportDetails.SvcTermFromName = SvcTermFrom?.ShowName;
                }
                if (data.SvcTermToId != null)
                {
                    var SvcTermTo = sysCodes.Where(w => w.Id == data.SvcTermToId).FirstOrDefault();
                    oceanExportDetails.SvcTermToName = SvcTermTo?.ShowName;
                }
                if (data.TransPort1Id != null)
                {
                    var TransPort1 = portMangements.Where(w => w.Id == data.TransPort1Id).FirstOrDefault();
                    oceanExportDetails.TransPort1Name = TransPort1?.PortName;
                }
            }
            
            oceanExportDetails.Commodity = data.GetProperty<List<ManifestCommodity>>("Commodities");
            oceanExportDetails.MblNo = data.MblNo;
            oceanExportDetails.SoNo = data.SoNo;
            oceanExportDetails.PodEta = data.PodEta;
            oceanExportDetails.DocNo = data.FilingNo;
            oceanExportDetails.Mark = data.Mark;
            oceanExportDetails.Description = data.Description;
            oceanExportDetails.DomesticInstructions = data.DomesticInstructions;
            oceanExportDetails.CurrentDate = DateTime.Now;

            return oceanExportDetails;
        }

        public async Task SelectedLockedOceanExportMblAsync(Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var mbl = await _repository.GetAsync(id);

                    mbl.IsLocked = true;
                    var query = await _oceanExportHblRepository.GetQueryableAsync();
                    var hbls = query.Where(x => x.MblId == id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = true;

                        await _oceanExportHblRepository.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);

                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task SelectedUnLockedOceanExportMblAsync(Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var mbl = await _repository.GetAsync(id);

                    mbl.IsLocked = false;
                    var query = await _oceanExportHblRepository.GetQueryableAsync();
                    var hbls = query.Where(x => x.MblId == id).ToList();
                    foreach (var hbl in hbls)
                    {
                        hbl.IsLocked = false;

                        await _oceanExportHblRepository.UpdateAsync(hbl);
                    }
                    await _repository.UpdateAsync(mbl);

                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task DeleteMultipleMblsAsync(Guid[] ids)
        {
            foreach(var id in ids)
            {
                var mbl = await _repository.GetAsync(id);

                mbl.IsDeleted = true;
                var query = await _oceanExportHblRepository.GetQueryableAsync();
                var hbls = query.Where(w => w.MblId == id).ToList();
                foreach (var hbl in hbls)
                {
                    hbl.IsDeleted = true;

                    await _oceanExportHblRepository.UpdateAsync(hbl);
                }
                await _repository.UpdateAsync(mbl);
            }
        }
    }
}