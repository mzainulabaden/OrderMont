using Abp.AutoMapper;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesInvoice
{
    [AutoMap(typeof(SalesInvoiceInfo))]
    public class SalesInvoiceGetAllDto : BaseDocumentGetAllDto
    {
        public string ReferenceNumber { get; set; }
        public long PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public long CustomerCOALevel04Id { get; set; }
        public string CustomerCOALevel04Name { get; set; }
        public long? AdvanceAmountBankCOALevl04Id { get; set; }
        public string AdvanceAmountBankCOALevl04Name { get; set; }
        public long TaxCOALevel04Id { get; set; }
        public string TaxCOALevel04Name { get; set; }
        public string EmployeeName { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetTotal { get; set; }
        public List<string> AttachedDocuments { get; set; }
        public List<long> SalesOrderIds { get; set; }
    }

    public class SalesInvoicePendingPaymentDto
    {
        public long Id { get; set; }
        public System.DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public long CustomerCOALevel04Id { get; set; }
        public string CustomerName { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string Status { get; set; }
    }
}
