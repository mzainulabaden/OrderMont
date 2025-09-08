using Abp.Domain.Entities;
using ERP.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.HumanResource.PayrollAdministration
{
    [Index(
        nameof(StartDate),
        nameof(EndDate)
        )]
    public class EmployeeSalaryInfo : ERPDocumentBaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public List<EmployeeSalaryDetailsInfo> EmployeeSalaryDetails { get; set; }
    }

    [Index(nameof(EmployeeId))]
    [Table("HRM_EmployeeSalaryDetailsInfo")]
    public class EmployeeSalaryDetailsInfo : Entity<long>
    {
        public long EmployeeId { get; set; }
        public int AttendanceDays { get; set; }
        public int RestDays { get; set; }
        public int LeaveDays { get; set; }
        public int GazettedHolidays { get; set; }
        public int PayableDays { get; set; }

        [Precision(16, 2)]
        public decimal DailyWageRate { get; set; }

        [Precision(16, 2)]
        public decimal NetPayable { get; set; }

        public long EmployeeSalaryInfoId { get; set; }
    }
}
