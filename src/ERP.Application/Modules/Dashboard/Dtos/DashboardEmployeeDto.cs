using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Modules.Dashboard.Dtos
{
    public class DashboardEmployeeDto
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? CheckInTime { get; set; }
    }
}
