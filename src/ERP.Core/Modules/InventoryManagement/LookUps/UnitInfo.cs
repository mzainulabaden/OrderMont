using ERP.Generics;
using Microsoft.EntityFrameworkCore;

namespace ERP.Modules.InventoryManagement.LookUps
{
    public class UnitInfo : SimpleEntityBase
    {
        [Precision(16, 2)]
        public decimal ConversionFactor { get; set; }

        public string Description { get; set; }
    }
}
