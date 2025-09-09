using Abp.Domain.Entities;
using Abp.Webhooks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    [Index(
        nameof(VendorId)
        )]
    public class PurchaseInvoiceInfo : ERPDocumentBaseEntity
    {
        public string PONumber { get; set; }
        public string ExternalRefNumber { get; set; }
        public long VendorId { get; set; }
        public DateTime ExFactoryDate { get; set; }
        public DateTime PlacedOrderDate { get; set; }
        public long StatusId { get; set; }
        public string Memo { get; set; }
        public decimal DepositTotal { get; set; }
        public decimal BilledTotal { get; set; }  
        public decimal UnBilledTotal { get; set; }  
        public decimal DepositAppliedTotal { get; set; }  
        public List<PurchaseInvoiceDetailsInfo> PurchaseInvoiceDetails { get; set; }
    }

    [Index(
        nameof(ItemId)
        )]
    [Table("IMS_PurchaseInvoiceDetailsInfo")]
    public class PurchaseInvoiceDetailsInfo : Entity<long>
    {
        public long ItemId { get; set; }

        [Precision(16, 2)]
        public decimal Rate { get; set; }
        public DateTime ExFactoryDate { get; set; }
        public decimal OrderQty { get; set; }
        public decimal RecievedQty { get; set; }

        [Precision(16, 2)]
        public decimal OnShipment { get; set; }

        public string VoucherNumber { get; set; }

        public long PurchaseInvoiceInfoId { get; set; }
    }
}
