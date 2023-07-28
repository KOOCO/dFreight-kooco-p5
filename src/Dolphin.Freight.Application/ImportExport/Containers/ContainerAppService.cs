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

        public async Task<CreateUpdateContainerDto> GetContainerByHblId(Guid id)
        {
            var list = await Repository.GetListAsync();
            var container = list.Where(w => w.HblId == id).FirstOrDefault();
            var containerDto = ObjectMapper.Map<Container, CreateUpdateContainerDto>(container);

            return containerDto;
        }
    }
}