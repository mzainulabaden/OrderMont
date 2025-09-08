using Abp.Application.Services.Dto;

namespace ERP.Modules.InventoryManagement.Item.Dtos
{
    public class ItemUnitDto : EntityDto<long>
    {
        public long UnitId { get; set; }
        public string UnitName { get; set; }
 
    }
} 