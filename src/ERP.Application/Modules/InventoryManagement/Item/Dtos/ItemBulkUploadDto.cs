using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.Item.Dtos
{
    public class ItemBulkUploadDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ItemCategoryName { get; set; }
        public bool IsDiscountable { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string SalesCOALevel04Name { get; set; }
        public string PurchaseCOALevel04Name { get; set; }
        public List<ItemDetailsBulkUploadDto> ItemDetails { get; set; } = new();
    }

    public class ItemDetailsBulkUploadDto
    {
        public string UnitName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MinSalePrice { get; set; }
        public decimal MaxSalePrice { get; set; }
        public decimal MinStockLevel { get; set; }
        public decimal PerBagPrice { get; set; }
        public string Barcode { get; set; }
    }

    public class ItemBulkUploadRequestDto
    {
        public List<ItemBulkUploadDto> Items { get; set; }
    }

    public class ItemBulkUploadResultDto
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; }
        public string ErrorFilePath { get; set; }

        public ItemBulkUploadResultDto()
        {
            Errors = new List<string>();
        }
    }
} 