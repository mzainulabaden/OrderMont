using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    [Index(
        nameof(SupplierCOALevel04Id),
        nameof(TaxCOALevel04Id),
        nameof(BrokerCOALevel04Id)
        )]
    public class PurchaseInvoiceInfo : ERPDocumentBaseEntity
    {
        public string ReferenceNumber { get; set; } 
        public long PaymentModeId { get; set; }
        public long SupplierCOALevel04Id { get; set; }
        public long? AdvanceAmountBankCOALevl04Id { get; set; }
        public long EmployeeId { get; set; }
        public long UserId { get; set; }
        public long WarehouseId { get; set; }

        [Precision(16, 2)]
        public decimal Total { get; set; }

        [Precision(16, 2)]
        public decimal VIAmount { get; set; }

        [Precision(16, 2)]
        public decimal VEAmount { get; set; }

        [Precision(16, 2)]
        public decimal DiscountPercentage { get; set; }

        [Precision(16, 2)]
        public decimal DiscountAmount { get; set; }

        [Precision(16, 2)]
        public decimal Tax { get; set; }

        public long TaxCOALevel04Id { get; set; }

        [Precision(16, 2)]
        public decimal BrokerPercentage { get; set; }

        [Precision(16, 2)]
        public decimal BrokerAmount { get; set; }

        public long BrokerCOALevel04Id { get; set; }

        [Precision(16, 2)]
        public decimal BuiltyExpense { get; set; }

        [Precision(16, 2)]
        public decimal LocalExpense { get; set; }

        [Precision(16, 2)]
        public decimal NetTotal { get; set; }

        [Precision(16, 2)]
        public decimal LastPurchaseRate { get; set; }

        [Precision(16, 2)]
        public decimal GrandTotal { get; set; }
        [Precision(16, 2)]
        public decimal AdvanceAmount { get; set; }
        public bool IsStockup { get; set; }
        public decimal CommissionAmount { get; set; }
        public List<long> PurchaseOrderIds { get; set; }
        public List<string> AttachedDocuments { get; set; }
        public List<PurchaseInvoiceDetailsInfo> PurchaseInvoiceDetails { get; set; }
    }

    [Index(
        nameof(ItemId)
        )]
    [Table("IMS_PurchaseInvoiceDetailsInfo")]
    public class PurchaseInvoiceDetailsInfo : Entity<long>
    {
        public long ItemId { get; set; }
        public long UnitId { get; set; }

        [Precision(16, 2)]
        public decimal PricePerKg { get; set; }
        [Precision(16, 2)]
        public decimal PricePerBag { get; set; }

        [Precision(16, 2)]
        public decimal LastPurchaseQty { get; set; }

        [Precision(16, 2)]
        public decimal Quantity { get; set; }

        [Precision(16, 2)]
        public decimal ActualQuantity { get; set; }

        [Precision(16, 2)]
        public decimal PricePerBag40Kg { get; set; }

        [Precision(16, 2)]
        public decimal LastPurchaseRate { get; set; }

        [Precision(16, 2)]
        public decimal CostRate { get; set; }

        [Precision(16, 2)]
        public decimal Adjustment { get; set; }

        [Precision(16, 2)]
        public decimal TotalWeight { get; set; }

        [Precision(16, 2)]
        public decimal GrandTotal { get; set; }

        public long PurchaseOrderDetailId { get; set; }
        public long PurchaseInvoiceInfoId { get; set; }
        public decimal RemainingQty { get; set; }
    }
}
