using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ERP.Modules.InventoryManagement.Item.Dtos
{
    [AutoMap(typeof(ItemDetailsInfo))]
    public class BulkUpdateItemDetailsDto : EntityDto<long>
    {
        public long UnitId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MinSalePrice { get; set; }
        public decimal MaxSalePrice { get; set; }
        public decimal MinStockLevel { get; set; }
        public decimal PerBagPrice { get; set; }
        public string Barcode { get; set; }
    }
}
