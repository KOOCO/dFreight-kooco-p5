using AutoMapper.Internal.Mappers;
using Dolphin.Freight.ImportExport.Containers;
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
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
            var list = ObjectMapper.Map<List<Container>, List<ContainerDto>>(Containers.ToList());
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

            return containerDto;
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
    }
}