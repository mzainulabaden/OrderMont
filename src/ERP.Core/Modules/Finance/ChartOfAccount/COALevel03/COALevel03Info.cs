using ERP.Generics;
using Microsoft.EntityFrameworkCore;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel03
{
    [Index(
        nameof(SerialNumber),
        nameof(COALevel02Id),
        nameof(AccountTypeId)
    )]
    public class COALevel03Info : SimpleEntityBase
    {
        public string SerialNumber { get; set; }
        public long COALevel02Id { get; set; }
        public long AccountTypeId { get; set; }
    }
}
