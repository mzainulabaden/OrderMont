using Abp.AutoMapper;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    [AutoMap(typeof(PurchaseInvoiceInfo))]
    public class PurchaseInvoiceGetForEditDto : PurchaseInvoiceGetAllDto
    {
        public List<PurchaseInvoiceDetailsGetForEditDto> PurchaseInvoiceDetails { get; set; }
    }

    [AutoMap(typeof(PurchaseInvoiceDetailsInfo))]
    public class PurchaseInvoiceDetailsGetForEditDto : PurchaseInvoiceDetailsDto
    {
        public string ItemName { get; set; }
        public string UnitName { get; set; }
    }
}
