using ERP.Generics;
using Microsoft.EntityFrameworkCore;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel01
{
    [Index(
        nameof(SerialNumber),
        nameof(AccountTypeId)
    )]
    public class COALevel01Info : SimpleEntityBase
    {
        public string SerialNumber { get; set; }
        public long AccountTypeId { get; set; }
    }
}
