using Abp.AutoMapper;
using Abp.Domain.Entities;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesReturn
{
    [AutoMap(typeof(SalesReturnInfo))]
    public class SalesReturnDto : BaseDocumentDto
    {
        public string ReferenceNumber { get; set; }
        public long? CustomerCOALevel04Id { get; set; }
        public bool IsReturnAgainstSalesInvoice { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SalesReturnDetailsDto> SalesReturnDetails { get; set; }
    }

    [AutoMap(typeof(SalesReturnDetailsInfo))]
    public class SalesReturnDetailsDto : Entity<long>
    {
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        public decimal Rate { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal ReturnedQty { get; set; }
        public decimal SalesInvoiceQty { get; set; }
        public decimal LastSaleRate { get; set; }
        public decimal GrandTotal { get; set; }
        public long SalesInvoiceDetailId { get; set; }
        public long WarehouseId { get; set; }
        public string WarehouseName { get; set; }
    }
}
