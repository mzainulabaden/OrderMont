using Abp.AutoMapper;
using ERP.Generics;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel03
{
    [AutoMap(typeof(COALevel03Info))]
    public class COALevel03GetAllDto : SimpleDtoBase
    {
        public string SerialNumber { get; set; }
        public long COALevel02Id { get; set; }
        public string COALevel02Name { get; set; }
        public string COALevel02SerialNumber { get; set; }
        public long AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
    }
}
