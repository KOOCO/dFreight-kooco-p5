
using Dolphin.Freight.Settings.Ports;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Dolphin.Freight.ImportExport.Attachments
{
    public class AttachmentAppService :
        CrudAppService<
            Attachment,
            AttachmentDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateAttachmentDto>,
        IAttachmentAppService
    {
        private IIdentityUserRepository _userRepository;
        private IRepository<Attachment, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        private IRepository<Port, Guid> _portRepository;
        private IRepository<Substation, Guid> _substationRepository;
        private IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        public AttachmentAppService(IRepository<Attachment, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<Port, Guid> portRepository, IRepository<Substation, Guid> substationRepository, IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> tradePartnerRepository, IIdentityUserRepository userRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _portRepository = portRepository;
            _substationRepository = substationRepository;
            _tradePartnerRepository = tradePartnerRepository;
            _userRepository = userRepository;
            /*
            GetPolicyName = OceanExportPermissions.Attachmentas.Default;
            GetListPolicyName = OceanExportPermissions.Attachmentas.Default;
            CreatePolicyName = OceanExportPermissions.Attachmentas.Create;
            UpdatePolicyName = OceanExportPermissions.Attachmentas.Edit;
            DeletePolicyName = OceanExportPermissions.Attachmentas.Delete;*/
        }
        public async Task<List<AttachmentDto>> QueryListAsync(QueryAttachmentDto query)
        {
            var Attachments = await _repository.GetListAsync();
            var attachments = Attachments.Where(x => x.Fid == query.QueryId && x.Ftype == query.QueryType);
            var list = ObjectMapper.Map<List<Attachment>, List<AttachmentDto>>(attachments.ToList());
            foreach (var item in list)
            { if (item.CreatorId != null)
                {
                    var user = await _userRepository.GetAsync((Guid)item.CreatorId);
                    item.CreatorName=user.Name;
                        }
                else {

                    item.CreatorName = null;
                }
            
            
            }
            return list;
        }
    }
}