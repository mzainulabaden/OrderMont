using ERP.Enums;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel04
{
    public class COALevel04FiltersDto : BaseFiltersDto
    {
        public string COALevel03Id { get; set; }
        public string AccountTypeId { get; set; }
        public string CurrencyId { get; set; }
        public string LinkWithId { get; set; }
        public List<NatureOfAccount> NatureOfAccounts { get; set; }
    }
}
