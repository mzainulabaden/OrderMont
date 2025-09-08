using ERP.Generics;

namespace ERP.Modules.HumanResource.EmployeeManagement.Dtos
{
    public class EmployeeFiltersDto : BaseFiltersDto
    {
        public string ErpId { get; set; }
        public bool? IsActive { get; set; }
        public string DesignationId { get; set; }
    }
}
