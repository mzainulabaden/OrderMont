using Abp.AutoMapper;
using ERP.Generics;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel01
{
    [AutoMap(typeof(COALevel01Info))]
    public class COALevel01Dto : SimpleDtoBase
    {
        public string SerialNumber { get; set; }
        public long AccountTypeId { get; set; }
    }
}
