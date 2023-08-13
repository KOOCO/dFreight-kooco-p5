using Dolphin.Freight.Common;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.AccountingSettings.GlCodes
{
    public class GlCodeAppService :
        CrudAppService<
            GlCode, 
            GlCodeDto,  
            Guid,  
            PagedAndSortedResultRequestDto, 
            CreateUpdateGlCodeDto>, 
        IGlCodeAppService 
    {
        private IRepository<GlCode, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        public GlCodeAppService(IRepository<GlCode, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
/*
            GetPolicyName = AccountingSettingPermissions.GlCodes.Default;
            GetListPolicyName = AccountingSettingPermissions.GlCodes.Default;
            CreatePolicyName = AccountingSettingPermissions.GlCodes.Create;
            UpdatePolicyName = AccountingSettingPermissions.GlCodes.Edit;
            DeletePolicyName = AccountingSettingPermissions.GlCodes.Delete;
*/
        }
        public async Task<PagedResultDto<GlCodeDto>> QueryListAsync(QueryGlCodeDto query)
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
            var rs = await _repository.GetListAsync();
            List<GlCodeDto> list = new List<GlCodeDto>();
            
            rs= rs.WhereIf(!string.IsNullOrWhiteSpace(query.Code), e => e.Code.Contains(query.Code))
            .WhereIf(!string.IsNullOrWhiteSpace(query.Remark), e => e.Remark.ToLower().Contains(query.Remark.ToLower())).ToList();

            //if (query != null && query.Code != null&&query.Code!="" || query != null && query.Remark != null && query.Remark != "")
            //{
            //    if (query.Remark != null && query.Remark != "" && query.Code != null && query.Code != "")
            //    { 
            //        rs = rs.where(x => x.Code.Equals(query.Code) && x.Remark.Equals(query.Remark)).ToList(); 
            //    }
            //    else {
            //        if (query.Code != null && query.Code != "")
            //        {
            //            rs = rs.Where(x => x.Code.Equals(query.Code)).ToList();
            //        }
            //        if (query.Remark != null && query.Remark != "")
            //        {
            //            rs = rs.Where(x => x.Remark.Equals(query.Remark)).ToList();
            //        }
            //    }
                
            //}

            if (rs != null && rs.Count > 0)
            {

                foreach (var gc in rs)
                {
                    var gcd = ObjectMapper.Map<GlCode, GlCodeDto>(gc);
                    if(gcd.GlTypeId != null) gcd.GlTypeName = dictionary[gcd.GlTypeId.Value];
                    if (gcd.GlGroupId != null) gcd.GlGroupName = dictionary[gcd.GlGroupId.Value];
                    list.Add(gcd);
                }
            }
            PagedResultDto<GlCodeDto> listDto = new PagedResultDto<GlCodeDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }
        public async Task<List<GlCodeDto>> GetGlCodesAsync(QueryDto query)
        {
            var glCodeDtos = await _repository.GetListAsync();
            var rs = glCodeDtos;
            var list = ObjectMapper.Map<List<GlCode>, List<GlCodeDto>>(rs);
            return list;
        }

    }
}
