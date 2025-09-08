using ERP.Generics;

namespace ERP.Modules.InventoryManagement.Item
{
    public class ItemFiltersDto : BaseDocumentFiltersDto
    {
        public string ItemCategoryId { get; set; }
        public string SalesCOALevel04Id { get; set; }
        public string PurchaseCOALevel04Id { get; set; }
    }
}
