using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settings.Substations;
using Dolphin.Freight.Settings.SysCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Dolphin.Freight.ImportExport.AirExports.Bookings
{
    public class ExportBookingAppService :
        CrudAppService<
            AirExportBooking,
            ExportBookingDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateExportBookingDto>,
        IExportBookingAppService
    {
        private IRepository<AirExportBooking, Guid> _repository;
        private IRepository<SysCode, Guid> _sysCodeRepository;
        private IRepository<PortsManagement, Guid> _portsManagementRepository;
        private IRepository<Substation, Guid> _substationRepository;
        private IRepository<Dolphin.Freight.TradePartners.TradePartner, Guid> _tradePartnerRepository;
        public ExportBookingAppService(IRepository<AirExportBooking, Guid> repository, IRepository<SysCode, Guid> sysCodeRepository, IRepository<PortsManagement,
                                       Guid> portsManagementRepository, IRepository<Substation, Guid> substationRepository, IRepository<TradePartners.TradePartner,
                                       Guid> tradePartnerRepository)
            : base(repository)
        {
            _repository = repository;
            _sysCodeRepository = sysCodeRepository;
            _portsManagementRepository = portsManagementRepository;
            _substationRepository = substationRepository;
            _tradePartnerRepository = tradePartnerRepository;
            /*
            GetPolicyName = OceanExportPermissions.ExportBookingas.Default;
            GetListPolicyName = OceanExportPermissions.ExportBookingas.Default;
            CreatePolicyName = OceanExportPermissions.ExportBookingas.Create;
            UpdatePolicyName = OceanExportPermissions.ExportBookingas.Edit;
            DeletePolicyName = OceanExportPermissions.ExportBookingas.Delete;*/
        }
        public async Task<List<ExportBookingDto>> GetSONo()
        {
            var exportBooking = await _repository.GetListAsync();
            var list = ObjectMapper.Map<List<AirExportBooking>, List<ExportBookingDto>>(exportBooking);
            return list;
        }

        //public Task<PagedResultDto<ExportBookingDto>> QueryListAsync(QueryExportBookingDto query)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
