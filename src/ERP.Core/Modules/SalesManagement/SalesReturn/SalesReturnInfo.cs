using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.SalesManagement.SalesReturn
{
    [Index(nameof(CustomerCOALevel04Id))]
    public class SalesReturnInfo : ERPDocumentBaseEntity
    {
        public string ReferenceNumber { get; set; }
        public long CustomerCOALevel04Id { get; set; }
        public bool IsReturnAgainstSalesInvoice { get; set; }
        [Precision(19, 2)]
        public decimal TotalAmount { get; set; }
        public List<long> SalesInvoiceIds { get; set; }
        public List<SalesReturnDetailsInfo> SalesReturnDetails { get; set; }
    }

    [Index(nameof(ItemId))]
    [Table("SALES_SalesReturnDetailsInfo")]
    public class SalesReturnDetailsInfo : Entity<long>
    {
        public long ItemId { get; set; }
        public long UnitId { get; set; }

        [Precision(16, 2)]
        public decimal Rate { get; set; }

        [Precision(16, 2)]
        public decimal ReturnedQty { get; set; }

        [Precision(16, 2)]
        public decimal PricePerKg { get; set; }

        [Precision(16, 2)]
        public decimal SalesInvoiceQty { get; set; }

        [Precision(16, 2)]
        public decimal LastSaleRate { get; set; }

        [Precision(16, 2)]
        public decimal GrandTotal { get; set; }
        public long WarehouseId { get; set; }
        public long SalesInvoiceDetailId { get; set; }
        public long SalesReturnInfoId { get; set; }
    }
}
