using AutoMapper;
using AutoMapper.Internal;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Common;
using Dolphin.Freight.EntityFrameworkCore;
using Dolphin.Freight.ImportExport.AirImports;
using Microsoft.EntityFrameworkCore;
using Scriban.Syntax;
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
        private readonly IInvoiceAppService _invoiceAppService;
        IDbContextProvider<FreightDbContext> _dbContextProvider;
        public ReportLogRepository(IDbContextProvider<FreightDbContext> dbContextProvider, IInvoiceAppService invoiceAppService)
            : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _invoiceAppService = invoiceAppService;
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

        public async Task<List<MawbReport>> GetMawbReport(MawbReportDto filter)
        {
            
            var _dbContext = await _dbContextProvider.GetDbContextAsync();
            var cargoTypes = _dbContext.SysCodes.Where(s => s.CodeType == "cargoTypeId")
                .Select(s =>new SysCodeForReport { Id= s.Id.ToString() , ShowName =  s.ShowName, CodeType = s.CodeType });

            var resultMawbs = (from mb in _dbContext.Containers
                              join hb in _dbContext.ContainerSizes on mb.ContainerSizeId equals hb.Id
                              select new PackageSizeReport()
                              {
                                  ContainerId = mb.Id,
                                  ContainerCode =  hb.ContainerCode,
                                  MblId =  mb.MblId
                              }).ToList();

            var airImportMawbsIds = await _dbContext.AirImportMawbs.Select(s => s.Id).ToListAsync();
            var airExportMawbsIds = await _dbContext.AirExportMawbs.Select(s => s.Id).ToListAsync();
            var oceanImportMblsIds = await _dbContext.OceanImportMbls.Select(s => s.Id).ToListAsync();
            var oceanExportMblsIds = await _dbContext.OceanExportMbls.Select(s => s.Id).ToListAsync();

            var allProfitData = new Dictionary<Guid, ProfitReport>();

            foreach (var id in airImportMawbsIds.Concat(airExportMawbsIds))
            {
                allProfitData[id] = await GetProfit(id, 0, _invoiceAppService);
            }

            foreach (var id in oceanImportMblsIds.Concat(oceanExportMblsIds))
            {
                allProfitData[id] = await GetProfit(id, 3, _invoiceAppService);
            }

            try
            {

                var airImports = (from mb in _dbContext.AirImportMawbs
                                  join hb in _dbContext.AirImportHawbs on mb.Id equals hb.MawbId into hm
                                  from subpet in hm.DefaultIfEmpty()
                                  select new MawbReport()
                                  {
                                      ReportType = "Air Import",
                                      Id = mb.Id,
                                      OverseaAgent = Convert.ToString((mb.OverseaAgent != null) ? mb.OverseaAgent.TPName : ""),
                                      OverseaAgentId = Convert.ToString(mb.OverseaAgentId),
                                      ConsigneeId = Convert.ToString(mb.ConsigneeId),
                                      OfficeId = Convert.ToString(mb.OfficeId),
                                      Office = Convert.ToString((mb.Office != null) ? mb.Office.SubstationName : ""),
                                      Consignee = Convert.ToString((mb.Consignee != null) ? mb.Consignee.TPName : ""),
                                      SalesId = Convert.ToString(mb.SalesId),
                                      SalesTypeId = "",
                                      ServiceTermTypeFrom = Convert.ToInt32(mb.ServiceTermTypeFrom),
                                      ServiceTermTypeTo = Convert.ToInt32(mb.ServiceTermTypeTo),
                                      IsEcommerce = mb.IsECom ? "yes" : "no",
                                      ShipperId = Convert.ToString(mb.ShipperId),
                                      Shipper = Convert.ToString((mb.Shipper != null) ? mb.Shipper.TPName : ""),
                                      CarrierId = Convert.ToString(mb.CarrierId),
                                      CarrierName = Convert.ToString((mb.Carrier != null) ? mb.Carrier.TPName : ""),
                                      CustomerId = Convert.ToString(mb.CustomerId),
                                      CustomerName = Convert.ToString((mb.Customer != null) ? mb.Customer.TPName : ""),
                                      BillToId = Convert.ToString(mb.BillToId),
                                      BillToName = Convert.ToString((mb.BillTo != null) ? mb.BillTo.TPName : ""),
                                      CustomerRefNo = null,
                                      OpId = Convert.ToString(mb.OPId),
                                      OpName = Convert.ToString((mb.OP != null) ? mb.OP.Name : ""),
                                      POLId = null,
                                      POLName = null,
                                      PODId = null,
                                      PODName = null,
                                      DestinationId = Convert.ToString(mb.DestinationId) ?? "",
                                      DestinationName = Convert.ToString((mb.Destination != null) ? mb.Destination.AirportName : ""),
                                      Vessel = null,
                                      MawbNo = Convert.ToString(mb.MawbNo ?? ""),
                                      CoLoaderId = Convert.ToString(mb.CoLoaderId),
                                      FileNo = Convert.ToString(mb.FilingNo ?? ""),
                                      ForwardingAgentId = null,
                                      ForwardingAgentName = null,
                                      ColorRemarkId = null,
                                      ColorRemarkName = null,
                                      FreightTermId = Convert.ToString(mb.FreightType),
                                      SalesPerson = "",
                                      BLPostDate = mb.PostDate,
                                      CargoType = "",
                                      ARTotal = allProfitData[mb.Id].ARTotal,
                                      APTotal = allProfitData[mb.Id].APTotal,
                                      DCTotal = allProfitData[mb.Id].DCTotal,
                                      ProfitAmt = allProfitData[mb.Id].ProfitAmt,
                                      ProfitMargin = allProfitData[mb.Id].ProfitMargin,
                                      Avg_Profit_Per_Cntr = allProfitData[mb.Id].Avg_Profit_Per_Cntr,
                                      V20 = 0,
                                      V40 = 0,
                                      HC  = 0,
                                      V45 = 0,
                                      RF  = 0,
                                      SOC = 0,
                                      ETC = 0
                                  }).AsEnumerable();

                var airExports = (from hb in _dbContext.AirExportHawbs
                                  join mb in _dbContext.AirExportMawbs on hb.MawbId equals mb.Id
                                  select new MawbReport()
                                  {
                                      ReportType = "Air Export",
                                      Id = mb.Id,
                                      OverseaAgent = null,
                                      OverseaAgentId = null,
                                      ConsigneeId = Convert.ToString(mb.ConsigneeId),
                                      OfficeId = Convert.ToString(mb.OfficeId),
                                      Office = Convert.ToString((mb.Office != null) ? mb.Office.SubstationName : ""),
                                      Consignee = Convert.ToString((mb.Consignee != null) ? mb.Consignee.TPName : ""),
                                      SalesId = null,
                                      SalesTypeId = null,
                                      ServiceTermTypeFrom = Convert.ToInt32(mb.ServiceTermTypeFrom),
                                      ServiceTermTypeTo = Convert.ToInt32(mb.ServiceTermTypeTo),
                                      IsEcommerce = mb.IsECom ? "yes" : "no",
                                      ShipperId = Convert.ToString(mb.ShipperId),
                                      Shipper = Convert.ToString((mb.Shipper != null) ? mb.Shipper.TPName : ""),
                                      CarrierId = Convert.ToString(mb.MawbCarrierId),
                                      CarrierName = Convert.ToString((mb.MawbCarrier != null) ? mb.MawbCarrier.TPName : ""),
                                      CustomerId = null,
                                      CustomerName = Convert.ToString(mb.DVCustomer),
                                      BillToId = null,
                                      BillToName = null,
                                      CustomerRefNo = null,
                                      OpId = null,
                                      OpName = null,
                                      POLId = null,
                                      POLName = null,
                                      PODId = null,
                                      PODName = null,
                                      DestinationId = Convert.ToString(mb.DestinationId) ?? "",
                                      DestinationName = Convert.ToString((mb.Destination != null) ? mb.Destination.AirportName : ""),
                                      Vessel = null,
                                      MawbNo = Convert.ToString(mb.MawbNo),
                                      CoLoaderId = Convert.ToString(mb.CoLoaderId),
                                      FileNo = Convert.ToString(mb.FilingNo),
                                      ForwardingAgentId = null,
                                      ForwardingAgentName = null,
                                      ColorRemarkId = null,
                                      ColorRemarkName = null,
                                      FreightTermId = null,
                                      SalesPerson = "",
                                      BLPostDate = mb.PostDate,
                                      CargoType = cargoTypes.Where(c => c.Id == hb.CargoType).Select(c => c.ShowName).FirstOrDefault(),
                                      ARTotal = allProfitData[mb.Id].ARTotal,
                                      APTotal = allProfitData[mb.Id].APTotal,
                                      DCTotal = allProfitData[mb.Id].DCTotal,
                                      ProfitAmt = allProfitData[mb.Id]  .ProfitAmt,
                                      ProfitMargin = allProfitData[mb.Id].ProfitMargin,
                                      Avg_Profit_Per_Cntr = allProfitData[mb.Id].Avg_Profit_Per_Cntr,
                                      V20 = 0,
                                      V40 = 0,
                                      HC = 0,
                                      V45 = 0,
                                      RF = 0,
                                      SOC = 0,
                                      ETC = 0
                                  }).AsEnumerable();

                var oceanImports = (from oih in _dbContext.OceanImportHbls
                                    join oi in _dbContext.OceanImportMbls on oih.MblId equals oi.Id
                                    select new MawbReport()
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
                                        FreightTermId = Convert.ToString(oi.FreightTermId),
                                        SalesPerson = "",
                                        BLPostDate = oi.PostDate,
                                        CargoType = "",
                                        ARTotal = allProfitData[oi.Id].ARTotal,
                                        APTotal = allProfitData[oi.Id].APTotal,
                                        DCTotal = allProfitData[oi.Id].DCTotal,
                                        ProfitAmt = allProfitData[oi.Id].ProfitAmt,
                                        ProfitMargin = allProfitData[oi.Id].ProfitMargin,
                                        Avg_Profit_Per_Cntr = allProfitData[oi.Id].Avg_Profit_Per_Cntr,
                                        V20 = GetContainerTypeCount(resultMawbs, oi.Id).V20,
                                        V40 = GetContainerTypeCount(resultMawbs, oi.Id).V40,
                                        HC = GetContainerTypeCount(resultMawbs,  oi.Id).HC,
                                        V45 = GetContainerTypeCount(resultMawbs, oi.Id).V45,
                                        RF = GetContainerTypeCount(resultMawbs,  oi.Id).RF,
                                        SOC = GetContainerTypeCount(resultMawbs, oi.Id).SOC,
                                        ETC = GetContainerTypeCount(resultMawbs, oi.Id).ETC
                                    }).AsEnumerable();
                                    
                var oceanExports = (from oeh in _dbContext.OceanExportHbls
                                    join oe in _dbContext.OceanExportMbls on oeh.MblId equals oe.Id
                                    select new MawbReport()
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
                                        FreightTermId = Convert.ToString(oe.FreightTermId),
                                        SalesPerson = "",
                                        BLPostDate = oe.PostDate,
                                        CargoType = "",
                                        ARTotal = allProfitData[oe.Id].ARTotal,
                                        APTotal = allProfitData[oe.Id].APTotal,
                                        DCTotal = allProfitData[oe.Id].DCTotal,
                                        ProfitAmt = allProfitData[oe.Id].ProfitAmt,
                                        ProfitMargin = allProfitData[oe.Id].ProfitMargin,
                                        Avg_Profit_Per_Cntr = allProfitData[oe.Id].Avg_Profit_Per_Cntr,
                                        Volume = GetContainerTypeCount(resultMawbs, oe.Id)
                                    }).AsEnumerable();

                IEnumerable<MawbReport> result = new List<MawbReport>();
                

                if (filter.IsOceanExport)
                        result = oceanExports;

                if (filter.IsOceanImport)
                {
                    if (result != null)
                       result =  result.Union(oceanImports);
                    else
                        result = oceanImports;
                }
                if (filter.IsAirExport) { 
                    if (result != null)
                        result =  result.Union(airExports);
                    else
                        result = airExports;
                }
                if (filter.IsAirImport) {
                    if (result != null)
                        result = result.Union(airImports);
                    else
                        result = airImports;
                }
                //if (filter.IsMisc)
                //if (filter.IsWarehouse)
                //if (filter.IsTruck)

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        
        public static async Task<ProfitReport> GetProfit(Guid Id, int queryType, IInvoiceAppService invoiceAppService)
        {
            ProfitReport data = new();

            QueryInvoiceDto query = new QueryInvoiceDto() { QueryType = queryType, ParentId = Id };

            data.Invoices = (await invoiceAppService.QueryInvoicesAsync(query)).ToList();

            if (data.Invoices != null && data.Invoices.Count > 0)
            {
                data.AR = new List<InvoiceDto>();
                data.AP = new List<InvoiceDto>();
                data.DC = new List<InvoiceDto>();
                foreach (var dto in data.Invoices)
                {
                    switch (dto.InvoiceType)
                    {
                        default:
                            data.AR.Add(dto);
                            break;
                        case 1:
                            data.DC.Add(dto);
                            break;
                        case 2:
                            data.AP.Add(dto);
                            break;
                    }
                }

                if (data.AR.Any())
                {
                    double arTotal = 0;
                    foreach (var ar in data.AR)
                    {
                        arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                    }
                    data.ARTotal = arTotal;
                }
                if (data.AP.Any())
                {
                    double apTotal = 0;
                    foreach (var ap in data.AP)
                    {
                        apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                    }

                    data.APTotal = apTotal;
                }
                if (data.DC.Any())
                {
                    double dcTotal = 0;
                    foreach (var dc in data.DC)
                    {
                        dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                    }
                    data.DCTotal = dcTotal;
                }
            }

            return data;
        }

        public static VolumeReport GetContainerTypeCount(List<PackageSizeReport> result, Guid Id)
        {

            var mblIds = result.Where(x => x.MblId == Id).ToList();
            //var _dbContext = _dbContextProvider.GetDbContext();
            //var cargoTypes = _dbContext.SysCodes.Where(s => s.CodeType == "cargoTypeId")
            //    .Select(s => new SysCodeForReport { Id = s.Id.ToString(), ShowName = s.ShowName, CodeType = s.CodeType });

            //List<PackageSizeReport> result = new List<PackageSizeReport>();
            //if (idType == "Mawb")
            //{
            //    result = (from mb in _dbContext.Containers
            //                  join hb in _dbContext.ContainerSizes on mb.ContainerSizeId equals hb.Id
            //                  where mb.MblId == Id
            //                  select new PackageSizeReport()
            //                  {
            //                      ContainerId = mb.Id,
            //                      ContainerCode =  hb.ContainerCode
            //                  }).ToList();
            //}
            //else if (idType == "Hawb")
            //{
            //    result = (from mb in _dbContext.Containers
            //               join hb in _dbContext.ContainerSizes on mb.ContainerSizeId equals hb.Id
            //               where mb.HblId == Id
            //               select new PackageSizeReport()
            //               {
            //                   ContainerId = mb.Id,
            //                   ContainerCode = hb.ContainerCode
            //               }).ToList();
            //}
            VolumeReport volume = new VolumeReport();
            foreach (var item in mblIds)
            {
                switch (item.ContainerCode)
                {
                    case "20":
                        volume.V20 = +1;
                        volume.ETC = +1;
                        break;

                    case "20DC":
                        volume.V20 = +1;
                        volume.ETC = +1;
                        break;

                    case "20FR":
                        volume.V20 = +1;
                        volume.ETC = +1;
                        break;

                    case "20FL":
                        volume.V20 = +1;
                        volume.ETC = +1;
                        break;

                    case "40DC":
                        volume.V40 = +1;
                        volume.ETC = +1;
                        break;

                    case "40FL":
                        volume.V40 = +1;
                        volume.ETC = +1;
                        break;

                    case "40FR":
                        volume.V40 = +1;
                        volume.ETC = +1;
                        break;

                    case "40GH":
                        volume.V40 = +1;
                        volume.ETC = +1;
                        break;

                    case "12RF":
                        volume.V45 = +1;
                        volume.ETC = +1;
                        break;

                    case "53FT":
                        volume.ETC = +1;
                        break;

                    case "2":
                        volume.ETC = +1;
                        break;

                    case "101":
                        volume.ETC = +1;
                        break;

                    case "102":
                        volume.ETC = +1;
                        break;

                    default:
                        volume.HC = 0;
                        volume.SOC = 0;
                        break;
                }
            }

            return volume;
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
