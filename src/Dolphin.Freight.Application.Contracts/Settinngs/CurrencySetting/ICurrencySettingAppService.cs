using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;



namespace Dolphin.Freight.Settings.CurrencySetting
{
    /// <summary>
    /// 貨幣表Interface
    /// </summary>
    public interface ICurrencySettingAppService : 
        ICrudAppService< //Defines CRUD methods
        CurrencySettingDTO,  //顯示IT號碼管理用
        Guid,  //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateCurrencySettingDTO //新增修改IT號碼管理用
        >
    {
        Task<List<CreateUpdateCurrencySettingDTO>> GetCurrenciesAsync();
    }
}
