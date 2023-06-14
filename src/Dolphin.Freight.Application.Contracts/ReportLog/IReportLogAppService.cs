using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.ReportLog
{
    public interface IReportLogAppService :
        ICrudAppService<
        ReportLogDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateReportLogDto>
    {
        Task UpdateReportLog(ReportLogDto reportLog);
        Task<ReportLogDto> QueryReportLog(ReportLogDto reportLog);
        Task<PagedResultDto<MawbReportDto>> GetMawbReport(MawbReportDto filter);
    }
}
