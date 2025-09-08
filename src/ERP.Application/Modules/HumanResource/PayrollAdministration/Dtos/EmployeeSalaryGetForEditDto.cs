using Abp.AutoMapper;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace ERP.Modules.HumanResource.PayrollAdministration.Dtos
{
    [AutoMap(typeof(EmployeeSalaryInfo))]
    public class EmployeeSalaryGetForEditDto : EmployeeSalaryGetAllDto
    {
        public List<EmployeeSalaryDetailsGetForEditDto> EmployeeSalaryDetails { get; set; }
    }

    [AutoMap(typeof(EmployeeSalaryDetailsInfo))]
    public class EmployeeSalaryDetailsGetForEditDto : Entity<long>
    {
        public long EmployeeId { get; set; }
        public string EmployeeErpId { get; set; }
        public string EmployeeName { get; set; }
        public int AttendanceDays { get; set; }
        public int RestDays { get; set; }
        public int LeaveDays { get; set; }
        public int GazettedHolidays { get; set; }
        public int PayableDays { get; set; }
        public decimal DailyWageRate { get; set; }
        public decimal NetPayable { get; set; }
    }
}
