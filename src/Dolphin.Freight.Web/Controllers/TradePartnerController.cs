﻿using Dolphin.Freight.ImportExport.AirImports;
using Dolphin.Freight.TradePartners;
using Dolphin.Freight.Web.Pages.Sales.TradePartner;
using Dolphin.Freight.Web.ViewModels.ImportExport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Dolphin.Freight.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradePartnerController : AbpController
    {
        private readonly ITradePartnerAppService _tradePartnerAppService;
        private readonly IAirImportHawbAppService _airImportHawbAppService;
        public new ILogger<TradePartnerController> Logger { get; set; }

        private readonly List<UserInfo> _data = new List<UserInfo>
    {
        new UserInfo{ Id = 1, Name = "Poy Chang"},
        new UserInfo{ Id = 2, Name = "foo2"},
        new UserInfo{ Id = 3, Name = "bar2"}
    };

        public TradePartnerController(ITradePartnerAppService tradePartnerAppService, IAirImportHawbAppService airImportHawbAppService)
        {
            _tradePartnerAppService = tradePartnerAppService;
            _airImportHawbAppService = airImportHawbAppService;
            Logger = NullLogger<TradePartnerController>.Instance;
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search(string term)
        {
            return new JsonResult(_data.Where(p => p.Name == term));
        }

        [HttpGet]
        public IActionResult GetData()
        {
            return new JsonResult(_data);
        }

        [HttpPost]
        public IActionResult PostData()
        {
            return new JsonResult(_data);
        }

        [HttpGet]
        [Route("hawbhblpartial")]
        public PartialViewResult GetHawbHBL(Guid Id)
        {
            HawbHblViewModel model = new();
            model.SubstationLookupList = new List<SelectListItem>();
            model.AirportLookupList = new List<SelectListItem>();
            model.TradePartnerLookupList = new List<SelectListItem>();
            model.PackageUnitLookupList = new List<SelectListItem>();
            model.HawbModel = _airImportHawbAppService.GetHawbCardById(Id).Result;

            return PartialView("_HawbHBL", model);
        }
    }
}
