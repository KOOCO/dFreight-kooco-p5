﻿using AutoMapper.Internal.Mappers;
using Dolphin.Freight.EntityFrameworkCore;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.ImportExport.OceanImports;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.ImportExport.Containers
{
    public class ContainerAppService :
        CrudAppService<
            Container,
            ContainerDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateContainerDto>,
        IContainerAppService
    {
        private IRepository<Container, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        public ContainerAppService(IRepository<Container, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository)
           : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
        }
        public async Task<List<ContainerDto>> QueryListAsync(QueryContainerDto query)
        {
            var Containers = await _repository.GetListAsync();

            if (query.QueryId != Guid.Empty) 
            {
                Containers = Containers.Where(x => x.MblId.Equals(query.QueryId)).ToList();
            }
            var list = ObjectMapper.Map<List<Container>, List<ContainerDto>>(Containers.OrderBy(o => o.CreationTime).ToList());
            return list;
        }
        public async Task<List<ContainerDto>> QueryListHblAsync(Guid hblId)
        {
            var Containers = await _repository.GetListAsync();

            if (hblId != Guid.Empty)
            {
                Containers = Containers.Where(x => x.HblId == hblId).ToList();
            }
            var list = ObjectMapper.Map<List<Container>, List<ContainerDto>>(Containers.ToList());
            return list;
        }
        public async Task<List<ContainerDto>> QueryListVesselAsync(Guid vesselId)
        {
            var Containers = await _repository.GetListAsync();

            if (vesselId != Guid.Empty)
            {
                Containers = Containers.Where(x => x.VesselId == vesselId).ToList();
            }
            var list = ObjectMapper.Map<List<Container>, List<ContainerDto>>(Containers.ToList());
            return list;
        }
        public async Task<List<ContainerDto>> QueryListBookingAsync(Guid bookingId)
        {
            var Containers = await _repository.GetListAsync();

            if (bookingId != Guid.Empty)
            {
                Containers = Containers.Where(x => x.BookingId == bookingId).ToList();
            }
            var list = ObjectMapper.Map<List<Container>, List<ContainerDto>>(Containers.ToList());
            return list;
        }
        public async Task<int> DeleteByBookingIdAsync(QueryContainerDto query)
        {
            var list = await this.QueryListBookingAsync(query.QueryId);
            foreach (var dto in list)
            {
                var did = dto.Id;
                await this.DeleteAsync(did);
            }
            return list.Count;
        }
        public async Task<int> DeleteByVesselIdAsync(QueryContainerDto query)
        {
            var list = await this.QueryListVesselAsync(query.QueryId);
            foreach (var dto in list)
            {
                var did = dto.Id;
                await this.DeleteAsync(did);
            }
            return list.Count;
        }
        public async Task<int> DeleteByMblIdAsync(QueryContainerDto query) 
        {
            var list = await this.QueryListAsync(query);
            foreach(var dto in list) 
            {
                var did = dto.Id;
                await this.DeleteAsync(did);
            }
            return list.Count;
        }
        public async Task SwitchPP(Guid id)
        {
            Container entity = await _repository.GetAsync(id);
            entity.IsPP = true;

            await _repository.UpdateAsync(entity);
        }
        public async Task SwitchCTF(Guid id)
        {
            Container entity = await _repository.GetAsync(id);
            entity.IsCTF = true;

            await _repository.UpdateAsync(entity);
        }
        public async Task<List<CreateUpdateContainerDto>> GetContainerByMblId(Guid id)
        {
            var list = await Repository.GetListAsync();
            var container = list.Where(w => w.MblId == id).ToList();
            var containerDto = ObjectMapper.Map<List<Container>, List<CreateUpdateContainerDto>>(container);

            return containerDto;
        }
        public async Task<CreateUpdateContainerDto> GetSingleContainerByMblId(Guid id)
        {
            var list = await Repository.GetListAsync();
            var container = list.Where(w => w.MblId == id).FirstOrDefault();
            var containerDto = ObjectMapper.Map<Container, CreateUpdateContainerDto>(container);

            return containerDto;
        }
        public async Task<CreateUpdateContainerDto> GetContainerByHblId(Guid id)
        {
            var list = await Repository.GetListAsync();
            var container = list.Where(w => w.HblId == id).FirstOrDefault();
            var containerDto = ObjectMapper.Map<Container, CreateUpdateContainerDto>(container);

            return containerDto;
        }
        public async Task<List<CreateUpdateContainerDto>> GetContainerListByHblId(Guid id)
        {
            var list = await Repository.GetListAsync();
            
            var container = list.Where(w => w.HblId == id).ToList();

            var containerDto = ObjectMapper.Map<List<Container>, List<CreateUpdateContainerDto>>(container);

            return containerDto.ToList();
        }
        public async Task<CreateUpdateContainerDto> GetContainerByBookingId(Guid id)
        {
            var list = await Repository.GetListAsync();
            var container = list.Where(w => w.BookingId == id).FirstOrDefault();
            var containerDto = ObjectMapper.Map<Container, CreateUpdateContainerDto>(container);

            return containerDto;
        }
        public async Task<List<CreateUpdateContainerDto>> GetContainersListByBookingId(Guid Id)
        {
            var list = await Repository.GetListAsync();
            var containerList = list.Where(w => w.BookingId == Id).ToList();

            return ObjectMapper.Map<List<Container>, List<CreateUpdateContainerDto>>(containerList);
        }
        public async Task<List<CreateUpdateContainerDto>> GetContainersByExtraPropertiesHblIds(Guid hblId, Guid MblId)
        {
            var containerList = await Repository.GetListAsync();

            containerList = containerList.Where(w => w.MblId == MblId && w.ExtraProperties != null && w.ExtraProperties.Count > 0).ToList();

            List<CreateUpdateContainerDto> containerDtoList = new();

            foreach (var item in containerList)
            {
                object extraProp = item.ExtraProperties.GetValueOrDefault("HblIds");

                if (extraProp is not null)
                {
                    if (extraProp.ToString().Contains(Convert.ToString(hblId)))
                    {
                        containerDtoList.Add(ObjectMapper.Map<Container, CreateUpdateContainerDto>(item));
                    }
                }
            }

            return containerDtoList.ToList();
        }
        public async Task<CreateUpdateContainerDto> GetSingleContainerByExtraPropertiesHblIds(Guid HblId, Guid MblId) {
            var containerList = await Repository.GetListAsync();

            containerList = containerList.Where(w => w.MblId == MblId && w.ExtraProperties != null && w.ExtraProperties.Count > 0).ToList();

            CreateUpdateContainerDto ContainerDto = new();

            foreach (var item in containerList)
            {
                object extraProp = item.ExtraProperties.GetValueOrDefault("HblIds");

                if (extraProp is not null)
                {
                    if (extraProp.ToString().Contains(Convert.ToString(HblId)))
                    {
                        ContainerDto = ObjectMapper.Map<Container, CreateUpdateContainerDto>(item);

                        return ContainerDto;
                    }
                }
            }

            return ContainerDto;
        }
        public async Task CreateMblHblContainerForCopiedOE_OI(Guid OldMblId, Guid NewMblId, Dictionary<Guid, Guid> hblIds)
        {
            var containers = await GetContainerByMblId(OldMblId);

            containers = containers.Where(w => w.ExtraProperties is not null && w.ExtraProperties.Count > 0).ToList();

            foreach (var item in containers)
            {
                List<Guid> newExtraPropList = new();

                object extraProp = item.ExtraProperties.GetValueOrDefault("HblIds");

                if (extraProp is not null)
                {
                    List<Guid> extraPropList = JsonConvert.DeserializeObject<List<Guid>>(extraProp.ToString());

                    foreach (var item1 in extraPropList)
                    {
                        if (hblIds.ContainsKey(item1))
                        {
                            var newItem1 = hblIds[item1];

                            newExtraPropList.Add(newItem1);
                        }
                    }

                    item.ExtraProperties.Remove("HblIds");

                    item.ExtraProperties.Add("HblIds", newExtraPropList);
                }

                item.Id = Guid.Empty;

                item.MblId = NewMblId;

                await CreateAsync(item);
            }
        }
    }
}