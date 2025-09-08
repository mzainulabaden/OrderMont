using Abp.AutoMapper;
using Abp.Domain.Entities;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesOrder
{
    [AutoMap(typeof(SalesOrderInfo))]
    public class SalesOrderDto : BaseDocumentDto
    {
        public string ReferenceNumber { get; set; }
        public long PaymentModeId { get; set; }
        public long? CustomerCOALevel04Id { get; set; }
        public long? EmployeeId { get; set; }
        public decimal? CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SalesOrderDetailsDto> SalesOrderDetails { get; set; }
    }

    [AutoMap(typeof(SalesOrderDetailsInfo))]
    public class SalesOrderDetailsDto : Entity<long>
    {
        public long ItemId { get; set; }
        public decimal ItemMinRate { get; set; }
        public decimal ItemMaxRate { get; set; }
        public decimal MinStockLevel { get; set; }
        public long UnitId { get; set; }
        public decimal Rate { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal LastSaleRate { get; set; }
        public decimal BagQty { get; set; }
        public decimal GrandTotal { get; set; }
        public long WarehouseId { get; set; }
    }
}
