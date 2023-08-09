using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Dolphin.Freight.Permissions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settinngs.Ports;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Dolphin.Freight.Settings.CurrencySetting
{
    public class CurrencySettingAppService : CrudAppService<
            CurrencySetting,//空運出口其他費用entity
            CurrencySettingDTO,//空運出口其他費用顯示用
            Guid, //Primary key of the book entity
            QueryDto,//Used for paging/sorting
            CreateUpdateCurrencySettingDTO>, //空運出口其他費用CRUD用
       ICurrencySettingAppService
    {

        private readonly IRepository<CurrencySetting, Guid> _repository;
        public CurrencySettingAppService(IRepository<CurrencySetting, Guid> repository) : base(repository)
        {
            GetPolicyName = SettingsPermissions.CurrencySetting.Default;
            GetListPolicyName = SettingsPermissions.CurrencySetting.Default;
            CreatePolicyName = SettingsPermissions.CurrencySetting.Create;
            UpdatePolicyName = SettingsPermissions.CurrencySetting.Edit;
            DeletePolicyName = SettingsPermissions.CurrencySetting.Delete;
            _repository = repository;
        }

        public override async Task<PagedResultDto<CurrencySettingDTO>> GetListAsync(QueryDto input)
        {
            PagedResultDto<CurrencySettingDTO> listDto = new PagedResultDto<CurrencySettingDTO>();
            var query = await _repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.CurrencyDepartment
                                   .Contains(input.Filter) || x.StartingCurrency
                                   .Contains(input.Filter) || x.EndCurrency
                                   .Contains(input.Filter) || x.ExChangeRate.ToString()
                                   .Contains(input.Filter));

            var result = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            listDto.Items = ObjectMapper.Map<List<CurrencySetting>, List<CurrencySettingDTO>>(result);
            listDto.TotalCount = query.Count();
            return listDto; ;
        }

        public async Task<List<CreateUpdateCurrencySettingDTO>> GetCurrenciesAsync()
        {
            var list = await  Repository.GetListAsync();

            var retVal = ObjectMapper.Map<List<CurrencySetting>, List<CreateUpdateCurrencySettingDTO>>(list);

            return retVal;
        }
    }
}

