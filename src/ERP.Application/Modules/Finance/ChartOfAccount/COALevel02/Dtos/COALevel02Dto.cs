using Abp.AutoMapper;
using ERP.Generics;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel02
{
    [AutoMap(typeof(COALevel02Info))]
    public class COALevel02Dto : SimpleDtoBase
    {
        public string SerialNumber { get; set; }
        public long COALevel01Id { get; set; }
        public long AccountTypeId { get; set; }
    }
}
