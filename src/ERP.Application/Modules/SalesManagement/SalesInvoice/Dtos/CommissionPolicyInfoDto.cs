using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesInvoice.Dtos
{
    public class CommissionPolicyInfoDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public long EmployeeId { get; set; }
        public string PolicyType { get; set; }
        public decimal? CommisionAmount { get; set; }
        public decimal? CommisionPercentage { get; set; }
        public List<CommissionPolicyDetailDto> CommissionPolicyDetails { get; set; }
    }

    public class CommissionPolicyDetailDto
    {
        public long Id { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal SalesCommisionAmount { get; set; }
    }
} 