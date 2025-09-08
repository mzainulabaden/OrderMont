using ERP.Generics;

namespace ERP.Modules.SalesManagement.SalesInvoice
{
    public class SalesInvoiceFiltersDto : BaseDocumentFiltersDto
    {
        public string PaymentModeId { get; set; }
        public string CustomerCOALevel04Id { get; set; }
        public string AdvanceAmountBankCOALevl04Id { get; set; }
        public string TaxCOALevel04Id { get; set; }
        public string WarehouseId { get; set; } // Now handled per detail, not per invoice
    }
}
