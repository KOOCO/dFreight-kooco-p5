using AutoMapper;
using AutoMapper.Internal;
using Dolphin.Freight.EntityFrameworkCore;
using Dolphin.Freight.ImportExport.AirImports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Dolphin.Freight.ReportLog
{
    public class ReportLogRepository : EfCoreRepository<FreightDbContext, ReportLog, Guid>, IReportLogRepository
    {
        IDbContextProvider<FreightDbContext> _dbContextProvider;
        public ReportLogRepository(IDbContextProvider<FreightDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public async Task<ReportLog> FindByReportIdAsync(Guid ReportId, string ReportName)
        {
            var dbSet = await GetDbSetAsync();

            return await dbSet.FirstOrDefaultAsync(x => x.ReportId == ReportId && x.ReportName == ReportName);
        }

        public void InsertByReportIdAsync(ReportLog reportLog)
        {
            var _dbContext = _dbContextProvider.GetDbContext();

            _dbContext.ReportLog.Add(reportLog);
            _dbContext.SaveChanges();
        }

        public void UpdateByReportIdAsync(ReportLog reportLog)
        {
            var _dbContext = _dbContextProvider.GetDbContext();
            var rlog = _dbContext.ReportLog.FirstOrDefault(x => x.ReportId == Guid.Parse(reportLog.ReportId.ToString()) && x.ReportName == reportLog.ReportName);
            if (rlog != null)
            {
                rlog.ReportId = reportLog.ReportId;
                rlog.ReportName = reportLog.ReportName;
                rlog.ReportData = reportLog.ReportData;
                rlog.LastUpdateTime = reportLog.LastUpdateTime;

                _dbContext.ReportLog.Update(rlog);
                _dbContext.SaveChanges();
            }
        }

        public async Task<List<MawbReport>> GetMawbReport()
        {
            var _dbContext = await _dbContextProvider.GetDbContextAsync();

            try
            {
                // Get Air Import Mawb
                var report = _dbContext.AirImportMawbs
                                       .Select(ai => new MawbReport()
                                       {
                                           ReportType = "Air Import",
                                           Id = ai.Id,
                                           OverseaAgent = Convert.ToString((ai.OverseaAgent != null) ? ai.OverseaAgent.TPName : ""),
                                           OverseaAgentId = Convert.ToString(ai.OverseaAgentId),
                                           ConsigneeId = Convert.ToString(ai.ConsigneeId),
                                           OfficeId = Convert.ToString(ai.OfficeId),
                                           Office = Convert.ToString((ai.Office != null) ? ai.Office.SubstationName : ""),
                                           Consignee = Convert.ToString((ai.Consignee != null) ? ai.Consignee.TPName : ""),
                                           SalesId = Convert.ToString(ai.SalesId),
                                           SalesTypeId = "",
                                           ServiceTermTypeFrom = Convert.ToInt32(ai.ServiceTermTypeFrom),
                                           ServiceTermTypeTo = Convert.ToInt32(ai.ServiceTermTypeTo),
                                           IsEcommerce = ai.IsECom ? "yes" : "no",
                                           ShipperId = Convert.ToString(ai.ShipperId),
                                           Shipper = Convert.ToString((ai.Shipper != null) ? ai.Shipper.TPName : ""),
                                           CarrierId = Convert.ToString(ai.CarrierId),
                                           CarrierName = Convert.ToString((ai.Carrier != null) ? ai.Carrier.TPName : ""),
                                           CustomerId = Convert.ToString(ai.CustomerId),
                                           CustomerName = Convert.ToString((ai.Customer != null) ? ai.Customer.TPName : ""),
                                           BillToId = Convert.ToString(ai.BillToId),
                                           BillToName = Convert.ToString((ai.BillTo != null) ? ai.BillTo.TPName : ""),
                                           CustomerRefNo = null,
                                           OpId = Convert.ToString(ai.OPId),
                                           OpName = Convert.ToString((ai.OP != null) ? ai.OP.Name : ""),
                                           POLId = null,
                                           POLName = null,
                                           PODId = null,
                                           PODName = null,
                                           DestinationId = Convert.ToString(ai.DestinationId) ?? "",
                                           DestinationName = Convert.ToString((ai.Destination != null) ? ai.Destination.AirportName : ""),
                                           Vessel = null,
                                           MawbNo = Convert.ToString(ai.MawbNo ?? ""),
                                           CoLoaderId = Convert.ToString(ai.CoLoaderId),
                                           FileNo = Convert.ToString(ai.FilingNo ?? ""),
                                           ForwardingAgentId = null,
                                           ForwardingAgentName = null,
                                           ColorRemarkId = null,
                                           ColorRemarkName = null,
                                           FreightTermId = Convert.ToString(ai.FreightType)
                                       }).AsEnumerable()
                                       .Union(_dbContext.AirExportMawbs
                                                       .Select(ae => new MawbReport()
                                                       {
                                                           ReportType = "Air Export",
                                                           Id = ae.Id,
                                                           OverseaAgent = null,
                                                           OverseaAgentId = null,
                                                           ConsigneeId = Convert.ToString(ae.ConsigneeId),
                                                           OfficeId = Convert.ToString(ae.OfficeId),
                                                           Office = Convert.ToString((ae.Office != null) ? ae.Office.SubstationName : ""),
                                                           Consignee = Convert.ToString((ae.Consignee != null) ? ae.Consignee.TPName : ""),
                                                           SalesId = null,
                                                           SalesTypeId = null,
                                                           ServiceTermTypeFrom = Convert.ToInt32(ae.ServiceTermTypeFrom),
                                                           ServiceTermTypeTo = Convert.ToInt32(ae.ServiceTermTypeTo),
                                                           IsEcommerce = ae.IsECom ? "yes" : "no",
                                                           ShipperId = Convert.ToString(ae.ShipperId),
                                                           Shipper = Convert.ToString((ae.Shipper != null) ? ae.Shipper.TPName : ""),
                                                           CarrierId = Convert.ToString(ae.MawbCarrierId),
                                                           CarrierName = Convert.ToString((ae.MawbCarrier != null) ? ae.MawbCarrier.TPName : ""),
                                                           CustomerId = null,
                                                           CustomerName = Convert.ToString(ae.DVCustomer),
                                                           BillToId = null,
                                                           BillToName = null,
                                                           CustomerRefNo = null,
                                                           OpId = null,
                                                           OpName = null,
                                                           POLId = null,
                                                           POLName = null,
                                                           PODId = null,
                                                           PODName = null,
                                                           DestinationId = Convert.ToString(ae.DestinationId) ?? "",
                                                           DestinationName = Convert.ToString((ae.Destination != null) ? ae.Destination.AirportName : ""),
                                                           Vessel = null,
                                                           MawbNo = Convert.ToString(ae.MawbNo),
                                                           CoLoaderId = Convert.ToString(ae.CoLoaderId),
                                                           FileNo = Convert.ToString(ae.FilingNo),
                                                           ForwardingAgentId = null,
                                                           ForwardingAgentName = null,
                                                           ColorRemarkId = null,
                                                           ColorRemarkName = null,
                                                           FreightTermId = null
                                                       })
                                       ).AsEnumerable()
                                       .Union(_dbContext.OceanImportMbls
                                                       .Select(oi => new MawbReport()
                                                       {
                                                           ReportType = "Ocean Import",
                                                           Id = oi.Id,
                                                           OverseaAgent = Convert.ToString((oi.MblOverseaAgent != null) ? oi.MblOverseaAgent.TPName : ""),
                                                           OverseaAgentId = Convert.ToString(oi.MblOverseaAgentId),
                                                           ConsigneeId = Convert.ToString(oi.MblConsigneeId),
                                                           OfficeId = Convert.ToString(oi.OfficeId),
                                                           Office = Convert.ToString((oi.Office != null) ? oi.Office.SubstationName : ""),
                                                           SalesId = Convert.ToString(oi.MblSaleId),
                                                           SalesTypeId = Convert.ToString(oi.MblSalesTypeId),
                                                           ServiceTermTypeFrom = 0,
                                                           ServiceTermTypeTo = 0,
                                                           IsEcommerce = oi.IsEcommerce ? "yes" : "no",
                                                           ShipperId = Convert.ToString(oi.MblShipperId),
                                                           Shipper = Convert.ToString((oi.MblShipper != null) ? oi.MblShipper.TPName : ""),
                                                           CarrierId = Convert.ToString(oi.MblCarrierId),
                                                           CarrierName = Convert.ToString((oi.MblCarrier != null) ? oi.MblCarrier.TPName : ""),
                                                           CustomerId = Convert.ToString(oi.MblCustomerId),
                                                           CustomerName = Convert.ToString((oi.MblCustomer != null) ? oi.MblCustomer.TPName : ""),
                                                           BillToId = Convert.ToString(oi.MblBillToId),
                                                           BillToName = Convert.ToString((oi.MblBillTo != null) ? oi.MblBillTo.TPName : ""),
                                                           CustomerRefNo = Convert.ToString(oi.CustomerRefNo),
                                                           OpId = Convert.ToString(oi.MblOperatorId),
                                                           OpName = Convert.ToString((oi.MblOperator != null) ? oi.MblOperator.Name : ""),
                                                           POLId = Convert.ToString(oi.PolId),
                                                           POLName = Convert.ToString((oi.Pol != null) ? oi.Pol.PortName : ""),
                                                           PODId = Convert.ToString(oi.PolId),
                                                           PODName = Convert.ToString((oi.Pod != null) ? oi.Pod.PortName : ""),
                                                           DestinationId = null,
                                                           DestinationName = null,
                                                           Vessel = Convert.ToString(oi.VesselName ?? ""),
                                                           MawbNo = Convert.ToString(oi.MblNo),
                                                           CoLoaderId = Convert.ToString(oi.CoLoaderId),
                                                           FileNo = Convert.ToString(oi.FilingNo),
                                                           ForwardingAgentId = Convert.ToString(oi.ForwardingAgentId),
                                                           ForwardingAgentName = Convert.ToString((oi.ForwardingAgent != null) ? oi.ForwardingAgent.TPName : ""),
                                                           ColorRemarkId = Convert.ToString(oi.ColorRemarkId),
                                                           ColorRemarkName = Convert.ToString((oi.ColorRemark != null) ? oi.ColorRemark.ShowName : ""),
                                                           FreightTermId = Convert.ToString(oi.FreightTermId)
                                                       })
                                       ).AsEnumerable()
                                       .Union(_dbContext.OceanExportMbls
                                                       .Select(oe => new MawbReport()
                                                       {
                                                           ReportType = "Ocean Export",
                                                           Id = oe.Id,
                                                           OverseaAgent = Convert.ToString((oe.MblOverseaAgent != null) ? oe.MblOverseaAgent.TPName : ""),
                                                           OverseaAgentId = Convert.ToString(oe.MblOverseaAgentId),
                                                           ConsigneeId = Convert.ToString(oe.MblConsigneeId),
                                                           OfficeId = Convert.ToString(oe.OfficeId),
                                                           Office = Convert.ToString((oe.Office != null) ? oe.Office.SubstationName : ""),
                                                           SalesId = Convert.ToString(oe.MblSaleId),
                                                           SalesTypeId = Convert.ToString(oe.MblSalesTypeId),
                                                           ServiceTermTypeFrom = 0,
                                                           ServiceTermTypeTo = 0,
                                                           IsEcommerce = oe.IsEcommerce ? "yes" : "no",
                                                           ShipperId = Convert.ToString(oe.ShippingAgentId),
                                                           Shipper = Convert.ToString((oe.ShippingAgent != null) ? oe.ShippingAgent.TPName : ""),
                                                           CarrierId = Convert.ToString(oe.MblCarrierId),
                                                           CarrierName = Convert.ToString((oe.MblCarrier != null) ? oe.MblCarrier.TPName : ""),
                                                           CustomerId = Convert.ToString(oe.MblCustomerId),
                                                           CustomerName = Convert.ToString((oe.MblCustomer != null) ? oe.MblCustomer.TPName : ""),
                                                           BillToId = Convert.ToString(oe.MblBillToId),
                                                           BillToName = Convert.ToString((oe.MblBillTo != null) ? oe.MblBillTo.TPName : ""),
                                                           CustomerRefNo = Convert.ToString(oe.CustomerRefNo),
                                                           OpId = Convert.ToString(oe.MblOperatorId),
                                                           OpName = Convert.ToString((oe.MblOperator != null) ? oe.MblOperator.Name : ""),
                                                           POLId = Convert.ToString(oe.PolId),
                                                           POLName = Convert.ToString((oe.Pol != null) ? oe.Pol.PortName : ""),
                                                           PODId = Convert.ToString(oe.PolId),
                                                           PODName = Convert.ToString((oe.Pod != null) ? oe.Pod.PortName : ""),
                                                           DestinationId = null,
                                                           DestinationName = null,
                                                           Vessel = Convert.ToString(oe.VesselName ?? ""),
                                                           MawbNo = Convert.ToString(oe.MblNo ?? ""),
                                                           CoLoaderId = Convert.ToString(oe.CoLoaderId),
                                                           FileNo = Convert.ToString(oe.FilingNo),
                                                           ForwardingAgentId = Convert.ToString(oe.ForwardingAgentId),
                                                           ForwardingAgentName = Convert.ToString((oe.ForwardingAgent != null) ? oe.ForwardingAgent.TPName : ""),
                                                           ColorRemarkId = Convert.ToString(oe.ColorRemarkId),
                                                           ColorRemarkName = Convert.ToString((oe.ColorRemark != null) ? oe.ColorRemark.ShowName : ""),
                                                           FreightTermId = Convert.ToString(oe.FreightTermId)
                                                       }))
                                      .ToList();

                return report;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private List<MawbReport> ApplyFilter(List<MawbReport> report, MawbReport filter)
        {
            if (filter is not null)
            {
                var reportEnumerable = report.AsEnumerable();

                if (!string.IsNullOrEmpty(filter.IsEcommerce))
                {
                    reportEnumerable = reportEnumerable.Where(w => (w.IsEcommerce != null) && w.IsEcommerce.ToLower().Equals(filter.IsEcommerce));
                }
                if (!string.IsNullOrEmpty(filter.OfficeId))
                {
                    reportEnumerable = reportEnumerable.Where(w => (w.OfficeId != null) && w.Office.ToLower().Equals(filter.Office));
                }
                if (filter.ServiceTermTypeFrom != null && filter.ServiceTermTypeFrom > 0)
                {
                    reportEnumerable = reportEnumerable.Where(w => w.ServiceTermTypeFrom == filter.ServiceTermTypeFrom);
                }

                return reportEnumerable.ToList();
            }
            return report;
        }
    }
}
