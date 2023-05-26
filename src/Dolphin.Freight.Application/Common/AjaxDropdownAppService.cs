﻿
using Dolphin.Freight.ImportExport.OceanExports;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.Common
{
    public class AjaxDropdownAppService :  ApplicationService,IAjaxDropdownAppService
    {
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IRepository<SysCode, Guid> _sysCideRepository;
        private readonly IRepository<Port, Guid> _portRepository;
        private readonly IRepository<OceanExportMbl, Guid> _oceanExportMblRepository;
        private readonly IRepository<VesselSchedule, Guid> _vesselScheduleRepository;
        public AjaxDropdownAppService(IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository, IRepository<SysCode, Guid> sysCideRepository, IRepository<Port, Guid> portRepository, IRepository<OceanExportMbl, Guid> oceanExportMblRepository, IRepository<VesselSchedule, Guid> vesselScheduleRepository)
        {
            _tradePartnerRepository = tradePartnerRepository;
            _sysCideRepository = sysCideRepository;
            _portRepository = portRepository;
            _oceanExportMblRepository = oceanExportMblRepository;
            _vesselScheduleRepository = vesselScheduleRepository;
        }
        public async Task<List<TradePartnerDto>> GetAllTradePartners(QueryDto query ) { 
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            var list = ObjectMapper.Map<List<Dolphin.Freight.TradePartners.TradePartner>, List<TradePartnerDto>>(tradePartners);
            return list;
        }
        public async Task<List<SysCodeDto>> GetSysCodeDtosByTypeAsync(QueryDto query) 
        {
            var sysCodes = await _sysCideRepository.GetListAsync();
            var rs = sysCodes.Where(x => x.CodeType.Equals(query.QueryType)).ToList();
            var list = ObjectMapper.Map<List<SysCode>, List<SysCodeDto>>(rs);
            return list;
        }
        public async Task<List<ReferenceItemDto>> GetReferenceItemsByTypeAsync(QueryDto query) {
            List<ReferenceItemDto> list = new();
            ReferenceItemDto dto;
            var ports = await _portRepository.GetListAsync();
            Dictionary<Guid, string> pdictionary = new();
            if (ports != null && ports.Count > 0) 
            {
                foreach (var port in ports) 
                {
                    pdictionary.Add(port.Id, port.SubDiv+" "+port.PortName+" ( "+port.Locode+" ) ");
                }
            }
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            Dictionary<Guid, string> tdictionary = new();
            if (tradePartners != null && tradePartners.Count > 0) 
            {
                foreach (var tradePartner in tradePartners) 
                {
                    tdictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }
            if (query.QueryType == null)
            {
                var vesselSchedules = await _vesselScheduleRepository.GetListAsync();
                if (vesselSchedules != null) 
                {
                    foreach (var item in vesselSchedules) 
                    {
                        dto = new ReferenceItemDto() {
                            Id = item.Id,
                            Pol = item.PolId,
                            Pod = item.PodId,
                            Eta = item.PodEta?.ToString("yyyy-MM-dd"),
                            Etd = item.PolEtd?.ToString("yyyy-MM-dd"),     
                            ReferenceType = 0,
                            ReferenceNo = item.ReferenceNo,
                            CarrierId = item.MblCarrierId
                        };
                        list.Add(dto);
                    }
                }
                var oceanExportMbls = await _oceanExportMblRepository.GetListAsync();
                if (oceanExportMbls != null)
                {
                    foreach (var item in oceanExportMbls)
                    {
                        dto = new ReferenceItemDto()
                        {
                            Id = item.Id,
                            Pol = item.PolId,
                            Pod = item.PodId,
                            Eta = item.PodEta?.ToString("yyyy-MM-dd"),
                            Etd = item.PolEtd?.ToString("yyyy-MM-dd"),
                            ReferenceType = 1,
                            ReferenceNo = item.FilingNo,
                            MblNo = item.MblNo,
                            CarrierId = item.MblCarrierId
                        };
                        list.Add(dto);
                    }
                }
            }
            else 
            {
                
                if ("0".Equals(query.QueryType))
                {
                    var vesselSchedules = await _vesselScheduleRepository.GetListAsync();
                    if (vesselSchedules != null)
                    {
                        foreach (var item in vesselSchedules)
                        {
                            dto = new ReferenceItemDto()
                            {
                                Id = item.Id,
                                Pol = item.PolId,
                                Pod = item.PodId,
                                Eta = item.PodEta?.ToString("yyyy-MM-dd"),
                                Etd = item.PolEtd?.ToString("yyyy-MM-dd"),
                                ReferenceType = 0,
                                ReferenceNo = item.ReferenceNo,
                                CarrierId = item.MblCarrierId
                            };
                            list.Add(dto);
                        }
                    }
                }
                else
                {
                    var oceanExportMbls = await _oceanExportMblRepository.GetListAsync();
                    if (oceanExportMbls != null)
                    {
                        foreach (var item in oceanExportMbls)
                        {
                            dto = new ReferenceItemDto()
                            {
                                Id = item.Id,
                                Pol = item.PolId,
                                Pod = item.PodId,
                                Eta = item.PodEta?.ToString("yyyy-MM-dd"),
                                Etd = item.PolEtd?.ToString("yyyy-MM-dd"),
                                ReferenceType = 1,
                                ReferenceNo = item.FilingNo ?? "",
                                MblNo = item.MblNo,
                                CarrierId = item.MblCarrierId
                            };
                            list.Add(dto);
                        }
                    }
                }
            }
            list = list.OrderBy(x=>x.Etd).ToList();
            foreach (var item in list) 
            {
                if (item.Pol != null) item.PolName = pdictionary[item.Pol.Value];
                if (item.Pod != null) item.PodName = pdictionary[item.Pod.Value];
                if(item.CarrierId != null) item.CarrierName = tdictionary[item.CarrierId.Value];

            }
            return list;
        }
    }
}
