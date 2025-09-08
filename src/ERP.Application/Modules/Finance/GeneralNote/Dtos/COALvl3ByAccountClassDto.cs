using Abp.AutoMapper;
using ERP.Modules.Finance.ChartOfAccount.COALevel03;

namespace ERP.Modules.Finance.GeneralNote.Dtos
{
    [AutoMap(typeof(COALevel03Info))]
    public class COALvl3ByAccountClassDto
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? SerialNumber { get; set; }
        public string? AccountTypeName { get; set; }
        public string? AccountTypeShortName { get; set; }
        public bool? IsActive { get; set; }
    }
}