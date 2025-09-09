using ERP.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Modules.InventoryManagement.Vendor
{
    public class VendorInfo : SimpleEntityBase
    {
        public decimal Incoterm { get; set; }
        public decimal PaymentTerm { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public decimal PercentageAtPo { get; set; }
    }
}
