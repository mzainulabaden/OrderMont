using Abp.Domain.Entities;
using ERP.Generics;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.InventoryManagement.Item
{
    [Index(
        nameof(SalesCOALevel04Id),
        nameof(PurchaseCOALevel04Id)
        )]
    public class ItemInfo : SimpleEntityBase
    {
        public string Description { get; set; }
        public long ItemCategoryId { get; set; }
        public bool IsDiscountable { get; set; }

        [Precision(16, 2)]
        public decimal DiscountAmount { get; set; }

        [Precision(16, 2)]
        public decimal DiscountPercentage { get; set; }
        public decimal ReOrderQty { get; set; }
        public string ImageUrl { get;set; }

        public long SalesCOALevel04Id { get; set; }
        public long PurchaseCOALevel04Id { get; set; }
        public List<ItemDetailsInfo> ItemDetails { get; set; }
    }

    [Table("IMS_ItemDetailsInfo")]
    public class ItemDetailsInfo : Entity<long>
    {
        public long UnitId { get; set; }

        [Precision(16, 2)]
        public decimal UnitPrice { get; set; }

        [Precision(16, 2)]
        public decimal PerBagPrice { get; set; }

        [Precision(16, 2)]
        public decimal MinSalePrice { get; set; }

        [Precision(16, 2)]
        public decimal MaxSalePrice { get; set; }

        [Precision(16, 2)]
        public decimal MinStockLevel { get; set; }

        public string Barcode { get; set; }
        public long ItemInfoId { get; set; }
    }
}
