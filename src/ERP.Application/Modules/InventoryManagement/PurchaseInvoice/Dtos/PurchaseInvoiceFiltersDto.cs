using ERP.Generics;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    public class PurchaseInvoiceFiltersDto : BaseDocumentFiltersDto
    {
        public string SupplierCOALevel04Id { get; set; }
        public string PaymentModeId { get; set; }
        public string TaxCOALevel04Id { get; set; }
        public string BrokerCOALevel04Id { get; set; }
        public string AdvanceAmountBankCOALevl04Id { get; set; }
        public string WarehouseId { get; set; }
    }
}
