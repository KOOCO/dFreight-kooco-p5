using System;
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
using Dolphin.Freight.ImportExport.AirImports;
using Volo.Abp.ObjectMapping;

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
        private readonly IRepository<AirImportMawb, Guid> _airImportMawbRepository;
        public InvoiceAppService(IRepository<Invoice, Guid> repository, IInvoiceBillAppService invoiceBillAppService,
                                 IRepository<SysCode, Guid> sysCideRepository, IRepository<Substation, Guid> substationRepository, 
                                 IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository, 
                                 IRepository<InvoiceBill, Guid> billRepository, IInvoiceRepository invoiceRepository, 
                                 IIdentityUserRepository identityUserRepository, IRepository<IdentityUser, Guid> userRepository,
                                 IRepository<AirImportMawb, Guid> airImportMawbRepository) : base(repository)
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
            _airImportMawbRepository = airImportMawbRepository;
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
                if (IsAP) await CreateAP(InvoiceDtos, NewMblId, false, false, IsAP, false);

                if (IsDC) await CreateDC(InvoiceDtos, NewMblId, false, false, IsDC, false);
                
                if (IsAR) await CreateAR(InvoiceDtos, NewMblId, false, false, IsAR, false);
            }
        }
        public async Task CreateHawbAccountingForCopiedAE(Guid OldHblId, Guid NewHblId, bool IsAP = false, bool IsAR = false, bool IsDC = false)
        {
            QueryInvoiceDto q1idto = new QueryInvoiceDto() { QueryType = 4, ParentId = OldHblId };
            var invoiceDtos1 = await QueryInvoicesAsync(q1idto);

            if (invoiceDtos1 != null && invoiceDtos1.Count > 0)
            {
                if (IsAP) await CreateAP(invoiceDtos1, NewHblId, false, IsAP, false, false);

                if (IsDC) await CreateDC(invoiceDtos1, NewHblId, false, IsDC, false, false);

                if (IsAR) await CreateAR(invoiceDtos1, NewHblId, false, IsAR, false, false);
            }
        }
        public async Task CreateAP(IList<InvoiceDto> InvoiceDto, Guid Id, bool IsMawb = false, bool IsHawb = false, bool IsMbl = false, bool IsHbl = false)
        {
            var InvoiceAP = InvoiceDto.Where(x => x.InvoiceType == 2).ToList();

            foreach (var Invoice in InvoiceAP)
            {
                var NewInvoiceAP = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(Invoice);

                if (IsMawb) NewInvoiceAP.MawbId = Id;
                if (IsHawb) NewInvoiceAP.HawbId = Id;
                if (IsMbl) NewInvoiceAP.MblId = Id;
                if (IsHbl) NewInvoiceAP.HblId = Id;

                NewInvoiceAP.Id = Guid.Empty;

                var CreateInvoiceAP = await CreateAsync(NewInvoiceAP);

                await CreateInvoiceBillsAsync(Invoice.Id.ToString(), CreateInvoiceAP.Id);
            }
        }
        public async Task CreateDC(IList<InvoiceDto> InvoiceDto, Guid Id, bool IsMawb = false, bool IsHawb = false, bool IsMbl = false, bool IsHbl = false)
        {
            var InvoiceDC = InvoiceDto.Where(x => x.InvoiceType == 1).ToList();

            foreach (var Invoice in InvoiceDC)
            {
                var NewInvoiceDC = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(Invoice);

                if (IsMawb) NewInvoiceDC.MawbId = Id;
                if (IsHawb) NewInvoiceDC.HawbId = Id;
                if (IsMbl) NewInvoiceDC.MblId = Id;
                if (IsHbl) NewInvoiceDC.HblId = Id;

                NewInvoiceDC.Id = Guid.Empty;

                var CreateInvoiceDC = await CreateAsync(NewInvoiceDC);

                await CreateInvoiceBillsAsync(Invoice.Id.ToString(), CreateInvoiceDC.Id);
            }
        }
        public async Task CreateAR(IList<InvoiceDto> InvoiceDto, Guid Id, bool IsMawb = false, bool IsHawb = false, bool IsMbl = false, bool IsHbl = false)
        {
            var InvoiceAR = InvoiceDto.Where(x => x.InvoiceType == 0).ToList();

            foreach (var Invoice in InvoiceAR)
            {
                var NewInvoiceAR = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(Invoice);

                if (IsMawb) NewInvoiceAR.MawbId = Id;
                if (IsHawb) NewInvoiceAR.HawbId = Id;
                if (IsMbl) NewInvoiceAR.MblId = Id;
                if (IsHbl) NewInvoiceAR.HblId = Id;

                NewInvoiceAR.Id = Guid.Empty;

                var CreateInvoiceAR = await CreateAsync(NewInvoiceAR);

                await CreateInvoiceBillsAsync(Invoice.Id.ToString(), CreateInvoiceAR.Id);
            }
        }

        #region Private Functions
        private async Task CreateInvoiceBillsAsync(string InvoiceNo, Guid InvoiceId)
        {
            QueryInvoiceBillDto Query = new() { InvoiceNo = InvoiceNo };

            var InvoiceBills = await _invoiceBillAppService.QueryInvoiceBillsAsync(Query);

            foreach (var InvoiceBill in InvoiceBills)
            {
                var NewInvoiceBill = ObjectMapper.Map<InvoiceBillDto, CreateUpdateInvoiceBillDto>(InvoiceBill);
                NewInvoiceBill.InvoiceId = InvoiceId;
                NewInvoiceBill.Id = Guid.Empty;

                await _invoiceBillAppService.CreateAsync(NewInvoiceBill);
            }
        }
        #endregion
    }
}