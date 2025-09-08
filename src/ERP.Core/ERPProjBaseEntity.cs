using Abp.Domain.Entities.Auditing;

namespace ERP
{
    public class ERPProjBaseEntity : FullAuditedEntity<long>
    {
        public int? TenantId { get; set; }
    }
}
