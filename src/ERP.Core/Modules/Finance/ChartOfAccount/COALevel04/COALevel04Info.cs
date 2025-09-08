using ERP.Enums;
using ERP.Generics;
using Microsoft.EntityFrameworkCore;
using System;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel04
{
    [Index(
         nameof(SerialNumber),
         nameof(COALevel03Id),
         nameof(AccountTypeId),
         nameof(CurrencyId),
         nameof(LinkWithId)
     )]
    public class COALevel04Info : SimpleEntityBase
    {
        public string SerialNumber { get; set; }
        public long COALevel03Id { get; set; }
        public DateTime? StopEntryBefore { get; set; }                   
        public NatureOfAccount NatureOfAccount { get; set; }
        public long AccountTypeId { get; set; }
        public long CurrencyId { get; set; }
        public long LinkWithId { get; set; }

        // Properties specifically for Client and Supplier
        public string CNIC { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string PhysicalAddress { get; set; }
        public string SalesTaxNumber { get; set; }
        public string NationalTaxNumber { get; set; }
        public Decimal? OpeningBalance { get; set; }
    }
}
