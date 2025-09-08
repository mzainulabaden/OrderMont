using ERP.Generics;

namespace ERP.Modules.InventoryManagement.LookUps
{
    public class WarehouseInfo : SimpleEntityBase
    {
        public string Address { get; set; }
        public string WarehouseCode { get; set; }
        public string Manager { get; set; }
        public string PhoneNumber { get; set; }
    }
}
