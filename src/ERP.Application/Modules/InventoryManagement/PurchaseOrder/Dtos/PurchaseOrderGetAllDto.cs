using Abp.AutoMapper;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.PurchaseOrder.Dtos
{
    [AutoMap(typeof(PurchaseOrderInfo))]
    public class PurchaseOrderGetAllDto : BaseDocumentGetAllDto
    {
        public string ReferenceNumber { get; set; }
        public long SupplierCOALevel04Id { get; set; }
        public string SupplierCOALevel04SerialNumber { get; set; }
        public string SupplierCOALevel04Name { get; set; }
        public long PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public decimal LastPurchaseRate { get; set; }   
        public decimal Total { get; set; }
        public decimal BuiltyExpense { get; set; }
        public decimal LocalExpense { get; set; }
        public decimal NetTotal { get; set; }
        public List<string> AttachedDocuments { get; set; }
        public List<PurchaseOrderDetailsGetAllDto> PurchaseOrderDetails { get; set; }
    }

    public class PurchaseOrderDetailsGetAllDto : PurchaseOrderDetailsDto
    {
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public decimal InvoicedQty { get; set; }
        public decimal RemainingQty { get; set; }
    }
}
