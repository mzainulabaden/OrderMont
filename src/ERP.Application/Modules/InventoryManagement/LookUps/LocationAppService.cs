using System;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;

namespace ERP.Modules.InventoryManagement.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_IMS_Location)]
    public class LocationAppService : GenericSimpleAppService<IMS_LocationDto, LocationInfo, SimpleSearchDtoBase>
    {
    }

    [AutoMap(typeof(LocationInfo))]
    public class IMS_LocationDto : SimpleDtoBase
    {
        public string Address { get; set; }
    }
}
