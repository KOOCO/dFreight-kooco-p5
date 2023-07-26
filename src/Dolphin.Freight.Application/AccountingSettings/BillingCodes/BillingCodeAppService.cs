using AutoMapper.Internal.Mappers;
using Dolphin.Freight.AccountingSettings.GlCodes;
using Dolphin.Freight.Common;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.AccountingSettings.BillingCodes
{
    public class BillingCodeAppService :
        CrudAppService<
            BillingCode, 
            BillingCodeDto, 
            Guid, 
            PagedAndSortedResultRequestDto, 
            CreateUpdateBillingCodeDto>, 
        IBillingCodeAppService 
    {
        private IRepository<BillingCode, Guid> _repository;
        private IRepository<GlCode, Guid> _glCodeRepository;
        public BillingCodeAppService(IRepository<BillingCode, Guid> repository, IRepository<GlCode, Guid> glCodeRepository)
            : base(repository)
        {
            _repository = repository;
            _glCodeRepository = glCodeRepository;
            /*
            GetPolicyName = AccountingSettingPermissions.BillingCodes.Default;
            GetListPolicyName = AccountingSettingPermissions.BillingCodes.Default;
            CreatePolicyName = AccountingSettingPermissions.BillingCodes.Create;
            UpdatePolicyName = AccountingSettingPermissions.BillingCodes.Edit;
            DeletePolicyName = AccountingSettingPermissions.BillingCodes.Delete;
            */
        }

        public async Task<List<BillingCodeDto>> GetAEHListAsync()
        {
            return (await _repository.GetListAsync())
                .Where(r => r.IsAirExportHbl = true)
                .Select(ObjectMapper.Map<BillingCode, BillingCodeDto>)
                .ToList();
        }

        public async Task<List<BillingCodeDto>> GetAIHListAsync()
        {
            return (await _repository.GetListAsync())
                .Where(r => r.IsAirImportHbl = true)
                .Select(ObjectMapper.Map<BillingCode, BillingCodeDto>)
                .ToList();
        }

        public async Task<List<BillingCodeDto>> GetMSMListAsync()
        {
            return (await _repository.GetListAsync())
                .Where(r => r.IsMsm = true)
                .Select(ObjectMapper.Map<BillingCode, BillingCodeDto>)
                .ToList();
        }

        public async Task<List<BillingCodeDto>> GetOEHListAsync()
        {
            return (await _repository.GetListAsync())
                .Where(r => r.IsOceanExportHbl = true)
                .Select(ObjectMapper.Map<BillingCode, BillingCodeDto>)
                .ToList();
        }

        public async Task<List<BillingCodeDto>> GetOIHListAsync()
        {
            return (await _repository.GetListAsync())
                .Where(r => r.IsOceanImportHbl = true)
                .Select(ObjectMapper.Map<BillingCode, BillingCodeDto>)
                .ToList();
        }

        public async Task<List<BillingCodeDto>> GetTKMListAsync()
        {
            return (await _repository.GetListAsync())
                .Where(r => r.IsTkm = true)
                .Select(ObjectMapper.Map<BillingCode, BillingCodeDto>)
                .ToList();
        }

        public async Task<List<BillingCodeDto>> GetWHSListAsync()
        {
            return (await _repository.GetListAsync())
                .Where(r => r.IsWhs = true)
                .Select(ObjectMapper.Map<BillingCode, BillingCodeDto>)
                .ToList();
        }

        public async Task<PagedResultDto<BillingCodeDto>> QueryListAsync(QueryBillingCodeDto query)
        {
            var glCodes = await _glCodeRepository.GetListAsync();

            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (glCodes != null)
            {
                foreach (var code in glCodes)
                {
                    dictionary.Add(code.Id, code.Code);
                }
            }
            List<string> glCodesKeys = new List<string>();
            string glCode = query.GlCode.ToLower();
            glCodesKeys = dictionary.Where(x => x.Value.Contains(glCode)).Select(x=>x.Key.ToString()).ToList();
            var resultQuery = await _repository.GetQueryableAsync();
            resultQuery = resultQuery.WhereIf(!string.IsNullOrWhiteSpace(query.Code), x => x.Code.ToLower()
                                     .Contains(query.Code.ToLower()))
                                     .WhereIf(!string.IsNullOrWhiteSpace(query.Name), x=>x.BillingName.ToLower()
                                     .Contains(query.Name.ToLower()) || x.LocalName.Contains(query.Name.ToLower()))
                                     .WhereIf(!string.IsNullOrWhiteSpace(query.GlDescription), x=>x.Revenue.Remark.ToLower()
                                     .Contains(query.Remark))
                                     .WhereIf(!string.IsNullOrEmpty(query.GlCode), x=> glCodesKeys.Contains(x.RevenueId.ToString()) || 
                                     glCodesKeys.Contains(x.DeitId.ToString()) ||
                                     glCodesKeys.Contains(x.CostId.ToString()) ||
                                     glCodesKeys.Contains(x.RevenueId.ToString()));
            var rs = resultQuery.Where(x => x.IsDeleted == false).ToList();
            
            List<BillingCodeDto> list = new List<BillingCodeDto>();
            if (query != null )
            {
                if (query.IsUsed != null && query.IsUsed.Value) rs = rs.Where(x => x.IsUsed == true).ToList();
                if (!string.IsNullOrWhiteSpace(query.Code)) rs = rs.Where(x => x.Code.Equals(query.Code)).ToList();
                if (query.BillType != null) {
                    switch (query.BillType.Value) 
                    {
                        default:
                            break;
                        case 0:
                            rs = rs.Where(x => x.IsAR == true).ToList();
                            break;
                        case 1:
                            rs = rs.Where(x => x.IsDC == true).ToList();
                            break;
                        case 2:
                            rs = rs.Where(x => x.IsAP == true).ToList();
                            break;
                        case 3:
                            rs = rs.Where(x => x.IsGA == true).ToList();
                            break;
                    }
                }
                if (query.MethodType != null)
                {
                    switch (query.MethodType.Value)
                    {
                        default:
                            break;
                        case 0:
                            rs = rs.Where(x => x.IsOceanImportMbl == true).ToList();
                            break;
                        case 1:
                            rs = rs.Where(x => x.IsOceanImportHbl == true).ToList();
                            break;
                        case 2:
                            rs = rs.Where(x => x.IsOceanExportMbl == true).ToList();
                            break;
                        case 3:
                            rs = rs.Where(x => x.IsOceanExportHbl == true).ToList();
                            break;
                        case 4:
                            rs = rs.Where(x => x.IsAirImportMbl == true).ToList();
                            break;
                        case 5:
                            rs = rs.Where(x => x.IsAirImportHbl == true).ToList();
                            break;
                        case 6:
                            rs = rs.Where(x => x.IsAirExportMbl == true).ToList();
                            break;
                        case 7:
                            rs = rs.Where(x => x.IsAirExportHbl == true).ToList();
                            break;
                    }
                }
            }


            if (rs != null && rs.Count > 0)
            {

                foreach (var r in rs)
                {
                    var bill = ObjectMapper.Map<BillingCode, BillingCodeDto>(r);
                    if (r.RevenueId != null) bill.RevenueName = dictionary[r.RevenueId.Value];
                    if (r.CostId != null) bill.CostName = dictionary[r.CostId.Value];
                    if (r.CreditId != null) bill.CreditName = dictionary[r.CreditId.Value];
                    if (r.DeitId != null) bill.DeitName = dictionary[r.DeitId.Value];
                    list.Add(bill);
                }
            }

            PagedResultDto<BillingCodeDto> listDto = new PagedResultDto<BillingCodeDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }
        public async Task<List<BillingCodeDto>> QueryListForTagAsync(QueryBillingCodeDto query) 
        {
            var glCodes = await _glCodeRepository.GetListAsync();

            Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
            if (glCodes != null)
            {
                foreach (var code in glCodes)
                {
                    dictionary.Add(code.Id, code.Code);
                }
            }
            var rs = await _repository.GetListAsync();
            rs = rs.Where(x => x.IsDeleted == false).ToList();

            List<BillingCodeDto> list = new List<BillingCodeDto>();
            if (query != null)
            {
                if (query.IsUsed != null && query.IsUsed.Value) rs = rs.Where(x => x.IsUsed == true).ToList();
                if (query.Code != null) rs = rs.Where(x => x.Code.Equals(query.Code)).ToList();
                if (query.BillType != null)
                {
                    switch (query.BillType.Value)
                    {
                        default:
                            break;
                        case 0:
                            rs = rs.Where(x => x.IsAR == true).ToList();
                            break;
                        case 1:
                            rs = rs.Where(x => x.IsDC == true).ToList();
                            break;
                        case 2:
                            rs = rs.Where(x => x.IsAP == true).ToList();
                            break;
                        case 3:
                            rs = rs.Where(x => x.IsGA == true).ToList();
                            break;
                    }
                }
                if (query.MethodType != null)
                {
                    switch (query.MethodType.Value)
                    {
                        default:
                            break;
                        case 0:
                            rs = rs.Where(x => x.IsOceanImportMbl == true).ToList();
                            break;
                        case 1:
                            rs = rs.Where(x => x.IsOceanImportHbl == true).ToList();
                            break;
                        case 2:
                            rs = rs.Where(x => x.IsOceanExportMbl == true).ToList();
                            break;
                        case 3:
                            rs = rs.Where(x => x.IsOceanExportHbl == true).ToList();
                            break;
                        case 4:
                            rs = rs.Where(x => x.IsAirImportMbl == true).ToList();
                            break;
                        case 5:
                            rs = rs.Where(x => x.IsAirImportHbl == true).ToList();
                            break;
                        case 6:
                            rs = rs.Where(x => x.IsAirExportMbl == true).ToList();
                            break;
                        case 7:
                            rs = rs.Where(x => x.IsAirExportHbl == true).ToList();
                            break;
                    }
                }
            }


            if (rs != null && rs.Count > 0)
            {

                foreach (var r in rs)
                {
                    var bill = ObjectMapper.Map<BillingCode, BillingCodeDto>(r);
                    if (r.RevenueId != null) bill.RevenueName = dictionary[r.RevenueId.Value];
                    if (r.CostId != null) bill.CostName = dictionary[r.CostId.Value];
                    if (r.CreditId != null) bill.CreditName = dictionary[r.CreditId.Value];
                    if (r.DeitId != null) bill.DeitName = dictionary[r.DeitId.Value];
                    list.Add(bill);
                }
            }

            return list;
        }

    }
}
