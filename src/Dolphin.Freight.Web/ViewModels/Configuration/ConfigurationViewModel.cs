using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Dolphin.Freight.Web.ViewModels.Configuration
{
    public class ConfigurationViewModel
    {
        public Guid Id { get; set; }

        [BindProperty]
        public List<Preference> Preference { get; set; }
        public string PreferenceSrc { get; set; }
        public Guid UserId { get; set; }
    }

    public class ConfigurationViewModel2
    {
        public Guid Id { get; set; }
        public List<Preference> Preference { get; set; }
        public string PreferenceSrc { get; set; }
        public Guid UserId { get; set; }
    }

    public class Preference
    {
        public bool Checkable { get; set; }
        public bool Configurable { get; set; }
        public bool Freeze { get; set; }
        public bool Lock { get; set; }
        public string Name { get; set; }
        public bool Show { get; set; }
        public string Text { get; set; }
        public string Width { get; set; }
    }
}
