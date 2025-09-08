using Abp.AutoMapper;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    [AutoMap(typeof(PurchaseInvoiceInfo))]
    public class PurchaseInvoiceGetAllDto : BaseDocumentGetAllDto
    {
        public long SupplierCOALevel04Id { get; set; }
        public string SupplierCOALevel04SerialNumber { get; set; }
        public string SupplierCOALevel04Name { get; set; }
        public string ReferenceNumber { get; set; }
        public long PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public long? AdvanceAmountBankCOALevl04Id { get; set; }
        public string AdvanceAmountBankCOALevl04Name { get; set; }
        public long TaxCOALevel04Id { get; set; }
        public string TaxCOALevel04SerialNumber { get; set; }
        public string TaxCOALevel04Name { get; set; }
        public long BrokerCOALevel04Id { get; set; }
        public string BrokerCOALevel04SerialNumber { get; set; }
        public string BrokerCOALevel04Name { get; set; }
        public long EmployeeId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LastPurchaseQty { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal VIAmount { get; set; }
        public decimal VEAmount { get; set; }
        public decimal BrokerPercentage { get; set; }
        public decimal BrokerAmount { get; set; }
        public decimal BuiltyExpense { get; set; }
        public decimal LocalExpense { get; set; }
        public decimal NetTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public List<string> AttachedDocuments { get; set; }
        public long WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public bool IsStockup { get; set; }
        public List<long> PurchaseOrderIds { get; set; }
    }

    public class PurchaseInvoicePendingPaymentDto
    {
        public long Id { get; set; }
        public System.DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public long COALevel04Id { get; set; }
        public string SupplierName { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string Status { get; set; }
    }
}
