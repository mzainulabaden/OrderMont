using Abp.Authorization;
using Abp.AutoMapper;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using static ERP.Modules.InventoryManagement.LookUps.CategoryAppService;

namespace ERP.Modules.InventoryManagement.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_ItemCategory)]
    public class CategoryAppService : GenericSimpleAppService<IMS_ItemCategoryDto, ItemCategoryInfo, SimpleSearchDtoBase>
    {


        [AutoMap(typeof(ItemCategoryInfo))]
        public class IMS_ItemCategoryDto : SimpleDtoBase
        {
        }

    }
}
