using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Modules.SalesManagement.SalesOrder.Dtos
{
    public class ItemPricingInfoDto
    {
        public decimal Rate { get; set; }
        public decimal? LastSaleRate { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinStockLevel { get; set; }
        public decimal PerBagPrice { get; set; }
    }
}
