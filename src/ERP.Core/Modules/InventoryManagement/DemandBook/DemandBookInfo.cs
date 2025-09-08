using ERP.Generics;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Modules.InventoryManagement.DemandBook
{
    public class DemandBookInfo:SimpleEntityBase
    {
        public long ItemId { get; set; }
        public decimal Qty { get; set; }
        public long WarehouseId { get; set; }
    }
}
