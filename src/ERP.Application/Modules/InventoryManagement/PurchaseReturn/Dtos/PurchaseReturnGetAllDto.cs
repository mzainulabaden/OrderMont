using Abp.AutoMapper;
using ERP.Generics;
using System;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.PurchaseReturn
{
    [AutoMap(typeof(PurchaseReturnInfo))]
    public class PurchaseReturnGetAllDto : BaseDocumentGetAllDto
    {
        public long SupplierCOALevel04Id { get; set; }
        public string SupplierCOALevel04Name { get; set; }
        public string ReferenceNumber { get; set; }
        public long WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public List<string> PurchaseInvoiceIds { get; set; }

    }
}
