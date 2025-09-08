using ERP.Generics;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel02
{
    public class COALevel02FiltersDto : BaseFiltersDto
    {
        public string COALevel01Id { get; set; }
        public string AccountTypeId { get; set; }
    }
}
