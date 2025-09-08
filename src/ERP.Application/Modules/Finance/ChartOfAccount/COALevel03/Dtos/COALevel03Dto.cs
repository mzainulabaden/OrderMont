using Abp.AutoMapper;
using ERP.Generics;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel03
{
    [AutoMap(typeof(COALevel03Info))]
    public class COALevel03Dto : SimpleDtoBase
    {
        public string SerialNumber { get; set; }
        public long COALevel02Id { get; set; }
        public long AccountTypeId { get; set; }
    }
}
