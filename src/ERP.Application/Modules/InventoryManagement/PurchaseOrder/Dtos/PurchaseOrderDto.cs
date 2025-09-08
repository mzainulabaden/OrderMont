using Abp.AutoMapper;
using Abp.Domain.Entities;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.PurchaseOrder.Dtos
{
    [AutoMap(typeof(PurchaseOrderInfo))]
    public class PurchaseOrderDto : BaseDocumentDto
    {
        public string ReferenceNumber { get; set; }
        public long PaymentModeId { get; set; }
        public long SupplierCOALevel04Id { get; set; }
        public decimal Total { get; set; }
        public decimal BuiltyExpense { get; set; }
        public decimal LocalExpense { get; set; }
        public decimal NetTotal { get; set; }
        public List<string> AttachedDocuments { get; set; }
        public List<PurchaseOrderDetailsDto> PurchaseOrderDetails { get; set; }
    }

    [AutoMap(typeof(PurchaseOrderDetailsInfo))]
    public class PurchaseOrderDetailsDto : Entity<long>
    {
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal PricePerBag { get; set; }
        public decimal LastPurchaseRate { get; set; }   
        public decimal Quantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal PricePerBag40Kg { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
