using Abp.AutoMapper;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.Finance.AccountGroups.Dtos
{
    [AutoMap(typeof(AccountGroupsInfo))]
    public class AccountGroupsDto : SimpleDtoBase
    {
        public List<long> AccountTypeIds { get; set; }
    }
}
