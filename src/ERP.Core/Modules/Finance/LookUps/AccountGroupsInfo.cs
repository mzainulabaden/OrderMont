using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.Finance.AccountGroups
{
    public class AccountGroupsInfo : SimpleEntityBase
    {
        public List<long> AccountTypeIds { get; set; }
    }
}
