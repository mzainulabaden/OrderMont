using ERP.Enums;
using System;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.WarehouseStockLedger.Dtos
{
    public class WarehouseStockLedgerBulkUploadItemDto
    {
        public DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string ItemName { get; set; }
        public string WarehouseName { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal ActualQty { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
        public WarehouseStockLedgerLinkedDocument WarehouseStockLedgerLinkedDocument { get; set; }
    }

    public class WarehouseStockLedgerBulkUploadRequestDto
    {
        public List<WarehouseStockLedgerBulkUploadItemDto> Items { get; set; }
    }

    public class WarehouseStockLedgerBulkUploadResultDto
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string ErrorFilePath { get; set; }
    }
}


