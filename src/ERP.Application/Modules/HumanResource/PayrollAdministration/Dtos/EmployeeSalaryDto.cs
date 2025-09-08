using Abp.AutoMapper;
using Abp.Domain.Entities;
using ERP.Enums;
using ERP.Generics;
using System;
using System.Collections.Generic;

namespace ERP.Modules.HumanResource.PayrollAdministration.Dtos
{
    [AutoMap(typeof(EmployeeSalaryInfo))]
    public class EmployeeSalaryDto : BaseDocumentDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public List<EmployeeSalaryDetailsDto> EmployeeSalaryDetails { get; set; }
    }

    [AutoMap(typeof(EmployeeSalaryDetailsInfo))]
    public class EmployeeSalaryDetailsDto : Entity<long>
    {
        public long EmployeeId { get; set; }
        public int AttendanceDays { get; set; }
        public int RestDays { get; set; }
        public int LeaveDays { get; set; }
        public int GazettedHolidays { get; set; }
        public int PayableDays { get; set; }
        public decimal DailyWageRate { get; set; }
        public decimal NetPayable { get; set; }
    }
}
