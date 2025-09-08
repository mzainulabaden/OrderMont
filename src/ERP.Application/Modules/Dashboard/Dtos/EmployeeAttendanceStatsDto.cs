using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Modules.Dashboard.Dtos
{
    public class EmployeeAttendanceStatsDto
    {
        public DateTime Date { get; set; }
        public int TotalEmployees { get; set; }
        public int PresentEmployees { get; set; }
        public int AbsentEmployees { get; set; }
        public List<DashboardEmployeeDto> AttendanceDetails { get; set; }
    }
}
