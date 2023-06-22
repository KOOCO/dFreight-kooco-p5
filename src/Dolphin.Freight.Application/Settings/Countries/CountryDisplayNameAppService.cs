using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.Offices;
using Dolphin.Freight.Settinngs.Countries;
using Dolphin.Freight.TradePartners;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.Settings.Countries
{
    public class CountryDisplayNameAppService : ApplicationService, ICountryDisplayNameAppService
    {
        private IRepository<CountryDisplayName, Guid> _repository;
        private IRepository<Country, Guid> _countryRepository;
        private IRepository<Airport, Guid> _airportRepository;
        private IRepository<Office, Guid> _officeRepository;

        public new ILogger<CountryDisplayNameAppService> Logger { get; set; }

        public CountryDisplayNameAppService(
            IRepository<CountryDisplayName, Guid> repository, 
            IRepository<Country, Guid> countryRepository,
            IRepository<Airport, Guid> airportRepository,
            IRepository<Office, Guid> officeRepository
        ) {
            _repository = repository;
            _countryRepository = countryRepository;
            _airportRepository = airportRepository;
            _officeRepository = officeRepository;

            Logger = NullLogger<CountryDisplayNameAppService>.Instance;
        }

        public async Task<CountryDisplayNameDto> GetAsync(Guid id)
        {
            await CheckPolicyAsync(SettingsPermissions.CountryDisplayName.Default);

            CountryDisplayName result = await _repository.GetAsync(id);
            Country country = await _countryRepository.GetAsync(result.CountryId);
            Office office = await _officeRepository.GetAsync(result.OfficeId);
            Airport airport = null;
            if (result.AirportId != null)
            {
                airport = await _airportRepository.FindAsync(result.AirportId.Value);
            }

            CountryDisplayNameDto dto = new() {
                Id = result.Id,
                DisplayName = result.DisplayName,
                AirportId = result.AirportId,
                AirportName = airport?.AirportName,
                CountryId = result.CountryId,
                CountryName = country?.CountryName,
                OfficeId = result.OfficeId,
                OfficeName = office?.OfficeName,
            };

            return dto;
        }

        public async Task SaveAsync(CreateUpdateCountryDisplayNameDto dto)
        {
            await CheckPolicyAsync(SettingsPermissions.CountryDisplayName.Edit);

            CountryDisplayName entity;
            if (dto.Id == null)
            {
                entity = new();
            }
            else
            {
                entity = await _repository.GetAsync(dto.Id.Value);
            }

            if (dto.AirportId.IsNullOrEmpty())
            {
                if (dto.AirportName.IsNullOrEmpty() || dto.AirportCode.IsNullOrEmpty())
                {
                    entity.AirportId = null;
                }
                else
                {
                    entity.AirportId = (await _airportRepository.InsertAsync(
                        new Airport()
                        {
                            AirportName = dto.AirportName,
                            AirportIataCode = dto.AirportCode,
                            IsDeleted = false,
                        },
                        true
                    )).Id;
                }
            }
            else
            {
                entity.AirportId = Guid.Parse(dto.AirportId);
            }

            entity.DisplayName = dto.DisplayName ?? "";
            entity.CountryId = dto.CountryId;
            entity.OfficeId = dto.OfficeId;
            entity.LastModifierId = dto.LastModifierId;
            entity.LastModificationTime = dto.LastModificationTime;
            
            if (dto.Id == null)
            {
                await _repository.InsertAsync(entity);
            }
            else
            {
                await _repository.UpdateAsync(entity);
            }
        }

        public async Task<PagedResultDto<CountryDisplayNameListDto>> GetListAsync(CountryDisplayNameQueryDto queryDto)
        {
            await CheckPolicyAsync(SettingsPermissions.CountryDisplayName.Default);

            PagedResultDto<CountryDisplayNameListDto> result = new();

            if (queryDto.OfficeId == null)
            {
                return result;
            }

            Office office = await _officeRepository.FindAsync(queryDto.OfficeId.Value);
            if (office == null)
            {
                return result;
            }

            List<CountryDisplayName> countryDisplayNameList = await _repository.GetListAsync();
            List<Country> countryList = await _countryRepository.GetListAsync();
            List<Airport> airportList = await _airportRepository.GetListAsync();

            var rows = from country in countryList
                       join display in countryDisplayNameList
                       on new { CountryId = country.Id, OfficeId = office.Id } equals new { display.CountryId, display.OfficeId }
                       into DI from di in DI.DefaultIfEmpty()
                       join airport in airportList
                       on di?.AirportId equals airport.Id
                       into Air from air in Air.DefaultIfEmpty() 
                       select new { CountryId = country.Id, country.CountryName, di?.Id, di?.DisplayName, di?.AirportId, air?.AirportName };

            if (queryDto.HasData == true)
            {
                rows = rows.Where(row => row.DisplayName != null && row.DisplayName.Length > 0).ToList();
            }

            if (queryDto.HasData == false)
            {
                rows = rows.Where(row => row.DisplayName == null || row.DisplayName.Length == 0).ToList();
            }

            if (queryDto.ShipLine != null) 
            {
                rows = rows.Where(row => row.AirportId != null && row.AirportId.Equals(queryDto.ShipLine)).ToList();
            }

            int totel = rows.ToList().Count;

            rows = rows.Skip(queryDto.SkipCount).Take(queryDto.MaxResultCount).ToList();

            List<CountryDisplayNameListDto> list = new();
            foreach (var row in rows)
            {
                CountryDisplayNameListDto dto = new()
                {
                    Id = row.Id,
                    CountryId = row.CountryId,
                    CountryName = row.CountryName,
                    DisplayName = row.DisplayName,
                    AirportName = row.AirportName
                };

                list.Add(dto);
            }

            result.Items = list;
            result.TotalCount = totel;

            return result;
        }

        //public async Task<ListResultDto<CountryDisplayNameDto>> GetCountryNameAsync()
        //{
        //    var countryNameQueryable = await _countryRepository.GetQueryableAsync();
        //    var query = from country in countryNameQueryable
        //                orderby country.CountryName
        //                select country;
        //    var countries = await AsyncExecuter.ToListAsync(query);

        //    return new ListResultDto<CountryDisplayNameListDto>(ObjectMapper.Map<List<Country>, List<CountryLookupDto>>(countries));
        //}
    }
}
