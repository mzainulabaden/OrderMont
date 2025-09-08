using ERP.Generics;

namespace ERP.Modules.InventoryManagement.PurchaseReturn
{
    public class PurchaseReturnFiltersDto : BaseDocumentFiltersDto
    {
        public string SupplierCOALevel04Id { get; set; }
        public string WarehouseId { get; set; }
    }
}
