using Abp.AutoMapper;
using ERP.Enums;
using ERP.Generics;
using System;

namespace ERP.Modules.HumanResource.PayrollAdministration.Dtos
{
    [AutoMap(typeof(EmployeeSalaryInfo))]
    public class EmployeeSalaryGetAllDto : BaseDocumentGetAllDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EmployeeType EmployeeType { get; set; }
    }
}
