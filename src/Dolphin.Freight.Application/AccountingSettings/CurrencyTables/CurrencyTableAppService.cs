using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Accounting.Currency;
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
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.AccountingSettings.CurrencyTables
{
    public class CurrencyTableAppService :
        CrudAppService<
            CurrencyTable,
            CurrencyTableDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateCurrencyTableDto>,
        ICurrencyTableAppService
    {
        private IRepository<CurrencyTable, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        public CurrencyTableAppService(IRepository<CurrencyTable, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            GetPolicyName = AccountingSettingPermissions.CurrencyTables.Default;
            GetListPolicyName = AccountingSettingPermissions.CurrencyTables.Default;
            CreatePolicyName = AccountingSettingPermissions.CurrencyTables.Create;
            UpdatePolicyName = AccountingSettingPermissions.CurrencyTables.Edit;
            DeletePolicyName = AccountingSettingPermissions.CurrencyTables.Delete;
        }
        public async Task<PagedResultDto<CurrencyTableDto>> QueryListAsync(QueryCurrencyTableDto query)
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
            List<CurrencyTableDto> list = new List<CurrencyTableDto>();
            rs.Where(x => x.Ccy1Id.Equals(query.Ccy1Id) && x.Ccy2Id.Equals(query.Ccy2Id));


            if (rs != null && rs.Count > 0)
            {

                foreach (var pu in rs)
                {
                    var pud = ObjectMapper.Map<CurrencyTable, CurrencyTableDto>(pu);
                    list.Add(pud);
                }
            }
            PagedResultDto<CurrencyTableDto> listDto = new PagedResultDto<CurrencyTableDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            return listDto;
        }

        public async Task<string> QueryRateInternalAsync(QueryCurrencyTableDto query)
        {
            var rs = await _repository.GetListAsync();

            string rateInternal = "";
            if (rs.Count> 0)
            {
                var queryList = rs.Where(x => x.Ccy1Id.ToString().ToUpper().Replace("{", "").Replace("}", "") == query.Ccy1Id.ToUpper()).OrderByDescending(x => x.StartDate).First();

                rateInternal = queryList.RateInternal.ToString();
            }
            return rateInternal;
        }
    }
}
