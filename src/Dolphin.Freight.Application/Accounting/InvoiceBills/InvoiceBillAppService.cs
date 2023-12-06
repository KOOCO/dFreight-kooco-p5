using AutoMapper.Internal.Mappers;
using Dolphin.Freight.Accounting.Invoices;
using Dolphin.Freight.Settings.SysCodes;
using Dolphin.Freight.Settinngs.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Dolphin.Freight.Accounting.InvoiceBills
{
    public class InvoiceBillAppService :
        CrudAppService<
            InvoiceBill,
            InvoiceBillDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateInvoiceBillDto>,
        IInvoiceBillAppService
    {
        private IRepository<InvoiceBill, Guid> _repository;
        public InvoiceBillAppService(IRepository<InvoiceBill, Guid> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<PagedResultDto<InvoiceBillDto>> QueryListAsync(QueryInvoiceBillDto query)
        {
            var rs = await Repository.GetListAsync();
            
            rs = rs.Where(x => x.InvoiceId.ToString().Equals(query.InvoiceNo)).ToList();
            
            var list = ObjectMapper.Map<List<InvoiceBill>, List<InvoiceBillDto>>(rs);

            PagedResultDto<InvoiceBillDto> listDto = new PagedResultDto<InvoiceBillDto>();
            listDto.Items = list;
            listDto.TotalCount = list.Count;
            
            return listDto;
        }
        public async Task<IList<InvoiceBillDto>> QueryInvoiceBillsAsync(QueryInvoiceBillDto query)
        {
            var res = await Repository.GetListAsync();
            res = res.Where(x => x.InvoiceId.ToString().Equals(query.InvoiceNo)).ToList();
            var list = ObjectMapper.Map<List<InvoiceBill>, List<InvoiceBillDto>>(res);
            
            return list;
        }
        public async void CopyByInvoiceIds(List<CopyIdDto> list) { 
            var rs = await _repository.GetListAsync();
            if (list != null && list.Count > 0) 
            {
                foreach (var ids in list) 
                { 
                    var bills = rs.Where(x=>x.InvoiceId == ids.Oid);
                    foreach (var bill in bills) 
                    { 
                        bill.InvoiceId = ids.Nid;
                        await this.CreateAsync(ObjectMapper.Map<InvoiceBill, CreateUpdateInvoiceBillDto>(bill));
                    }
                }
            }
        }
    }
}