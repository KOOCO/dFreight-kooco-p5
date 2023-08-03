using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Common;
using Dolphin.Freight.Settings.Countries;
using Dolphin.Freight.Settings.Currencies;
using Dolphin.Freight.TradePartner;
using Dolphin.Freight.TradePartners.Credits;
using Microsoft.Extensions.Logging;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.TradePartners
{
    public class TradePartnerAppService : 
        CrudAppService<
            TradePartner,
            TradePartnerDto, // show
            Guid, // primary key
            QueryDto, // for paging & sorting
            CreateUpdateTradePartnerDto>,
        ITradePartnerAppService
    {
        private readonly ITradePartnerRepository _tradePartnerRepository;
        private readonly TradePartnerManager _tradePartnerManager;
        private readonly IRepository<Country, Guid> _countryRepository;
        private readonly IRepository<Currency, Guid> _currencyRepository;

        public TradePartnerAppService(
            IRepository<TradePartner, Guid> repository,
            ITradePartnerRepository tradePartnerRepository,

            TradePartnerManager tradePartnerManager,
            IRepository<Country, Guid> countryRepository,
            IRepository<Currency, Guid> currencyRepository)
            : base(repository)
        {
            _tradePartnerRepository = tradePartnerRepository;
            _tradePartnerManager = tradePartnerManager;
            _countryRepository = countryRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<TradePartnerDto> GetTPAsync(Guid id) { 
            var tp = await _tradePartnerRepository.GetAsync(id);
            return ObjectMapper.Map<TradePartner, TradePartnerDto>(tp);
        }

        public async Task<TradePartnerDto> CreateTPAsync(CreateUpdateTradePartnerDto input) {
            var tp = await _tradePartnerManager.CreateAsync(
                input.AccountGroupId,
                input.CreditLimitGroupId,
                input.TPType,
                input.TPName,
                input.TPNamePrint,

                input.BussinessStatusType,
                input.PaymentType,
                input.CreditTermType,
                input.ImporterCodeType,
                input.CreditTermDays,
                input.CreditLimit,

                input.IsActive,
                input.TrackPayment,
                input.Clm,
                input.BillToAgentCode,

                input.TPNameLocal,
                input.TPLocalAddress,
                input.CityCode,
                input.StateCode,
                input.PostCode,
                input.CountryCode,
                input.Telephone,
                input.Fax,
                input.Website,
                input.TPPrintAddress,
                input.Remark,
                input.TPAliasName,
                input.Ceo,
                input.CorpNo,
                input.IataCode,
                input.IataPrefix,
                input.ScacCode,
                input.FirmsCode,
                input.CbsaCode,
                input.DutyPaymentType,
                input.OpenHours,
                input.CsCode,
                input.SalesOfficeCode,
                input.SalesCode,
                input.SalesCodeOe,
                input.SalesCodeOi,
                input.SalesCodeAe,
                input.SalesCodeAi,
                input.SalesCodeCuOe,
                input.SalesCodeCuAe,
                input.SalesCodeCuOi,
                input.SalesCodeCuAi,
                input.AccountAddress,
                input.TaxId,
                input.AccountName,
                input.BankName,
                input.AccountNo,
                input.AccountCurrencyCode,
                input.BankName2,
                input.AccountNo2,
                input.AccountCurrencyCode2,
                input.BankName3,
                input.AccountNo3,
                input.AccountCurrencyCode3,
                input.IsfSubmissionName,
                input.ImporterNo,
                input.PopUpTips
            );

            await _tradePartnerRepository.InsertAsync(tp);
            return ObjectMapper.Map<TradePartner, TradePartnerDto>(tp);
        }

        public async Task UpdateTPAsync(Guid id, CreateUpdateTradePartnerDto input)
        {
            var tradePartner = await _tradePartnerRepository.GetAsync(id);
           // Logger.LogDebug("Update AccountGroupName:" + input.AccountGroupName);

            if (tradePartner.TPName != input.TPName)
            {
                await _tradePartnerManager.ChangeTPNameAsync(tradePartner, input.TPName);
            }

            Logger.LogDebug("Code:" + input.TPCode);
            Logger.LogDebug("TPType:" + input.TPType);
            Logger.LogDebug("TPName:" + input.TPName);
            Logger.LogDebug("Country:" + input.CountryCode);

            tradePartner.TPType = input.TPType;
            tradePartner.TPNamePrint = input.TPNamePrint;
            tradePartner.TPNameLocal = input.TPNameLocal;
            tradePartner.TPLocalAddress = input.TPLocalAddress;
            tradePartner.CityCode = input.CityCode;
            tradePartner.StateCode = input.StateCode;
            tradePartner.PostCode = input.PostCode;
            tradePartner.CountryCode = input.CountryCode;
            tradePartner.Telephone = input.Telephone;
            tradePartner.Fax = input.Fax;
            tradePartner.Website = input.Website;
            tradePartner.TPPrintAddress = input.TPPrintAddress;
            tradePartner.Remark = input.Remark;
            tradePartner.IsActive = input.IsActive;
            tradePartner.TPAliasName = input.TPAliasName;

            tradePartner.Ceo = input.Ceo;

            tradePartner.CorpNo = input.CorpNo;
            tradePartner.IataCode = input.IataCode;

            tradePartner.IataPrefix = input.IataPrefix;
            tradePartner.ScacCode = input.ScacCode;
            tradePartner.FirmsCode = input.FirmsCode;
            tradePartner.CbsaCode = input.CbsaCode;

            tradePartner.DutyPaymentType = input.DutyPaymentType;
            tradePartner.OpenHours = input.OpenHours;

            tradePartner.BussinessStatusType = input.BussinessStatusType;
            tradePartner.CsCode = input.CsCode;
            tradePartner.SalesOfficeCode = input.SalesOfficeCode;

            tradePartner.SalesCode = input.SalesCode;

            tradePartner.AccountAddress = input.AccountAddress;
            tradePartner.TaxId = input.TaxId;
            tradePartner.TrackPayment = input.TrackPayment;
            tradePartner.PaymentType = input.PaymentType;

            tradePartner.CreditTermType = input.CreditTermType;
            tradePartner.CreditTermDays = input.CreditTermDays;
            tradePartner.CreditLimit = input.CreditLimit;
            tradePartner.BillToAgentCode = input.BillToAgentCode;
            tradePartner.Clm = input.Clm;
            tradePartner.AccountName = input.AccountName;
            tradePartner.BankName = input.BankName;
            tradePartner.AccountNo = input.AccountNo;
            tradePartner.AccountCurrencyCode = input.AccountCurrencyCode;
            tradePartner.BankName2 = input.BankName2;
            tradePartner.AccountNo2 = input.AccountNo2;
            tradePartner.AccountCurrencyCode2 = input.AccountCurrencyCode2;
            tradePartner.IsfSubmissionName = input.IsfSubmissionName;
            tradePartner.ImporterCodeType = input.ImporterCodeType;
            tradePartner.ImporterNo = input.ImporterNo;

            tradePartner.PopUpTips = input.PopUpTips;

            tradePartner.AccountGroupId = input.AccountGroupId;
            tradePartner.CreditLimitGroupId = input.CreditLimitGroupId;
            
            Logger.LogDebug("Update3");
           // await CurrentUnitOfWork.SaveChangesAsync();
           await _tradePartnerRepository.UpdateAsync(tradePartner);
        }

        public async Task<TradePartnerDto> FindByTpNameAsync(String tpName)
        {
            var existingTP = await _tradePartnerRepository.FindByTpNameAsync(tpName);
            return ObjectMapper.Map<TradePartner, TradePartnerDto>(existingTP);
        }

        public async Task<ListResultDto<TradePartnerLookupDto>> GetTradePartnersLookupAsync()
        {
            var tradePartnersQueryable = await _tradePartnerRepository.GetQueryableAsync();
            var query = from tradePartner in tradePartnersQueryable
                        orderby tradePartner.TPName
                        select tradePartner;
            var tradePartners = await AsyncExecuter.ToListAsync(query);
            return new ListResultDto<TradePartnerLookupDto>(ObjectMapper.Map<List<TradePartner>, List<TradePartnerLookupDto>>(tradePartners));
        }

        // for auto-complete-select 
        public async Task<ListResultDto<TradePartnerLookupDto>> SearchTradePartnersLookupAsync(string filter)
        {
            if (filter.IsNullOrEmpty()) { }
            var tradePartnersQueryable = await _tradePartnerRepository.GetQueryableAsync();
            var query = from tradePartner in tradePartnersQueryable
                        where tradePartner.TPName.Contains(filter)
                        orderby tradePartner.TPName
                        select tradePartner;
            var tradePartners = await AsyncExecuter.ToListAsync(query);
            return new ListResultDto<TradePartnerLookupDto>(ObjectMapper.Map<List<TradePartner>, List<TradePartnerLookupDto>>(tradePartners));
        }

        public async Task<ListResultDto<CountryLookupDto>> GetCountriesLookupAsync()
        {
            var countryNames = await _countryRepository.GetListAsync();
            return new ListResultDto<CountryLookupDto>(
                ObjectMapper.Map<List<Country>, List<CountryLookupDto>>(countryNames)
            );
        }

        public async Task<ListResultDto<CurrencyLookupDto>> GetCurrenciesLookupAsync()
        {
            var currencyNames = await _currencyRepository.GetListAsync();
            return new ListResultDto<CurrencyLookupDto>(
                ObjectMapper.Map<List<Currency>, List<CurrencyLookupDto>>(currencyNames)
            );
        }

        public override async Task<PagedResultDto<TradePartnerDto>> GetListAsync(QueryDto input) 
        {
            var queryable = await Repository.GetQueryableAsync();

            queryable = queryable.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), x => x.TPCode
                                .Contains(input.Filter) || x.TPName
                                .Contains(input.Filter) || x.TPAliasName
                                .Contains(input.Filter) || x.TPNameLocal
                                .Contains(input.Filter) || x.ScacCode
                                .Contains(input.Filter) || x.IataCode
                                .Contains(input.Filter) || x.TPPrintAddress
                                .Contains(input.Filter));

            // join tradepartner and country
            var query = from tradePartner in queryable
                        join country in await _countryRepository.GetQueryableAsync() on tradePartner.CountryCode equals country.Id.ToString()
                        select new { tradePartner, country };

           

            // paging
            query = query
                .OrderBy(NormalizeSorting(input.Sorting))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            //Execute the query and get a list
            var queryResult = await AsyncExecuter.ToListAsync(query);

            //Convert the query result to a list of TradePartnerDto objects
            var tradePartnerDtos = queryResult.Select(x =>
            {
                var tradePartnerDto = ObjectMapper.Map<TradePartner, TradePartnerDto>(x.tradePartner);
                tradePartnerDto.CountryName = x.country.CountryName;
                return tradePartnerDto;
            }).ToList();

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<TradePartnerDto>(
                totalCount,
                tradePartnerDtos
            );
        }

        // 將Sorting輸入加上tradepartner
        private static string NormalizeSorting(string sorting)
        {
            if (sorting.IsNullOrEmpty())
            {
                return $"tradepartner.{nameof(TradePartner.TPName)}";
            }

            if (sorting.Contains("tradepartnertpName", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.Replace(
                    "tradepartnertpName",
                    "tradepartner.tpName",
                    StringComparison.OrdinalIgnoreCase
                );
            }

            return $"tradepartner.{sorting}";
        }

		public async Task<string> FindByTpIdAsync(Guid id)
        {
            var tpName = await _tradePartnerRepository.FindByTpIdAsync(id);
            return tpName.TPName;
        }

    }
}
