using Dolphin.Freight.Web.ViewModels.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
namespace Dolphin.Freight.Web.ViewModels.ExcelExportConfiguration
{
    public class ExcelExportConfigurationViewModel
    {
        public Guid Id { get; set; }

        [BindProperty]
        public List<Preference> Preference { get; set; }
        public string PreferenceSrc { get; set; }
        public Guid UserId { get; set; }
    }

 
}
