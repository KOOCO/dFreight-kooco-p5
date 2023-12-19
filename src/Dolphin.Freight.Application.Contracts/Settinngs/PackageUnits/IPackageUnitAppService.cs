using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.Settinngs.PackageUnits
{
    public interface IPackageUnitAppService :
        ICrudAppService< //Defines CRUD methods
        PackageUnitDto, //顯示Awb號碼管理用
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdatePackageUnitDto> //新增修改Awb號碼管理用
    {
        Task<PagedResultDto<PackageUnitDto>> QueryListAsync(QueryDto query);
        Task<ListResultDto<PackageUnitLookupDto>> GetPackageUnitsLookupAsync();
        Task<bool> IsPackageUnitAlreadyExist(string packageCode, Guid? packageUnitId = null);

    }
}
