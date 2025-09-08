using Abp.AutoMapper;
using ERP.Enums;
using ERP.Generics;
using System;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel04
{
    [AutoMap(typeof(COALevel04Info))]
    public class COALevel04GetAllDto : SimpleDtoBase
    {
        public string SerialNumber { get; set; }
        public long COALevel03Id { get; set; }
        public string COALevel03Name { get; set; }
        public string COALevel03SerialNumber { get; set; }
        public DateTime? StopEntryBefore { get; set; }
        public NatureOfAccount NatureOfAccount { get; set; }
        public long AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public long CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public long LinkWithId { get; set; }
        public string LinkWithName { get; set; }

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
