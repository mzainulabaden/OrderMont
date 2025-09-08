using System;

namespace ERP.Modules.ReportingPreview.Dtos
{
    public class SalesOrderDetailsRequestDto
    {
        public long? SalesOrderId { get; set; }
        public int? PaymentModeId { get; set; }
        public long? CustomerId { get; set; }
        public long? ItemId { get; set; }
        public int? UnitId { get; set; }
        public string VoucherNumber { get; set; }
    }

    public class SalesOrderDetailsResultDto
    {
        public long SalesOrderId { get; set; }
        public string VoucherNumber { get; set; }
        public long? PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string WarehouseName { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public long? CreatedBy { get; set; }

        public long OrderDetailId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public long UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal ItemRate { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal LastSaleRate { get; set; }
        public decimal ItemMinRate { get; set; }
        public decimal ItemMaxRate { get; set; }
        public decimal LineTotal { get; set; }
    }
}


