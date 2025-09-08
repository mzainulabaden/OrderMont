using Abp.AutoMapper;
using Abp.Domain.Entities;
using ERP.Generics;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    [AutoMap(typeof(PurchaseInvoiceInfo))]
    public class PurchaseInvoiceDto : BaseDocumentDto
    {
        public string ReferenceNumber { get; set; }
        public long PaymentModeId { get; set; }
        public long SupplierCOALevel04Id { get; set; }
        public long? AdvanceAmountBankCOALevl04Id { get; set; }
        public long? TaxCOALevel04Id { get; set; }
        public long? BrokerCOALevel04Id { get; set; }
        public long WarehouseId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }
        public decimal VIAmount { get; set; }
        public decimal VEAmount { get; set; }
        public decimal? BrokerPercentage { get; set; }
        public decimal? BrokerAmount { get; set; }
        public decimal? BuiltyExpense { get; set; }
        public decimal? LocalExpense { get; set; }
        public decimal NetTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public List<string> AttachedDocuments { get; set; }
        public List<PurchaseInvoiceDetailsDto> PurchaseInvoiceDetails { get; set; }
    }

    [AutoMap(typeof(PurchaseInvoiceDetailsInfo))]
    public class PurchaseInvoiceDetailsDto : Entity<long>
    {
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal PricePerBag { get; set; }
        public decimal LastPurchaseQty { get; set; }
        public decimal Quantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal PricePerBag40Kg { get; set; }
        public decimal LastPurchaseRate { get; set; }
        public decimal CostRate { get; set; }
        public decimal Adjustment { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal GrandTotal { get; set; }
        public long PurchaseOrderDetailId { get; set; }
        public decimal RemainingQty { get; set; }
    }
}
