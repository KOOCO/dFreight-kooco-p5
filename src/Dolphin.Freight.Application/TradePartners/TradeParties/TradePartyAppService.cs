using Dolphin.Freight.Settings.Countries;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.TradePartners.TradeParties
{
    public class TradePartyAppService : CrudAppService<TradeParty, TradePartyDto, Guid,  PagedAndSortedResultRequestDto, CreateUpdateTradePartyDto>, ITradePartyAppService
    {
        private IRepository<TradeParty, Guid> _repository;
        private IRepository<TradePartner, Guid> _tradePartnerRepository;
        private IRepository<Country, Guid> _countryRepository;
        private IRepository<ContactPerson, Guid> _contactPersonRepository;

        private new ILogger<TradePartyAppService> Logger { get; set; }

        public TradePartyAppService(
            IRepository<TradeParty, Guid> repository, 
            IRepository<TradePartner, Guid> tradePartnerRepository, 
            IRepository<Country, Guid> countryRepository, 
            IRepository<ContactPerson, Guid> contactPersonRepository
        ): base(repository)
        {
            _repository = repository;
            _tradePartnerRepository = tradePartnerRepository;
            _countryRepository = countryRepository;
            _contactPersonRepository = contactPersonRepository;

            Logger = NullLogger<TradePartyAppService>.Instance;
        }

        public async Task<TradePartyDto> GetAsync(Guid id)
        {
            TradeParty entity = await _repository.GetAsync(id);

            return ObjectMapper.Map<TradeParty, TradePartyDto>(entity);
        }

        public async Task<List<TradePartyListDto>> GetListByPartnerIdAndTypeAsync(Guid id, TradePartyType type)
        {
            IQueryable<TradeParty> query = await _repository.GetQueryableAsync();
            IQueryable<TradePartner> partnerQuery = await _tradePartnerRepository.GetQueryableAsync();
            IQueryable<Country> countryQuery = await _countryRepository.GetQueryableAsync();
            IQueryable<ContactPerson> personQuery = await _contactPersonRepository.GetQueryableAsync();

            var rows = from party in query
                       where party.TradePartnerId == id && party.TradePartyType == type
                       join partner in partnerQuery on party.TargetTradePartnerId equals partner.Id
                       join country in countryQuery on partner.CountryCode.ToLower() equals country.Id.ToString().ToLower()
                       join person in personQuery on partner.Id equals person.TradePartnerId
                       into Person1
                       from p1 in Person1.DefaultIfEmpty()
                       select new { Party = party, Partner = partner, Country = country, Person = p1 };

            return rows.ToList().Select(row => {
                return new TradePartyListDto() {
                    Id = row.Party.Id,
                    TradePartnerId = row.Party.TradePartnerId,
                    TradePartyDescription = row.Party.TradePartyDescription,
                    TargetTradePartnerId = row.Party.TargetTradePartnerId,
                    IsDefault = row.Party.IsDefault,
                    CompanyName = row.Partner.TPName,
                    Address = String.Format("{0},{1},{2},{3},{4}", row.Partner.TPLocalAddress, row.Partner.CityCode, row.Partner.StateCode, row.Partner.PostCode, row.Country.CountryName),
                    ContactPersonId = row.Person?.Id,
                    IsRep = row.Person?.IsRep,
                    Contact = row.Person?.ContactName,
                    Tel = row.Person?.ContactCellPhone,
                    Fax = row.Person?.ContactFax,
                    Email = row.Person?.ContactEmailAddress,
                };
            }).ToList();
        }

        public async Task SaveAsync(CreateUpdateTradePartyDto dto)
        {
            try
            {
                TradeParty entity = dto.Id==Guid.Empty ? new() : await _repository.GetAsync(dto.Id);

                if (dto.Id == dto.TargetTradePartnerId)
                {
                    return;
                }

                //entity.TradePartnerId = dto.TradePartnerId;
                //entity.TargetTradePartnerId = dto.TargetTradePartnerId;
                //entity.TradePartyDescription = dto.TradePartyDescription;
                //entity.TradePartyType = dto.TradePartyType;
                //entity.IsDefault = dto.IsDefault.GetValueOrDefault();
                //entity.ExtraProperties = dto.ExtraProperties;

                entity =  ObjectMapper.Map<CreateUpdateTradePartyDto, TradeParty>(dto);


                //if (entity.IsDefault)
                //{
                //    await _repository.UpdateManyAsync(
                //        (await _repository.GetListAsync())
                //        .Where(row => row.TradePartyType == entity.TradePartyType)
                //        .ToList()
                //        .Select(row => {
                //            row.IsDefault = false;
                //            return row;
                //        })
                //        .ToList()
                //    );
                //}

                if (dto.Id == Guid.Empty)
                {
                    await _repository.InsertAsync(entity,true);
                }
                else
                {
                    await _repository.UpdateAsync(entity,true);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task SwitchDefaultAsync(SwitchDefaultTradePartyDto dto)
        {
            TradeParty entity = await _repository.GetAsync(dto.Id);

            await _repository.UpdateManyAsync(
                (await _repository.GetListAsync())
                .Where(row => row.TradePartyType == entity.TradePartyType)
                .ToList()
                .Select(row => {                
                    if (row.Id != entity.Id)
                    {
                        row.IsDefault = false;
                        return row;
                    }

                    entity.IsDefault = dto.IsDefault;
                    return entity;
                })
                .ToList()
            );
        }

        public async Task<List<CreateUpdateTradePartyDto>> GetListByTradePartnerId(Guid id)
        {
            var list = await _repository.GetListAsync();
            var filtered = list.Where(w => w.TradePartnerId == id).ToList();

            var retVal = ObjectMapper.Map<List<TradeParty>, List<CreateUpdateTradePartyDto>>(filtered);

            return retVal;
        }
    }
}
