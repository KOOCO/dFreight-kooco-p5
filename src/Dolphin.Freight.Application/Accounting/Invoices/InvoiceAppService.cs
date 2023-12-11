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

        /// <summary>
        /// Creates replica accounting entries by querying existing invoices, filtering them based on specified parameters,
        /// and generating new invoices with common ID assignment logic.
        /// </summary>
        /// <param name="OldId">The old identifier used for querying existing invoices.</param>
        /// <param name="NewId">The common identifier to assign to new invoices.</param>
        /// <param name="QueryType">The type of query to determine filtering criteria.</param>
        /// <param name="IsAP">Flag indicating whether to create Accounts Payable (AP) invoices.</param>
        /// <param name="IsAR">Flag indicating whether to create Accounts Receivable (AR) invoices.</param>
        /// <param name="IsDC">Flag indicating whether to create Debit/Credit (DC) invoices.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateReplicaAccounting(Guid OldId, Guid NewId, int QueryType, bool IsAP = false, bool IsAR = false, bool IsDC = false)
        {
            QueryInvoiceDto Query = new() { QueryType = QueryType, ParentId = OldId };
            
            var invoiceDtos1 = await QueryInvoicesAsync(Query);

            if (invoiceDtos1 != null && invoiceDtos1.Count > 0)
            {
                if (IsAP) await CreateInvoice<InvoiceDto>(invoiceDtos1, NewId, 2, QueryType == 0 ? IsAP : false, QueryType == 4 ? IsAP : false, QueryType == 3 ? IsAP : false, QueryType == 1 ? IsAP : false);

                if (IsDC) await CreateInvoice<InvoiceDto>(invoiceDtos1, NewId, 1, QueryType == 0 ? IsDC : false, QueryType == 4 ? IsDC : false, QueryType == 3 ? IsDC : false, QueryType == 1 ? IsDC : false);
                
                if (IsAR) await CreateInvoice<InvoiceDto>(invoiceDtos1, NewId, 0, QueryType == 0 ? IsAR : false, QueryType == 4 ? IsAR : false, QueryType == 3 ? IsAR : false, QueryType == 1 ? IsAR : false);
            }
        }

        /// <summary>
        /// Creates invoices based on a specified type (T) and common parameters. 
        /// Filters the provided list of invoice data based on the specified invoice type, applies common ID assignment logic, 
        /// creates new invoices, and associates them with bills.
        /// </summary>
        /// <typeparam name="T">The type of invoice to create.</typeparam>
        /// <param name="InvoiceDtoList">The list of invoice data.</param>
        /// <param name="Id">The common ID to assign to invoices.</param>
        /// <param name="InvoiceType">The type of invoices to filter in the list.</param>
        /// <param name="IsMawb">Flag to assign Mawb ID.</param>
        /// <param name="IsHawb">Flag to assign Hawb ID.</param>
        /// <param name="IsMbl">Flag to assign Mbl ID.</param>
        /// <param name="IsHbl">Flag to assign Hbl ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateInvoice<T>(IList<InvoiceDto> InvoiceDtoList, Guid Id, int InvoiceType, bool IsMawb = false, bool IsHawb = false, bool IsMbl = false, bool IsHbl = false)
        {
            InvoiceDtoList = InvoiceDtoList.Where(w => w.InvoiceType == InvoiceType).ToList();

            foreach (var Invoice in InvoiceDtoList)
            {
                var NewInvoice = ObjectMapper.Map<InvoiceDto, CreateUpdateInvoiceDto>(Invoice);

                SetIdProperties(NewInvoice, Id, IsMawb, IsHawb, IsMbl, IsHbl);

                NewInvoice.Id = Guid.Empty;

                var CreateInvoice = await CreateAsync(NewInvoice);

                await CreateInvoiceBillsAsync(Invoice.Id.ToString(), CreateInvoice.Id);
            }
        }

        #region Private Functions
        /// <summary>
        /// This helper method responsible for setting ID properties on the CreateUpdateInvoiceDto object based on specified conditions. 
        /// This method encapsulates the common logic for ID assignment.
        /// </summary>
        protected virtual void SetIdProperties(CreateUpdateInvoiceDto Invoice, Guid Id, bool IsMawb = false, bool IsHawb = false, bool IsMbl = false, bool IsHbl = false)
        {
            if (IsMawb) Invoice.MawbId = Id;
            if (IsHawb) Invoice.HawbId = Id;
            if (IsMbl) Invoice.MblId = Id;
            if (IsHbl) Invoice.HblId = Id;
        }
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