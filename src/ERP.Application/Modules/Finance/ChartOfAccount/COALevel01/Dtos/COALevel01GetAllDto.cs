using Abp.AutoMapper;
using ERP.Generics;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel01
{
    [AutoMap(typeof(COALevel01Info))]
    public class COALevel01GetAllDto : SimpleDtoBase
    {
        public string SerialNumber { get; set; }
        public long AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
    }
}
