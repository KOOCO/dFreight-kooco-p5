using Dolphin.Freight.Accounting.Inv;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Dolphin.Freight.ReportLog
{
    public class ReportLogAppService :
        CrudAppService<
            ReportLog,
            ReportLogDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateReportLogDto>,
        IReportLogAppService
    {
        private readonly IReportLogRepository _ReportLogRepository;
        private IRepository<Airport, Guid> _airportRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        public ReportLogAppService(IRepository<ReportLog, Guid> repository,
                                   IReportLogRepository ReportLogRepository,
                                   IRepository<Airport, Guid> airportRepository,
                                   IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository)
        : base(repository)
        {
            _ReportLogRepository = ReportLogRepository;
            _airportRepository = airportRepository;
            _tradePartnerRepository = tradePartnerRepository;
        }

        public async Task<PagedResultDto<MawbReportDto>> GetMawbReport(MawbReportDto filter)
        {
            var report = await _ReportLogRepository.GetMawbReport();

            report = ApplyFilter(report, filter);

            report = ApplyColumnFilter(report, filter);

            return new PagedResultDto<MawbReportDto>(report.Count, ObjectMapper.Map<List<MawbReport>, List<MawbReportDto>>(report));
        }

        private List<MawbReport> ApplyFilter(List<MawbReport> report, MawbReportDto filter)
        {
            var predictBuilder = PredicateBuilder.New<MawbReport>();

            if (filter is not null)
            {
                if (!string.IsNullOrEmpty(filter.IsEcommerce))
                {
                    predictBuilder.And(w => (w.IsEcommerce != null) && w.IsEcommerce.ToLower().Equals(filter.IsEcommerce));
                }
                if (!string.IsNullOrEmpty(filter.OfficeId))
                {
                    predictBuilder.And(w => (w.OfficeId != null) && w.Office.ToLower().Equals(filter.Office));
                }
                if (filter.ServiceTermTypeFrom != null && filter.ServiceTermTypeFrom > 0)
                {
                    predictBuilder.And(w => w.ServiceTermTypeFrom == filter.ServiceTermTypeFrom);
                }


                return report.Where(predictBuilder).ToList();
            }
            return report;
        }

        private List<MawbReport> ApplyColumnFilter(List<MawbReport> report, MawbReportDto filter)
        {
            var predictBuilderOr = PredicateBuilder.New<MawbReport>();
            var predictBuilderAnd = PredicateBuilder.New<MawbReport>();

            if (filter is not null)
            {
                if (filter.IsOverseaAgent)
                {
                    predictBuilderOr.Or(w => w.OverseaAgentId == null);
                }
                else
                {
                    predictBuilderAnd.And(w => w.OverseaAgentId == null);
                }

                if (filter.IsShipper)
                {
                    predictBuilderOr.Or(w => string.IsNullOrEmpty(w.ShipperId));
                }
                else
                {
                    predictBuilderAnd.And(w => string.IsNullOrEmpty(w.ShipperId));
                }

                if (filter.IsCarrier)
                {
                    predictBuilderOr.Or(w => string.IsNullOrEmpty(w.CarrierId));
                }
                else
                {
                    predictBuilderOr.And(w => string.IsNullOrEmpty(w.CarrierId));
                }

                if (filter.IsCustomer)
                {
                    predictBuilderOr.Or(w => !string.IsNullOrEmpty(w.CustomerId));
                }
                else
                {
                    predictBuilderAnd.And(w => w.CustomerId == null);
                }

                if (filter.IsBillTo)
                {
                    predictBuilderOr.Or(w => !string.IsNullOrEmpty(w.BillToId));
                }
                else
                {
                    predictBuilderAnd.And(w => w.BillToId == null);
                }

                if (filter.IsMBLOP)
                {
                    predictBuilderOr.Or(w => w.OpId == null);
                }
                else
                {
                    predictBuilderAnd.And(w => w.OpId == null);
                }

                if (filter.IsPOL)
                {
                    predictBuilderOr.Or(w => w.PODId == null);
                }
                else
                {
                    predictBuilderAnd.And(w => w.PODId == null);
                }

                if (filter.IsPOD)
                {
                    predictBuilderOr.Or(w => w.PODId == null);
                }
                else
                {
                    predictBuilderOr.And(w => w.PODId == null);
                }

                if (!filter.IsFinalDestination)
                {
                    predictBuilderOr.Or(w => w.DestinationId == null);
                }

                if (!filter.IsVesselFlight)
                {
                    predictBuilderOr.Or(w => w.Vessel == null);
                }

                if (!filter.IsMblMawbWarehouse)
                {
                    predictBuilderOr.Or(w => w.MawbNo == null);
                }

                if (!filter.IsCoLoader)
                {
                    predictBuilderOr.Or(w => w.CoLoaderId == null);
                }

                if (!filter.IsOutputFile)
                {
                    predictBuilderOr.Or(w => string.IsNullOrEmpty(w.FileNo));
                }
                else
                {
                    predictBuilderOr.Or(w => w.FileNo == null);
                }

                if (!filter.IsForwardingAgent)
                {
                    predictBuilderOr.Or(w => w.ForwardingAgentId == null);
                }

                if (!filter.IsMblColorRemark)
                {
                    predictBuilderOr.Or(w => w.ColorRemarkId == null);
                }

                if (!filter.IsOutputFreightTerm)
                {
                    predictBuilderOr.Or(w => w.FreightTermId == null);
                }

                if (!predictBuilderOr.IsStarted)
                {
                    predictBuilderOr.Or(w => true);
                }

                if (!predictBuilderAnd.IsStarted)
                {
                    predictBuilderAnd.Or(w => true);
                }

                report = report.Where(predictBuilderAnd).ToList();

                return report.Where(predictBuilderOr).ToList();
            }
            return report;
        }


        [UnitOfWork]
        public async Task UpdateReportLog(ReportLogDto reportLog)
        {
            IQueryable<ReportLog> queryable = await _ReportLogRepository.GetQueryableAsync();

            var query = from rlog in queryable
                        where rlog.ReportId == reportLog.ReportId && rlog.ReportName == reportLog.ReportName
                        select rlog;

            var reportModel = new ReportLog();
            reportModel.ReportId = reportLog.ReportId;
            reportModel.ReportName = reportLog.ReportName;
            reportModel.ReportData = reportLog.ReportData;
            reportModel.LastUpdateTime = reportLog.LastUpdateTime;

            if (query.Count() > 0)
            {
                #region Update

                _ReportLogRepository.UpdateByReportIdAsync(reportModel);

                #endregion
            }
            else
            {
                #region Insert

                _ReportLogRepository.InsertByReportIdAsync(reportModel);

                #endregion
            }
        }

        [UnitOfWork]
        public async Task<ReportLogDto> QueryReportLog(ReportLogDto reportLog)
        {
            var query = await _ReportLogRepository.FindByReportIdAsync(reportLog.ReportId, reportLog.ReportName);

            return ObjectMapper.Map<ReportLog, ReportLogDto>(query);
        }
    }
}