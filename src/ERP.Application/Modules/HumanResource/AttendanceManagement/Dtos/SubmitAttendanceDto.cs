using Abp.AutoMapper;
using System;
using System.Collections.Generic;

namespace ERP.Modules.HumanResource.AttendanceManagement.Dtos
{
    [AutoMap(typeof(AttendanceInfo))]
    public class SubmitAttendanceDto
    {
        public List<long> EmployeeIds { get; set; }
        public DateTime? CheckIn_Time { get; set; }
        public DateTime? CheckOut_Time { get; set; }
        public DateTime AttendanceDate { get; set; }
    }
}
