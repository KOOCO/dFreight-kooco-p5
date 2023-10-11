using Dolphin.Freight.Common;
using Dolphin.Freight.Permissions;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.SysCodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.Accounting.Payment
{
    public class CustomerPaymentAppService :
        CrudAppService<
            CustomerPayment,
            CustomerPaymentDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateCustomerPaymentDto>,
        ICustomerPaymentAppService
    {
        private readonly ICustomerPaymentRepository _customerPaymentRepository;
        private IRepository<SysCode, Guid> _syscodeRepository;

        public CustomerPaymentAppService(IRepository<CustomerPayment, Guid> repository, ICustomerPaymentRepository customerPaymentRepository, IRepository<SysCode, Guid> syscodeRepository)
        : base(repository)
        {
            _customerPaymentRepository = customerPaymentRepository;
            _syscodeRepository = syscodeRepository;
            GetPolicyName = AccountingPermissions.CustomerPayment.Default;
            GetListPolicyName = AccountingPermissions.CustomerPayment.Default;
            CreatePolicyName = AccountingPermissions.CustomerPayment.Create;
            UpdatePolicyName = AccountingPermissions.CustomerPayment.Edit;
            DeletePolicyName = AccountingPermissions.CustomerPayment.Delete;
        }

        public async Task<CustomerPaymentDto> GetDataAsync(Guid id)
        {
            var cp = await _customerPaymentRepository.FindByIdAsync(id);
            return ObjectMapper.Map<CustomerPayment, CustomerPaymentDto>(cp);
        }

        public async Task<PagedResultDto<CustomerPaymentDto>> GetDataList()
        {
            IQueryable<CustomerPayment> queryable = await _customerPaymentRepository.GetQueryableAsync();

            IQueryable<SysCode> sysCodes = await _syscodeRepository.GetQueryableAsync();

            var list = (from cp in queryable
                        join sc in sysCodes on cp.Category equals sc.CodeValue where sc.CodeType.Equals("Category")
                        select new CustomerPaymentDto
                        {
                            Id = cp.Id,
                            ReleaseDate = cp.ReleaseDate,
                            ReceivablesSources = cp.ReceivablesSources,
                            ReceivablesSourcesName = cp.ReceivablesSourcesName != null ? cp.ReceivablesSourcesName.TPName : "",
                            Category = sc.ShowName,
                            CheckNo = cp.CheckNo,
                            Bank = cp.Bank,
                            BankCurrency = cp.BankCurrency,
                            Deposit = cp.Deposit,
                            Invalid = cp.Invalid,
                            OfficeId = cp.OfficeId,
                            OfficeName = cp.Office != null ? cp.Office.SubstationName : "",
                            CreatorId = cp.CreatorId,
                            Memo = cp.Memo,
                            //Creator = cp.CreatorId
                        }).ToList();

            PagedResultDto<CustomerPaymentDto> listDto = new PagedResultDto<CustomerPaymentDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;

            return listDto;
        }

        public async Task<CustomerPaymentDto> CheckByPaymentIdAsync(Guid PaymentId)
        {
            var cp = await _customerPaymentRepository.CheckByPaymentIdAsync(PaymentId);
            return ObjectMapper.Map<CustomerPayment, CustomerPaymentDto>(cp);
        }

        public async Task DeleteMultiplePaymentsAsync(Guid[] ids)
        {
            foreach (var id in ids)
            {


              await  base.DeleteAsync(id);
            }
        }
    }
}