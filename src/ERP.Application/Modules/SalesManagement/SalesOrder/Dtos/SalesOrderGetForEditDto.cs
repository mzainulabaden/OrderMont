using Abp.AutoMapper;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesOrder
{
    [AutoMap(typeof(SalesOrderInfo))]
    public class SalesOrderGetForEditDto : SalesOrderGetAllDto
    {
        public List<SalesOrderDetailsGetForEditDto> SalesOrderDetails { get; set; }
    }

    [AutoMap(typeof(SalesOrderDetailsInfo))]
    public class SalesOrderDetailsGetForEditDto : SalesOrderDetailsDto
    {
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public long WarehouseId { get; set; }
        public string WarehouseName { get; set; }
    }
}
