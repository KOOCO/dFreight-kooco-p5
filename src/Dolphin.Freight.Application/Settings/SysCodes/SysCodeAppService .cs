using AutoMapper;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settinngs.SysCodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.Settings.SysCodes
{
    public class SysCodeAppService :
        CrudAppService<
            SysCode, //IT號碼管理entity
            SysCodeDto, //顯示IT號碼管理用
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateSysCodeDto>, //新增修改IT號碼管理用
        ISysCodeAppService //實作ISysCodeAppService
    {
        private IRepository<SysCode, Guid> _repository;
        public SysCodeAppService(IRepository<SysCode, Guid> repository)
            : base(repository)
        {
            _repository = repository;
        }
        public async Task<List<SysCodeDto>> GetSysCodeDtosByTypeAsync(QueryDto query)
        {
            var sysCodes = await _repository.GetListAsync();
            var rs = sysCodes.Where(x => x.CodeType.Equals(query.QueryType)).ToList();
            var list = ObjectMapper.Map<List<SysCode>, List<SysCodeDto>>(rs);
            return list;
        }
        public async Task<string> GetSystemNoAsync(QueryDto query)
        {
            var sysCodes = await _repository.GetListAsync();
            var sysCode = sysCodes.Where(x => x.CodeType.Equals(query.QueryType)).FirstOrDefault();
            var year = DateTime.Now.Year - 2000;
            var month = DateTime.Now.ToString("MM");
            string rs = null;
            if (sysCode != null)
            {
                var codes = sysCode.ShowName.Split("-");
                var code = year + month;
                if (codes != null && codes.Length>1 && code.Equals(codes[1]))
                {
                    var codeValue = getIntValue(sysCode.CodeValue);
                    sysCode.CodeValue = codeValue.ToString();
                    rs = sysCode.ShowName + codeValue.ToString().PadLeft(4, '0');

                }
                else 
                {
                    sysCode.ShowName = await this.getPrefix(query.QueryType)+"-"+code;
                    sysCode.CodeValue = "1";
                    rs = sysCode.ShowName + "1".PadLeft(4, '0');
                }
                await _repository.UpdateAsync(sysCode);
            }
            return rs;
        }
        public async Task<string> GetSystemBookingNoAsync(QueryDto query)
        {
            var sysCodes = await _repository.GetListAsync();
            var sysCode = sysCodes.Where(x => x.CodeType.Equals(query.QueryType)).FirstOrDefault();
            string rs = null;
            if (sysCode != null)
            {
                var codes = sysCode.ShowName.Split("-");
                if (codes != null && codes.Length >= 1)
                {
                    var codeValue = getIntValue(sysCode.CodeValue);

                    if (codeValue > 9999)
                    {
                        codeValue = 0;

                        DateTime oldSerial = new DateTime(int.Parse("20" + codes[0].Substring(0, 2)), int.Parse(codes[0].Substring(2)), 1);
                        DateTime newSerial = oldSerial.AddMonths(1);

                        sysCode.ShowName = newSerial.ToString("yyMM");
                    } else
                    {
                        sysCode.CodeValue = codeValue.ToString();
                        rs = sysCode.ShowName + codeValue.ToString().PadLeft(4, '0');
                    }
                }
                await _repository.UpdateAsync(sysCode);
            }
            return rs;
        }
        private int getIntValue(string value)
        {
            int rs = 1;
            try
            {
                rs = 1+Int32.Parse(value);
            }
            catch (Exception ex) { }
            return rs;
        }
        private async Task<string> getPrefix(string queryType) 
        {
            string rs = null;
            var sysCodes = await _repository.GetListAsync();
            var sysCode = sysCodes.Where(x => x.CodeType.Equals("Prefix_" + queryType)).FirstOrDefault();
            return sysCode == null?"NO":sysCode.CodeValue;
        }
        public async Task<List<SysCodeDto>> FillSysCodesAmsNo()
        {
            var lookup = await this.GetSysCodeDtosByTypeAsync(new QueryDto { QueryType = "AmsNoId" });

            return lookup.OrderBy(o => o.CodeValue).ToList();
        }
    }
}
