using ERP.Generics;
using Microsoft.EntityFrameworkCore;
using System;

namespace ERP.Modules.HumanResource.GazettedHoliday
{
    [Index(
        nameof(EventStartDate),
        nameof(EventEndDate)
        )]
    public class GazettedHolidayInfo : SimpleEntityBase
    {
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public bool IsRecurring { get; set; }
        public string Description { get; set; }
    }
}
