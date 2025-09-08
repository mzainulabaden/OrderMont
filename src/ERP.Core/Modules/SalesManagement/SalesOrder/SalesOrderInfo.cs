using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.SalesManagement.SalesOrder
{
    [Index(
        nameof(CustomerCOALevel04Id))
        ]
    public class SalesOrderInfo : ERPDocumentBaseEntity
    {
        public string ReferenceNumber { get; set; }
        public long PaymentModeId { get; set; }
        public long CustomerCOALevel04Id { get; set; }
        public long? EmployeeId { get; set; }
        public decimal? CommissionAmount { get; set; }

        [Precision(16, 2)]
        public decimal TotalAmount { get; set; }

        public List<SalesOrderDetailsInfo> SalesOrderDetails { get; set; }
    }

    [Index(
        nameof(ItemId)
        )]
    [Table("SALES_SalesOrderDetailsInfo")]
    public class SalesOrderDetailsInfo : Entity<long>
    {
        public long ItemId { get; set; }

        [Precision(16, 2)]
        public decimal ItemMinRate { get; set; }

        [Precision(16, 2)]
        public decimal ItemMaxRate { get; set; }

        [Precision(16, 2)]
        public decimal MinStockLevel { get; set; }

        public long UnitId { get; set; }

        [Precision(16, 2)]
        public decimal Rate { get; set; }

        [Precision(16, 2)]
        public decimal PricePerKg { get; set; }

        [Precision(16, 2)]
        public decimal OrderedQty { get; set; }

        [Precision(16, 2)]
        public decimal LastSaleRate { get; set; }

        [Precision(16, 2)]
        public decimal BagQty { get; set; }

        [Precision(16, 2)]
        public decimal GrandTotal { get; set; }
        public long WarehouseId { get; set; }
        public long SalesOrderInfoId { get; set; }
    }
}
