using Abp.AutoMapper;
using ERP.Enums;
using ERP.Generics;
using System;
using System.Collections.Generic;

namespace ERP.Modules.HumanResource.EmployeeManagement.Dtos
{
    [AutoMap(typeof(EmployeeInfo))]
    public class EmployeeGetAllDto : SimpleDtoBase
    {
        public string ErpId { get; set; }
        public int Age { get; set; }
        public string CNIC { get; set; }
        public string FatherName { get; set; }
        public string PhoneNumber { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public DateTime JoiningDate { get; set; }
        public string JobDuration { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<int> RestDays { get; set; }
        public bool IsActive { get; set; }
        public decimal DailyWageRate { get; set; }
        public decimal MonthlySalary { get; set; }
        public long CommissionPolicyId { get; set; }
        public string CommissionPolicy { get; set; }
    }
}
