using System;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;

namespace ERP.Modules.InventoryManagement.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_IMS_ItemCategory)]
    public class CategoryAppService : GenericSimpleAppService<IMS_ItemCategoryDto, ItemCategoryInfo, SimpleSearchDtoBase>
    {
    }

    [AutoMap(typeof(ItemCategoryInfo))]
    public class IMS_ItemCategoryDto : SimpleDtoBase
    {
    }
}
