using Abp.AutoMapper;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesReturn
{
    [AutoMap(typeof(SalesReturnInfo))]
    public class SalesReturnGetForEditDto : SalesReturnGetAllDto
    {
        public List<SalesReturnDetailsGetForEditDto> SalesReturnDetails { get; set; }
    }

    [AutoMap(typeof(SalesReturnDetailsInfo))]
    public class SalesReturnDetailsGetForEditDto : SalesReturnDetailsDto
    {
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public long WarehouseId { get; set; }
        public string WarehouseName { get; set; }
    }
}
