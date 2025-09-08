using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.Item
{
    [AutoMap(typeof(ItemInfo))]
    public class ItemDto : SimpleDtoBase
    {
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public long ItemCategoryId { get; set; }
        public bool IsDiscountable { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal ReOrderQty { get; set; }
        public long ? SalesCOALevel04Id { get; set; }
        public long ? PurchaseCOALevel04Id { get; set; }
        public List<ItemDetailsDto> ItemDetails { get; set; }
    }

    [AutoMap(typeof(ItemDetailsInfo))]
    public class ItemDetailsDto : EntityDto<long>
    {
        public long UnitId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MinSalePrice { get; set; }
        public decimal MaxSalePrice { get; set; }
        public decimal MinStockLevel { get; set; }
        public string Barcode { get; set; }
        public decimal PerBagPrice { get; set; }
    }
}
