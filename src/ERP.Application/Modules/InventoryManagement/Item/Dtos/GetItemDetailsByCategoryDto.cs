using Abp.AutoMapper;

namespace ERP.Modules.InventoryManagement.Item.Dtos
{
    [AutoMap(typeof(ItemDetailsInfo))]
    public class GetItemDetailsByCategoryDto : ItemDetailsGetForEditDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
