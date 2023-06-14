using System;
using System.Collections.Generic;
using System.Text;

namespace Dolphin.Freight.ImportExport.Configuration
{
    public class CreateUpdateGridPreferenceDto
    {
        public Guid Id { get; set; }
        public string Preference { get; set; }
        public string PreferenceSrc { get; set; }
        public Guid UserId { get; set; }
    }
}
