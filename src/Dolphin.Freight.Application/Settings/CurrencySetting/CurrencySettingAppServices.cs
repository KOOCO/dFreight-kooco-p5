using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Dolphin.Freight.Permissions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Dolphin.Freight.Settings.CurrencySetting
{
    public class CurrencySettingAppService : CrudAppService<
            CurrencySetting,//空運出口其他費用entity
            CurrencySettingDTO,//空運出口其他費用顯示用
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto,//Used for paging/sorting
            CreateUpdateCurrencySettingDTO>, //空運出口其他費用CRUD用
       ICurrencySettingAppService
    {
        public CurrencySettingAppService(IRepository<CurrencySetting, Guid> repository) : base(repository)
        {
            GetPolicyName = SettingsPermissions.CurrencySetting.Default;
            GetListPolicyName = SettingsPermissions.CurrencySetting.Default;
            CreatePolicyName = SettingsPermissions.CurrencySetting.Create;
            UpdatePolicyName = SettingsPermissions.CurrencySetting.Edit;
            DeletePolicyName = SettingsPermissions.CurrencySetting.Delete;

        }

        public async Task<List<CreateUpdateCurrencySettingDTO>> GetCurrenciesAsync()
        {
            var list = await  Repository.GetListAsync();

            var retVal = ObjectMapper.Map<List<CurrencySetting>, List<CreateUpdateCurrencySettingDTO>>(list);

            return retVal;
        }
    }
}

