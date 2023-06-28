﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using SelectItems = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

namespace Dolphin.Freight.Web.CommonService
{
    public interface IDropdownService
    {
        List<SelectItems> TradePartnerLookupList { get; }
        List<SelectItems> SubstationLookupList { get; }
        List<SelectItems> AirportLookupList { get; }
        List<SelectItems> PackageUnitLookupList { get; }
        List<SelectItems> WtValOtherLookupList { get; }
        List<SelectItems> BlTypeLookupList { get; }
        List<SelectItems> OblTypeLookupList { get; }
        List<SelectItems> TransPortLookupList { get; }
        List<SelectItems> PortLookupLookupList { get; }   
        List<SelectItems> ShipModeLookupList { get; }
        List<SelectItems> FreightTermLookupList { get; }
        List<SelectItems> ReferenceLookupList { get; }
        List<SelectItems> CancelReasonLookupList { get; }
        List<SelectItems> SvgTermLookupList { get; }
        List<SelectItems> IncotermsLookupList { get; }
        List<SelectItems> CargoTypeLookupList { get; }
        List<SelectItems> PortsManagementLookupList { get; }
        List<SelectItems> ContainerLookupList { get; }
        List<SelectItems> PreCarriageVesselLookupList { get; }
    }
}
