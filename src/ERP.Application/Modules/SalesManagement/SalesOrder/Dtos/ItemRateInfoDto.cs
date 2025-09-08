namespace ERP.Modules.SalesManagement.SalesOrder.Dtos
{
    public class ItemRateInfoDto
    {
        public decimal CurrentRate { get; set; }
        public decimal MinRate { get; set; }
        public decimal MaxRate { get; set; }
        public decimal LastSaleRate { get; set; }
    }
}
