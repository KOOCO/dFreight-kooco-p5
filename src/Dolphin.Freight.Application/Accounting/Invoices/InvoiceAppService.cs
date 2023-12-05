﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Accounting.InvoiceBills;
using Dolphin.Freight.Accounting.Inv;
using Volo.Abp.Uow;
using Dolphin.Freight.Settings.Substations;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Forms;

namespace Dolphin.Freight.Accounting.Invoices
{
    public class InvoiceAppService :
        CrudAppService<
            Invoice,
            InvoiceDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateInvoiceDto>,
        IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private IRepository<Invoice, Guid> _repository;
        private IRepository<Substation, Guid> _substationRepository;
        private IRepository<InvoiceBill, Guid> _billRepository;
        private IRepository<IdentityUser, Guid> _userRepository;
        private readonly IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IInvoiceBillAppService _invoiceBillAppService;
        private readonly IRepository<SysCode, Guid> _sysCideRepository;
        public InvoiceAppService(IRepository<Invoice, Guid> repository, IInvoiceBillAppService invoiceBillAppService, IRepository<SysCode, Guid> sysCideRepository, IRepository<Substation, Guid> substationRepository, IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository, IRepository<InvoiceBill, Guid> billRepository, IInvoiceRepository invoiceRepository, IIdentityUserRepository identityUserRepository, IRepository<IdentityUser, Guid> userRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCideRepository = sysCideRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _invoiceRepository = invoiceRepository;
            _billRepository = billRepository;
            _substationRepository = substationRepository;
            _userRepository = userRepository;
            _identityUserRepository = identityUserRepository;
            _invoiceBillAppService = invoiceBillAppService;
        }
        public async Task<PagedResultDto<InvoiceDto>> QueryListAsync(QueryInvoiceDto query)
        {
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            var Users = await _identityUserRepository.GetListAsync();
            Dictionary<Guid, string> tDictionary = new Dictionary<Guid, string>();
            Dictionary<Guid, string> UserDictionary = new Dictionary<Guid, string>();
            if (tradePartners != null)
            {
                foreach (var tradePartner in tradePartners)
                {
                    tDictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }
            var substations = await _substationRepository.GetListAsync();
            Dictionary<Guid, string> subDictionary = new Dictionary<Guid, string>();
            if (substations != null)
            {
                foreach (var substation in substations)
                {
                    subDictionary.Add(substation.Id, substation.SubstationName);
                }
            }
            var users = await _userRepository.GetListAsync();
            Dictionary<Guid, string> userDictionary = new Dictionary<Guid, string>();
            if (users is not null)
            {
                foreach (var user in users)
                {
                    userDictionary.Add(user.Id, user.Name);
                }
            }

            if (Users != null)
            {
                foreach (var User in Users)
                {
                    UserDictionary.Add(User.Id, User.Name);
                }
            }
            var result= await _repository.GetQueryableAsync();
            var rs=result.WhereIf(!string.IsNullOrWhiteSpace(query.Search), x => x.InvoiceNo
                                           .Contains(query.Search) || x.FilingNo
                                           
                                           .Contains(query.Search) )
                                           .WhereIf(query.OfficeId.HasValue, e => e.OfficeId == query.OfficeId)
                                   .WhereIf(query.TypeId.HasValue, e => e.InvoiceType == query.TypeId)
                                   
                                   .WhereIf(!string.IsNullOrWhiteSpace(query.InvoiceNo), x => x.InvoiceNo == query.InvoiceNo)
                                   
                                  .WhereIf(query.InvoiceDate.HasValue, e => e.InvoiceDate == query.InvoiceDate.Value.Date.AddDays(1))
                                   .WhereIf(query.PostDate.HasValue, e => e.PostDate == query.PostDate.Value.Date.AddDays(1))
                                  .WhereIf(query.DueDate.HasValue, e => e.DueDate == query.DueDate.Value.Date.AddDays(1))
                                  .WhereIf(query.LastDate.HasValue, e => e.LastDate == query.LastDate.Value.Date.AddDays(1))
                                  
                                 
                                          .OrderByDescending(x => x.CreationTime).ToList();
           
            List<InvoiceDto> list = new List<InvoiceDto>();
            if (query != null && query.ParentId != null)
            {
                if(query.QueryType == 0) rs = rs.Where(x => x.MawbId.Equals(query.ParentId.Value)).ToList();
                if(query.QueryType == 1) rs = rs.Where(x => x.HblId.Equals(query.ParentId.Value)).ToList();
                if(query.QueryType == 2) rs = rs.Where(x => x.BookingId.Equals(query.ParentId.Value)).ToList();
                if(query.QueryType == 3) rs = rs.Where(x => x.MblId.Equals(query.ParentId.Value)).ToList();
                if(query.QueryType == 4) rs = rs.Where(x => x.HawbId.Equals(query.ParentId.Value)).ToList();
            }
            if (query != null && query.QueryInvoiceType == 1) {
                rs = rs.Where(x => x.InvoiceType < 3).ToList();
            }
            if (query != null && query.QueryInvoiceType == 2)
            {
                rs = rs.Where(x => x.InvoiceType > 2).ToList();
            }
            var count = rs.Count();
            rs = rs.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            if (rs != null && rs.Count > 0)
            {

                foreach (var r in rs)
                {
                    var invoiceBillQueryable = await _billRepository.GetQueryableAsync();
                    var bill = ObjectMapper.Map<Invoice, InvoiceDto>(r);
                    if (r.ShipToId is not null) bill.ShipToName = tDictionary[r.ShipToId.Value];
                    if (r.OfficeId is not null) bill.OfficeName = subDictionary[r.OfficeId.Value];
                    if (r.InvoiceCompanyId != null) bill.InvoiceCompanyName = tDictionary[r.InvoiceCompanyId.Value];
                    if (r.ShipToId != null) bill.ShipTo = tDictionary[r.ShipToId.Value];
                    if (r.ShipToId != null) bill.PartyLocalName = tDictionary[r.ShipToId.Value];
                    if (r.CreatorId != null) bill.OpName = UserDictionary[r.CreatorId.Value];
                    if (r.CreatorId != null) bill.IssuedBy = UserDictionary[r.CreatorId.Value];
                    if (r.LastModifierId != null) bill.LastModifiedBy = UserDictionary[r.LastModifierId.Value];
                    if (r.MblId != null && r.MblId != Guid.Empty) { 
                        
                    }
                    bill.TaxAmount = (decimal?)r.TotalTax;
                    bill.TotalTax = r.TotalTax;
                    bill.TotalAmount= r.TotalAmount;
                    bill.TotalBeforeTax = r.TotalBeforeTax;
                    bill.TaxAmountAc = (decimal?)r.TotalTax;
                    bill.AmountAc= (decimal?)r.TotalAmount;
                    bill.BalanceAc = (decimal?)r.TotalAmount;
                    list.Add(bill);
                    var invoiceBills = invoiceBillQueryable.Where(w => w.InvoiceId.Value == r.Id).ToList();
                    bill.InvoiceBillDtos = ObjectMapper.Map<List<InvoiceBill>, List<CreateUpdateInvoiceBillDto>>(invoiceBills);
              
                }
            }
            PagedResultDto<InvoiceDto> listDto = new PagedResultDto<InvoiceDto>();
            listDto.Items = list;
            listDto.TotalCount = count;
            return listDto;
        }
        public async Task<IList<InvoiceDto>> QueryInvoicesAsync(QueryInvoiceDto query) 
        {
            var tradePartners = await _tradePartnerRepository.GetListAsync();
            Dictionary<Guid, string> tDictionary = new Dictionary<Guid, string>();
            if (tradePartners != null)
            {
                foreach (var tradePartner in tradePartners)
                {
                    tDictionary.Add(tradePartner.Id, tradePartner.TPName);
                }
            }
            var rs = await _repository.GetListAsync(true);
            List<InvoiceDto> list = new List<InvoiceDto>();
            if (query != null && query.ParentId != null)
            {
                switch (query.QueryType) 
                { 
                    default:
                        rs = rs.Where(x => x.MawbId.Equals(query.ParentId.Value)).ToList();
                        break;
                    case 1:
                        rs = rs.Where(x => x.HblId.Equals(query.ParentId.Value)).ToList();
                        break;
                    case 2:
                        rs = rs.Where(x => x.BookingId.Equals(query.ParentId.Value)).ToList();
                        break;
                    case 3:
                        rs = rs.Where(x => x.MblId.Equals(query.ParentId.Value)).ToList();
                        break;
                    case 4:
                        rs = rs.Where(x => x.HawbId.Equals(query.ParentId.Value)).ToList();
                        break;
                    case 5:
                        rs = rs.Where(x => x.VesselScheduleId.Equals(query.ParentId.Value)).ToList();
                        break;
                }
            }


            if (rs != null && rs.Count > 0)
            {
                var invoiceBillQueryable = await _billRepository.GetQueryableAsync();

                foreach (var r in rs)
                {
                    var bill = ObjectMapper.Map<Invoice, InvoiceDto>(r);

                    if (r.InvoiceCompanyId != null)
                    {
                        bill.InvoiceCompanyName = tDictionary[r.InvoiceCompanyId.Value];
                    }
                    else if (r.ShipToId != null)
                    {
                        bill.ShipTo = tDictionary[r.ShipToId.Value];
                    }
                    var invoiceBills = invoiceBillQueryable.Where(w => w.InvoiceId.Value == r.Id).ToList();

                    bill.InvoiceBillDtos = ObjectMapper.Map<List<InvoiceBill>, List<CreateUpdateInvoiceBillDto>>(invoiceBills);
                    
                    list.Add(bill);
                }
            }
            return list;
        }
        public async Task<bool> QueryInvoicesCheckAsync(QueryInvoiceDto query)
        {
            
            var rs = await _repository.GetListAsync(true);
           
            if (query != null && query.ParentId != null)
            {
                switch (query.QueryType)
                {
                    default:
                        return rs.Where(x => x.MawbId.Equals(query.ParentId.Value)).Any();
                        break;
                    case 1:
                        return rs.Where(x => x.HblId.Equals(query.ParentId.Value)).Any();
                        break;
                    case 2:
                        return rs.Where(x => x.BookingId.Equals(query.ParentId.Value)).Any();
                        break;
                    case 3:
                        return rs.Where(x => x.MblId.Equals(query.ParentId.Value)).Any();
                        break;
                    case 4:
                        return rs.Where(x => x.HawbId.Equals(query.ParentId.Value)).Any();
                        break;
                    case 5:
                         return rs.Where(x => x.VesselScheduleId.Equals(query.ParentId.Value)).Any();
                        break;
                }
            }
            return false;

           
        }
        public async Task<List<CopyIdDto>> CopyByBookingId(QueryInvoiceDto query, int IsAR, int IsAP, int IsDC) {
            List<CopyIdDto> list = new List<CopyIdDto>();
            var invoices = await _repository.GetListAsync();
            invoices = invoices.Where(x => x.BookingId == query.ParentId).ToList();
            bool IsInsert = false;
            CopyIdDto ids;
            foreach (var invoice in invoices) 
            {
                switch (invoice.InvoiceType)
                {
                    default:
                        if (IsAR == 1) IsInsert = true;
                        break;
                    case 2:
                        if (IsAP == 1) IsInsert = true;
                        break;
                    case 3:
                        if (IsDC == 1) IsInsert = true;
                        break;
                }
                if (IsInsert) 
                {
                    invoice.BookingId = query.NewParentId;
                    ids = new CopyIdDto();
                    ids.Oid = invoice.Id;
                    var udto = ObjectMapper.Map<Invoice, CreateUpdateInvoiceDto>(invoice);
                    udto.Id = new Guid();
                    var dto = await this.CreateAsync(udto);
                    ids.Nid = dto.Id;
                    list.Add(ids);
                }
                
            }
            return list;
            

        }
        public async Task DeleteMultipleInvoicesByIdsAsync(Guid[] ids)
        {
            foreach (var id in ids)
            {
                 await _repository.DeleteAsync(id);

               

            }
        }
        public async Task DeleteGAInvoicesByIdAsync(Guid[] Ids)
        {
            foreach (var Id in Ids)
            {
                var Invoice = await _invoiceRepository.GetAsync(Id);

                Invoice.IsDeleted = true;
                var Query = await _billRepository.GetQueryableAsync();
                var InvoiceBills = Query.Where(w => w.InvoiceId == Id).ToList();

                foreach (var InvoiceBill in InvoiceBills)
                {
                    InvoiceBill.IsDeleted = true;

                    await _billRepository.UpdateAsync(InvoiceBill);
                }

                await _invoiceRepository.UpdateAsync(Invoice);
            }
        }
        public async Task<JsonResult> CopyGAInvoiceAsync(Guid Id)
        {
            var Invoice = ObjectMapper.Map<Invoice, InvoiceDto>(await _invoiceRepository.GetAsync(Id));
            var InvoiceId = Invoice.Id;
            Invoice.Id = Guid.Empty;

            var NewInvoice = await _invoiceRepository.InsertAsync(ObjectMapper.Map<InvoiceDto, Invoice>(Invoice));

            var Query = await _billRepository.GetQueryableAsync();
            var InvoiceBills = ObjectMapper.Map<List<InvoiceBill>, List<InvoiceBillDto>>(Query.Where(w => w.InvoiceId == InvoiceId).ToList());

            foreach (var InvoiceBill in InvoiceBills)
            {
                InvoiceBill.Id = Guid.Empty;
                InvoiceBill.InvoiceId = NewInvoice.Id;

                await _billRepository.InsertAsync(ObjectMapper.Map<InvoiceBillDto, InvoiceBill>(InvoiceBill));
            }

            return new JsonResult(new { invoiceId = NewInvoice.Id, invoiceType = NewInvoice.InvoiceType });
        }
        public async Task CreateMblHblAccountingForCopiedOE_OI(Guid OldMblId, Guid NewMblId, bool IsAP = false, bool IsAR = false, bool IsDC = false)
        {
            QueryInvoiceDto QueryInvoiceDto = new() { QueryType = 3, ParentId = OldMblId };

            var InvoiceDtos = await QueryInvoicesAsync(QueryInvoiceDto);

            if (InvoiceDtos != null && InvoiceDtos.Count > 0)
            {
                if (IsAP)
                {
                    var APInvoice = InvoiceDtos.Where(w => w.InvoiceType == 0).ToList();

                    foreach (var item in APInvoice)
                    {
                        var newAPInvoice = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(item);

                        newAPInvoice.MblId = NewMblId;
                        newAPInvoice.Id = Guid.Empty;

                        var CreateInvoice = await CreateAsync(newAPInvoice);

                        QueryInvoiceBillDto InvoiceBillDto = new();
                        InvoiceBillDto.InvoiceNo = item.Id.ToString();

                        var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(InvoiceBillDto);
                        foreach (var bill in invoiceBills)
                        {
                            var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                            newbill.InvoiceId = CreateInvoice.Id;
                            newbill.Id = Guid.Empty;
                            await _invoiceBillAppService.CreateAsync(newbill);
                        }
                    }
                }
                if (IsDC)
                {
                    var invoiceDc = InvoiceDtos.Where(x => x.InvoiceType == 1).ToList();
                    
                    foreach (var invoice in invoiceDc)
                    {
                        var newInvoiceDc = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                        newInvoiceDc.MblId = NewMblId;
                        newInvoiceDc.Id = Guid.Empty;
                        
                        var createInvoice = await CreateAsync(newInvoiceDc);
                        
                        QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                        query.InvoiceNo = invoice.Id.ToString();
                        
                        var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                        
                        foreach (var bill in invoiceBills)
                        {
                            var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                            newbill.InvoiceId = createInvoice.Id;
                            newbill.Id = Guid.Empty;
                            await _invoiceBillAppService.CreateAsync(newbill);
                        }
                    }
                }
                if (IsAR)
                {
                    var invoiceAr = InvoiceDtos.Where(x => x.InvoiceType == 2).ToList();
                    
                    foreach (var invoice in invoiceAr)
                    {
                        var newInvoiceAr = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(invoice);
                        newInvoiceAr.MblId = NewMblId;
                        newInvoiceAr.Id = Guid.Empty;
                    
                        var createInvoice = await CreateAsync(newInvoiceAr);
                        
                        QueryInvoiceBillDto query = new QueryInvoiceBillDto();
                        query.InvoiceNo = invoice.Id.ToString();
                        
                        var invoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(query);
                        
                        foreach (var bill in invoiceBills)
                        {
                            var newbill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(bill);
                            newbill.InvoiceId = createInvoice.Id;
                            newbill.Id = Guid.Empty;
                            await _invoiceBillAppService.CreateAsync(newbill);
                        }
                    }
                }
            }
        }
    }
}