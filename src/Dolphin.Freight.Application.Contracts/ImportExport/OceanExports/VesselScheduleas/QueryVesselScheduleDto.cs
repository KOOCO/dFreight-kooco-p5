﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Dolphin.Freight.ImportExport.OceanExports.VesselScheduleas
{
    public class QueryVesselScheduleDto : PagedAndSortedResultRequestDto
    {
        public string QueryType { get; set; }
        public string QueryName { get; set; }
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string QueryKey { get; set; }

    }
}

