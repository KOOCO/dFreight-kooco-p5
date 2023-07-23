using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settinngs.AwbNoRanges;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.Settings.AwbNoRanges
{
    public class AwbNoRangeAppService : CrudAppService<
            AwbNoRange, //Awb號碼管理entity
            AwbNoRangeDto, //顯示Awb號碼管理用
            Guid, //Primary key of the book entity
            AwbNoRangeSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateAwbNoRangeDto>, //新增修改Awb號碼管理用
        IAwbNoRangeAppService //實作IAwbNoRangeAppService
    {
        IRepository<AwbNoRange, Guid> _repository;
        public AwbNoRangeAppService(IRepository<AwbNoRange, Guid> repository)
            : base(repository)
        {
            GetPolicyName = SettingsPermissions.AwbNoRanges.Default;
            GetListPolicyName = SettingsPermissions.AwbNoRanges.Default;
            CreatePolicyName = SettingsPermissions.AwbNoRanges.Create;
            UpdatePolicyName = SettingsPermissions.AwbNoRanges.Edit;
            DeletePolicyName = SettingsPermissions.AwbNoRanges.Delete;
            _repository = repository;
        }

        public override async Task<PagedResultDto<AwbNoRangeDto>> GetListAsync(AwbNoRangeSortedResultRequestDto input)
        {
            var query = await _repository.GetQueryableAsync();
            query = query.WhereIf((input.CompanyId != Guid.Empty && input.CompanyId != null), x => x.CompanyId == input.CompanyId)
                           .WhereIf(input.CreatedDate != null, x => x.CreationTime == input.CreatedDate)
                           //.WhereIf(string.IsNullOrWhiteSpace(input.Prefix), x => x.Note == "")
                           .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.EndNo.ToLower()
                           .Contains(input.Filter.ToLower()) || x.StartNo.ToLower()
                           .Contains(input.Filter.ToLower()) || x.CurrentNo.ToLower()
                           .Contains(input.Filter.ToLower())).OrderBy(input.Sorting) ;

            var count = await AsyncExecuter.CountAsync(query);
            var awbNoRangeList = await AsyncExecuter.ToListAsync(query.PageBy(input));
            var result = ObjectMapper.Map<List<AwbNoRange>, List<AwbNoRangeDto>>(awbNoRangeList);
            return new PagedResultDto<AwbNoRangeDto> { Items = result, TotalCount = count };
        }

    }
}
