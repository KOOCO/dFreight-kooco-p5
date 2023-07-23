using Dolphin.Freight.Permissions;
using System;
using System.Collections.Generic;
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
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateAwbNoRangeDto>, //新增修改Awb號碼管理用
        IAwbNoRangeAppService //實作IAwbNoRangeAppService
    {

        public AwbNoRangeAppService(IRepository<AwbNoRange, Guid> repository)
            : base(repository)
        {
            GetPolicyName = SettingsPermissions.AwbNoRanges.Default;
            GetListPolicyName = SettingsPermissions.AwbNoRanges.Default;
            CreatePolicyName = SettingsPermissions.AwbNoRanges.Create;
            UpdatePolicyName = SettingsPermissions.AwbNoRanges.Edit;
            DeletePolicyName = SettingsPermissions.AwbNoRanges.Delete;
        }
      
    }
}
