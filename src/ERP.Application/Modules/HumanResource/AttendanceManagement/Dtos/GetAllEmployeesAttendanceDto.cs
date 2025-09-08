using Abp.AutoMapper;
using Abp.Domain.Entities;
using ERP.Modules.HumanResource.EmployeeManagement;
using System.Collections.Generic;

namespace ERP.Modules.HumanResource.AttendanceManagement.Dtos
{
    [AutoMap(typeof(EmployeeInfo))]
    public class GetAllEmployeesAttendanceDto : Entity<long>
    {
        public string EmployeeId { get; set; }
        public string ErpId { get; set; }
        public string Name { get; set; }
        public string AbsenteesCount { get; set; }
        public IEnumerable<string> AbsenteesDates { get; set; }
    }
}
