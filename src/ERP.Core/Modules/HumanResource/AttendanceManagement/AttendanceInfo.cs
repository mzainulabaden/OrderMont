using Microsoft.EntityFrameworkCore;
using System;

namespace ERP.Modules.HumanResource.AttendanceManagement
{
    [Index(
        nameof(EmployeeId),
        nameof(AttendanceDate)
    )]
    public class AttendanceInfo : ERPProjBaseEntity
    {
        public long EmployeeId { get; set; }
        public DateTime? CheckIn_Time { get; set; }
        public DateTime? CheckOut_Time { get; set; }
        public DateTime AttendanceDate { get; set; }
    }
}
