using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.Accounting.Payment
{
    public class PaymentAppService :
        CrudAppService<
            Payment,
            PaymentDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdatePaymentDto>,
        IPaymentAppService
    {
        private readonly IPaymentRepository _PaymentRepository;
        private IRepository<SysCode, Guid> _syscodeRepository;
        private readonly IIdentityUserRepository _identityUserRepository;


        public PaymentAppService(IRepository<Payment, Guid> repository, IPaymentRepository PaymentRepository, IRepository<SysCode, Guid> syscodeRepository, IIdentityUserRepository identityUserRepository)
        : base(repository)
        {
            _PaymentRepository = PaymentRepository;
            _syscodeRepository = syscodeRepository;
            _identityUserRepository = identityUserRepository;
            GetPolicyName = AccountingPermissions.Payment.Default;
            GetListPolicyName = AccountingPermissions.Payment.Default;
            CreatePolicyName = AccountingPermissions.Payment.Create;
            UpdatePolicyName = AccountingPermissions.Payment.Edit;
            DeletePolicyName = AccountingPermissions.Payment.Delete;
        }

        public async Task<PaymentDto> GetDataAsync(Guid id)
        {
            var cp = await _PaymentRepository.FindByIdAsync(id);
            return ObjectMapper.Map<Payment, PaymentDto>(cp);
        }

        public async Task<PagedResultDto<PaymentDto>> GetDataList(QueryPaymentDto query)
        {
            IQueryable<Payment> queryable = await _PaymentRepository.GetQueryableAsync();

            IQueryable<SysCode> sysCodes = await _syscodeRepository.GetQueryableAsync();

            var list = (from cp in queryable
                        join sc in sysCodes on cp.Category equals sc.CodeValue
                        where sc.CodeType.Equals("Category")
                        
                        select new PaymentDto
                        {
                            Id = cp.Id,
                            ReleaseDate = cp.ReleaseDate,
                            PaidTo = cp.PaidTo,
                            PaidToName = cp.PaidToName != null ? cp.PaidToName.TPName : "",
                            Category = sc.ShowName,
                            CheckNo = cp.CheckNo,
                            Bank = cp.Bank,
                            BankCurrency = cp.BankCurrency,
                            Clear = cp.Clear,
                            Invalid = cp.Invalid,
                            OfficeId = cp.OfficeId,
                            OfficeName = cp.Office != null ? cp.Office.SubstationName : "",
                            CreatorId = cp.CreatorId,
                            Memo = cp.Memo,
                            //Creator = cp.CreatorId
                        }).ToList();
            list=list.WhereIf(!string.IsNullOrWhiteSpace(query.RefNo), x => x.CheckNo == query.RefNo)
                    .WhereIf(!string.IsNullOrWhiteSpace(query.Bank), x => x.Bank == query.Bank)
                    .WhereIf(query.PostDate.HasValue, e => e.ReleaseDate == query.PostDate.Value.Date.AddDays(1))
                    .WhereIf(query.ClearDate.HasValue  , e =>  e.Clear?.Month == query.ClearDate.Value.Month && e.Clear?.Year==query.ClearDate.Value.Year)
                    .WhereIf(query.VoidDate.HasValue, e => e.Invalid == query.VoidDate.Value.Date.AddDays(1))
                    .WhereIf(query.PaidTo.HasValue,e=>e.PaidTo==query.PaidTo)
                    .WhereIf(query.OfficeId.HasValue, e => e.OfficeId == query.OfficeId)
                    .WhereIf(query.IssuedBy.HasValue, e => e.CreatorId == query.IssuedBy)
                    .WhereIf(query.Void.HasValue, e =>(query.Void==true? e.Invalid !=null: e.Invalid == null))
                    .WhereIf(query.Clear.HasValue, e => (query.Clear == true ? e.Clear != null : e.Clear == null)).ToList();
            PagedResultDto<PaymentDto> listDto = new PagedResultDto<PaymentDto>();
            listDto.Items = list.Skip(query.SkipCount).Take(query.MaxResultCount).ToList();
            listDto.TotalCount = list.Count;
            foreach (var item in listDto.Items) {
                if(item.CreatorId != null) {
                    var user = await _identityUserRepository.GetAsync((Guid)item.CreatorId);
                    item.OpName = user.Name;
                
                }
            
            }
            return listDto;
        }

        public async Task<PaymentDto> CheckByPaymentIdAsync(Guid PaymentId)
        {
            var cp = await _PaymentRepository.CheckByPaymentIdAsync(PaymentId);
            return ObjectMapper.Map<Payment, PaymentDto>(cp);
        }
    }
}