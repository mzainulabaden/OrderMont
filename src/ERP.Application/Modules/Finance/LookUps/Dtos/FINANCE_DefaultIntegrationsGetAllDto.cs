using Abp.AutoMapper;
using Abp.Domain.Entities;

namespace ERP.Modules.Finance.LookUps
{
    [AutoMap(typeof(DefaultIntegrationsInfo))]
    public class FINANCE_DefaultIntegrationsGetAllDto : Entity<long>
    {
        public long ChartOfAccountId { get; set; }
        public string ChartOfAccountName { get; set; }
        public string Remarks { get; set; }
        public string Name { get; set; }
    }
}
