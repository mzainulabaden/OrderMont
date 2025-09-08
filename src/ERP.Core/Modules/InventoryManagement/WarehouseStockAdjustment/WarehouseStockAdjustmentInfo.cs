using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.WarehouseStockAdjustment
{

    public class WarehouseStockAdjustmentInfo : ERPDocumentBaseEntity
    {
        public List<WarehouseStockAdjustmentDetailsInfo> WarehouseStockAdjustmentDetails { get; set; }
    }

    [Index(
        nameof(InventoryItemId)
    )]
    [Index(
        nameof(UnitId)
    )]
    public class WarehouseStockAdjustmentDetailsInfo : Entity<long>
    {
        public long InventoryItemId { get; set; }
        public long UnitId { get; set; }

        [Precision(19, 2)]
        public decimal MinStockLevel { get; set; }

        [Precision(19, 2)]
        public decimal Credit { get; set; }

        [Precision(19, 2)]
        public decimal Debit { get; set; }

        [Precision(19, 2)]
        public decimal CostRate { get; set; }
        public string Remarks { get; set; }
        public long WarehouseId { get; set; }
        public long WarehouseStockAdjustmentInfoId { get; set; }
    }
}
