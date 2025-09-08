using ERP.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace ERP.Modules.InventoryManagement.StockLedger
{
    [Index(
        nameof(ItemId)
    )]
    public class WarehouseStockLedgerInfo : ERPProjBaseEntity
    {
        public DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public long ItemId { get; set; }

        [Precision(16, 2)]
        public decimal Credit { get; set; }

        [Precision(16, 2)]
        public decimal Debit { get; set; }

        public decimal ActualQty { get; set; }

        [Precision(16, 2)]
        public decimal Rate { get; set; }

        [Precision(16, 2)]
        public decimal TotalAmount { get; set; }
        [Precision(16, 2)]
        public long? COALevel04Id { get; set; }
        public string Remarks { get; set; }
        public long WarehouseId { get; set; }
        public long DocumentId { get; set; }
        public WarehouseStockLedgerLinkedDocument WarehouseStockLedgerLinkedDocument { get; set; }
    }
}
