using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.ReportLog
{
    public interface IReportLogRepository : IRepository<ReportLog, Guid>
    {
        Task<ReportLog> FindByReportIdAsync(Guid ReportId, string ReportName);

        public void InsertByReportIdAsync(ReportLog reportLog);

        public void UpdateByReportIdAsync(ReportLog reportLog);

        Task<List<MawbReport>> GetMawbReport(MawbReportDto filter);
        Task<ReportLogPdf> GetMawbPdfReport(MawbReportDto filter);
    }
}
