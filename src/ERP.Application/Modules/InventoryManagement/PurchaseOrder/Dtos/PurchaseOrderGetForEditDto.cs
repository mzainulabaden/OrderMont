using Abp.AutoMapper;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.PurchaseOrder.Dtos
{
    [AutoMap(typeof(PurchaseOrderInfo))]
    public class PurchaseOrderGetForEditDto : PurchaseOrderGetAllDto
    {
        public List<PurchaseOrderDetailsGetForEditDto> PurchaseOrderDetails { get; set; }
    }

    [AutoMap(typeof(PurchaseOrderDetailsInfo))]
    public class PurchaseOrderDetailsGetForEditDto : PurchaseOrderDetailsDto
    {
        public string ItemName { get; set; }
        public string UnitName { get; set; }
    }
}
