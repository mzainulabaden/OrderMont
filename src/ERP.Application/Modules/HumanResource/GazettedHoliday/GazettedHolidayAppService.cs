using Abp.Authorization;
using Abp.AutoMapper;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using System;

namespace ERP.Modules.HumanResource.GazettedHoliday
{
    [AbpAuthorize(PermissionNames.LookUps_GazettedHoliday)]
    public class GazettedHolidayAppService : GenericSimpleAppService<GazettedHolidayDto, GazettedHolidayInfo, SimpleSearchDtoBase>
    {

    }

    [AutoMap(typeof(GazettedHolidayInfo))]
    public class GazettedHolidayDto : SimpleDtoBase
    {
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public bool IsRecurring { get; set; }
        public string Description { get; set; }
    }
}
