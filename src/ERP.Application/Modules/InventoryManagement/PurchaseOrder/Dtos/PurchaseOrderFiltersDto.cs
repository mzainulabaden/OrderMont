using ERP.Generics;

namespace ERP.Modules.InventoryManagement.PurchaseOrder.Dtos
{
    public class PurchaseOrderFiltersDto : BaseDocumentFiltersDto
    {
        public string SupplierCOALevel04Id { get; set; }
        public string PaymentModeId { get; set; }
    }
}
