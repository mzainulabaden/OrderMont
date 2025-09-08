using System;

namespace ERP.Modules.ReportingPreview.Dtos
{
    public class SalesInvoiceDetailsRequestDto
    {
        public long? SalesInvoiceId { get; set; }
        public string ReferenceNumber { get; set; }
        public long? PaymentModeId { get; set; }
        public long? CustomerId { get; set; }
        public long? EmployeeId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemId { get; set; }
    }

    public class SalesInvoiceDetailsResultDto
    {
        public long SalesInvoiceId { get; set; }
        public string ReferenceNumber { get; set; }
        public string VoucherNumber { get; set; }
        public long? PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FreightAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long? CreatedBy { get; set; }
        public long InvoiceDetailId { get; set; }
        public long ItemId { get; set; }
        public String ItemName { get; set; }
        public long? ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public long UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal ConversionFactor { get; set; }
        public decimal ItemMinRate { get; set; }
        public decimal ItemMaxRate { get; set; }
        public decimal Rate { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal ProfitAmount { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal LastSaleRate { get; set; }
        public decimal LineTotal { get; set; }
        public long? SalesOrderDetailId { get; set; }
        public decimal CustomerTotalDebit { get; set; }
        public decimal CustomerTotalCredit { get; set; }
        public decimal CustomerBalance { get; set; }
    }
}


