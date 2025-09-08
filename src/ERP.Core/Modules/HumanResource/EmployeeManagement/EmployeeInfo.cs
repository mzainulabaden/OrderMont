using ERP.Enums;
using ERP.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ERP.Modules.HumanResource.EmployeeManagement
{
    [Index(
        nameof(Name),
        nameof(ErpId),
        nameof(DesignationId)
    )]
    public class EmployeeInfo : SimpleEntityBase
    {
        public string ErpId { get; set; }
        public string CNIC { get; set; }
        public string FatherName { get; set; }
        public string PhoneNumber { get; set; }
        public EmployeeType? EmployeeType { get; set; }
        public long DesignationId { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<int> RestDays { get; set; }
        public bool IsActive { get; set; }

        [Precision(16, 2)]
        public decimal DailyWageRate { get; set; }

        [Precision(16, 2)]
        public decimal MonthlySalary { get; set; }
        public long CommissionPolicyId { get; set; }
        public long UserId { get; set; }
    }
}
