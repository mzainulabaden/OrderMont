using ERP.Generics;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel03
{
    public class COALevel03FiltersDto : BaseFiltersDto
    {
        public string COALevel02Id { get; set; }
        public string AccountTypeId { get; set; }
    }
}
