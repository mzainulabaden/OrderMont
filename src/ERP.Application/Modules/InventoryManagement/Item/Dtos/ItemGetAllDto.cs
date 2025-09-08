using Abp.AutoMapper;
using ERP.Generics;

namespace ERP.Modules.InventoryManagement.Item
{
    [AutoMap(typeof(ItemInfo))]
    public class ItemGetAllDto : SimpleDtoBase
    {
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public long ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public bool IsDiscountable { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal ReOrderQty { get; set; }
        public long SalesCOALevel04Id { get; set; }
        public string SalesCOALevel04SerialNumber { get; set; }
        public string SalesCOALevel04Name { get; set; }
        public long PurchaseCOALevel04Id { get; set; }
        public string PurchaseCOALevel04SerialNumber { get; set; }
        public string PurchaseCOALevel04Name { get; set; }
    }
}
