using Abp.AutoMapper;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesOrder
{
    [AutoMap(typeof(SalesOrderInfo))]
    public class SalesOrderGetAllDto : BaseDocumentGetAllDto
    {
        public string ReferenceNumber { get; set; }
        public long PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public long CustomerCOALevel04Id { get; set; }
        public string CustomerCOALevel04Name { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string CreatorName { get; set; }
        public long CreatorUserId { get; set; }
        public List<SalesOrderDetailsGetAllDto> SalesOrderDetails { get; set; }
    }

    [AutoMap(typeof(SalesOrderDetailsInfo))]
    public class SalesOrderDetailsGetAllDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public long UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal Rate { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal BagQty { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal MinStockLevel { get; set; }
        public long WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public decimal InvoicedQty { get; set; }
        public decimal RemainingQty { get; set; }
        public decimal LastSaleRate { get; set; }
    }
}
