using Abp.Application.Services.Dto;

namespace ERP.Modules.HumanResource.AttendanceManagement.Dtos
{
    public class GetAllAttendanceDto : EntityDto<long>
    {
        public long EmployeeId { get; set; }
        public string EmployeeErpId { get; set; }
        public string EmployeeName { get; set; }
        public string CheckIn_Time { get; set; }
        public string CheckOut_Time { get; set; }
        public string AttendanceDate { get; set; }
    }
}