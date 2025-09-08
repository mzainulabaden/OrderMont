using Abp.AutoMapper;
using Abp.Domain.Entities;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesInvoice
{
    [AutoMap(typeof(SalesInvoiceInfo))]
    public class SalesInvoiceDto : BaseDocumentDto
    {
        public string ReferenceNumber { get; set; }
        public long PaymentModeId { get; set; }
        public long CustomerCOALevel04Id { get; set; }
        public long? AdvanceAmountBankCOALevl04Id { get; set; }
        public long TaxCOALevel04Id { get; set; }
        public string EmployeeName { get; set; }
        public decimal CommissionAmount{ get; set; }
        public decimal GrandTotal { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? FreightAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal NetTotal { get; set; }
        public List<string> AttachedDocuments { get; set; }
        public List<SalesInvoiceDetailsDto> SalesInvoiceDetails { get; set; }
    }

    [AutoMap(typeof(SalesInvoiceDetailsInfo))]
    public class SalesInvoiceDetailsDto : Entity<long>
    {
        public long ItemId { get; set; }
        public decimal ItemMinRate { get; set; }
        public decimal ItemMaxRate { get; set; }
        public decimal MinStockLevel { get; set; }
        public long UnitId { get; set; }
        public decimal Rate { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal ProfitAmount { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal LastSaleRate { get; set; }
        public decimal GrandTotal { get; set; }
        public long WarehouseId { get; set; }
        public long SalesOrderDetailId { get; set; }
        public decimal RemainingQty { get; set; }
    }
}
