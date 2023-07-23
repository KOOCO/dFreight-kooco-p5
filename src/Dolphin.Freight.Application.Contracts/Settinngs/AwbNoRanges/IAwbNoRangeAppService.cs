using Dolphin.Freight.Settinngs.AwbNoRanges;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.Settings.AwbNoRanges
{
    /// <summary>
    /// Awb號碼管理interface
    /// </summary>
    public interface IAwbNoRangeAppService :
            ICrudAppService< //Defines CRUD methods
            AwbNoRangeDto, //顯示Awb號碼管理用
            Guid, //Primary key of the book entity
            AwbNoRangeSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateAwbNoRangeDto> //新增修改Awb號碼管理用
    {
    }
}
