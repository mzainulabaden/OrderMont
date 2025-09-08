using Abp.AutoMapper;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.PurchaseReturn
{
    [AutoMap(typeof(PurchaseReturnInfo))]
    public class PurchaseReturnGetForEditDto : PurchaseReturnGetAllDto
    {
        public decimal GrandTotal { get; set; }
        public long WarehouseId { get; set; }
        public List<PurchaseReturnDetailsGetForEditDto> PurchaseReturnDetails { get; set; }
    }

    [AutoMap(typeof(PurchaseReturnDetailsInfo))]
    public class PurchaseReturnDetailsGetForEditDto : PurchaseReturnDetailsDto
    {
        public string ItemName { get; set; }
        public string UnitName { get; set; }
    }
}
