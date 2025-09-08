using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.PurchaseReturn
{
    [AutoMap(typeof(PurchaseReturnInfo))]
    public class PurchaseReturnDto : BaseDocumentDto
    {
        public long SupplierCOALevel04Id { get; set; }
        public string ReferenceNumber { get; set; }
        public long WarehouseId { get; set; }
        public List<PurchaseReturnDetailsDto> PurchaseReturnDetails { get; set; }
    }

    [AutoMap(typeof(PurchaseReturnDetailsInfo))]
    public class PurchaseReturnDetailsDto : EntityDto<long>
    {
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal PricePerBag { get; set; }
        public decimal QuantityReturned { get; set; }
        public decimal Quantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal LastPurchaseRate { get; set; }
        public decimal GrandTotal { get; set; }
        public long PurchaseInvoiceDetailId { get; set; }
    }
}
