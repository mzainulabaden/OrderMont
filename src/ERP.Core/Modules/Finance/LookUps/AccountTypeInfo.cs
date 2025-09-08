using ERP.Generics;

namespace ERP.Modules.Finance.LookUps
{
    public class AccountTypeInfo : SimpleEntityBase
    {
        public int? AccountGroupId { get; set; }
        public string ShortName { get; set; }
    }
}
