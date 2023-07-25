using Dolphin.Freight.Common;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.AwbNoRanges;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using static Dolphin.Freight.Permissions.SettingsPermissions;

namespace Dolphin.Freight.Settings.PackageUnits
{
    public class PackageUnitAppService :
        CrudAppService<
            PackageUnit, 
            PackageUnitDto, 
            Guid, 
            PagedAndSortedResultRequestDto, 
            CreateUpdatePackageUnitDto>, 
        IPackageUnitAppService 
    {
        private IRepository<PackageUnit, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        public PackageUnitAppService(IRepository<PackageUnit, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            GetPolicyName = SettingsPermissions.PackageUnits.Default;
            GetListPolicyName = SettingsPermissions.PackageUnits.Default;
            CreatePolicyName = SettingsPermissions.PackageUnits.Create;
            UpdatePolicyName = SettingsPermissions.PackageUnits.Edit;
            DeletePolicyName = SettingsPermissions.PackageUnits.Delete;
        }
        public async Task<PagedResultDto<PackageUnitDto>>  QueryListAsync(QueryDto query)
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
            var PackageUnits = await _repository.GetQueryableAsync();
            PackageUnits = PackageUnits.WhereIf(!string.IsNullOrWhiteSpace(query.Filter), x => x.PackageCode.ToLower()
                                        .Contains(query.Filter.ToLower()) || x.PackageName.ToLower()
                                        .Contains(query.Filter.ToLower()));

            List<PackageUnit> rs;
            List<PackageUnitDto> list = new List<PackageUnitDto>();
            if (query != null && query.QueryKey != null)
            {
                rs = PackageUnits.Where(x => x.PackageName.Contains(query.QueryKey) || x.PackageName.Contains(query.QueryKey) || x.AmsNo.ShowName.Contains(query.QueryKey)).ToList();
            }
            else 
            {
                rs = PackageUnits.ToList();
            }
            if (rs != null && rs.Count > 0) 
            {
                
                foreach (var pu in rs) 
                {
                    var pud = ObjectMapper.Map<PackageUnit, PackageUnitDto>(pu);
                    if (pu.AmsNoId != null) pud.AmsNo = dictionary[pu.AmsNoId.Value];
                    if (pu.EManifestId != null) pud.EManifestNo = dictionary[pu.EManifestId.Value];
                    list.Add(pud);
                }
            }
            PagedResultDto < PackageUnitDto> listDto = new PagedResultDto<PackageUnitDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count; 
            return listDto;
        }

        public async Task<ListResultDto<PackageUnitLookupDto>> GetPackageUnitsLookupAsync()
        {
            var packageUnitsQueryable = await _repository.GetQueryableAsync();
            var query = from packageUnit in packageUnitsQueryable
                        orderby packageUnit.PackageName
                        select packageUnit;
            var packageUnits = await AsyncExecuter.ToListAsync(query);
            return new ListResultDto<PackageUnitLookupDto>(ObjectMapper.Map<List<PackageUnit>, List<PackageUnitLookupDto>>(packageUnits));
        }

    }
}
