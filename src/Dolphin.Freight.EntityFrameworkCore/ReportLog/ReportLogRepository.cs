﻿using AutoMapper;
using AutoMapper.Internal;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.AirImports;
using Dolphin.Freight.Common;
using Dolphin.Freight.EntityFrameworkCore;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settings.ContainerSizes;
using Dolphin.Freight.Settinngs.ContainerSizes;
using Dolphin.Freight.Settinngs.SysCodes;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using Scriban.Syntax;
using SixLabors.ImageSharp.ColorSpaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;
using QueryHblDto = Dolphin.Freight.ImportExport.OceanExports.QueryHblDto;

namespace Dolphin.Freight.ReportLog
{
    public class ReportLogRepository : EfCoreRepository<FreightDbContext, ReportLog, Guid>, IReportLogRepository
    {
        private readonly IInvoiceAppService _invoiceAppService;
        private readonly IContainerAppService _containerAppService;
        private readonly IOceanExportHblAppService _oceanExportHblAppService;
        private readonly IOceanImportHblAppService _oceanImportHblAppService;
        private readonly IContainerSizeAppService _containerSizeAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IAirExportHawbAppService _airExportHawbAppService;
        private readonly IAirImportHawbAppService _airImportHawbAppService;
        IDbContextProvider<FreightDbContext> _dbContextProvider;
        public ReportLogRepository(IDbContextProvider<FreightDbContext> dbContextProvider, IInvoiceAppService invoiceAppService, 
            IContainerAppService containerAppService, IOceanExportHblAppService oceanExportHblAppService, 
            IOceanImportHblAppService oceanImportHblAppService, ISysCodeAppService sysCodeAppService, IContainerSizeAppService containerSizeAppService,
            IAirExportHawbAppService airExportHawbAppService,IAirImportHawbAppService airImportHawbAppService)
            : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _invoiceAppService = invoiceAppService;
            _containerAppService = containerAppService;
            _oceanExportHblAppService=oceanExportHblAppService;
            _oceanImportHblAppService = oceanImportHblAppService;
            _sysCodeAppService=sysCodeAppService;
            _containerSizeAppService = containerSizeAppService;
            _airExportHawbAppService = airExportHawbAppService;
            _airImportHawbAppService=airImportHawbAppService;
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

            var resultMawbs = (from hb in _dbContext.ContainerSizes
                               join mb in _dbContext.Containers on hb.Id equals mb.ContainerSizeId
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
            var SaleTypes=await  _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "ShipModeId" });

            var allProfitData = new Dictionary<Guid, ProfitReport>();

            foreach (var id in airImportMawbsIds.Concat(airExportMawbsIds))
            {
                allProfitData[id] = await GetProfit(id, 0, _invoiceAppService, _containerAppService,_oceanExportHblAppService,_oceanImportHblAppService,_airExportHawbAppService, _airImportHawbAppService, SaleTypes);
            }

            foreach (var id in oceanImportMblsIds.Concat(oceanExportMblsIds))
            {
                allProfitData[id] = await GetProfit(id, 3, _invoiceAppService, _containerAppService, _oceanExportHblAppService, _oceanImportHblAppService, _airExportHawbAppService, _airImportHawbAppService, SaleTypes);
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
                                      ProfitAmt = allProfitData[mb.Id].ARTotal + allProfitData[mb.Id].DCTotal - allProfitData[mb.Id].APTotal,
                                      ProfitMargin = allProfitData[mb.Id].ProfitMargin,
                                      Avg_Profit_Per_Cntr = allProfitData[mb.Id].Avg_Profit_Per_Cntr,
                                      V20 = 0,
                                      V40 = 0,
                                      HC  = 0,
                                      V45 = 0,
                                      RF  = 0,
                                      SOC = 0,
                                      ETC = 0
                                  }).Distinct().AsEnumerable();

                var airExports = (from mb in _dbContext.AirExportMawbs
                                  join hb in _dbContext.AirExportHawbs on mb.Id equals hb.MawbId
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
                                      ProfitAmt = allProfitData[mb.Id].ARTotal + allProfitData[mb.Id].DCTotal - allProfitData[mb.Id].APTotal,
                                      ProfitMargin = allProfitData[mb.Id].ProfitMargin,
                                      Avg_Profit_Per_Cntr = allProfitData[mb.Id].Avg_Profit_Per_Cntr,
                                      V20 = 0,
                                      V40 = 0,
                                      HC = 0,
                                      V45 = 0,
                                      RF = 0,
                                      SOC = 0,
                                      ETC = 0
                                  }).Distinct().AsEnumerable();

                var oceanImports = (from oi in _dbContext.OceanImportMbls
                                    //join oih in _dbContext.OceanImportHbls on oi.Id equals oih.MblId
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
                                        
                                        ShipModeId=oi.ShipModeId,
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
                                        ProfitAmt = allProfitData[oi.Id].ARTotal + allProfitData[oi.Id].DCTotal - allProfitData[oi.Id].APTotal,
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
                                    
                var oceanExports = (from oe in _dbContext.OceanExportMbls
                                    //join oeh in _dbContext.OceanExportHbls on oe.Id equals oeh.MblId
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
                                        ShipModeId=oe.ShipModeId,
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
                                        ProfitAmt = allProfitData[oe.Id].ARTotal + allProfitData[oe.Id].DCTotal - allProfitData[oe.Id].APTotal,
                                        ProfitMargin = allProfitData[oe.Id].ProfitMargin,
                                        Avg_Profit_Per_Cntr = allProfitData[oe.Id].Avg_Profit_Per_Cntr,
                                        Volume = GetContainerTypeCount(resultMawbs, oe.Id)
                                    }).AsEnumerable();

                IEnumerable<MawbReport> result = new List<MawbReport>();


                if (filter.IsOceanExport)
                {
                    if (filter.ShipModeId != null)
                    {
                        oceanExports = oceanExports.Where(w => (w.ShipModeId != null) && w.ShipModeId.Equals(filter.ShipModeId)).ToList();

                    }
                    result = oceanExports;
                }
                if (filter.IsOceanImport)
                {
                    if (result != null)
                    result = result.Union(oceanImports);
                   
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

        public async Task<ReportLogPdf> GetMawbPdfReport(MawbReportDto filter)
        {
            ReportLogPdf result = new ReportLogPdf();
            var _dbContext = await _dbContextProvider.GetDbContextAsync();
            var cargoTypes = _dbContext.SysCodes.Where(s => s.CodeType == "cargoTypeId")
                .Select(s => new SysCodeForReport { Id = s.Id.ToString(), ShowName = s.ShowName, CodeType = s.CodeType });

            var resultMawbs = (from hb in _dbContext.ContainerSizes
                               join mb in _dbContext.Containers on hb.Id equals mb.ContainerSizeId
                               select new PackageSizeReport()
                               {
                                   ContainerId = mb.Id,
                                   ContainerCode = hb.ContainerCode,
                                   MblId = mb.MblId
                               }).ToList();
            List<Guid> airImportMawbsIds = new List<Guid>();
            List<Guid> airExportMawbsIds = new List<Guid>();
            List<Guid> oceanImportMblsIds = new List<Guid>();
            List<Guid> oceanExportMblsIds = new List<Guid>();
            if(filter.IsAirImport)
            airImportMawbsIds = await _dbContext.AirImportMawbs.Select(s => s.Id).ToListAsync();
            if(filter.IsAirExport)
            airExportMawbsIds = await _dbContext.AirExportMawbs.Select(s => s.Id).ToListAsync();
            if(filter.IsOceanImport)
             oceanImportMblsIds = await _dbContext.OceanImportMbls.Select(s => s.Id).ToListAsync();
            if(filter.IsOceanExport)
             oceanExportMblsIds = await _dbContext.OceanExportMbls.Select(s => s.Id).ToListAsync();
            var ShipModes = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "ShipModeId" });
            var SalesType = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "MblSalesTypeId" });
            var allProfitData = new Dictionary<Guid, ProfitReport>();
           if(filter.IsAirImport)
            foreach (var id in airImportMawbsIds.Concat(airExportMawbsIds))
            {
                allProfitData[id] = await GetProfit(id, 0, _invoiceAppService,_containerAppService, _oceanExportHblAppService, _oceanImportHblAppService, _airExportHawbAppService,_airImportHawbAppService, SalesType);
            }
           if(filter.IsOceanExport)
            foreach (var id in oceanImportMblsIds.Concat(oceanExportMblsIds))
            {
                allProfitData[id] = await GetProfit(id, 3, _invoiceAppService, _containerAppService, _oceanExportHblAppService, _oceanImportHblAppService, _airExportHawbAppService, _airImportHawbAppService, SalesType);
            }
            IEnumerable<MawbReport> OceanExports = new List<MawbReport>();

            IEnumerable<MawbReport> OceanImports = new List<MawbReport>();
            IEnumerable<MawbReport> AirExports = new List<MawbReport>();
            IEnumerable<MawbReport> AirImports = new List<MawbReport>();
            try
            {
                if (filter.IsAirImport)
                {
                    AirImports = (from mb in _dbContext.AirImportMawbs
                                
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
                                          ProfitAmt = allProfitData[mb.Id].ARTotal + allProfitData[mb.Id].DCTotal - allProfitData[mb.Id].APTotal,
                                          ProfitMargin = allProfitData[mb.Id].ProfitMargin,
                                          Avg_Profit_Per_Cntr = allProfitData[mb.Id].Avg_Profit_Per_Cntr,
                                          FreeProfit = allProfitData[mb.Id].FreeProfit,
                                          NomiProfit = allProfitData[mb.Id].NomiProfit,
                                          CoProfit = allProfitData[mb.Id].CoProfit,
                                          ETCProfit = allProfitData[mb.Id].ETCProfit,
                                          HblFreeProfit = allProfitData[mb.Id].HblFreeProfit,
                                          HblNomiProfit = allProfitData[mb.Id].HblNomiProfit,
                                          HblCoProfit = allProfitData[mb.Id].HblCoProfit,
                                          HblETCProfit = allProfitData[mb.Id].HblETCProfit,
                                          V20 = 0,
                                          V40 = 0,
                                          HC = 0,
                                          V45 = 0,
                                          RF = 0,
                                          SOC = 0,
                                          ETC = 0,
                                          GrossWeightKG=mb.GrossWeightKg,
                                          GrossWeightLB=mb.GrossWeightLb,
                                          ChargeableWeightKG=mb.ChargeableWeightKg,
                                          ChargeableWeightLB=mb.ChargeableWeightLb,
                                          
                                      }).Distinct().AsEnumerable();
            }
               if (filter.IsAirExport)
            {
                    AirExports = (from mb in _dbContext.AirExportMawbs
                                  join hb in _dbContext.AirExportHawbs on mb.Id equals hb.MawbId
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
                                      ProfitAmt = allProfitData[mb.Id].ARTotal + allProfitData[mb.Id].DCTotal - allProfitData[mb.Id].APTotal,
                                      ProfitMargin = allProfitData[mb.Id].ProfitMargin,
                                      Avg_Profit_Per_Cntr = allProfitData[mb.Id].Avg_Profit_Per_Cntr,
                                      FreeProfit = allProfitData[mb.Id].FreeProfit,
                                      NomiProfit = allProfitData[mb.Id].NomiProfit,
                                      CoProfit = allProfitData[mb.Id].CoProfit,
                                      ETCProfit = allProfitData[mb.Id].ETCProfit,
                                      HblFreeProfit = allProfitData[mb.Id].HblFreeProfit,
                                      HblNomiProfit = allProfitData[mb.Id].HblNomiProfit,
                                      HblCoProfit = allProfitData[mb.Id].HblCoProfit,
                                      HblETCProfit = allProfitData[mb.Id].HblETCProfit,
                                      V20 = 0,
                                      V40 = 0,
                                      HC = 0,
                                      V45 = 0,
                                      RF = 0,
                                      SOC = 0,
                                      ETC = 0,
                                      GrossWeightKG = mb.GrossWeightKg,
                                      GrossWeightLB = mb.GrossWeightLb,
                                      ChargeableWeightKG = mb.ChargeableWeightKg,
                                      ChargeableWeightLB = mb.ChargeableWeightLb,
                                  }).Distinct().AsEnumerable();
            }
            if (filter.IsOceanImport) {
                    OceanImports = (from oi in _dbContext.OceanImportMbls
                                        //join oih in _dbContext.OceanImportHbls on oi.Id equals oih.MblId
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

                                        ShipModeId = oi.ShipModeId,
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
                                        ProfitAmt = allProfitData[oi.Id].ARTotal + allProfitData[oi.Id].DCTotal - allProfitData[oi.Id].APTotal,
                                        ProfitMargin = allProfitData[oi.Id].ProfitMargin,
                                        Avg_Profit_Per_Cntr = allProfitData[oi.Id].Avg_Profit_Per_Cntr,
                                        FreeProfit = allProfitData[oi.Id].FreeProfit,
                                        NomiProfit = allProfitData[oi.Id].NomiProfit,
                                        CoProfit = allProfitData[oi.Id].CoProfit,
                                        ETCProfit = allProfitData[oi.Id].ETCProfit,
                                        HblFreeProfit = allProfitData[oi.Id].HblFreeProfit,
                                        HblNomiProfit = allProfitData[oi.Id].HblNomiProfit,
                                        HblCoProfit = allProfitData[oi.Id].HblCoProfit,
                                        HblETCProfit = allProfitData[oi.Id].HblETCProfit,
                                        V20 = GetContainerTypeCount(resultMawbs, oi.Id).V20,
                                        V40 = GetContainerTypeCount(resultMawbs, oi.Id).V40,
                                        HC = GetContainerTypeCount(resultMawbs, oi.Id).HC,
                                        V45 = GetContainerTypeCount(resultMawbs, oi.Id).V45,
                                        RF = GetContainerTypeCount(resultMawbs, oi.Id).RF,
                                        SOC = GetContainerTypeCount(resultMawbs, oi.Id).SOC,
                                        ETC = GetContainerTypeCount(resultMawbs, oi.Id).ETC
                                    }).AsEnumerable();
            }
            if (filter.IsOceanExport)
            {
               OceanExports = (from oe in _dbContext.OceanExportMbls
                                        //join oeh in _dbContext.OceanExportHbls on oe.Id equals oeh.MblId
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
                                        ShipModeId = oe.ShipModeId,
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
                                        ProfitAmt = allProfitData[oe.Id].ARTotal + allProfitData[oe.Id].DCTotal - allProfitData[oe.Id].APTotal,
                                        ProfitMargin = allProfitData[oe.Id].ProfitMargin,
                                        Avg_Profit_Per_Cntr = allProfitData[oe.Id].Avg_Profit_Per_Cntr,
                                        FreeProfit= allProfitData[oe.Id].FreeProfit,
                                        NomiProfit= allProfitData[oe.Id].NomiProfit,
                                        CoProfit= allProfitData[oe.Id].CoProfit,
                                        ETCProfit= allProfitData[oe.Id].ETCProfit,
                                        HblFreeProfit = allProfitData[oe.Id].HblFreeProfit,
                                        HblNomiProfit = allProfitData[oe.Id].HblNomiProfit,
                                        HblCoProfit = allProfitData[oe.Id].HblCoProfit,
                                        HblETCProfit = allProfitData[oe.Id].HblETCProfit,
                                        Volume = GetContainerTypeCount(resultMawbs, oe.Id),
                                        V20 = GetContainerTypeCount(resultMawbs, oe.Id).V20,
                                        V40 = GetContainerTypeCount(resultMawbs, oe.Id).V40,
                                        HC = GetContainerTypeCount(resultMawbs, oe.Id).HC,
                                        V45 = GetContainerTypeCount(resultMawbs, oe.Id).V45,
                                        RF = GetContainerTypeCount(resultMawbs, oe.Id).RF,
                                        SOC = GetContainerTypeCount(resultMawbs, oe.Id).SOC,
                                        ETC = GetContainerTypeCount(resultMawbs, oe.Id).ETC,
                                       
                                    }).AsEnumerable();
            }
                result.AirExports = AirExports.ToList();
                result.AirImports = AirImports.ToList();
                result.OceanExports = OceanExports.ToList();
                result.OceanImports = OceanImports.ToList();
                foreach (var item in result.OceanImports)
                {

                    item.ShipModeVolume = await GetShipModeVolumeCount(item.Id, item.ShipModeId, _oceanImportHblAppService, _oceanExportHblAppService, _containerAppService, ShipModes, true);
                    item.LCL = (double)(item.ShipModeVolume?.Lcl);
                    item.FCL = (double)(item.ShipModeVolume?.Fcl);
                    item.CoLoaded = (double)(item.ShipModeVolume?.CoLoaded);
                    item.Bulk = (double)(item.ShipModeVolume?.Bulk);
                    item.HblLCL = (double)(item.ShipModeVolume?.HblLcl);
                    item.HblFCL = (double)(item.ShipModeVolume?.HblFcl);
                    item.HblCoLoaded = (double)(item.ShipModeVolume?.HblCoLoaded);
                    item.HblBulk = (double)(item.ShipModeVolume?.HblBulk);
                    item.HblVolumeReport = await GetHblContainerTypeCount(item.Id, _oceanExportHblAppService, _oceanImportHblAppService, _containerAppService, _containerSizeAppService, true);
                    item.HblV20 = (int)(item.HblVolumeReport?.V20!=null? item.HblVolumeReport?.V20:0);
                    item.HblV40 = (int)(item.HblVolumeReport?.V40 != null ? item.HblVolumeReport?.V40 : 0);
                    item.HblV45 = (int)(item.HblVolumeReport?.V45 != null ? item.HblVolumeReport?.V45 : 0);
                    item.HblHC = (int)(item.HblVolumeReport?.HC != null ? item.HblVolumeReport?.HC : 0);
                    item.HblRF = (int)(item.HblVolumeReport?.RF != null ? item.HblVolumeReport?.RF : 0);
                    item.HblSOC = (int)(item.HblVolumeReport?.SOC != null ? item.HblVolumeReport?.SOC : 0);
                    item.HblCntETC = (int)(item.HblVolumeReport?.ETC != null ? item.HblVolumeReport?.ETC : 0);
                    item.HblVolumeReport = await GetHblContainerTypeCount(item.Id, _oceanExportHblAppService, _oceanImportHblAppService, _containerAppService, _containerSizeAppService, true);
                    item.HblV20 = (int)(item.HblVolumeReport?.V20 != null ? item.HblVolumeReport?.V20 : 0);
                    item.HblV40 = (int)(item.HblVolumeReport?.V40 != null ? item.HblVolumeReport?.V40 : 0);
                    item.HblV45 = (int)(item.HblVolumeReport?.V45 != null ? item.HblVolumeReport?.V45 : 0);
                    item.HblHC = (int)(item.HblVolumeReport?.HC != null ? item.HblVolumeReport?.HC : 0);
                    item.HblRF = (int)(item.HblVolumeReport?.RF != null ? item.HblVolumeReport?.RF : 0);
                    item.HblSOC = (int)(item.HblVolumeReport?.SOC != null ? item.HblVolumeReport?.SOC : 0);
                    item.HblCntETC = (int)(item.HblVolumeReport?.ETC != null ? item.HblVolumeReport?.ETC : 0);


                }
                foreach (var item in result.OceanExports)
                {

                    item.ShipModeVolume = await GetShipModeVolumeCount(item.Id, item.ShipModeId, _oceanImportHblAppService, _oceanExportHblAppService, _containerAppService, ShipModes, false);
                    item.LCL = (double)(item.ShipModeVolume?.Lcl);
                    item.FCL = (double)(item.ShipModeVolume?.Fcl);
                    item.CoLoaded = (double)(item.ShipModeVolume?.CoLoaded);
                    item.Bulk = (double)(item.ShipModeVolume?.Bulk);
                    item.HblLCL = (double)(item.ShipModeVolume?.HblLcl);
                    item.HblFCL = (double)(item.ShipModeVolume?.HblFcl);
                    item.HblCoLoaded = (double)(item.ShipModeVolume?.HblCoLoaded);
                    item.HblBulk = (double)(item.ShipModeVolume?.HblBulk);
                    item.SaleVolume = await GetSaleTypeVolumeCount(item.Id, _oceanImportHblAppService, _oceanExportHblAppService, _containerAppService, SalesType, false);
                    item.SaleFree = (double)(item.SaleVolume?.Free);
                    item.SaleNomi = (double)(item.SaleVolume?.Nomi);
                    item.SaleCo = (double)(item.SaleVolume?.Co);
                    item.SaleETC = (double)(item.SaleVolume?.ETC);
                    item.HblFree = (double)(item.SaleVolume?.HblFree);
                    item.HblNomi = (double)(item.SaleVolume?.HblNomi);
                    item.HblCo = (double)(item.SaleVolume?.HblCo);
                    item.HblETC = (double)(item.SaleVolume?.HblETC);

                }
             

                //if (filter.IsMisc)
                //if (filter.IsWarehouse)
                //if (filter.IsTruck)
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }

        }



        public static async Task<ProfitReport> GetProfit(Guid Id, int queryType, IInvoiceAppService invoiceAppService,IContainerAppService containerAppService,IOceanExportHblAppService oceanExportHblAppService,IOceanImportHblAppService oceanImportHblAppService,IAirExportHawbAppService airExportHawbAppService,IAirImportHawbAppService airImportHawbAppService,List<SysCodeDto> SaleTypes)
        {
            int containerCont = 1;
            ProfitReport data = new();
            ProfitReport dataHbl = new();
            if (queryType == 3)
            {
                QueryContainerDto dto = new QueryContainerDto();
                dto.QueryId = Id;
                var containers = await containerAppService.QueryListAsync(dto);
                containerCont = containers.Count > 0 ? containers.Count : 1;
                    }
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
                if (queryType == 3)
                {
                    var hbls = await oceanImportHblAppService.QueryListByMidAsync(new ImportExport.OceanImports.QueryHblDto { MblId = Id });
                    if (hbls.Count > 0)
                    {
                        foreach (var hbl in hbls)
                        {
                            QueryInvoiceDto Hblquery = new QueryInvoiceDto() { QueryType = 1, ParentId = Id };

                            dataHbl.Invoices = (await invoiceAppService.QueryInvoicesAsync(Hblquery)).ToList();

                            if (dataHbl.Invoices != null && dataHbl.Invoices.Count > 0)
                            {
                                dataHbl.AR = new List<InvoiceDto>();
                                dataHbl.AP = new List<InvoiceDto>();
                                dataHbl.DC = new List<InvoiceDto>();
                                foreach (var dto in data.Invoices)
                                {
                                    switch (dto.InvoiceType)
                                    {
                                        default:
                                            dataHbl.AR.Add(dto);
                                            break;
                                        case 1:
                                            dataHbl.DC.Add(dto);
                                            break;
                                        case 2:
                                            dataHbl.AP.Add(dto);
                                            break;
                                    }
                                }

                                if (dataHbl.AR.Any())
                                {
                                    double arTotal = 0;
                                    foreach (var ar in dataHbl.AR)
                                    {
                                        arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                                    }
                                    dataHbl.ARTotal = arTotal;
                                }
                                if (dataHbl.AP.Any())
                                {
                                    double apTotal = 0;
                                    foreach (var ap in dataHbl.AP)
                                    {
                                        apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                                    }

                                    dataHbl.APTotal = apTotal;
                                }
                                if (dataHbl.DC.Any())
                                {
                                    double dcTotal = 0;
                                    foreach (var dc in dataHbl.DC)
                                    {
                                        dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                                    }
                                    dataHbl.DCTotal = dcTotal;
                                }
                            }
                                var saleType1 = SaleTypes.Where(x => x.Id == hbl.HblSalesTypeId).FirstOrDefault();
                            if (saleType1?.CodeValue == "F")
                            {
                                dataHbl.FreeProfit = dataHbl.ARTotal + dataHbl.DCTotal - dataHbl.APTotal;

                            }
                            else
                            if (saleType1?.CodeValue == "N")
                            {
                                dataHbl.NomiProfit = dataHbl.ARTotal + dataHbl.DCTotal - dataHbl.APTotal;
                            }
                            else
                            if (saleType1?.CodeValue == "C")
                            {
                                dataHbl.CoProfit = dataHbl.ARTotal + dataHbl.DCTotal - dataHbl.APTotal;
                            }
                            else
                            {
                                dataHbl.ETCProfit = dataHbl.ARTotal + dataHbl.DCTotal - dataHbl.APTotal;
                            }
                        }
                        var saleType = SaleTypes.Where(x => x.Id == hbls[0].HblSalesTypeId).FirstOrDefault();
                        if (saleType?.CodeValue == "F")
                        {
                            data.FreeProfit = data.ARTotal + data.DCTotal - data.APTotal;

                        }
                        else
                        if (saleType?.CodeValue == "N")
                        {
                            data.NomiProfit = data.ARTotal + data.DCTotal - data.APTotal;
                        }
                        else
                        if (saleType?.CodeValue == "C")
                        {
                            data.CoProfit = data.ARTotal + data.DCTotal - data.APTotal;
                        }
                        else
                        {
                            data.ETCProfit = data.ARTotal + data.DCTotal - data.APTotal;
                        }
                    }
                    else {

                        data.ETCProfit = data.ARTotal + data.DCTotal - data.APTotal;

                    }
                }
                else {

                    var hbls = await oceanImportHblAppService.QueryListByMidAsync(new ImportExport.OceanImports.QueryHblDto { MblId = Id });
                    if (hbls.Count > 0)
                    {
                        foreach (var hbl in hbls)
                        {
                            QueryInvoiceDto Hblquery = new QueryInvoiceDto() { QueryType = 4, ParentId = Id };

                            dataHbl.Invoices = (await invoiceAppService.QueryInvoicesAsync(Hblquery)).ToList();

                            if (dataHbl.Invoices != null && dataHbl.Invoices.Count > 0)
                            {
                                dataHbl.AR = new List<InvoiceDto>();
                                dataHbl.AP = new List<InvoiceDto>();
                                dataHbl.DC = new List<InvoiceDto>();
                                foreach (var dto in data.Invoices)
                                {
                                    switch (dto.InvoiceType)
                                    {
                                        default:
                                            dataHbl.AR.Add(dto);
                                            break;
                                        case 1:
                                            dataHbl.DC.Add(dto);
                                            break;
                                        case 2:
                                            dataHbl.AP.Add(dto);
                                            break;
                                    }
                                }

                                if (dataHbl.AR.Any())
                                {
                                    double arTotal = 0;
                                    foreach (var ar in dataHbl.AR)
                                    {
                                        arTotal += ar.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                                    }
                                    dataHbl.ARTotal = arTotal;
                                }
                                if (dataHbl.AP.Any())
                                {
                                    double apTotal = 0;
                                    foreach (var ap in dataHbl.AP)
                                    {
                                        apTotal += ap.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                                    }

                                    dataHbl.APTotal = apTotal;
                                }
                                if (dataHbl.DC.Any())
                                {
                                    double dcTotal = 0;
                                    foreach (var dc in dataHbl.DC)
                                    {
                                        dcTotal += dc.InvoiceBillDtos.Sum(s => (s.Rate * s.Quantity));
                                    }
                                    dataHbl.DCTotal = dcTotal;
                                }
                            }
                            var saleType1 = SaleTypes.Where(x => x.Id == hbl.HblSalesTypeId).FirstOrDefault();
                            if (saleType1?.CodeValue == "F")
                            {
                                dataHbl.FreeProfit = dataHbl.ARTotal + dataHbl.DCTotal - dataHbl.APTotal;

                            }
                            else
                            if (saleType1?.CodeValue == "N")
                            {
                                dataHbl.NomiProfit = dataHbl.ARTotal + dataHbl.DCTotal - dataHbl.APTotal;
                            }
                            else
                            if (saleType1?.CodeValue == "C")
                            {
                                dataHbl.CoProfit = dataHbl.ARTotal + dataHbl.DCTotal - dataHbl.APTotal;
                            }
                            else
                            {
                                dataHbl.ETCProfit = dataHbl.ARTotal + dataHbl.DCTotal - dataHbl.APTotal;
                            }
                        }
                        var saleType = SaleTypes.Where(x => x.Id == hbls[0].HblSalesTypeId).FirstOrDefault();
                        if (saleType?.CodeValue == "F")
                        {
                            data.FreeProfit = data.ARTotal + data.DCTotal - data.APTotal;

                        }
                        else
                        if (saleType?.CodeValue == "N")
                        {
                            data.NomiProfit = data.ARTotal + data.DCTotal - data.APTotal;
                        }
                        else
                        if (saleType?.CodeValue == "C")
                        {
                            data.CoProfit = data.ARTotal + data.DCTotal - data.APTotal;
                        }
                        else
                        {
                            data.ETCProfit = data.ARTotal + data.DCTotal - data.APTotal;
                        }
                    }
                    else
                    {

                        data.ETCProfit = data.ARTotal + data.DCTotal - data.APTotal;

                    }

                }
            }
            if (data.ARTotal != 0)
            {
                if (data.APTotal != 0)
                {
                    data.ProfitMargin = (((data.ARTotal - data.APTotal) / data.ARTotal));

                }
                else {

                    data.ProfitMargin = (((data.ARTotal - data.DCTotal) / data.ARTotal));


                }
                
            }
            else
            {
                // Handle division by zero case, e.g., assign 0 or "N/A"
                data.ProfitMargin = 0; // or "N/A"
            }
            data.Avg_Profit_Per_Cntr = (data.ARTotal + data.DCTotal - data.APTotal) / containerCont;
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
                        volume.V20 += 1;
                        volume.ETC += 1;
                        break;

                    case "20DC":
                        volume.V20 += 1;
                        volume.ETC += 1;
                        break;

                    case "20FR":
                        volume.V20 += 1;
                        volume.ETC += 1;
                        break;

                    case "20FL":
                        volume.V20 += 1;
                        volume.ETC += 1;
                        break;

                    case "40DC":
                        volume.V40 += 1;
                        volume.ETC += 1;
                        break;

                    case "40FL":
                        volume.V40 += 1;
                        volume.ETC += 1;
                        break;

                    case "40FR":
                        volume.V40 += 1;
                        volume.ETC += 1;
                        break;

                    case "40GH":
                        volume.V40 += 1;
                        volume.ETC = +1;
                        break;

                    case "12RF":
                        volume.RF += 1;
                        
                        break;

                    case "53FT":
                        volume.ETC += 1;
                        break;

                    case "2":
                        volume.ETC += 1;
                        break;

                    case "101":
                        volume.ETC += 1;
                        break;

                    case "102":
                        volume.ETC += 1;
                        break;

                    default:
                        volume.HC = 0;
                        volume.SOC = 0;
                        break;
                }
            }

            return volume;
        }

        public static async Task<HblVolumeReport> GetHblContainerTypeCount(Guid Id,IOceanExportHblAppService oceanExportHblAppService,IOceanImportHblAppService oceanImportHblAppService,IContainerAppService containerAppService,IContainerSizeAppService containerSizeAppService,bool IsOceanImport)
        {
            HblVolumeReport volume = new HblVolumeReport();
            if (IsOceanImport)
            {
               var MblContainers= await containerAppService.QueryListAsync(new QueryContainerDto { QueryId = Id });
                if (MblContainers.Count > 0)
                {
                    var containerSize = await containerSizeAppService.GetAsync(MblContainers.Select(x => x.ContainerSizeId).FirstOrDefault());
                    var hbls = await oceanImportHblAppService.QueryListByMidAsync(new ImportExport.OceanImports.QueryHblDto { MblId = Id });

                    foreach (var hbl in hbls)
                    {
                        var hblContainer = await containerAppService.GetContainerByHblId(hbl.Id);
                        if (hblContainer != null)
                        {
                            switch (containerSize?.ContainerCode)
                            {
                                case "20":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20DC":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20FR":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20FL":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40DC":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40FL":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40FR":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40GH":
                                    volume.V40 += 1;
                                    volume.ETC = +1;
                                    break;

                                case "12RF":
                                    volume.RF += 1;

                                    break;

                                case "53FT":
                                    volume.ETC += 1;
                                    break;

                                case "2":
                                    volume.ETC += 1;
                                    break;

                                case "101":
                                    volume.ETC += 1;
                                    break;

                                case "102":
                                    volume.ETC += 1;
                                    break;

                                default:
                                    volume.HC = 0;
                                    volume.SOC = 0;
                                    break;
                            }


                        }
                    }



                }
                else {

                    var hbls = await oceanImportHblAppService.QueryListByMidAsync(new ImportExport.OceanImports.QueryHblDto { MblId = Id });

                    foreach (var hbl in hbls)
                    {
                        var hblContainer = await containerAppService.GetContainerByHblId(hbl.Id);
                        
                        if (hblContainer != null)
                        {
                            var containerSize = await containerSizeAppService.GetAsync(hblContainer.ContainerSizeId);
                            switch (containerSize?.ContainerCode)
                            {
                                case "20":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20DC":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20FR":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20FL":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40DC":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40FL":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40FR":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40GH":
                                    volume.V40 += 1;
                                    volume.ETC = +1;
                                    break;

                                case "12RF":
                                    volume.RF += 1;

                                    break;

                                case "53FT":
                                    volume.ETC += 1;
                                    break;

                                case "2":
                                    volume.ETC += 1;
                                    break;

                                case "101":
                                    volume.ETC += 1;
                                    break;

                                case "102":
                                    volume.ETC += 1;
                                    break;

                                default:
                                    volume.HC = 0;
                                    volume.SOC = 0;
                                    break;
                            }


                        }
                    }


                }
            }

            else
            {
                var MblContainers = await containerAppService.QueryListAsync(new QueryContainerDto { QueryId = Id });
                if (MblContainers.Count > 0)
                {
                    var containerSize = await containerSizeAppService.GetAsync(MblContainers.Select(x => x.ContainerSizeId).FirstOrDefault());
                    var hbls = await oceanExportHblAppService.QueryListByMidAsync(new ImportExport.OceanExports.QueryHblDto { MblId = Id });

                    foreach (var hbl in hbls)
                    {
                        var hblContainer = await containerAppService.GetContainerByHblId(hbl.Id);
                        if (hblContainer != null)
                        {
                            switch (containerSize?.ContainerCode)
                            {
                                case "20":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20DC":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20FR":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20FL":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40DC":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40FL":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40FR":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40GH":
                                    volume.V40 += 1;
                                    volume.ETC = +1;
                                    break;

                                case "12RF":
                                    volume.RF += 1;

                                    break;

                                case "53FT":
                                    volume.ETC += 1;
                                    break;

                                case "2":
                                    volume.ETC += 1;
                                    break;

                                case "101":
                                    volume.ETC += 1;
                                    break;

                                case "102":
                                    volume.ETC += 1;
                                    break;

                                default:
                                    volume.HC = 0;
                                    volume.SOC = 0;
                                    break;
                            }


                        }
                    }



                }
                else
                {

                    var hbls = await oceanExportHblAppService.QueryListByMidAsync(new ImportExport.OceanExports.QueryHblDto { MblId = Id });

                    foreach (var hbl in hbls)
                    {
                        var hblContainer = await containerAppService.GetContainerByHblId(hbl.Id);
                       
                        if (hblContainer != null)
                        {
                            var containerSize = await containerSizeAppService.GetAsync(hblContainer.ContainerSizeId);
                            switch (containerSize?.ContainerCode)
                            {
                                case "20":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20DC":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20FR":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "20FL":
                                    volume.V20 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40DC":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40FL":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40FR":
                                    volume.V40 += 1;
                                    volume.ETC += 1;
                                    break;

                                case "40GH":
                                    volume.V40 += 1;
                                    volume.ETC = +1;
                                    break;

                                case "12RF":
                                    volume.RF += 1;

                                    break;

                                case "53FT":
                                    volume.ETC += 1;
                                    break;

                                case "2":
                                    volume.ETC += 1;
                                    break;

                                case "101":
                                    volume.ETC += 1;
                                    break;

                                case "102":
                                    volume.ETC += 1;
                                    break;

                                default:
                                    volume.HC = 0;
                                    volume.SOC = 0;
                                    break;
                            }


                        }
                    }


                }
            }



            return volume;
        }

        public static async Task<ShipModeVolumeReport> GetShipModeVolumeCount(Guid Id,Guid? shipModeId,IOceanImportHblAppService oceanImportHblAppService,IOceanExportHblAppService oceanExportHblAppService,IContainerAppService containerAppService,List<SysCodeDto> ShipModes,bool IsOceanImport)
        {
            ShipModeVolumeReport report = new ShipModeVolumeReport();
            if (IsOceanImport)
            {
                if (shipModeId != null)
                {
                    ImportExport.OceanImports.QueryHblDto dto = new ImportExport.OceanImports.QueryHblDto() { MblId = Id };

                    var hbls = await oceanImportHblAppService.QueryListByMidAsync(dto);
                    foreach (var hbl in hbls)
                    {


                        var container = await containerAppService.GetContainerByHblId(hbl.Id);
                        var saleType = ShipModes.Where(x => x.Id == (Guid)shipModeId).FirstOrDefault();
                        if (saleType.CodeValue == "FCL")
                        {
                            report.HblFcl += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);


                        }
                        if (saleType.CodeValue == "LCL")
                        {
                            report.HblLcl += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);

                        }
                        if (saleType.CodeValue == "FAK")
                        {
                            report.HblCoLoaded += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);

                        }
                        if (saleType.CodeValue == "BLK")
                        {
                            report.HblBulk += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);

                        }


                    }
                    var MblContainers = await containerAppService.QueryListAsync(new QueryContainerDto { QueryId = Id });

                    foreach (var mbl in MblContainers)
                    {
                        var saleType = ShipModes.Where(x => x.Id == (Guid)shipModeId).FirstOrDefault();
                        if (saleType.CodeValue == "FCL")
                        {
                            report.Fcl += (mbl.PackageMeasure);


                        }
                        if (saleType.CodeValue == "LCL")
                        {
                            report.Lcl += mbl.PackageMeasure;

                        }
                        if (saleType.CodeValue == "FAK")
                        {
                            report.CoLoaded += mbl.PackageMeasure;

                        }
                        if (saleType.CodeValue == "BLK")
                        {
                            report.Bulk += mbl.PackageMeasure;
                        }
                    }

                    return report;
                }
                return report;

            }
            else
            {
                if (shipModeId != null)
                {
                    QueryHblDto dto = new QueryHblDto() { MblId = Id };

                    var hbls = await oceanExportHblAppService.QueryListByMidAsync(dto);
                    foreach (var hbl in hbls)
                    {


                        var container = await containerAppService.GetContainerByHblId(hbl.Id);
                        var saleType = ShipModes.Where(x => x.Id == (Guid)shipModeId).FirstOrDefault();
                        if (saleType.CodeValue == "FCL")
                        {
                            report.HblFcl = (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);


                        }
                        if (saleType.CodeValue == "LCL")
                        {
                            report.HblLcl = (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);

                        }
                        if (saleType.CodeValue == "FAK")
                        {
                            report.HblCoLoaded = (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);

                        }
                        if (saleType.CodeValue == "BLK")
                        {
                            report.HblBulk = (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);

                        }



                    }
                    var MblContainers = await containerAppService.QueryListAsync(new QueryContainerDto { QueryId = Id });

                    foreach (var mbl in MblContainers)
                    {
                        var saleType = ShipModes.Where(x => x.Id == (Guid)shipModeId).FirstOrDefault();
                        if (saleType.CodeValue == "FCL")
                        {
                            report.Fcl += (mbl.PackageMeasure);


                        }
                        if (saleType.CodeValue == "LCL")
                        {
                            report.Lcl += mbl.PackageMeasure;

                        }
                        if (saleType.CodeValue == "FAK")
                        {
                            report.CoLoaded += mbl.PackageMeasure;

                        }
                        if (saleType.CodeValue == "BLK")
                        {
                            report.Bulk += mbl.PackageMeasure;
                        }
                    }
                    return report;
                }
                return report;
            }
           

        }
        public static async Task<SaleVolumeReport> GetSaleTypeVolumeCount(Guid Id,  IOceanImportHblAppService oceanImportHblAppService, IOceanExportHblAppService oceanExportHblAppService, IContainerAppService containerAppService, List<SysCodeDto> SaleTypes, bool IsOceanImport)
        {
            SaleVolumeReport report = new SaleVolumeReport();
            if (IsOceanImport)
            {
                ImportExport.OceanImports.QueryHblDto dto = new ImportExport.OceanImports.QueryHblDto() { MblId = Id };

                var hbls = await oceanImportHblAppService.QueryListByMidAsync(dto);
                foreach (var hbl in hbls)
                {


                    var container = await containerAppService.GetContainerByHblId(hbl.Id);
                    var saleType = SaleTypes.Where(x => x.Id == hbl.HblSalesTypeId).FirstOrDefault();
                    if (saleType?.CodeValue == "F")
                    {
                        report.HblFree += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                        report.Free += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);

                    }
                    else
                    if (saleType?.CodeValue == "N")
                    {
                        report.HblNomi += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                        report.Nomi += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                    }
                    else
                    if (saleType?.CodeValue == "C")
                    {
                        report.HblCo += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                        report.Co += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                    }
                    else 
                    {
                        report.HblETC += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                        report.ETC += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                    }


                }
             
                return report;


            }
            else
            {

                QueryHblDto dto = new QueryHblDto() { MblId = Id };

                var hbls = await oceanExportHblAppService.QueryListByMidAsync(dto);
                foreach (var hbl in hbls)
                {


                    var container = await containerAppService.GetContainerByHblId(hbl.Id);
                    var saleType = SaleTypes.Where(x => x.Id == hbl.HblSaleId).FirstOrDefault();
                    if (saleType?.CodeValue == "F")
                    {
                        report.HblFree += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                        report.Free += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);

                    }
                    else
                    if (saleType?.CodeValue == "N")
                    {
                        report.HblNomi += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                        report.Nomi += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                    }
                    else
                    if (saleType?.CodeValue == "C")
                    {
                        report.HblCo += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                        report.Co += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                    }
                    else
                    {
                        report.HblETC += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                        report.ETC += (double)(container?.PackageMeasure != null ? container?.PackageMeasure : 0);
                    }

                }
                return report;
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
