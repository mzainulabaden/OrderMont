using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.InventoryManagement.PurchaseReturn
{
    [Index(
        nameof(SupplierCOALevel04Id)
        )]
    public class PurchaseReturnInfo : ERPDocumentBaseEntity
    {
        public long SupplierCOALevel04Id { get; set; }
        public string ReferenceNumber { get; set; }
        public long WarehouseId { get; set; }
        public List<long> PurchaseInvoiceIds { get; set; }
        public List<PurchaseReturnDetailsInfo> PurchaseReturnDetails { get; set; }
    }

    [Index(nameof(ItemId))]
    [Table("IMS_PurchaseReturnDetailsInfo")]
    public class PurchaseReturnDetailsInfo : Entity<long>
    {
        public long ItemId { get; set; }
        public long UnitId { get; set; }

        [Precision(16, 2)]
        public decimal PricePerKg { get; set; }

        [Precision(16, 2)]
        public decimal PricePerBag { get; set; }

        [Precision(16, 2)]
        public decimal QuantityReturned { get; set; }
        public decimal Quantity { get; set; }
        public decimal ActualQuantity { get; set; }

        [Precision(16, 2)]
        public decimal LastPurchaseRate { get; set; }

        [Precision(16, 2)]
        public decimal GrandTotal { get; set; }

        public long PurchaseInvoiceDetailId { get; set; }
        public long PurchaseReturnInfoId { get; set; }
    }
}
