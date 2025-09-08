using Abp.AutoMapper;
using ERP.Generics;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel02
{
    [AutoMap(typeof(COALevel02Info))]
    public class COALevel02GetAllDto : SimpleDtoBase
    {
        public string SerialNumber { get; set; }
        public long COALevel01Id { get; set; }
        public string COALevel01Name { get; set; }
        public string COALevel01SerialNumber { get; set; }
        public long AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
    }
}
