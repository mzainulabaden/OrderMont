using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.InventoryManagement.PurchaseOrder
{
    [Index(
        nameof(SupplierCOALevel04Id)
        )]
    public class PurchaseOrderInfo : ERPDocumentBaseEntity
    {
        public string ReferenceNumber { get; set; } 
        public long PaymentModeId { get; set; }
        public long SupplierCOALevel04Id { get; set; }

        [Precision(16, 2)]
        public decimal Total { get; set; }

        [Precision(16, 2)]
        public decimal BuiltyExpense { get; set; }

        [Precision(16, 2)]
        public decimal LocalExpense { get; set; }
        
        [Precision(16, 2)]
        public decimal NetTotal { get; set; }
        public List<string> AttachedDocuments { get; set; }
        public List<PurchaseOrderDetailsInfo> PurchaseOrderDetails { get; set; }
    }

    [Index(
        nameof(ItemId)
        )]
    [Table("IMS_PurchaseOrderDetailsInfo")]
    public class PurchaseOrderDetailsInfo : Entity<long>
    {
        public long ItemId { get; set; }
        public long UnitId { get; set; }

        [Precision(16, 2)]
        public decimal PricePerKg { get; set; }

        [Precision(16, 2)]
        public decimal PricePerBag { get; set; }

        [Precision(16, 2)]
        public decimal LastPurchaseRate { get; set; }

        [Precision(16, 2)]
        public decimal Quantity { get; set; }

        [Precision(16, 2)]
        public decimal ActualQuantity { get; set; }

        [Precision(16, 2)]
        public decimal PricePerBag40Kg { get; set; }

        [Precision(16, 2)]
        public decimal GrandTotal { get; set; }

        public long PurchaseOrderInfoId { get; set; }
    }
}
