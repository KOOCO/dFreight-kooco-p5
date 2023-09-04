using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dolphin.Freight.TradePartners
{
    public class ContactPersonAppService : 
        CrudAppService<
            ContactPerson, // The contactperson entity
            ContactPersonDto, // Used to show contactPersons
            Guid, // primary key of the entity
            ContactPersonPagedAndSortedResultRequestDto, // used for paging/sorting
            CreateUpdateContactPersonDto>,
        IContactPersonAppService
    {
        public new ILogger<ContactPersonAppService> Logger { get; set; }
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly ContactPersonManager _contactPersonManager;

        public ContactPersonAppService(
            IContactPersonRepository contactPersonRepository,
            ContactPersonManager contactPersonManager
            ): base(contactPersonRepository)
        {
            _contactPersonRepository = contactPersonRepository;
            _contactPersonManager = contactPersonManager;
            Logger = NullLogger<ContactPersonAppService>.Instance;
        }


        public async Task<ContactPersonDto> CreateContactPersonAsync(CreateUpdateContactPersonDto input)
        {
            Logger.LogDebug("Enter into ContactPersonAppService CreateContactPersonAsync:" + input.ContactName + ", " + input.TradePartnerId.ToString());
            var contactPerson = await _contactPersonManager.CreateContactPersonAsync(
                    input.TradePartnerId,
                    input.IsRep,
                    input.IsEmailNotification,
                    input.ContactName,
                    input.ContactTitle,
                    input.ContactDivision,
                    input.ContactCellPhone,
                    input.ContactPhone,
                    input.ContactFax,
                    input.ContactEmailAddress,
                    input.ContactRemark,
                    input.ContactGender,
                    input.ContactMarriage,
                    input.ContactSmokes,
                    input.ContactDrink,
                    input.ContactAge,
                    input.ContactGarment,
                    input.ContactHobby,
                    input.ContactInterest,
                    input.ContactConstellation,
                    input.ContactMemorialDay,
                    input.ContactBirthday,
                    input.ContactCountryId,
                    input.ContactCityCode,
                    input.ContactStateCode,
                    input.ContactPostCode,
                    input.ContactAddress
                );
            contactPerson.IsDeleted = false;
            Logger.LogDebug("Enter into InsertAsync:" + contactPerson.ContactName + ", " + contactPerson.TradePartnerId.ToString());
            await _contactPersonRepository.InsertAsync(contactPerson);
            return ObjectMapper.Map<ContactPerson, ContactPersonDto>(contactPerson);
        }

        public async Task DeleteContactPersonAsync(Guid id)
        {
            await _contactPersonRepository.DeleteAsync(id);
        }

        public override async Task<PagedResultDto<ContactPersonDto>> GetListAsync(ContactPersonPagedAndSortedResultRequestDto input)
        {
            var filter = ObjectMapper.Map<ContactPersonPagedAndSortedResultRequestDto, ContactPersonFilter>(input);

            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(ContactPerson.ContactName);
            }

            var contactPersons = await _contactPersonRepository.GetListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                filter
            );

            var totalCount = await _contactPersonRepository.GetTotalCountAsync(filter);

            return new PagedResultDto<ContactPersonDto>(
                totalCount,
                ObjectMapper.Map<List<ContactPerson>, List<ContactPersonDto>>(contactPersons)
            );
        }

        public async Task UpdateContactPersonAsync(Guid id, CreateUpdateContactPersonDto input)
        {
            var contactPerson = await _contactPersonRepository.GetAsync(id);
            if (contactPerson.ContactName != input.ContactName)
            {
                await _contactPersonManager.ChangeContactNameAsync(contactPerson, input.ContactName);
            }
            contactPerson.IsRep = input.IsRep;
            contactPerson.IsEmailNotification = input.IsEmailNotification;
            contactPerson.ContactTitle = input.ContactTitle;
            contactPerson.ContactDivision = input.ContactDivision;
            contactPerson.ContactCellPhone = input.ContactCellPhone;
            contactPerson.ContactPhone = input.ContactPhone;
            contactPerson.ContactFax = input.ContactFax;
            contactPerson.ContactEmailAddress = input.ContactEmailAddress;
            contactPerson.ContactRemark = input.ContactRemark;
            contactPerson.ContactGender = input.ContactGender;
            contactPerson.ContactMarriage = input.ContactMarriage;
            contactPerson.ContactSmokes = input.ContactSmokes;
            contactPerson.ContactDrink = input.ContactDrink;
            contactPerson.ContactAge = input.ContactAge;
            contactPerson.ContactGarment = input.ContactGarment;
            contactPerson.ContactHobby = input.ContactHobby;
            contactPerson.ContactInterest = input.ContactInterest;
            contactPerson.ContactConstellation = input.ContactConstellation;
            contactPerson.ContactMemorialDay = input.ContactMemorialDay;
            contactPerson.ContactBirthday = input.ContactBirthday;
            contactPerson.ContactCountryId = input.ContactCountryId;
            contactPerson.ContactCityCode = input.ContactCityCode;
            contactPerson.ContactStateCode = input.ContactStateCode;
            contactPerson.ContactPostCode = input.ContactPostCode;
            contactPerson.ContactAddress = input.ContactAddress;

            await _contactPersonRepository.UpdateAsync(contactPerson);
        }

        public async Task SwitchRepAsync(SwitchRepContactPersonDto dto)
        {
            ContactPerson entity = await _contactPersonRepository.GetAsync(dto.Id);
            entity.IsRep = dto.IsRep;

            await _contactPersonRepository.UpdateAsync(entity);
        }

        public async Task<List<CreateUpdateContactPersonDto>> GetListByTradePartnerId(Guid id)
        {
            var list = await Repository.GetListAsync();
            var filtered = list.Where(w => w.TradePartnerId == id).ToList();
            var dto = ObjectMapper.Map<List<ContactPerson>, List<CreateUpdateContactPersonDto>>(filtered);

            return dto;
        }

        //public async Task<Guid> GetContactPersonIdByTradePartnerId(Guid Id)
        //{
            
        //}
    }
}
