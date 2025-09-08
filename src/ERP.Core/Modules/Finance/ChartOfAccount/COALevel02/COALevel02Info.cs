using ERP.Generics;
using Microsoft.EntityFrameworkCore;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel02
{
    [Index(
        nameof(SerialNumber),
        nameof(COALevel01Id),
        nameof(AccountTypeId)
    )]
    public class COALevel02Info : SimpleEntityBase
    {
        public string SerialNumber { get; set; }
        public long COALevel01Id { get; set; }
        public long AccountTypeId { get; set; }
    }
}
