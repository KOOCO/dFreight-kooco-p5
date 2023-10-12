using Dolphin.Freight.AccountingSettings.GlCodes;
using Dolphin.Freight.Common;
using Dolphin.Freight.ImportExport.AirExports;
using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.Settings.Countries;
using Dolphin.Freight.Settings.PortsManagement;
using Dolphin.Freight.Settinngs.ContainerSizes;
using Dolphin.Freight.Settinngs.PackageUnits;
using Dolphin.Freight.Settinngs.Substations;
using Dolphin.Freight.Settinngs.SysCodes;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.TradePartners.Credits;
using NPOI.POIFS.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using SelectItems = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;
using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

namespace Dolphin.Freight.Web.CommonService
{
    public class DropdownService : IDropdownService
    {
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly ISubstationAppService _substationAppService;
        private readonly IAirportAppService _airportAppService;
        private readonly IPackageUnitAppService _packageUnitAppService;
        private readonly ISysCodeAppService _sysCodeAppService;
        private readonly IAjaxDropdownAppService _ajaxDropdownAppService;
        private readonly IPortsManagementAppService _portsManagementAppService;
        private readonly IContainerSizeAppService _containerAppService;
        private readonly ICountryAppService _countryAppService;
        private readonly IGlCodeAppService _glCodeAppService;
        private readonly ICreditLimitGroupAppService _creditLimitGroupAppService;
        private readonly IAccountGroupAppService _accountGroupAppService;
        private readonly IAirImportMawbAppService _airImportMawbAppService;
        private readonly IIdentityUserAppService _identityUserAppService;

        public DropdownService(ITradePartnerAppService tradePartnerAppService,
                               ISubstationAppService substationAppService,
                               IAirportAppService airportAppService,
                               IPackageUnitAppService packageUnitAppService,
                               ISysCodeAppService sysCodeAppService,
                               IAjaxDropdownAppService ajaxDropdownAppService,
                               IPortsManagementAppService portsManagementAppService,
                               IContainerSizeAppService containerAppService,
                               ICountryAppService countryAppService,
                               IGlCodeAppService glCodeAppService,
                               ICreditLimitGroupAppService creditLimitGroupAppService,
                               IAccountGroupAppService accountGroupAppService,
                               IAirImportMawbAppService airImportMawbAppService,
                               IIdentityUserAppService identityUserAppService
                               )
        {
            _tradePartnerAppService = tradePartnerAppService;
            _substationAppService = substationAppService;
            _airportAppService = airportAppService;
            _packageUnitAppService = packageUnitAppService;
            _sysCodeAppService = sysCodeAppService;
            _portsManagementAppService = portsManagementAppService;
            _ajaxDropdownAppService = ajaxDropdownAppService;
            _containerAppService = containerAppService;
            _countryAppService = countryAppService;
            _glCodeAppService = glCodeAppService;
            _creditLimitGroupAppService = creditLimitGroupAppService;
            _accountGroupAppService = accountGroupAppService;
            _airImportMawbAppService = airImportMawbAppService;
            _identityUserAppService = identityUserAppService;
        }
        public List<SelectItems> TradePartnerLookupList => FillTradePartnerAsync().Result;

        public List<SelectItems> SubstationLookupList => FillSubstationAsync().Result;

        public List<SelectItems> AirportLookupList => FillAirportAsync().Result;

        public List<SelectItems> PackageUnitLookupList => FillPackageUnitAsync().Result;

        public List<SelectItems> WtValOtherLookupList => FillWtValOther();

        public List<SelectItems> BlTypeLookupList => FillBlType().Result;

        public List<SelectItems> SalesTypeLookupList => FillMblSalesType().Result;

        public List<SelectItems> OblTypeLookupList => FillOBlType().Result;

        public List<SelectItems> TransPortLookupList => FillTransPortType().Result;

        public List<SelectItems> ShipModeLookupList => FillShipModeAsync().Result;

        public List<SelectItems> FreightTermLookupList => FillFreightTermAsync().Result;

        public List<SelectItems> ReferenceLookupList => FillReferenceNumberAsync().Result;
        public List<SelectItems> ReferenceMawbLookupList => FillReferenceMawbNumberAsync().Result;

        public List<SelectItems> CancelReasonLookupList => FillCancelReasonAsync().Result;

        public List<SelectItems> SvgTermLookupList => FillSvcTermType().Result;

        public List<SelectItems> IncotermsLookupList => FillIncotermsIdAsync().Result;

        public List<SelectItems> CargoTypeLookupList => FillCargoTypeAsync().Result;

        public List<SelectItems> PortsManagementLookupList => FillPortAsync().Result;

        public List<SelectItems> ContainerLookupList => FillContainerAsync().Result;

        public List<SelectItems> CountryLookupList => FillCountryAsync().Result;
        public List<SelectItems> AirImportMawbLookupList => FillAirImportMawbAsync().Result;
        public List<SelectItems> PreCarriageVesselLookupList => FillPreCarriageVesselTypeAsync().Result;
        public List<SelectItems> GiCodeLookupList => FillGiCodesAsync().Result;
        public List<SelectItems> CreditLimitGroupNameLookupList => FillCreditLimitGroupName().Result;
        public List<SelectItems> AccountGroupnameLookupList => FillAccountGroupName().Result;

        public List<SelectItems> OperatorLookupList => FillOperatorAsync().Result;


        #region FillTradePartnerAsync()
        private async Task<List<SelectItems>> FillTradePartnerAsync()
        {
            var tradePartnerLookup = await _tradePartnerAppService.GetTradePartnersLookupAsync();
            return tradePartnerLookup.Items
                                            .Select(x => new SelectListItem(x.TPName + " / " + x.TPCode, x.Id.ToString(), false))
                                            .ToList();
        }
        #endregion

        #region FillSubstationAsync()
        private async Task<List<SelectItems>> FillSubstationAsync()
        {
            var substationLookup = await _substationAppService.GetSubstationsLookupAsync();
            return substationLookup.Items
                                        .Select(x => new SelectListItem(x.SubstationName + "  (" + x.AbbreviationName + ")", x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion

        #region FillAirportAsync()
        private async Task<List<SelectItems>> FillAirportAsync()
        {
            var airportLookup = await _airportAppService.GetAirportLookupAsync();
            return airportLookup.Items
                                    .Select(x => new SelectListItem(x.AirportName , x.Id.ToString(), false))
                                    .ToList();
        }
        #endregion

        #region FillPackageUnitAsync()
        private async Task<List<SelectItems>> FillPackageUnitAsync()
        {
            var packageUnitLookup = await _packageUnitAppService.GetPackageUnitsLookupAsync();
            return packageUnitLookup.Items
                                        .Select(x => new SelectListItem(x.PackageName, x.Id.ToString(), false))
                                        .ToList();
        }
        #endregion

        #region FillWtVal()
        private List<SelectListItem> FillWtValOther()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "PPD", Text = "PPD"},
                new SelectListItem { Value = "COLL", Text = "COLL"}
            };
        }
        #endregion

        #region MyRegion
        public async Task<List<SelectItems>> FillBlType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "BlTypeId" });

            return blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                               .ToList();

        }

        public async Task<List<SelectItems>> FillOBlType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "OblTypeId" });

            return blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }

        public async Task<List<SelectItems>> FillTransPortType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "TransPort1Id" });

            return blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }

        public async Task<List<SelectItems>> FillMblCarrierType()
        {
            var blTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "MblCarrierId" });

            return blTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }

        public async Task<List<SelectItems>> FillMblSalesType()
        {
            var salesTypeLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "MblSalesTypeId" });

            return salesTypeLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }

        public async Task<List<SelectItems>> FillSvcTermType()
        {
            var svcTermLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new Common.QueryDto() { QueryType = "SvcTermFromId" });

            return svcTermLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }
        public async Task<List<SelectItems>> FillOperatorAsync()
        {
            var svcTermLookup = await _identityUserAppService.GetListAsync(new GetIdentityUsersInput() {MaxResultCount=1000 });

            return svcTermLookup.Items.Select(x => new SelectListItem(x.Name, x.Id.ToString(), false))
                                     .ToList();

        }
        public async Task<List<SelectItems>> FillPreCarriageVesselTypeAsync()
        {
            var svcTermLookup = await _sysCodeAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "PreCarriageVesselNameId" });

            return svcTermLookup.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                                     .ToList();

        }

        #endregion

        #region FillShipModeAsync()
        private async Task<List<SelectItems>> FillShipModeAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "ShipModeId" });

            return lookUp
                        .Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                        .ToList();
        }
        #endregion

        #region FillFreightTermAsync()
        private async Task<List<SelectItems>> FillFreightTermAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "FreightTermId" });

            return lookUp
                        .Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                        .ToList();
        }
        #endregion

        #region FillReferenceNumberAsync()
        private async Task<List<SelectItems>> FillReferenceNumberAsync()
        {
            var referenceLookup = await _ajaxDropdownAppService.GetReferenceItemsByTypeAsync(new QueryDto());

            referenceLookup = referenceLookup.Where(w => !string.IsNullOrEmpty(w.ReferenceNo)).ToList();

            return referenceLookup
                                                 .Select(x => new SelectListItem(x.ReferenceNo, x.Id.ToString(), false))
                                                 .ToList();
        }
        #endregion
        #region FillReferenceMawbNumberAsync()
        private async Task<List<SelectItems>> FillReferenceMawbNumberAsync()
        {
            var referenceLookup = await _ajaxDropdownAppService.GetReferenceMawabAsync(new QueryDto());

            referenceLookup = referenceLookup.Where(w => !string.IsNullOrEmpty(w.ReferenceNo)).ToList();

            return referenceLookup
                                                 .Select(x => new SelectListItem(x.ReferenceNo, x.Id.ToString(), false))
                                                 .ToList();
        }
        #endregion

        #region FillCancelReasonAsync()
        private async Task<List<SelectItems>> FillCancelReasonAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "CancelReason" });

            return lookUp.Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false)).ToList();
        }
        #endregion

        #region FillSvgTermAsync()
        private async Task<List<SelectItems>> FillSvgTermAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "SvcTermFromId" });

            return lookUp.Select(x => new SelectListItem(x.CodeValue, x.Id.ToString(), false)).ToList();
        }
        #endregion
        #region AirImportMawbList()
        private async Task<List<SelectItems>> FillAirImportMawbAsync()
        {
            var lookUp = await _airImportMawbAppService.GetMawbListAsync();

            return lookUp.Select(x => new SelectListItem(x.FilingNo+'/'+x.MawbNo, x.Id.ToString(), false)).ToList();
        }
        #endregion
        #region FillIncotermsIdAsync()
        private async Task<List<SelectItems>> FillIncotermsIdAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "IncotermsId" });

            return lookUp
                        .Select(x => new SelectListItem(x.CodeValue, x.Id.ToString(), false))
                        .ToList();
        }
        #endregion

        #region FillCategoryTypeAsync()
        private async Task<List<SelectItems>> FillCargoTypeAsync()
        {
            var lookUp = await _ajaxDropdownAppService.GetSysCodeDtosByTypeAsync(new QueryDto() { QueryType = "CargoTypeId" });

            return lookUp
                        .Select(x => new SelectListItem(x.ShowName, x.Id.ToString(), false))
                        .ToList();
        }
        #endregion

        #region FillPortAsync()
        private async Task<List<SelectItems>> FillPortAsync()
        {
            var lookup = await _portsManagementAppService.QueryListAsync();

            return lookup.Select(x => new SelectListItem(x.PortName, x.Id.ToString(), false))
                         .ToList();
        }
        #endregion

        #region FillContainerAsync()
        private async Task<List<SelectItems>> FillContainerAsync()
        {
            var containerLookup = await _containerAppService.QueryListAsync(new QueryDto());

            return containerLookup.Items
                                     .Select(x => new SelectListItem(x.ContainerCode, x.Id.ToString(), false))
                                     .ToList();
        }
        #endregion

        #region FillCountryAsync()

        private async Task<List<SelectItems>> FillCountryAsync()
        {
            var lookup = await _countryAppService.GetListAsync();

            return lookup.Select(x => new SelectListItem(x.CountryName, x.Id.ToString(), false)).ToList();
        }

        #endregion

        #region FillCountryAsync()
        private async Task<List<SelectItems>> FillGiCodesAsync()
        {
            var lookup = await _glCodeAppService.GetGlCodesAsync(new Common.QueryDto());

            return lookup.Select(x => new SelectListItem(x.Code, x.Id.ToString(), false)).ToList();
        }
        #endregion

        #region FillCreditLimitAsync()

        private async Task<List<SelectItems>> FillCreditLimitGroupName()
        {
            var lookup = await _creditLimitGroupAppService.GetCreditLimitGroupNameLookupAsync();

            return lookup.Items.Select(s => new SelectListItem(s.CreditLimitGroupName, s.Id.ToString(), false)).ToList();
        }

        #endregion

        #region AccountGroupNameAsync()

        private async Task<List<SelectItems>> FillAccountGroupName()
        {
            var lookup = await _accountGroupAppService.GetAccountGroupNameLookupAsync();

            return lookup.Items.Select(s => new SelectItems(s.AccountGroupName, s.Id.ToString(), false)).ToList();
        }

        #endregion
    }
}

